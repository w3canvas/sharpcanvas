using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;
using System;

// This tool helps find the correct pixel coordinates for bezier curve tests

var mockWindow = new Mock<IWindow>();
var mockDocument = new Mock<IDocument>();
var fontFaceSet = new FontFaceSet();
mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

var info = new SKImageInfo(200, 200);
using var surface = SKSurface.Create(info);
var context = new CanvasRenderingContext2D(surface, mockDocument.Object);

// Test helpers
void FindPixels(string testName, Action<CanvasRenderingContext2D> drawAction, (int x, int y)[] checkPoints)
{
    Console.WriteLine($"\n=== {testName} ===");
    surface.Canvas.Clear(SKColors.Transparent);
    drawAction(context);

    var bitmap = new SKBitmap(surface.PeekPixels().Info);
    surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

    // Count total pixels to verify drawing happened
    int totalPixels = 0;
    for (int py = 0; py < 200; py++)
        for (int px = 0; px < 200; px++)
            if (bitmap.GetPixel(px, py).Alpha > 0)
                totalPixels++;

    Console.WriteLine($"  Total pixels drawn: {totalPixels}");

    foreach (var (x, y) in checkPoints)
    {
        var alpha = bitmap.GetPixel(x, y).Alpha;
        Console.WriteLine($"  Checking ({x},{y}): Alpha={alpha}");

        // Search nearby for pixels if this one is empty
        if (alpha == 0)
        {
            bool found = false;
            for (int dy = -15; dy <= 15 && !found; dy++)
            {
                for (int dx = -15; dx <= 15 && !found; dx++)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < 200 && ny >= 0 && ny < 200)
                    {
                        var nearAlpha = bitmap.GetPixel(nx, ny).Alpha;
                        if (nearAlpha > 200)  // High alpha
                        {
                            Console.WriteLine($"    -> Suggest ({nx},{ny}): Alpha={nearAlpha}");
                            found = true;
                        }
                    }
                }
            }
            if (!found) Console.WriteLine($"    -> No nearby high-alpha pixels found!");
        }
    }
}

// TestCubicBezierBasic
FindPixels("TestCubicBezierBasic", ctx => {
    ctx.strokeStyle = "purple";
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(20, 100);
    ctx.bezierCurveTo(20, 20, 180, 20, 180, 100);
    ctx.stroke();
}, new[] { (100, 30) });

// TestCubicBezierComplex
FindPixels("TestCubicBezierComplex", ctx => {
    ctx.strokeStyle = "magenta";
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(30, 100);
    ctx.bezierCurveTo(60, 30, 140, 30, 170, 100);
    ctx.bezierCurveTo(140, 170, 60, 170, 30, 100);
    ctx.closePath();
    ctx.stroke();
}, new[] { (100, 40), (100, 160) });

// TestMixedCurvesPath
FindPixels("TestMixedCurvesPath", ctx => {
    ctx.strokeStyle = "brown";
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(20, 100);
    ctx.quadraticCurveTo(60, 50, 100, 100);
    ctx.bezierCurveTo(120, 120, 140, 120, 160, 100);
    ctx.stroke();
}, new[] { (50, 75), (140, 110) });

// TestQuadraticCurveFromLine
FindPixels("TestQuadraticCurveFromLine", ctx => {
    ctx.strokeStyle = "navy";
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(20, 20);
    ctx.lineTo(50, 20);
    ctx.quadraticCurveTo(80, 50, 50, 80);
    ctx.lineTo(20, 80);
    ctx.closePath();
    ctx.stroke();
}, new[] { (35, 20), (70, 55) });

// TestBezierWithTransform
FindPixels("TestBezierWithTransform", ctx => {
    ctx.translate(50, 50);
    ctx.scale(1.5, 1.5);
    ctx.strokeStyle = "pink";
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.moveTo(0, 0);
    ctx.bezierCurveTo(30, 0, 30, 30, 0, 30);
    ctx.stroke();
}, new[] { (80, 70) });

Console.WriteLine("\nDone!");
