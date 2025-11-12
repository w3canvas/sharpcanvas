using NUnit.Framework;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace SharpCanvas.Tests.Skia.Modern
{
    /// <summary>
    /// Comprehensive tests for ImageData manipulation methods
    /// </summary>
    public class ImageDataTests
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

        #region getImageData Tests

        [Test]
        public void TestGetImageDataBasic()
        {
            // Draw something first
            _context.fillStyle = "red";
            _context.fillRect(50, 50, 100, 100);

            var imageData = _context.getImageData(50, 50, 100, 100);

            Assert.That(imageData, Is.Not.Null);
            Assert.That(imageData, Is.InstanceOf<ImageData>());

            var data = imageData as ImageData;
            Assert.That(data.width, Is.EqualTo(100));
            Assert.That(data.height, Is.EqualTo(100));
            Assert.That(data.data, Is.Not.Null);
            Assert.That(data.data.Length, Is.EqualTo(100 * 100 * 4)); // RGBA
        }

        [Test]
        public void TestGetImageDataReadsCorrectColors()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Draw red rectangle
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 50, 50);

            // Draw blue rectangle
            _context.fillStyle = "blue";
            _context.fillRect(50, 0, 50, 50);

            // Get red area
            var redData = _context.getImageData(25, 25, 1, 1) as ImageData;
            Assert.That(redData.data[0], Is.EqualTo(255), "Red channel should be 255");
            Assert.That(redData.data[1], Is.EqualTo(0), "Green channel should be 0");
            Assert.That(redData.data[2], Is.EqualTo(0), "Blue channel should be 0");
            Assert.That(redData.data[3], Is.EqualTo(255), "Alpha channel should be 255");

            // Get blue area
            var blueData = _context.getImageData(75, 25, 1, 1) as ImageData;
            Assert.That(blueData.data[0], Is.EqualTo(0), "Red channel should be 0");
            Assert.That(blueData.data[1], Is.EqualTo(0), "Green channel should be 0");
            Assert.That(blueData.data[2], Is.EqualTo(255), "Blue channel should be 255");
            Assert.That(blueData.data[3], Is.EqualTo(255), "Alpha channel should be 255");
        }

        [Test]
        public void TestGetImageDataWithAlpha()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            _context.globalAlpha = 0.5;
            _context.fillStyle = "red";
            _context.fillRect(10, 10, 50, 50);

            var imageData = _context.getImageData(30, 30, 1, 1) as ImageData;

            Assert.That(imageData.data[0], Is.EqualTo(255), "Red channel should be 255");
            Assert.That(imageData.data[3], Is.LessThan(255), "Alpha should be less than 255");
            Assert.That(imageData.data[3], Is.GreaterThan(0), "Alpha should be greater than 0");
        }

        [Test]
        public void TestGetImageDataTransparentArea()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            var imageData = _context.getImageData(10, 10, 10, 10) as ImageData;

            // All pixels should be transparent
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                Assert.That(imageData.data[i + 3], Is.EqualTo(0), $"Pixel at index {i/4} should be transparent");
            }
        }

        [Test]
        public void TestGetImageDataLargeArea()
        {
            _context.fillStyle = "green";
            _context.fillRect(0, 0, 200, 200);

            var imageData = _context.getImageData(0, 0, 200, 200) as ImageData;

            Assert.That(imageData.width, Is.EqualTo(200));
            Assert.That(imageData.height, Is.EqualTo(200));
            Assert.That(imageData.data.Length, Is.EqualTo(200 * 200 * 4));
        }

        #endregion

        #region createImageData Tests

        [Test]
        public void TestCreateImageDataBasic()
        {
            var imageData = _context.createImageData(50, 50) as ImageData;

            Assert.That(imageData, Is.Not.Null);
            Assert.That(imageData.width, Is.EqualTo(50));
            Assert.That(imageData.height, Is.EqualTo(50));
            Assert.That(imageData.data, Is.Not.Null);
            Assert.That(imageData.data.Length, Is.EqualTo(50 * 50 * 4));
        }

        [Test]
        public void TestCreateImageDataInitiallyTransparent()
        {
            var imageData = _context.createImageData(10, 10) as ImageData;

            // All pixels should be initialized to transparent black (0, 0, 0, 0)
            for (int i = 0; i < imageData.data.Length; i++)
            {
                Assert.That(imageData.data[i], Is.EqualTo(0), $"Byte at index {i} should be 0");
            }
        }

        [Test]
        public void TestCreateImageDataVariousSizes()
        {
            var sizes = new[] { (1, 1), (10, 10), (100, 1), (1, 100), (256, 256) };

            foreach (var (width, height) in sizes)
            {
                var imageData = _context.createImageData(width, height) as ImageData;

                Assert.That(imageData.width, Is.EqualTo(width));
                Assert.That(imageData.height, Is.EqualTo(height));
                Assert.That(imageData.data.Length, Is.EqualTo(width * height * 4));
            }
        }

        #endregion

        #region putImageData Tests

        [Test]
        public void TestPutImageDataBasic()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Create red imageData
            var imageData = _context.createImageData(50, 50) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 255;     // R
                imageData.data[i + 1] = 0;   // G
                imageData.data[i + 2] = 0;   // B
                imageData.data[i + 3] = 255; // A
            }

            _context.putImageData(imageData, 10, 10);

            // Verify it was drawn
            var readBack = _context.getImageData(30, 30, 1, 1) as ImageData;
            Assert.That(readBack.data[0], Is.EqualTo(255), "Red channel should be 255");
            Assert.That(readBack.data[1], Is.EqualTo(0), "Green channel should be 0");
            Assert.That(readBack.data[2], Is.EqualTo(0), "Blue channel should be 0");
        }

        [Test]
        public void TestPutImageDataOverwrites()
        {
            // Draw blue background
            _context.fillStyle = "blue";
            _context.fillRect(0, 0, 200, 200);

            // Create red imageData and put it on top
            var imageData = _context.createImageData(50, 50) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 255;     // R
                imageData.data[i + 1] = 0;   // G
                imageData.data[i + 2] = 0;   // B
                imageData.data[i + 3] = 255; // A
            }

            _context.putImageData(imageData, 50, 50);

            // Check red area
            var redArea = _context.getImageData(75, 75, 1, 1) as ImageData;
            Assert.That(redArea.data[0], Is.EqualTo(255), "Should be red, not blue");

            // Check blue area (outside putImageData region)
            var blueArea = _context.getImageData(10, 10, 1, 1) as ImageData;
            Assert.That(blueArea.data[2], Is.EqualTo(255), "Should still be blue");
        }

        [Test]
        public void TestPutImageDataWithTransparency()
        {
            _surface.Canvas.Clear(SKColors.White);

            // Create semi-transparent imageData
            var imageData = _context.createImageData(50, 50) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 255;   // R
                imageData.data[i + 1] = 0; // G
                imageData.data[i + 2] = 0; // B
                imageData.data[i + 3] = 0; // A (fully transparent)
            }

            _context.putImageData(imageData, 50, 50);

            // Should replace with transparent pixels
            var readBack = _context.getImageData(75, 75, 1, 1) as ImageData;
            Assert.That(readBack.data[3], Is.EqualTo(0), "Should be transparent");
        }

        [Test]
        public void TestPutImageDataAtEdge()
        {
            var imageData = _context.createImageData(50, 50) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 255;     // R
                imageData.data[i + 1] = 255; // G
                imageData.data[i + 2] = 0;   // B
                imageData.data[i + 3] = 255; // A
            }

            // Put at edge - should not throw
            Assert.DoesNotThrow(() => _context.putImageData(imageData, 0, 0));
            Assert.DoesNotThrow(() => _context.putImageData(imageData, 150, 150));
        }

        [Test]
        public void TestPutImageDataWithDirtyRect()
        {
            _surface.Canvas.Clear(SKColors.White);

            // Create red imageData
            var imageData = _context.createImageData(100, 100) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 255;     // R
                imageData.data[i + 1] = 0;   // G
                imageData.data[i + 2] = 0;   // B
                imageData.data[i + 3] = 255; // A
            }

            // Only put a portion (dirty rect: x=25, y=25, width=50, height=50)
            _context.putImageData(imageData, 0, 0, 25, 25, 50, 50);

            // Check inside dirty rect - should be red
            var insideData = _context.getImageData(40, 40, 1, 1) as ImageData;
            Assert.That(insideData.data[0], Is.EqualTo(255), "Inside dirty rect should be red");

            // Check outside dirty rect - should still be white
            var outsideData = _context.getImageData(10, 10, 1, 1) as ImageData;
            Assert.That(outsideData.data[0], Is.EqualTo(255), "Outside dirty rect should be white");
            Assert.That(outsideData.data[1], Is.EqualTo(255), "Outside dirty rect should be white");
            Assert.That(outsideData.data[2], Is.EqualTo(255), "Outside dirty rect should be white");
        }

        #endregion

        #region Round-trip Tests

        [Test]
        public void TestImageDataRoundTrip()
        {
            // Draw a pattern
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 50, 50);
            _context.fillStyle = "green";
            _context.fillRect(50, 0, 50, 50);
            _context.fillStyle = "blue";
            _context.fillRect(0, 50, 50, 50);
            _context.fillStyle = "yellow";
            _context.fillRect(50, 50, 50, 50);

            // Get the data
            var imageData = _context.getImageData(0, 0, 100, 100);

            // Clear canvas
            _context.clearRect(0, 0, 200, 200);

            // Put the data back at a different location
            _context.putImageData(imageData, 50, 50);

            // Verify colors are preserved
            var redCheck = _context.getImageData(65, 65, 1, 1) as ImageData;
            Assert.That(redCheck.data[0], Is.GreaterThan(200), "Red should be preserved");

            var greenCheck = _context.getImageData(115, 65, 1, 1) as ImageData;
            Assert.That(greenCheck.data[1], Is.GreaterThan(200), "Green should be preserved");
        }

        [Test]
        public void TestImageDataModificationAndPut()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Get empty imageData
            var imageData = _context.createImageData(50, 50) as ImageData;

            // Modify it - create gradient effect
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    int index = (y * 50 + x) * 4;
                    imageData.data[index] = (byte)(x * 255 / 49);     // R increases left to right
                    imageData.data[index + 1] = (byte)(y * 255 / 49); // G increases top to bottom
                    imageData.data[index + 2] = 0;                     // B
                    imageData.data[index + 3] = 255;                   // A
                }
            }

            _context.putImageData(imageData, 0, 0);

            // Verify gradient
            var topLeft = _context.getImageData(5, 5, 1, 1) as ImageData;
            Assert.That(topLeft.data[0], Is.LessThan(50), "Top-left should have low red");
            Assert.That(topLeft.data[1], Is.LessThan(50), "Top-left should have low green");

            var bottomRight = _context.getImageData(45, 45, 1, 1) as ImageData;
            Assert.That(bottomRight.data[0], Is.GreaterThan(200), "Bottom-right should have high red");
            Assert.That(bottomRight.data[1], Is.GreaterThan(200), "Bottom-right should have high green");
        }

        #endregion

        #region ImageData with Transforms

        [Test]
        public void TestImageDataIgnoresTransform()
        {
            _surface.Canvas.Clear(SKColors.Transparent);

            // Apply transform
            _context.translate(50, 50);
            _context.rotate(Math.PI / 4);

            // Draw something
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 20, 20);

            // putImageData should ignore transform and use device coordinates
            var imageData = _context.createImageData(30, 30) as ImageData;
            for (int i = 0; i < imageData.data.Length; i += 4)
            {
                imageData.data[i] = 0;       // R
                imageData.data[i + 1] = 255; // G
                imageData.data[i + 2] = 0;   // B
                imageData.data[i + 3] = 255; // A
            }

            // This should put green at device coordinates (10, 10), ignoring transform
            _context.putImageData(imageData, 10, 10);

            // Reset transform to read
            _context.resetTransform();
            var checkData = _context.getImageData(20, 20, 1, 1) as ImageData;
            Assert.That(checkData.data[1], Is.EqualTo(255), "Should be green at device coordinates");
        }

        #endregion

        #region ImageData Cloning

        [Test]
        public void TestImageDataIndependence()
        {
            // Get imageData
            _context.fillStyle = "red";
            _context.fillRect(0, 0, 50, 50);
            var imageData1 = _context.getImageData(0, 0, 50, 50) as ImageData;

            // Create new imageData with same dimensions
            var imageData2 = _context.createImageData(50, 50) as ImageData;

            // Copy data
            for (int i = 0; i < imageData1.data.Length; i++)
            {
                imageData2.data[i] = imageData1.data[i];
            }

            // Modify imageData2
            imageData2.data[0] = 0;   // Change to not-red
            imageData2.data[2] = 255; // Add blue

            // imageData1 should be unchanged
            Assert.That(imageData1.data[0], Is.EqualTo(255), "Original imageData should be unchanged");
            Assert.That(imageData1.data[2], Is.EqualTo(0), "Original imageData should be unchanged");
        }

        #endregion
    }
}
