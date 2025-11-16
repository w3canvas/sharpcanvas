using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpCanvas.Common;

namespace SharpCanvas.Context.Drawing2D
{
    /// <summary>
    /// Implements a radial gradient using System.Drawing's PathGradientBrush.
    /// This class adapts the HTML5 Canvas radial gradient API to PathGradientBrush semantics.
    /// </summary>
    /// <remarks>
    /// PathGradientBrush creates gradients from a center point outward to a boundary path.
    /// To match Canvas API behavior (gradient from inner circle to outer circle), we use:
    /// - Boundary path: ellipse at the end point with end radius (Canvas outer circle)
    /// - Center point: start point (Canvas inner circle center)
    /// - FocusScales: ratio of start radius to end radius (makes focus area match Canvas inner circle)
    /// </remarks>
    public class PathCanvasGradient : ILinearCanvasGradient //IPathCanvasGradient
    {
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string SYNTAX_ERR = "Syntax error";
        private readonly List<Color> _colors = new List<Color>();

        private readonly float _endRadius;
        private readonly List<float> _positions = new List<float>();
        private readonly float _startRadius;
        private PointF _end;
        private PointF _start;

        /// <summary>
        /// Initializes a new instance of the PathCanvasGradient class with specified start and end points (concentric circles).
        /// </summary>
        /// <param name="_start">The center point of the start circle</param>
        /// <param name="_end">The center point of the end circle</param>
        public PathCanvasGradient(PointF _start, PointF _end)
        {
            this._start = _start;
            this._end = _end;
        }

        /// <summary>
        /// Creates a radial gradient with default radii and a graphics path.
        /// </summary>
        /// <param name="_start">Center point of the start (inner) circle</param>
        /// <param name="_end">Center point of the end (outer) circle</param>
        /// <param name="path">Graphics path to use for the gradient boundary</param>
        public PathCanvasGradient(PointF _start, PointF _end, GraphicsPath path)
        {
            this._start = _start;
            this._end = _end;
            this.path = path;
        }

        /// <summary>
        /// Creates a radial gradient matching the HTML5 Canvas createRadialGradient API.
        /// </summary>
        /// <param name="_start">Center point of the start (inner) circle</param>
        /// <param name="r0">Radius of the start (inner) circle</param>
        /// <param name="_end">Center point of the end (outer) circle</param>
        /// <param name="r1">Radius of the end (outer) circle</param>
        /// <param name="path">Graphics path to use for the gradient boundary</param>
        public PathCanvasGradient(PointF _start, float r0, PointF _end, float r1, GraphicsPath path)
        {
            this._start = _start;
            this._end = _end;
            this.path = path;
            _startRadius = r0;
            _endRadius = r1;
        }

        /// <summary>
        /// Creates an empty radial gradient.
        /// </summary>
        public PathCanvasGradient()
        {
        }


        public GraphicsPath path { get; set; }

        public PointF start
        {
            get { return _start; }
            set { _start = value; }
        }

        public PointF end
        {
            get { return _end; }
            set { _end = value; }
        }

        #region ILinearCanvasGradient Members

        /// <summary>
        /// Adds a color stop to the gradient at the specified offset.
        /// </summary>
        /// <param name="offset">Position of the color stop, between 0.0 (start circle) and 1.0 (end circle)</param>
        /// <param name="color">Color value in any CSS color format</param>
        /// <exception cref="Exception">Thrown if offset is out of range or color is invalid</exception>
        public void addColorStop(double offset, string color)
        {
            if (!ColorUtils.isValidColor(color))
                throw new Exception(SYNTAX_ERR);
            if (offset < 0 || offset > 1)
                throw new Exception(INDEX_SIZE_ERR);
            _colors.Add(ColorUtils.ParseColor(color));
            _positions.Add((float) offset);
        }

        /// <summary>
        /// Creates and returns the PathGradientBrush with all configured color stops.
        /// </summary>
        /// <returns>A PathGradientBrush configured to match Canvas radial gradient behavior</returns>
        public object GetBrush()
        {
            // Edge case: If no color stops are defined, return a transparent brush
            if (_colors.Count == 0)
            {
                var path0 = new GraphicsPath();
                path0.AddEllipse(_end.X - _endRadius, _end.Y - _endRadius, _endRadius * 2, _endRadius * 2);
                var emptyGradient = new PathGradientBrush(path0);
                emptyGradient.CenterColor = Color.Transparent;
                return emptyGradient;
            }

            // Edge case: If only one color stop is defined, use it as a solid color
            if (_colors.Count == 1)
            {
                var path1 = new GraphicsPath();
                path1.AddEllipse(_end.X - _endRadius, _end.Y - _endRadius, _endRadius * 2, _endRadius * 2);
                var solidGradient = new PathGradientBrush(path1);
                solidGradient.CenterColor = _colors[0];
                solidGradient.CenterPoint = _start;
                return solidGradient;
            }

            var colorBlend = new ColorBlend(_colors.Count);

            // Ensure gradient spans full range [0, 1]
            // Add missing endpoints to match Canvas API behavior
            if (_positions[_positions.Count - 1] != 1.0)
            {
                _positions.Add(1.0f);
                _colors.Add(_colors[_colors.Count - 1]);
            }
            if (_positions[0] != 0.0)
            {
                _positions.Insert(0, 0.0f);
                _colors.Insert(0, _colors[0]);
            }

            // Create the boundary path (outer circle in Canvas terms)
            var path = new GraphicsPath();
            path.AddEllipse(_end.X - _endRadius, _end.Y - _endRadius, _endRadius*2, _endRadius*2);

            var gradient = new PathGradientBrush(path);

            // Set the center point to the inner circle center
            gradient.CenterPoint = _start;

            // FocusScales defines the size of the "center color" region as a percentage of the boundary
            // By setting it to startRadius/endRadius, we make the focus area match the Canvas inner circle
            gradient.FocusScales = new PointF(_startRadius/_endRadius, _startRadius/_endRadius);

            // IMPORTANT: PathGradientBrush and Canvas API have opposite gradient directions
            //
            // Canvas API radial gradient:
            //   - Position 0 = start circle edge (inner circle at radius r0)
            //   - Position 1 = end circle edge (outer circle at radius r1)
            //   - Gradient flows from inner (start) to outer (end)
            //
            // PathGradientBrush:
            //   - Position 0 = focus/center area (controlled by FocusScales)
            //   - Position 1 = boundary path edge
            //   - Gradient flows from center/focus outward to boundary
            //
            // Since we set:
            //   - PathGradientBrush boundary = Canvas outer circle (end)
            //   - PathGradientBrush focus = Canvas inner circle (start, via FocusScales)
            //
            // The geometries match, BUT PathGradientBrush's InterpolationColors positions
            // are interpreted in reverse order compared to the Canvas API. We must reverse
            // both colors and positions, then invert the position values to achieve the
            // correct gradient direction.

            _colors.Reverse();
            for (int i = 0; i < _positions.Count; i++)
            {
                _positions[i] = 1 - _positions[i];
            }
            _positions.Reverse();

            colorBlend.Colors = _colors.ToArray();
            colorBlend.Positions = _positions.ToArray();

            // InterpolationColors overrides CenterColor and SurroundColors
            gradient.InterpolationColors = colorBlend;

            return gradient;
        }

        #endregion
    }
}