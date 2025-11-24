#nullable enable
using SharpCanvas.Shared;
using SharpCanvas.Runtime.Workers;
using System;
using System.Threading.Tasks;

namespace SharpCanvas.Context.Skia
{
    /// <summary>
    /// Advanced helper for canvas operations in workers.
    /// Provides high-level abstractions for common worker + canvas patterns.
    /// </summary>
    public static class WorkerCanvas
    {
        /// <summary>
        /// Renders canvas content in a dedicated worker and returns an ImageBitmap
        /// </summary>
        /// <param name="width">Canvas width</param>
        /// <param name="height">Canvas height</param>
        /// <param name="document">Document context</param>
        /// <param name="drawFunction">Drawing function</param>
        /// <returns>Task that resolves with the resulting ImageBitmap</returns>
        public static Task<ImageBitmap> RenderAsync(
            int width,
            int height,
            IDocument document,
            Action<OffscreenCanvas> drawFunction)
        {
            var tcs = new TaskCompletionSource<ImageBitmap>();

            Task.Run(() =>
            {
                try
                {
                    var canvas = new OffscreenCanvas(width, height, document);
                    drawFunction(canvas);
                    var imageBitmap = canvas.transferToImageBitmap();
                    tcs.SetResult(imageBitmap);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Renders canvas content and exports as a blob
        /// </summary>
        public static Task<byte[]> RenderToBlobAsync(
            int width,
            int height,
            IDocument document,
            Action<OffscreenCanvas> drawFunction,
            string format = "image/png",
            double quality = 1.0)
        {
            var tcs = new TaskCompletionSource<byte[]>();

            Task.Run(async () =>
            {
                try
                {
                    var canvas = new OffscreenCanvas(width, height, document);
                    drawFunction(canvas);
                    var blob = await canvas.convertToBlob(format, quality);
                    tcs.SetResult(blob);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Processes multiple frames in parallel using workers
        /// </summary>
        /// <param name="frameCount">Number of frames to process</param>
        /// <param name="width">Frame width</param>
        /// <param name="height">Frame height</param>
        /// <param name="document">Document context</param>
        /// <param name="drawFrame">Function that draws a specific frame</param>
        /// <returns>Array of ImageBitmaps, one per frame</returns>
        public static Task<ImageBitmap[]> RenderFramesAsync(
            int frameCount,
            int width,
            int height,
            IDocument document,
            Action<OffscreenCanvas, int> drawFrame)
        {
            var tasks = new Task<ImageBitmap>[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                int frameIndex = i; // Capture loop variable
                tasks[i] = RenderAsync(width, height, document, canvas =>
                {
                    drawFrame(canvas, frameIndex);
                });
            }

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Sets up a SharedWorker for collaborative canvas rendering
        /// Multiple contexts can connect to the same worker and receive rendered frames
        /// </summary>
        /// <param name="workerName">Unique name for the shared worker</param>
        /// <param name="width">Canvas width</param>
        /// <param name="height">Canvas height</param>
        /// <param name="document">Document context</param>
        /// <param name="drawFunction">Drawing function called when requested</param>
        /// <returns>SharedWorker instance</returns>
        public static SharedWorker CreateSharedRenderer(
            string workerName,
            int width,
            int height,
            IDocument document,
            Action<OffscreenCanvas, object> drawFunction)
        {
            var worker = new SharedWorker(workerName, scope =>
            {
                scope.OnConnect += (sender, e) =>
                {
                    var port = e.Port;
                    if (port == null) return;

                    // Set up message handler for this port
                    port.OnMessage += (msgSender, msgEvent) =>
                    {
                        try
                        {
                            if (msgEvent.Data is null)
                            {
                                return;
                            }
                            var canvas = new OffscreenCanvas(width, height, document);
                            drawFunction(canvas, msgEvent.Data);
                            var imageBitmap = canvas.transferToImageBitmap();

                            // Send back the result
                            port.postMessage(new
                            {
                                type = "result",
                                imageBitmap = imageBitmap
                            });
                        }
                        catch (Exception ex)
                        {
                            port.postMessage(new
                            {
                                type = "error",
                                message = ex.Message
                            });
                        }
                    };

                    port.start();
                };
            });

            worker.Start();
            return worker;
        }

        /// <summary>
        /// Creates a streaming canvas worker that processes a sequence of draw commands
        /// </summary>
        public static Worker CreateStreamingRenderer(
            int width,
            int height,
            IDocument document,
            Action<OffscreenCanvas, object> drawFunction)
        {
            var worker = new Worker();

            worker.Start(w =>
            {
                var canvas = new OffscreenCanvas(width, height, document);

                while (true)
                {
                    var message = w.ReceiveMessage();
                    if (message == null) break;

                    try
                    {
                        var command = message.Data;

                        if (command is string cmd && cmd == "close")
                        {
                            break;
                        }
                        if(command is null)
                        {
                            continue;
                        }

                        drawFunction(canvas, command);
                        var imageBitmap = canvas.transferToImageBitmap();

                        w.SendToMainThread(new
                        {
                            type = "frame",
                            imageBitmap = imageBitmap
                        });
                    }
                    catch (Exception ex)
                    {
                        w.SendToMainThread(new
                        {
                            type = "error",
                            message = ex.Message
                        });
                    }
                }
            });

            return worker;
        }
    }
}
