using NUnit.Framework;
using SkiaSharp;
using System.IO;

namespace SharpCanvas.Tests.Skia.Standalone
{
    public class StandaloneFontTests
    {
        [Test]
        public void ShouldRenderTextWithFontFromFile()
        {
            Assert.Ignore("This test is for diagnostic purposes and will fail without the 'fontconfig' native dependency installed in the environment. It is skipped by default.");

            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                string fontPath = "DejaVuSans.ttf";
                Assert.That(File.Exists(fontPath), $"Font file not found at {Path.GetFullPath(fontPath)}");

                using (var typeface = SKTypeface.FromFile(fontPath))
                using (var paint = new SKPaint())
                {
                    paint.Typeface = typeface;
                    paint.TextSize = 20;
                    paint.Color = SKColors.Black;
                    canvas.DrawText("Hello", 10, 50, paint);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                Assert.That(bitmap.GetPixel(15, 45), Is.Not.EqualTo(SKColors.White), "The pixel should have been drawn on, but this will fail if 'fontconfig' is missing.");
            }
        }
    }
}
