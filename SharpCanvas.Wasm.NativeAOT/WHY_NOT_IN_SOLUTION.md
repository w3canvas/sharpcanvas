# Why This Project Is NOT in SharpCanvas.sln

The `SharpCanvas.Wasm.NativeAOT` project is **intentionally excluded** from the main solution file for the following reasons:

## 1. Experimental Status

This project uses experimental tooling that is not production-ready:
- `componentize-dotnet` (preview versions only)
- NativeAOT-LLVM compiler (experimental)
- WASI 0.2 target (not yet stable)

Including it in the main solution would make the entire solution unstable.

## 2. Platform Limitations

- **Windows only**: Currently, `componentize-dotnet` only works on Windows
- Adding this to the solution would break builds on macOS and Linux
- The main SharpCanvas library must remain cross-platform

## 3. Build Dependencies

This project requires:
- .NET 9 SDK (newer than main solution)
- Experimental NuGet package source
- Additional workload installations

Not all contributors or CI/CD systems will have these configured.

## 4. Separate Package Source

This project needs the experimental package source:
```
https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json
```

Adding this to the main solution would require all developers to configure it.

## 5. Unknown Compatibility

We don't yet know if:
- SkiaSharp works with NativeAOT-LLVM
- The native library bundling works
- File I/O works in WASI
- The package actually runs

Including it in CI/CD would cause build failures until these are proven.

## 6. Opt-In Philosophy

This is an **experiment** to explore future WASM compilation options:
- Developers who want to try it can build it separately
- The main build/test cycle remains stable
- No one is forced to install experimental tools

## How to Build

This project is designed to be built **independently**:

```bash
# Windows
cd SharpCanvas.Wasm.NativeAOT
build.cmd

# Linux/macOS (when supported)
cd SharpCanvas.Wasm.NativeAOT
chmod +x build.sh
./build.sh
```

Or manually:

```bash
cd SharpCanvas.Wasm.NativeAOT
dotnet restore
dotnet build -c Release
dotnet publish -c Release
```

## When Will It Be Included?

This project **may** be added to the solution when:

1. ✅ `componentize-dotnet` reaches stable release
2. ✅ Cross-platform support (Windows, Linux, macOS)
3. ✅ We confirm SkiaSharp compatibility with NativeAOT-LLVM
4. ✅ We confirm it produces working WASM output
5. ✅ The tooling is included in standard .NET SDK (no experimental sources)
6. ✅ It provides clear benefits over Blazor WASM AOT

Until then, it remains an **opt-in experiment**.

## Alternative: Conditional Build

If we want to add it to the solution in the future while keeping it opt-in, we could use:

```xml
<!-- In SharpCanvas.sln -->
<PropertyGroup>
  <BuildNativeAOT Condition="'$(BuildNativeAOT)' == ''">false</BuildNativeAOT>
</PropertyGroup>
```

Then build with:

```bash
# Normal build (excludes NativeAOT)
dotnet build

# Opt-in build (includes NativeAOT)
dotnet build -p:BuildNativeAOT=true
```

But for now, complete separation is safer and clearer.
