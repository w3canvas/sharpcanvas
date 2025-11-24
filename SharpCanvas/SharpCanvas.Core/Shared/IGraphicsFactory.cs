using SharpCanvas.Shared;

namespace SharpCanvas
{
    /// <summary>
    /// Factory interface for creating graphics contexts and related objects.
    /// This abstraction allows different rendering backends (SkiaSharp, System.Drawing, etc.)
    /// to be used interchangeably with the runtime layer (Workers, Event Loop).
    /// </summary>
    /// <remarks>
    /// Implementations:
    /// - SkiaGraphicsFactory (Context.Skia) - Cross-platform using SkiaSharp
    /// - GdiGraphicsFactory (Context.Drawing2D) - Windows-only using System.Drawing
    ///
    /// This interface enables:
    /// - Backend-agnostic Worker implementations
    /// - Shared runtime logic across all backends
    /// - Easy addition of new rendering backends
    /// - Testability through mocking
    /// </remarks>
    public interface IGraphicsFactory
    {
        /// <summary>
        /// Creates a new 2D rendering context with the specified dimensions.
        /// </summary>
        /// <param name="width">Width of the rendering context in pixels</param>
        /// <param name="height">Height of the rendering context in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new ICanvasRenderingContext2D instance</returns>
        ICanvasRenderingContext2D CreateContext(int width, int height, IDocument document);

        /// <summary>
        /// Creates a new offscreen canvas for background rendering.
        /// </summary>
        /// <param name="width">Width of the offscreen canvas in pixels</param>
        /// <param name="height">Height of the offscreen canvas in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new offscreen canvas instance</returns>
        object CreateOffscreenCanvas(int width, int height, IDocument document);

        /// <summary>
        /// Creates an ImageBitmap from raw image data.
        /// </summary>
        /// <param name="data">Raw image data bytes</param>
        /// <returns>A new ImageBitmap instance</returns>
        object CreateImageBitmap(byte[] data);
    }
}
