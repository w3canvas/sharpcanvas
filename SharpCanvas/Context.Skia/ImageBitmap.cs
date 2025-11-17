using SkiaSharp;
using System;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Represents an ImageBitmap - a bitmap image which can be drawn to a canvas without undue latency.
    /// This is a high-performance image type designed for use with OffscreenCanvas and Workers.
    /// Implements ITransferable for zero-copy transfer between workers.
    /// </summary>
    public class ImageBitmap : ITransferable, IDisposable
    {
        private SKBitmap? _bitmap;
        private bool _isClosed;
        private bool _isNeutered;

        public ImageBitmap(SKBitmap bitmap)
        {
            _bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            _isClosed = false;
            _isNeutered = false;
        }

        /// <summary>
        /// Gets the width of the image in CSS pixels.
        /// </summary>
        public int width
        {
            get
            {
                ThrowIfClosed();
                return _bitmap?.Width ?? 0;
            }
        }

        /// <summary>
        /// Gets the height of the image in CSS pixels.
        /// </summary>
        public int height
        {
            get
            {
                ThrowIfClosed();
                return _bitmap?.Height ?? 0;
            }
        }

        /// <summary>
        /// Disposes of the bitmap data associated with this ImageBitmap.
        /// After calling close(), the ImageBitmap can no longer be used.
        /// </summary>
        public void close()
        {
            if (!_isClosed)
            {
                _bitmap?.Dispose();
                _bitmap = null;
                _isClosed = true;
            }
        }

        /// <summary>
        /// Gets the underlying SKBitmap. For internal use only.
        /// </summary>
        internal SKBitmap? GetBitmap()
        {
            ThrowIfClosed();
            return _bitmap;
        }

        /// <summary>
        /// Checks if the ImageBitmap has been closed.
        /// </summary>
        internal bool IsClosed => _isClosed;

        private void ThrowIfClosed()
        {
            if (_isClosed)
            {
                throw new InvalidOperationException("The ImageBitmap has been closed and can no longer be used.");
            }
            if (_isNeutered)
            {
                throw new InvalidOperationException("The ImageBitmap has been transferred and can no longer be used.");
            }
        }

        public void Dispose()
        {
            close();
        }

        // ITransferable implementation
        /// <summary>
        /// Checks if this ImageBitmap has been transferred to another context
        /// </summary>
        public bool IsNeutered => _isNeutered;

        /// <summary>
        /// Marks this ImageBitmap as transferred. After neutering, the ImageBitmap cannot be used.
        /// </summary>
        public void Neuter()
        {
            _isNeutered = true;
        }
    }
}
