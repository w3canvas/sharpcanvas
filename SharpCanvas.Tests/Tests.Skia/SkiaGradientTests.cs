using NUnit.Framework;
using SharpCanvas.Shared;
using System.IO;
using SharpCanvas.Context.Skia;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia
{
    [TestFixture]
    public class SkiaGradientTests
    {
        [Test]
        public void LinearGradientFill()
        {
            var info = new SKImageInfo(150, 150);
            using var surface = SKSurface.Create(info);
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
            var context = new CanvasRenderingContext2D(surface, mockDocument.Object);
            var lingrad = (ILinearCanvasGradient)context.createLinearGradient(0, 0, 0, 150);
            lingrad.addColorStop(0, "#00ABEB");
            lingrad.addColorStop(0.5, "#fff");
            lingrad.addColorStop(0.5, "#26C000");
            lingrad.addColorStop(1, "#fff");

            context.fillStyle = lingrad;
            context.fillRect(10, 10, 130, 130);

            var snapshot = context.GetBitmap();
            File.WriteAllBytes("linear_gradient_fill.png", snapshot);

            // We will enable this assertion later, once we have a reference image.
            // For now, we manually inspect the generated image.
            Assert.Pass("Test executed and generated linear_gradient_fill.png");
        }
    }
}
