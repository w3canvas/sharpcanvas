using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
namespace SharpCanvas.Tests.Skia.Modern
{
    public class SkiaModernContextTests
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
        public void TestGetContextAttributes()
        {
            var attributes = _context.getContextAttributes() as IContextAttributes;
            Assert.That(attributes, Is.Not.Null);
            Assert.That(attributes.alpha, Is.True);
            Assert.That(attributes.colorSpace, Is.EqualTo("srgb"));
            Assert.That(attributes.desynchronized, Is.False);
            Assert.That(attributes.willReadFrequently, Is.False);
        }

        [Test]
        public void TestArc()
        {
            // Part 1: Clockwise arc (bottom semi-circle)
            _context.reset();
            _context.fillStyle = "black";
            _context.beginPath();
            _context.arc(50, 50, 25, 0, System.Math.PI, false);
            _context.closePath();
            _context.fill();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            bool blackPixelFound = false;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y) == SKColors.Black)
                    {
                        blackPixelFound = true;
                        break;
                    }
                }
                if (blackPixelFound)
                {
                    break;
                }
            }
            Assert.That(blackPixelFound, Is.True, "Expected to find at least one black pixel in the clockwise arc.");

            // Part 2: Anticlockwise arc (top semi-circle)
            _context.reset();
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.fillStyle = "black";
            _context.beginPath();
            _context.arc(50, 50, 25, System.Math.PI, 0, true);
            _context.closePath();
            _context.fill();

            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            blackPixelFound = false;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y) == SKColors.Black)
                    {
                        blackPixelFound = true;
                        break;
                    }
                }
                if (blackPixelFound)
                {
                    break;
                }
            }
            Assert.That(blackPixelFound, Is.True, "Expected to find at least one black pixel in the anticlockwise arc.");
        }

        [Test]
        public void TestEllipse()
        {
            // Clear the canvas
            _surface.Canvas.Clear(SKColors.Transparent);

            // Set fill style
            _context.fillStyle = "black";

            // Draw the ellipse
            _context.beginPath();
            _context.ellipse(50, 50, 25, 40, 0, 0, 2 * System.Math.PI, false);
            _context.fill();

            // Read the pixels
            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check a pixel in the center of the ellipse
            var pixel = bitmap.GetPixel(50, 50);
            Assert.That(pixel, Is.EqualTo(SKColors.Black));
        }

        [Test]
        public void TestRoundRect()
        {
            // Clear the canvas
            _surface.Canvas.Clear(SKColors.Transparent);

            // Set stroke style
            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            // Draw the rounded rectangle
            _context.beginPath();
            _context.roundRect(10, 10, 80, 80, 10);
            _context.stroke();

            // Read the pixels
            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check a pixel on one of the straight edges
            var pixel = bitmap.GetPixel(50, 10);
            Assert.That(pixel, Is.EqualTo(SKColors.Black));
        }

        [Test]
        public void TestLineDash()
        {
            var dash = new double[] { 5, 5 };
            _context.setLineDash(dash);
            Assert.That(_context.getLineDash(), Is.EqualTo(dash));

            // Clear the canvas
            _surface.Canvas.Clear(SKColors.Transparent);

            // Set stroke style
            _context.strokeStyle = "black";
            _context.lineWidth = 2;

            // Draw a dashed line
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(90, 10);
            _context.stroke();

            // Read the pixels
            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check a pixel in the first dash
            var pixel1 = bitmap.GetPixel(12, 10);
            Assert.That(pixel1, Is.EqualTo(SKColors.Black));

            // Check a pixel in the first gap
            var pixel2 = bitmap.GetPixel(17, 10);
            Assert.That(pixel2, Is.EqualTo(new SKColor(0, 0, 0, 0)));
        }

        [Test]
        public void TestCreateConicGradient()
        {
            var gradient = _context.createConicGradient(0, 50, 50);
            Assert.That(gradient, Is.Not.Null);
        }

        [Test]
        public void TestIsPointInStroke()
        {
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(90, 10);
            Assert.That(_context.isPointInStroke(50, 10), Is.True);
            Assert.That(_context.isPointInStroke(50, 20), Is.False);
        }

        [Test]
        public void TestGetAndResetTransform()
        {
            _context.translate(10, 10);
            var transform = _context.getTransform() as DOMMatrix;
            Assert.That(transform, Is.Not.Null);
            Assert.That(transform.e, Is.EqualTo(10));
            Assert.That(transform.f, Is.EqualTo(10));

            _context.resetTransform();
            transform = _context.getTransform() as DOMMatrix;
            Assert.That(transform, Is.Not.Null);
            Assert.That(transform.e, Is.EqualTo(0));
            Assert.That(transform.f, Is.EqualTo(0));
        }

        [Test]
        public void TestFilter()
        {
            // Test grayscale
            _context.reset();
            _context.fillStyle = "red";
            _context.filter = "grayscale(100%)";
            _context.fillRect(0, 0, 10, 10);
            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
            var pixel = bitmap.GetPixel(5, 5);
            // Grayscale of red should be a shade of gray
            Assert.That(pixel.Red, Is.EqualTo(pixel.Green).Within(1));
            Assert.That(pixel.Green, Is.EqualTo(pixel.Blue).Within(1));

            // Test sepia
            _context.reset();
            _context.fillStyle = "blue";
            _context.filter = "sepia(100%)";
            _context.fillRect(0, 0, 10, 10);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);
            pixel = bitmap.GetPixel(5, 5);
            // Sepia of blue should have more red and green than blue
            Assert.That(pixel.Red, Is.GreaterThan(pixel.Blue));
            Assert.That(pixel.Green, Is.GreaterThan(pixel.Blue));
        }

        [Test]
        public void TestReset()
        {
            // Change properties from their defaults
            _context.fillStyle = "red";
            _context.globalAlpha = 0.5;
            _context.font = "20px Arial";
            _context.lineWidth = 5;
            _context.setLineDash(new double[] { 5, 5 });
            _context.lineDashOffset = 2;
            _context.translate(10, 10);

            // Reset the context
            _context.reset();

            // Assert that properties are reset to their defaults
            Assert.That(_context.fillStyle, Is.EqualTo("#000000"));
            Assert.That(_context.globalAlpha, Is.EqualTo(1.0));
            Assert.That(_context.font, Is.EqualTo("10px sans-serif"));
            Assert.That(_context.lineWidth, Is.EqualTo(1.0));
            Assert.That(_context.getLineDash(), Is.Empty);
            Assert.That(_context.lineDashOffset, Is.EqualTo(0.0));

            // Check that the transform is reset
            var transform = _context.getTransform() as DOMMatrix;
            Assert.That(transform, Is.Not.Null);
            Assert.That(transform.a, Is.EqualTo(1));
            Assert.That(transform.b, Is.EqualTo(0));
            Assert.That(transform.c, Is.EqualTo(0));
            Assert.That(transform.d, Is.EqualTo(1));
            Assert.That(transform.e, Is.EqualTo(0));
            Assert.That(transform.f, Is.EqualTo(0));
        }

        [Test]
        public void TestTextAlign()
        {
            // Default direction is "ltr"
            _context.textAlign = "start";
            Assert.That(_context.textAlign, Is.EqualTo("start"));
            // In LTR, "start" should be "left"
            // We can't directly check the internal SKTextAlign, so we'll have to trust the implementation for now.
            // We can, however, check that the property is set correctly.

            _context.textAlign = "end";
            Assert.That(_context.textAlign, Is.EqualTo("end"));
            // In LTR, "end" should be "right"

            // Change direction to "rtl"
            _context.direction = "rtl";
            _context.textAlign = "start";
            Assert.That(_context.textAlign, Is.EqualTo("start"));
            // In RTL, "start" should be "right"

            _context.textAlign = "end";
            Assert.That(_context.textAlign, Is.EqualTo("end"));
            // In RTL, "end" should be "left"
        }
    }
}
