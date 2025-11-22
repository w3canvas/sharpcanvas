using SkiaSharp;
using System;

namespace BezierDiagnostic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing Bezier Curve Rendering ===\n");

            // Create a surface
            var info = new SKImageInfo(200, 200);
            using var surface = SKSurface.Create(info);
            surface.Canvas.Clear(SKColors.Transparent);

            // Create a path with a quadratic curve
            var path = new SKPath();
            path.MoveTo(20, 20);
            path.QuadTo(100, 20, 100, 100);

            Console.WriteLine($"Path created:");
            Console.WriteLine($"  Bounds: {path.Bounds}");
            Console.WriteLine($"  IsEmpty: {path.IsEmpty}");
            Console.WriteLine($"  PointCount: {path.PointCount}");

            // Create a paint for stroking
            var paint = new SKPaint
            {
                Color = SKColors.Blue,
                StrokeWidth = 5,  // Wider stroke to make it easier to see
                Style = SKPaintStyle.Stroke,
                IsAntialias = true
            };

            // Draw the path
            surface.Canvas.DrawPath(path, paint);

            // Read pixels to check if anything was drawn
            var bitmap = new SKBitmap(info);
            surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            // Check many pixels along the expected curve path
            var pixel1 = bitmap.GetPixel(20, 20);    // Start point
            var pixel2 = bitmap.GetPixel(40, 25);    // Early curve
            var pixel3 = bitmap.GetPixel(60, 30);    // Mid-early curve
            var pixel4 = bitmap.GetPixel(80, 45);    // Mid curve
            var pixel5 = bitmap.GetPixel(95, 75);    // Late curve
            var pixel6 = bitmap.GetPixel(100, 100);  // End point

            Console.WriteLine($"\nPixel checks along curve:");
            Console.WriteLine($"  Start (20,20): Alpha={pixel1.Alpha}");
            Console.WriteLine($"  Early (40,25): Alpha={pixel2.Alpha}");
            Console.WriteLine($"  Mid-early (60,30): Alpha={pixel3.Alpha}");
            Console.WriteLine($"  Mid (80,45): Alpha={pixel4.Alpha}");
            Console.WriteLine($"  Late (95,75): Alpha={pixel5.Alpha}");
            Console.WriteLine($"  End (100,100): Alpha={pixel6.Alpha}");

            // Save the bitmap to see what's actually being drawn
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite("quadratic_curve_test.png"))
            {
                data.SaveTo(stream);
            }
            Console.WriteLine($"  Saved to: quadratic_curve_test.png");

            // Now test with a simple line for comparison
            Console.WriteLine($"\n=== Testing Simple Line ===\n");

            surface.Canvas.Clear(SKColors.Transparent);
            var linePath = new SKPath();
            linePath.MoveTo(20, 20);
            linePath.LineTo(100, 100);

            Console.WriteLine($"Line path created:");
            Console.WriteLine($"  Bounds: {linePath.Bounds}");
            Console.WriteLine($"  IsEmpty: {linePath.IsEmpty}");
            Console.WriteLine($"  PointCount: {linePath.PointCount}");

            surface.Canvas.DrawPath(linePath, paint);
            surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

            var linePixel1 = bitmap.GetPixel(20, 20);
            var linePixel2 = bitmap.GetPixel(60, 60);

            Console.WriteLine($"\nLine pixel checks:");
            Console.WriteLine($"  Start (20,20): Alpha={linePixel1.Alpha}, Color={linePixel1}");
            Console.WriteLine($"  Mid (60,60): Alpha={linePixel2.Alpha}, Color={linePixel2}");

            path.Dispose();
            linePath.Dispose();
            paint.Dispose();
        }
    }
}
