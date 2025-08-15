using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using System.IO;
using System.Threading.Tasks;
using Moq;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaFontFaceTests
    {
        [Test]
        public async Task TestLoadFontFromByteArray()
        {
            var fontBytes = File.ReadAllBytes("Fonts/DejaVuSans.ttf");
            var fontFace = new FontFace("MyTestFont", fontBytes, new FontFaceDescriptors());

            await fontFace.load();

            Assert.That(fontFace.status, Is.EqualTo("loaded"));

            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                var mockFonts = new FontFaceSet();
                mockDocument.Setup(d => d.fonts).Returns(mockFonts);
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                ((FontFaceSet)context.fonts).add(fontFace);
                context.font = "20px MyTestFont";
                context.fillText("Hello", 20, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                bool foundPixel = false;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (bitmap.GetPixel(x, y) != SKColors.White)
                        {
                            foundPixel = true;
                            break;
                        }
                    }
                    if (foundPixel) break;
                }
                Assert.That(foundPixel, Is.True, "Expected to find a non-white pixel, but the canvas was empty.");
            }
        }
    }
}
