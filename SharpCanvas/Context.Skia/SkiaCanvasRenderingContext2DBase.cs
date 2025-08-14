#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

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
        protected System.Threading.Tasks.Task _fontLoadingTask = System.Threading.Tasks.Task.CompletedTask;

        public void SetFontLoadingTask(System.Threading.Tasks.Task task)
        {
            _fontLoadingTask = task;
        }

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
            return this;
        }

        public object __proto__ => this;

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
        private string _globalCompositeOperation = "source-over";
        public string globalCompositeOperation
        {
            get => _globalCompositeOperation;
            set
            {
                _globalCompositeOperation = value;
                _fillPaint.BlendMode = GetBlendMode(value);
                _strokePaint.BlendMode = GetBlendMode(value);
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
        private object _fillStyleObject = "#000000";
        private object _strokeStyleObject = "#000000";

        public object strokeStyle
        {
            get => _strokeStyleObject;
            set
            {
                _strokeStyleObject = value;
                if (value is string colorString)
                {
                    _strokePaint.Shader?.Dispose();
                    _strokePaint.Shader = null;
                    _strokePaint.Color = ColorParser.Parse(colorString);
                }
                else if (value is SkiaLinearCanvasGradient gradient)
                {
                    _strokePaint.Shader?.Dispose();
                    _strokePaint.Shader = gradient.GetShader();
                }
                else if (value is SkiaCanvasPattern pattern)
                {
                    _strokePaint.Shader?.Dispose();
                    _strokePaint.Shader = pattern.GetShader();
                }
                else if (value is SkiaRadialCanvasGradient radialGradient)
                {
                    _strokePaint.Shader?.Dispose();
                    _strokePaint.Shader = radialGradient.GetShader();
                }
                else if (value is SkiaConicCanvasGradient conicGradient)
                {
                    _strokePaint.Shader?.Dispose();
                    _strokePaint.Shader = conicGradient.GetShader();
                }
            }
        }
        public object fillStyle
        {
            get => _fillStyleObject;
            set
            {
                _fillStyleObject = value;
                if (value is string colorString)
                {
                    _fillPaint.Shader?.Dispose();
                    _fillPaint.Shader = null;
                    _fillPaint.Color = ColorParser.Parse(colorString);
                }
                else if (value is SkiaLinearCanvasGradient gradient)
                {
                    _fillPaint.Shader?.Dispose();
                    _fillPaint.Shader = gradient.GetShader();
                }
                else if (value is SkiaCanvasPattern pattern)
                {
                    _fillPaint.Shader?.Dispose();
                    _fillPaint.Shader = pattern.GetShader();
                }
                else if (value is SkiaRadialCanvasGradient radialGradient)
                {
                    _fillPaint.Shader?.Dispose();
                    _fillPaint.Shader = radialGradient.GetShader();
                }
                else if (value is SkiaConicCanvasGradient conicGradient)
                {
                    _fillPaint.Shader?.Dispose();
                    _fillPaint.Shader = conicGradient.GetShader();
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

        private string _lineCap = "butt";
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

        private string _lineJoin = "miter";
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

        private string _shadowColor = "rgba(0, 0, 0, 0)";
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
        private string _font = "10px sans-serif";
        public string font
        {
            get => _font;
            set
            {
                _font = value;
                FontUtils.ApplyFont(this, _fillPaint);
                FontUtils.ApplyFont(this, _strokePaint);
            }
        }

        private string _textAlign = "start";
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
                "start" => direction == "rtl" ? SKTextAlign.Right : SKTextAlign.Left,
                "end" => direction == "rtl" ? SKTextAlign.Left : SKTextAlign.Right,
                _ => SKTextAlign.Left,
            };
            _fillPaint.TextAlign = align;
            _strokePaint.TextAlign = align;
        }

        public string textBaseLine { get; set; }
        public FontFaceSet fonts { get; } = new FontFaceSet();
        object ICanvasRenderingContext2D.fonts => fonts;

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

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool anticlockwise)
        {
            var startDegrees = (float)(startAngle * 180 / System.Math.PI);
            var endDegrees = (float)(endAngle * 180 / System.Math.PI);
            var sweepAngle = endDegrees - startDegrees;

            if (anticlockwise) // we want positive sweep for CCW
            {
                if (sweepAngle < 0)
                {
                    sweepAngle += 360;
                }
            }
            else // clockwise, we want negative sweep
            {
                if (sweepAngle > 0)
                {
                    sweepAngle -= 360;
                }
            }

            using (var arcPath = new SKPath())
            {
                arcPath.AddArc(new SKRect((float)(x - r), (float)(y - r), (float)(x + r), (float)(y + r)), startDegrees, sweepAngle);
                _path.AddPath(arcPath);
            }
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
            _fontLoadingTask.Wait();
            // Re-apply font settings in case they were loaded asynchronously
            FontUtils.ApplyFont(this, _fillPaint);
            var yOffset = FontUtils.GetYOffset(textBaseLine, _fillPaint);
            using (var paint = ApplyPaint(_fillPaint))
            {
                _surface.Canvas.DrawText(text, (float)x, (float)y + yOffset, paint);
            }
        }

        public void strokeText(string text, double x, double y)
        {
            _fontLoadingTask.Wait();
            // Re-apply font settings in case they were loaded asynchronously
            FontUtils.ApplyFont(this, _strokePaint);
            var yOffset = FontUtils.GetYOffset(textBaseLine, _strokePaint);
            using (var paint = ApplyPaint(_strokePaint))
            {
                _surface.Canvas.DrawText(text, (float)x, (float)y + yOffset, paint);
            }
        }

        private SKPaint GetImagePaint()
        {
            var filterQuality = imageSmoothingQuality switch
            {
                "high" => SKFilterQuality.High,
                "medium" => SKFilterQuality.Medium,
                "low" => SKFilterQuality.Low,
                _ => SKFilterQuality.Low,
            };
            return new SKPaint { FilterQuality = imageSmoothingEnabled ? filterQuality : SKFilterQuality.None };
        }

        public void drawImage(object image, double sx, double sy, double sw, double sh, double dx, double dy, double dw, double dh)
        {
            var bitmap = GetBitmapFromImageSource(image);
            if (bitmap != null)
            {
                using (var paint = GetImagePaint())
                {
                    var sourceRect = new SKRect((float)sx, (float)sy, (float)(sx + sw), (float)(sy + sh));
                    var destRect = new SKRect((float)dx, (float)dy, (float)(dx + dw), (float)(dy + dh));
                    _surface.Canvas.DrawBitmap(bitmap, sourceRect, destRect, paint);
                }
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
                using (var paint = GetImagePaint())
                {
                    var destRect = new SKRect((float)dx, (float)dy, (float)(dx + dw), (float)(dy + dh));
                    _surface.Canvas.DrawBitmap(bitmap, destRect, paint);
                }
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
                using (var paint = GetImagePaint())
                {
                    _surface.Canvas.DrawBitmap(bitmap, (float)dx, (float)dy, paint);
                }
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
            return _path.Contains((float)x, (float)y);
        }

        public object createLinearGradient(double x0, double y0, double x1, double y1)
        {
            var startPoint = new SKPoint((float)x0, (float)y0);
            var endPoint = new SKPoint((float)x1, (float)y1);
            return new SkiaLinearCanvasGradient(startPoint, endPoint);
        }

        public object createPattern(object pImg, string repeat)
        {
            var bitmap = GetBitmapFromImageSource(pImg);
            if (bitmap != null)
            {
                return new SkiaCanvasPattern(bitmap, repeat);
            }
            // This behavior is not defined in the spec, but throwing an exception is reasonable.
            throw new System.ArgumentException("Invalid image source for createPattern");
        }

        public object createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            var start = new SKPoint((float)x0, (float)y0);
            var end = new SKPoint((float)x1, (float)y1);
            return new SkiaRadialCanvasGradient(start, (float)r0, end, (float)r1);
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
            var x = (int)sx;
            var y = (int)sy;
            var width = (int)sw;
            var height = (int)sh;

            var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            var data = new byte[width * height * 4];
            var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            bool success;
            try
            {
                var ptr = gcHandle.AddrOfPinnedObject();
                success = _surface.ReadPixels(info, ptr, info.RowBytes, x, y);
            }
            finally
            {
                gcHandle.Free();
            }

            if (success)
            {
                return new ImageData((uint)width, (uint)height) { data = data };
            }
            else
            {
                return createImageData(sw, sh);
            }
        }

        public object createImageData(double sw, double sh)
        {
            if (double.IsNaN(sw) || double.IsInfinity(sw) || sw <= 0 ||
                double.IsNaN(sh) || double.IsInfinity(sh) || sh <= 0)
            {
                throw new System.NotSupportedException("Invalid arguments for createImageData");
            }
            var width = (uint)sw;
            var height = (uint)sh;
            var data = new byte[width * height * 4];
            return new ImageData(width, height) { data = data };
        }

        public void putImageData(object pData, double dx, double dy)
        {
            if (pData is ImageData imageData && imageData.data is byte[] bytes)
            {
                var info = new SKImageInfo((int)imageData.width, (int)imageData.height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
                using (var bitmap = new SKBitmap(info))
                {
                    var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
                    try
                    {
                        var ptr = gcHandle.AddrOfPinnedObject();
                        bitmap.InstallPixels(info, ptr);
                        _surface.Canvas.DrawBitmap(bitmap, (float)dx, (float)dy);
                    }
                    finally
                    {
                        gcHandle.Free();
                    }
                }
            }
        }

        public void putImageData(object imagedata, double dx, double dy, double dirtyX, double dirtyY, double dirtyWidth, double dirtyHeight)
        {
            if (imagedata is ImageData sourceImageData && sourceImageData.data is byte[] bytes)
            {
                var info = new SKImageInfo((int)sourceImageData.width, (int)sourceImageData.height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

                var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    var ptr = gcHandle.AddrOfPinnedObject();
                    using (var fullBitmap = new SKBitmap())
                    {
                        fullBitmap.InstallPixels(info, ptr);

                        var subsetRect = SKRectI.Create((int)dirtyX, (int)dirtyY, (int)dirtyWidth, (int)dirtyHeight);
                        using (var subsetBitmap = new SKBitmap(subsetRect.Width, subsetRect.Height))
                        {
                            if (fullBitmap.ExtractSubset(subsetBitmap, subsetRect))
                            {
                                _surface.Canvas.DrawBitmap(subsetBitmap, (float)dx, (float)dy);
                            }
                        }
                    }
                }
                finally
                {
                    gcHandle.Free();
                }
            }
        }

        public object createFilterChain()
        {
            return new SkiaFilterChain();
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
            var pixmap = _surface.PeekPixels();
            var newInfo = pixmap != null ? new SKImageInfo(width, height, pixmap.Info.ColorType, pixmap.Info.AlphaType) : new SKImageInfo(width, height);

            var newSurface = SKSurface.Create(newInfo);

            if (!reset)
            {
                using (var snapshot = _surface.Snapshot())
                {
                    var sourceRect = new SKRect(0, 0, snapshot.Width, snapshot.Height);
                    var destRect = new SKRect(0, 0, width, height);
                    newSurface.Canvas.DrawImage(snapshot, sourceRect, destRect);
                }
            }

            _surface.Dispose();
            _surface = newSurface;

            _path.Reset();
        }

        public int GetHeight()
        {
            return _surface.Canvas.DeviceClipBounds.Height;
        }

        public int GetWidth()
        {
            return _surface.Canvas.DeviceClipBounds.Width;
        }

        private void UpdateFont()
        {
            FontUtils.ApplyFont(this, _fillPaint);
            FontUtils.ApplyFont(this, _strokePaint);
        }

        private void UpdateFilters()
        {
            var imageFilter = FilterParser.Parse(filter);
            _fillPaint.ImageFilter = imageFilter;
            _strokePaint.ImageFilter = imageFilter;
        }
        // MDN properties
        private string _direction = "ltr";
        public string direction { get => _direction; set { _direction = value; UpdateTextAlign(); } }
        private string _filter = "none";
        public string filter
        {
            get => _filter;
            set
            {
                _filter = value;
                UpdateFilters();
            }
        }

        private string _fontKerning = "auto";
        public string fontKerning { get => _fontKerning; set { _fontKerning = value; UpdateFont(); } }
        private string _fontStretch = "normal";
        public string fontStretch { get => _fontStretch; set { _fontStretch = value; UpdateFont(); } }
        private string _fontVariantCaps = "normal";
        public string fontVariantCaps { get => _fontVariantCaps; set { _fontVariantCaps = value; UpdateFont(); } }
        public bool imageSmoothingEnabled { get; set; } = true;
        public string imageSmoothingQuality { get; set; } = "low";
        public string lang { get; set; } = "en-US";

        private string _letterSpacing = "0px";
        public string letterSpacing { get => _letterSpacing; set { _letterSpacing = value; UpdateFont(); } }
        private double _lineDashOffset = 0.0;
        public double lineDashOffset
        {
            get => _lineDashOffset;
            set
            {
                _lineDashOffset = value;
                UpdateLineDash();
            }
        }
        private string _textRendering = "auto";
        public string textRendering { get => _textRendering; set { _textRendering = value; UpdateFont(); } }

        private string _wordSpacing = "0px";
        public string wordSpacing { get => _wordSpacing; set { _wordSpacing = value; UpdateFont(); } }

        public void resetTransform()
        {
            _surface.Canvas.ResetMatrix();
        }

        public object getTransform()
        {
            var matrix = _surface.Canvas.TotalMatrix;
            return new DOMMatrix(matrix.ScaleX, matrix.SkewY, matrix.SkewX, matrix.ScaleY, matrix.TransX, matrix.TransY);
        }

        public void reset()
        {
            resetTransform();
            globalAlpha = 1.0;
            globalCompositeOperation = "source-over";
            strokeStyle = "#000000";
            fillStyle = "#000000";
            lineWidth = 1.0;
            lineCap = "butt";
            lineJoin = "miter";
            miterLimit = 10.0;
            shadowOffsetX = 0;
            shadowOffsetY = 0;
            shadowBlur = 0;
            shadowColor = "rgba(0, 0, 0, 0)";
            font = "10px sans-serif";
            textAlign = "start";
            textBaseLine = "alphabetic";
            direction = "ltr";
            filter = "none";
            fontKerning = "auto";
            fontStretch = "normal";
            fontVariantCaps = "normal";
            imageSmoothingEnabled = true;
            imageSmoothingQuality = "low";
            lang = "en-US";
            letterSpacing = "0px";
            lineDashOffset = 0.0;
            textRendering = "auto";
            wordSpacing = "0px";
            setLineDash(new double[0]);
            _path.Reset();
            _surface.Canvas.ResetMatrix();
            _surface.Canvas.ClipRect(new SKRect(0, 0, _surface.Canvas.DeviceClipBounds.Width, _surface.Canvas.DeviceClipBounds.Height), SKClipOperation.Intersect, true);
        }

        public bool isContextLost()
        {
            return false; // SkiaSharp context is not lost in the same way a WebGL context is.
        }

        public void drawFocusIfNeeded(object element)
        {
            // No-op for now. This is for accessibility and browser focus rings.
        }

        public void ellipse(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool anticlockwise)
        {
            if (radiusX < 0 || radiusY < 0)
            {
                throw new System.NotSupportedException("Radius values for ellipse must be non-negative.");
            }

            var rect = new SKRect((float)(x - radiusX), (float)(y - radiusY), (float)(x + radiusX), (float)(y + radiusY));
            var startDegrees = (float)(startAngle * 180 / System.Math.PI);
            var endDegrees = (float)(endAngle * 180 / System.Math.PI);

            var sweepAngle = endDegrees - startDegrees;
            if (anticlockwise)
            {
                if (sweepAngle > 0)
                {
                    sweepAngle -= 360;
                }
            }
            else
            {
                if (sweepAngle < 0)
                {
                    sweepAngle += 360;
                }
            }

            using (var ellipsePath = new SKPath())
            {
                if (rotation != 0)
                {
                    var matrix = SKMatrix.CreateRotation((float)rotation, (float)x, (float)y);
                    ellipsePath.Transform(matrix);
                }

                ellipsePath.AddArc(rect, startDegrees, sweepAngle);
                _path.AddPath(ellipsePath);
            }
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            var rect = new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h));
            var radiiList = new List<float>();

            if (radii is System.Collections.IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    radiiList.Add(System.Convert.ToSingle(item));
                }
            }
            else if (radii is double || radii is int || radii is float)
            {
                radiiList.Add(System.Convert.ToSingle(radii));
            }

            if (radiiList.Count == 0)
            {
                _path.AddRect(rect);
                return;
            }

            float topLeft, topRight, bottomRight, bottomLeft;
            switch (radiiList.Count)
            {
                case 1:
                    topLeft = topRight = bottomRight = bottomLeft = radiiList[0];
                    break;
                case 2:
                    topLeft = bottomRight = radiiList[0];
                    topRight = bottomLeft = radiiList[1];
                    break;
                case 3:
                    topLeft = radiiList[0];
                    topRight = bottomLeft = radiiList[1];
                    bottomRight = radiiList[2];
                    break;
                case 4:
                    topLeft = radiiList[0];
                    topRight = radiiList[1];
                    bottomRight = radiiList[2];
                    bottomLeft = radiiList[3];
                    break;
                default:
                    // Spec says to use the first 4 values if more are provided.
                    topLeft = radiiList[0];
                    topRight = radiiList[1];
                    bottomRight = radiiList[2];
                    bottomLeft = radiiList[3];
                    break;
            }
            var roundRect = new SKRoundRect(rect);
            roundRect.SetRectRadii(rect, new[]
            {
                new SKPoint(topLeft, topLeft),
                new SKPoint(topRight, topRight),
                new SKPoint(bottomRight, bottomRight),
                new SKPoint(bottomLeft, bottomLeft),
            });
            _path.AddRoundRect(roundRect);
        }

        private double[] _lineDash = new double[0];

        private void UpdateLineDash()
        {
            if (_lineDash == null || _lineDash.Length == 0)
            {
                _strokePaint.PathEffect = null;
            }
            else
            {
                var intervals = _lineDash.Select(d => (float)d).ToArray();
                if (intervals.Length % 2 != 0)
                {
                    intervals = intervals.Concat(intervals).ToArray();
                }
                _strokePaint.PathEffect = SKPathEffect.CreateDash(intervals, (float)lineDashOffset);
            }
        }

        public void setLineDash(object segments)
        {
            if (segments is System.Collections.IEnumerable enumerable)
            {
                var list = new List<double>();
                foreach (var item in enumerable)
                {
                    list.Add(System.Convert.ToDouble(item));
                }
                _lineDash = list.ToArray();
            }
            else
            {
                _lineDash = new double[0];
            }
            UpdateLineDash();
        }

        public object getLineDash()
        {
            return _lineDash;
        }

        public object createConicGradient(double startAngle, double x, double y)
        {
            var center = new SKPoint((float)x, (float)y);
            var startDegrees = (float)(startAngle * 180 / System.Math.PI);
            return new SkiaConicCanvasGradient(startDegrees, center);
        }

        public bool isPointInStroke(double x, double y)
        {
            using (var strokePath = new SKPath())
            {
                _strokePaint.GetFillPath(_path, strokePath);
                return strokePath.Contains((float)x, (float)y);
            }
        }

        public object getContextAttributes()
        {
            return new ContextAttributes
            {
                alpha = true, // SKSurface always has an alpha channel
                colorSpace = "srgb", // Assuming sRGB for now
                desynchronized = false, // Not applicable in SkiaSharp
                willReadFrequently = false // Default value
            };
        }
    }
}
