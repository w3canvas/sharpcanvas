using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaShadowTests
    {
        [Test]
        public void TestBasicShadow()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.shadowColor = "black";
                context.shadowOffsetX = 5;
                context.shadowOffsetY = 5;
                context.shadowBlur = 0; // No blur for easier pixel checking

                context.fillRect(20, 20, 30, 30);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // The rect is at (20, 20) with size (30, 30), so it covers from (20, 20) to (49, 49).
                // The shadow is offset by (5, 5), so it covers from (25, 25) to (54, 54).

                // Check a pixel that should only be shadow (e.g., bottom-right corner of shadow)
                Assert.That(bitmap.GetPixel(52, 52), Is.EqualTo(SKColors.Black));

                // Check a pixel that should only be the shape (e.g., top-left corner of shape)
                Assert.That(bitmap.GetPixel(22, 22), Is.EqualTo(SKColors.Blue));

                // Check a pixel in the overlapping area. Shape should be on top.
                Assert.That(bitmap.GetPixel(30, 30), Is.EqualTo(SKColors.Blue));

                // Check a pixel outside both
                Assert.That(bitmap.GetPixel(10, 10), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestShadowWithBlur()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.shadowColor = "black";
                context.shadowOffsetX = 5;
                context.shadowOffsetY = 5;
                context.shadowBlur = 5;

                context.fillRect(20, 20, 30, 30);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel in the blurred shadow area. It should not be pure black or pure white.
                var pixel = bitmap.GetPixel(55, 55);
                Assert.That(pixel.Red, Is.LessThan(255));
                Assert.That(pixel.Green, Is.LessThan(255));
                Assert.That(pixel.Blue, Is.LessThan(255));
                Assert.That(pixel.Alpha, Is.EqualTo(255)); // The background is white, so alpha should be 255.

                // Check a pixel in the original shape
                Assert.That(bitmap.GetPixel(30, 30), Is.EqualTo(SKColors.Blue));
            }
        }

        [Test]
        public void TestNoShadow()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.fillRect(20, 20, 30, 30);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel where the shadow would have been
                Assert.That(bitmap.GetPixel(52, 52), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestDisabledShadow()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.shadowColor = "black";
                context.shadowOffsetX = 5;
                context.shadowOffsetY = 5;
                context.shadowBlur = 5;

                // Disable the shadow
                context.shadowColor = "transparent";

                context.fillRect(20, 20, 30, 30);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel where the shadow would have been
                Assert.That(bitmap.GetPixel(52, 52), Is.EqualTo(SKColors.White));
            }
        }
    }
}
