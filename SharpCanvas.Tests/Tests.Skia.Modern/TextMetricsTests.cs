using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    public class TextMetricsTests
    {
        private SKSurface _surface;
        private SkiaCanvasRenderingContext2DBase _context;
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
            var info = new SKImageInfo(200, 200);
            _surface = SKSurface.Create(info);
            _context = new SkiaCanvasRenderingContext2DBaseTestWrapper(_surface, _document);
        }

        [TearDown]
        public void Teardown()
        {
            _surface.Dispose();
        }

        private class SkiaCanvasRenderingContext2DBaseTestWrapper : SkiaCanvasRenderingContext2DBase
        {
            public SkiaCanvasRenderingContext2DBaseTestWrapper(SKSurface surface, IDocument document) : base(surface, document) { }
        }

        [Test]
        public void TestTextMetricsProperties()
        {
            _context.font = "20px sans-serif";
            _context.textBaseLine = "alphabetic";
            _context.textAlign = "start"; // left

            dynamic metrics = _context.measureText("Hello World");

            Assert.That(metrics.width, Is.GreaterThan(0));
            Assert.That(metrics.actualBoundingBoxAscent, Is.GreaterThan(0));
            Assert.That(metrics.actualBoundingBoxDescent, Is.GreaterThanOrEqualTo(0));
            Assert.That(metrics.fontBoundingBoxAscent, Is.GreaterThan(0));
            Assert.That(metrics.fontBoundingBoxDescent, Is.GreaterThan(0));
        }

        [Test]
        public void TestTextMetricsAlignment()
        {
            _context.font = "20px sans-serif";
            var text = "Hello";

            // Left align
            _context.textAlign = "left";
            dynamic mLeft = _context.measureText(text);

            // Center align
            _context.textAlign = "center";
            dynamic mCenter = _context.measureText(text);

            // Right align
            _context.textAlign = "right";
            dynamic mRight = _context.measureText(text);

            // Width should be same
            Assert.That(mLeft.width, Is.EqualTo(mCenter.width).Within(0.1));

            // Center approx width/2
            Assert.That(mCenter.actualBoundingBoxLeft, Is.EqualTo(mLeft.width / 2).Within(5));

            // Right approx width
            Assert.That(mRight.actualBoundingBoxLeft, Is.EqualTo(mLeft.width).Within(5));
        }

        [Test]
        public void TestTextMetricsBaseline()
        {
            _context.font = "20px sans-serif";

            // Alphabetic
            _context.textBaseLine = "alphabetic";
            dynamic mAlphabetic = _context.measureText("Hello");
            Assert.That(mAlphabetic.alphabeticBaseline, Is.EqualTo(0));

            // Top
            _context.textBaseLine = "top";
            dynamic mTop = _context.measureText("Hello");

            // Alphabetic baseline is below Top.
            // If property is "Distance UP", it should be negative.
            Assert.That(mTop.alphabeticBaseline, Is.LessThan(0));
        }
    }
}
