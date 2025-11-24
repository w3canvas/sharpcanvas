# SharpCanvas Roadmap

**Last Updated:** November 2025

This document outlines future enhancements and optional improvements for the SharpCanvas project.

## Current Status

**Both backends are production-ready:**
- **SkiaSharp Backend:** 287/287 tests passing (100%), cross-platform
- **System.Drawing Backend:** 100% Canvas API, 0 compilation errors, Windows-native

All core features are complete and fully tested. This roadmap focuses on optional enhancements and future improvements.

## Completed Milestones

### ✅ Phase 1: Core Implementation (Completed 2024)
- [x] Full HTML5 Canvas 2D API implementation
- [x] Gradients (linear, radial, conic) and patterns
- [x] Shadow effects and compositing operations
- [x] Text rendering with custom fonts
- [x] Image data manipulation
- [x] Path2D reusable path objects
- [x] Complete transformation API
- [x] Error handling and validation

### ✅ Phase 2: Advanced Features (Completed 2024-2025)
- [x] 10 CSS filter functions (blur, brightness, contrast, etc.)
- [x] 25+ composite operations and blend modes
- [x] Workers and SharedWorker support
- [x] ImageBitmap and OffscreenCanvas
- [x] Accessibility features (drawFocusIfNeeded)
- [x] Modern SkiaSharp APIs (replaced obsolete APIs)

### ✅ Phase 3: Quality & Documentation (Completed November 2024)
- [x] 100% test pass rate (287 tests)
- [x] Comprehensive README with API reference
- [x] Project structure documentation (STRUCTURE.md)
- [x] Production readiness documentation
- [x] XML documentation for IntelliSense
- [x] Migration guides and examples

### ✅ Phase 4: System.Drawing Backend & Deployment (Completed November 2024)
- [x] Fixed 52 compilation errors in System.Drawing backend
- [x] Complete Canvas Path API implementation (beginPath, moveTo, lineTo, arc, bezierCurveTo, etc.)
- [x] Text rendering with font parsing
- [x] Both backends production-ready
- [x] JavaScript integration via ClearScript V8
- [x] Blazor WebAssembly component with interactive demos
- [x] Standalone WASM console app for Wasmtime
- [x] Experimental NativeAOT project
- [x] Complete WASM deployment documentation

## Future Enhancements

### 🏗️ Architecture Refactoring (Phase 5 - Proposed)

**Priority: Medium**
**Status: Documented, awaiting implementation**

See [ARCHITECTURE_REFACTORING_PLAN.md](ARCHITECTURE_REFACTORING_PLAN.md) for complete details.

**Summary:**
- Extract runtime logic (Workers, Event Loop) from rendering backends
- Create `SharpCanvas.Runtime` project
- Implement `IGraphicsFactory` abstraction
- Platform-specific Event Loop implementations
- Better separation of concerns

**Benefits:**
- ✅ Easier to add new rendering backends (DirectX, WebGPU, etc.)
- ✅ Runtime logic reusable across all backends
- ✅ Platform-specific event loops (Console, WPF, Blazor)
- ✅ Better testability and maintainability

**Estimated Effort:** 18-28 hours
**Timeline:** Phase 2 / v2.0 (after WASM validation)

**Tasks:**
- [ ] Create SharpCanvas.Runtime project
- [ ] Extract Worker classes from Context.Skia
- [ ] Create IGraphicsFactory interface
- [ ] Implement platform-specific event loops
- [ ] Update all project references
- [ ] Verify all 287 tests still pass
- [ ] Update documentation

### Performance Optimizations (Optional)

**Priority: Medium**

- [ ] SIMD acceleration for filter operations
- [ ] Texture atlas for pattern rendering
- [ ] Advanced GPU memory management
- [ ] Lazy evaluation for transform chains
- [ ] Benchmark suite for performance tracking

**Estimated Effort:** 2-3 weeks

### Extended Features (Optional)

**Priority: Low-Medium**

- [ ] Additional SVG path parsing features
  - Complex path animations
  - Path morphing support
  - Extended path commands

- [ ] Advanced text features
  - Vertical text layout
  - Ruby annotations
  - Text decoration effects

- [ ] Extended filter support
  - Custom shader filters
  - Cross-platform createFilterChain()
  - Filter animation support

**Estimated Effort:** 1-2 weeks per feature

### Developer Tools (Optional)

**Priority: Low**

- [ ] Visual debugging utilities
  - Path visualization
  - Bounding box display
  - Performance overlay

- [ ] Performance profiling tools
  - Frame time analysis
  - Memory allocation tracking
  - GPU usage monitoring

- [ ] Code generation tools
  - Canvas to C# converter
  - SVG to Path2D generator

**Estimated Effort:** 2-4 weeks

### Additional Backend Testing (Optional)

**Priority: Low**

System.Drawing backend is production-ready but could benefit from:

- [ ] Comprehensive test suite for System.Drawing backend (currently: manual validation)
- [ ] Cross-backend compatibility tests
- [ ] Performance comparison benchmarks
- [ ] .NET Framework 4.x backward compatibility testing

**Estimated Effort:** 2-4 weeks
**Note:** System.Drawing is fully implemented and production-ready; this is purely for additional validation

### Build & Distribution (Optional)

**Priority: Medium**

- [ ] Publish to NuGet Gallery
  - SharpCanvas.Core package
  - SharpCanvas.Skia package
  - SharpCanvas.Legacy package (optional)

- [ ] CI/CD pipeline improvements
  - Automated testing on all platforms
  - Performance regression detection
  - Automated NuGet publishing

- [ ] Sample projects
  - WPF sample application
  - .NET MAUI sample
  - ASP.NET server-side rendering
  - Console chart/graph generation

**Estimated Effort:** 1-2 weeks

## Community Contributions Welcome

We welcome contributions in these areas:

### High Impact, Low Effort

1. **Examples and Samples**
   - Real-world usage examples
   - Tutorial articles
   - Video demonstrations

2. **Bug Reports**
   - Edge case identification
   - Performance issues
   - API inconsistencies

3. **Documentation**
   - Improve existing docs
   - Translate to other languages
   - Create quick-start guides

### Medium Impact, Medium Effort

1. **Performance Improvements**
   - Profile bottlenecks
   - Optimize hot paths
   - Reduce allocations

2. **Feature Enhancements**
   - Additional filter effects
   - Extended SVG support
   - New drawing primitives

3. **Platform Support**
   - Test on different platforms
   - Fix platform-specific issues
   - Optimize for mobile

### High Impact, High Effort

1. **Major Features**
   - WebGL 2D context support
   - Canvas recording/replay
   - Animation timeline API

2. **Tooling**
   - Visual debugger
   - Performance profiler
   - Testing framework

## Not Planned

These items are explicitly not planned:

- ❌ Breaking changes to stable APIs
- ❌ Platform-specific features (except in separate packages)
- ❌ Support for older .NET versions (< .NET 8)
- ❌ Legacy backend feature parity (use SkiaSharp instead)
- ❌ Non-standard Canvas extensions (unless widely adopted)

## How to Contribute

1. **Check existing issues** at https://github.com/w3canvas/sharpcanvas/issues
2. **Discuss proposals** before starting major work
3. **Follow code style** and existing patterns
4. **Add tests** for new features
5. **Update documentation** as needed

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines (if available).

## Release Planning

### Next Minor Release

**Target:** Q1 2026 (tentative)

Possible inclusions:
- Performance optimizations
- Additional samples
- Documentation improvements
- Community contributions

### Next Major Release

**Target:** Q3 2026 (tentative)

Possible inclusions:
- NuGet package releases
- Extended feature set
- Developer tools
- Major performance work

## Questions or Suggestions?

- GitHub Issues: https://github.com/w3canvas/sharpcanvas/issues
- GitHub Discussions: https://github.com/w3canvas/sharpcanvas/discussions
- Email: w3canvas@jumis.com

---

**Note:** This roadmap is subject to change based on community feedback and project priorities. All dates are tentative.
