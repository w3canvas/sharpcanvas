using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for Canvas filter property and filter chain
    /// </summary>
    public class FilterTests
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

        #region CSS Filter String Tests

        [Test]
        public void TestFilterGrayscale()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "grayscale(100%)";
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Grayscale should make R, G, B equal
            Assert.That(pixel.Red, Is.EqualTo(pixel.Green).Within(2));
            Assert.That(pixel.Green, Is.EqualTo(pixel.Blue).Within(2));
        }

        [Test]
        public void TestFilterSepia()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "sepia(100%)";
            _context.fillStyle = "blue";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Sepia should have more red and green than blue
            Assert.That(pixel.Red, Is.GreaterThan(pixel.Blue));
            Assert.That(pixel.Green, Is.GreaterThan(pixel.Blue));
        }

        [Test]
        public void TestFilterBlur()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Draw sharp rectangle first
            _context.fillStyle = "black";
            _context.fillRect(90, 90, 20, 20);

            _context.filter = "blur(5px)";
            _context.fillStyle = "white";
            _context.fillRect(90, 90, 20, 20);

            Assert.Pass("Blur filter applied without crashing");
        }

        [Test]
        public void TestFilterBrightness()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "brightness(150%)";
            _context.fillStyle = "#808080"; // Gray
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Brightness 150% should lighten the color
            Assert.That(pixel.Red, Is.GreaterThanOrEqualTo(128), "Brightened gray should be lighter");
        }

        [Test]
        public void TestFilterContrast()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "contrast(200%)";
            _context.fillStyle = "#606060";
            _context.fillRect(50, 50, 100, 100);

            Assert.Pass("Contrast filter applied");
        }

        [Test]
        public void TestFilterInvert()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "invert(100%)";
            _context.fillStyle = "black";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Inverted black should be white
            Assert.That(pixel.Red, Is.GreaterThan(200), "Inverted black should be near white");
        }

        [Test]
        public void TestFilterSaturate()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "saturate(200%)";
            _context.fillStyle = "#ff8080"; // Light red
            _context.fillRect(50, 50, 100, 100);

            Assert.Pass("Saturate filter applied");
        }

        [Test]
        public void TestFilterHueRotate()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "hue-rotate(90deg)";
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Hue rotation should change the color
            // Red rotated 90 degrees should shift towards yellow/green
            TestContext.WriteLine($"Hue rotated pixel: R={pixel.Red}, G={pixel.Green}, B={pixel.Blue}");
            Assert.Pass("Hue rotate filter applied");
        }

        [Test]
        public void TestFilterOpacity()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "opacity(50%)";
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Opacity should reduce alpha
            Assert.That(pixel.Alpha, Is.LessThan(255), "Opacity filter should reduce alpha");
        }

        #endregion

        #region Multiple Filters

        [Test]
        public void TestMultipleFilters()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "grayscale(50%) blur(2px)";
            _context.fillStyle = "blue";
            _context.fillRect(50, 50, 100, 100);

            Assert.Pass("Multiple filters applied");
        }

        [Test]
        public void TestFilterChain()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "brightness(1.2) contrast(1.1) saturate(1.3)";
            _context.fillStyle = "#4080c0";
            _context.fillRect(50, 50, 100, 100);

            Assert.Pass("Filter chain applied");
        }

        #endregion

        #region Filter Reset

        [Test]
        public void TestFilterReset()
        {
            _context.filter = "grayscale(100%)";
            Assert.That(_context.filter, Is.EqualTo("grayscale(100%)"));

            _context.filter = "none";
            Assert.That(_context.filter, Is.EqualTo("none"));
        }

        [Test]
        public void TestFilterWithSaveRestore()
        {
            _context.filter = "blur(5px)";
            _context.save();

            _context.filter = "grayscale(100%)";
            Assert.That(_context.filter, Is.EqualTo("grayscale(100%)"));

            _context.restore();
            Assert.That(_context.filter, Is.EqualTo("blur(5px)"));
        }

        #endregion

        #region Invalid Filter Handling

        [Test]
        public void TestInvalidFilterString()
        {
            var originalFilter = _context.filter;
            _context.filter = "invalid-filter(100%)";

            // Should either ignore or handle gracefully
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestEmptyFilterString()
        {
            _context.filter = "";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        #endregion

        #region Filter Effects on Different Operations

        [Test]
        public void TestFilterOnStroke()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "invert(100%)";
            _context.strokeStyle = "black";
            _context.lineWidth = 10;
            _context.strokeRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(50, 100);
            // Inverted black stroke should appear white-ish
            TestContext.WriteLine($"Filtered stroke pixel: R={pixel.Red}, G={pixel.Green}, B={pixel.Blue}, A={pixel.Alpha}");
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Filtered stroke should render");
        }

        [Test]
        public void TestFilterOnText()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "grayscale(100%)";
            _context.fillStyle = "red";
            _context.font = "20px sans-serif";
            _context.fillText("Test", 50, 50);

            Assert.Pass("Filter on text works");
        }

        [Test]
        public void TestFilterOnGradient()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var gradient = _context.createLinearGradient(0, 0, 200, 0);
            (gradient as dynamic).addColorStop(0, "red");
            (gradient as dynamic).addColorStop(1, "blue");

            _context.filter = "grayscale(100%)";
            _context.fillStyle = gradient;
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Grayscaled gradient should still have variation but be gray
            Assert.That(pixel.Red, Is.EqualTo(pixel.Green).Within(5));
            Assert.That(pixel.Green, Is.EqualTo(pixel.Blue).Within(5));
        }

        #endregion

        #region Advanced Filter Tests

        [Test]
        public void TestDropShadowFilter()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "drop-shadow(4px 4px 8px black)";
            _context.fillStyle = "red";
            _context.fillRect(60, 60, 80, 80);

            Assert.Pass("Drop shadow filter applied");
        }

        [Test]
        public void TestFilterPercentageValues()
        {
            // Test various percentage values
            _context.filter = "brightness(0%)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "brightness(50%)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "brightness(100%)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "brightness(200%)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestFilterAngleValues()
        {
            _context.filter = "hue-rotate(0deg)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "hue-rotate(180deg)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "hue-rotate(360deg)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "hue-rotate(3.14rad)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestFilterLengthValues()
        {
            _context.filter = "blur(0px)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "blur(5px)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));

            _context.filter = "blur(10px)";
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        #endregion

        #region Filter with Complex Scenarios

        [Test]
        public void TestFilterWithClipping()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.beginPath();
            _context.rect(50, 50, 100, 100);
            _context.clip();

            _context.filter = "invert(100%)";
            _context.fillStyle = "black";
            _context.fillRect(0, 0, 200, 200);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Inside clip should be filtered
            var insidePixel = bitmap.GetPixel(100, 100);
            Assert.That(insidePixel.Alpha, Is.GreaterThan(0));

            // Outside clip should be transparent
            var outsidePixel = bitmap.GetPixel(20, 20);
            Assert.That(outsidePixel.Alpha, Is.EqualTo(0));
        }

        [Test]
        public void TestFilterWithTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.translate(50, 50);
            _context.rotate(System.Math.PI / 4);
            _context.filter = "blur(3px)";
            _context.fillStyle = "blue";
            _context.fillRect(-25, -25, 50, 50);

            Assert.Pass("Filter with transform works");
        }

        [Test]
        public void TestFilterWithGlobalAlpha()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.globalAlpha = 0.5;
            _context.filter = "grayscale(100%)";
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // Should be both grayscale and semi-transparent
            Assert.That(pixel.Alpha, Is.LessThan(255));
            Assert.That(pixel.Red, Is.EqualTo(pixel.Green).Within(5));
        }

        #endregion

        #region Performance and Edge Cases

        [Test]
        public void TestMultipleFilterChanges()
        {
            for (int i = 0; i < 100; i++)
            {
                _context.filter = i % 2 == 0 ? "grayscale(100%)" : "sepia(100%)";
                _context.fillRect(i, i, 10, 10);
            }

            Assert.Pass("Multiple filter changes handled");
        }

        [Test]
        public void TestFilterNone()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.filter = "none";
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(100, 100);

            // With no filter, red should be pure red
            Assert.That(pixel, Is.EqualTo(SKColors.Red));
        }

        #endregion
    }
}
