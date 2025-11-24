# Experimental: NativeAOT-LLVM WASI Compilation

**Location**: `SharpCanvas.Wasm.NativeAOT/`
**Status**: Experimental / Opt-In
**Not included in main solution**

## What Is This?

An experimental project to test compiling SharpCanvas to **native WebAssembly** using NativeAOT-LLVM instead of the standard Blazor WASM runtime.

## Why Try It?

**Potential Benefits:**
- Much smaller package sizes (no .NET runtime needed)
- Better runtime performance (truly native code)
- WASI 0.2 compatibility (run outside browsers)
- Future-proof for .NET WASM evolution

**Current Limitations:**
- Experimental tooling (preview releases)
- Windows-only (for now)
- Unknown if SkiaSharp works with NativeAOT-LLVM
- Requires .NET 9 and experimental package sources

## How to Try It

```bash
cd SharpCanvas.Wasm.NativeAOT

# Windows
build.cmd

# Linux/macOS (when supported)
chmod +x build.sh
./build.sh
```

See `SharpCanvas.Wasm.NativeAOT/README.md` for full instructions.

## Why Not in Main Solution?

This project is kept separate to avoid breaking the main build with experimental dependencies. See `SharpCanvas.Wasm.NativeAOT/WHY_NOT_IN_SOLUTION.md` for details.

## Comparison: WASM Approaches

| Approach | Status | Size | Speed | Runtime |
|----------|--------|------|-------|---------|
| **Blazor WASM** | ✅ Production | ~12 MB | Good | Mono WASM |
| **Blazor WASM AOT** | ✅ Production | ~6 MB | Better | Mono WASM |
| **NativeAOT-LLVM** | ⚠️ Experimental | ~2-3 MB? | Best? | None |

## More Info

- [NativeAOT Project README](SharpCanvas.Wasm.NativeAOT/README.md)
- [Why Not in Solution](SharpCanvas.Wasm.NativeAOT/WHY_NOT_IN_SOLUTION.md)
- [componentize-dotnet GitHub](https://github.com/bytecodealliance/componentize-dotnet)
- [Bytecode Alliance Blog](https://bytecodealliance.org/articles/simplifying-components-for-dotnet-developers-with-componentize-dotnet)

## Help Wanted

If you successfully build and run this project, please report:
- Whether SkiaSharp loads
- Whether canvas operations work
- Actual package size
- Performance comparison

This will help determine if NativeAOT-LLVM is viable for SharpCanvas in the future.
