# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Partially Implemented Features
- **Text Rendering**: The `fontVariantCaps` property is not fully implemented. The `FontUtils` class needs to be updated to handle OpenType font features. **Note:** This is a complex task that requires using the `HarfBuzzSharp` library for text shaping. The current implementation using `SKShaper` is not sufficient. A deeper integration with `HarfBuzzSharp` is needed to correctly handle OpenType features. **Future work:** The SkiaSharp implementation will be updated in the future to a newer version with better HarfBuzz integration, which should make this task easier.
- **`filter` property**: The `filter` property has been enhanced to support `grayscale`, `sepia`, `contrast`, `hue-rotate`, `invert`, `opacity`, and `saturate` filters.

### SkiaSharp v3 Upgrade Research
As part of the effort to improve text rendering capabilities, research was conducted into upgrading the `SkiaSharp` dependency from version `2.88.8` to `3.119.0`. This upgrade is expected to provide better integration with the `HarfBuzzSharp` library, which is essential for advanced text shaping features required by properties like `fontVariantCaps`.

**Key Findings:**

- **Breaking Changes**: The upgrade to SkiaSharp v3 introduces significant changes. The library has been modernized to use newer .NET features such as `LibraryImport` and function pointers for native interop. This is expected to cause compilation errors that will need to be addressed by updating the existing codebase.
- **HarfBuzz Integration**: The SkiaSharp v3 releases include updated versions of `HarfBuzzSharp`. Specifically, version `3.118.0-preview.1.2` of SkiaSharp updates `HarfBuzz` to `8.3.1`. This is a positive indicator that the upgrade will provide the necessary improvements for text shaping.
- **Unified Versioning**: All related `SkiaSharp` packages (e.g., `SkiaSharp.HarfBuzz`, `SkiaSharp.NativeAssets.Linux.NoDependencies`, `SkiaSharp.Views.Desktop.Common`) must be updated to version `3.119.0` in unison to ensure compatibility.
- **.NET Framework Compatibility**: The SkiaSharp v3 series targets .NET 8, which aligns with the target framework of this project. No compatibility issues are expected in this regard.

**Conclusion:**
The upgrade is a necessary step towards implementing advanced text features. The next step is to perform the upgrade and address any resulting breaking changes.

## 2. Legacy Code (`System.Drawing`) Known Issues
The following `FIXME` items exist in the legacy codebase and should be addressed.

- **`SharpCanvas.Drawing/Context.Drawing2D/CanvasRenderingContext2D.cs`**
  - `FIXME: This library has not been converted to use the ObjectWithPrototype class.`
  - `FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).`
  - `FIXME: Cleanup and move to Share.`
  - `FIXME: Throw debug error.`
  - `FIXME: Wrap IExpando from Host. This one targets JScript.`
