using System;
using Microsoft.ClearScript.V8;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.JsHost
{
    class Program
    {
        static void Main(string[] args)
        {
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

                var blob = canvas.convertToBlob();
                System.IO.File.WriteAllBytes("output.png", blob);

                Console.WriteLine("Canvas saved to output.png");
            }
        }
    }
}
