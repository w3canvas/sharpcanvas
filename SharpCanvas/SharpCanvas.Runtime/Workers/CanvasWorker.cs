using SharpCanvas.Shared;
using System;
using System.Threading.Tasks;

namespace SharpCanvas.Runtime.Workers
{
    /// <summary>
    /// Helper class for running canvas operations in a background worker.
    /// Uses OffscreenCanvas and ImageBitmap for efficient off-thread rendering.
    /// </summary>
    public class CanvasWorker
    {
        private readonly IGraphicsFactory _graphicsFactory;

        public delegate void DrawDelegate(dynamic canvas);
        public event EventHandler<object> OnWorkComplete;

        public CanvasWorker(IGraphicsFactory graphicsFactory)
        {
            _graphicsFactory = graphicsFactory ?? throw new ArgumentNullException(nameof(graphicsFactory));
        }

        /// <summary>
        /// Runs a canvas drawing operation in the background and returns the result as an ImageBitmap
        /// </summary>
        public void Run(DrawDelegate drawDelegate, int width, int height, IDocument document)
        {
            Task.Run(() =>
            {
                dynamic canvas = _graphicsFactory.CreateOffscreenCanvas(width, height, document);
                drawDelegate(canvas);
                var imageBitmap = canvas.transferToImageBitmap();
                OnWorkComplete?.Invoke(this, imageBitmap);
            });
        }
    }
}
