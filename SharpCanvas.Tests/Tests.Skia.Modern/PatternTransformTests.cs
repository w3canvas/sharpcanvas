using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    public class PatternTransformTests
    {
        private SKSurface _surface;
        private SkiaCanvasRenderingContext2DBase _context;
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
            // We cast to SkiaCanvasRenderingContext2DBase to access Skia-specific methods if needed,
            // but we are testing public API mostly.
            _context = new SkiaCanvasRenderingContext2DBaseTestWrapper(_surface, _document);
        }

        [TearDown]
        public void Teardown()
        {
            _surface.Dispose();
        }

        // Wrapper to instantiate abstract class
        private class SkiaCanvasRenderingContext2DBaseTestWrapper : SkiaCanvasRenderingContext2DBase
        {
            public SkiaCanvasRenderingContext2DBaseTestWrapper(SKSurface surface, IDocument document) : base(surface, document) { }
        }

        [Test]
        public void TestPatternSetTransformTranslate()
        {
            // Create a 20x20 pattern: Left half Red, Right half Blue
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.Red);
                using (var paint = new SKPaint { Color = SKColors.Blue })
                {
                    patternSurface.Canvas.DrawRect(10, 0, 10, 20, paint);
                }
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat");

                // Original: 0-10 Red, 10-20 Blue.
                // Translate pattern by 10px on X.
                // Pattern origin becomes x=10.
                // So at x=10, we see the start of pattern (Red).
                // At x=0, we see the end of previous repeat (Blue).

                var matrix = new DOMMatrix();
                matrix.e = 10; // Translate X
                ((dynamic)pattern).setTransform(matrix);

                _context.fillStyle = pattern;
                _context.fillRect(0, 0, 40, 20);

                var bitmap = new SKBitmap(_surface.PeekPixels().Info);
                _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Check pixel at x=5 (should be Blue)
                var pixel1 = bitmap.GetPixel(5, 10);
                Assert.That(pixel1.Blue, Is.GreaterThan(200), "Pixel at 5,10 should be Blue (shifted)");
                Assert.That(pixel1.Red, Is.LessThan(50), "Pixel at 5,10 should not be Red");

                // Check pixel at x=15 (should be Red)
                var pixel2 = bitmap.GetPixel(15, 10);
                Assert.That(pixel2.Red, Is.GreaterThan(200), "Pixel at 15,10 should be Red (shifted origin)");
                Assert.That(pixel2.Blue, Is.LessThan(50), "Pixel at 15,10 should not be Blue");
            }
        }

        [Test]
        public void TestPatternSetTransformScale()
        {
            // Create a 20x20 pattern: Left half Red, Right half Blue
            using (var patternSurface = SKSurface.Create(new SKImageInfo(20, 20)))
            {
                patternSurface.Canvas.Clear(SKColors.Red);
                using (var paint = new SKPaint { Color = SKColors.Blue })
                {
                    patternSurface.Canvas.DrawRect(10, 0, 10, 20, paint);
                }
                var patternImage = patternSurface.Snapshot();

                var pattern = _context.createPattern(patternImage, "repeat");

                // Scale pattern by 2.0.
                // Original: Red 0-10, Blue 10-20.
                // Scaled: Red 0-20, Blue 20-40.

                var matrix = new DOMMatrix();
                matrix.a = 2.0; // Scale X
                matrix.d = 2.0; // Scale Y
                ((dynamic)pattern).setTransform(matrix);

                _context.fillStyle = pattern;
                _context.fillRect(0, 0, 60, 20);

                var bitmap = new SKBitmap(_surface.PeekPixels().Info);
                _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

                // Pixel at 15,10. Original 15 was Blue. Scaled, 15 is still in first half (0-20), so Red.
                var pixel1 = bitmap.GetPixel(15, 10);
                Assert.That(pixel1.Red, Is.GreaterThan(200), "Pixel at 15,10 should be Red (scaled up)");

                // Pixel at 25,10. Scaled, 25 is in second half (20-40), so Blue.
                var pixel2 = bitmap.GetPixel(25, 10);
                Assert.That(pixel2.Blue, Is.GreaterThan(200), "Pixel at 25,10 should be Blue");
            }
        }
    }
}
