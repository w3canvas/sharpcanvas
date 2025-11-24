using System;
using SharpCanvas.Shared;
using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Graphics factory implementation using SkiaSharp rendering backend.
    /// This factory creates SkiaSharp-based rendering contexts for cross-platform support.
    /// </summary>
    public class SkiaGraphicsFactory : IGraphicsFactory
    {
        /// <summary>
        /// Creates a new SkiaSharp-based 2D rendering context.
        /// </summary>
        /// <param name="width">Width of the rendering context in pixels</param>
        /// <param name="height">Height of the rendering context in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new CanvasRenderingContext2D instance backed by SkiaSharp</returns>
        public ICanvasRenderingContext2D CreateContext(int width, int height, IDocument document)
        {
            var info = new SKImageInfo(width, height);
            var surface = SKSurface.Create(info);

            if (surface == null)
            {
                throw new InvalidOperationException($"Failed to create SKSurface with dimensions {width}x{height}");
            }

            return new CanvasRenderingContext2D(surface, document);
        }

        /// <summary>
        /// Creates a new offscreen canvas for background rendering using SkiaSharp.
        /// </summary>
        /// <param name="width">Width of the offscreen canvas in pixels</param>
        /// <param name="height">Height of the offscreen canvas in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new OffscreenCanvas instance</returns>
        public object CreateOffscreenCanvas(int width, int height, IDocument document)
        {
            return new OffscreenCanvas(width, height, document);
        }

        /// <summary>
        /// Creates an ImageBitmap from raw image data using SkiaSharp.
        /// </summary>
        /// <param name="data">Raw image data bytes</param>
        /// <returns>A new ImageBitmap instance</returns>
        public object CreateImageBitmap(byte[] data)
        {
            using var codec = SKCodec.Create(new SKMemoryStream(data));
            if (codec == null)
            {
                throw new ArgumentException("Invalid image data", nameof(data));
            }

            var info = new SKImageInfo(codec.Info.Width, codec.Info.Height);
            var bitmap = new SKBitmap(info);
            codec.GetPixels(info, bitmap.GetPixels());

            return new ImageBitmap(bitmap);
        }
    }
}
