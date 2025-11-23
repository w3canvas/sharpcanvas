using System;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.JsHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("SharpCanvas JavaScript Integration Test Suite\n");

            if (args.Length > 0 && args[0] == "--comprehensive")
            {
                await ComprehensiveTest.RunAllTests();
            }
            else
            {
                // Run simple test
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

                using (var engine = new V8ScriptEngine())
                {
                    var canvas = new OffscreenCanvas(200, 200, mockDocument.Object);
                    engine.AddHostObject("canvas", canvas);

                    engine.Execute(@"
                        var ctx = canvas.getContext('2d');
                        ctx.fillStyle = 'red';
                        ctx.fillRect(0, 0, 200, 200);
                    ");

                    var blob = await canvas.convertToBlob();
                    System.IO.File.WriteAllBytes("output.png", blob);

                    Console.WriteLine("✓ Simple test passed - Canvas saved to output.png");
                    Console.WriteLine("\nRun with --comprehensive flag for full test suite");
                }
            }
        }
    }
}
