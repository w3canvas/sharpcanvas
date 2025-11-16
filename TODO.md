# SharpCanvas Project TODO List (November 2025)

This document outlines the major tasks and improvements planned for the SharpCanvas project, updated to reflect the project's current stable state.

## 1. High-Priority Tasks

### Improve Documentation
Clear and accurate documentation is the highest priority.
- [ ] Write comprehensive documentation for both the `System.Drawing` and `SkiaSharp` rendering contexts.
- [ ] Provide clear examples of how to use SharpCanvas in different environments (e.g., Windows Forms, WPF, cross-platform).
- [ ] Document the build process and project structure.
- [ ] Update `IMPLEMENTATION_STATUS.md` to match the current feature set.

### Modernize Project Structure
The project structure can be simplified and modernized for easier maintenance and development.
- [ ] Update all NuGet dependencies to the latest stable versions.
- [ ] Improve and simplify the build scripts.
- [ ] Evaluate unifying the `LegacyWindows` and `SharpCanvas` projects using multi-targeting.

### Address Legacy Code Issues
The legacy `System.Drawing` codebase has several known issues that need to be addressed.
- [ ] Investigate and resolve the transient build error (CS0117) in `SharpCanvas.Context.Drawing2D`.
- [ ] Add comments to formally deprecate the `FIXME`s related to `IExpando` and `JScript` interop, as per `AGENTS.md`.

## 2. Future Enhancements

### Performance Optimization
- [ ] Benchmark rendering performance for complex scenes and animations.
- [ ] Identify and optimize any performance bottlenecks.

### Advanced Testing
- [ ] Increase test coverage for more complex and edge-case Canvas API features.
- [ ] Implement a visual regression testing suite using the existing test images in `SharpCanvas.Tests/Originals`.

## 3. Completed Milestones

- [x] **Skia Rendering Context Implementation**
  - The modern `SkiaCanvasRenderingContext2DBase` is feature-complete and stable.
  - All 206 tests in the `SharpCanvas.Tests.Skia.Modern` suite pass.
  - All core HTML5 Canvas API features are implemented, including gradients, patterns, shadows, text, image data, Path2D, and transformations.

- [x] **Unified Testing Framework**
  - An abstraction layer for tests allows them to run against both `System.Drawing` and `SkiaSharp` contexts.
  - A comprehensive test suite for `arc` and `arcTo` has been created and verified.
