using SkiaSharp;
using System;

namespace BezierDiagnostic2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing LineWidth Effect on Curves ===\n");

            // Test the exact same curve from the failing test
            TestCurveWithLineWidth(2, "width2_test.png");
            TestCurveWithLineWidth(3, "width3_test.png");
        }

        static void TestCurveWithLineWidth(float width, string filename)
        {
            Console.WriteLine($"\n=== Testing with lineWidth = {width} ===");

            var info = new SKImageInfo(200, 200);
            using var surface = SKSurface.Create(info);
            surface.Canvas.Clear(SKColors.Transparent);

            var paint = new SKPaint
            {
                Color = SKColors.Blue,
                StrokeWidth = width,
                Style = SKPaintStyle.Stroke,
                IsAntialias = true
            };

            var path = new SKPath();
            path.MoveTo(20, 20);
            path.QuadTo(100, 20, 100, 100);

            surface.Canvas.DrawPath(path, paint);

            var bitmap = new SKBitmap(info);
            surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check the exact pixels from the failing test
            var midPixel = bitmap.GetPixel(70, 40);
            var endPixel = bitmap.GetPixel(100, 100);

            Console.WriteLine($"  Mid point (70, 40): Alpha={midPixel.Alpha}");
            Console.WriteLine($"  End point (100, 100): Alpha={endPixel.Alpha}");

            // Check where pixels actually ARE around y=40
            Console.WriteLine($"  Scanning y=40 for pixels:");
            for (int x = 60; x <= 100; x += 2)
            {
                var alpha = bitmap.GetPixel(x, 40).Alpha;
                if (alpha > 0)
                {
                    Console.WriteLine($"    x={x}: Alpha={alpha}");
                }
            }

            // Check where pixels ARE around x=100
            Console.WriteLine($"  Scanning x=100 for pixels:");
            for (int y = 90; y <= 105; y++)
            {
                var alpha = bitmap.GetPixel(100, y).Alpha;
                if (alpha > 0)
                {
                    Console.WriteLine($"    y={y}: Alpha={alpha}");
                }
            }

            // Check many points along the curve
            int pixelCount = 0;
            for (int y = 20; y <= 100; y++)
            {
                for (int x = 20; x <= 100; x++)
                {
                    if (bitmap.GetPixel(x, y).Alpha > 0)
                    {
                        pixelCount++;
                    }
                }
            }
            Console.WriteLine($"  Total pixels with Alpha > 0: {pixelCount}");

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = System.IO.File.OpenWrite(filename))
            {
                data.SaveTo(stream);
            }
            Console.WriteLine($"  Saved to: {filename}");

            path.Dispose();
            paint.Dispose();
        }
    }
}
