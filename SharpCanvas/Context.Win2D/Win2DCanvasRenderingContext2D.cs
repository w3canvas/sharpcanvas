using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;

namespace SharpCanvas.Context.Win2D
{
    /// <summary>
    /// Win2D/Direct2D Canvas 2D rendering context.
    ///
    /// This is the GPU-accelerated backend for SharpCanvas on Windows 10+.
    /// It wraps Microsoft.Graphics.Canvas (Win2D) which wraps Direct2D/DirectWrite.
    ///
    /// Architecture:
    ///   Canvas 2D API → Win2DCanvasRenderingContext2D → Win2D → Direct2D → Direct3D → GPU
    ///
    /// Key differences from other backends:
    ///   - GPU-accelerated (vs CPU-only for Skia/GDI+)
    ///   - Path model: CanvasPathBuilder → CanvasGeometry (immutable, finalized on fill/stroke)
    ///   - Arc model: endpoint arcs via AddArc (same as WPF ArcSegment)
    ///     Conversion from center+angles already solved in Context.WindowsMedia/CanvasPath.cs:493-544
    ///   - Text: DirectWrite (same engine Chrome uses on Windows)
    ///   - Compositing: Full Porter-Duff via CanvasComposite (26 modes)
    ///   - No native save/restore stack (manual state management, same as GDI+)
    ///
    /// See also: mm0/lib/applied/canvas/win2d_mapping.py for the complete
    /// 34-op mapping specification with notes on each operation.
    /// </summary>
    public class Win2DCanvasRenderingContext2D
    {
        // --- Core Win2D objects ---
        private readonly CanvasDevice _device;
        private CanvasRenderTarget _renderTarget;
        private CanvasDrawingSession _session;

        // --- Current path (Canvas 2D mutable path model → Win2D immutable geometry) ---
        private CanvasPathBuilder _pathBuilder;
        private bool _figureOpen;
        private Vector2 _currentPoint;
        private Vector2 _subpathStart;

        // --- State stack (no native save/restore in Win2D) ---
        private readonly Stack<Win2DState> _stateStack = new();
        private Win2DState _currentState;

        // --- Canvas dimensions ---
        private int _width;
        private int _height;

        public Win2DCanvasRenderingContext2D(int width, int height)
        {
            _width = width;
            _height = height;
            _device = CanvasDevice.GetSharedDevice();
            _renderTarget = new CanvasRenderTarget(_device, width, height, 96);
            _session = _renderTarget.CreateDrawingSession();
            _currentState = new Win2DState(_device);
            _pathBuilder = new CanvasPathBuilder(_device);
        }

        // ================================================================
        // TRANSFORMS (direct — Matrix3x2 property on session)
        // ================================================================

        public void scale(double x, double y)
        {
            _session.Transform = Matrix3x2.CreateScale((float)x, (float)y) * _session.Transform;
        }

        public void rotate(double angle)
        {
            // Win2D uses radians (same as Canvas 2D) — no conversion needed
            _session.Transform = Matrix3x2.CreateRotation((float)angle) * _session.Transform;
        }

        public void translate(double x, double y)
        {
            _session.Transform = Matrix3x2.CreateTranslation((float)x, (float)y) * _session.Transform;
        }

        public void setTransform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            _session.Transform = new Matrix3x2(
                (float)m11, (float)m12,
                (float)m21, (float)m22,
                (float)dx, (float)dy
            );
        }

        public void resetTransform()
        {
            _session.Transform = Matrix3x2.Identity;
        }

        // ================================================================
        // RECTANGLES (direct — session.FillRectangle / DrawRectangle)
        // ================================================================

        public void fillRect(double x, double y, double w, double h)
        {
            _session.FillRectangle((float)x, (float)y, (float)w, (float)h, _currentState.FillBrush);
        }

        public void strokeRect(double x, double y, double w, double h)
        {
            _session.DrawRectangle(
                (float)x, (float)y, (float)w, (float)h,
                _currentState.StrokeBrush, (float)_currentState.LineWidth, _currentState.StrokeStyle);
        }

        public void clearRect(double x, double y, double w, double h)
        {
            // Win2D: clip to rect, then clear with transparent
            var prevTransform = _session.Transform;
            using (var clipGeometry = CanvasGeometry.CreateRectangle(_device, (float)x, (float)y, (float)w, (float)h))
            using (var layer = _session.CreateLayer(1.0f, clipGeometry))
            {
                _session.Clear(Windows.UI.Colors.Transparent);
            }
        }

        // ================================================================
        // PATH OPERATIONS (adapter — CanvasPathBuilder model)
        // ================================================================

        public void beginPath()
        {
            CloseFigureIfOpen();
            _pathBuilder?.Dispose();
            _pathBuilder = new CanvasPathBuilder(_device);
            _figureOpen = false;
        }

        public void closePath()
        {
            if (_figureOpen)
            {
                _pathBuilder.EndFigure(CanvasFigureLoop.Closed);
                _figureOpen = false;
                _currentPoint = _subpathStart;
            }
        }

        public void moveTo(double x, double y)
        {
            CloseFigureIfOpen();
            _pathBuilder.BeginFigure((float)x, (float)y);
            _figureOpen = true;
            _currentPoint = new Vector2((float)x, (float)y);
            _subpathStart = _currentPoint;
        }

        public void lineTo(double x, double y)
        {
            EnsureFigureOpen(x, y);
            _pathBuilder.AddLine((float)x, (float)y);
            _currentPoint = new Vector2((float)x, (float)y);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            EnsureFigureOpen(cp1x, cp1y);
            _pathBuilder.AddCubicBezier(
                new Vector2((float)cp1x, (float)cp1y),
                new Vector2((float)cp2x, (float)cp2y),
                new Vector2((float)x, (float)y));
            _currentPoint = new Vector2((float)x, (float)y);
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            // Win2D has NATIVE quadratic bezier (unlike GDI+ which must elevate to cubic)
            EnsureFigureOpen(cpx, cpy);
            _pathBuilder.AddQuadraticBezier(
                new Vector2((float)cpx, (float)cpy),
                new Vector2((float)x, (float)y));
            _currentPoint = new Vector2((float)x, (float)y);
        }

        // ================================================================
        // ARC (adapter — reuses Context.WindowsMedia ArcInternal pattern)
        // ================================================================
        // See CanvasPath.cs:493-544 for the original WPF implementation.
        // Win2D's CanvasPathBuilder.AddArc uses the SAME endpoint-arc model
        // as WPF's ArcSegment (D2D1_ARC_SEGMENT underneath).

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool anticlockwise = false)
        {
            if (r < 0) throw new ArgumentException("Radius must be non-negative", nameof(r));

            // Compute start and end points on the circle
            var startX = (float)(x + r * Math.Cos(startAngle));
            var startY = (float)(y + r * Math.Sin(startAngle));
            var endX = (float)(x + r * Math.Cos(endAngle));
            var endY = (float)(y + r * Math.Sin(endAngle));

            // Connect to start point of arc
            if (_figureOpen)
            {
                _pathBuilder.AddLine(new Vector2(startX, startY));
            }
            else
            {
                _pathBuilder.BeginFigure(new Vector2(startX, startY));
                _figureOpen = true;
                _subpathStart = new Vector2(startX, startY);
            }

            // Normalize angles for sweep computation
            double sweepAngle;
            if (anticlockwise)
            {
                sweepAngle = startAngle - endAngle;
                if (sweepAngle <= 0) sweepAngle += 2 * Math.PI;
            }
            else
            {
                sweepAngle = endAngle - startAngle;
                if (sweepAngle <= 0) sweepAngle += 2 * Math.PI;
            }

            // Full circle handling: split into two half-arcs
            // (D2D/WPF cannot represent a full circle as a single arc)
            if (sweepAngle >= 2 * Math.PI)
            {
                // Two half circles
                var midX = (float)(x + r * Math.Cos(startAngle + Math.PI));
                var midY = (float)(y + r * Math.Sin(startAngle + Math.PI));

                _pathBuilder.AddArc(
                    new Vector2(midX, midY),
                    (float)r, (float)r, 0,
                    anticlockwise ? CanvasSweepDirection.CounterClockwise : CanvasSweepDirection.Clockwise,
                    CanvasArcSize.Large);

                _pathBuilder.AddArc(
                    new Vector2(startX, startY),
                    (float)r, (float)r, 0,
                    anticlockwise ? CanvasSweepDirection.CounterClockwise : CanvasSweepDirection.Clockwise,
                    CanvasArcSize.Large);
            }
            else
            {
                bool isLargeArc = sweepAngle > Math.PI;

                _pathBuilder.AddArc(
                    new Vector2(endX, endY),
                    (float)r, (float)r, 0,
                    anticlockwise ? CanvasSweepDirection.CounterClockwise : CanvasSweepDirection.Clockwise,
                    isLargeArc ? CanvasArcSize.Large : CanvasArcSize.Small);
            }

            _currentPoint = new Vector2(endX, endY);
        }

        // ================================================================
        // RENDERING (fill/stroke — finalize path to CanvasGeometry)
        // ================================================================

        public void fill()
        {
            CloseFigureIfOpen();
            using var geometry = CanvasGeometry.CreatePath(_pathBuilder);
            _session.FillGeometry(geometry, _currentState.FillBrush);
            // Re-create path builder for potential reuse
            _pathBuilder = new CanvasPathBuilder(_device);
        }

        public void stroke()
        {
            CloseFigureIfOpen();
            using var geometry = CanvasGeometry.CreatePath(_pathBuilder);
            _session.DrawGeometry(geometry, _currentState.StrokeBrush,
                (float)_currentState.LineWidth, _currentState.StrokeStyle);
            _pathBuilder = new CanvasPathBuilder(_device);
        }

        // ================================================================
        // TEXT (DirectWrite — Chrome's text engine on Windows)
        // ================================================================

        public void fillText(string text, double x, double y)
        {
            _session.DrawText(text, (float)x, (float)y,
                _currentState.FillBrush, _currentState.TextFormat);
        }

        // ================================================================
        // STATE (polyfill — manual save/restore stack)
        // ================================================================

        public void save()
        {
            _stateStack.Push(_currentState.Clone());
        }

        public void restore()
        {
            if (_stateStack.Count > 0)
            {
                _currentState = _stateStack.Pop();
                _session.Transform = _currentState.Transform;
            }
        }

        // ================================================================
        // PROPERTIES
        // ================================================================

        public double globalAlpha
        {
            get => _currentState.GlobalAlpha;
            set => _currentState.GlobalAlpha = value;
        }

        public double lineWidth
        {
            get => _currentState.LineWidth;
            set => _currentState.LineWidth = value;
        }

        // ================================================================
        // Internal helpers
        // ================================================================

        private void CloseFigureIfOpen()
        {
            if (_figureOpen)
            {
                _pathBuilder.EndFigure(CanvasFigureLoop.Open);
                _figureOpen = false;
            }
        }

        private void EnsureFigureOpen(double x, double y)
        {
            if (!_figureOpen)
            {
                _pathBuilder.BeginFigure((float)x, (float)y);
                _figureOpen = true;
                _subpathStart = new Vector2((float)x, (float)y);
            }
        }
    }

    /// <summary>
    /// Canvas 2D state snapshot — used for save()/restore() polyfill.
    /// Win2D/Direct2D has no native state stack, so we manage it manually.
    /// Same pattern as GDI+ backend (Legacy/Drawing/Context.Drawing2D).
    /// </summary>
    internal class Win2DState
    {
        private readonly CanvasDevice _device;

        public Matrix3x2 Transform { get; set; } = Matrix3x2.Identity;
        public ICanvasBrush FillBrush { get; set; }
        public ICanvasBrush StrokeBrush { get; set; }
        public double LineWidth { get; set; } = 1.0;
        public double GlobalAlpha { get; set; } = 1.0;
        public CanvasStrokeStyle StrokeStyle { get; set; }
        public CanvasTextFormat TextFormat { get; set; }

        public Win2DState(CanvasDevice device)
        {
            _device = device;
            FillBrush = new CanvasSolidColorBrush(device, Windows.UI.Colors.Black);
            StrokeBrush = new CanvasSolidColorBrush(device, Windows.UI.Colors.Black);
            StrokeStyle = new CanvasStrokeStyle();
            TextFormat = new CanvasTextFormat { FontSize = 10 };
        }

        public Win2DState Clone()
        {
            return new Win2DState(_device)
            {
                Transform = Transform,
                FillBrush = FillBrush, // Note: brushes are shared, not cloned
                StrokeBrush = StrokeBrush,
                LineWidth = LineWidth,
                GlobalAlpha = GlobalAlpha,
                StrokeStyle = StrokeStyle,
                TextFormat = TextFormat,
            };
        }
    }
}
