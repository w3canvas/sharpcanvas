# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Unimplemented Methods
- `ChangeSize(int width, int height, bool reset)`

### Known Issues
- **`putImageData` build error**: There is a persistent build error related to the `putImageData` method in `SkiaCanvasRenderingContext2DBase.cs`. The error is `CS1503: Argument 2: cannot convert from 'byte[]' to 'nint'`, and it occurs even when the method body is commented out. This suggests a problem with the build environment or a dependency issue that needs further investigation.

### Partially Implemented Features
- **Text Rendering**: The `fontVariantCaps` property is not fully implemented.
- **`filter` property**: The `filter` property is only partially implemented. Support for more filter functions is needed.

## 2. Legacy Code (`System.Drawing`) Known Issues
The following `FIXME` items exist in the legacy codebase and should be addressed.

- **`LegacyWindows/Context.Drawing2D/CanvasRenderingContext2D.cs`**
  - `FIXME: This library has not been converted to use the ObjectWithPrototype class.`
  - `FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).`
  - `FIXME: Cleanup and move to Share.`
  - `FIXME: Throw debug error.`
  - `FIXME: Wrap IExpando from Host. This one targets JScript.`

## 3. Missing Properties and Methods from MDN
The following properties and methods are defined in the MDN documentation for `CanvasRenderingContext2D` but are not present in the `ICanvasRenderingContext2D` interface.

### Missing Properties
- `direction`
- `filter`
- `fontKerning`
- `fontStretch`
- `fontVariantCaps`
- `imageSmoothingEnabled`
- `imageSmoothingQuality`
- `lang`
- `letterSpacing`
- `lineDashOffset`
- `textRendering`
- `wordSpacing`

### Missing Methods
- `createConicGradient()`
- `drawFocusIfNeeded()`
- `ellipse()`
- `getContextAttributes()`
- `getLineDash()`
- `getTransform()`
- `isContextLost()`
- `isPointInStroke()`
- `reset()`
- `resetTransform()`
- `roundRect()`
- `setLineDash()`
