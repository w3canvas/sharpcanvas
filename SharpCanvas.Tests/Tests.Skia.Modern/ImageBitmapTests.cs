using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using System;
using System.Threading.Tasks;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia.Modern
{
    [TestFixture]
    public class ImageBitmapTests
    {
        [Test]
        public async Task TestCreateImageBitmapFromSKBitmap()
        {
            var skBitmap = new SKBitmap(100, 100);
            var imageBitmap = await ImageBitmapFactory.createImageBitmap(skBitmap);

            Assert.That(imageBitmap, Is.Not.Null);
            Assert.That(imageBitmap.width, Is.EqualTo(100));
            Assert.That(imageBitmap.height, Is.EqualTo(100));
            Assert.That(imageBitmap.IsClosed, Is.False);

            imageBitmap.close();
            Assert.That(imageBitmap.IsClosed, Is.True);
        }

        [Test]
        public async Task TestCreateImageBitmapWithResize()
        {
            var skBitmap = new SKBitmap(200, 200);
            var options = new ImageBitmapOptions
            {
                resizeWidth = 100,
                resizeHeight = 100,
                resizeQuality = "high"
            };

            var imageBitmap = await ImageBitmapFactory.createImageBitmap(skBitmap, options);

            Assert.That(imageBitmap.width, Is.EqualTo(100));
            Assert.That(imageBitmap.height, Is.EqualTo(100));

            imageBitmap.close();
        }

        [Test]
        public async Task TestCreateImageBitmapWithCrop()
        {
            var skBitmap = new SKBitmap(200, 200);
            using (var canvas = new SKCanvas(skBitmap))
            {
                canvas.Clear(SKColors.Red);
            }

            var imageBitmap = await ImageBitmapFactory.createImageBitmap(skBitmap, 50, 50, 100, 100);

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
        public async Task TestDrawImageWithImageBitmap()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            // Create source bitmap
            var sourceBitmap = new SKBitmap(50, 50);
            using (var canvas = new SKCanvas(sourceBitmap))
            {
                canvas.Clear(SKColors.Red);
            }

            var imageBitmap = new ImageBitmap(sourceBitmap);

            // Create destination canvas
            var destCanvas = new OffscreenCanvas(100, 100, mockDocument.Object);
            var context = destCanvas.getContext("2d");

            // Draw the ImageBitmap
            context.drawImage(imageBitmap, 25, 25);

            // Verify the pixel at the center of the drawn image
            var resultBitmap = destCanvas.transferToImageBitmap();
            var skBitmap = resultBitmap.GetBitmap();
            var pixel = skBitmap.GetPixel(50, 50);

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
    }
}
