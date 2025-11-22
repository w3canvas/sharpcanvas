# Optional Extensions and Tools

This document outlines "clean" and "optional" additions to the SharpCanvas project. These are not core requirements but serve specific purposes for testing, performance analysis, and documentation. They should be implemented as separate, optional components that do not clutter the main solution unless explicitly included.

## 1. Blazor Razor Component (`SharpCanvas.Blazor`)

A wrapper component to easily use SharpCanvas within Blazor applications (both Server and WebAssembly).

### Concept
- **Project**: `SharpCanvas.Blazor` (Optional Library)
- **Component**: `<SharpCanvasView>`
- **Usage**:
  ```razor
  <SharpCanvasView Width="800" Height="600" OnRender="@DrawScene" />

  @code {
      void DrawScene(ICanvasRenderingContext2D ctx)
      {
          ctx.fillStyle = "red";
          ctx.fillRect(0, 0, 100, 100);
      }
  }
  ```
- **Implementation Details**:
  - For **Server-Side Blazor**: Renders to an image (SkiaSharp) and streams it to the client as an `<img>` source or via JS interop to a canvas.
  - For **WebAssembly**: Could potentially use SkiaSharp for WebAssembly directly or similar image streaming.
  - **Cleanliness**: Kept in a separate solution folder `Extensions` or a separate repository if needed.

## 2. Performance Benchmarking (`SharpCanvas.Benchmarks`)

A dedicated project for tracking performance metrics using **BenchmarkDotNet**.

### Concept
- **Project**: `SharpCanvas.Benchmarks` (Console App)
- **Tool**: [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)
- **Purpose**:
  - Measure regression in critical paths (e.g., `fillRect`, `drawImage`, `putImageData`).
  - Compare backends (Skia vs. System.Drawing).
  - Profile memory allocation.
- **Usage**:
  - Run only when needed for performance tuning or release verification.
  - `dotnet run -c Release --project SharpCanvas.Benchmarks`

### Example Benchmark
```csharp
[MemoryDiagnoser]
public class RenderingBenchmarks
{
    private SkiaCanvasRenderingContext2D _ctx;

    [GlobalSetup]
    public void Setup()
    {
        _ctx = new SkiaCanvasRenderingContext2D(800, 600);
    }

    [Benchmark]
    public void FillRect_1000_Times()
    {
        for (int i = 0; i < 1000; i++)
        {
            _ctx.fillRect(0, 0, 100, 100);
        }
    }
}
```

## 3. Documentation Generation (`DocFX`)

Automated API documentation generation using **DocFX**.

### Concept
- **Tool**: [DocFX](https://dotnet.github.io/docfx/)
- **Purpose**: Generate a static HTML documentation site from XML comments and Markdown files.
- **Integration**:
  - Keep the `docfx.json` and related files in a `docs/` folder.
  - Do not require DocFX to be installed to build the main project.
  - Run via a specific script or CI/CD step.
- **Output**: Professional API reference + Articles (from `AGENTS.md`, `STRUCTURE.md`, etc.).

## Implementation Strategy

To keep these "clean" and "optional":

1.  **Separate Solution Folder**: Group these under an `Optional` or `Tools` solution folder.
2.  **Conditional Build**: Ensure they don't break the main build if dependencies are missing.
3.  **Scripts**: Provide separate scripts to run them (e.g., `run_benchmarks.sh`, `build_docs.sh`).
