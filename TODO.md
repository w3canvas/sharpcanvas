# SharpCanvas Project TODO List

This document outlines the major tasks and improvements planned for the SharpCanvas project.

## 1. Complete Skia Rendering Context Implementation
The `SkiaCanvasRenderingContext2DBase` is the modern, cross-platform backend for SharpCanvas, but it is currently incomplete. The highest priority is to bring it to feature parity with the legacy `System.Drawing` implementation.

- [ ] Implement all missing methods (see `UNDONE.md` for a detailed list).
- [ ] Implement support for gradients and patterns in `fillStyle` and `strokeStyle`.
- [ ] Implement shadow effects (`shadowColor`, `shadowOffsetX`, `shadowOffsetY`, `shadowBlur`).
- [ ] Implement text rendering (`fillText`, `strokeText`) and measurement (`measureText`).
- [ ] Implement image data manipulation (`getImageData`, `putImageData`, `createImageData`).
- [ ] Implement `globalAlpha` and `globalCompositeOperation`.

## 2. Address Legacy Code Issues
The legacy `System.Drawing` codebase has several `FIXME` comments that should be addressed.

- [ ] Investigate and fix the `FIXME` related to `ObjectWithPrototype`.
- [ ] Clean up and refactor code marked for cleanup.
- [ ] Resolve the `IExpando` and `JScript` interop issues.

## 3. Unify and Improve Testing
A robust test suite is needed to ensure consistency between the legacy and modern backends and to prevent regressions.

- [ ] Create an abstraction layer for tests that allows them to run against both `System.Drawing` and `SkiaSharp` contexts.
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
