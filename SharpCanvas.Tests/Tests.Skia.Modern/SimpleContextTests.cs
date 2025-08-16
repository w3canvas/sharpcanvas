using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    public class SimpleContextTests
    {
        private SKSurface _surface;
        private CanvasRenderingContext2D _context;
        private IDocument _document;

        [SetUp]
        public void Setup()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);
            _document = mockDocument.Object;
            var info = new SKImageInfo(100, 100);
            _surface = SKSurface.Create(info);
            _context = new CanvasRenderingContext2D(_surface, _document);
        }

        [TearDown]
        public void Teardown()
        {
            _surface.Dispose();
        }

        [Test]
        public void TestArcTo()
        {
            _context.fillStyle = "blue";
            _context.beginPath();
            _context.moveTo(20, 20);
            _context.lineTo(80, 20);
            _context.arcTo(80, 80, 20, 80, 20);
            _context.lineTo(20, 80);
            _context.closePath();
            _context.fill();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check a pixel within the main body of the shape
            var centerPixel = bitmap.GetPixel(50, 50);
            Assert.That(centerPixel, Is.EqualTo(SKColors.Blue), "The center pixel should be blue.");

            // Check a pixel within the rounded corner
            var cornerPixel = bitmap.GetPixel(70, 70);
            Assert.That(cornerPixel, Is.EqualTo(SKColors.Blue), "The corner pixel should be blue.");
        }
    }
}
