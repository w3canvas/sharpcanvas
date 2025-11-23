# WASM Workload Installation Status

## Current Situation

We are experiencing issues installing the `wasm-tools` and `wasm-tools-net8` workloads required for standalone WASM console application compilation.

### Error Encountered

```
NETSDK1147: To build this project, the following workloads must be installed: wasm-tools-net8
To install these workloads, run the following command: dotnet workload restore
```

### Installation Attempts Failed

Multiple automated installation attempts have failed with errors:

```
Workload installation failed: Value cannot be null. (Parameter 's')
Installation rollback failed: Value cannot be null. (Parameter 's')
```

The installation process appears to fail when downloading `microsoft.net.sdk.android.manifest-9.0.100.msi.x64 (35.0.105)`.

## Manual Installation Command (Windows)

Run from an **elevated PowerShell or Command Prompt**:

```powershell
dotnet workload install wasm-tools
```

Alternative commands if the above fails:

```powershell
# More specific version
dotnet workload install wasm-tools-net8

# Or restore for specific project
dotnet workload restore --project SharpCanvas.Wasm.Console\SharpCanvas.Wasm.Console.csproj
```

## Related GitHub Issue

The user pointed to this issue thread for reference:
**[dotnet/aspnetcore#64009](https://github.com/dotnet/aspnetcore/issues/64009)**

This issue discusses Blazor WASM download performance problems in .NET 10 RC1:
- Excessive WebAssembly file downloads during startup
- Performance degradation (10 seconds locally, several minutes remotely)
- Related to changes from custom browser cache to standard HTTP caching
- Partially resolved in RC2 with ongoing optimizations

**Note:** While this issue is about runtime performance rather than workload installation, it may be relevant for understanding WASM deployment challenges in .NET 10.

## Current Project Status

### ✅ Working Components

1. **Blazor WebAssembly** - `SharpCanvas.Blazor.Wasm` compiles and runs successfully on http://localhost:5233
   - Uses standard Blazor WASM template with built-in workload support
   - Includes interactive Canvas component with 4 demo modes
   - Successfully generates and displays canvas output in browser

2. **Wasmtime Installation** - Version 39.0.0 installed and validated
   - Path: `C:\Users\[user]\.wasmtime\bin\wasmtime.exe`
   - Can validate existing WASM files
   - Ready for direct execution testing once console app builds

3. **JavaScript Integration** - ClearScript V8 engine fully tested
   - 5/5 comprehensive tests passing
   - PNG output generation confirmed

4. **Test Suite** - 229/229 SkiaSharp tests passing

### ⏳ Blocked Components

1. **Standalone WASM Console App** - `SharpCanvas.Wasm.Console`
   - Project created but cannot build without `wasm-tools-net8` workload
   - Intended for direct wasmtime execution (headless rendering)
   - Would prove WASM compatibility outside browser environment

2. **Direct Wasmtime Testing** - Cannot proceed until console app builds
   - Goal: Run `wasmtime run bin\Debug\net8.0\browser-wasm\AppBundle\SharpCanvas.Wasm.Console.wasm`
   - Would demonstrate true headless canvas rendering
   - Would validate WASI build compatibility

## Package Size Expectations

Once workload is installed and console app builds:

- **Full Blazor package**: ~38 MB (includes all framework components)
- **Minimal wasmtime-only**: ~12 MB (core runtime + SharpCanvas + SkiaSharp)
- **With AOT/trimming**: ~6 MB (optimized for production)

See `WASM_PACKAGE_SIZES.md` for detailed breakdown.

## Execution Model Clarification

We have two different WASM execution approaches:

### 1. Browser/Node.js Serving (Currently Working)
- **File**: `test-nodejs-wasm.mjs`
- **Purpose**: HTTP server that serves WASM files to browsers
- **Technology**: Node.js HTTP server + Browser runtime
- **Status**: ✅ Working (Blazor WASM runs in browser)
- **Use case**: Web applications, interactive UI

### 2. Direct Wasmtime Execution (Blocked)
- **File**: `test-wasmtime.sh` and standalone console app
- **Purpose**: Direct WASM execution without browser
- **Technology**: Wasmtime standalone runtime
- **Status**: ⏳ Blocked by workload installation
- **Use case**: Headless rendering, server-side image generation, CLI tools

See `WASM_CLARIFICATION.md` for detailed explanation of these execution models.

## Next Steps

1. **Complete workload installation** manually from elevated prompt (in progress)
2. **Build standalone console app** once workload is available
3. **Test direct wasmtime execution** with headless canvas rendering
4. **Validate WASI build** and confirm PNG output without browser
5. **Optimize package size** with AOT compilation and trimming

## Technical Notes

### Workload Architecture
- .NET workloads provide platform-specific build tools and runtime components
- `wasm-tools-net8` includes:
  - WASM-specific MSBuild targets
  - Browser runtime libraries
  - WASI runtime support
  - AOT compilation toolchain

### Why We Need This Workload
The Blazor WASM template includes workload support by default, but standalone WASM console applications require explicit workload installation to:
- Compile C# to WASM bytecode
- Link .NET runtime components for `browser-wasm` runtime identifier
- Generate AppBundle with proper file structure
- Support WASI system interface for file I/O

### Alternative Approaches If Installation Continues to Fail
1. Use Blazor WASM project exclusively (already working)
2. Create a minimal Blazor console template as workaround
3. Wait for .NET SDK updates that may resolve installation issues
4. Use Docker container with pre-installed workloads
