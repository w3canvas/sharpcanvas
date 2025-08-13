using NUnit.Framework;
using SharpCanvas.Shared;
using SharpCanvas.Context.Skia;
using SkiaSharp;

namespace SharpCanvas.Tests.Skia
{
    public class ApiUsageTests
    {
        [Test]
        [Ignore("putImageData is known to be broken")]
        public void TestPutImageData()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                var imageData = (ImageData)context.createImageData(50, 50);
                var data = (byte[])imageData.data;
                for (var i = 0; i < data.Length; i += 4)
                {
                    data[i] = 255;
                    data[i + 3] = 255;
                }
                context.putImageData(imageData, 25, 25);
                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
                Assert.That(bitmap.GetPixel(30, 30), Is.EqualTo(SKColors.Red));
            }
        }

        [Test]
        public void TestChangeSize()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var context = new CanvasRenderingContext2D(surface);
                context.fillStyle = "red";
                context.fillRect(0, 0, 50, 50);
                context.ChangeSize(200, 200, false);
                var bytes = context.GetBitmap();
                using (var bitmap = SKBitmap.Decode(bytes))
                {
                    Assert.That(bitmap.GetPixel(25, 25), Is.EqualTo(SKColors.Red));
                }
            }
        }
    }
}
