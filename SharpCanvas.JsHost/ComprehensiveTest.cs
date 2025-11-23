using System;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using Moq;

namespace SharpCanvas.JsHost
{
    public class ComprehensiveTest
    {
        public static async Task RunAllTests()
        {
            Console.WriteLine("=== SharpCanvas JavaScript Integration Tests ===\n");

            await TestBasicDrawing();
            await TestPathAPI();
            await TestTransformations();
            await TestGradients();
            await TestText();

            Console.WriteLine("\n=== All JavaScript Integration Tests Passed! ===");
        }

        static async Task TestBasicDrawing()
        {
            Console.WriteLine("[1/5] Testing Basic Drawing...");
            var (engine, canvas) = CreateTestEnvironment();

            engine.Execute(@"
                var ctx = canvas.getContext('2d');

                // Fill rectangle
                ctx.fillStyle = '#FF0000';
                ctx.fillRect(10, 10, 50, 50);

                // Stroke rectangle
                ctx.strokeStyle = '#00FF00';
                ctx.lineWidth = 3;
                ctx.strokeRect(70, 10, 50, 50);

                // Clear rectangle
                ctx.clearRect(20, 20, 30, 30);
            ");

            var blob = await canvas.convertToBlob();
            System.IO.File.WriteAllBytes("test-basic.png", blob);
            Console.WriteLine("  ✓ Basic drawing works - saved to test-basic.png");

            engine.Dispose();
        }

        static async Task TestPathAPI()
        {
            Console.WriteLine("[2/5] Testing Path API...");
            var (engine, canvas) = CreateTestEnvironment();

            engine.Execute(@"
                var ctx = canvas.getContext('2d');

                // Begin path
                ctx.beginPath();
                ctx.moveTo(50, 50);
                ctx.lineTo(150, 50);
                ctx.lineTo(100, 150);
                ctx.closePath();

                ctx.fillStyle = '#0000FF';
                ctx.fill();
                ctx.strokeStyle = '#FF00FF';
                ctx.stroke();

                // Test arc
                ctx.beginPath();
                ctx.arc(100, 100, 30, 0, Math.PI * 2);
                ctx.fillStyle = '#FFFF00';
                ctx.fill();

                // Test bezier curve
                ctx.beginPath();
                ctx.moveTo(20, 20);
                ctx.bezierCurveTo(20, 100, 180, 100, 180, 20);
                ctx.strokeStyle = '#00FFFF';
                ctx.lineWidth = 2;
                ctx.stroke();
            ");

            var blob = await canvas.convertToBlob();
            System.IO.File.WriteAllBytes("test-paths.png", blob);
            Console.WriteLine("  ✓ Path API works - saved to test-paths.png");

            engine.Dispose();
        }

        static async Task TestTransformations()
        {
            Console.WriteLine("[3/5] Testing Transformations...");
            var (engine, canvas) = CreateTestEnvironment();

            engine.Execute(@"
                var ctx = canvas.getContext('2d');

                ctx.fillStyle = '#FF0000';

                // Save state
                ctx.save();

                // Translate
                ctx.translate(50, 50);
                ctx.fillRect(0, 0, 30, 30);

                // Rotate
                ctx.rotate(Math.PI / 4);
                ctx.fillStyle = '#00FF00';
                ctx.fillRect(0, 0, 30, 30);

                // Scale
                ctx.scale(0.5, 0.5);
                ctx.fillStyle = '#0000FF';
                ctx.fillRect(0, 0, 30, 30);

                // Restore state
                ctx.restore();
                ctx.fillStyle = '#FFFF00';
                ctx.fillRect(150, 150, 30, 30);
            ");

            var blob = await canvas.convertToBlob();
            System.IO.File.WriteAllBytes("test-transforms.png", blob);
            Console.WriteLine("  ✓ Transformations work - saved to test-transforms.png");

            engine.Dispose();
        }

        static async Task TestGradients()
        {
            Console.WriteLine("[4/5] Testing Gradients...");
            var (engine, canvas) = CreateTestEnvironment();

            engine.Execute(@"
                var ctx = canvas.getContext('2d');

                // Linear gradient
                var gradient = ctx.createLinearGradient(0, 0, 200, 0);
                gradient.addColorStop(0, 'red');
                gradient.addColorStop(0.5, 'yellow');
                gradient.addColorStop(1, 'blue');

                ctx.fillStyle = gradient;
                ctx.fillRect(0, 0, 200, 100);

                // Radial gradient
                var radialGradient = ctx.createRadialGradient(100, 150, 10, 100, 150, 50);
                radialGradient.addColorStop(0, 'white');
                radialGradient.addColorStop(1, 'green');

                ctx.fillStyle = radialGradient;
                ctx.fillRect(50, 100, 100, 100);
            ");

            var blob = await canvas.convertToBlob();
            System.IO.File.WriteAllBytes("test-gradients.png", blob);
            Console.WriteLine("  ✓ Gradients work - saved to test-gradients.png");

            engine.Dispose();
        }

        static async Task TestText()
        {
            Console.WriteLine("[5/5] Testing Text Rendering...");
            var (engine, canvas) = CreateTestEnvironment();

            engine.Execute(@"
                var ctx = canvas.getContext('2d');

                ctx.font = '20px Arial';
                ctx.fillStyle = '#000000';
                ctx.fillText('Hello Canvas!', 10, 30);

                ctx.font = '30px Arial';
                ctx.strokeStyle = '#FF0000';
                ctx.strokeText('Stroke Text', 10, 70);

                // Test text alignment
                ctx.textAlign = 'center';
                ctx.fillText('Centered', 100, 120);

                ctx.textAlign = 'right';
                ctx.fillText('Right Aligned', 190, 160);
            ");

            var blob = await canvas.convertToBlob();
            System.IO.File.WriteAllBytes("test-text.png", blob);
            Console.WriteLine("  ✓ Text rendering works - saved to test-text.png");

            engine.Dispose();
        }

        static (V8ScriptEngine, OffscreenCanvas) CreateTestEnvironment()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            var engine = new V8ScriptEngine();
            var canvas = new OffscreenCanvas(200, 200, mockDocument.Object);

            engine.AddHostObject("canvas", canvas);
            engine.AddHostObject("Math", typeof(Math));

            return (engine, canvas);
        }
    }
}
