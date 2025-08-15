# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Partially Implemented Features
- **Text Rendering**: The `fontVariantCaps` property is not fully implemented. The `FontUtils` class needs to be updated to handle OpenType font features. **Note:** This is a complex task that requires using the `HarfBuzzSharp` library for text shaping. The current implementation using `SKShaper` is not sufficient. A deeper integration with `HarfBuzzSharp` is needed to correctly handle OpenType features. **Future work:** The SkiaSharp implementation will be updated in the future to a newer version with better HarfBuzz integration, which should make this task easier.

### SkiaSharp v3 Upgrade Research
As part of the effort to improve text rendering capabilities, research was conducted into upgrading the `SkiaSharp` dependency from version `2.88.8` to `3.119.0`. This upgrade is expected to provide better integration with the `HarfBuzzSharp` library, which is essential for advanced text shaping features required by properties like `fontVariantCaps`.

**Key Findings:**

- **Breaking Changes**: The upgrade to SkiaSharp v3 introduces significant changes. The library has been modernized to use newer .NET features such as `LibraryImport` and function pointers for native interop. This is expected to cause compilation errors that will need to be addressed by updating the existing codebase.
- **HarfBuzz Integration**: The SkiaSharp v3 releases include updated versions of `HarfBuzzSharp`. Specifically, version `3.118.0-preview.1.2` of SkiaSharp updates `HarfBuzz` to `8.3.1`. This is a positive indicator that the upgrade will provide the necessary improvements for text shaping.
- **Unified Versioning**: All related `SkiaSharp` packages (e.g., `SkiaSharp.HarfBuzz`, `SkiaSharp.NativeAssets.Linux.NoDependencies`, `SkiaSharp.Views.Desktop.Common`) must be updated to version `3.119.0` in unison to ensure compatibility.
- **.NET Framework Compatibility**: The SkiaSharp v3 series targets .NET 8, which aligns with the target framework of this project. No compatibility issues are expected in this regard.

**Conclusion:**
The upgrade is a necessary step towards implementing advanced text features. The next step is to perform the upgrade and address any resulting breaking changes.

## Known Build Issues
- **`SharpCanvas.Context.Drawing2D` Project Reference**: There is a persistent, transient build error (CS0117) where the `SharpCanvas.Context.Drawing2D` project is unable to find methods from the `SharpCanvas.Common` project, despite a valid project reference. This may be due to an issue in the build environment. The code has been committed with the correct references, but the project may not build successfully until the underlying issue is resolved.

