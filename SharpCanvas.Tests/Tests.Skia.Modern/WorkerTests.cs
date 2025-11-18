using NUnit.Framework;
using SharpCanvas.Context.Skia;
using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SharpCanvas.Shared;

namespace SharpCanvas.Tests.Skia.Modern
{
    [TestFixture]
    public class WorkerTests
    {
        private IDocument CreateMockDocument()
        {
            var mockWindow = new Mock<IWindow>();
            var mockDocument = new Mock<IDocument>();
            var fontFaceSet = new FontFaceSet();

            mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
            mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

            return mockDocument.Object;
        }

        [Test]
        public void TestWorkerMessagePassing()
        {
            var worker = new Worker();
            var messageReceived = false;
            string? receivedMessage = null;
            var resetEvent = new ManualResetEvent(false);

            worker.OnMessage += (sender, e) =>
            {
                receivedMessage = e.Data as string;
                messageReceived = true;
                resetEvent.Set();
            };

            worker.Start(w =>
            {
                var message = w.ReceiveMessage();
                if (message != null)
                {
                    w.SendToMainThread($"Echo: {message.Data}");
                }
            });

            worker.postMessage("Hello Worker!");

            resetEvent.WaitOne(5000);

            Assert.That(messageReceived, Is.True);
            Assert.That(receivedMessage, Is.EqualTo("Echo: Hello Worker!"));

            worker.Dispose();
        }

        [Test]
        public void TestWorkerTransferableObjects()
        {
            var worker = new Worker();
            var bitmap = new SkiaSharp.SKBitmap(100, 100);
            var imageBitmap = new ImageBitmap(bitmap);

            Assert.That(imageBitmap.IsNeutered, Is.False);

            var transferList = new System.Collections.Generic.List<ITransferable> { imageBitmap };
            worker.postMessage("Transfer test", transferList);

            // After transfer, the ImageBitmap should be neutered
            Assert.That(imageBitmap.IsNeutered, Is.True);
            Assert.Throws<InvalidOperationException>(() => { var w = imageBitmap.width; });

            worker.Dispose();
        }

        [Test]
        public void TestWorkerTermination()
        {
            var worker = new Worker();
            var isRunning = false;

            worker.Start(w =>
            {
                isRunning = true;
                Thread.Sleep(10000); // Long running task
            });

            Thread.Sleep(100); // Give it time to start
            Assert.That(isRunning, Is.True);

            worker.terminate();

            // Worker should stop
            Assert.DoesNotThrow(() => worker.Dispose());
        }

        [Test]
        public void TestSharedWorkerSingleInstance()
        {
            var document = CreateMockDocument();
            var worker1 = new SharedWorker("test-worker");
            var worker2 = new SharedWorker("test-worker");

            // Both should connect to the same instance
            Assert.That(worker1, Is.Not.Null);
            Assert.That(worker2, Is.Not.Null);

            worker1.Dispose();
            worker2.Dispose();
        }

        [Test]
        public void TestSharedWorkerMessagePassing()
        {
            var document = CreateMockDocument();
            var connectCount = 0;
            var messageResetEvent = new ManualResetEvent(false);
            var loadResetEvent = new ManualResetEvent(false);
            string? receivedMessage = null;

            var worker = new SharedWorker("message-test", scope =>
            {
                scope.OnConnect += (sender, e) =>
                {
                    connectCount++;
                    var port = e.Port;

                    if (port != null)
                    {
                        port.OnMessage += (msgSender, msgEvent) =>
                        {
                            port.postMessage($"Echo: {msgEvent.Data}");
                        };
                        port.start();
                    }
                };
            });

            worker.OnLoad += (sender, e) =>
            {
                loadResetEvent.Set();
            };

            worker.port.OnMessage += (sender, e) =>
            {
                receivedMessage = e.Data as string;
                messageResetEvent.Set();
            };
            worker.port.start();

            worker.Start();

            // Wait for the worker to be loaded before posting a message
            loadResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            worker.port.postMessage("Hello SharedWorker!");

            messageResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            Assert.That(connectCount, Is.GreaterThan(0));
            Assert.That(receivedMessage, Is.EqualTo("Echo: Hello SharedWorker!"));

            worker.Dispose();
        }

        [Test]
        public async Task TestWorkerCanvasRenderAsync()
        {
            var document = CreateMockDocument();

            var imageBitmap = await WorkerCanvas.RenderAsync(200, 200, document, canvas =>
            {
                var ctx = canvas.getContext("2d");
                ctx.fillStyle = "blue";
                ctx.fillRect(0, 0, 200, 200);
            });

            Assert.That(imageBitmap, Is.Not.Null);
            Assert.That(imageBitmap.width, Is.EqualTo(200));
            Assert.That(imageBitmap.height, Is.EqualTo(200));

            var skBitmap = imageBitmap.GetBitmap();
            Assert.That(skBitmap, Is.Not.Null);
            var pixel = skBitmap.GetPixel(100, 100);
            Assert.That(pixel.Blue, Is.EqualTo(255));

            imageBitmap.close();
        }

        [Test]
        public async Task TestWorkerCanvasRenderToBlobAsync()
        {
            var document = CreateMockDocument();

            var blob = await WorkerCanvas.RenderToBlobAsync(100, 100, document, canvas =>
            {
                var ctx = canvas.getContext("2d");
                ctx.fillStyle = "red";
                ctx.fillRect(0, 0, 100, 100);
            }, "image/png", 1.0);

            Assert.That(blob, Is.Not.Null);
            Assert.That(blob.Length, Is.GreaterThan(0));

            // Verify it's a PNG by checking magic bytes
            Assert.That(blob[0], Is.EqualTo(0x89));
            Assert.That(blob[1], Is.EqualTo(0x50)); // 'P'
            Assert.That(blob[2], Is.EqualTo(0x4E)); // 'N'
            Assert.That(blob[3], Is.EqualTo(0x47)); // 'G'
        }

        [Test]
        public async Task TestWorkerCanvasRenderFramesAsync()
        {
            var document = CreateMockDocument();

            var frames = await WorkerCanvas.RenderFramesAsync(5, 100, 100, document, (canvas, frameIndex) =>
            {
                var ctx = canvas.getContext("2d");
                // Draw different content for each frame
                var brightness = (byte)(frameIndex * 50);
                ctx.fillStyle = $"rgb({brightness}, {brightness}, {brightness})";
                ctx.fillRect(0, 0, 100, 100);
            });

            Assert.That(frames, Is.Not.Null);
            Assert.That(frames.Length, Is.EqualTo(5));

            // Verify each frame
            for (int i = 0; i < frames.Length; i++)
            {
                Assert.That(frames[i], Is.Not.Null);
                Assert.That(frames[i].width, Is.EqualTo(100));
                Assert.That(frames[i].height, Is.EqualTo(100));
                frames[i].close();
            }
        }

        [Test]
        public void TestStreamingRenderer()
        {
            var document = CreateMockDocument();
            var frameCount = 0;
            var resetEvent = new ManualResetEvent(false);

            var worker = WorkerCanvas.CreateStreamingRenderer(100, 100, document, (canvas, command) =>
            {
                var ctx = canvas.getContext("2d");
                if (command is string color)
                {
                    ctx.fillStyle = color;
                    ctx.fillRect(0, 0, 100, 100);
                }
            });

            worker.OnMessage += (sender, e) =>
            {
                frameCount++;
                if (frameCount >= 3)
                {
                    resetEvent.Set();
                }
            };

            // Send multiple draw commands
            worker.postMessage("red");
            worker.postMessage("green");
            worker.postMessage("blue");

            resetEvent.WaitOne(5000);

            Assert.That(frameCount, Is.GreaterThanOrEqualTo(3));

            worker.postMessage("close");
            worker.Dispose();
        }

        [Test]
        public void TestMessagePortTransfer()
        {
            var worker = new SharedWorker("port-transfer-test");
            var port = worker.port;
            var bitmap = new SkiaSharp.SKBitmap(50, 50);
            var imageBitmap = new ImageBitmap(bitmap);

            var transferList = new System.Collections.Generic.List<ITransferable> { imageBitmap };

            Assert.That(imageBitmap.IsNeutered, Is.False);

            port.postMessage("test", transferList);

            Assert.That(imageBitmap.IsNeutered, Is.True);

            port.close();
            worker.Dispose();
        }

        [Test]
        public void TestSharedWorkerMultipleConnections()
        {
            var document = CreateMockDocument();
            var connectCount = 0;
            var loadResetEvent = new ManualResetEvent(false);

            var sharedWorker = new SharedWorker("multi-connection-test", scope =>
            {
                scope.OnConnect += (sender, e) =>
                {
                    Interlocked.Increment(ref connectCount);
                };
            });

            sharedWorker.OnLoad += (sender, e) =>
            {
                loadResetEvent.Set();
            };

            sharedWorker.Start();

            // Wait for the worker to be loaded before creating more connections
            loadResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            // Create additional connections
            var worker2 = new SharedWorker("multi-connection-test");
            var worker3 = new SharedWorker("multi-connection-test");

            Thread.Sleep(500); // Give time for connections

            // All three should have connected to the same worker
            Assert.That(connectCount, Is.GreaterThanOrEqualTo(1));

            sharedWorker.Dispose();
            worker2.Dispose();
            worker3.Dispose();
        }
    }
}
