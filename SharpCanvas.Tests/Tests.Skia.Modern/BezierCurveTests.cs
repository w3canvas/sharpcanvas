using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for Bézier curve rendering (quadratic and cubic)
    /// </summary>
    public class BezierCurveTests
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
            var info = new SKImageInfo(200, 200);
            _surface = SKSurface.Create(info);
            _context = new CanvasRenderingContext2D(_surface, _document);
        }

        [TearDown]
        public void Teardown()
        {
            _surface.Dispose();
        }

        [Test]
        public void TestQuadraticCurveBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "blue";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(20, 20);
            _context.quadraticCurveTo(100, 20, 100, 100);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // The curve should pass through or near the midpoint
            var midPixel = bitmap.GetPixel(70, 40);
            Assert.That(midPixel.Alpha, Is.GreaterThan(0), "Quadratic curve should be drawn");

            // End point should have pixels
            var endPixel = bitmap.GetPixel(100, 100);
            Assert.That(endPixel.Alpha, Is.GreaterThan(0), "Curve should reach end point");
        }

        [Test]
        public void TestQuadraticCurveFilled()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.beginPath();
            _context.moveTo(50, 150);
            _context.quadraticCurveTo(100, 50, 150, 150);
            _context.closePath();
            _context.fill();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // The filled region should contain the center
            var centerPixel = bitmap.GetPixel(100, 120);
            Assert.That(centerPixel, Is.EqualTo(SKColors.Red), "Filled quadratic curve area should be red");
        }

        [Test]
        public void TestQuadraticCurveMultiple()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "green";
            _context.lineWidth = 3;
            _context.beginPath();
            _context.moveTo(20, 100);
            _context.quadraticCurveTo(50, 50, 100, 100);
            _context.quadraticCurveTo(150, 150, 180, 100);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // First curve section
            var firstCurve = bitmap.GetPixel(60, 75);
            Assert.That(firstCurve.Alpha, Is.GreaterThan(0), "First curve segment should be drawn");

            // Second curve section
            var secondCurve = bitmap.GetPixel(140, 125);
            Assert.That(secondCurve.Alpha, Is.GreaterThan(0), "Second curve segment should be drawn");
        }

        [Test]
        public void TestCubicBezierBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "purple";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(20, 100);
            _context.bezierCurveTo(20, 20, 180, 20, 180, 100);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Curve should have pixels along its path
            var curvePixel = bitmap.GetPixel(100, 30);
            Assert.That(curvePixel.Alpha, Is.GreaterThan(0), "Cubic bezier curve should be drawn");
        }

        [Test]
        public void TestCubicBezierSShape()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "orange";
            _context.lineWidth = 3;
            _context.beginPath();
            _context.moveTo(50, 50);
            _context.bezierCurveTo(150, 50, 50, 150, 150, 150);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // S-curve should have pixels in the middle
            var middlePixel = bitmap.GetPixel(100, 100);
            Assert.That(middlePixel.Alpha, Is.GreaterThan(0), "S-shaped curve should be drawn");

            // Start point
            var startPixel = bitmap.GetPixel(50, 50);
            Assert.That(startPixel.Alpha, Is.GreaterThan(0), "Curve should start at specified point");
        }

        [Test]
        public void TestCubicBezierFilled()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "cyan";
            _context.beginPath();
            _context.moveTo(50, 100);
            _context.bezierCurveTo(50, 50, 150, 50, 150, 100);
            _context.lineTo(150, 150);
            _context.bezierCurveTo(150, 180, 50, 180, 50, 150);
            _context.closePath();
            _context.fill();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Center of filled region
            var centerPixel = bitmap.GetPixel(100, 120);
            Assert.That(centerPixel, Is.EqualTo(SKColors.Cyan), "Filled bezier shape should be cyan");
        }

        [Test]
        public void TestCubicBezierComplex()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "magenta";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(30, 100);
            _context.bezierCurveTo(60, 30, 140, 30, 170, 100);
            _context.bezierCurveTo(140, 170, 60, 170, 30, 100);
            _context.closePath();
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Complex curve should create a shape
            var topCurve = bitmap.GetPixel(100, 40);
            Assert.That(topCurve.Alpha, Is.GreaterThan(0), "Top curve should be visible");

            var bottomCurve = bitmap.GetPixel(100, 160);
            Assert.That(bottomCurve.Alpha, Is.GreaterThan(0), "Bottom curve should be visible");
        }

        [Test]
        public void TestMixedCurvesPath()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "brown";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(20, 100);
            // Quadratic curve
            _context.quadraticCurveTo(60, 50, 100, 100);
            // Cubic curve
            _context.bezierCurveTo(120, 120, 140, 120, 160, 100);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Quadratic section
            var quadPixel = bitmap.GetPixel(50, 75);
            Assert.That(quadPixel.Alpha, Is.GreaterThan(0), "Quadratic section should be drawn");

            // Cubic section
            var cubicPixel = bitmap.GetPixel(140, 110);
            Assert.That(cubicPixel.Alpha, Is.GreaterThan(0), "Cubic section should be drawn");
        }

        [Test]
        public void TestQuadraticCurveFromLine()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "navy";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(20, 20);
            _context.lineTo(50, 20);
            _context.quadraticCurveTo(80, 50, 50, 80);
            _context.lineTo(20, 80);
            _context.closePath();
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should have a complete path
            var linePixel = bitmap.GetPixel(35, 20);
            Assert.That(linePixel.Alpha, Is.GreaterThan(0), "Line should be drawn");

            var curvePixel = bitmap.GetPixel(70, 55);
            Assert.That(curvePixel.Alpha, Is.GreaterThan(0), "Curve should be drawn");
        }

        [Test]
        public void TestBezierWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.translate(50, 50);
            _context.scale(1.5, 1.5);
            _context.strokeStyle = "pink";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(0, 0);
            _context.bezierCurveTo(30, 0, 30, 30, 0, 30);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Transformed curve should be drawn
            var pixel = bitmap.GetPixel(80, 70);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Transformed bezier curve should be drawn");
        }

        [Test]
        public void TestCoincidentControlPoints()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "gray";
            _context.lineWidth = 2;
            _context.beginPath();
            _context.moveTo(50, 50);
            // Quadratic with control point at start = straight line
            _context.quadraticCurveTo(50, 50, 150, 50);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should still draw (as a line)
            var pixel = bitmap.GetPixel(100, 50);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Degenerate curve should still draw");
        }
    }
}
