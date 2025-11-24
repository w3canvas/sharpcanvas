using System;
using SharpCanvas.Shared;
using SharpCanvas.Context.Skia;
using SkiaSharp;

namespace SharpCanvas.Wasm.NativeAOT;

/// <summary>
/// EXPERIMENTAL: NativeAOT-LLVM WASI compilation test
///
/// This tests whether SharpCanvas can compile to native WASM using
/// NativeAOT-LLVM instead of the standard Blazor WASM runtime.
///
/// Expected benefits:
/// - Smaller package size (no .NET runtime)
/// - Better performance (truly native code)
/// - WASI 0.2 compatibility
///
/// Potential issues:
/// - SkiaSharp native dependencies may not be compatible
/// - File I/O uses different APIs in WASI
/// - Experimental tooling may have limitations
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("SharpCanvas NativeAOT-LLVM WASI Test");
        Console.WriteLine("======================================");
        Console.WriteLine();

        try
        {
            // Create a simple canvas and draw a rectangle
            var info = new SKImageInfo(400, 300);
            using var surface = SKSurface.Create(info);

            if (surface == null)
            {
                Console.WriteLine("ERROR: Failed to create SkiaSharp surface");
                return;
            }

            var document = new Document();
            var ctx = new SkiaCanvasRenderingContext2D(surface, document);

            Console.WriteLine("✓ Created canvas context");

            // Draw a simple red rectangle
            ctx.fillStyle = "#FF0000";
            ctx.fillRect(50, 50, 100, 80);
            Console.WriteLine("✓ Drew red rectangle");

            // Draw a blue stroked rectangle
            ctx.strokeStyle = "#0000FF";
            ctx.lineWidth = 3;
            ctx.strokeRect(200, 50, 100, 80);
            Console.WriteLine("✓ Drew blue stroke rectangle");

            // Draw some text
            ctx.font = "24px Arial";
            ctx.fillStyle = "#000000";
            ctx.fillText("NativeAOT", 50, 200);
            Console.WriteLine("✓ Drew text");

            // Draw a circle path
            ctx.beginPath();
            ctx.arc(200, 200, 40, 0, Math.PI * 2, false);
            ctx.fillStyle = "#00FF00";
            ctx.fill();
            Console.WriteLine("✓ Drew circle");

            // Get the bitmap data
            var pngBytes = ctx.GetBitmap();
            Console.WriteLine($"✓ Generated PNG: {pngBytes.Length} bytes");

            // Try to write output file (WASI file I/O)
            try
            {
                File.WriteAllBytes("output-nativeaot.png", pngBytes);
                Console.WriteLine("✓ Wrote output-nativeaot.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Could not write file (WASI limitation?): {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("SUCCESS: All canvas operations completed!");
            Console.WriteLine();
            Console.WriteLine("NativeAOT Compatibility: ✓ CONFIRMED");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"ERROR: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("Stack trace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            Console.WriteLine("NativeAOT Compatibility: ✗ FAILED");
        }
    }
}

/// <summary>
/// Minimal document implementation for testing
/// </summary>
class Document : IDocument
{
    public IElement? GetElementById(string id) => null;
}
