using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaLineStylesTests
    {
        [Test]
        public void TestLineWidth()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);

                context.strokeStyle = "black";
                context.lineWidth = 10;
                context.beginPath();
                context.moveTo(20, 50);
                context.lineTo(80, 50);
                context.stroke();

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel in the middle of the line
                Assert.That(bitmap.GetPixel(50, 50), Is.EqualTo(SKColors.Black));
                // Check a pixel at the edge of the line
                Assert.That(bitmap.GetPixel(50, 45), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(50, 54), Is.EqualTo(SKColors.Black));
                // Check a pixel just outside the line
                Assert.That(bitmap.GetPixel(50, 44), Is.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(50, 55), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestLineCap()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);
                context.strokeStyle = "black";
                context.lineWidth = 10;

                // Butt cap
                context.lineCap = "butt";
                context.beginPath();
                context.moveTo(20, 25);
                context.lineTo(80, 25);
                context.stroke();

                // Round cap
                context.lineCap = "round";
                context.beginPath();
                context.moveTo(20, 50);
                context.lineTo(80, 50);
                context.stroke();

                // Square cap
                context.lineCap = "square";
                context.beginPath();
                context.moveTo(20, 75);
                context.lineTo(80, 75);
                context.stroke();

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Butt cap - should not extend beyond the path
                Assert.That(bitmap.GetPixel(19, 25), Is.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(81, 25), Is.EqualTo(SKColors.White));

                // Round cap - should have rounded ends
                Assert.That(bitmap.GetPixel(18, 50), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(82, 50), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(14, 50), Is.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(86, 50), Is.EqualTo(SKColors.White));


                // Square cap - should extend beyond the path
                Assert.That(bitmap.GetPixel(18, 75), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(82, 75), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(14, 75), Is.EqualTo(SKColors.White));
                Assert.That(bitmap.GetPixel(86, 75), Is.EqualTo(SKColors.White));
            }
        }

        [Test]
        public void TestLineJoin()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                surface.Canvas.Clear(SKColors.White);
                context.strokeStyle = "black";
                context.lineWidth = 10;

                // Miter join
                context.lineJoin = "miter";
                context.beginPath();
                context.moveTo(10, 10);
                context.lineTo(50, 50);
                context.lineTo(10, 90);
                context.stroke();

                // Bevel join
                context.lineJoin = "bevel";
                context.beginPath();
                context.moveTo(30, 10);
                context.lineTo(70, 50);
                context.lineTo(30, 90);
                context.stroke();

                // Round join
                context.lineJoin = "round";
                context.beginPath();
                context.moveTo(50, 10);
                context.lineTo(90, 50);
                context.lineTo(50, 90);
                context.stroke();

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Miter join - sharp corner
                Assert.That(bitmap.GetPixel(50, 50), Is.EqualTo(SKColors.Black));

                // Bevel join - flat corner
                Assert.That(bitmap.GetPixel(70, 50), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(65, 45), Is.EqualTo(SKColors.Black));

                // Round join - rounded corner
                Assert.That(bitmap.GetPixel(90, 50), Is.EqualTo(SKColors.Black));
                Assert.That(bitmap.GetPixel(85, 45), Is.EqualTo(SKColors.Black));
            }
        }
    }
}
