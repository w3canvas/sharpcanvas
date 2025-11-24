using System.Drawing;
using System.Drawing.Drawing2D;
using SharpCanvas.Shared;

namespace SharpCanvas.Context.Drawing2D
{
    /// <summary>
    /// Graphics factory implementation using System.Drawing (GDI+) rendering backend.
    /// This factory creates GDI+-based rendering contexts for Windows-native support.
    /// </summary>
    public class GdiGraphicsFactory : IGraphicsFactory
    {
        /// <summary>
        /// Creates a new GDI+-based 2D rendering context.
        /// </summary>
        /// <param name="width">Width of the rendering context in pixels</param>
        /// <param name="height">Height of the rendering context in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new CanvasRenderingContext2D instance backed by System.Drawing</returns>
        public ICanvasRenderingContext2D CreateContext(int width, int height, IDocument document)
        {
            // Create bitmap and graphics surface
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            // Set high quality rendering
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            // Create default stroke and fill
            var defaultStroke = new Pen(Color.Black, 1.0f);
            var defaultFill = new Fill(Color.Black);

            return new CanvasRenderingContext2D(graphics, bitmap, defaultStroke, defaultFill, true);
        }

        /// <summary>
        /// Creates a new offscreen canvas for background rendering using GDI+.
        /// </summary>
        /// <param name="width">Width of the offscreen canvas in pixels</param>
        /// <param name="height">Height of the offscreen canvas in pixels</param>
        /// <param name="document">Document implementation for context interaction</param>
        /// <returns>A new offscreen canvas instance</returns>
        /// <remarks>
        /// Note: OffscreenCanvas implementation for System.Drawing may need to be created.
        /// For now, this creates a standard context that can be used for offscreen rendering.
        /// </remarks>
        public object CreateOffscreenCanvas(int width, int height, IDocument document)
        {
            // Create a standard bitmap-based context for offscreen rendering
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            // Set high quality rendering
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var defaultStroke = new Pen(Color.Black, 1.0f);
            var defaultFill = new Fill(Color.Black);

            return new CanvasRenderingContext2D(graphics, bitmap, defaultStroke, defaultFill, false);
        }

        /// <summary>
        /// Creates an ImageBitmap from raw image data using GDI+.
        /// </summary>
        /// <param name="data">Raw image data bytes</param>
        /// <returns>A new Bitmap instance</returns>
        public object CreateImageBitmap(byte[] data)
        {
            using var stream = new System.IO.MemoryStream(data);
            return new Bitmap(stream);
        }
    }
}
