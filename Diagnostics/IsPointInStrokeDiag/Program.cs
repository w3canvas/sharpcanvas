using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

var mockWindow = new Mock<IWindow>();
var mockDocument = new Mock<IDocument>();
var fontFaceSet = new FontFaceSet();
mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

var info = new SKImageInfo(200, 200);
using var surface = SKSurface.Create(info);
var context = new CanvasRenderingContext2D(surface, mockDocument.Object);

context.lineWidth = 10;
context.beginPath();
context.moveTo(50, 50);
context.lineTo(150, 150);

Console.WriteLine("=== Is Point In Stroke Test ===");
Console.WriteLine($"Line: (50,50) to (150,150), lineWidth=10");
Console.WriteLine();

// Test various points
var points = new[] {
    (100, 100, "On the line"),
    (105, 95, "Near the line (test expects true)"),
    (103, 97, "Closer to line"),
    (102, 98, "Even closer"),
    (50, 150, "Far from line")
};

foreach (var (x, y, desc) in points)
{
    var result = context.isPointInStroke(x, y);

    // Calculate perpendicular distance to line y=x
    var dist = Math.Abs(x - y) / Math.Sqrt(2);

    Console.WriteLine($"({x},{y}) - {desc}:");
    Console.WriteLine($"  isPointInStroke: {result}");
    Console.WriteLine($"  Perpendicular distance: {dist:F2} pixels");
    Console.WriteLine($"  Expected within stroke: {dist <= 5} (stroke radius = 5)");
    Console.WriteLine();
}

// Verify actual rendering
context.strokeStyle = "blue";
context.stroke();

var bitmap = new SKBitmap(surface.PeekPixels().Info);
surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

Console.WriteLine("Actual rendering check:");
foreach (var (x, y, desc) in points)
{
    var pixel = bitmap.GetPixel(x, y);
    Console.WriteLine($"  ({x},{y}): Alpha={pixel.Alpha} {(pixel.Alpha > 0 ? "RENDERED" : "NOT RENDERED")}");
}
