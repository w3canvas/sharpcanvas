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

void FindPixel(string name, Action test, int checkX, int checkY)
{
    Console.WriteLine($"\n=== {name} ===");
    test();

    var bitmap = new SKBitmap(surface.PeekPixels().Info);
    surface.ReadPixels(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes, 0, 0);

    int totalPixels = 0;
    for (int y = 0; y < 200; y++)
        for (int x = 0; x < 200; x++)
            if (bitmap.GetPixel(x, y).Alpha > 0)
                totalPixels++;

    Console.WriteLine($"  Total pixels: {totalPixels}");
    Console.WriteLine($"  Checking ({checkX},{checkY}): Alpha={bitmap.GetPixel(checkX, checkY).Alpha}");

    // Search nearby
    bool found = false;
    for (int dy = -15; dy <= 15 && !found; dy++)
    {
        for (int dx = -15; dx <= 15 && !found; dx++)
        {
            int nx = checkX + dx, ny = checkY + dy;
            if (nx >= 0 && nx < 200 && ny >= 0 && ny < 200)
            {
                if (bitmap.GetPixel(nx, ny).Alpha > 200)
                {
                    Console.WriteLine($"  -> Suggest ({nx},{ny})");
                    found = true;
                }
            }
        }
    }
}

// TestPath2DQuadraticCurve
FindPixel("TestPath2DQuadraticCurve", () => {
    var path = new Path2D();
    path.moveTo(20, 100);
    path.quadraticCurveTo(100, 20, 180, 100);
    surface.Canvas.Clear(SKColors.Transparent);
    context.strokeStyle = "purple";
    context.lineWidth = 2;
    context.stroke(path);
}, 100, 50);

// TestPath2DBezierCurve
FindPixel("TestPath2DBezierCurve", () => {
    var path = new Path2D();
    path.moveTo(20, 100);
    path.bezierCurveTo(20, 20, 180, 20, 180, 100);
    surface.Canvas.Clear(SKColors.Transparent);
    context.strokeStyle = "orange";
    context.lineWidth = 2;
    context.stroke(path);
}, 100, 30);

// TestPath2DComplexShape
FindPixel("TestPath2DComplexShape", () => {
    var path = new Path2D();
    path.moveTo(100, 50);
    path.lineTo(150, 100);
    path.arc(100, 100, 50, 0, Math.PI, false);
    path.closePath();
    surface.Canvas.Clear(SKColors.Transparent);
    context.fillStyle = "navy";
    context.fill(path);
}, 100, 90);

Console.WriteLine("\nDone!");
