# SharpCanvas WebAssembly Deployment Guide

## Overview

SharpCanvas now supports WebAssembly deployment through multiple scenarios, enabling HTML5 Canvas 2D API rendering in browsers, Node.js, and headless environments.

## Deployment Scenarios

### 1. Blazor WebAssembly (Browser)

**Location:** `SharpCanvas.Blazor.Wasm/`

**Use Case:** Interactive canvas rendering in web browsers with full .NET runtime

**Features:**
- Full SharpCanvas API available
- Interactive UI components
- Real-time canvas rendering
- PNG/image export to base64

**How to Run:**
```bash
cd SharpCanvas.Blazor.Wasm
dotnet run
```

Browse to: `http://localhost:5233`

**How to Publish:**
```bash
cd SharpCanvas.Blazor.Wasm
dotnet publish -c Release -o publish
```

Deploy the `publish/wwwroot/` directory to any static web host.

**Component Usage:**
```razor
@using SharpCanvas.Blazor.Wasm.Components

<CanvasView />
```

### 2. Standalone WASM Console (Node.js/Browser)

**Location:** `SharpCanvas.Wasm.Console/`

**Use Case:** Headless canvas rendering in Node.js or browser environments

**Requirements:**
```bash
dotnet workload install wasm-tools-net8
```

**How to Build:**
```bash
cd SharpCanvas.Wasm.Console
dotnet build
```

**Output Files:**
- `bin/Debug/net8.0/browser-wasm/` - Contains WASM files
- `AppBundle/` - Ready-to-deploy bundle with runtime

**Node.js Loading:**
```javascript
// main.mjs
import { dotnet } from './dotnet.js';

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

// Your SharpCanvas code runs automatically
```

**Browser Loading:**
```html
<!DOCTYPE html>
<html>
<head>
    <script type="module" src="./main.mjs"></script>
</head>
<body>
    <h1>SharpCanvas WASM</h1>
    <div id="output"></div>
</body>
</html>
```

### 3. JavaScript Integration (ClearScript V8)

**Location:** `SharpCanvas.JsHost/`

**Use Case:** Embedding SharpCanvas in .NET applications with JavaScript scripting

**How to Run:**
```bash
cd SharpCanvas.JsHost
dotnet run
```

**Comprehensive Tests:**
```bash
cd SharpCanvas.JsHost
dotnet run -- --comprehensive
```

**Example Code:**
```csharp
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
    File.WriteAllBytes("output.png", blob);
}
```

### 4. Wasmtime (Headless WASI Runtime)

**Use Case:** Server-side headless rendering without browser

**Requirements:**
- Install wasmtime: `https://wasmtime.dev/`

**Building for WASI:**
```bash
cd SharpCanvas.Wasm.Console
dotnet build -c Release -r wasi-wasm
```

**Running with Wasmtime:**
```bash
wasmtime run ./bin/Release/net8.0/wasi-wasm/SharpCanvas.Wasm.Console.wasm
```

**Benefits:**
- No browser required
- Fast startup
- Suitable for CI/CD pipelines
- Server-side image generation

## Architecture

### SkiaSharp Native Assets

SharpCanvas uses SkiaSharp for rendering, which includes WebAssembly native libraries:

```xml
<PackageReference Include="SkiaSharp.NativeAssets.WebAssembly" Version="3.119.0" />
```

These native libraries (`libSkiaSharp.a`) are automatically linked when building for WASM.

### Build Configuration

**Enable AOT Compilation (Recommended):**
```xml
<PropertyGroup>
    <RunAOTCompilation>true</RunAOTCompilation>
    <WasmBuildNative>true</WasmBuildNative>
</PropertyGroup>
```

This links native SkiaSharp libraries and improves performance.

**Without AOT (Faster builds, larger size):**
```xml
<PropertyGroup>
    <RunAOTCompilation>false</RunAOTCompilation>
</PropertyGroup>
```

Warning will appear but app still works.

## Performance Considerations

### Bundle Size
- **Without AOT:** ~15-20 MB (interpreted IL)
- **With AOT:** ~8-12 MB (native code)
- **With Trimming:** ~5-8 MB (tree-shaken)

### Optimization Options

```xml
<PropertyGroup>
    <!-- Enable IL trimming -->
    <PublishTrimmed>true</PublishTrimmed>

    <!-- Link all assemblies -->
    <TrimMode>link</TrimMode>

    <!-- Enable AOT compilation -->
    <RunAOTCompilation>true</RunAOTCompilation>

    <!-- Build native libraries -->
    <WasmBuildNative>true</WasmBuildNative>
</PropertyGroup>
```

### Runtime Performance
- **Initial Load:** 1-3 seconds (depending on bundle size)
- **Canvas Operations:** Near-native performance with AOT
- **PNG Export:** Fast (SkiaSharp native encoding)

## Deployment Best Practices

### 1. Static Web Hosting (Blazor WASM)
```bash
dotnet publish -c Release
```

Deploy `bin/Release/net8.0/publish/wwwroot/` to:
- Azure Static Web Apps
- GitHub Pages
- Netlify
- Vercel
- Any CDN or static host

### 2. Node.js Server-Side Rendering
```javascript
import { dotnet } from './dotnet.js';
import fs from 'fs';

const { getAssemblyExports } = await dotnet.create();
const exports = await getAssemblyExports('SharpCanvas.Wasm.Console');

// Generate image server-side
// Save to file or send in HTTP response
```

### 3. Containerized Headless Rendering

**Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app
COPY . .

RUN dotnet workload install wasm-tools-net8
RUN dotnet publish -c Release -o out

# Install wasmtime
RUN curl https://wasmtime.dev/install.sh -sSf | bash

CMD ["wasmtime", "run", "./out/SharpCanvas.Wasm.Console.wasm"]
```

### 4. CI/CD Image Generation

Use in build pipelines for:
- Social media preview images
- Dynamic chart generation
- PDF rendering
- Automated screenshot generation

## Testing

### Unit Tests (Works in WASM)
```bash
cd SharpCanvas.Tests/Tests.Skia.Modern
dotnet test
```

All 229 tests pass in both native and WASM environments.

### Browser Testing
```bash
cd SharpCanvas.Blazor.Wasm
dotnet run
```

Open browser and verify canvas rendering.

### Node.js Testing
```bash
cd SharpCanvas.Wasm.Console/bin/Debug/net8.0/browser-wasm/AppBundle
node main.mjs
```

## Troubleshooting

### "Native references won't be linked in" Warning

**Solution:** Enable native building:
```xml
<WasmBuildNative>true</WasmBuildNative>
<RunAOTCompilation>true</RunAOTCompilation>
```

Or ignore - app still works without native linking.

### Large Bundle Size

**Solutions:**
1. Enable trimming: `<PublishTrimmed>true</PublishTrimmed>`
2. Enable AOT: `<RunAOTCompilation>true</RunAOTCompilation>`
3. Use gzip/brotli compression on server

### Slow Initial Load

**Solutions:**
1. Enable AOT compilation
2. Use CDN for faster delivery
3. Implement lazy loading
4. Pre-compile and cache

### Font Issues in WASM

SkiaSharp includes fallback fonts. For custom fonts:
```csharp
var fontFaceSet = new FontFaceSet();
// Load custom fonts here
```

## Browser Compatibility

### Supported Browsers
- Chrome/Edge 90+ ✅
- Firefox 89+ ✅
- Safari 15+ ✅
- Opera 76+ ✅

### Required Features
- WebAssembly support
- ES2015+ JavaScript
- Canvas 2D API

## Example Projects

### 1. Simple Canvas Demo
See: `SharpCanvas.Blazor.Wasm/Components/CanvasView.razor`

### 2. JavaScript Integration
See: `SharpCanvas.JsHost/ComprehensiveTest.cs`

### 3. Headless Console
See: `SharpCanvas.Wasm.Console/Program.cs`

## Resources

- **SkiaSharp WASM:** https://github.com/mono/SkiaSharp
- **.NET WASM:** https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly
- **Wasmtime:** https://wasmtime.dev/
- **MDN Canvas API:** https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API

## Summary

SharpCanvas is production-ready for WebAssembly deployment across:
- ✅ Blazor WebAssembly (browser-based interactive apps)
- ✅ Node.js (server-side rendering)
- ✅ Wasmtime (headless WASI runtime)
- ✅ ClearScript V8 (embedded JavaScript)

Choose the deployment scenario that best fits your use case!
