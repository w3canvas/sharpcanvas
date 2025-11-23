using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using Moq;

Console.WriteLine("SharpCanvas WASM Console - Starting...");

// Create mock dependencies
var mockWindow = new Mock<IWindow>();
var mockDocument = new Mock<IDocument>();
var fontFaceSet = new FontFaceSet();

mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

// Create offscreen canvas
var canvas = new OffscreenCanvas(400, 300, mockDocument.Object);
var ctx = canvas.getContext("2d") as OffscreenCanvasRenderingContext2D;

if (ctx != null)
{
    Console.WriteLine("Canvas context created successfully");

    // Draw something
    ctx.fillStyle = "#FFFFFF";
    ctx.fillRect(0, 0, 400, 300);

    ctx.fillStyle = "#FF0000";
    ctx.fillRect(50, 50, 100, 80);

    ctx.fillStyle = "#00FF00";
    ctx.fillRect(170, 50, 100, 80);

    ctx.fillStyle = "#0000FF";
    ctx.fillRect(290, 50, 100, 80);

    // Draw text
    ctx.fillStyle = "#000000";
    ctx.font = "30px Arial";
    ctx.fillText("SharpCanvas WASM!", 50, 200);

    Console.WriteLine("Drawing complete");

    // Export to PNG
    var blob = await canvas.convertToBlob();
    Console.WriteLine($"Generated PNG: {blob.Length} bytes");

    // In Node.js or browser, we could save/display this
    // For now, just report success
    Console.WriteLine("✓ SharpCanvas WASM test completed successfully!");
}
else
{
    Console.WriteLine("Failed to create canvas context");
}
