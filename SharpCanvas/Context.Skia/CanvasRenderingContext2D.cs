using SharpCanvas;
using SkiaSharp;
using System.Collections.Generic;

namespace SharpCanvas.Context.Skia
{
    public class CanvasRenderingContext2D : ICanvasRenderingContext2D
    {
        private SKSurface _surface;
        private SKPath _path;
        private SKPaint _fillPaint;
        private Stack<SKPaint> _fillPaintStack = new Stack<SKPaint>();

        public CanvasRenderingContext2D(SKSurface surface)
        {
            _surface = surface;
            _path = new SKPath();
            _fillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black };
        }

        public void fillRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            canvas.DrawRect((float)x, (float)y, (float)w, (float)h, _fillPaint);
        }

        public void strokeRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black, // Default color
                StrokeWidth = 1
            };
            canvas.DrawRect((float)x, (float)y, (float)w, (float)h, paint);
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
        }

        public void restore()
        {
            _surface.Canvas.Restore();
            if (_fillPaintStack.Count > 0)
            {
                _fillPaint = _fillPaintStack.Pop();
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

        public double globalAlpha { get; set; }
        public string globalCompositeOperation { get; set; }
        public object strokeStyle { get; set; }
        public object fillStyle
        {
            get => _fillPaint.Color.ToString();
            set
            {
                if (value is string colorString)
                {
                    if (SKColor.TryParse(colorString, out var color))
                    {
                        _fillPaint.Color = color;
                    }
                }
                // Later: Handle gradients and patterns
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
            _path.AddArc(new SKRect((float)(x-r), (float)(y-r), (float)(x+r), (float)(y+r)), (float)(startAngle * 180 / System.Math.PI), (float)((endAngle - startAngle) * 180 / System.Math.PI));
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.AddRect(new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h)));
        }

        public void fill()
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black
            };
            _surface.Canvas.DrawPath(_path, paint);
        }

        public void stroke()
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 1
            };
            _surface.Canvas.DrawPath(_path, paint);
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

        public event OnPartialDrawHanlder OnPartialDraw;
    }
}
