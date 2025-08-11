# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### Unimplemented Methods
- `prototype()`
- `__proto__`
- `fillText(string text, double x, double y)`
- `strokeText(string text, double x, double y)`
- `drawImage(...)` (all overloads)
- `isPointInPath(double x, double y)`
- `createLinearGradient(double x0, double y0, double x1, double y1)`
- `createPattern(object pImg, string repeat)`
- `createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)`
- `measureText(string text)`
- `getImageData(double sx, double sy, double sw, double sh)`
- `createImageData(double sw, double sh)`
- `putImageData(...)` (all overloads)
- `createFilterChain()`
- `ChangeSize(int width, int height, bool reset)`

### Partially Implemented Features
- **`fillStyle` and `strokeStyle`**: Currently only support solid color strings. Gradient and pattern support is missing.
- **Shadows**: The properties `shadowOffsetX`, `shadowOffsetY`, `shadowBlur`, and `shadowColor` are present but are not used in any drawing operations.
- **Text Rendering**: The properties `font`, `textAlign`, and `textBaseLine` exist, but the core text rendering methods (`fillText`, `strokeText`) are not implemented.
- **Compositing**: `globalAlpha` and `globalCompositeOperation` are defined but not yet applied to drawing operations.
- **Line Styles**: `lineCap`, `lineJoin`, `lineWidth`, and `miterLimit` properties are defined but are not fully wired up to the `SKPaint` object for all drawing operations.

## 2. Legacy Code (`System.Drawing`) Known Issues
The following `FIXME` items exist in the legacy codebase and should be addressed.

- **`LegacyWindows/Context.Drawing2D/CanvasRenderingContext2D.cs`**
  - `FIXME: This library has not been converted to use the ObjectWithPrototype class.`
  - `FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).`
  - `FIXME: Cleanup and move to Share.`
  - `FIXME: Throw debug error.`
  - `FIXME: Wrap IExpando from Host. This one targets JScript.`
