using SkiaSharp;
using System;

namespace ArcDiagnostic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing New Arc Calculation ===\n");

            // Test the new calculation logic
            TestNewCalculation("Clockwise 0 to π", 0, Math.PI, false);
            TestNewCalculation("Anticlockwise π to 0", Math.PI, 0, true);
        }

        static void TestNewCalculation(string name, double startAngle, double endAngle, bool anticlockwise)
        {
            Console.WriteLine($"\n{name}:");
            Console.WriteLine($"  Start: {startAngle:F4} rad, End: {endAngle:F4} rad, Anticlockwise: {anticlockwise}");

            var startDegrees = (float)(startAngle * 180 / Math.PI);
            var endDegrees = (float)(endAngle * 180 / Math.PI);

            Console.WriteLine($"  Start: {startDegrees:F2}°, End: {endDegrees:F2}°");

            // New calculation
            float sweepAngle;
            if (anticlockwise)
            {
                sweepAngle = startDegrees - endDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }
            else
            {
                sweepAngle = endDegrees - startDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }

            Console.WriteLine($"  Calculated sweep: {sweepAngle:F2}°");

            // Test what this actually renders
            var path = new SKPath();
            var rect = new SKRect(25, 25, 75, 75);
            var startX = (float)(50 + 25 * Math.Cos(startAngle));
            var startY = (float)(50 + 25 * Math.Sin(startAngle));

            path.MoveTo(startX, startY);
            path.AddArc(rect, startDegrees, sweepAngle);
            path.Close();

            Console.WriteLine($"  Path bounds: {path.Bounds}");
            Console.WriteLine($"  Contains top (50,35): {path.Contains(50, 35)}");
            Console.WriteLine($"  Contains bottom (50,65): {path.Contains(50, 65)}");

            path.Dispose();
        }
    }
}
