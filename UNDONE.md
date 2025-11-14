# SharpCanvas Project - Undone Features

This document lists features that are currently unimplemented, incomplete, or have known issues. It serves as a detailed companion to `TODO.md`.

## 1. Skia Rendering Context (`SkiaCanvasRenderingContext2DBase`)
The following features are not yet implemented in the Skia backend. The goal is to achieve feature parity with the legacy `System.Drawing` context.

### SkiaSharp v3 Upgrade Complete
The project's `SkiaSharp` dependencies have been successfully upgraded from version `2.88.8` to `3.119.0`. All resulting compilation errors and warnings caused by breaking changes in the new version have been resolved. The project now builds cleanly.

**Conclusion:**
The upgrade is complete. The next major tasks are:
1.  **Fix `TestArc` failure**: The upgrade caused a regression in the `arc` method, which needs to be investigated (see "Known Test Failures").

## Known Test Failures
- **`TestArc` in `SharpCanvas.Tests.Skia.Modern`**: After upgrading to SkiaSharp v3, the `TestArc` test case began to fail. The test expects a filled arc to be drawn, but the resulting pixels are transparent, indicating the path is not being filled correctly. The exact cause is unknown and requires further investigation into the SkiaSharp v3 API for path creation and filling. This issue is deferred to allow the upgrade to be completed.

## Known Build Issues
- **`SharpCanvas.Context.Drawing2D` Project Reference**: There is a persistent, transient build error (CS0117) where the `SharpCanvas.Context.Drawing2D` project is unable to find methods from the `SharpCanvas.Common` project, despite a valid project reference. This may be due to an issue in the build environment. The code has been committed with the correct references, but the project may not build successfully until the underlying issue is resolved.

## Project Analysis (November 2025)

Based on a recent code review, the following is a summary of the project's current state:

**Implementation Status:** The project is incomplete. The modern SkiaSharp backend, which is the focus of future development, is missing significant features required for a complete HTML5 Canvas API implementation.

**Feature Gaps:**
As noted in `TODO.md`, the SkiaSharp implementation lacks several key features, including:
- Gradients and patterns
- Shadow effects
- Text rendering (`fillText`, `strokeText`)
- Image data manipulation

**Testing Gaps:**
The test suite for the modern backend has significant gaps and a large number of failing tests. While the `TestArc` failure is a known issue, numerous other failures in `Path2DTests`, `GradientAndPatternTests`, and `TransformationTests` indicate systemic problems in the current implementation. Furthermore, there is no unified testing strategy to ensure consistent behavior between the legacy and modern backends.

**Conclusion:**
The SharpCanvas project is a work in progress and requires significant effort to be considered a complete and modern implementation. Key areas for improvement are:
1.  **Feature Implementation:** Complete the missing features in the SkiaSharp backend.
2.  **Bug Fixing:** Address the large number of failing tests in the modern test suite.
3.  **Unified Testing:** Develop a testing strategy that allows for consistent testing across both backends.
