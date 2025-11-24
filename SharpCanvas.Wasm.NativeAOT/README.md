# SharpCanvas NativeAOT-LLVM WASI (EXPERIMENTAL)

**Status**: Experimental / Opt-in
**Platform**: Windows only (currently)
**Build**: NOT included in main solution

## Overview

This project tests whether SharpCanvas can compile to native WebAssembly using **NativeAOT-LLVM** instead of the standard Blazor WASM runtime. This approach produces truly native WASM components that:

- ✅ Don't require the .NET runtime (smaller packages)
- ✅ Have better performance (native code compilation)
- ✅ Support WASI 0.2 (WebAssembly System Interface)
- ⚠️ Use experimental tooling (may not work)

## Why Separate Project?

This is **opt-in** and kept separate to avoid breaking the main build:

1. **Experimental tooling** - componentize-dotnet is in preview
2. **Windows-only** - Tooling doesn't support macOS/Linux yet
3. **Unknown compatibility** - SkiaSharp may not work with NativeAOT-LLVM
4. **Different build process** - Requires experimental package sources

## Prerequisites

### 1. Install .NET 9 SDK

```bash
# Download from https://dotnet.microsoft.com/download/dotnet/9.0
```

### 2. Add Experimental Package Source

```bash
dotnet nuget add source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json -n dotnet-experimental
```

### 3. Verify Package Source

```bash
dotnet nuget list source
```

You should see `dotnet-experimental` in the list.

## Building

**Important**: Build this project **separately** from the main solution.

```bash
# Navigate to this directory
cd SharpCanvas.Wasm.NativeAOT

# Restore dependencies (will download NativeAOT-LLVM tools)
dotnet restore

# Build
dotnet build -c Release

# Publish to WASM
dotnet publish -c Release
```

## Expected Results

### If It Works ✓

```
SharpCanvas NativeAOT-LLVM WASI Test
======================================

✓ Created canvas context
✓ Drew red rectangle
✓ Drew blue stroke rectangle
✓ Drew text
✓ Drew circle
✓ Generated PNG: 12345 bytes
✓ Wrote output-nativeaot.png

SUCCESS: All canvas operations completed!

NativeAOT Compatibility: ✓ CONFIRMED
```

Output location: `bin/Release/net9.0/wasi-wasm/publish/`

### If It Fails ✗

Potential failure points:

1. **SkiaSharp native dependencies**
   - SkiaSharp uses native libSkiaSharp.dll/so
   - NativeAOT-LLVM may not support bundling native libraries
   - Error: Unable to load shared library

2. **File I/O limitations**
   - WASI has different file system APIs
   - May not support writing PNG files
   - Error: PlatformNotSupportedException

3. **Trimming issues**
   - NativeAOT aggressively trims unused code
   - May trim reflection-based code
   - Error: MissingMethodException

## Running with Wasmtime

If build succeeds:

```bash
# Navigate to publish directory
cd bin/Release/net9.0/wasi-wasm/publish/

# Run with Wasmtime
wasmtime run SharpCanvas.Wasm.NativeAOT.wasm
```

With file system access (for output PNG):

```bash
wasmtime run --dir=. SharpCanvas.Wasm.NativeAOT.wasm
```

## Package Size Comparison

Expected sizes (if successful):

| Approach | Runtime | Size | Status |
|----------|---------|------|--------|
| Blazor WASM | Mono WASM | ~12 MB | ✓ Working |
| Blazor AOT | Mono WASM | ~6 MB | ✓ Expected |
| NativeAOT-LLVM | None | ~2-3 MB | ❓ Unknown |

## Enabling the SDK

To use the componentize-dotnet SDK, uncomment this in the `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="BytecodeAlliance.Componentize.DotNet.Wasm.SDK" Version="0.6.0-preview00009" />
</ItemGroup>
```

Or change the SDK to:

```xml
<Project Sdk="BytecodeAlliance.Componentize.DotNet.Wasm.SDK/0.6.0-preview00009">
```

## Troubleshooting

### Error: Unable to restore package

**Problem**: Can't find BytecodeAlliance.Componentize.DotNet.Wasm.SDK

**Solution**: Verify experimental package source is added:
```bash
dotnet nuget list source
```

### Error: WASI not supported

**Problem**: .NET 8 doesn't support WASI

**Solution**: Upgrade to .NET 9:
```bash
dotnet --version  # Should be 9.0.x
```

### Error: SkiaSharp native library not found

**Problem**: NativeAOT-LLVM can't bundle SkiaSharp's native dependencies

**Solution**: This is a known limitation. Possible approaches:
1. Wait for SkiaSharp WASM native builds
2. Use a different rendering backend
3. Stick with Blazor WASM approach

### Error: Project not building

**Problem**: Conflicting with main solution build

**Solution**: **Never** add this project to the main `.sln` file. Always build separately.

## Platform Support

| Platform | Support | Notes |
|----------|---------|-------|
| Windows | ✓ Yes | Full support |
| macOS | ⏳ Soon | Maintainers working on it |
| Linux | ⏳ Soon | Maintainers working on it |

## References

- [componentize-dotnet GitHub](https://github.com/bytecodealliance/componentize-dotnet)
- [Bytecode Alliance Blog Post](https://bytecodealliance.org/articles/simplifying-components-for-dotnet-developers-with-componentize-dotnet)
- [.NET WASI Native AOT Performance](https://byandriykozachuk.wordpress.com/2023/12/29/net-wasi-native-aot-performance/)
- [Build WebAssembly components with .NET](https://wasmcloud.com/blog/2024-09-05-build-wasm-components-dotnet-wasmcloud/)

## Status Tracking

- [ ] Project compiles
- [ ] Dependencies restore
- [ ] SkiaSharp loads
- [ ] Canvas operations work
- [ ] PNG generation works
- [ ] File I/O works
- [ ] Runs in Wasmtime
- [ ] Smaller than Blazor WASM

## Notes

This is an **experiment** to explore the future of .NET WASM compilation. Even if it fails now, it provides valuable insight into what's needed for true native WASM support.

The main SharpCanvas build uses Blazor WASM, which is production-ready and fully supported. This NativeAOT approach is exploratory only.
