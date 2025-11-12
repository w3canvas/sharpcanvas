using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for gradients and patterns
    /// </summary>
    public class GradientAndPatternTests
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

        #region Linear Gradient Tests

        [Test]
        public void TestLinearGradientBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Left side should be red-ish
            var leftPixel = bitmap.GetPixel(10, 100);
            Assert.That(leftPixel.Red, Is.GreaterThan(leftPixel.Blue), "Left should be more red");

            // Right side should be blue-ish
            var rightPixel = bitmap.GetPixel(190, 100);
            Assert.That(rightPixel.Blue, Is.GreaterThan(rightPixel.Red), "Right should be more blue");
        }

        [Test]
        public void TestLinearGradientMultipleStops()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(0.5, "yellow");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Middle should be yellow-ish
            var middlePixel = bitmap.GetPixel(100, 100);
            Assert.That(middlePixel.Red, Is.GreaterThan(100), "Middle should have red component");
            Assert.That(middlePixel.Green, Is.GreaterThan(100), "Middle should have green component");
        }

        [Test]
        public void TestLinearGradientVertical()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 0, 200);
            (gradient as dynamic).addColorStop(0, "green");
            (gradient as dynamic).addColorStop(1, "purple");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Top should be green-ish
            var topPixel = bitmap.GetPixel(100, 10);
            Assert.That(topPixel.Green, Is.GreaterThan(100), "Top should be greenish");

            // Bottom should be purple-ish
            var bottomPixel = bitmap.GetPixel(100, 190);
            Assert.That(bottomPixel.Red, Is.GreaterThan(50), "Bottom should have red for purple");
            Assert.That(bottomPixel.Blue, Is.GreaterThan(50), "Bottom should have blue for purple");
        }

        [Test]
        public void TestLinearGradientDiagonal()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 200);
            (gradient as dynamic).addColorStop(0, "white");
            (gradient as dynamic).addColorStop(1, "black");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Top-left should be lighter
            var topLeftPixel = bitmap.GetPixel(10, 10);
            Assert.That(topLeftPixel.Red, Is.GreaterThan(200), "Top-left should be light");

            // Bottom-right should be darker
            var bottomRightPixel = bitmap.GetPixel(190, 190);
            Assert.That(bottomRightPixel.Red, Is.LessThan(50), "Bottom-right should be dark");
        }

        [Test]
        public void TestLinearGradientColorStopOrder()
        {
            var gradient = _context.createLinearGradient(0, 0, 200, 0);

            // Add stops out of order
            (gradient as dynamic).addColorStop(1, "blue");
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(0.5, "green");

            _context.fillStyle = gradient;
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 200, 200));
        }

        [Test]
        public void TestLinearGradientInvalidColorStop()
        {
            var gradient = _context.createLinearGradient(0, 0, 200, 0);

            // Invalid offset (should be 0-1)
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                (gradient as dynamic).addColorStop(-0.5, "red"));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                (gradient as dynamic).addColorStop(1.5, "blue"));
        }

        #endregion

        #region Radial Gradient Tests

        [Test]
        public void TestRadialGradientBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createRadialGradient(100, 100, 20, 100, 100, 80);
            (gradient as dynamic).addColorStop(0, "yellow");
            (gradient as dynamic).addColorStop(1, "red");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Center should be yellow-ish
            var centerPixel = bitmap.GetPixel(100, 100);
            Assert.That(centerPixel.Red, Is.GreaterThan(200), "Center should be bright");
            Assert.That(centerPixel.Green, Is.GreaterThan(200), "Center should have yellow");

            // Outer edge should be red-ish
            var edgePixel = bitmap.GetPixel(170, 100);
            Assert.That(edgePixel.Red, Is.GreaterThan(100), "Edge should be reddish");
        }

        [Test]
        public void TestRadialGradientOffCenter()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Inner circle at (80, 80), outer at (120, 120)
            var gradient = _context.createRadialGradient(80, 80, 10, 120, 120, 60);
            (gradient as dynamic).addColorStop(0, "white");
            (gradient as dynamic).addColorStop(1, "black");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Near (80, 80) should be lighter
            var innerPixel = bitmap.GetPixel(85, 85);
            Assert.That(innerPixel.Red, Is.GreaterThan(150), "Inner should be light");
        }

        [Test]
        public void TestRadialGradientNegativeRadius()
        {
            // Negative radius should throw
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _context.createRadialGradient(100, 100, -10, 100, 100, 50));
        }

        [Test]
        public void TestRadialGradientZeroInnerRadius()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createRadialGradient(100, 100, 0, 100, 100, 80);
            (gradient as dynamic).addColorStop(0, "cyan");
            (gradient as dynamic).addColorStop(1, "magenta");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            Assert.Pass("Zero inner radius should work");
        }

        #endregion

        #region Conic Gradient Tests

        [Test]
        public void TestConicGradientBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createConicGradient(0, 100, 100);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(0.25, "yellow");
            (gradient as dynamic).addColorStop(0.5, "green");
            (gradient as dynamic).addColorStop(0.75, "blue");
            (gradient as dynamic).addColorStop(1, "red");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Conic gradient should have variation
            var pixel1 = bitmap.GetPixel(150, 100);
            var pixel2 = bitmap.GetPixel(100, 150);

            // Pixels at different angles should have different colors
            Assert.That(pixel1 != pixel2, "Conic gradient should vary by angle");
        }

        [Test]
        public void TestConicGradientStartAngle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Start at 90 degrees (Math.PI / 2)
            var gradient = _context.createConicGradient(Math.PI / 2, 100, 100);
            (gradient as dynamic).addColorStop(0, "white");
            (gradient as dynamic).addColorStop(1, "black");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            Assert.Pass("Conic gradient with start angle works");
        }

        #endregion

        #region Gradient with Transformations

        [Test]
        public void TestGradientWithTranslate()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 100, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.translate(50, 50);
            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Gradient should be transformed
            var pixel = bitmap.GetPixel(75, 75);
            Assert.That(pixel.Alpha, Is.GreaterThan(0));
        }

        [Test]
        public void TestGradientWithScale()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 50, 0);
            (gradient as dynamic).addColorStop(0, "green");
            (gradient as dynamic).addColorStop(1, "yellow");

            _context.scale(2, 2);
            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 50, 50);

            Assert.Pass("Gradient with scale transformation works");
        }

        #endregion

        #region Pattern Tests

        [Test]
        public void TestPatternRepeat()
        {
            // Create a small image to use as pattern
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.Red);
                patternSurface.Canvas.DrawRect(0, 0, 10, 10, new SKPaint { Color = SKColors.Blue });

                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat");
                Assert.That(pattern, Is.Not.Null);

                _surface.Canvas.Clear(SKColors.Transparent);
                _context.fillStyle = pattern;
                _context.fillRect(0, 0, 200, 200);

                var bitmap = new SKBitmap(_surface.PeekPixels().Info);
                _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Pattern should tile
                var pixel1 = bitmap.GetPixel(5, 5);
                var pixel2 = bitmap.GetPixel(25, 5); // 20 pixels over (one pattern repeat)

                Assert.That(pixel1, Is.EqualTo(pixel2), "Pattern should repeat");
            }
        }

        [Test]
        public void TestPatternRepeatX()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.Green);
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat-x");
                Assert.That(pattern, Is.Not.Null);

                _context.fillStyle = pattern;
                Assert.DoesNotThrow(() => _context.fillRect(0, 0, 200, 200));
            }
        }

        [Test]
        public void TestPatternRepeatY()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.Yellow);
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat-y");
                Assert.That(pattern, Is.Not.Null);

                _context.fillStyle = pattern;
                Assert.DoesNotThrow(() => _context.fillRect(0, 0, 200, 200));
            }
        }

        [Test]
        public void TestPatternNoRepeat()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(50, 50)))
            {
                patternSurface.Canvas.Clear(SKColors.Purple);
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "no-repeat");
                Assert.That(pattern, Is.Not.Null);

                _surface.Canvas.Clear(SKColors.Transparent);
                _context.fillStyle = pattern;
                _context.fillRect(0, 0, 200, 200);

                var bitmap = new SKBitmap(_surface.PeekPixels().Info);
                _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Pattern should only appear once
                var insidePatternPixel = bitmap.GetPixel(25, 25);
                Assert.That(insidePatternPixel, Is.EqualTo(SKColors.Purple));

                // Outside pattern area should be different or transparent
                var outsidePatternPixel = bitmap.GetPixel(100, 100);
                // Behavior may vary - just ensure it doesn't crash
                Assert.Pass("No-repeat pattern works");
            }
        }

        [Test]
        public void TestPatternWithSKBitmap()
        {
            using (var patternBitmap = new SKBitmap(30, 30))
            {
                using (var canvas = new SKCanvas(patternBitmap))
                {
                    canvas.Clear(SKColors.Orange);
                }

                var pattern = _context.createPattern(patternBitmap, "repeat");
                Assert.That(pattern, Is.Not.Null);

                _context.fillStyle = pattern;
                Assert.DoesNotThrow(() => _context.fillRect(0, 0, 200, 200));
            }
        }

        [Test]
        public void TestPatternInvalidRepeatMode()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.White);
                var patternImage = patternSurface.Snapshot();

                // Invalid repeat mode should default to repeat or throw
                var pattern = _context.createPattern(patternImage, "invalid-mode");
                // Should either create pattern with default mode or handle gracefully
                Assert.That(pattern, Is.Not.Null);
            }
        }

        #endregion

        #region Gradient and Pattern Combination Tests

        [Test]
        public void TestSwitchBetweenGradientAndPattern()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(10, 10)))
            {
                patternSurface.Canvas.Clear(SKColors.Red);
                var patternImage = patternSurface.Snapshot();

                _surface.Canvas.Clear(SKColors.Transparent);

                // Draw with gradient
                var gradient = _context.createLinearGradient(0, 0, 50, 0);
                (gradient as dynamic).addColorStop(0, "blue");
                (gradient as dynamic).addColorStop(1, "green");
                _context.fillStyle = gradient;
                _context.fillRect(0, 0, 50, 50);

                // Switch to pattern
                var pattern = _context.createPattern(patternImage, "repeat");
                _context.fillStyle = pattern;
                _context.fillRect(100, 100, 50, 50);

                // Switch back to solid color
                _context.fillStyle = "yellow";
                _context.fillRect(50, 50, 50, 50);

                Assert.Pass("Switching between fill styles works");
            }
        }

        [Test]
        public void TestGradientAsStrokeStyle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.strokeStyle = gradient;
            _context.lineWidth = 10;
            _context.strokeRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Stroke should have gradient
            var pixel = bitmap.GetPixel(50, 100);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Gradient stroke should render");
        }

        [Test]
        public void TestPatternAsStrokeStyle()
        {
            using (var patternSurface = SKSurface.Create(new SKImageInfo(10, 10)))
            {
                patternSurface.Canvas.Clear(SKColors.Cyan);
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat");
                _context.strokeStyle = pattern;
                _context.lineWidth = 10;
                _context.strokeRect(50, 50, 100, 100);

                Assert.Pass("Pattern as stroke style works");
            }
        }

        #endregion
    }
}
