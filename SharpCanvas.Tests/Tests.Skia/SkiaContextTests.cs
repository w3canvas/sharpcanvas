using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaContextTests
    {
        [Test]
        public void TestReadPixel()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.Clear(SKColors.Blue);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                Console.WriteLine($"Expected: {SKColors.Blue}, Actual: {bitmap.GetPixel(0, 0)}");
                Assert.That(bitmap.GetPixel(0, 0), Is.EqualTo(SKColors.Blue));
            }
        }

        [Test]
        public void TestfillRect()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.Clear(SKColors.Transparent);
                var context = new CanvasRenderingContext2D(surface);
                context.fillRect(10, 10, 50, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel inside the rectangle
                Console.WriteLine($"Inside - Expected: {SKColors.Black}, Actual: {bitmap.GetPixel(20, 20)}");
                Assert.That(bitmap.GetPixel(20, 20), Is.EqualTo(SKColors.Black));

                // Check a pixel outside the rectangle
                var transparent = new SKColor(0, 0, 0, 0);
                Console.WriteLine($"Outside - Expected: {transparent}, Actual: {bitmap.GetPixel(70, 70)}");
                Assert.That(bitmap.GetPixel(70, 70), Is.EqualTo(transparent));
            }
        }

        [Test]
        public void TestSaveRestore()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.Clear(SKColors.Transparent);
                var context = new CanvasRenderingContext2D(surface);
                var bitmap = new SKBitmap(info);

                // Initial color is black
                context.fillRect(0, 0, 1, 1);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
                Console.WriteLine($"Initial - Expected: {SKColors.Black}, Actual: {bitmap.GetPixel(0, 0)}");
                Assert.That(bitmap.GetPixel(0, 0), Is.EqualTo(SKColors.Black));

                context.save();

                // Change color to red
                context.fillStyle = "#FF0000";
                context.fillRect(0, 0, 1, 1);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
                Console.WriteLine($"Red - Expected: {SKColors.Red}, Actual: {bitmap.GetPixel(0, 0)}");
                Assert.That(bitmap.GetPixel(0, 0), Is.EqualTo(SKColors.Red));

                context.restore();

                // Color should be black again
                context.fillRect(0, 0, 1, 1);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
                Console.WriteLine($"Restored - Expected: {SKColors.Black}, Actual: {bitmap.GetPixel(0, 0)}");
                Assert.That(bitmap.GetPixel(0, 0), Is.EqualTo(SKColors.Black));
            }
        }

        [Test]
        public void TestGlobalAlpha()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                context.fillStyle = "#FF0000";
                context.globalAlpha = 0.5;
                context.fillRect(0, 0, 100, 100);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                var pixel = bitmap.GetPixel(50, 50);
                Assert.That(pixel.Alpha, Is.EqualTo(127).Within(1));
            }
        }

        [Test]
        public void TestGlobalCompositeOperation()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                context.fillStyle = "#FF0000";
                context.fillRect(0, 0, 50, 50);

                context.globalCompositeOperation = "source-in";
                context.fillStyle = "#0000FF";
                context.fillRect(25, 25, 50, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                var pixel = bitmap.GetPixel(40, 40);
                Assert.That(pixel, Is.EqualTo(SKColors.Blue));
            }
        }

        [Test]
        public void TestClearRect()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);

                // Fill the canvas with blue
                context.fillStyle = "blue";
                context.fillRect(0, 0, 100, 100);

                // Clear a rectangle
                context.clearRect(10, 10, 50, 50);

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel inside the cleared rectangle
                var transparent = new SKColor(0, 0, 0, 0);
                Assert.That(bitmap.GetPixel(20, 20), Is.EqualTo(transparent));

                // Check a pixel outside the cleared rectangle
                Assert.That(bitmap.GetPixel(70, 70), Is.EqualTo(SKColors.Blue));
            }
        }
    }
}
