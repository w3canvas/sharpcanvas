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
            public object body { get; set; } = null!;
            public string title { get; set; } = "";
            public ILocation? location { get; set; } = null;
            public IWindow defaultView { get; }

            public SimpleDocument()
            {
                defaultView = new SimpleWindow(this);
            }

            public object createElement(string tagName) => throw new NotImplementedException();
            public object createElementNS(string ns, string tagName) => throw new NotImplementedException();
        }

        private class SimpleWindow : IWindow
        {
            public IDocument document { get; }
            public FontFaceSet fonts { get; } = new FontFaceSet();

            public SimpleWindow(IDocument document)
            {
                this.document = document;
            }

            #region Not Implemented
            public object? parentNode => throw new NotImplementedException();
            public object childNodes => throw new NotImplementedException();
            public void appendChild(object child) => throw new NotImplementedException();
            public void removeChild(object child) => throw new NotImplementedException();
            public object? ownerDocument => throw new NotImplementedException();
            public ILocation? location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IWindow self => this;
            public IWindow window => this;
            public IDocument parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public object onload { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int innerHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int innerWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string src { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public object top { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public object frameElement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IWindow? parentWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int Left { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public int Top { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public void setAttribute(object name, object value) => throw new NotImplementedException();
            public int setTimeout(object func, object milliseconds) => throw new NotImplementedException();
            public void clearTimeout(int key) => throw new NotImplementedException();
            public int setInterval(object func, object milliseconds) => throw new NotImplementedException();
            public void clearInterval(int key) => throw new NotImplementedException();
            public INavigator navigator => throw new NotImplementedException();
            public void RedrawChildren() => throw new NotImplementedException();
            public IEventModel eventModel => throw new NotImplementedException();
            #endregion
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
            System.Runtime.InteropServices.GCHandle GCH = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            pixmap.ReadPixels(pixmap.Info, GCH.AddrOfPinnedObject(), pixmap.RowBytes, 0, 0);
            GCH.Free();
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
