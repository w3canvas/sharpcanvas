# ImageBitmap API and Worker Implementation

## Overview

This document describes the comprehensive ImageBitmap API and Web Workers implementation added to SharpCanvas, enabling efficient multi-threaded canvas rendering for video/audio work and performance-critical applications.

## Components

### 1. ImageBitmap API (HIGH PRIORITY) ✅

**Location:** `SharpCanvas/Context.Skia/ImageBitmap.cs`

A high-performance bitmap image class designed for use with OffscreenCanvas and Workers.

**Features:**
- `width` and `height` properties (read-only, in CSS pixels)
- `close()` method for explicit resource cleanup
- Implements `IDisposable` for automatic cleanup
- Implements `ITransferable` for zero-copy worker transfers
- Neutering support prevents use after transfer

**Example:**
```csharp
var bitmap = new SKBitmap(200, 200);
var imageBitmap = new ImageBitmap(bitmap);

Console.WriteLine($"Size: {imageBitmap.width}x{imageBitmap.height}");

imageBitmap.close(); // Cleanup resources
```

### 2. createImageBitmap() Function (HIGH PRIORITY) ✅

**Location:** `SharpCanvas/Context.Skia/ImageBitmapFactory.cs`

Global factory function for creating ImageBitmap instances from various sources.

**Signatures:**
```csharp
Task<ImageBitmap> createImageBitmap(object source, ImageBitmapOptions? options = null)
Task<ImageBitmap> createImageBitmap(object source, int sx, int sy, int sw, int sh, ImageBitmapOptions? options = null)
```

**Supported Sources:**
- `SKBitmap` - SkiaSharp bitmap
- `SKImage` - SkiaSharp image
- `ImageBitmap` - Another ImageBitmap (creates a copy)
- `IImage` - SharpCanvas image interface
- `IHTMLCanvasElement` - Canvas element
- `OffscreenCanvas` - Offscreen canvas
- `ImageData` - Raw pixel data
- `byte[]` - Encoded image bytes
- `Stream` - Image stream
- `string` - File path

**Options:**
- `premultiplyAlpha` - "default", "premultiply", or "none"
- `colorSpaceConversion` - "default" or custom
- `resizeQuality` - "high", "medium", or "low"
- `resizeWidth` / `resizeHeight` - Target dimensions
- `imageOrientation` - "none" or orientation

**Examples:**
```csharp
// Basic creation
var imageBitmap = await ImageBitmapFactory.createImageBitmap(skBitmap);

// With cropping
var cropped = await ImageBitmapFactory.createImageBitmap(source, 50, 50, 100, 100);

// With resizing
var options = new ImageBitmapOptions
{
    resizeWidth = 100,
    resizeHeight = 100,
    resizeQuality = "high"
};
var resized = await ImageBitmapFactory.createImageBitmap(source, options);
```

### 3. OffscreenCanvas Enhancements ✅

**Location:** `SharpCanvas/Context.Skia/OffscreenCanvas.cs`

Enhanced OffscreenCanvas with ImageBitmap support and blob conversion.

**New Features:**

#### transferToImageBitmap() (MEDIUM PRIORITY)
Captures the current canvas content as an ImageBitmap for efficient frame capture.

```csharp
var canvas = new OffscreenCanvas(200, 200, document);
var ctx = canvas.getContext("2d");
ctx.fillRect(0, 0, 200, 200);

var imageBitmap = canvas.transferToImageBitmap();
```

#### convertToBlob() (MEDIUM PRIORITY)
Exports canvas content as an encoded image blob.

```csharp
// PNG (default)
var pngBlob = await canvas.convertToBlob();

// JPEG with quality
var jpegBlob = await canvas.convertToBlob("image/jpeg", 0.9);

// WebP
var webpBlob = await canvas.convertToBlob("image/webp", 0.8);
```

#### width/height Properties
Dynamic canvas resizing with content preservation.

```csharp
var canvas = new OffscreenCanvas(100, 100, document);
canvas.width = 200;  // Resize while preserving content
canvas.height = 150;
```

### 4. Transferable Objects (HIGH PRIORITY) ✅

**Location:** `SharpCanvas/Context.Skia/ITransferable.cs`

Marker interface for zero-copy transfers between workers.

**Interface:**
```csharp
public interface ITransferable
{
    bool IsNeutered { get; }
    void Neuter();
}
```

**Implementations:**
- `ImageBitmap` - Can be transferred between workers
- `OffscreenCanvas` - Can be transferred between workers

**Behavior:**
- After transfer, original object is "neutered" (unusable)
- No data copying occurs (zero-copy transfer)
- Attempting to use neutered objects throws `InvalidOperationException`

### 5. Worker API (HIGH PRIORITY) ✅

**Location:** `SharpCanvas/Context.Skia/Worker.cs`

Dedicated Web Worker implementation for background threading.

**Features:**
- Message passing with `postMessage()`
- Transferable object support
- Event handlers: `OnMessage`, `OnError`
- `terminate()` for worker cleanup

**Example:**
```csharp
var worker = new Worker();

worker.OnMessage += (sender, e) =>
{
    Console.WriteLine($"Received: {e.Data}");
};

worker.Start(w =>
{
    while (true)
    {
        var message = w.ReceiveMessage();
        if (message == null) break;

        w.SendToMainThread($"Processed: {message.Data}");
    }
});

worker.postMessage("Hello Worker!");

// With transferable objects
var imageBitmap = new ImageBitmap(bitmap);
worker.postMessage("Image data", new List<ITransferable> { imageBitmap });

worker.terminate();
```

### 6. SharedWorker API (HIGH PRIORITY) ✅

**Location:** `SharpCanvas/Context.Skia/SharedWorker.cs`

Shared worker that can be accessed from multiple contexts.

**Features:**
- Single instance shared across multiple connections
- Port-based communication via `MessagePort`
- Connect event for handling new connections
- Persistent across multiple clients

**Example:**
```csharp
// Create/connect to shared worker
var worker = new SharedWorker("my-worker", scope =>
{
    scope.OnConnect += (sender, e) =>
    {
        var port = e.Port;

        port.OnMessage += (msgSender, msgEvent) =>
        {
            // Handle message from a client
            port.postMessage($"Response: {msgEvent.Data}");
        };

        port.start();
    };
});

worker.Start();

// Communicate via port
worker.port.OnMessage += (sender, e) =>
{
    Console.WriteLine($"Received: {e.Data}");
};

worker.port.start();
worker.port.postMessage("Hello Shared Worker!");

// Multiple contexts can connect to the same worker
var worker2 = new SharedWorker("my-worker"); // Same instance
```

### 7. WorkerCanvas Utilities (UTILITY) ✅

**Location:** `SharpCanvas/Context.Skia/WorkerCanvas.cs`

High-level helper functions for common worker + canvas patterns.

#### RenderAsync
Renders canvas content in a worker and returns ImageBitmap.

```csharp
var imageBitmap = await WorkerCanvas.RenderAsync(200, 200, document, canvas =>
{
    var ctx = canvas.getContext("2d");
    ctx.fillStyle = "blue";
    ctx.fillRect(0, 0, 200, 200);
});
```

#### RenderToBlobAsync
Renders and exports as blob in one operation.

```csharp
var pngBytes = await WorkerCanvas.RenderToBlobAsync(
    100, 100, document,
    canvas => { /* draw */ },
    "image/png", 1.0
);
```

#### RenderFramesAsync
Parallel frame rendering for animations.

```csharp
var frames = await WorkerCanvas.RenderFramesAsync(30, 1920, 1080, document,
    (canvas, frameIndex) =>
    {
        var ctx = canvas.getContext("2d");
        // Draw frame based on frameIndex
    }
);
```

#### CreateSharedRenderer
Collaborative rendering via SharedWorker.

```csharp
var renderer = WorkerCanvas.CreateSharedRenderer(
    "video-renderer", 1920, 1080, document,
    (canvas, command) =>
    {
        // Process render commands
    }
);
```

#### CreateStreamingRenderer
Continuous frame processing.

```csharp
var renderer = WorkerCanvas.CreateStreamingRenderer(
    800, 600, document,
    (canvas, command) =>
    {
        // Process each frame
    }
);

renderer.OnMessage += (sender, e) =>
{
    // Handle rendered frames
};

renderer.postMessage(new { type = "draw", color = "red" });
```

### 8. ImageBitmapBridge (COMPATIBILITY) ✅

**Location:** `SharpCanvas/Context.Skia/ImageBitmapBridge.cs`

Compatibility layer between ImageBitmap and other bitmap formats.

**Features:**

#### System.Drawing.Bitmap Conversion (Windows only)
```csharp
#if WINDOWS
var systemBitmap = new System.Drawing.Bitmap(100, 100);
var imageBitmap = ImageBitmapBridge.FromSystemBitmap(systemBitmap);

var backToSystem = ImageBitmapBridge.ToSystemBitmap(imageBitmap);
#endif
```

#### Raw Pixel Data
```csharp
byte[] pixels = new byte[width * height * 4]; // RGBA
var imageBitmap = ImageBitmapBridge.FromRawPixels(pixels, width, height);

byte[] extractedPixels = ImageBitmapBridge.ToRawPixels(imageBitmap);
```

#### File I/O
```csharp
var imageBitmap = ImageBitmapBridge.FromFile("image.png");

ImageBitmapBridge.SaveToFile(imageBitmap, "output.png", SKEncodedImageFormat.Png, 100);
```

### 9. Updated CanvasWorker ✅

**Location:** `SharpCanvas/Context.Skia/CanvasWorker.cs`

Simple helper for background canvas rendering (now uses ImageBitmap).

```csharp
var worker = new CanvasWorker();

worker.OnWorkComplete += (sender, imageBitmap) =>
{
    // Use the rendered ImageBitmap
    Console.WriteLine($"Rendered: {imageBitmap.width}x{imageBitmap.height}");
};

worker.Run(canvas =>
{
    var ctx = canvas.getContext("2d");
    ctx.fillStyle = "red";
    ctx.fillRect(0, 0, 100, 100);
}, 200, 200, document);
```

## Testing

### ImageBitmap Tests
**Location:** `SharpCanvas.Tests/Tests.Skia.Modern/ImageBitmapTests.cs`

- Creation from various sources
- Resizing and cropping
- Options handling
- close() and disposal
- Transferable semantics
- Integration with drawImage()
- OffscreenCanvas integration
- Blob conversion

### Worker Tests
**Location:** `SharpCanvas.Tests/Tests.Skia.Modern/WorkerTests.cs`

- Message passing
- Transferable objects
- Worker termination
- SharedWorker single instance
- SharedWorker message passing
- Multiple connections
- WorkerCanvas utilities
- Streaming renderer
- Frame rendering

### OffscreenCanvas Tests
**Location:** `SharpCanvas.Tests/Tests.Skia.Modern/OffscreenCanvasTests.cs`

- Basic worker usage
- ImageBitmap integration
- Updated to use new API

## Use Cases

### 1. Lottie Video Rendering
```csharp
// Render animation frames in parallel
var frames = await WorkerCanvas.RenderFramesAsync(
    frameCount, 1920, 1080, document,
    (canvas, frameIndex) =>
    {
        var ctx = canvas.getContext("2d");
        // Render Lottie frame
        lottieAnimation.RenderFrame(ctx, frameIndex);
    }
);

// Export frames
foreach (var (frame, index) in frames.Select((f, i) => (f, i)))
{
    var blob = await OffscreenCanvas.convertToBlob("image/png");
    File.WriteAllBytes($"frame_{index:D4}.png", blob);
}
```

### 2. Real-time Image Processing
```csharp
var worker = WorkerCanvas.CreateStreamingRenderer(
    1280, 720, document,
    (canvas, command) =>
    {
        var ctx = canvas.getContext("2d");
        if (command is ImageData imageData)
        {
            // Apply filters, transformations, etc.
            ctx.putImageData(imageData, 0, 0);
            // ... processing ...
        }
    }
);

// Feed frames
worker.postMessage(videoFrame);
```

### 3. Collaborative Rendering
```csharp
var sharedRenderer = new SharedWorker("render-engine", scope =>
{
    scope.OnConnect += (sender, e) =>
    {
        var port = e.Port;
        port.OnMessage += (s, msg) =>
        {
            // Render and send back to all connected clients
        };
        port.start();
    };
});
```

## Performance Characteristics

- **Zero-copy transfers**: Transferable objects avoid memory duplication
- **Parallel rendering**: Multiple frames can render simultaneously
- **Async operations**: Non-blocking canvas operations
- **Resource management**: Explicit cleanup with close() and Dispose()
- **Shared workers**: One instance serves multiple clients

## Browser API Compatibility

This implementation follows the Web APIs specification:
- [ImageBitmap](https://html.spec.whatwg.org/multipage/imagebitmap-and-animations.html#imagebitmap)
- [createImageBitmap()](https://html.spec.whatwg.org/multipage/imagebitmap-and-animations.html#dom-createimagebitmap)
- [OffscreenCanvas](https://html.spec.whatwg.org/multipage/canvas.html#the-offscreencanvas-interface)
- [Worker](https://html.spec.whatwg.org/multipage/workers.html#dedicated-workers-and-the-worker-interface)
- [SharedWorker](https://html.spec.whatwg.org/multipage/workers.html#shared-workers-and-the-sharedworker-interface)
- [Transferable Objects](https://html.spec.whatwg.org/multipage/structured-data.html#transferable-objects)

## Migration Guide

### From Old CanvasWorker
```csharp
// OLD
worker.OnWorkComplete += (sender, skBitmap) =>
{
    // skBitmap was SKBitmap
};

// NEW
worker.OnWorkComplete += (sender, imageBitmap) =>
{
    // imageBitmap is ImageBitmap
    var skBitmap = imageBitmap.GetBitmap(); // If needed
};
```

### From Direct SKBitmap Usage
```csharp
// OLD
var skBitmap = canvas.transferToImageBitmap(); // Returned SKBitmap

// NEW
var imageBitmap = canvas.transferToImageBitmap(); // Returns ImageBitmap
imageBitmap.close(); // Don't forget cleanup
```

## Summary

This implementation provides a complete, spec-compliant ImageBitmap API and Web Workers implementation for SharpCanvas, enabling:

✅ High-performance image handling
✅ Zero-copy worker transfers
✅ Parallel rendering capabilities
✅ Frame capture and export
✅ Multi-threaded canvas operations
✅ Shared worker instances
✅ Legacy compatibility via bridges

Perfect for video processing, animation rendering, and performance-critical canvas applications.
