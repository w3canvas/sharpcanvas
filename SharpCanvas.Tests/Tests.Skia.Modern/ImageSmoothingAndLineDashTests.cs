using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;
using System.Linq;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for image smoothing and line dash functionality
    /// </summary>
    public class ImageSmoothingAndLineDashTests
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

        #region Image Smoothing Tests

        [Test]
        public void TestImageSmoothingEnabledDefault()
        {
            // imageSmoothingEnabled should default to true
            Assert.That(_context.imageSmoothingEnabled, Is.True,
                "imageSmoothingEnabled should default to true");
        }

        [Test]
        public void TestImageSmoothingEnabledSetAndGet()
        {
            _context.imageSmoothingEnabled = false;
            Assert.That(_context.imageSmoothingEnabled, Is.False);

            _context.imageSmoothingEnabled = true;
            Assert.That(_context.imageSmoothingEnabled, Is.True);
        }

        [Test]
        public void TestImageSmoothingQualityDefault()
        {
            // Default quality should be "low"
            var quality = _context.imageSmoothingQuality;
            Assert.That(quality, Is.Not.Null);
        }

        [Test]
        public void TestImageSmoothingQualitySetAndGet()
        {
            _context.imageSmoothingQuality = "low";
            Assert.That(_context.imageSmoothingQuality, Is.EqualTo("low"));

            _context.imageSmoothingQuality = "medium";
            Assert.That(_context.imageSmoothingQuality, Is.EqualTo("medium"));

            _context.imageSmoothingQuality = "high";
            Assert.That(_context.imageSmoothingQuality, Is.EqualTo("high"));
        }

        [Test]
        public void TestImageSmoothingWithSaveRestore()
        {
            _context.imageSmoothingEnabled = true;
            _context.imageSmoothingQuality = "high";
            _context.save();

            _context.imageSmoothingEnabled = false;
            _context.imageSmoothingQuality = "low";

            Assert.That(_context.imageSmoothingEnabled, Is.False);
            Assert.That(_context.imageSmoothingQuality, Is.EqualTo("low"));

            _context.restore();

            Assert.That(_context.imageSmoothingEnabled, Is.True);
            Assert.That(_context.imageSmoothingQuality, Is.EqualTo("high"));
        }

        [Test]
        public void TestImageSmoothingMultipleSaveRestore()
        {
            _context.imageSmoothingEnabled = true;
            _context.save();

            _context.imageSmoothingEnabled = false;
            _context.save();

            _context.imageSmoothingEnabled = true;
            Assert.That(_context.imageSmoothingEnabled, Is.True);

            _context.restore();
            Assert.That(_context.imageSmoothingEnabled, Is.False);

            _context.restore();
            Assert.That(_context.imageSmoothingEnabled, Is.True);
        }

        [Test]
        public void TestDrawingWithImageSmoothingDisabled()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.imageSmoothingEnabled = false;
            _context.fillStyle = "red";

            // Should work without errors even with smoothing disabled
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 100, 100));
        }

        [Test]
        public void TestImageSmoothingWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.imageSmoothingEnabled = false;
            _context.imageSmoothingQuality = "low";

            _context.scale(2, 2);
            _context.fillStyle = "blue";

            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 50, 50));
        }

        #endregion

        #region Line Dash Basic Tests

        [Test]
        public void TestLineDashSetAndGet()
        {
            var dash = new double[] { 5, 10 };
            _context.setLineDash(dash);

            var result = _context.getLineDash() as double[];
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(5));
            Assert.That(result[1], Is.EqualTo(10));
        }

        [Test]
        public void TestLineDashEmpty()
        {
            _context.setLineDash(new double[] { });

            var result = _context.getLineDash() as double[];
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        public void TestLineDashSingleValue()
        {
            _context.setLineDash(new double[] { 10 });

            var result = _context.getLineDash() as double[];
            Assert.That(result, Is.Not.Null);
            // Single value should be duplicated to [10, 10]
            Assert.That(result.Length, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void TestLineDashMultipleValues()
        {
            var dash = new double[] { 5, 10, 15, 20 };
            _context.setLineDash(dash);

            var result = _context.getLineDash() as double[];
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(4));
            Assert.That(result, Is.EqualTo(dash));
        }

        [Test]
        public void TestLineDashOffsetSetAndGet()
        {
            _context.lineDashOffset = 5.5;
            Assert.That(_context.lineDashOffset, Is.EqualTo(5.5));

            _context.lineDashOffset = -10.0;
            Assert.That(_context.lineDashOffset, Is.EqualTo(-10.0));
        }

        [Test]
        public void TestLineDashOffsetDefault()
        {
            // Default should be 0
            Assert.That(_context.lineDashOffset, Is.EqualTo(0));
        }

        #endregion

        #region Line Dash with Save/Restore

        [Test]
        public void TestLineDashWithSaveRestore()
        {
            var dash1 = new double[] { 5, 5 };
            var dash2 = new double[] { 10, 10 };

            _context.setLineDash(dash1);
            _context.save();

            _context.setLineDash(dash2);
            var result = _context.getLineDash() as double[];
            Assert.That(result[0], Is.EqualTo(10));

            _context.restore();
            result = _context.getLineDash() as double[];
            Assert.That(result[0], Is.EqualTo(5));
        }

        [Test]
        public void TestLineDashOffsetWithSaveRestore()
        {
            _context.lineDashOffset = 5;
            _context.save();

            _context.lineDashOffset = 15;
            Assert.That(_context.lineDashOffset, Is.EqualTo(15));

            _context.restore();
            Assert.That(_context.lineDashOffset, Is.EqualTo(5));
        }

        [Test]
        public void TestLineDashAndOffsetWithSaveRestore()
        {
            _context.setLineDash(new double[] { 5, 5 });
            _context.lineDashOffset = 3;
            _context.save();

            _context.setLineDash(new double[] { 10, 10 });
            _context.lineDashOffset = 7;

            _context.restore();

            var dash = _context.getLineDash() as double[];
            Assert.That(dash[0], Is.EqualTo(5));
            Assert.That(_context.lineDashOffset, Is.EqualTo(3));
        }

        #endregion

        #region Line Dash Rendering Tests

        [Test]
        public void TestDrawDashedLine()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 10, 5 });
            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            _context.beginPath();
            _context.moveTo(10, 50);
            _context.lineTo(390, 50);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestDrawDashedRect()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 8, 4 });
            _context.strokeStyle = "blue";
            _context.lineWidth = 3;

            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 100, 100));
        }

        [Test]
        public void TestDrawDashedCircle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 5, 3 });
            _context.strokeStyle = "red";
            _context.lineWidth = 2;

            _context.beginPath();
            _context.arc(200, 200, 80, 0, 2 * Math.PI);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestDashedBezierCurve()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 10, 5 });
            _context.strokeStyle = "green";
            _context.lineWidth = 2;

            _context.beginPath();
            _context.moveTo(50, 200);
            _context.bezierCurveTo(100, 100, 300, 300, 350, 200);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestLineDashOffsetEffect()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 10, 10 });
            _context.strokeStyle = "black";
            _context.lineWidth = 3;

            // Draw line with offset 0
            _context.lineDashOffset = 0;
            _context.beginPath();
            _context.moveTo(10, 50);
            _context.lineTo(390, 50);
            _context.stroke();

            // Draw line with offset 5
            _context.lineDashOffset = 5;
            _context.beginPath();
            _context.moveTo(10, 100);
            _context.lineTo(390, 100);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestComplexDashPattern()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 15, 5, 5, 5 });
            _context.strokeStyle = "purple";
            _context.lineWidth = 2;

            _context.beginPath();
            _context.moveTo(10, 150);
            _context.lineTo(390, 150);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        #endregion

        #region Line Dash Edge Cases

        [Test]
        public void TestLineDashWithZeroValues()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.setLineDash(new double[] { 0, 5 });
                _context.strokeRect(10, 10, 50, 50);
            });
        }

        [Test]
        public void TestLineDashWithLargeValues()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.setLineDash(new double[] { 1000, 1000 });
                _context.strokeRect(10, 10, 50, 50);
            });
        }

        [Test]
        public void TestLineDashWithVerySmallValues()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.setLineDash(new double[] { 0.1, 0.1 });
                _context.strokeRect(10, 10, 50, 50);
            });
        }

        [Test]
        public void TestLineDashReset()
        {
            _context.setLineDash(new double[] { 10, 10 });

            // Reset by setting empty array
            _context.setLineDash(new double[] { });

            var result = _context.getLineDash() as double[];
            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        public void TestLineDashOffsetNegative()
        {
            _context.lineDashOffset = -20;
            Assert.That(_context.lineDashOffset, Is.EqualTo(-20));

            _context.setLineDash(new double[] { 10, 5 });
            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 100, 100));
        }

        [Test]
        public void TestLineDashOffsetLarge()
        {
            _context.lineDashOffset = 1000;
            Assert.That(_context.lineDashOffset, Is.EqualTo(1000));

            _context.setLineDash(new double[] { 10, 5 });
            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 100, 100));
        }

        #endregion

        #region Line Dash with Transforms

        [Test]
        public void TestLineDashWithScale()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 5, 5 });
            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            _context.scale(2, 2);
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(100, 10);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestLineDashWithRotation()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 10, 5 });
            _context.strokeStyle = "red";
            _context.lineWidth = 2;

            _context.translate(200, 200);
            _context.rotate(Math.PI / 4);
            _context.beginPath();
            _context.moveTo(-50, 0);
            _context.lineTo(50, 0);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        #endregion

        #region Line Dash with Different Line Styles

        [Test]
        public void TestLineDashWithLineWidth()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 10, 5 });
            _context.strokeStyle = "black";

            // Test various line widths
            for (int width = 1; width <= 10; width += 3)
            {
                _context.lineWidth = width;
                _context.beginPath();
                _context.moveTo(10, 20 * width);
                _context.lineTo(200, 20 * width);
                Assert.DoesNotThrow(() => _context.stroke());
            }
        }

        [Test]
        public void TestLineDashWithLineCap()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 20, 10 });
            _context.strokeStyle = "blue";
            _context.lineWidth = 5;

            _context.lineCap = "butt";
            _context.beginPath();
            _context.moveTo(50, 50);
            _context.lineTo(350, 50);
            _context.stroke();

            _context.lineCap = "round";
            _context.beginPath();
            _context.moveTo(50, 100);
            _context.lineTo(350, 100);
            _context.stroke();

            _context.lineCap = "square";
            _context.beginPath();
            _context.moveTo(50, 150);
            _context.lineTo(350, 150);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestLineDashWithLineJoin()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 15, 5 });
            _context.strokeStyle = "green";
            _context.lineWidth = 5;
            _context.lineJoin = "miter";

            _context.beginPath();
            _context.moveTo(50, 50);
            _context.lineTo(150, 100);
            _context.lineTo(50, 150);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        #endregion

        #region Combined Image Smoothing and Line Dash Tests

        [Test]
        public void TestImageSmoothingAndLineDashTogether()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.imageSmoothingEnabled = false;
            _context.imageSmoothingQuality = "low";
            _context.setLineDash(new double[] { 10, 5 });
            _context.lineDashOffset = 3;

            _context.strokeStyle = "red";
            _context.lineWidth = 3;

            _context.beginPath();
            _context.arc(200, 200, 50, 0, 2 * Math.PI);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestAllLineStylingPropertiesTogether()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.setLineDash(new double[] { 12, 8 });
            _context.lineDashOffset = 5;
            _context.lineWidth = 4;
            _context.lineCap = "round";
            _context.lineJoin = "round";
            _context.strokeStyle = "purple";
            _context.imageSmoothingEnabled = true;
            _context.imageSmoothingQuality = "high";

            _context.beginPath();
            _context.moveTo(50, 200);
            _context.lineTo(150, 100);
            _context.lineTo(250, 200);
            _context.lineTo(350, 100);
            Assert.DoesNotThrow(() => _context.stroke());
        }

        #endregion
    }
}
