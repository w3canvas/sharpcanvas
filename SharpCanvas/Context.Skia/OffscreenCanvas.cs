using SharpCanvas.Shared;
using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class OffscreenCanvas
    {
        private SKSurface _surface;
        private int _width;
        private int _height;
        public IDocument Document { get; }

        public OffscreenCanvas(int width, int height, IDocument document)
        {
            _width = width;
            _height = height;
            Document = document;
            _surface = SKSurface.Create(new SKImageInfo(width, height));
        }

        public ICanvasRenderingContext2D getContext(string contextId)
        {
            if (contextId == "2d")
            {
                return new OffscreenCanvasRenderingContext2D(_surface, Document, this);
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
