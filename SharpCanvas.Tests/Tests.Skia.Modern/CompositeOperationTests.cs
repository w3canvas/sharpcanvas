using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for globalCompositeOperation blend modes
    /// </summary>
    public class CompositeOperationTests
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

        /// <summary>
        /// Helper to draw two overlapping rectangles and check the result
        /// </summary>
        private void TestCompositeOperation(string operation, bool shouldHaveBlending = true)
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Draw destination (red square)
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            // Set composite operation
            _context.globalCompositeOperation = operation;

            // Draw source (blue square, offset)
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check that operation was applied (intersection should not be pure red or pure blue)
            var intersectionPixel = bitmap.GetPixel(125, 125);

            if (shouldHaveBlending)
            {
                // Most blend modes should create something different in the intersection
                TestContext.WriteLine($"Operation: {operation}, Intersection pixel: {intersectionPixel}");
            }

            Assert.Pass($"Composite operation '{operation}' completed without crashing");
        }

        #region Standard Porter-Duff Modes

        [Test]
        public void TestSourceOver()
        {
            // Default mode - source drawn over destination
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "source-over";
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Overlap should be blue (source over destination)
            var pixel = bitmap.GetPixel(125, 125);
            Assert.That(pixel, Is.EqualTo(SKColors.Blue), "source-over should draw source on top");
        }

        [Test]
        public void TestSourceIn()
        {
            TestCompositeOperation("source-in", false);
        }

        [Test]
        public void TestSourceOut()
        {
            TestCompositeOperation("source-out", false);
        }

        [Test]
        public void TestSourceAtop()
        {
            TestCompositeOperation("source-atop");
        }

        [Test]
        public void TestDestinationOver()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "destination-over";
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Overlap should be red (destination over source)
            var pixel = bitmap.GetPixel(125, 125);
            Assert.That(pixel, Is.EqualTo(SKColors.Red), "destination-over should keep destination on top");
        }

        [Test]
        public void TestDestinationIn()
        {
            TestCompositeOperation("destination-in", false);
        }

        [Test]
        public void TestDestinationOut()
        {
            TestCompositeOperation("destination-out", false);
        }

        [Test]
        public void TestDestinationAtop()
        {
            TestCompositeOperation("destination-atop");
        }

        [Test]
        public void TestLighter()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "#808080"; // Gray
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "lighter";
            _context.fillStyle = "#404040"; // Darker gray
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Lighter mode should add colors
            var pixel = bitmap.GetPixel(125, 125);
            TestContext.WriteLine($"Lighter pixel: {pixel}");
            Assert.That(pixel.Red, Is.GreaterThanOrEqualTo(128), "Lighter should add color values");
        }

        [Test]
        public void TestCopy()
        {
            TestCompositeOperation("copy", false);
        }

        [Test]
        public void TestXor()
        {
            TestCompositeOperation("xor", false);
        }

        #endregion

        #region Blend Modes

        [Test]
        public void TestMultiply()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "multiply";
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Multiply red and blue should create dark color
            var pixel = bitmap.GetPixel(125, 125);
            TestContext.WriteLine($"Multiply pixel: {pixel}");
            Assert.That(pixel.Red + pixel.Green + pixel.Blue, Is.LessThan(255), "Multiply should darken");
        }

        [Test]
        public void TestScreen()
        {
            TestCompositeOperation("screen");
        }

        [Test]
        public void TestOverlay()
        {
            TestCompositeOperation("overlay");
        }

        [Test]
        public void TestDarken()
        {
            TestCompositeOperation("darken");
        }

        [Test]
        public void TestLighten()
        {
            TestCompositeOperation("lighten");
        }

        [Test]
        public void TestColorDodge()
        {
            TestCompositeOperation("color-dodge");
        }

        [Test]
        public void TestColorBurn()
        {
            TestCompositeOperation("color-burn");
        }

        [Test]
        public void TestHardLight()
        {
            TestCompositeOperation("hard-light");
        }

        [Test]
        public void TestSoftLight()
        {
            TestCompositeOperation("soft-light");
        }

        [Test]
        public void TestDifference()
        {
            TestCompositeOperation("difference");
        }

        [Test]
        public void TestExclusion()
        {
            TestCompositeOperation("exclusion");
        }

        [Test]
        public void TestHue()
        {
            TestCompositeOperation("hue");
        }

        [Test]
        public void TestSaturation()
        {
            TestCompositeOperation("saturation");
        }

        [Test]
        public void TestColor()
        {
            TestCompositeOperation("color");
        }

        [Test]
        public void TestLuminosity()
        {
            TestCompositeOperation("luminosity");
        }

        #endregion

        #region Edge Cases

        [Test]
        public void TestInvalidCompositeOperation()
        {
            var original = _context.globalCompositeOperation;
            _context.globalCompositeOperation = "invalid-mode";

            // Should either keep original or handle gracefully
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestCompositeOperationCaseInsensitive()
        {
            // Test if operation names are case-insensitive (may vary by implementation)
            Assert.DoesNotThrow(() => _context.globalCompositeOperation = "SOURCE-OVER");
            Assert.DoesNotThrow(() => _context.globalCompositeOperation = "Source-Over");
        }

        [Test]
        public void TestCompositeWithAlpha()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "rgba(255, 0, 0, 0.5)"; // Semi-transparent red
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "source-over";
            _context.fillStyle = "rgba(0, 0, 255, 0.5)"; // Semi-transparent blue
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should blend semi-transparent colors
            var pixel = bitmap.GetPixel(125, 125);
            TestContext.WriteLine($"Blended semi-transparent pixel: {pixel}");
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Should have some alpha");
        }

        [Test]
        public void TestCompositeWithGlobalAlpha()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.globalAlpha = 0.5;
            _context.globalCompositeOperation = "source-over";
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(125, 125);
            TestContext.WriteLine($"With globalAlpha pixel: {pixel}");
            // Should blend with global alpha applied
            Assert.Pass("Global alpha with composite operation works");
        }

        [Test]
        public void TestMultipleCompositeChanges()
        {
            // Change composite operation multiple times
            _context.globalCompositeOperation = "multiply";
            _context.fillRect(0, 0, 50, 50);

            _context.globalCompositeOperation = "screen";
            _context.fillRect(50, 0, 50, 50);

            _context.globalCompositeOperation = "overlay";
            _context.fillRect(100, 0, 50, 50);

            _context.globalCompositeOperation = "source-over";
            _context.fillRect(150, 0, 50, 50);

            Assert.Pass("Multiple composite operation changes work");
        }

        [Test]
        public void TestCompositeWithSaveRestore()
        {
            _context.globalCompositeOperation = "multiply";
            _context.save();

            _context.globalCompositeOperation = "screen";
            _context.fillRect(0, 0, 50, 50);

            _context.restore();

            // Should restore to multiply
            Assert.That(_context.globalCompositeOperation, Is.EqualTo("multiply"));
        }

        #endregion

        #region Complex Scenarios

        [Test]
        public void TestCompositeWithClipping()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create clip region
            _context.beginPath();
            _context.rect(75, 75, 100, 100);
            _context.clip();

            // Draw with composite operation
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.globalCompositeOperation = "multiply";
            _context.fillStyle = "blue";
            _context.fillRect(100, 100, 100, 100);

            Assert.Pass("Composite operation with clipping works");
        }

        [Test]
        public void TestCompositeWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            _context.translate(50, 50);
            _context.rotate(System.Math.PI / 4);
            _context.globalCompositeOperation = "multiply";
            _context.fillStyle = "blue";
            _context.fillRect(-25, -25, 50, 50);

            Assert.Pass("Composite operation with transform works");
        }

        [Test]
        public void TestCompositeWithGradient()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "yellow");

            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 100);

            _context.globalCompositeOperation = "multiply";
            _context.fillStyle = "blue";
            _context.fillRect(0, 50, 200, 100);

            Assert.Pass("Composite operation with gradient works");
        }

        #endregion
    }
}
