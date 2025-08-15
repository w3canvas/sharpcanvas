using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaTextTests
    {
        [Test]
        public void TestFillText()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.font = "20px DejaVuSans";
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

        [Test]
        public void TestStrokeText()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);

                context.strokeStyle = "red";
                context.font = "20px DejaVuSans";
                context.strokeText("Hello", 20, 50);

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

        [Test]
        public void TestMeasureText()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                context.font = "20px DejaVuSans";

                var metrics = (TextMetrics)context.measureText("Hello");

                Assert.That(metrics.width, Is.GreaterThan(0));
                Assert.That(metrics.height, Is.GreaterThan(0));
            }
        }

        private bool IsColored(SKBitmap bitmap, SKRectI rect)
        {
            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    if (bitmap.GetPixel(x, y) != SKColors.White)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [Test]
        public void TestTextAlign()
        {
            var info = new SKImageInfo(200, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);
                context.fillStyle = "black";
                context.font = "20px DejaVuSans";

                context.textAlign = "left";
                context.fillText("left", 100, 20);

                context.textAlign = "center";
                context.fillText("center", 100, 50);

                context.textAlign = "right";
                context.fillText("right", 100, 80);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Left align: text should be to the right of 100
                Assert.That(IsColored(bitmap, SKRectI.Create(100, 10, 20, 20)), Is.True);
                Assert.That(IsColored(bitmap, SKRectI.Create(80, 10, 20, 20)), Is.False);

                // Center align: text should be centered around 100
                Assert.That(IsColored(bitmap, SKRectI.Create(90, 40, 20, 20)), Is.True);

                // Right align: text should be to the left of 100
                Assert.That(IsColored(bitmap, SKRectI.Create(80, 70, 20, 20)), Is.True);
                Assert.That(IsColored(bitmap, SKRectI.Create(100, 70, 20, 20)), Is.False);
            }
        }

        [Test]
        public void TestTextBaseline()
        {
            var info = new SKImageInfo(100, 200);
            using (var surface = SKSurface.Create(info))
            {
                var mockDocument = new Mock<IDocument>();
                mockDocument.Setup(d => d.fonts).Returns(new FontFaceSet());
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
                surface.Canvas.Clear(SKColors.White);
                context.fillStyle = "black";
                context.font = "20px DejaVuSans";

                context.textBaseLine = "top";
                context.fillText("Top", 10, 20);

                context.textBaseLine = "middle";
                context.fillText("Middle", 10, 80);

                context.textBaseLine = "bottom";
                context.fillText("Bottom", 10, 140);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Top baseline - text should be below y=20
                Assert.That(IsColored(bitmap, SKRectI.Create(10, 20, 20, 20)), Is.True);
                Assert.That(IsColored(bitmap, SKRectI.Create(10, 0, 20, 20)), Is.False);

                // Middle baseline - text should be centered around y=80
                Assert.That(IsColored(bitmap, SKRectI.Create(10, 70, 20, 20)), Is.True);

                // Bottom baseline - text should be above y=140
                Assert.That(IsColored(bitmap, SKRectI.Create(10, 120, 20, 20)), Is.True);
                Assert.That(IsColored(bitmap, SKRectI.Create(10, 140, 20, 20)), Is.False);
            }
        }
    }
}
