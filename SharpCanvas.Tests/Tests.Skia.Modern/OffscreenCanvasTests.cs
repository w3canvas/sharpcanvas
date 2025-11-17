using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System.Threading;
using Moq;
using SharpCanvas.Shared;
namespace SharpCanvas.Tests.Skia.Modern
{
    public class OffscreenCanvasTests
    {
        [Test]
        public void TestOffscreenCanvasWithWorker()
        {
            var worker = new CanvasWorker();
            var manualResetEvent = new ManualResetEvent(false);
            ImageBitmap? resultImageBitmap = null;

            worker.OnWorkComplete += (sender, imageBitmap) =>
            {
                resultImageBitmap = imageBitmap;
                manualResetEvent.Set();
            };
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
            worker.Run((canvas) =>
            {
                var context = canvas.getContext("2d");
                context.fillStyle = "red";
                context.fillRect(10, 10, 100, 100);
            }, 200, 200, mockDocument.Object);

            manualResetEvent.WaitOne();

            Assert.That(resultImageBitmap, Is.Not.Null);
            Assert.That(resultImageBitmap.width, Is.EqualTo(200));
            Assert.That(resultImageBitmap.height, Is.EqualTo(200));

            // Check a pixel to make sure it's red
            var bitmap = resultImageBitmap.GetBitmap();
            Assert.That(bitmap, Is.Not.Null);
            var pixel = bitmap.GetPixel(20, 20);
            Assert.That(pixel.Red, Is.EqualTo(255));
            Assert.That(pixel.Green, Is.EqualTo(0));
            Assert.That(pixel.Blue, Is.EqualTo(0));

            // Clean up
            resultImageBitmap.close();
        }
    }
}
