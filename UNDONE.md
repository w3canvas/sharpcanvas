# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Known Issues
- **`arc()` method not drawing**: The `arc()` method does not appear to draw anything to the canvas. The implementation logic for calculating the sweep angle based on the `anticlockwise` parameter appears to be correct according to the SkiaSharp documentation, but the unit test consistently fails, finding a transparent pixel where a filled one is expected. Debugging efforts, including using `fill()` instead of `stroke()`, simplifying the test case, and refactoring the implementation, have not resolved the issue. This requires further investigation.
- **Blocking Font Loading**: The current implementation of `FontFace` in the Skia backend uses a blocking `Wait()` on the font loading task within the `fillText` and `strokeText` methods. This is not ideal as it can block the UI thread. A more sophisticated, non-blocking approach should be investigated in the future. This would likely require changes to the `ICanvasRenderingContext2D` interface to support asynchronous drawing methods.

### Partially Implemented Features
- **Text Rendering**: The `fontVariantCaps` property is not fully implemented. The `FontUtils` class needs to be updated to handle OpenType font features.
- **`filter` property**: The `filter` property has been enhanced to support `grayscale` and `sepia` filters. However, other filters like `contrast`, `hue-rotate`, `invert`, `opacity`, and `saturate` are not yet implemented.

## 2. Legacy Code (`System.Drawing`) Known Issues
The following `FIXME` items exist in the legacy codebase and should be addressed.

- **`SharpCanvas.Drawing/Context.Drawing2D/CanvasRenderingContext2D.cs`**
  - `FIXME: This library has not been converted to use the ObjectWithPrototype class.`
  - `FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).`
  - `FIXME: Cleanup and move to Share.`
  - `FIXME: Throw debug error.`
  - `FIXME: Wrap IExpando from Host. This one targets JScript.`
