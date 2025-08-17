using NUnit.Framework;
using SkiaSharp;

namespace SharpCanvas.Tests.Skia.Minimal
{
    public class ArcApiUsageTests
    {
        [Test]
        public void TestFilledArcFromPath()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);

                // Create a path for a semi-circle
                using (var path = new SKPath())
                {
                    var rect = new SKRect(25, 25, 75, 75); // Bounding box for a circle at (50,50) with radius 25
                    path.AddArc(rect, 0, 180); // Bottom semi-circle
                    path.Close(); // Close the path to form a "D" shape

                    // Create a paint to fill the path
                    using (var paint = new SKPaint
                    {
                        Color = SKColors.Black,
                        Style = SKPaintStyle.Fill,
                        IsAntialias = true
                    })
                    {
                        canvas.DrawPath(path, paint);
                    }
                }

                // Read the pixels to check the result
                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel that should be inside the filled semi-circle
                var pixel = bitmap.GetPixel(50, 62);
                Assert.That(pixel, Is.EqualTo(SKColors.Black), "The pixel at (50, 62) should be black.");
            }
        }
    }
}
