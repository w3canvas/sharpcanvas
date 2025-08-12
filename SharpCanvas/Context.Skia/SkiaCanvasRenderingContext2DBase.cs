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
        protected Stack<double> _globalAlphaStack = new Stack<double>();

        public SkiaCanvasRenderingContext2DBase(SKSurface surface)
        {
            _surface = surface;
            _path = new SKPath();
            _fillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black };
            _strokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 1 };
            this.globalCompositeOperation = "source-over";
            this.lineCap = "butt";
            this.lineJoin = "miter";
            this.shadowColor = "rgba(0, 0, 0, 0)";
            this.font = "10px sans-serif";
            this.textAlign = "start";
            this.textBaseLine = "alphabetic";
        }

        public void fillRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            using (var paint = ApplyPaint(_fillPaint))
            {
                canvas.DrawRect((float)x, (float)y, (float)w, (float)h, paint);
            }
        }

        public void strokeRect(double x, double y, double w, double h)
        {
            var canvas = _surface.Canvas;
            using (var paint = ApplyPaint(_strokePaint))
            {
                canvas.DrawRect((float)x, (float)y, (float)w, (float)h, paint);
            }
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
            _globalAlphaStack.Push(_globalAlpha);
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
            if (_globalAlphaStack.Count > 0)
            {
                globalAlpha = _globalAlphaStack.Pop();
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
                }
            }
        }

        private SKPaint ApplyPaint(SKPaint paint)
        {
            var newPaint = paint.Clone();
            newPaint.Color = newPaint.Color.WithAlpha((byte)(newPaint.Color.Alpha * _globalAlpha));
            return newPaint;
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
        private double _lineWidth;
        public double lineWidth
        {
            get => _lineWidth;
            set
            {
                _lineWidth = value;
                _strokePaint.StrokeWidth = (float)value;
            }
        }

        private string _lineCap;
        public string lineCap
        {
            get => _lineCap;
            set
            {
                _lineCap = value;
                _strokePaint.StrokeCap = GetStrokeCap(value);
            }
        }

        private SKStrokeCap GetStrokeCap(string cap)
        {
            return cap.ToLower() switch
            {
                "butt" => SKStrokeCap.Butt,
                "round" => SKStrokeCap.Round,
                "square" => SKStrokeCap.Square,
                _ => SKStrokeCap.Butt,
            };
        }

        private string _lineJoin;
        public string lineJoin
        {
            get => _lineJoin;
            set
            {
                _lineJoin = value;
                _strokePaint.StrokeJoin = GetStrokeJoin(value);
            }
        }

        private SKStrokeJoin GetStrokeJoin(string join)
        {
            return join.ToLower() switch
            {
                "round" => SKStrokeJoin.Round,
                "bevel" => SKStrokeJoin.Bevel,
                "miter" => SKStrokeJoin.Miter,
                _ => SKStrokeJoin.Miter,
            };
        }

        private double _miterLimit;
        public double miterLimit
        {
            get => _miterLimit;
            set
            {
                _miterLimit = value;
                _strokePaint.StrokeMiter = (float)value;
            }
        }
        private double _shadowOffsetX;
        public double shadowOffsetX
        {
            get => _shadowOffsetX;
            set
            {
                _shadowOffsetX = value;
                UpdateShadows();
            }
        }

        private double _shadowOffsetY;
        public double shadowOffsetY
        {
            get => _shadowOffsetY;
            set
            {
                _shadowOffsetY = value;
                UpdateShadows();
            }
        }

        private double _shadowBlur;
        public double shadowBlur
        {
            get => _shadowBlur;
            set
            {
                _shadowBlur = value;
                UpdateShadows();
            }
        }

        private string _shadowColor;
        public string shadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                UpdateShadows();
            }
        }

        private void UpdateShadows()
        {
            var color = ColorParser.Parse(_shadowColor ?? "rgba(0,0,0,0)");
            bool hasShadow = color.Alpha != 0 && (_shadowBlur > 0 || _shadowOffsetX != 0 || _shadowOffsetY != 0);

            _fillPaint.ImageFilter?.Dispose();
            _strokePaint.ImageFilter?.Dispose();

            if (hasShadow)
            {
                _fillPaint.ImageFilter = SKImageFilter.CreateDropShadow(
                    (float)_shadowOffsetX,
                    (float)_shadowOffsetY,
                    (float)_shadowBlur,
                    (float)_shadowBlur,
                    color
                );
                _strokePaint.ImageFilter = SKImageFilter.CreateDropShadow(
                    (float)_shadowOffsetX,
                    (float)_shadowOffsetY,
                    (float)_shadowBlur,
                    (float)_shadowBlur,
                    color
                );
            }
            else
            {
                _fillPaint.ImageFilter = null;
                _strokePaint.ImageFilter = null;
            }
        }
        private string _font;
        public string font
        {
            get => _font;
            set
            {
                _font = value;
                FontUtils.ApplyFont(_font, _fillPaint);
                FontUtils.ApplyFont(_font, _strokePaint);
            }
        }

        private string _textAlign;
        public string textAlign
        {
            get => _textAlign;
            set
            {
                _textAlign = value;
                UpdateTextAlign();
            }
        }

        private void UpdateTextAlign()
        {
            var align = _textAlign.ToLower() switch
            {
                "left" => SKTextAlign.Left,
                "right" => SKTextAlign.Right,
                "center" => SKTextAlign.Center,
                "start" => SKTextAlign.Left, // Assuming LTR for now
                "end" => SKTextAlign.Right, // Assuming LTR for now
                _ => SKTextAlign.Left,
            };
            _fillPaint.TextAlign = align;
            _strokePaint.TextAlign = align;
        }

        public string textBaseLine { get; set; }
        public object canvas => throw new System.NotImplementedException();
        public bool IsVisible => true;

        public void clearRect(double x, double y, double w, double h)
        {
            using (var paint = new SKPaint { BlendMode = SKBlendMode.Clear })
            {
                _surface.Canvas.DrawRect((float)x, (float)y, (float)w, (float)h, paint);
            }
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
            using (var paint = ApplyPaint(_fillPaint))
            {
                _surface.Canvas.DrawPath(_path, paint);
            }
        }

        public void stroke()
        {
            using (var paint = ApplyPaint(_strokePaint))
            {
                _surface.Canvas.DrawPath(_path, paint);
            }
        }

        public void clip()
        {
            _surface.Canvas.ClipPath(_path);
        }

        public void fillText(string text, double x, double y)
        {
            var yOffset = FontUtils.GetYOffset(textBaseLine, _fillPaint);
            using (var paint = ApplyPaint(_fillPaint))
            {
                _surface.Canvas.DrawText(text, (float)x, (float)y + yOffset, paint);
            }
        }

        public void strokeText(string text, double x, double y)
        {
            var yOffset = FontUtils.GetYOffset(textBaseLine, _strokePaint);
            using (var paint = ApplyPaint(_strokePaint))
            {
                _surface.Canvas.DrawText(text, (float)x, (float)y + yOffset, paint);
            }
        }

        public void drawImage(object image, double sx, double sy, double sw, double sh, double dx, double dy, double dw, double dh)
        {
            var bitmap = GetBitmapFromImageSource(image);
            if (bitmap != null)
            {
                var sourceRect = new SKRect((float)sx, (float)sy, (float)(sx + sw), (float)(sy + sh));
                var destRect = new SKRect((float)dx, (float)dy, (float)(dx + dw), (float)(dy + dh));
                _surface.Canvas.DrawBitmap(bitmap, sourceRect, destRect);
                if (ShouldDisposeBitmap(image))
                {
                    bitmap.Dispose();
                }
            }
        }

        public void drawImage(object pImg, double dx, double dy, double dw, double dh)
        {
            var bitmap = GetBitmapFromImageSource(pImg);
            if (bitmap != null)
            {
                var destRect = new SKRect((float)dx, (float)dy, (float)(dx + dw), (float)(dy + dh));
                _surface.Canvas.DrawBitmap(bitmap, destRect);
                if (ShouldDisposeBitmap(pImg))
                {
                    bitmap.Dispose();
                }
            }
        }

        public void drawImage(object pImg, double dx, double dy)
        {
            var bitmap = GetBitmapFromImageSource(pImg);
            if (bitmap != null)
            {
                _surface.Canvas.DrawBitmap(bitmap, (float)dx, (float)dy);
                if (ShouldDisposeBitmap(pImg))
                {
                    bitmap.Dispose();
                }
            }
        }
        private SKBitmap? GetBitmapFromImageSource(object imageSource)
        {
            if (imageSource is SKBitmap bitmap) return bitmap;
            if (imageSource is IImage image) return image.getImage() as SKBitmap;
            if (imageSource is IHTMLCanvasElement canvas)
            {
                var context = canvas.getCanvas();
                var bytes = context.GetBitmap();
                return SKBitmap.Decode(bytes);
            }
            return null;
        }

        private bool ShouldDisposeBitmap(object imageSource)
        {
            return imageSource is IHTMLCanvasElement;
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
            var width = _fillPaint.MeasureText(text);
            var metrics = _fillPaint.FontMetrics;
            var height = metrics.Descent - metrics.Ascent;
            return new TextMetrics { width = (int)width, height = (int)height };
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
