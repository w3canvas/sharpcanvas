# SharpCanvas Roadmap

**Last Updated:** November 2025

This document outlines future enhancements and optional improvements for the SharpCanvas project.

## Current Status

**The modern SkiaSharp backend is production-ready with 100% test success (286/286 tests).**

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

### ✅ Phase 3: Quality & Documentation (Completed November 2025)
- [x] 100% test pass rate (286 tests)
- [x] Comprehensive README with API reference
- [x] Project structure documentation (STRUCTURE.md)
- [x] Production readiness documentation
- [x] XML documentation for IntelliSense
- [x] Migration guides and examples

## Future Enhancements

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

### Legacy Backend (Low Priority)

**Priority: Low**

The System.Drawing and WPF backends are in maintenance mode. Consider these only if Windows-only support is required:

- [ ] Bring System.Drawing backend to feature parity
- [ ] Create unified test suite for all backends
- [ ] Document legacy backend limitations
- [ ] Provide migration tools

**Estimated Effort:** 8-12 weeks
**Recommendation:** Use SkiaSharp backend instead

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
