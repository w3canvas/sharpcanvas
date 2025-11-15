using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for clipping and hit detection operations
    /// </summary>
    public class ClippingAndHitDetectionTests
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

        #region Clipping Tests

        [Test]
        public void TestClipRectangle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create a rectangular clipping region
            _context.beginPath();
            _context.rect(50, 50, 100, 100);
            _context.clip();

            // Fill entire canvas - only clipped region should be filled
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Inside clip region
            var insidePixel = bitmap.GetPixel(100, 100);
            Assert.That(insidePixel, Is.EqualTo(SKColors.Red), "Inside clip region should be filled");

            // Outside clip region
            var outsidePixel = bitmap.GetPixel(20, 20);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0), "Outside clip region should be transparent");
        }

        [Test]
        public void TestClipCircle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create a circular clipping region
            _context.beginPath();
            _context.arc(100, 100, 50, 0, 2 * Math.PI, false);
            _context.clip();

            // Fill entire canvas
            _context.fillStyle = "blue";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Center of circle
            var centerPixel = bitmap.GetPixel(100, 100);
            Assert.That(centerPixel, Is.EqualTo(SKColors.Blue), "Center should be filled");

            // Edge of circle
            var edgePixel = bitmap.GetPixel(145, 100);
            Assert.That(edgePixel.Alpha, Is.GreaterThan(0), "Near edge should be filled");

            // Outside circle
            var outsidePixel = bitmap.GetPixel(10, 10);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0), "Outside circle should be transparent");
        }

        [Test]
        public void TestClipComplex()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create a complex path
            _context.beginPath();
            _context.moveTo(50, 50);
            _context.lineTo(150, 50);
            _context.lineTo(150, 150);
            _context.lineTo(50, 150);
            _context.closePath();
            _context.clip();

            _context.fillStyle = "green";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var insidePixel = bitmap.GetPixel(100, 100);
            Assert.That(insidePixel, Is.EqualTo(SKColors.Green));

            var outsidePixel = bitmap.GetPixel(180, 180);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0));
        }

        [Test]
        public void TestClipIntersection()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // First clip
            _context.beginPath();
            _context.rect(50, 50, 100, 100);
            _context.clip();

            // Second clip - creates intersection
            _context.beginPath();
            _context.rect(75, 75, 100, 100);
            _context.clip();

            _context.fillStyle = "purple";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // In intersection (75-150, 75-150)
            var intersectionPixel = bitmap.GetPixel(100, 100);
            Assert.That(intersectionPixel, Is.EqualTo(SKColors.Purple), "Intersection should be filled");

            // In first clip only
            var firstOnlyPixel = bitmap.GetPixel(60, 60);
            Assert.That(firstOnlyPixel.Alpha, Is.EqualTo(0), "Outside intersection should be transparent");

            // In second clip only
            var secondOnlyPixel = bitmap.GetPixel(170, 100);
            Assert.That(secondOnlyPixel.Alpha, Is.EqualTo(0), "Outside intersection should be transparent");
        }

        [Test]
        public void TestClipWithSaveRestore()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Apply clip
            _context.beginPath();
            _context.rect(50, 50, 50, 50);
            _context.clip();

            _context.save();

            // Apply second clip
            _context.beginPath();
            _context.rect(75, 75, 50, 50);
            _context.clip();

            _context.fillStyle = "orange";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var intersectionPixel = bitmap.GetPixel(90, 90);
            Assert.That(intersectionPixel, Is.EqualTo(SKColors.Orange));

            _context.restore();

            // After restore, only first clip should apply
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "cyan";
            _context.fillRect(0, 0, 200, 200);

            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var firstClipPixel = bitmap.GetPixel(60, 60);
            Assert.That(firstClipPixel, Is.EqualTo(SKColors.Cyan), "First clip region should be filled");

            var secondClipPixel = bitmap.GetPixel(120, 120);
            Assert.That(secondClipPixel.Alpha, Is.EqualTo(0), "Second clip region should not be filled");
        }

        [Test]
        public void TestClipWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.translate(50, 50);
            _context.beginPath();
            _context.rect(0, 0, 50, 50);
            _context.clip();

            _context.fillStyle = "magenta";
            _context.fillRect(-100, -100, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Clipped region is at (50, 50) to (100, 100) in canvas space
            var insidePixel = bitmap.GetPixel(75, 75);
            Assert.That(insidePixel, Is.EqualTo(SKColors.Magenta));

            var outsidePixel = bitmap.GetPixel(30, 30);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0));
        }

        #endregion

        #region Hit Detection Tests

        [Test]
        public void TestIsPointInPathRectangle()
        {
            _context.beginPath();
            _context.rect(50, 50, 100, 100);

            // Inside
            Assert.That(_context.isPointInPath(75, 75), Is.True, "Point inside rectangle should return true");
            Assert.That(_context.isPointInPath(100, 100), Is.True, "Point in center should return true");

            // Outside
            Assert.That(_context.isPointInPath(25, 25), Is.False, "Point outside should return false");
            Assert.That(_context.isPointInPath(175, 175), Is.False, "Point far outside should return false");

            // Edge (may vary by implementation)
            Assert.That(_context.isPointInPath(50, 50), Is.True, "Point on edge should be inside");
        }

        [Test]
        public void TestIsPointInPathCircle()
        {
            _context.beginPath();
            _context.arc(100, 100, 50, 0, 2 * Math.PI, false);

            // Center
            Assert.That(_context.isPointInPath(100, 100), Is.True, "Center should be inside");

            // Inside
            Assert.That(_context.isPointInPath(120, 100), Is.True, "Point inside circle should return true");

            // Outside
            Assert.That(_context.isPointInPath(160, 100), Is.False, "Point outside circle should return false");
            Assert.That(_context.isPointInPath(10, 10), Is.False, "Far point should be outside");
        }

        [Test]
        public void TestIsPointInPathComplex()
        {
            // Create a triangle
            _context.beginPath();
            _context.moveTo(100, 50);
            _context.lineTo(150, 150);
            _context.lineTo(50, 150);
            _context.closePath();

            // Inside triangle
            Assert.That(_context.isPointInPath(100, 100), Is.True, "Center of triangle should be inside");

            // Outside triangle
            Assert.That(_context.isPointInPath(100, 40), Is.False, "Above triangle should be outside");
            Assert.That(_context.isPointInPath(100, 160), Is.False, "Below triangle should be outside");
        }

        [Test]
        public void TestIsPointInStrokeBasic()
        {
            _context.lineWidth = 10;
            _context.beginPath();
            _context.moveTo(50, 50);
            _context.lineTo(150, 150);

            // On the line
            Assert.That(_context.isPointInStroke(100, 100), Is.True, "Point on line should be in stroke");

            // Near the line (within stroke width) - (103,97) is 4.24 pixels from line, within radius of 5
            Assert.That(_context.isPointInStroke(103, 97), Is.True, "Point near line should be in stroke");

            // Far from line
            Assert.That(_context.isPointInStroke(50, 150), Is.False, "Point far from line should not be in stroke");
        }

        [Test]
        public void TestIsPointInStrokeCircle()
        {
            _context.lineWidth = 5;
            _context.beginPath();
            _context.arc(100, 100, 40, 0, 2 * Math.PI, false);

            // On the circle perimeter
            Assert.That(_context.isPointInStroke(140, 100), Is.True, "Point on perimeter should be in stroke");

            // Inside circle (not on stroke)
            Assert.That(_context.isPointInStroke(100, 100), Is.False, "Center should not be in stroke");

            // Outside circle
            Assert.That(_context.isPointInStroke(160, 100), Is.False, "Far outside should not be in stroke");
        }

        [Test]
        public void TestIsPointInStrokeWithLineWidth()
        {
            _context.lineWidth = 20;
            _context.beginPath();
            _context.moveTo(100, 50);
            _context.lineTo(100, 150);

            // Directly on line
            Assert.That(_context.isPointInStroke(100, 100), Is.True, "On line should be in stroke");

            // Within stroke width
            Assert.That(_context.isPointInStroke(109, 100), Is.True, "Within stroke width should be in stroke");

            // Outside stroke width
            Assert.That(_context.isPointInStroke(121, 100), Is.False, "Outside stroke width should not be in stroke");
        }

        [Test]
        public void TestIsPointInPathWithTransform()
        {
            _context.translate(50, 50);
            _context.beginPath();
            _context.rect(0, 0, 50, 50);

            // Test in transformed space
            // Rectangle is at (50, 50) to (100, 100) in canvas space
            Assert.That(_context.isPointInPath(75, 75), Is.True, "Transformed point should be inside");
            Assert.That(_context.isPointInPath(25, 25), Is.False, "Non-transformed point should be outside");
        }

        [Test]
        public void TestCanvasPropertyExists()
        {
            // Test that canvas property doesn't throw
            var canvas = _context.canvas;
            // For now it might be null, but shouldn't throw NotImplementedException
            Assert.That(() => _context.canvas, Throws.Nothing, "canvas property should not throw");
        }

        #endregion
    }
}
