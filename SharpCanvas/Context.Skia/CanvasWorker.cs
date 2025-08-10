using SkiaSharp;
using System;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    public class CanvasWorker
    {
        public delegate void DrawDelegate(OffscreenCanvas canvas);
        public event EventHandler<SKBitmap> OnWorkComplete;

        public void Run(DrawDelegate drawDelegate, int width, int height)
        {
            Task.Run(() =>
            {
                var canvas = new OffscreenCanvas(width, height);
                drawDelegate(canvas);
                var bitmap = canvas.transferToImageBitmap();
                OnWorkComplete?.Invoke(this, bitmap);
            });
        }
    }
}
