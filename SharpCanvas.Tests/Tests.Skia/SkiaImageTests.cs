using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaImageTests
    {
        [Test]
        public void TestDrawImage()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                // Create a source bitmap
                using (var sourceBitmap = new SKBitmap(20, 20))
                {
                    sourceBitmap.Erase(SKColors.Green);
                    context.drawImage(sourceBitmap, 10, 10);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel where the image was drawn
                Assert.That(bitmap.GetPixel(15, 15), Is.EqualTo(SKColors.Green));
                // Check a pixel outside the image
                Assert.That(bitmap.GetPixel(40, 40), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestDrawImageWithScaling()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                using (var sourceBitmap = new SKBitmap(20, 20))
                {
                    sourceBitmap.Erase(SKColors.Green);
                    context.drawImage(sourceBitmap, 10, 10, 40, 40);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // The 20x20 image is scaled to 40x40.
                Assert.That(bitmap.GetPixel(30, 30), Is.EqualTo(SKColors.Green));
                Assert.That(bitmap.GetPixel(55, 55), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestDrawImageWithSlicing()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                using (var sourceBitmap = new SKBitmap(20, 20))
                {
                    // Fill the top half with green and the bottom half with blue.
                    sourceBitmap.Erase(SKColors.Green);
                    using (var canvas = new SKCanvas(sourceBitmap))
                    {
                        using (var paint = new SKPaint { Color = SKColors.Blue })
                        {
                            canvas.DrawRect(new SKRect(0, 10, 20, 20), paint);
                        }
                    }

                    // Draw only the top half (green part)
                    context.drawImage(sourceBitmap, 0, 0, 20, 10, 10, 10, 20, 10);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check that the green part is drawn
                Assert.That(bitmap.GetPixel(15, 15), Is.EqualTo(SKColors.Green));
                // Check that the blue part is not drawn
                Assert.That(bitmap.GetPixel(15, 25), Is.EqualTo(SKColors.White));
            }
        }
    }
}
