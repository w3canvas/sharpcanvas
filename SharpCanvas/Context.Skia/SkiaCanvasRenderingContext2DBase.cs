#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;

namespace SharpCanvas.Context.Skia
{
    public abstract class SkiaCanvasRenderingContext2DBase : ICanvasRenderingContext2D
    {
        protected SKSurface _surface;
        protected SKPath _path;
        protected SKPaint _fillPaint;
        protected SKPaint _strokePaint;
        protected Stack<SKPaint> _fillPaintStack = new Stack<SKPaint>();
        protected Stack<SKPaint> _strokePaintStack = new Stack<SKPaint>();

        public SkiaCanvasRenderingContext2DBase(SKSurface surface)
        {
            _surface = surface;
            _path = new SKPath();
            _fillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black };
            _strokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 1 };
            globalCompositeOperation = "source-over";
            lineCap = "butt";
            lineJoin = "miter";
            shadowColor = "rgba(0, 0, 0, 0)";
            font = "10px sans-serif";
            textAlign = "start";
            textBaseLine = "alphabetic";
            this.globalCompositeOperation = "source-over";
        }

        public void fillRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            canvas.DrawRect((float)x, (float)y, (float)w, (float)h, _fillPaint);
        }

        public void strokeRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            canvas.DrawRect((float)x, (float)y, (float)w, (float)h, _strokePaint);
        }

        public object prototype()
        {
            throw new System.NotImplementedException();
        }

        public object __proto__ => throw new System.NotImplementedException();

        public void save()
        {
            _surface.Canvas.Save();
            _fillPaintStack.Push(_fillPaint.Clone());
            _strokePaintStack.Push(_strokePaint.Clone());
        }

        public void restore()
        {
            _surface.Canvas.Restore();
            if (_fillPaintStack.Count > 0)
            {
                _fillPaint = _fillPaintStack.Pop();
            }
            if (_strokePaintStack.Count > 0)
            {
                _strokePaint = _strokePaintStack.Pop();
            }
        }

        public void scale(double x, double y)
        {
            _surface.Canvas.Scale((float)x, (float)y);
        }

        public void rotate(double angle)
        {
            _surface.Canvas.RotateDegrees((float)(angle * 180 / System.Math.PI));
        }

        public void translate(double x, double y)
        {
            _surface.Canvas.Translate((float)x, (float)y);
        }

        public void transform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            var matrix = new SKMatrix
            {
                ScaleX = (float)m11,
                SkewX = (float)m21,
                TransX = (float)dx,
                SkewY = (float)m12,
                ScaleY = (float)m22,
                TransY = (float)dy,
                Persp0 = 0,
                Persp1 = 0,
                Persp2 = 1
            };
            _surface.Canvas.Concat(ref matrix);
        }

        public void setTransform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            var matrix = new SKMatrix
            {
                ScaleX = (float)m11,
                SkewX = (float)m21,
                TransX = (float)dx,
                SkewY = (float)m12,
                ScaleY = (float)m22,
                TransY = (float)dy,
                Persp0 = 0,
                Persp1 = 0,
                Persp2 = 1
            };
            _surface.Canvas.SetMatrix(matrix);
        }

        private double _globalAlpha = 1.0;
        public double globalAlpha
        {
            get => _globalAlpha;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _globalAlpha = value;
                    _fillPaint.Color = _fillPaint.Color.WithAlpha((byte)(value * 255));
                    _strokePaint.Color = _strokePaint.Color.WithAlpha((byte)(value * 255));
                }
            }
        }
        private string? _globalCompositeOperation;
        public string? globalCompositeOperation
        {
            get => _globalCompositeOperation;
            set
            {
                if (value != null)
                {
                    _globalCompositeOperation = value;
                    _fillPaint.BlendMode = GetBlendMode(value);
                    _strokePaint.BlendMode = GetBlendMode(value);
                }
            }
        }

        private SKBlendMode GetBlendMode(string compositeOperation)
        {
            return compositeOperation.ToLower() switch
            {
                "source-over" => SKBlendMode.SrcOver,
                "source-in" => SKBlendMode.SrcIn,
                "source-out" => SKBlendMode.SrcOut,
                "source-atop" => SKBlendMode.SrcATop,
                "destination-over" => SKBlendMode.DstOver,
                "destination-in" => SKBlendMode.DstIn,
                "destination-out" => SKBlendMode.DstOut,
                "destination-atop" => SKBlendMode.DstATop,
                "lighter" => SKBlendMode.Lighten,
                "copy" => SKBlendMode.Src,
                "xor" => SKBlendMode.Xor,
                "multiply" => SKBlendMode.Multiply,
                "screen" => SKBlendMode.Screen,
                "overlay" => SKBlendMode.Overlay,
                "darken" => SKBlendMode.Darken,
                "lighten" => SKBlendMode.Lighten,
                "color-dodge" => SKBlendMode.ColorDodge,
                "color-burn" => SKBlendMode.ColorBurn,
                "hard-light" => SKBlendMode.HardLight,
                "soft-light" => SKBlendMode.SoftLight,
                "difference" => SKBlendMode.Difference,
                "exclusion" => SKBlendMode.Exclusion,
                "hue" => SKBlendMode.Hue,
                "saturation" => SKBlendMode.Saturation,
                "color" => SKBlendMode.Color,
                "luminosity" => SKBlendMode.Luminosity,
                _ => SKBlendMode.SrcOver,
            };
        }
        public object strokeStyle
        {
            get => _strokePaint.Color.ToString();
            set
            {
                if (value is string colorString)
                {
                    _strokePaint.Color = ColorParser.Parse(colorString);
                }
            }
        }
        public object fillStyle
        {
            get => _fillPaint.Color.ToString();
            set
            {
                if (value is string colorString)
                {
                    _fillPaint.Color = ColorParser.Parse(colorString);
                }
            }
        }
        public double lineWidth { get; set; }
        public string lineCap { get; set; }
        public string lineJoin { get; set; }
        public double miterLimit { get; set; }
        public double shadowOffsetX { get; set; }
        public double shadowOffsetY { get; set; }
        public double shadowBlur { get; set; }
        public string shadowColor { get; set; }
        public string font { get; set; }
        public string textAlign { get; set; }
        public string textBaseLine { get; set; }
        public object canvas => throw new System.NotImplementedException();
        public bool IsVisible => true;

        public void clearRect(double x, double y, double w, double h)
        {
            _surface.Canvas.Clear(SKColors.Transparent);
        }

        public void beginPath()
        {
            _path.Reset();
        }

        public void closePath()
        {
            _path.Close();
        }

        public void moveTo(double x, double y)
        {
            _path.MoveTo((float)x, (float)y);
        }

        public void lineTo(double x, double y)
        {
            _path.LineTo((float)x, (float)y);
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            _path.QuadTo((float)cpx, (float)cpy, (float)x, (float)y);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            _path.CubicTo((float)cp1x, (float)cp1y, (float)cp2x, (float)cp2y, (float)x, (float)y);
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            _path.ArcTo((float)x1, (float)y1, (float)x2, (float)y2, (float)radius);
        }

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool clockwise)
        {
            _path.AddArc(new SKRect((float)(x - r), (float)(y - r), (float)(x + r), (float)(y + r)), (float)(startAngle * 180 / System.Math.PI), (float)((endAngle - startAngle) * 180 / System.Math.PI));
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.AddRect(new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h)));
        }

        public void fill()
        {
            _surface.Canvas.DrawPath(_path, _fillPaint);
        }

        public void stroke()
        {
            _surface.Canvas.DrawPath(_path, _strokePaint);
        }

        public void clip()
        {
            _surface.Canvas.ClipPath(_path);
        }

        public void fillText(string text, double x, double y)
        {
            throw new System.NotImplementedException();
        }

        public void strokeText(string text, double x, double y)
        {
            throw new System.NotImplementedException();
        }

        public void drawImage(object image, double sx, double sy, double sw, double sh, double dx, double dy, double dw, double dh)
        {
            throw new System.NotImplementedException();
        }

        public void drawImage(object pImg, double dx, double dy, double dw, double dh)
        {
            throw new System.NotImplementedException();
        }

        public void drawImage(object pImg, double dx, double dy)
        {
            throw new System.NotImplementedException();
        }

        public void drawImage(object pImg, float dx, float dy)
        {
            if (pImg is SKBitmap bitmap)
            {
                _surface.Canvas.DrawBitmap(bitmap, dx, dy);
            }
        }

        public bool isPointInPath(double x, double y)
        {
            throw new System.NotImplementedException();
        }

        public object createLinearGradient(double x0, double y0, double x1, double y1)
        {
            throw new System.NotImplementedException();
        }

        public object createPattern(object pImg, string repeat)
        {
            throw new System.NotImplementedException();
        }

        public object createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            throw new System.NotImplementedException();
        }

        public object measureText(string text)
        {
            throw new System.NotImplementedException();
        }

        public object getImageData(double sx, double sy, double sw, double sh)
        {
            throw new System.NotImplementedException();
        }

        public object createImageData(double sw, double sh)
        {
            throw new System.NotImplementedException();
        }

        public void putImageData(object pData, double dx, double dy)
        {
            throw new System.NotImplementedException();
        }

        public void putImageData(object imagedata, double dx, double dy, double dirtyX, double dirtyY, double dirtyWidth, double dirtyHeight)
        {
            throw new System.NotImplementedException();
        }

        public object createFilterChain()
        {
            throw new System.NotImplementedException();
        }

        public void commit()
        {
            // No-op for Skia
        }

        public byte[] GetBitmap()
        {
            using (var image = _surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new System.IO.MemoryStream())
            {
                data.SaveTo(stream);
                return stream.ToArray();
            }
        }

        public void ChangeSize(int width, int height, bool reset)
        {
            throw new System.NotImplementedException();
        }

        public int GetHeight()
        {
            return _surface.Canvas.DeviceClipBounds.Height;
        }

        public int GetWidth()
        {
            return _surface.Canvas.DeviceClipBounds.Width;
        }

    }
}
