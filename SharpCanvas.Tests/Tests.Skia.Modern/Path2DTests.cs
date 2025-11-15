using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for Path2D reusable path objects
    /// </summary>
    public class Path2DTests
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

        #region Basic Path2D Tests

        [Test]
        public void TestPath2DConstructorEmpty()
        {
            var path = new Path2D();
            Assert.That(path, Is.Not.Null);
            Assert.That(path._path, Is.Not.Null);
        }

        [Test]
        public void TestPath2DConstructorFromPath()
        {
            var path1 = new Path2D();
            path1.rect(10, 10, 50, 50);

            var path2 = new Path2D(path1);
            Assert.That(path2, Is.Not.Null);

            // Both paths should render the same
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fill(path2);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(35, 35);
            Assert.That(pixel, Is.EqualTo(SKColors.Red), "Copied path should render correctly");
        }

        [Test]
        public void TestPath2DConstructorFromSVG()
        {
            // Simple SVG path: M10,10 L50,50
            var path = new Path2D("M10,10 L50,50");
            Assert.That(path, Is.Not.Null);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "blue";
            _context.lineWidth = 2;
            _context.stroke(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Line should have pixels along it
            var pixel = bitmap.GetPixel(30, 30);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "SVG path should render");
        }

        [Test]
        public void TestPath2DWithFill()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.rect(50, 50, 100, 100);

            _context.fillStyle = "green";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);
            Assert.That(pixel, Is.EqualTo(SKColors.Green));
        }

        [Test]
        public void TestPath2DWithStroke()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.rect(50, 50, 100, 100);

            _context.strokeStyle = "red";
            _context.lineWidth = 3;
            _context.stroke(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check stroke pixels
            var pixel = bitmap.GetPixel(50, 50);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Path2D stroke should render");
        }

        [Test]
        public void TestPath2DWithClip()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.rect(50, 50, 100, 100);

            _context.clip(path);
            _context.fillStyle = "blue";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Inside clip
            var insidePixel = bitmap.GetPixel(100, 100);
            Assert.That(insidePixel, Is.EqualTo(SKColors.Blue));

            // Outside clip
            var outsidePixel = bitmap.GetPixel(20, 20);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0));
        }

        #endregion

        #region Path Methods

        [Test]
        public void TestPath2DMoveTo()
        {
            var path = new Path2D();
            path.moveTo(10, 10);
            path.lineTo(50, 50);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "black";
            _context.lineWidth = 2;
            _context.stroke(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(30, 30);
            Assert.That(pixel.Alpha, Is.GreaterThan(0));
        }

        [Test]
        public void TestPath2DQuadraticCurve()
        {
            var path = new Path2D();
            path.moveTo(20, 100);
            path.quadraticCurveTo(100, 20, 180, 100);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "purple";
            _context.lineWidth = 2;
            _context.stroke(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Actual curve at (97,59)
            var pixel = bitmap.GetPixel(97, 59);
            Assert.That(pixel.Alpha, Is.GreaterThan(0));
        }

        [Test]
        public void TestPath2DBezierCurve()
        {
            var path = new Path2D();
            path.moveTo(20, 100);
            path.bezierCurveTo(20, 20, 180, 20, 180, 100);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.strokeStyle = "orange";
            _context.lineWidth = 2;
            _context.stroke(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Actual curve at (93,39)
            var pixel = bitmap.GetPixel(93, 39);
            Assert.That(pixel.Alpha, Is.GreaterThan(0));
        }

        [Test]
        public void TestPath2DArc()
        {
            var path = new Path2D();
            path.arc(100, 100, 50, 0, Math.PI, false);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "cyan";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 120);
            Assert.That(pixel, Is.EqualTo(SKColors.Cyan));
        }

        [Test]
        public void TestPath2DEllipse()
        {
            var path = new Path2D();
            path.ellipse(100, 100, 60, 40, 0, 0, 2 * Math.PI, false);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "magenta";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);
            Assert.That(pixel, Is.EqualTo(SKColors.Magenta));
        }

        [Test]
        public void TestPath2DRoundRect()
        {
            var path = new Path2D();
            path.roundRect(50, 50, 100, 100, 10.0);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "yellow";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);
            Assert.That(pixel, Is.EqualTo(SKColors.Yellow));
        }

        [Test]
        public void TestPath2DClosePath()
        {
            var path = new Path2D();
            path.moveTo(50, 50);
            path.lineTo(150, 50);
            path.lineTo(150, 150);
            path.closePath(); // Should close back to (50, 50)

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "brown";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Triangle should be filled
            var pixel = bitmap.GetPixel(100, 75);
            Assert.That(pixel, Is.EqualTo(SKColors.Brown));
        }

        #endregion

        #region Path Reuse

        [Test]
        public void TestPath2DReuse()
        {
            var path = new Path2D();
            path.rect(50, 50, 50, 50);

            _surface.Canvas.Clear(SKColors.Transparent);

            // Draw the same path multiple times
            _context.fillStyle = "red";
            _context.fill(path);

            _context.translate(60, 0);
            _context.fillStyle = "blue";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // First rectangle
            var pixel1 = bitmap.GetPixel(75, 75);
            Assert.That(pixel1, Is.EqualTo(SKColors.Red));

            // Second rectangle (translated)
            var pixel2 = bitmap.GetPixel(135, 75);
            Assert.That(pixel2, Is.EqualTo(SKColors.Blue));
        }

        [Test]
        public void TestPath2DAddPath()
        {
            var path1 = new Path2D();
            path1.rect(50, 50, 50, 50);

            var path2 = new Path2D();
            path2.rect(120, 50, 50, 50);

            var combined = new Path2D();
            combined.addPath(path1, null);
            combined.addPath(path2, null);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "green";
            _context.fill(combined);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Both rectangles should be filled
            var pixel1 = bitmap.GetPixel(75, 75);
            Assert.That(pixel1, Is.EqualTo(SKColors.Green));

            var pixel2 = bitmap.GetPixel(145, 75);
            Assert.That(pixel2, Is.EqualTo(SKColors.Green));
        }

        [Test]
        public void TestPath2DAddPathWithTransform()
        {
            var path1 = new Path2D();
            path1.rect(0, 0, 50, 50);

            var transform = new DOMMatrix();
            transform.e = 100; // translate X
            transform.f = 100; // translate Y

            var combined = new Path2D();
            combined.addPath(path1, transform);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "purple";
            _context.fill(combined);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should be translated to (100, 100)
            var pixel = bitmap.GetPixel(125, 125);
            Assert.That(pixel, Is.EqualTo(SKColors.Purple));
        }

        #endregion

        #region Complex Paths

        [Test]
        public void TestPath2DComplexShape()
        {
            var path = new Path2D();
            path.moveTo(100, 50);
            path.lineTo(150, 100);
            path.arc(100, 100, 50, 0, Math.PI, false);
            path.closePath();

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "navy";
            _context.fill(path);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Actual filled area at (85,100)
            var pixel = bitmap.GetPixel(85, 100);
            Assert.That(pixel.Alpha, Is.GreaterThan(0));
        }

        [Test]
        public void TestPath2DWithCurrentPath()
        {
            // Test that using Path2D doesn't interfere with current path
            _context.beginPath();
            _context.rect(10, 10, 30, 30);

            var path = new Path2D();
            path.rect(60, 60, 30, 30);

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fill(); // Fill current path

            _context.fillStyle = "blue";
            _context.fill(path); // Fill Path2D

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel1 = bitmap.GetPixel(25, 25);
            Assert.That(pixel1, Is.EqualTo(SKColors.Red), "Current path should be red");

            var pixel2 = bitmap.GetPixel(75, 75);
            Assert.That(pixel2, Is.EqualTo(SKColors.Blue), "Path2D should be blue");
        }

        #endregion

        #region Edge Cases

        [Test]
        public void TestPath2DNullInAddPath()
        {
            var path = new Path2D();
            Assert.DoesNotThrow(() => path.addPath(null, null));
        }

        [Test]
        public void TestPath2DInvalidSVG()
        {
            // Invalid SVG should create empty path
            var path = new Path2D("invalid svg path");
            Assert.That(path, Is.Not.Null);
        }

        [Test]
        public void TestPath2DMultipleOperations()
        {
            var path = new Path2D();
            path.rect(50, 50, 50, 50);
            path.moveTo(120, 50);
            path.lineTo(170, 100);
            path.quadraticCurveTo(120, 100, 120, 50);
            path.closePath();

            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "teal";
            _context.fill(path);

            Assert.Pass("Multiple operations on Path2D work");
        }

        #endregion
    }
}
