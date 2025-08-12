using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;

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
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);

                context.fillStyle = "blue";
                context.font = "20px sans-serif";
                context.fillText("Hello", 20, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel where the text should be. This is not a precise test,
                // but it's a good sanity check. A better test would involve
                // comparing against a known good image.
                Assert.That(bitmap.GetPixel(30, 40), Is.Not.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestStrokeText()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);

                context.strokeStyle = "red";
                context.font = "20px sans-serif";
                context.strokeText("Hello", 20, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                Assert.That(bitmap.GetPixel(30, 40), Is.Not.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestMeasureText()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                context.font = "20px sans-serif";

                var metrics = (TextMetrics)context.measureText("Hello");

                Assert.That(metrics.width, Is.GreaterThan(0));
                Assert.That(metrics.height, Is.GreaterThan(0));
            }
        }

        [Test]
        public void TestTextAlign()
        {
            var info = new SKImageInfo(200, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);
                context.fillStyle = "black";
                context.font = "20px sans-serif";

                context.textAlign = "left";
                context.fillText("left", 100, 20);

                context.textAlign = "center";
                context.fillText("center", 100, 50);

                context.textAlign = "right";
                context.fillText("right", 100, 80);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Left align
                Assert.That(bitmap.GetPixel(101, 15), Is.Not.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(99, 15), Is.EqualTo(SKColors.White));

                // Center align
                Assert.That(bitmap.GetPixel(100, 45), Is.Not.EqualTo(SKColors.White));

                // Right align
                Assert.That(bitmap.GetPixel(99, 75), Is.Not.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(101, 75), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestTextBaseline()
        {
            var info = new SKImageInfo(100, 200);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);
                context.fillStyle = "black";
                context.font = "20px sans-serif";

                context.textBaseLine = "top";
                context.fillText("Top", 10, 20);

                context.textBaseLine = "middle";
                context.fillText("Middle", 10, 80);

                context.textBaseLine = "bottom";
                context.fillText("Bottom", 10, 140);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Top baseline - text should be below y=20
                Assert.That(bitmap.GetPixel(15, 25), Is.Not.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(15, 15), Is.EqualTo(SKColors.White));

                // Middle baseline - text should be centered around y=80
                Assert.That(bitmap.GetPixel(15, 75), Is.Not.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(15, 85), Is.Not.EqualTo(SKColors.White));

                // Bottom baseline - text should be above y=140
                Assert.That(bitmap.GetPixel(15, 135), Is.Not.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(15, 145), Is.EqualTo(SKColors.White));
            }
        }
    }
}
