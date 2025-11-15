using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

namespace ContextDiagnostic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing CanvasRenderingContext2D Bezier ===\n");

            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var info = new SKImageInfo(200, 200);
            using var surface = SKSurface.Create(info);
            var context = new CanvasRenderingContext2D(surface, mockDocument.Object);

            // Replicate the exact failing test
            surface.Canvas.Clear(SKColors.Transparent);
            context.strokeStyle = "blue";
            context.lineWidth = 2;
            context.beginPath();
            context.moveTo(20, 20);
            context.quadraticCurveTo(100, 20, 100, 100);
            context.stroke();

            var bitmap = new SKBitmap(surface.PeekPixels().Info);
            surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check the pixels the test checks
            var midPixel = bitmap.GetPixel(70, 40);
            var endPixel = bitmap.GetPixel(100, 100);

            Console.WriteLine($"Test expectations:");
            Console.WriteLine($"  Mid point (70, 40): Alpha={midPixel.Alpha} (test expects > 0)");
            Console.WriteLine($"  End point (100, 100): Alpha={endPixel.Alpha} (test expects > 0)");

            // Check where pixels actually are
            Console.WriteLine($"\nActual curve location at y=40:");
            for (int x = 60; x <= 100; x += 2)
            {
                var alpha = bitmap.GetPixel(x, 40).Alpha;
                if (alpha > 0)
                {
                    Console.WriteLine($"  x={x}: Alpha={alpha}");
                }
            }

            Console.WriteLine($"\nActual curve location at x=100:");
            for (int y = 85; y <= 105; y++)
            {
                var alpha = bitmap.GetPixel(100, y).Alpha;
                if (alpha > 0)
                {
                    Console.WriteLine($"  y={y}: Alpha={alpha}");
                }
            }

            // Count total non-transparent pixels
            int pixelCount = 0;
            for (int y = 0; y < 200; y++)
            {
                for (int x = 0; x < 200; x++)
                {
                    if (bitmap.GetPixel(x, y).Alpha > 0)
                    {
                        pixelCount++;
                    }
                }
            }
            Console.WriteLine($"\nTotal pixels drawn: {pixelCount}");

            if (pixelCount == 0)
            {
                Console.WriteLine("ERROR: Nothing was drawn!");
            }
        }
    }
}
