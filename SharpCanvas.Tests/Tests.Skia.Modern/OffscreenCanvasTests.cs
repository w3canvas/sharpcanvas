using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System.Threading;

namespace SharpCanvas.Tests.Skia.Modern
{
    public class OffscreenCanvasTests
    {
        [Test]
        public void TestOffscreenCanvasWithWorker()
        {
            var worker = new CanvasWorker();
            var manualResetEvent = new ManualResetEvent(false);
            SKBitmap? resultBitmap = null;

            worker.OnWorkComplete += (sender, bitmap) =>
            {
                resultBitmap = bitmap;
                manualResetEvent.Set();
            };

            worker.Run((canvas) =>
            {
                var context = canvas.getContext("2d");
                context.fillStyle = "red";
                context.fillRect(10, 10, 100, 100);
            }, 200, 200);

            manualResetEvent.WaitOne();

            Assert.That(resultBitmap, Is.Not.Null);
            Assert.That(resultBitmap.Width, Is.EqualTo(200));
            Assert.That(resultBitmap.Height, Is.EqualTo(200));

            // Check a pixel to make sure it's red
            var pixel = resultBitmap.GetPixel(20, 20);
            Assert.That(pixel.Red, Is.EqualTo(255));
            Assert.That(pixel.Green, Is.EqualTo(0));
            Assert.That(pixel.Blue, Is.EqualTo(0));
        }
    }
}
