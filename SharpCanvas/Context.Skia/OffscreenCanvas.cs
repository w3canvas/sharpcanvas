using SharpCanvas.Shared;
using SkiaSharp;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// OffscreenCanvas provides a canvas that can be rendered off screen.
    /// It is available in both Window and Worker contexts, making it ideal for
    /// offloading canvas rendering work to background threads.
    /// Implements ITransferable for zero-copy transfer between workers.
    /// </summary>
    public class OffscreenCanvas : ITransferable
    {
        private SKSurface _surface;
        private int _width;
        private int _height;
        private bool _isNeutered;
        private ICanvasRenderingContext2D _context;
        public IDocument Document { get; }

        public OffscreenCanvas(int width, int height, IDocument document)
        {
            _width = width;
            _height = height;
            _isNeutered = false;
            Document = document;
            _surface = SKSurface.Create(new SKImageInfo(width, height));
            _context = new OffscreenCanvasRenderingContext2D(_surface, Document, this);
        }

        /// <summary>
        /// Gets the width of the canvas
        /// </summary>
        public int width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    ResizeSurface();
                }
            }
        }

        /// <summary>
        /// Gets the height of the canvas
        /// </summary>
        public int height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    ResizeSurface();
                }
            }
        }

        private void ResizeSurface()
        {
            var oldSurface = _surface;
            _surface = SKSurface.Create(new SKImageInfo(_width, _height));
            _context = new OffscreenCanvasRenderingContext2D(_surface, Document, this);

            // Copy old content to new surface
            using (var snapshot = oldSurface.Snapshot())
            {
                _surface.Canvas.DrawImage(snapshot, 0, 0);
            }

            oldSurface.Dispose();
        }

        public ICanvasRenderingContext2D getContext(string contextId)
        {
            if (contextId == "2d")
            {
                return _context;
            }
            throw new NotSupportedException($"The context id '{contextId}' is not supported.");
        }

        /// <summary>
        /// Creates an ImageBitmap from the current canvas content.
        /// This is used for efficient frame capture in video/animation workflows.
        /// </summary>
        public ImageBitmap transferToImageBitmap()
        {
            _surface.Flush();
            var info = new SKImageInfo(_width, _height);
            var bitmap = new SKBitmap(info);
            _surface.ReadPixels(info, bitmap.GetPixels(), info.RowBytes, 0, 0);
            return new ImageBitmap(bitmap);
        }

        /// <summary>
        /// Converts the current canvas content to a Blob.
        /// Returns a Promise that resolves with a Blob containing the image data.
        /// </summary>
        /// <param name="type">The image format (e.g., "image/png", "image/jpeg")</param>
        /// <param name="quality">Image quality from 0 to 1 (for lossy formats like JPEG)</param>
        public async Task<byte[]> convertToBlob(string type = "image/png", double quality = 1.0)
        {
            using (var image = _surface.Snapshot())
            {
                SKEncodedImageFormat format;
                int qualityInt = (int)(quality * 100);

                // Parse the MIME type
                if (type.Contains("jpeg") || type.Contains("jpg"))
                {
                    format = SKEncodedImageFormat.Jpeg;
                }
                else if (type.Contains("webp"))
                {
                    format = SKEncodedImageFormat.Webp;
                }
                else
                {
                    // Default to PNG
                    format = SKEncodedImageFormat.Png;
                    qualityInt = 100; // PNG is lossless
                }

                using (var data = image.Encode(format, qualityInt))
                using (var stream = new MemoryStream())
                {
                    data.SaveTo(stream);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts the current canvas content to a Blob with default PNG format.
        /// </summary>
        public async Task<byte[]> convertToBlob()
        {
            return await convertToBlob("image/png", 1.0);
        }

        // ITransferable implementation
        /// <summary>
        /// Checks if this OffscreenCanvas has been transferred to another context
        /// </summary>
        public bool IsNeutered => _isNeutered;

        /// <summary>
        /// Marks this OffscreenCanvas as transferred. After neutering, the canvas cannot be used.
        /// </summary>
        public void Neuter()
        {
            _isNeutered = true;
        }

        private void ThrowIfNeutered()
        {
            if (_isNeutered)
            {
                throw new InvalidOperationException("The OffscreenCanvas has been transferred and can no longer be used.");
            }
        }
    }
}
