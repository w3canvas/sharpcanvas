# Unified Testing Strategy for SharpCanvas

## Overview

This document describes the unified testing strategy implemented for SharpCanvas, which allows tests to run against multiple canvas rendering backends (modern SkiaSharp and legacy System.Drawing contexts) using a single test codebase.

## Background

The SharpCanvas project has evolved from a legacy Windows-only implementation using System.Drawing to a modern cross-platform implementation using SkiaSharp. To ensure the modern implementation is a correct replacement for the legacy one, we need a testing strategy that can verify both implementations produce the same results.

## Architecture

### Core Components

1. **ICanvasContextProvider** - Abstract interface for creating and managing canvas contexts
   - Location: `SharpCanvas.Tests/Tests.Unified/ICanvasContextProvider.cs`
   - Provides methods for:
     - Creating contexts with specified dimensions
     - Getting pixel data for verification
     - Saving test outputs for debugging
     - Proper resource disposal

2. **SkiaContextProvider** - SkiaSharp implementation of the context provider
   - Location: `SharpCanvas.Tests/Tests.Unified/SkiaContextProvider.cs`
   - Creates `CanvasRenderingContext2D` instances backed by `SKSurface`
   - Provides pixel-level access for test assertions
   - Supports PNG output for visual debugging

3. **UnifiedTestBase** - Base class for unified tests
   - Location: `SharpCanvas.Tests/Tests.Unified/UnifiedTestBase.cs`
   - Provides common test infrastructure:
     - Setup/teardown lifecycle management
     - Helper methods for pixel assertions
     - Optional test image output on failure
     - Parameterized tests across all providers

## Test Structure

### Writing Unified Tests

Tests inherit from `UnifiedTestBase` and use `TestCaseSource` to run against all available providers:

```csharp
[TestFixture]
public class MyTests : UnifiedTestBase
{
    [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
    public void MyTest(ICanvasContextProvider provider)
    {
        Provider = provider;
        SetUp();

        try
        {
            // Test code using Context property
            Context.beginPath();
            Context.arc(100, 100, 50, 0, 2 * Math.PI, false);
            Context.fillStyle = "red";
            Context.fill();

            // Assertions using helper methods
            AssertPixelColor(100, 100, 255, 0, 0, 255);
        }
        finally
        {
            TearDown();
        }
    }
}
```

### Available Assertion Helpers

- `GetPixel(x, y)` - Get RGBA color at specific coordinates
- `AssertPixelColor(x, y, r, g, b, a, tolerance)` - Assert pixel matches expected color
- `AssertPixelTransparent(x, y, tolerance)` - Assert pixel is transparent
- `AssertPixelOpaque(x, y, tolerance)` - Assert pixel is opaque

## Arc and ArcTo Verification

### Arc Tests (`ArcTests.cs`)

Comprehensive tests for the `arc()` method covering:

1. **Basic Shapes**
   - Simple full circles
   - Half circles
   - Quarter circles

2. **Direction Handling**
   - Clockwise arcs
   - Anticlockwise arcs
   - Arcs larger than 2π

3. **Edge Cases**
   - Zero radius
   - Negative radius (should throw)
   - Empty vs. existing paths
   - Start angle > end angle

4. **Integration**
   - Arc with transformations
   - Stroked vs. filled arcs
   - Path connections

### ArcTo Tests (`ArcToTests.cs`)

Comprehensive tests for the `arcTo()` method covering:

1. **Basic Functionality**
   - Simple rounded corners
   - Various radii (zero, small, large)

2. **Geometric Cases**
   - Right angle corners (90°)
   - Acute angles (< 90°)
   - Obtuse angles (> 90°)
   - Collinear points

3. **Edge Cases**
   - Zero radius (straight line)
   - Negative radius (should throw)
   - Same start and p1
   - Same p1 and p2

4. **Complex Paths**
   - Multiple consecutive arcTo calls
   - Rounded rectangles
   - Integration with transforms

## Implementation Analysis

### Modern Skia Implementation
**File**: `SharpCanvas/Context.Skia/SkiaCanvasRenderingContext2DBase.cs`

**arc() method (lines 546-584)**:
- Converts radians to degrees
- Handles anticlockwise flag correctly
- Properly implements moveTo/lineTo behavior for empty/existing paths
- Uses `SKPath.AddArc` for the actual arc rendering

**arcTo() method (line 541-544)**:
- Delegates directly to `SKPath.ArcTo`
- SkiaSharp handles all the complex tangent point calculations

**Assessment**: ✅ Clean, relies on SkiaSharp's well-tested implementation

### Legacy System.Drawing Implementation
**File**: `SharpCanvas/Legacy/Drawing/Context.Drawing2D/CanvasRenderingContext2D.cs`

**arc() method (lines 1124-1153)**:
- Complex angle calculation logic
- Handles various start/end angle combinations
- Uses `GraphicsPath.AddArc`

**arcTo() method (lines 1052-1096)**:
- Manual calculation of tangent points using trigonometry
- Custom `FindTangentPoint` and `DrawArcBetweenTwoPoints` helpers
- More complex implementation with more room for edge case bugs

**Assessment**: ⚠️ More complex, requires thorough testing to verify correctness

### Path2D Implementation
**File**: `SharpCanvas/Context.Skia/Path2D.cs`

Both `arc()` (lines 75-106) and `arcTo()` (lines 70-73) implementations match the main context implementation.

**Assessment**: ✅ Consistent with main context

## Running Tests

### All Unified Tests
```bash
dotnet test SharpCanvas.Tests/Tests.Unified/
```

### Specific Test Class
```bash
dotnet test SharpCanvas.Tests/Tests.Unified/ --filter "FullyQualifiedName~ArcTests"
dotnet test SharpCanvas.Tests/Tests.Unified/ --filter "FullyQualifiedName~ArcToTests"
```

### With Detailed Output
```bash
dotnet test SharpCanvas.Tests/Tests.Unified/ --verbosity detailed
```

## Future Enhancements

### Adding System.Drawing Provider (Windows-only)

To enable testing against the legacy System.Drawing backend:

1. Create `SystemDrawingContextProvider.cs`:
```csharp
public class SystemDrawingContextProvider : ICanvasContextProvider
{
    public string Name => "System.Drawing";

    public ICanvasRenderingContext2D CreateContext(int width, int height)
    {
        // Implementation using System.Drawing
    }

    // ... implement other methods
}
```

2. Update `UnifiedTestBase.GetContextProviders()`:
```csharp
public static IEnumerable<ICanvasContextProvider> GetContextProviders()
{
    yield return new SkiaContextProvider();

    #if WINDOWS
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        yield return new SystemDrawingContextProvider();
    }
    #endif
}
```

### Expanding Test Coverage

Additional test suites that could use this framework:

- **BezierCurveTests** - quadraticCurveTo, bezierCurveTo
- **TransformationTests** - translate, rotate, scale, transform
- **GradientTests** - linear, radial, conic gradients
- **PatternTests** - pattern fills with different repetition modes
- **TextTests** - fillText, strokeText, measureText
- **ImageTests** - drawImage with various signatures
- **CompositeTests** - globalCompositeOperation modes
- **ClippingTests** - clip regions and hit testing

### Visual Regression Testing

The framework supports saving test outputs as PNG files. Future enhancements could include:

1. **Baseline Images** - Store expected output images
2. **Pixel Diff Comparison** - Compare test output to baselines
3. **Tolerance Configuration** - Allow per-test tolerance settings
4. **CI Integration** - Automatic visual regression detection

## Benefits

1. **Single Test Codebase** - Write tests once, run against all implementations
2. **Confidence in Migration** - Verify modern implementation matches legacy behavior
3. **Prevent Regressions** - Catch breaking changes in either implementation
4. **Cross-Platform** - Tests run on Linux, macOS, and Windows
5. **Easy Debugging** - Optional PNG output on test failures
6. **Comprehensive Coverage** - 25+ tests for arc/arcTo alone

## Test Results

The unified testing framework has been implemented with:

- **2 Test Classes**: ArcTests, ArcToTests
- **25+ Test Cases**: Covering all major scenarios and edge cases
- **Pixel-Level Verification**: Ensures visual correctness
- **Error Handling**: Verifies exceptions are thrown correctly
- **Transform Integration**: Tests interaction with canvas transformations

All tests use the modern SkiaSharp implementation as the primary backend. When run on Windows with the System.Drawing provider enabled, tests will execute against both implementations, ensuring behavioral parity.

## Conclusion

This unified testing strategy addresses the requirement mentioned in TODO.md:

> "Create an abstraction layer for tests that allows them to run against both System.Drawing and SkiaSharp contexts."

The implementation provides:
- ✅ Abstraction layer (ICanvasContextProvider)
- ✅ Skia implementation (SkiaContextProvider)
- ✅ Base test infrastructure (UnifiedTestBase)
- ✅ Comprehensive arc/arcTo tests (ArcTests, ArcToTests)
- ✅ Clear path for adding System.Drawing provider
- ✅ Documentation and examples

The project now has a solid foundation for ensuring the modern SkiaSharp implementation is a correct replacement for the legacy System.Drawing implementation.
