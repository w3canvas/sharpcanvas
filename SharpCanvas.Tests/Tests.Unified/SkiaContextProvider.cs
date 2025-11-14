using SharpCanvas;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using System;
using System.IO;
using Moq;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Context provider for SkiaSharp-based rendering context.
    /// </summary>
    public class SkiaContextProvider : ICanvasContextProvider
    {
        private readonly Dictionary<ICanvasRenderingContext2D, SKSurface> _surfaces = new();

        public string Name => "SkiaSharp";

        public ICanvasRenderingContext2D CreateContext(int width, int height)
        {
            // Create mock window and document following the pattern from SimpleContextTests.cs
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var surface = SKSurface.Create(info);
            var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
            _surfaces[context] = surface;
            return context;
        }

        public byte[] GetPixelData(ICanvasRenderingContext2D context)
        {
            if (!_surfaces.TryGetValue(context, out var surface))
                throw new InvalidOperationException("Context not found");

            using var image = surface.Snapshot();
            using var pixmap = image.PeekPixels();

            var bytes = new byte[pixmap.BytesSize];
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    pixmap.ReadPixels(pixmap.Info, (IntPtr)ptr, pixmap.RowBytes, 0, 0);
                }
            }
            return bytes;
        }

        public (byte r, byte g, byte b, byte a) GetPixel(ICanvasRenderingContext2D context, int x, int y)
        {
            if (!_surfaces.TryGetValue(context, out var surface))
                throw new InvalidOperationException("Context not found");

            var bitmap = new SKBitmap(surface.Canvas.DeviceClipBounds.Width, surface.Canvas.DeviceClipBounds.Height);
            surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var color = bitmap.GetPixel(x, y);
            bitmap.Dispose();

            return (color.Red, color.Green, color.Blue, color.Alpha);
        }

        public void SaveToPng(ICanvasRenderingContext2D context, string filePath)
        {
            if (!_surfaces.TryGetValue(context, out var surface))
                throw new InvalidOperationException("Context not found");

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(filePath);
            data.SaveTo(stream);
        }

        public void DisposeContext(ICanvasRenderingContext2D context)
        {
            if (_surfaces.TryGetValue(context, out var surface))
            {
                surface.Dispose();
                _surfaces.Remove(context);
            }
        }
    }
}
