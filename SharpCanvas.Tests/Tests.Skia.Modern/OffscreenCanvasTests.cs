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
            SKBitmap resultBitmap = null;

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

            Assert.IsNotNull(resultBitmap);
            Assert.AreEqual(200, resultBitmap.Width);
            Assert.AreEqual(200, resultBitmap.Height);

            // Check a pixel to make sure it's red
            var pixel = resultBitmap.GetPixel(20, 20);
            Assert.AreEqual(255, pixel.Red);
            Assert.AreEqual(0, pixel.Green);
            Assert.AreEqual(0, pixel.Blue);
        }
    }
}
