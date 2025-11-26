using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using SharpCanvas.Context.Skia;
using SharpCanvas.Shared;
using Moq;

namespace SharpCanvas.JsHost
{
    public class Server
    {
        private readonly HttpListener _listener;
        private readonly ConcurrentDictionary<string, Session> _sessions = new();

        private class Session : IDisposable
        {
            public V8ScriptEngine Engine { get; }
            public OffscreenCanvas Canvas { get; }

            public Session()
            {
                Engine = new V8ScriptEngine();
                
                // Setup mocks
                var mockWindow = new Mock<IWindow>();
                var mockDocument = new Mock<IDocument>();
                var fontFaceSet = new FontFaceSet();

                mockWindow.Setup(w => w.fonts).Returns(fontFaceSet);
                mockDocument.Setup(d => d.defaultView).Returns(mockWindow.Object);

                Canvas = new OffscreenCanvas(800, 600, mockDocument.Object);
                Engine.AddHostObject("canvas", Canvas);
                
                // Helper to get context easily in JS
                Engine.Execute("var ctx = canvas.getContext('2d');");

                // Inject navigator.gpu
                var navigator = new SharpCanvas.WebGPU.Navigator();
                Engine.AddHostObject("navigator", navigator);
            }

            public void Dispose()
            {
                Engine.Dispose();
            }
        }

        public Server(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{port}/");
        }

        public async Task Start()
        {
            _listener.Start();
            Console.WriteLine($"SharpCanvas Server listening on {_listener.Prefixes.First()}");

            while (true)
            {
                var context = await _listener.GetContextAsync();
                _ = HandleRequest(context);
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;

                Console.WriteLine($"[SharpCanvas] Request: {request.HttpMethod} {request.Url.AbsolutePath}");
                foreach (string key in request.Headers.AllKeys)
                {
                    Console.WriteLine($"[SharpCanvas] Header {key}: {request.Headers[key]}");
                }

                if (request.Url.AbsolutePath == "/create-session" && request.HttpMethod == "POST")
                {
                    var sessionId = Guid.NewGuid().ToString();
                    var session = new Session();
                    _sessions.TryAdd(sessionId, session);

                    var responseString = $"{{\"sessionId\": \"{sessionId}\"}}";
                    var buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                }
                else if (request.Url.AbsolutePath == "/render" && request.HttpMethod == "POST")
                {
                    string sessionId = request.Headers["X-Session-ID"];
                    Session session = null;

                    if (!string.IsNullOrEmpty(sessionId) && _sessions.TryGetValue(sessionId, out var s))
                    {
                        session = s;
                    }
                    else
                    {
                        // Stateless fallback
                        session = new Session();
                    }

                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string script = await reader.ReadToEndAsync();
                        session.Engine.Execute(script);
                    }

                    var blob = await session.Canvas.convertToBlob();
                    
                    response.ContentType = "image/png";
                    response.ContentLength64 = blob.Length;
                    await response.OutputStream.WriteAsync(blob, 0, blob.Length);

                    if (string.IsNullOrEmpty(sessionId))
                    {
                        session.Dispose();
                    }
                }
                else
                {
                    response.StatusCode = 404;
                }

                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                context.Response.StatusCode = 500;
                using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync(ex.ToString());
                context.Response.Close();
            }
        }
    }
}
