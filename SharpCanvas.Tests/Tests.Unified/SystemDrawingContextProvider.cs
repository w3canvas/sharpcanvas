#if WINDOWS
using SharpCanvas;
using SharpCanvas.Context.Drawing2D;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Context provider for System.Drawing-based rendering context (Windows only).
    /// </summary>
    public class SystemDrawingContextProvider : ICanvasContextProvider
    {
        public string Name => "System.Drawing";

        public ICanvasRenderingContext2D CreateContext(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var graphics = Graphics.FromImage(bitmap);
            
            // Initialize with default settings similar to Skia context
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            
            var stroke = new Pen(Color.Black);
            var fill = new SharpCanvas.Context.Drawing2D.Fill(Color.Black);
            
            // Note: The constructor signature might need adjustment based on the actual class definition
            // I'm using the one I saw in the file view: 
            // CanvasRenderingContext2D(Graphics s, Bitmap bitmap, Pen stroke, IFill fill, bool visible)
            return new CanvasRenderingContext2D(graphics, bitmap, stroke, fill, true);
        }

        public byte[] GetPixelData(ICanvasRenderingContext2D context)
        {
            var ctx = (CanvasRenderingContext2D)context;
            var bitmap = (Bitmap)ctx.GetType().GetField("_surfaceBitmap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ctx);
            
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bytes = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            bitmap.UnlockBits(data);
            
            return bytes;
        }

        public (byte r, byte g, byte b, byte a) GetPixel(ICanvasRenderingContext2D context, int x, int y)
        {
            var ctx = (CanvasRenderingContext2D)context;
            // Accessing private field _surfaceBitmap via reflection since it's not exposed
            var bitmap = (Bitmap)ctx.GetType().GetField("_surfaceBitmap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ctx);
            
            var color = bitmap.GetPixel(x, y);
            return (color.R, color.G, color.B, color.A);
        }

        public void SaveToPng(ICanvasRenderingContext2D context, string filePath)
        {
            var ctx = (CanvasRenderingContext2D)context;
            var bitmap = (Bitmap)ctx.GetType().GetField("_surfaceBitmap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ctx);
            
            bitmap.Save(filePath, ImageFormat.Png);
        }

        public void DisposeContext(ICanvasRenderingContext2D context)
        {
            var ctx = (CanvasRenderingContext2D)context;
            ctx.Surface?.Dispose();
            // Bitmap disposal might be handled by the context or needs to be done here
            // The context doesn't seem to implement IDisposable, so we might need to be careful
            // For now, we'll just leave it as the context seems to own the graphics object
        }
    }
}
#endif
