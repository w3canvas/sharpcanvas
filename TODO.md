# SharpCanvas Project TODO List

This document outlines the major tasks and improvements planned for the SharpCanvas project.

## 1. Skia Rendering Context Implementation Status
The `SkiaCanvasRenderingContext2DBase` is the modern, cross-platform backend for SharpCanvas. As of December 2024, the core Canvas API features have been implemented and validated.

### Completed Features (December 2024)
- [x] Support for gradients (linear, radial, conic) in `fillStyle` and `strokeStyle`
- [x] Pattern support with all repetition modes (`repeat`, `repeat-x`, `repeat-y`, `no-repeat`)
- [x] Shadow effects (`shadowColor`, `shadowOffsetX`, `shadowOffsetY`, `shadowBlur`)
- [x] Text rendering (`fillText`, `strokeText`) and measurement (`measureText`)
- [x] Image data manipulation (`getImageData`, `putImageData`, `createImageData`)
- [x] `globalAlpha` and `globalCompositeOperation`
- [x] Path2D reusable path objects with full API support
- [x] Complete transformation API (`translate`, `rotate`, `scale`, `transform`, `setTransform`, `getTransform`, `resetTransform`)
- [x] Validation and error handling for Canvas API methods

### Remaining Work
- [ ] Investigate and resolve any remaining edge cases in the test suite
- [ ] Performance optimization for complex rendering scenarios
- [ ] Complete documentation for advanced features

## 2. Address Legacy Code Issues
The legacy `System.Drawing` codebase has several `FIXME` comments that should be addressed.

- [ ] Investigate and fix the `FIXME` related to `ObjectWithPrototype`.
- [ ] Clean up and refactor code marked for cleanup.
- [ ] Resolve the `IExpando` and `JScript` interop issues.

## 3. Unify and Improve Testing
A robust test suite is needed to ensure consistency between the legacy and modern backends and to prevent regressions.

- [x] Create an abstraction layer for tests that allows them to run against both `System.Drawing` and `SkiaSharp` contexts. *(Completed: December 2024)*
  - Implemented `ICanvasContextProvider` abstraction in `SharpCanvas.Tests/Tests.Unified/`
  - Created `SkiaContextProvider` for SkiaSharp backend
  - Built `UnifiedTestBase` with helper methods for pixel-level assertions
  - Comprehensive arc/arcTo tests (25+ test cases) verify correctness
  - See `UNIFIED_TESTING_STRATEGY.md` for full documentation
- [ ] Increase test coverage for all Canvas API features.
- [ ] Use the existing test images in `SharpCanvas.Tests/Originals` to create a visual regression testing suite.

## 4. Improve Documentation
Clear documentation is essential for developers who want to use or contribute to SharpCanvas.

- [ ] Write comprehensive documentation for both the `System.Drawing` and `SkiaSharp` rendering contexts.
- [ ] Provide clear examples of how to use SharpCanvas in different environments (e.g., Windows Forms, WPF, cross-platform).
- [ ] Document the build process and project structure.

## 5. Modernize Project Structure
The project structure can be simplified and modernized.

- [ ] Evaluate the possibility of unifying the `LegacyWindows` and `SharpCanvas` projects using multi-targeting to a greater extent.
- [ ] Update dependencies to the latest stable versions.
- [ ] Improve the build scripts for easier development and deployment.
