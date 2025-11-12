using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for ellipse, roundRect, and other advanced context methods
    /// </summary>
    public class AdvancedShapeAndContextTests
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
            var info = new SKImageInfo(400, 400);
            _surface = SKSurface.Create(info);
            _context = new CanvasRenderingContext2D(_surface, _document);
        }

        [TearDown]
        public void Teardown()
        {
            _surface.Dispose();
        }

        #region Ellipse Context Method Tests

        [Test]
        public void TestEllipseBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 50, 80, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = "blue";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseWithStroke()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 60, 40, 0, 0, 2 * Math.PI, false);
            _context.strokeStyle = "red";
            _context.lineWidth = 3;

            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestEllipseWithRotation()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 80, 40, Math.PI / 4, 0, 2 * Math.PI, false);
            _context.fillStyle = "green";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipsePartialArc()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 70, 50, 0, 0, Math.PI, false);
            _context.strokeStyle = "purple";
            _context.lineWidth = 2;

            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestEllipseAnticlockwise()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 60, 90, 0, 0, Math.PI * 1.5, true);
            _context.fillStyle = "orange";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseCircle()
        {
            // When radiusX == radiusY, ellipse should be a circle
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.ellipse(200, 200, 50, 50, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = "cyan";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseMultiple()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.fillStyle = "rgba(255, 0, 0, 0.5)";

            _context.beginPath();
            _context.ellipse(150, 200, 40, 60, 0, 0, 2 * Math.PI, false);
            _context.fill();

            _context.beginPath();
            _context.ellipse(250, 200, 60, 40, Math.PI / 3, 0, 2 * Math.PI, false);
            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.translate(100, 100);
            _context.scale(1.5, 0.8);

            _context.beginPath();
            _context.ellipse(50, 50, 30, 40, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = "yellow";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseEdgeCases()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Zero start and end angles
            Assert.DoesNotThrow(() =>
            {
                _context.beginPath();
                _context.ellipse(100, 100, 50, 30, 0, 0, 0, false);
                _context.stroke();
            });

            // Very small radii
            Assert.DoesNotThrow(() =>
            {
                _context.beginPath();
                _context.ellipse(200, 100, 1, 1, 0, 0, 2 * Math.PI, false);
                _context.stroke();
            });
        }

        #endregion

        #region RoundRect Context Method Tests

        [Test]
        public void TestRoundRectBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(50, 50, 100, 80, 10);
            _context.fillStyle = "blue";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectWithStroke()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(100, 100, 150, 100, 15);
            _context.strokeStyle = "red";
            _context.lineWidth = 3;

            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestRoundRectZeroRadius()
        {
            // Zero radius should produce a regular rectangle
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(50, 50, 100, 80, 0);
            _context.fillStyle = "green";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectLargeRadius()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(50, 50, 100, 100, 50); // radius = half of dimension
            _context.fillStyle = "purple";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectWithArray()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            // Different radii for each corner
            _context.roundRect(50, 50, 120, 100, new double[] { 5, 10, 15, 20 });
            _context.fillStyle = "orange";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectMultiple()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            _context.beginPath();
            _context.roundRect(50, 50, 80, 60, 10);
            _context.stroke();

            _context.beginPath();
            _context.roundRect(150, 50, 80, 60, 20);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestRoundRectWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.translate(100, 100);
            _context.rotate(Math.PI / 6);

            _context.beginPath();
            _context.roundRect(0, 0, 100, 60, 12);
            _context.fillStyle = "cyan";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectNullRadii()
        {
            // Null radii should produce a regular rectangle
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(50, 50, 100, 80, null);
            _context.fillStyle = "yellow";

            Assert.DoesNotThrow(() => _context.fill());
        }

        #endregion

        #region Reset Method Tests

        [Test]
        public void TestResetClearsCanvas()
        {
            // Draw something
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 400, 400);

            // Reset
            _context.reset();

            // Check that canvas is cleared
            var imageData = _context.getImageData(200, 200, 1, 1) as ImageData;
            Assert.That(imageData.data[3], Is.EqualTo(0), "Canvas should be transparent after reset");
        }

        [Test]
        public void TestResetResetsTransform()
        {
            _context.translate(100, 100);
            _context.scale(2, 2);
            _context.rotate(Math.PI / 4);

            _context.reset();

            var transform = _context.getTransform() as DOMMatrix;
            Assert.That(transform.a, Is.EqualTo(1).Within(0.001));
            Assert.That(transform.b, Is.EqualTo(0).Within(0.001));
            Assert.That(transform.c, Is.EqualTo(0).Within(0.001));
            Assert.That(transform.d, Is.EqualTo(1).Within(0.001));
            Assert.That(transform.e, Is.EqualTo(0).Within(0.001));
            Assert.That(transform.f, Is.EqualTo(0).Within(0.001));
        }

        [Test]
        public void TestResetResetsProperties()
        {
            _context.fillStyle = "red";
            _context.strokeStyle = "blue";
            _context.lineWidth = 10;
            _context.globalAlpha = 0.5;
            _context.font = "24px Arial";

            _context.reset();

            // Properties should be reset to defaults
            Assert.That(_context.lineWidth, Is.EqualTo(1));
            Assert.That(_context.globalAlpha, Is.EqualTo(1));
        }

        [Test]
        public void TestResetClearsPath()
        {
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(100, 100);

            _context.reset();

            // After reset, drawing should not show previous path
            _context.strokeStyle = "black";
            Assert.DoesNotThrow(() => _context.stroke()); // Should not throw, but won't draw old path
        }

        [Test]
        public void TestResetClearsStateStack()
        {
            _context.save();
            _context.save();
            _context.translate(50, 50);

            _context.reset();

            // State stack should be cleared, so restore should not have effect
            Assert.DoesNotThrow(() => _context.restore());
        }

        #endregion

        #region isContextLost Method Tests

        [Test]
        public void TestIsContextLostDefault()
        {
            // Context should not be lost by default
            Assert.That(_context.isContextLost(), Is.False);
        }

        [Test]
        public void TestIsContextLostAfterOperations()
        {
            _context.fillRect(0, 0, 100, 100);
            _context.save();
            _context.restore();
            _context.translate(50, 50);

            // Context should still not be lost
            Assert.That(_context.isContextLost(), Is.False);
        }

        [Test]
        public void TestIsContextLostAfterReset()
        {
            _context.reset();

            // Context should still not be lost after reset
            Assert.That(_context.isContextLost(), Is.False);
        }

        #endregion

        #region getContextAttributes Method Tests

        [Test]
        public void TestGetContextAttributes()
        {
            var attributes = _context.getContextAttributes();

            Assert.That(attributes, Is.Not.Null, "getContextAttributes should return an object");
        }

        [Test]
        public void TestGetContextAttributesConsistency()
        {
            var attributes1 = _context.getContextAttributes();
            var attributes2 = _context.getContextAttributes();

            Assert.That(attributes1, Is.Not.Null);
            Assert.That(attributes2, Is.Not.Null);
        }

        #endregion

        #region Combined Advanced Shape Tests

        [Test]
        public void TestEllipseAndRoundRectTogether()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Draw rounded rectangle
            _context.beginPath();
            _context.roundRect(50, 50, 150, 100, 15);
            _context.fillStyle = "lightblue";
            _context.fill();

            // Draw ellipse on top
            _context.beginPath();
            _context.ellipse(125, 100, 40, 30, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = "red";
            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestComplexPathWithEllipseAndRoundRect()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.roundRect(50, 50, 100, 80, 10);
            _context.ellipse(200, 90, 50, 40, 0, 0, 2 * Math.PI, false);
            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestEllipseWithClipping()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create clipping region
            _context.beginPath();
            _context.rect(100, 100, 200, 200);
            _context.clip();

            // Draw ellipse (partially clipped)
            _context.beginPath();
            _context.ellipse(200, 200, 150, 100, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = "green";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestRoundRectWithShadow()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.shadowColor = "rgba(0, 0, 0, 0.5)";
            _context.shadowBlur = 10;
            _context.shadowOffsetX = 5;
            _context.shadowOffsetY = 5;

            _context.beginPath();
            _context.roundRect(100, 100, 150, 100, 20);
            _context.fillStyle = "yellow";

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestEllipseWithGradient()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(150, 150, 250, 250);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.beginPath();
            _context.ellipse(200, 200, 60, 80, 0, 0, 2 * Math.PI, false);
            _context.fillStyle = gradient;

            Assert.DoesNotThrow(() => _context.fill());
        }

        #endregion

        #region Shape Method Edge Cases

        [Test]
        public void TestEllipseNegativeRadii()
        {
            // Negative radii should throw or be handled gracefully
            Assert.Throws<NotSupportedException>(() =>
            {
                _context.beginPath();
                _context.ellipse(200, 200, -50, 30, 0, 0, 2 * Math.PI, false);
            });
        }

        [Test]
        public void TestRoundRectNegativeDimensions()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Negative dimensions should be handled
            Assert.DoesNotThrow(() =>
            {
                _context.beginPath();
                _context.roundRect(100, 100, -50, -50, 10);
                _context.stroke();
            });
        }

        [Test]
        public void TestEllipseWithNaNValues()
        {
            // NaN values should be handled gracefully
            Assert.DoesNotThrow(() =>
            {
                _context.beginPath();
                _context.ellipse(double.NaN, 200, 50, 30, 0, 0, 2 * Math.PI, false);
            });
        }

        [Test]
        public void TestRoundRectWithNaNRadius()
        {
            // NaN radius should be handled
            Assert.DoesNotThrow(() =>
            {
                _context.beginPath();
                _context.roundRect(50, 50, 100, 80, double.NaN);
                _context.stroke();
            });
        }

        #endregion

        #region Integration with Path2D

        [Test]
        public void TestEllipseInPath2D()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.ellipse(200, 200, 60, 40, 0, 0, 2 * Math.PI, false);

            _context.fillStyle = "purple";
            Assert.DoesNotThrow(() => _context.fill(path));
        }

        [Test]
        public void TestRoundRectInPath2D()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.roundRect(50, 50, 120, 80, 15);

            _context.strokeStyle = "orange";
            _context.lineWidth = 3;
            Assert.DoesNotThrow(() => _context.stroke(path));
        }

        [Test]
        public void TestComplexPath2DWithShapes()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var path = new Path2D();
            path.moveTo(100, 100);
            path.lineTo(150, 100);
            path.ellipse(150, 125, 25, 25, 0, -Math.PI / 2, Math.PI / 2, false);
            path.lineTo(150, 150);
            path.lineTo(100, 150);
            path.closePath();

            _context.fillStyle = "teal";
            Assert.DoesNotThrow(() => _context.fill(path));
        }

        #endregion
    }
}
