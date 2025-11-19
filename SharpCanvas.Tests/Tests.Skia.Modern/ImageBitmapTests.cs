using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System;
using System.Threading.Tasks;
using Moq;
using SharpCanvas.Shared;
using Microsoft.ClearScript.V8;

namespace SharpCanvas.Tests.Skia.Modern
{
    [TestFixture]
    public class ImageBitmapTests
    {
        [Test]
        public void TestCreateImageBitmapFromSKBitmap()
        {
            var skBitmap = new SKBitmap(100, 100);
            var imageBitmap = ImageBitmapFactory.createImageBitmap(skBitmap);

            Assert.That(imageBitmap, Is.Not.Null);
            Assert.That(imageBitmap.width, Is.EqualTo(100));
            Assert.That(imageBitmap.height, Is.EqualTo(100));
            Assert.That(imageBitmap.IsClosed, Is.False);

            imageBitmap.close();
            Assert.That(imageBitmap.IsClosed, Is.True);
        }

        [Test]
        public void TestCreateImageBitmapWithResize()
        {
            var skBitmap = new SKBitmap(200, 200);
            var options = new ImageBitmapOptions
            {
                resizeWidth = 100,
                resizeHeight = 100,
                resizeQuality = "high"
            };

            var imageBitmap = ImageBitmapFactory.createImageBitmap(skBitmap, options);

            Assert.That(imageBitmap.width, Is.EqualTo(100));
            Assert.That(imageBitmap.height, Is.EqualTo(100));

            imageBitmap.close();
        }

        [Test]
        public void TestCreateImageBitmapWithCrop()
        {
            var skBitmap = new SKBitmap(200, 200);
            using (var canvas = new SKCanvas(skBitmap))
            {
                canvas.Clear(SKColors.Red);
            }

            var imageBitmap = ImageBitmapFactory.createImageBitmap(skBitmap, 50, 50, 100, 100);

            Assert.That(imageBitmap.width, Is.EqualTo(100));
            Assert.That(imageBitmap.height, Is.EqualTo(100));

            imageBitmap.close();
        }

        [Test]
        public void TestImageBitmapClose()
        {
            var skBitmap = new SKBitmap(100, 100);
            var imageBitmap = new ImageBitmap(skBitmap);

            imageBitmap.close();

            Assert.Throws<InvalidOperationException>(() => { var w = imageBitmap.width; });
            Assert.Throws<InvalidOperationException>(() => { var h = imageBitmap.height; });
        }

        [Test]
        public void TestImageBitmapTransferable()
        {
            var skBitmap = new SKBitmap(100, 100);
            var imageBitmap = new ImageBitmap(skBitmap);

            Assert.That(imageBitmap.IsNeutered, Is.False);

            imageBitmap.Neuter();

            Assert.That(imageBitmap.IsNeutered, Is.True);
            Assert.Throws<InvalidOperationException>(() => { var w = imageBitmap.width; });
        }

        [Test]
        public void TestOffscreenCanvasTransferToImageBitmap()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var canvas = new OffscreenCanvas(200, 200, mockDocument.Object);
            var context = canvas.getContext("2d");
            context.fillStyle = "blue";
            context.fillRect(0, 0, 200, 200);

            var imageBitmap = canvas.transferToImageBitmap();

            Assert.That(imageBitmap, Is.Not.Null);
            Assert.That(imageBitmap.width, Is.EqualTo(200));
            Assert.That(imageBitmap.height, Is.EqualTo(200));

            var bitmap = imageBitmap.GetBitmap();
            Assert.That(bitmap, Is.Not.Null);
            var pixel = bitmap.GetPixel(100, 100);
            Assert.That(pixel.Blue, Is.EqualTo(255));

            imageBitmap.close();
        }

        [Test]
        public async Task TestOffscreenCanvasConvertToBlob()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var canvas = new OffscreenCanvas(100, 100, mockDocument.Object);
            var context = canvas.getContext("2d");
            context.fillStyle = "green";
            context.fillRect(0, 0, 100, 100);

            var blob = await canvas.convertToBlob();

            Assert.That(blob, Is.Not.Null);
            Assert.That(blob.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task TestOffscreenCanvasConvertToBlobJpeg()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var canvas = new OffscreenCanvas(100, 100, mockDocument.Object);
            var context = canvas.getContext("2d");
            context.fillStyle = "yellow";
            context.fillRect(0, 0, 100, 100);

            var blob = await canvas.convertToBlob("image/jpeg", 0.9);

            Assert.That(blob, Is.Not.Null);
            Assert.That(blob.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TestDrawImageWithImageBitmap()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            // Create source canvas and draw a red rectangle
            var sourceCanvas = new OffscreenCanvas(50, 50, mockDocument.Object);
            var sourceContext = sourceCanvas.getContext("2d");
            sourceContext.fillStyle = "red";
            sourceContext.fillRect(0, 0, 50, 50);

            // Get an ImageBitmap from the source canvas
            var imageBitmap = sourceCanvas.transferToImageBitmap();

            // Diagnostic check: verify the source bitmap itself is correct
            var sourceSkBitmap = imageBitmap.GetBitmap();
            Assert.That(sourceSkBitmap, Is.Not.Null, "Source SKBitmap should not be null");
            var sourcePixel = sourceSkBitmap!.GetPixel(25, 25);
            Assert.That(sourcePixel.Red, Is.EqualTo(255), "Source pixel should be red");

            // Create destination canvas
            var destCanvas = new OffscreenCanvas(100, 100, mockDocument.Object);
            var context = destCanvas.getContext("2d");

            // Draw the ImageBitmap
            context.drawImage(imageBitmap, 25, 25);

            // Verify the pixel at the center of the drawn image
            var resultBitmap = destCanvas.transferToImageBitmap();
            var skBitmap = resultBitmap.GetBitmap();
            Assert.That(skBitmap, Is.Not.Null);
            var pixel = skBitmap!.GetPixel(50, 50);

            Assert.That(pixel.Red, Is.EqualTo(255));

            imageBitmap.close();
            resultBitmap.close();
        }

        [Test]
        public void TestOffscreenCanvasTransferable()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var canvas = new OffscreenCanvas(100, 100, mockDocument.Object);

            Assert.That(canvas.IsNeutered, Is.False);

            canvas.Neuter();

            Assert.That(canvas.IsNeutered, Is.True);
        }

        [Test]
        public void TestOffscreenCanvasWidthHeight()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var canvas = new OffscreenCanvas(100, 100, mockDocument.Object);

            Assert.That(canvas.width, Is.EqualTo(100));
            Assert.That(canvas.height, Is.EqualTo(100));

            canvas.width = 200;
            canvas.height = 150;

            Assert.That(canvas.width, Is.EqualTo(200));
            Assert.That(canvas.height, Is.EqualTo(150));
        }

        [Test]
        public void TestJsHostDrawing()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            using (var engine = new V8ScriptEngine())
            {
                var canvas = new OffscreenCanvas(100, 100, mockDocument.Object);
                engine.AddHostObject("canvas", canvas);

                engine.Execute(@"
                    var ctx = canvas.getContext('2d');
                    ctx.fillStyle = 'blue';
                    ctx.fillRect(0, 0, 100, 100);
                ");

                var imageBitmap = canvas.transferToImageBitmap();
                var bitmap = imageBitmap.GetBitmap();
                Assert.That(bitmap, Is.Not.Null);
                var pixel = bitmap.GetPixel(50, 50);
                Assert.That(pixel.Blue, Is.EqualTo(255));
            }
        }
    }
}
