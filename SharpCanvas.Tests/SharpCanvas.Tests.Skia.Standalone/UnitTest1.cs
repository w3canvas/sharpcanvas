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
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                string fontPath = "DejaVuSans.ttf";
                Assert.That(File.Exists(fontPath), $"Font file not found at {Path.GetFullPath(fontPath)}");

                using (var stream = new FileStream(fontPath, FileMode.Open))
                using (var typeface = SKTypeface.FromStream(stream))
                using (var paint = new SKPaint())
                using (var font = new SKFont(typeface, 20))
                {
                    paint.Color = SKColors.Black;
                    canvas.DrawText("Hello", 10, 50, font, paint);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                Assert.That(bitmap.GetPixel(12, 35), Is.Not.EqualTo(SKColors.White));
            }
        }
    }
}
