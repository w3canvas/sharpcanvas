using SharpCanvas.Shared;
using SkiaSharp;
using System;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Helper class for running canvas operations in a background worker.
    /// Uses OffscreenCanvas and ImageBitmap for efficient off-thread rendering.
    /// </summary>
    public class CanvasWorker
    {
        public delegate void DrawDelegate(OffscreenCanvas canvas);
        public event EventHandler<ImageBitmap> OnWorkComplete;

        /// <summary>
        /// Runs a canvas drawing operation in the background and returns the result as an ImageBitmap
        /// </summary>
        public void Run(DrawDelegate drawDelegate, int width, int height, IDocument document)
        {
            Task.Run(() =>
            {
                var canvas = new OffscreenCanvas(width, height, document);
                drawDelegate(canvas);
                var imageBitmap = canvas.transferToImageBitmap();
                OnWorkComplete?.Invoke(this, imageBitmap);
            });
        }
    }
}
