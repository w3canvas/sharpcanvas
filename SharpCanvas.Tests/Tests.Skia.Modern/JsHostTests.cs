using NUnit.Framework;
using Microsoft.ClearScript.V8;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using SkiaSharp;
using Moq;

namespace SharpCanvas.Tests.Skia.Modern
{
    [TestFixture]
    public class JsHostTests
    {
        [Test]
        public void TestJsHost()
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
                var skBitmap = SKBitmap.Decode(blob);
                var pixel = skBitmap.GetPixel(100, 100);

                Assert.That(pixel.Red, Is.EqualTo(255));
            }
        }
    }
}
