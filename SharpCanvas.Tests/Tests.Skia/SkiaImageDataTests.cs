using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia
{
    public class SkiaImageDataTests
    {
        [Test]
        public void TestGetAndPutImageData()
        {
            var info = new SKImageInfo(100, 100);
            using (var surface = SKSurface.Create(info))
            {
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
                var context = new CanvasRenderingContext2D(surface, mockDocument.Object);

                // 1. Draw a red rectangle on a white background
                surface.Canvas.Clear(SKColors.White);
                context.fillStyle = "red";
                context.fillRect(10, 10, 20, 20);

                // 2. Get the image data
                var imageData = context.getImageData(0, 0, 100, 100) as SharpCanvas.Shared.ImageData;
                Assert.NotNull(imageData);
                Assert.That(imageData.width, Is.EqualTo(100));
                Assert.That(imageData.height, Is.EqualTo(100));

                // 3. Clear the canvas to a different color
                surface.Canvas.Clear(SKColors.Blue);

                // 4. Put the image data back
                context.putImageData(imageData, 0, 0);

                // 5. Verify the pixels
                var bitmap = new SKBitmap(info);
                surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check a pixel inside the rectangle
                Assert.That(bitmap.GetPixel(15, 15), Is.EqualTo(SKColors.Red));
                // Check a pixel in the white background area
                Assert.That(bitmap.GetPixel(40, 40), Is.EqualTo(SKColors.White));
                // Check a pixel at the corner
                Assert.That(bitmap.GetPixel(0, 0), Is.EqualTo(SKColors.White));
            }
        }
    }
}
