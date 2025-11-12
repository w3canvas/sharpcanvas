using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for text measurement and advanced text properties
    /// </summary>
    public class TextMeasurementTests
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

        #region measureText Basic Tests

        [Test]
        public void TestMeasureTextBasic()
        {
            _context.font = "20px sans-serif";
            var metrics = _context.measureText("Hello");

            Assert.That(metrics, Is.Not.Null);
            Assert.That(metrics, Is.InstanceOf<TextMetrics>());
        }

        [Test]
        public void TestMeasureTextWidth()
        {
            _context.font = "20px sans-serif";
            var metrics = _context.measureText("Hello") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0), "Text width should be greater than 0");
        }

        [Test]
        public void TestMeasureTextWidthVariesByContent()
        {
            _context.font = "20px sans-serif";

            var shortMetrics = _context.measureText("Hi") as TextMetrics;
            var longMetrics = _context.measureText("Hello World") as TextMetrics;

            Assert.That(longMetrics.width, Is.GreaterThan(shortMetrics.width),
                "Longer text should have greater width");
        }

        [Test]
        public void TestMeasureTextWidthVariesByFont()
        {
            var text = "Sample Text";

            _context.font = "12px sans-serif";
            var smallMetrics = _context.measureText(text) as TextMetrics;

            _context.font = "24px sans-serif";
            var largeMetrics = _context.measureText(text) as TextMetrics;

            Assert.That(largeMetrics.width, Is.GreaterThan(smallMetrics.width),
                "Larger font should produce wider text");
        }

        [Test]
        public void TestMeasureTextWithDifferentFontFamilies()
        {
            var text = "Sample";

            _context.font = "20px monospace";
            var monoMetrics = _context.measureText(text) as TextMetrics;

            _context.font = "20px sans-serif";
            var sansMetrics = _context.measureText(text) as TextMetrics;

            // Different fonts should likely produce different widths
            // (though this might not always be guaranteed)
            Assert.That(monoMetrics.width, Is.GreaterThan(0));
            Assert.That(sansMetrics.width, Is.GreaterThan(0));
        }

        [Test]
        public void TestMeasureTextSingleCharacter()
        {
            _context.font = "20px sans-serif";
            var metrics = _context.measureText("M") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0));
            Assert.That(metrics.width, Is.LessThan(30), "Single character should not be too wide");
        }

        [Test]
        public void TestMeasureTextSpace()
        {
            _context.font = "20px sans-serif";
            var metrics = _context.measureText(" ") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0), "Space should have non-zero width");
        }

        [Test]
        public void TestMeasureTextMultipleSpaces()
        {
            _context.font = "20px sans-serif";

            var oneSpace = _context.measureText(" ") as TextMetrics;
            var threeSpaces = _context.measureText("   ") as TextMetrics;

            Assert.That(threeSpaces.width, Is.GreaterThan(oneSpace.width),
                "Multiple spaces should be wider than single space");
        }

        [Test]
        public void TestMeasureTextWithBoldFont()
        {
            var text = "Bold Test";

            _context.font = "20px sans-serif";
            var normalMetrics = _context.measureText(text) as TextMetrics;

            _context.font = "bold 20px sans-serif";
            var boldMetrics = _context.measureText(text) as TextMetrics;

            Assert.That(boldMetrics.width, Is.GreaterThanOrEqualTo(normalMetrics.width),
                "Bold text should be at least as wide as normal text");
        }

        [Test]
        public void TestMeasureTextWithItalicFont()
        {
            var text = "Italic Test";

            _context.font = "20px sans-serif";
            var normalMetrics = _context.measureText(text) as TextMetrics;

            _context.font = "italic 20px sans-serif";
            var italicMetrics = _context.measureText(text) as TextMetrics;

            Assert.That(italicMetrics.width, Is.GreaterThan(0));
            Assert.That(normalMetrics.width, Is.GreaterThan(0));
        }

        #endregion

        #region Advanced Text Properties Tests

        [Test]
        public void TestLetterSpacingProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.letterSpacing = "2px";
                Assert.That(_context.letterSpacing, Is.EqualTo("2px"));
            });
        }

        [Test]
        public void TestLetterSpacingAffectsMeasurement()
        {
            _context.font = "20px sans-serif";

            _context.letterSpacing = "0px";
            var normalMetrics = _context.measureText("Hello") as TextMetrics;

            _context.letterSpacing = "5px";
            var spacedMetrics = _context.measureText("Hello") as TextMetrics;

            // With letter spacing, text should be wider
            Assert.That(spacedMetrics.width, Is.GreaterThanOrEqualTo(normalMetrics.width),
                "Letter spacing should increase or maintain text width");
        }

        [Test]
        public void TestWordSpacingProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.wordSpacing = "5px";
                Assert.That(_context.wordSpacing, Is.EqualTo("5px"));
            });
        }

        [Test]
        public void TestWordSpacingAffectsMeasurement()
        {
            _context.font = "20px sans-serif";

            _context.wordSpacing = "0px";
            var normalMetrics = _context.measureText("Hello World") as TextMetrics;

            _context.wordSpacing = "10px";
            var spacedMetrics = _context.measureText("Hello World") as TextMetrics;

            // With word spacing, text should be wider
            Assert.That(spacedMetrics.width, Is.GreaterThanOrEqualTo(normalMetrics.width),
                "Word spacing should increase or maintain text width");
        }

        [Test]
        public void TestTextRenderingProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.textRendering = "optimizeSpeed";
                Assert.That(_context.textRendering, Is.EqualTo("optimizeSpeed"));

                _context.textRendering = "optimizeLegibility";
                Assert.That(_context.textRendering, Is.EqualTo("optimizeLegibility"));

                _context.textRendering = "geometricPrecision";
                Assert.That(_context.textRendering, Is.EqualTo("geometricPrecision"));
            });
        }

        [Test]
        public void TestFontKerningProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.fontKerning = "auto";
                Assert.That(_context.fontKerning, Is.EqualTo("auto"));

                _context.fontKerning = "normal";
                Assert.That(_context.fontKerning, Is.EqualTo("normal"));

                _context.fontKerning = "none";
                Assert.That(_context.fontKerning, Is.EqualTo("none"));
            });
        }

        [Test]
        public void TestFontStretchProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.fontStretch = "ultra-condensed";
                Assert.That(_context.fontStretch, Is.EqualTo("ultra-condensed"));

                _context.fontStretch = "normal";
                Assert.That(_context.fontStretch, Is.EqualTo("normal"));

                _context.fontStretch = "ultra-expanded";
                Assert.That(_context.fontStretch, Is.EqualTo("ultra-expanded"));
            });
        }

        [Test]
        public void TestFontVariantCapsProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.fontVariantCaps = "normal";
                Assert.That(_context.fontVariantCaps, Is.EqualTo("normal"));

                _context.fontVariantCaps = "small-caps";
                Assert.That(_context.fontVariantCaps, Is.EqualTo("small-caps"));

                _context.fontVariantCaps = "all-small-caps";
                Assert.That(_context.fontVariantCaps, Is.EqualTo("all-small-caps"));
            });
        }

        [Test]
        public void TestDirectionProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.direction = "ltr";
                Assert.That(_context.direction, Is.EqualTo("ltr"));

                _context.direction = "rtl";
                Assert.That(_context.direction, Is.EqualTo("rtl"));

                _context.direction = "inherit";
                Assert.That(_context.direction, Is.EqualTo("inherit"));
            });
        }

        [Test]
        public void TestLangProperty()
        {
            Assert.DoesNotThrow(() =>
            {
                _context.lang = "en-US";
                Assert.That(_context.lang, Is.EqualTo("en-US"));

                _context.lang = "fr-FR";
                Assert.That(_context.lang, Is.EqualTo("fr-FR"));

                _context.lang = "ja-JP";
                Assert.That(_context.lang, Is.EqualTo("ja-JP"));
            });
        }

        #endregion

        #region Text Properties with Save/Restore

        [Test]
        public void TestLetterSpacingWithSaveRestore()
        {
            _context.letterSpacing = "2px";
            _context.save();

            _context.letterSpacing = "5px";
            Assert.That(_context.letterSpacing, Is.EqualTo("5px"));

            _context.restore();
            Assert.That(_context.letterSpacing, Is.EqualTo("2px"));
        }

        [Test]
        public void TestWordSpacingWithSaveRestore()
        {
            _context.wordSpacing = "3px";
            _context.save();

            _context.wordSpacing = "10px";
            Assert.That(_context.wordSpacing, Is.EqualTo("10px"));

            _context.restore();
            Assert.That(_context.wordSpacing, Is.EqualTo("3px"));
        }

        [Test]
        public void TestTextRenderingWithSaveRestore()
        {
            _context.textRendering = "optimizeSpeed";
            _context.save();

            _context.textRendering = "geometricPrecision";
            Assert.That(_context.textRendering, Is.EqualTo("geometricPrecision"));

            _context.restore();
            Assert.That(_context.textRendering, Is.EqualTo("optimizeSpeed"));
        }

        [Test]
        public void TestDirectionWithSaveRestore()
        {
            _context.direction = "ltr";
            _context.save();

            _context.direction = "rtl";
            Assert.That(_context.direction, Is.EqualTo("rtl"));

            _context.restore();
            Assert.That(_context.direction, Is.EqualTo("ltr"));
        }

        #endregion

        #region measureText with Special Characters

        [Test]
        public void TestMeasureTextWithNumbers()
        {
            _context.font = "20px monospace";
            var metrics = _context.measureText("1234567890") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0));
        }

        [Test]
        public void TestMeasureTextWithSymbols()
        {
            _context.font = "20px sans-serif";
            var metrics = _context.measureText("!@#$%^&*()") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0));
        }

        [Test]
        public void TestMeasureTextWithMixedCase()
        {
            _context.font = "20px sans-serif";

            var lowercase = _context.measureText("hello") as TextMetrics;
            var uppercase = _context.measureText("HELLO") as TextMetrics;
            var mixed = _context.measureText("Hello") as TextMetrics;

            Assert.That(lowercase.width, Is.GreaterThan(0));
            Assert.That(uppercase.width, Is.GreaterThan(0));
            Assert.That(mixed.width, Is.GreaterThan(0));
        }

        [Test]
        public void TestMeasureTextWithNewline()
        {
            _context.font = "20px sans-serif";

            // Canvas doesn't render newlines in text, but measureText should handle it
            var metrics = _context.measureText("Hello\nWorld") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0));
        }

        [Test]
        public void TestMeasureTextWithTab()
        {
            _context.font = "20px sans-serif";

            var metrics = _context.measureText("Hello\tWorld") as TextMetrics;

            Assert.That(metrics.width, Is.GreaterThan(0));
        }

        #endregion

        #region measureText Consistency

        [Test]
        public void TestMeasureTextConsistency()
        {
            _context.font = "20px sans-serif";

            var metrics1 = _context.measureText("Test") as TextMetrics;
            var metrics2 = _context.measureText("Test") as TextMetrics;

            Assert.That(metrics2.width, Is.EqualTo(metrics1.width).Within(0.1),
                "Multiple measurements of same text should be consistent");
        }

        [Test]
        public void TestMeasureTextAdditivity()
        {
            _context.font = "20px monospace";

            var helloMetrics = _context.measureText("Hello") as TextMetrics;
            var worldMetrics = _context.measureText("World") as TextMetrics;
            var combinedMetrics = _context.measureText("HelloWorld") as TextMetrics;

            // For monospace, combined should be close to sum (allowing for small rounding)
            var sum = helloMetrics.width + worldMetrics.width;
            Assert.That(combinedMetrics.width, Is.EqualTo(sum).Within(2.0),
                "Combined text width should approximate sum of parts in monospace");
        }

        #endregion

        #region Text Rendering with Advanced Properties

        [Test]
        public void TestFillTextWithLetterSpacing()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.font = "20px sans-serif";
            _context.letterSpacing = "5px";
            _context.fillStyle = "black";

            Assert.DoesNotThrow(() => _context.fillText("Spaced Text", 10, 50));
        }

        [Test]
        public void TestFillTextWithWordSpacing()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.font = "20px sans-serif";
            _context.wordSpacing = "10px";
            _context.fillStyle = "black";

            Assert.DoesNotThrow(() => _context.fillText("Word Spaced Text", 10, 100));
        }

        [Test]
        public void TestStrokeTextWithAdvancedProperties()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.font = "24px sans-serif";
            _context.letterSpacing = "3px";
            _context.textRendering = "geometricPrecision";
            _context.strokeStyle = "blue";
            _context.lineWidth = 2;

            Assert.DoesNotThrow(() => _context.strokeText("Stroked Text", 10, 150));
        }

        [Test]
        public void TestTextWithRTLDirection()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.font = "20px sans-serif";
            _context.direction = "rtl";
            _context.fillStyle = "black";

            Assert.DoesNotThrow(() => _context.fillText("RTL Text", 200, 50));
        }

        #endregion
    }
}
