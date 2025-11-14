using SharpCanvas;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using System;
using System.IO;

namespace SharpCanvas.Tests.Unified
{
    /// <summary>
    /// Context provider for SkiaSharp-based rendering context.
    /// </summary>
    public class SkiaContextProvider : ICanvasContextProvider
    {
        private class SimpleDocument : IDocument
        {
            public string GetLocation() => "";
            public object CreateElement(string tagName) => null!;
            public object QuerySelector(string selector) => null!;
            public void LoadFont(string url, Action<bool> callback) => callback(true);
        }

        private readonly Dictionary<ICanvasRenderingContext2D, SKSurface> _surfaces = new();

        public string Name => "SkiaSharp";

        public ICanvasRenderingContext2D CreateContext(int width, int height)
        {
            var info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var surface = SKSurface.Create(info);
            var context = new CanvasRenderingContext2D(surface, new SimpleDocument());
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
            pixmap.ReadPixels(pixmap.Info, bytes, pixmap.RowBytes, 0, 0);
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
