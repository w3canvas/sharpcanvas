using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class OffscreenCanvas
    {
        private SKSurface _surface;
        private int _width;
        private int _height;

        public OffscreenCanvas(int width, int height)
        {
            _width = width;
            _height = height;
            _surface = SKSurface.Create(new SKImageInfo(width, height));
        }

        public ICanvasRenderingContext2D getContext(string contextId)
        {
            if (contextId == "2d")
            {
                return new OffscreenCanvasRenderingContext2D(_surface);
            }
            return null;
        }

        public SKBitmap transferToImageBitmap()
        {
            var image = _surface.Snapshot();
            var bitmap = new SKBitmap(image.Width, image.Height);
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.DrawImage(image, 0, 0);
            }
            return bitmap;
        }
    }
}
