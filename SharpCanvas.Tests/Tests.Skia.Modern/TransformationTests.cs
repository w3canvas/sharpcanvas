using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for Canvas 2D transformation operations
    /// </summary>
    public class TransformationTests
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
        public void TestTranslate()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.translate(50, 50);
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 10, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Rectangle should be at (50, 50) not (0, 0)
            var pixel = bitmap.GetPixel(55, 55);
            Assert.That(pixel, Is.EqualTo(SKColors.Red), "Translated rectangle should be at (50,50)");

            var originPixel = bitmap.GetPixel(5, 5);
            Assert.That(originPixel.Alpha, Is.EqualTo(0), "Origin should be transparent");
        }

        [Test]
        public void TestScale()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.scale(2, 2);
            _context.fillStyle = "blue";
            _context.fillRect(10, 10, 10, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Rectangle scaled by 2x should now be from (20, 20) to (40, 40)
            var pixel = bitmap.GetPixel(30, 30);
            Assert.That(pixel, Is.EqualTo(SKColors.Blue), "Scaled rectangle should be larger");

            // Original size would end at 20, scaled should extend to 40
            var scaledPixel = bitmap.GetPixel(35, 35);
            Assert.That(scaledPixel, Is.EqualTo(SKColors.Blue), "Scaled rectangle should extend further");
        }

        [Test]
        public void TestScaleNonUniform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.scale(2, 0.5);
            _context.fillStyle = "green";
            _context.fillRect(10, 10, 20, 40);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Width scaled by 2: 20 * 2 = 40, starting at 10 * 2 = 20, ending at 60
            // Height scaled by 0.5: 40 * 0.5 = 20, starting at 10 * 0.5 = 5, ending at 25
            var widthPixel = bitmap.GetPixel(50, 15);
            Assert.That(widthPixel, Is.EqualTo(SKColors.Green), "Width should be scaled by 2");

            var heightPixel = bitmap.GetPixel(30, 20);
            Assert.That(heightPixel, Is.EqualTo(SKColors.Green), "Height should be scaled by 0.5");
        }

        [Test]
        public void TestRotate()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            // Translate to center, then rotate
            _context.translate(100, 100);
            _context.rotate(Math.PI / 4); // 45 degrees
            _context.fillStyle = "orange";
            _context.fillRect(-10, -10, 20, 20);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // After 45 degree rotation, the square's corners should be at the cardinal directions
            var topPixel = bitmap.GetPixel(100, 100 - 14); // ~14 pixels up
            Assert.That(topPixel.Alpha, Is.GreaterThan(0), "Rotated square should extend upward");
        }

        [Test]
        public void TestRotate90Degrees()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.translate(100, 100);
            _context.rotate(Math.PI / 2); // 90 degrees
            _context.fillStyle = "purple";
            _context.fillRect(0, 0, 30, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Rectangle that was horizontal (30x10) should now be vertical (10x30)
            // Starting at (0, 0) in rotated space = (100, 100) in canvas space
            // After 90° rotation, x becomes -y and y becomes x
            var pixel = bitmap.GetPixel(95, 115); // Should be inside rotated rect
            Assert.That(pixel, Is.EqualTo(SKColors.Purple), "90° rotated rectangle should be vertical");
        }

        [Test]
        public void TestTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            // Apply a custom transformation matrix
            // Identity with translation: m11=1, m12=0, m21=0, m22=1, dx=50, dy=50
            _context.transform(1, 0, 0, 1, 50, 50);
            _context.fillStyle = "cyan";
            _context.fillRect(0, 0, 10, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(55, 55);
            Assert.That(pixel, Is.EqualTo(SKColors.Cyan), "Transform should translate");
        }

        [Test]
        public void TestSetTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            // First apply a translation
            _context.translate(100, 100);
            // Then setTransform to override with new absolute transform
            _context.setTransform(2, 0, 0, 2, 20, 20);
            _context.fillStyle = "yellow";
            _context.fillRect(0, 0, 10, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // setTransform should reset the matrix, so the translate(100,100) is ignored
            // The rect at (0,0) size (10,10) with scale 2 and translate (20,20)
            // should appear at (20,20) with size (20,20)
            var pixel = bitmap.GetPixel(30, 30);
            Assert.That(pixel, Is.EqualTo(SKColors.Yellow), "setTransform should replace current transform");

            var oldTransformPixel = bitmap.GetPixel(105, 105);
            Assert.That(oldTransformPixel.Alpha, Is.EqualTo(0), "Old transform should be ignored");
        }

        [Test]
        public void TestGetTransform()
        {
            _context.translate(10, 20);
            _context.scale(2, 3);

            var matrix = _context.getTransform() as DOMMatrix;
            Assert.That(matrix, Is.Not.Null, "getTransform should return a matrix");

            // After scale(2, 3) and translate(10, 20), the matrix should be:
            // [2, 0, 0, 3, 20, 60] because translate is applied first
            // Actually, in canvas, transforms are applied in reverse order
            // So scale(2,3) then translate(10,20) means: scale first, then translate
            // The final matrix: translate by (10*2, 20*3) = (20, 60)
            Assert.That(matrix.a, Is.EqualTo(2).Within(0.001), "Scale X should be 2");
            Assert.That(matrix.d, Is.EqualTo(3).Within(0.001), "Scale Y should be 3");
            Assert.That(matrix.e, Is.EqualTo(20).Within(0.001), "Translation X should be 20");
            Assert.That(matrix.f, Is.EqualTo(60).Within(0.001), "Translation Y should be 60");
        }

        [Test]
        public void TestResetTransform()
        {
            _context.translate(50, 50);
            _context.scale(2, 2);
            _context.rotate(Math.PI / 4);

            _context.resetTransform();

            var matrix = _context.getTransform() as DOMMatrix;
            Assert.That(matrix, Is.Not.Null);
            Assert.That(matrix.a, Is.EqualTo(1).Within(0.001), "Should be identity matrix");
            Assert.That(matrix.b, Is.EqualTo(0).Within(0.001));
            Assert.That(matrix.c, Is.EqualTo(0).Within(0.001));
            Assert.That(matrix.d, Is.EqualTo(1).Within(0.001));
            Assert.That(matrix.e, Is.EqualTo(0).Within(0.001));
            Assert.That(matrix.f, Is.EqualTo(0).Within(0.001));
        }

        [Test]
        public void TestSaveRestoreTransform()
        {
            _context.translate(10, 10);
            _context.save();
            _context.translate(20, 20);

            var matrix1 = _context.getTransform() as DOMMatrix;
            Assert.That(matrix1, Is.Not.Null);
            Assert.That(matrix1!.e, Is.EqualTo(30).Within(0.001), "Combined translation should be 30");

            _context.restore();

            var matrix2 = _context.getTransform() as DOMMatrix;
            Assert.That(matrix2, Is.Not.Null);
            Assert.That(matrix2!.e, Is.EqualTo(10).Within(0.001), "Should restore to original translation");
        }

        [Test]
        public void TestCombinedTransforms()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.translate(100, 100);
            _context.rotate(Math.PI / 4);
            _context.scale(2, 2);
            _context.fillStyle = "magenta";
            _context.fillRect(-5, -5, 10, 10);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should draw a rotated, scaled rectangle at center
            var centerPixel = bitmap.GetPixel(100, 100);
            Assert.That(centerPixel, Is.EqualTo(SKColors.Magenta), "Combined transforms should work");
        }

        [Test]
        public void TestTransformAffectsStrokes()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.scale(2, 2);
            _context.strokeStyle = "red";
            _context.lineWidth = 1;
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(20, 20);
            _context.stroke();

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Line from (10,10) to (20,20) scaled by 2 = (20,20) to (40,40)
            var pixel = bitmap.GetPixel(30, 30);
            Assert.That(pixel.Alpha, Is.GreaterThan(0), "Scaled line should be drawn");
        }

        [Test]
        public void TestNegativeScale()
        {
            _surface.Canvas.Clear(SKColors.Transparent);
            _context.translate(100, 100);
            _context.scale(-1, 1); // Flip horizontally
            _context.fillStyle = "brown";
            _context.fillRect(0, 0, 20, 20);

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Rectangle at (0,0) flipped horizontally should appear to the left of center
            var pixel = bitmap.GetPixel(90, 100);
            Assert.That(pixel, Is.EqualTo(SKColors.Brown), "Negative scale should flip");
        }
    }
}
