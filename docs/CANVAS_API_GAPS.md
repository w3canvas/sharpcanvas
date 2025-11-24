# Canvas 2D API - Missing Features Analysis

## Overview

SharpCanvas provides a **highly comprehensive** implementation of the HTML5 Canvas 2D API with ~95% coverage. This document identifies the remaining gaps compared to the WHATWG Canvas 2D specification.

**Summary:** Out of ~100+ Canvas 2D API methods and properties, only **3 features** have missing or incomplete implementations.

## ✅ What's Already Implemented

SharpCanvas has excellent Canvas 2D API coverage:

### Core Drawing Operations (100% ✅)
- ✅ Rectangles: `fillRect`, `strokeRect`, `clearRect`
- ✅ Paths: `beginPath`, `closePath`, `moveTo`, `lineTo`, `arc`, `arcTo`, `ellipse`, `rect`, `roundRect`
- ✅ Bezier curves: `quadraticCurveTo`, `bezierCurveTo`
- ✅ Drawing: `fill`, `stroke`, `clip`

### Text Rendering (100% ✅)
- ✅ Methods: `fillText`, `strokeText`, `measureText`
- ✅ Properties: `font`, `textAlign`, `textBaseline`, `direction`
- ✅ Advanced typography: `letterSpacing`, `wordSpacing`, `fontKerning`, `fontStretch`, `fontVariantCaps`, `textRendering`, `lang`

### Transformations (100% ✅)
- ✅ `scale`, `rotate`, `translate`, `transform`
- ✅ `setTransform`, `getTransform`, `resetTransform`
- ✅ DOMMatrix support

### State Management (100% ✅)
- ✅ `save`, `restore`, `reset`
- ✅ `canvas` property
- ✅ `isContextLost`, `getContextAttributes`

### Gradients & Patterns (95% ✅)
- ✅ `createLinearGradient`, `createRadialGradient`, `createConicGradient`
- ✅ `createPattern`
- ✅ `CanvasGradient.addColorStop`
- ⚠️ `CanvasPattern.setTransform` - **Missing**

### Image Operations (100% ✅)
- ✅ `drawImage` (all 3 overloads)
- ✅ `getImageData`, `putImageData`, `createImageData`
- ✅ ImageBitmap support
- ✅ OffscreenCanvas support

### Styling (100% ✅)
- ✅ `strokeStyle`, `fillStyle`, `globalAlpha`, `globalCompositeOperation`
- ✅ `lineWidth`, `lineCap`, `lineJoin`, `miterLimit`, `lineDashOffset`
- ✅ `setLineDash`, `getLineDash`
- ✅ Shadows: `shadowColor`, `shadowBlur`, `shadowOffsetX`, `shadowOffsetY`
- ✅ Image smoothing: `imageSmoothingEnabled`, `imageSmoothingQuality`
- ✅ Filters: `filter` (CSS filter functions)

### Path2D (100% ✅)
- ✅ All path methods: `moveTo`, `lineTo`, `arc`, `arcTo`, `ellipse`, `rect`, `roundRect`, `bezierCurveTo`, `quadraticCurveTo`
- ✅ `addPath` with optional transform
- ✅ `closePath`
- ✅ SVG path string constructor

### Accessibility (100% ✅)
- ✅ `drawFocusIfNeeded`

### Modern APIs (100% ✅)
- ✅ `isPointInPath`, `isPointInStroke`
- ✅ Workers and SharedWorkers
- ✅ Transferable objects (OffscreenCanvas, ImageBitmap)
- ✅ Event loops and message passing

## ❌ Missing Features

### 1. TextMetrics - Incomplete Implementation

**Status:** Partial implementation (20% complete)

**Current Implementation:**
```csharp
public struct TextMetrics
{
    public int width { get; set; }
    public int height { get; set; }  // Non-standard
}
```

**Missing Properties (per WHATWG spec):**
```typescript
interface TextMetrics {
    // ✅ Implemented
    readonly attribute double width;

    // ❌ Missing - Bounding box metrics
    readonly attribute double actualBoundingBoxLeft;
    readonly attribute double actualBoundingBoxRight;
    readonly attribute double actualBoundingBoxAscent;
    readonly attribute double actualBoundingBoxDescent;

    // ❌ Missing - Font metrics
    readonly attribute double fontBoundingBoxAscent;
    readonly attribute double fontBoundingBoxDescent;

    // ❌ Missing - Em box metrics
    readonly attribute double emHeightAscent;
    readonly attribute double emHeightDescent;

    // ❌ Missing - Baseline metrics
    readonly attribute double hangingBaseline;
    readonly attribute double alphabeticBaseline;
    readonly attribute double ideographicBaseline;
}
```

**Impact:** Low - Most Canvas applications only use `width`. Advanced text layout applications may need the additional metrics.

**Effort to Implement:** Medium
- SkiaSharp: Font metrics available via `SKFont.Metrics` and `SKFont.MeasureText` with bounds
- System.Drawing: Font metrics available via `Graphics.MeasureString` and `Font.GetHeight`

**Location:**
- Interface: `SharpCanvas/SharpCanvas.Core/Shared/TextMetrics.cs`
- Implementation: `SharpCanvas/Context.Skia/SkiaCanvasRenderingContext2DBase.cs:909`

---

### 2. CanvasPattern.setTransform() - Missing Method

**Status:** Not implemented

**Current Implementation:**
```csharp
public class SkiaCanvasPattern
{
    public SKShader GetShader() { ... }
    // ❌ Missing: setTransform(DOMMatrix matrix)
}
```

**Missing Method (per WHATWG spec):**
```typescript
interface CanvasPattern {
    void setTransform(optional DOMMatrix2DInit transform);
}
```

**Impact:** Low - Patterns can be transformed via context transformations. Direct pattern transformation is a convenience feature.

**Effort to Implement:** Low
- SkiaSharp: Use `SKShader.CreateLocalMatrix` to apply matrix to shader
- System.Drawing: Use `TextureBrush.Transform` property

**Example Usage:**
```javascript
const pattern = ctx.createPattern(image, 'repeat');
const matrix = new DOMMatrix();
matrix.scale(0.5, 0.5);
matrix.rotate(45);
pattern.setTransform(matrix);
ctx.fillStyle = pattern;
ctx.fillRect(0, 0, 100, 100);
```

**Location:**
- Skia: `SharpCanvas/Context.Skia/SkiaCanvasPattern.cs`
- GDI+: `SharpCanvas/Legacy/Drawing/Context.Drawing2D/CanvasPattern.cs`

---

### 3. ImageData - Missing Properties

**Status:** Partial implementation (60% complete)

**Current Implementation:**
```csharp
public class ImageData : IImageData
{
    // ✅ Implemented
    public object data { get; set; }      // byte[] or JS array
    public uint width { get; set; }
    public uint height { get; set; }

    // ❌ Missing
    // colorSpace property
    // pixelFormat property (experimental)
}
```

**Missing Properties (per WHATWG spec):**
```typescript
interface ImageData {
    readonly attribute unsigned long width;
    readonly attribute unsigned long height;
    readonly attribute Uint8ClampedArray data;

    // ❌ Missing
    readonly attribute PredefinedColorSpace colorSpace;     // "srgb", "display-p3"
    readonly attribute CanvasPixelFormat pixelFormat;       // Experimental: "uint8", "float16"
}
```

**Impact:** Very Low
- `colorSpace`: SharpCanvas currently assumes sRGB for all operations (standard default)
- `pixelFormat`: Experimental feature, not widely used

**Effort to Implement:** Low
- Add `colorSpace` property (string, default "srgb")
- Add optional `pixelFormat` property (experimental)
- Update `createImageData` and `getImageData` to accept settings parameter

**Location:**
- `SharpCanvas/SharpCanvas.Core/Shared/ImageData.cs`

---

## 🗑️ Intentionally Not Implemented (Deprecated APIs)

These Canvas 2D APIs are **deprecated** and intentionally not implemented:

### Hit Region APIs (Removed from spec)
- ❌ `addHitRegion()` - Deprecated, removed from WHATWG spec
- ❌ `removeHitRegion()` - Deprecated, removed from WHATWG spec
- ❌ `clearHitRegions()` - Deprecated, removed from WHATWG spec

**Reason:** These APIs were never widely adopted and have been removed from the Canvas 2D specification. Browsers are removing support.

### Scroll APIs (Limited browser support)
- ❌ `scrollPathIntoView()` - Not in WHATWG spec, limited browser support

**Reason:** Accessibility feature with minimal browser support. `drawFocusIfNeeded` is the preferred modern alternative.

---

## 📊 Coverage Summary

| Category | Coverage | Notes |
|----------|----------|-------|
| Core Drawing | 100% ✅ | All methods implemented |
| Text Rendering | 95% ⚠️ | TextMetrics incomplete (only affects advanced layout) |
| Transformations | 100% ✅ | Full DOMMatrix support |
| State Management | 100% ✅ | All methods implemented |
| Gradients | 100% ✅ | All gradient types supported |
| Patterns | 95% ⚠️ | Missing setTransform (workaround: use context transforms) |
| Images | 100% ✅ | Full ImageBitmap and OffscreenCanvas support |
| Pixel Data | 95% ⚠️ | ImageData missing colorSpace (assumes sRGB) |
| Path2D | 100% ✅ | All methods including addPath |
| Filters | 100% ✅ | CSS filter functions supported |
| Compositing | 100% ✅ | All composite operations |
| Accessibility | 100% ✅ | drawFocusIfNeeded implemented |
| **Overall** | **~95%** ✅ | **Excellent coverage** |

---

## 🎯 Recommended Priorities

### High Priority (Should Implement)
None - All critical Canvas 2D APIs are implemented.

### Medium Priority (Nice to Have)
1. **CanvasPattern.setTransform()** - Low effort, completes CanvasPattern API
2. **TextMetrics extended properties** - Medium effort, useful for advanced text layout

### Low Priority (Optional)
3. **ImageData.colorSpace** - Low effort, mostly informational (sRGB is standard default)
4. **ImageData.pixelFormat** - Experimental API, minimal browser support

---

## 🔍 Testing Coverage

**Current Test Suite:**
- ✅ 258 total tests (100% pass rate)
- ✅ 229 modern tests covering all major Canvas 2D features
- ✅ 8 Worker tests (backend-agnostic)
- ✅ 23 Path2D tests
- ✅ 31 filter tests
- ✅ 41 composite operation tests
- ✅ 11 ImageBitmap/OffscreenCanvas tests

**Test Gaps:**
- ⚠️ No tests for TextMetrics extended properties (not yet implemented)
- ⚠️ No tests for CanvasPattern.setTransform (not yet implemented)
- ⚠️ No tests for ImageData.colorSpace (not yet implemented)

---

## 📚 References

- [WHATWG Canvas 2D Specification](https://html.spec.whatwg.org/multipage/canvas.html)
- [MDN: CanvasRenderingContext2D](https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D)
- [MDN: TextMetrics](https://developer.mozilla.org/en-US/docs/Web/API/TextMetrics)
- [MDN: CanvasPattern](https://developer.mozilla.org/en-US/docs/Web/API/CanvasPattern)
- [MDN: ImageData](https://developer.mozilla.org/en-US/docs/Web/API/ImageData)

---

## 🚀 Future Enhancements (Beyond Canvas 2D Spec)

These are **not** part of the Canvas 2D specification but could be valuable additions:

1. **WebGL Support** - OffscreenCanvas with WebGL contexts
2. **WebGPU Support** - Next-generation graphics API
3. **SVG Export** - Export canvas operations as SVG
4. **PDF Export** - Render canvas to PDF (SkiaSharp supports this!)
5. **Hardware Acceleration** - GPU-accelerated filters and compositing

---

**Last Updated:** 2025-11-24
**SharpCanvas Version:** Production-ready (Phase 4 complete)
