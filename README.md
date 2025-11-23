# SharpCanvas

[![License: CC0](https://img.shields.io/badge/License-CC0-blue.svg)](http://creativecommons.org/publicdomain/zero/1.0/)

A cross-platform C# implementation of the HTML5 Canvas 2D rendering API, supporting both modern (SkiaSharp) and legacy (System.Drawing) backends.

## 🚀 Features

- **Full HTML5 Canvas API** - Complete implementation of the Canvas 2D rendering context
- **Cross-Platform** - Works on Windows, Linux, and macOS via SkiaSharp
- **Multiple Backends** - Modern SkiaSharp backend and legacy System.Drawing support
- **Production Ready** - 100% test pass rate with comprehensive test coverage
- **WebAssembly Support** - Run in browsers via Blazor WASM or headless with Wasmtime
- **Blazor Component** - Ready-to-use interactive Canvas component for Blazor apps
- **JavaScript Interoperability** - Full JavaScript integration via Microsoft.ClearScript V8
- **Performance** - Hardware-accelerated rendering through Skia
- **Accessibility** - Focus ring support for enhanced accessibility

## 📦 Quick Start

### Installation

Add SharpCanvas to your project:

```bash
dotnet add package SharpCanvas.Context.Skia
```

### Basic Usage

```csharp
using SharpCanvas.Context.Skia;
using SkiaSharp;

// Create a surface
var info = new SKImageInfo(800, 600);
var surface = SKSurface.Create(info);

// Create a canvas context
var document = new Document(); // or your IDocument implementation
var context = new SkiaCanvasRenderingContext2D(surface, document);

// Draw something
context.fillStyle = "red";
context.fillRect(10, 10, 100, 100);

context.strokeStyle = "blue";
context.lineWidth = 5;
context.strokeRect(150, 10, 100, 100);

// Draw text
context.font = "24px Arial";
context.fillStyle = "black";
context.fillText("Hello, SharpCanvas!", 10, 150);

// Export to image
byte[] pngBytes = context.GetBitmap();
```

### Advanced Example

```csharp
// Gradients
var gradient = context.createLinearGradient(0, 0, 200, 0);
gradient.addColorStop(0, "red");
gradient.addColorStop(0.5, "yellow");
gradient.addColorStop(1, "green");
context.fillStyle = gradient;
context.fillRect(10, 200, 200, 50);

// Transformations
context.save();
context.translate(100, 100);
context.rotate(Math.PI / 4);
context.fillStyle = "purple";
context.fillRect(-25, -25, 50, 50);
context.restore();

// Paths
context.beginPath();
context.arc(300, 100, 50, 0, 2 * Math.PI);
context.fillStyle = "orange";
context.fill();
context.strokeStyle = "black";
context.lineWidth = 2;
context.stroke();
```

## 🌐 WebAssembly and Blazor

SharpCanvas supports WebAssembly deployment for running .NET Canvas code in browsers and headless environments.

### Blazor WebAssembly Component

Use SharpCanvas in Blazor WASM applications:

```bash
cd SharpCanvas.Blazor.Wasm
dotnet run
```

Then navigate to http://localhost:5233 to see the interactive demo with 4 rendering modes:
- Basic shapes (rectangles, fills, strokes)
- Gradients (linear and radial)
- Paths (arcs, curves, bezier)
- Text rendering

### JavaScript Integration

SharpCanvas includes JavaScript engine integration via ClearScript V8:

```bash
cd SharpCanvas.JsHost
dotnet run
```

This runs comprehensive JavaScript-driven Canvas tests including:
- Basic drawing operations
- Path API (moveTo, lineTo, arc, curves)
- Transformations (translate, rotate, scale)
- Gradients and patterns
- Text rendering

All tests generate PNG output files for validation.

### Standalone WASM Execution

For headless WASM execution with Wasmtime (requires `wasm-tools-net8` workload):

```bash
# Install Wasmtime
curl https://wasmtime.dev/install.sh -sSf | bash

# Build WASM console app
cd SharpCanvas.Wasm.Console
dotnet build

# Run with Wasmtime
wasmtime run bin/Debug/net8.0/browser-wasm/AppBundle/SharpCanvas.Wasm.Console.wasm
```

**Note:** See [WASM_WORKLOAD_STATUS.md](WASM_WORKLOAD_STATUS.md) for workload installation instructions and troubleshooting.

### WASM Deployment Documentation

- [WASM Deployment Guide](WASM_DEPLOYMENT.md) - Comprehensive deployment instructions
- [WASM Workload Status](WASM_WORKLOAD_STATUS.md) - Installation troubleshooting
- [WASM Package Sizes](WASM_PACKAGE_SIZES.md) - Size analysis and optimization
- [WASM Clarification](WASM_CLARIFICATION.md) - Execution models explained

## 🏗️ Architecture

### Project Structure

```
SharpCanvas/
├── SharpCanvas.Core/          # Core interfaces and shared types
├── Context.Skia/              # Modern SkiaSharp backend (recommended)
├── Context.Drawing2D/         # Legacy System.Drawing backend (Windows only)
├── Context.WindowsMedia/      # Legacy WPF backend (Windows only)
├── SharpCanvas.Tests/         # Test suites
│   ├── Tests.Skia.Modern/    # Modern backend tests
│   ├── Tests.Unified/        # Cross-backend unified tests
│   └── Tests.Skia.Standalone/ # Standalone integration tests
├── SharpCanvas.JsHost/        # JavaScript host integration (ClearScript V8)
├── SharpCanvas.Blazor.Wasm/   # Blazor WebAssembly component
└── SharpCanvas.Wasm.Console/  # Standalone WASM console app (for Wasmtime)
```

### Backend Comparison

| Feature | SkiaSharp (Modern) | System.Drawing (Legacy) |
|---------|-------------------|------------------------|
| Cross-platform | ✅ Windows, Linux, macOS | ❌ Windows only |
| Performance | ⚡ Hardware-accelerated | 🐌 Software rendering |
| Test Pass Rate | ✅ 84.5% (174/206) | ⚠️ Not fully tested |
| Maintenance | ✅ Active | ⚠️ Maintenance mode |
| Recommended | ✅ Yes | ❌ Legacy only |

## 📚 API Documentation

### Core Canvas API

SharpCanvas implements the full HTML5 Canvas 2D API:

#### Drawing Rectangles
- `fillRect(x, y, width, height)` - Draw filled rectangle
- `strokeRect(x, y, width, height)` - Draw rectangle outline
- `clearRect(x, y, width, height)` - Clear rectangle area

#### Paths
- `beginPath()` - Start new path
- `closePath()` - Close current path
- `moveTo(x, y)` - Move to point
- `lineTo(x, y)` - Line to point
- `arc(x, y, radius, startAngle, endAngle, anticlockwise)` - Draw arc
- `arcTo(x1, y1, x2, y2, radius)` - Arc to point
- `quadraticCurveTo(cpx, cpy, x, y)` - Quadratic curve
- `bezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y)` - Bezier curve
- `ellipse(x, y, radiusX, radiusY, rotation, startAngle, endAngle, anticlockwise)` - Draw ellipse
- `rect(x, y, width, height)` - Add rectangle to path
- `roundRect(x, y, width, height, radii)` - Add rounded rectangle

#### Drawing Paths
- `fill()` / `fill(path)` - Fill current path or Path2D object
- `stroke()` / `stroke(path)` - Stroke current path or Path2D object
- `clip()` / `clip(path)` - Set clipping region

#### Text
- `fillText(text, x, y)` - Draw filled text
- `strokeText(text, x, y)` - Draw text outline
- `measureText(text)` - Measure text dimensions

#### Images
- `drawImage(image, dx, dy)` - Draw image
- `drawImage(image, dx, dy, dWidth, dHeight)` - Draw scaled image
- `drawImage(image, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight)` - Draw image slice

#### Transformations
- `translate(x, y)` - Translate origin
- `rotate(angle)` - Rotate coordinate system
- `scale(x, y)` - Scale coordinate system
- `transform(a, b, c, d, e, f)` - Apply transformation matrix
- `setTransform(a, b, c, d, e, f)` - Set transformation matrix
- `getTransform()` - Get current transformation
- `resetTransform()` - Reset to identity matrix

#### State Management
- `save()` - Save current state
- `restore()` - Restore previous state
- `reset()` - Reset to default state

#### Styles
- `fillStyle` - Fill color, gradient, or pattern
- `strokeStyle` - Stroke color, gradient, or pattern
- `lineWidth` - Line width
- `lineCap` - Line cap style (`"butt"`, `"round"`, `"square"`)
- `lineJoin` - Line join style (`"miter"`, `"round"`, `"bevel"`)
- `miterLimit` - Miter limit
- `setLineDash(segments)` - Set line dash pattern
- `getLineDash()` - Get line dash pattern
- `lineDashOffset` - Dash offset

#### Shadows
- `shadowColor` - Shadow color
- `shadowBlur` - Shadow blur radius
- `shadowOffsetX` - Shadow X offset
- `shadowOffsetY` - Shadow Y offset

#### Compositing
- `globalAlpha` - Global transparency (0.0 - 1.0)
- `globalCompositeOperation` - Compositing mode

#### Gradients and Patterns
- `createLinearGradient(x0, y0, x1, y1)` - Create linear gradient
- `createRadialGradient(x0, y0, r0, x1, y1, r1)` - Create radial gradient
- `createConicGradient(startAngle, x, y)` - Create conic gradient
- `createPattern(image, repetition)` - Create pattern

#### Image Data
- `getImageData(sx, sy, sw, sh)` - Get pixel data
- `putImageData(imageData, dx, dy)` - Put pixel data
- `createImageData(width, height)` - Create blank image data

#### Context State
- `isContextLost()` - Check if context is lost
- `getContextAttributes()` - Get context attributes

#### Accessibility
- `drawFocusIfNeeded(element)` - Draw focus ring if element focused

### Properties
- `font` - Text font
- `textAlign` - Text alignment (`"start"`, `"end"`, `"left"`, `"right"`, `"center"`)
- `textBaseLine` - Text baseline
- `direction` - Text direction (`"ltr"`, `"rtl"`)
- `imageSmoothingEnabled` - Enable/disable image smoothing
- `imageSmoothingQuality` - Image smoothing quality

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run modern backend tests only
dotnet test SharpCanvas.Tests/Tests.Skia.Modern/

# Run unified tests (cross-backend)
dotnet test SharpCanvas.Tests/Tests.Unified/

# Run with detailed output
dotnet test --verbosity detailed
```

### Test Coverage

- **Modern Backend**: 230/230 tests passing (100%)
- **Standalone Tests**: 1/1 tests passing (100%)
- **Core Tests**: 28/28 tests passing (100%)
- **Windows-specific Tests**: 28/28 tests passing (100%)
- **Total**: 287/287 tests passing (100%)

All tests pass successfully, including:
- All bezier curve and path operations
- All composite operations and blend modes
- All filter effects and combinations
- All transformation scenarios
- Workers and SharedWorker tests
- ImageBitmap and OffscreenCanvas tests

## 🛠️ Building from Source

### Prerequisites

- .NET SDK 8.0 or later
- SkiaSharp (automatically restored via NuGet)

### Build Steps

```bash
# Clone the repository
git clone https://github.com/w3canvas/sharpcanvas.git
cd sharpcanvas

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

### Building in Claude Code Web

If you encounter NuGet proxy authentication issues in Claude Code Web, use the provided proxy:

```bash
# Start the NuGet proxy
python3 .claude/nuget-proxy.py > /tmp/nuget_proxy.log 2>&1 &

# Set proxy environment variables
export all_proxy=http://127.0.0.1:8889
export ALL_PROXY=http://127.0.0.1:8889
export http_proxy=http://127.0.0.1:8889
export HTTP_PROXY=http://127.0.0.1:8889
export https_proxy=http://127.0.0.1:8889
export HTTPS_PROXY=http://127.0.0.1:8889

# Now build normally
dotnet restore
dotnet build
```

See [.claude/NUGET_PROXY_README.md](.claude/NUGET_PROXY_README.md) for details.

## 📖 Documentation

### Core Documentation
- [Production Readiness](PRODUCTION_READINESS.md) - Status, metrics, and deployment guide
- [Project Structure](STRUCTURE.md) - Architecture and organization
- [Roadmap](TODO.md) - Future enhancements and community contributions
- [Implementation Status](IMPLEMENTATION_STATUS.md) - Feature implementation details
- [Testing Instructions](TESTING_INSTRUCTIONS.md) - How to run tests
- [Unified Testing Strategy](UNIFIED_TESTING_STRATEGY.md) - Cross-backend testing
- [ImageBitmap and Workers](IMAGEBITMAP_AND_WORKERS.md) - Advanced features

### WebAssembly and Blazor Documentation
- [WASM Deployment Guide](WASM_DEPLOYMENT.md) - Complete WASM deployment instructions
- [WASM Workload Status](WASM_WORKLOAD_STATUS.md) - Workload installation and troubleshooting
- [WASM Package Sizes](WASM_PACKAGE_SIZES.md) - Package size analysis and optimization
- [WASM Clarification](WASM_CLARIFICATION.md) - Browser vs headless execution models
- [Completion Summary](COMPLETION_SUMMARY.md) - Full project implementation summary

## 🎯 Production Readiness

**SharpCanvas modern SkiaSharp backend is production-ready!**

### ✅ Fully Implemented Features
- ✅ Core Canvas API (rectangles, paths, text, images)
- ✅ All transformation operations
- ✅ Gradients and patterns (linear, radial, conic)
- ✅ Shadow effects
- ✅ Image data manipulation
- ✅ All compositing operations (25+ blend modes)
- ✅ Complete filter support (10 CSS filter functions)
- ✅ Accessibility features (drawFocusIfNeeded)
- ✅ Workers and SharedWorker support
- ✅ ImageBitmap and OffscreenCanvas
- ✅ Path2D reusable paths
- ✅ **100% test pass rate (287/287 tests)**

### ⚠️ Known Limitations
- Legacy System.Drawing backend is in maintenance mode
- Custom filter chains (`createFilterChain`) are Windows-only

### 🔜 Optional Future Enhancements
- Cross-platform custom filter chain support
- Performance optimizations for very large canvases
- Additional SVG path parsing features
- Legacy backend modernization (if needed)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests.

### Areas for Contribution

See [Roadmap](TODO.md) for detailed contribution opportunities.

**High-impact areas:**
1. **Examples and Samples** - Real-world usage examples and tutorials
2. **Performance** - Profile and optimize rendering for complex scenes
3. **Documentation** - Additional examples, translations, quick-start guides
4. **Platform Testing** - Test and optimize on different platforms
5. **Developer Tools** - Visual debuggers, profilers, and utilities

**Note:** The modern SkiaSharp backend is feature-complete. Focus contributions on enhancements, tools, and community support.

## 📄 License

Unless otherwise noted, all source code and documentation is released into the public domain under CC0.

- [CC0 1.0 Universal](http://creativecommons.org/publicdomain/zero/1.0/)
- [Public Domain Dedication](http://creativecommons.org/licenses/publicdomain/)

For questions about licensing, please contact:
- w3canvas at jumis.com

## 🙏 Credits

Developed by [Jumis, Inc.](http://jumis.com) and contributors.

Based on the HTML5 Canvas specification:
- [WHATWG Canvas Specification](https://html.spec.whatwg.org/multipage/canvas.html)

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/w3canvas/sharpcanvas/issues)
- **Discussions**: [GitHub Discussions](https://github.com/w3canvas/sharpcanvas/discussions)
- **Email**: w3canvas at jumis.com
