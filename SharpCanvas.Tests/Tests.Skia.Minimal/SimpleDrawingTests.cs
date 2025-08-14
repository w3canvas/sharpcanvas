using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System;

namespace SharpCanvas.Tests.Skia.Minimal
{
    public class SimpleArcTests
    {
        [Test]
        public void TestSimpleArc()
        {
            var info = new SKImageInfo(100, 100, SKColorType.Rgba8888);
            using (var surface = SKSurface.Create(info))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                using (var paint = new SKPaint
                {
                    Color = SKColors.Blue,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 5,
                    IsAntialias = false
                })
                {
                    var rect = new SKRect(10, 10, 90, 90);
                    canvas.DrawArc(rect, 0, 90, false, paint);
                }

                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = System.IO.File.OpenWrite("test.png"))
                {
                    data.SaveTo(stream);
                }

                // Check if there are any blue pixels in the image
                var hasBlue = false;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (bitmap.GetPixel(x, y) == SKColors.Blue)
                        {
                            hasBlue = true;
                            break;
                        }
                    }
                    if (hasBlue)
                    {
                        break;
                    }
                }
                Assert.That(hasBlue, Is.True, "No blue pixels found in the image.");
            }
        }
    }
}
