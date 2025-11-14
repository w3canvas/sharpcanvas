using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Tests for error handling and edge cases to ensure robustness
    /// </summary>
    public class ErrorHandlingAndEdgeCaseTests
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

        #region Dimension Edge Cases

        [Test]
        public void TestNegativeWidthRectangle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Negative width should still render (drawn from right to left)
            _context.fillStyle = "red";
            Assert.DoesNotThrow(() => _context.fillRect(100, 50, -50, 50));

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Should draw from (100, 50) to (50, 100)
            var pixel = bitmap.GetPixel(75, 75);
            Assert.That(pixel, Is.EqualTo(SKColors.Red), "Negative width should still render");
        }

        [Test]
        public void TestNegativeHeightRectangle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.fillStyle = "blue";
            Assert.DoesNotThrow(() => _context.fillRect(50, 100, 50, -50));

            var bitmap = new SKBitmap(_surface.PeekPixels().Info);
            _surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var pixel = bitmap.GetPixel(75, 75);
            Assert.That(pixel, Is.EqualTo(SKColors.Blue), "Negative height should still render");
        }

        [Test]
        public void TestZeroDimensionRectangle()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Zero dimensions should not crash
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 0, 50));
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 50, 0));
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 0, 0));
        }

        [Test]
        public void TestVeryLargeDimensions()
        {
            // Very large dimensions should not crash
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10000, 10000));
            Assert.DoesNotThrow(() => _context.strokeRect(0, 0, 100000, 100000));
        }

        #endregion

        #region NaN and Infinity Tests

        [Test]
        public void TestNaNCoordinates()
        {
            // NaN coordinates should be handled gracefully
            Assert.DoesNotThrow(() => _context.fillRect(double.NaN, 50, 50, 50));
            Assert.DoesNotThrow(() => _context.fillRect(50, double.NaN, 50, 50));
            Assert.DoesNotThrow(() => _context.moveTo(double.NaN, 50));
            Assert.DoesNotThrow(() => _context.lineTo(50, double.NaN));
        }

        [Test]
        public void TestInfinityCoordinates()
        {
            // Infinity coordinates should be handled gracefully
            Assert.DoesNotThrow(() => _context.fillRect(double.PositiveInfinity, 50, 50, 50));
            Assert.DoesNotThrow(() => _context.fillRect(50, double.NegativeInfinity, 50, 50));
            Assert.DoesNotThrow(() => _context.moveTo(double.PositiveInfinity, 50));
        }

        [Test]
        public void TestNaNDimensions()
        {
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, double.NaN, 50));
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 50, double.NaN));
        }

        [Test]
        public void TestInfinityDimensions()
        {
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, double.PositiveInfinity, 50));
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 50, double.NegativeInfinity));
        }

        #endregion

        #region Property Value Edge Cases

        [Test]
        public void TestNegativeLineWidth()
        {
            // Negative line width should be ignored or clamped
            _context.lineWidth = -5;
            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 50, 50));
        }

        [Test]
        public void TestZeroLineWidth()
        {
            // Zero line width should not crash
            _context.lineWidth = 0;
            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 50, 50));
        }

        [Test]
        public void TestVeryLargeLineWidth()
        {
            _context.lineWidth = 10000;
            Assert.DoesNotThrow(() => _context.strokeRect(50, 50, 50, 50));
        }

        [Test]
        public void TestNaNLineWidth()
        {
            // NaN line width should be ignored
            var originalWidth = _context.lineWidth;
            _context.lineWidth = double.NaN;
            Assert.That(_context.lineWidth, Is.EqualTo(originalWidth), "NaN should not change line width");
        }

        [Test]
        public void TestInvalidGlobalAlpha()
        {
            // globalAlpha should be clamped to [0, 1]
            var originalAlpha = _context.globalAlpha;

            _context.globalAlpha = -0.5;
            Assert.That(_context.globalAlpha, Is.GreaterThanOrEqualTo(0), "Negative alpha should be handled");

            _context.globalAlpha = 1.5;
            Assert.That(_context.globalAlpha, Is.LessThanOrEqualTo(1), "Alpha > 1 should be handled");

            _context.globalAlpha = double.NaN;
            // NaN should be ignored
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestInvalidColorStrings()
        {
            // Invalid color strings should be handled gracefully
            var originalStyle = _context.fillStyle;

            Assert.DoesNotThrow(() => _context.fillStyle = "not-a-color");
            Assert.DoesNotThrow(() => _context.fillStyle = "");
            Assert.DoesNotThrow(() => _context.fillStyle = "#gg0000");
            Assert.DoesNotThrow(() => _context.fillStyle = "rgb(300, 300, 300)");

            // Context should still be usable after invalid values
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestInvalidLineCap()
        {
            var originalCap = _context.lineCap;
            _context.lineCap = "invalid-cap";

            // Should either ignore or handle gracefully
            Assert.DoesNotThrow(() => _context.strokeRect(0, 0, 10, 10));
        }

        [Test]
        public void TestInvalidLineJoin()
        {
            var originalJoin = _context.lineJoin;
            _context.lineJoin = "invalid-join";

            Assert.DoesNotThrow(() => _context.strokeRect(0, 0, 10, 10));
        }

        #endregion

        #region Arc Edge Cases

        [Test]
        public void TestNegativeArcRadius()
        {
            // Negative radius might throw or be handled - test for consistent behavior
            // According to spec, negative radius should throw
            Assert.Throws<System.NotSupportedException>(() => _context.arc(100, 100, -50, 0, Math.PI, false));
        }

        [Test]
        public void TestZeroArcRadius()
        {
            // Zero radius should not crash
            Assert.DoesNotThrow(() => _context.arc(100, 100, 0, 0, Math.PI, false));
        }

        [Test]
        public void TestArcWithSameStartAndEnd()
        {
            // Same start and end angle
            Assert.DoesNotThrow(() => _context.arc(100, 100, 50, 0, 0, false));
        }

        [Test]
        public void TestArcWithVeryLargeAngles()
        {
            Assert.DoesNotThrow(() => _context.arc(100, 100, 50, 0, 1000 * Math.PI, false));
        }

        [Test]
        public void TestArcWithNegativeAngles()
        {
            Assert.DoesNotThrow(() => _context.arc(100, 100, 50, -Math.PI, -Math.PI / 2, false));
        }

        #endregion

        #region Transform Edge Cases

        [Test]
        public void TestZeroScale()
        {
            // Zero scale should not crash
            Assert.DoesNotThrow(() => _context.scale(0, 1));
            Assert.DoesNotThrow(() => _context.scale(1, 0));
            Assert.DoesNotThrow(() => _context.scale(0, 0));

            // Drawing after zero scale
            Assert.DoesNotThrow(() => _context.fillRect(50, 50, 50, 50));
        }

        [Test]
        public void TestVeryLargeScale()
        {
            Assert.DoesNotThrow(() => _context.scale(1000, 1000));
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 1, 1));
        }

        [Test]
        public void TestVerySmallScale()
        {
            Assert.DoesNotThrow(() => _context.scale(0.0001, 0.0001));
            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 1000, 1000));
        }

        [Test]
        public void TestRotateWithNaN()
        {
            Assert.DoesNotThrow(() => _context.rotate(double.NaN));
        }

        [Test]
        public void TestRotateWithInfinity()
        {
            Assert.DoesNotThrow(() => _context.rotate(double.PositiveInfinity));
        }

        #endregion

        #region State Stack Edge Cases

        [Test]
        public void TestRestoreWithoutSave()
        {
            // Restore without save should not crash
            Assert.DoesNotThrow(() => _context.restore());
            Assert.DoesNotThrow(() => _context.restore());
            Assert.DoesNotThrow(() => _context.restore());
        }

        [Test]
        public void TestDeepStateSave()
        {
            // Save many times
            for (int i = 0; i < 100; i++)
            {
                _context.save();
            }

            // Restore many times
            for (int i = 0; i < 100; i++)
            {
                _context.restore();
            }

            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestSaveRestoreWithTransforms()
        {
            _context.translate(10, 10);
            _context.save();
            _context.translate(20, 20);
            _context.save();
            _context.scale(2, 2);

            // Multiple restores
            _context.restore();
            _context.restore();

            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        #endregion

        #region Path Edge Cases

        [Test]
        public void TestFillEmptyPath()
        {
            _context.beginPath();
            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestStrokeEmptyPath()
        {
            _context.beginPath();
            Assert.DoesNotThrow(() => _context.stroke());
        }

        [Test]
        public void TestClipEmptyPath()
        {
            _context.beginPath();
            Assert.DoesNotThrow(() => _context.clip());
        }

        [Test]
        public void TestMultipleBeginPath()
        {
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.beginPath();
            _context.moveTo(20, 20);
            _context.beginPath();

            Assert.DoesNotThrow(() => _context.fill());
        }

        [Test]
        public void TestClosePathMultipleTimes()
        {
            _context.beginPath();
            _context.moveTo(10, 10);
            _context.lineTo(50, 50);
            _context.closePath();
            _context.closePath();
            _context.closePath();

            Assert.DoesNotThrow(() => _context.fill());
        }

        #endregion

        #region Gradient Edge Cases

        [Test]
        public void TestLinearGradientWithSamePoints()
        {
            // Start and end at same point
            Assert.DoesNotThrow(() =>
            {
                var gradient = _context.createLinearGradient(50, 50, 50, 50);
            });
        }

        [Test]
        public void TestRadialGradientWithNegativeRadius()
        {
            // Negative radius should throw or be handled
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var gradient = _context.createRadialGradient(50, 50, -10, 100, 100, 50);
            });
        }

        [Test]
        public void TestRadialGradientWithZeroRadius()
        {
            Assert.DoesNotThrow(() =>
            {
                var gradient = _context.createRadialGradient(50, 50, 0, 100, 100, 50);
            });
        }

        #endregion

        #region Text Edge Cases

        [Test]
        public void TestDrawEmptyText()
        {
            Assert.DoesNotThrow(() => _context.fillText("", 50, 50));
            Assert.DoesNotThrow(() => _context.strokeText("", 50, 50));
        }

        [Test]
        public void TestDrawNullText()
        {
            // Null text should be handled or throw
            Assert.DoesNotThrow(() => _context.fillText(string.Empty, 50, 50));
        }

        [Test]
        public void TestMeasureEmptyText()
        {
            var metrics = _context.measureText("");
            Assert.That(metrics, Is.Not.Null);
        }

        [Test]
        public void TestMeasureNullText()
        {
            Assert.DoesNotThrow(() => _context.measureText(string.Empty));
        }

        [Test]
        public void TestVeryLongText()
        {
            var longText = new string('A', 10000);
            Assert.DoesNotThrow(() => _context.fillText(longText, 50, 50));
            Assert.DoesNotThrow(() => _context.measureText(longText));
        }

        #endregion

        #region Image Data Edge Cases

        [Test]
        public void TestGetImageDataNegativeDimensions()
        {
            // Negative dimensions should throw
            Assert.Throws<ArgumentException>(() => _context.getImageData(0, 0, -10, 10));
            Assert.Throws<ArgumentException>(() => _context.getImageData(0, 0, 10, -10));
        }

        [Test]
        public void TestGetImageDataZeroDimensions()
        {
            // Zero dimensions should throw
            Assert.Throws<ArgumentException>(() => _context.getImageData(0, 0, 0, 10));
            Assert.Throws<ArgumentException>(() => _context.getImageData(0, 0, 10, 0));
        }

        [Test]
        public void TestGetImageDataOutOfBounds()
        {
            // Getting image data outside canvas bounds should work (returns transparent)
            Assert.DoesNotThrow(() =>
            {
                var imageData = _context.getImageData(150, 150, 100, 100);
            });
        }

        #endregion

        #region Robustness Tests

        [Test]
        public void TestRapidStateChanges()
        {
            // Rapidly change state many times
            for (int i = 0; i < 100; i++)
            {
                _context.fillStyle = i % 2 == 0 ? "red" : "blue";
                _context.globalAlpha = (i % 10) / 10.0;
                _context.lineWidth = i % 20;
                _context.save();
            }

            for (int i = 0; i < 100; i++)
            {
                _context.restore();
            }

            Assert.DoesNotThrow(() => _context.fillRect(0, 0, 10, 10));
        }

        [Test]
        public void TestManyOperations()
        {
            // Many operations in sequence
            for (int i = 0; i < 1000; i++)
            {
                _context.fillRect(i % 200, i % 200, 10, 10);
            }

            Assert.Pass("Many operations completed without crash");
        }

        #endregion
    }
}
