using SharpCanvas;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Abstraction layer for creating canvas contexts from different backends.
    /// This allows tests to run against both modern Skia and legacy System.Drawing contexts.
    /// </summary>
    public interface ICanvasContextProvider
    {
        /// <summary>
        /// Gets a descriptive name for this context provider (e.g., "SkiaSharp", "System.Drawing")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a new rendering context with the specified dimensions.
        /// </summary>
        ICanvasRenderingContext2D CreateContext(int width, int height);

        /// <summary>
        /// Gets the pixel data from the canvas as RGBA bytes.
        /// </summary>
        byte[] GetPixelData(ICanvasRenderingContext2D context);

        /// <summary>
        /// Gets the color of a specific pixel as RGBA (0-255 range).
        /// </summary>
        (byte r, byte g, byte b, byte a) GetPixel(ICanvasRenderingContext2D context, int x, int y);

        /// <summary>
        /// Saves the canvas to a PNG file for visual debugging.
        /// </summary>
        void SaveToPng(ICanvasRenderingContext2D context, string filePath);

        /// <summary>
        /// Disposes the context and releases resources.
        /// </summary>
        void DisposeContext(ICanvasRenderingContext2D context);
    }
}
