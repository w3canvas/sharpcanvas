# Unified Tests for SharpCanvas

This directory contains the unified testing framework for SharpCanvas, which allows tests to run against multiple canvas rendering backends.

## Quick Start

### Running Tests

```bash
# Run all unified tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~ArcTests"
dotnet test --filter "FullyQualifiedName~ArcToTests"

# Run with detailed output
dotnet test --verbosity detailed
```

### Project Structure

```
Tests.Unified/
├── ICanvasContextProvider.cs    # Abstraction for canvas backends
├── SkiaContextProvider.cs       # SkiaSharp implementation
├── UnifiedTestBase.cs           # Base class for all unified tests
├── ArcTests.cs                  # Comprehensive arc() tests
├── ArcToTests.cs                # Comprehensive arcTo() tests
└── README.md                    # This file
```

## Writing Tests

Create a new test class inheriting from `UnifiedTestBase`:

```csharp
using NUnit.Framework;

namespace SharpCanvas.Tests.Unified
{
    [TestFixture]
    public class MyFeatureTests : UnifiedTestBase
    {
        [TestCaseSource(typeof(UnifiedTestBase), nameof(GetContextProviders))]
        public void MyTest(ICanvasContextProvider provider)
        {
            Provider = provider;
            SetUp();

            try
            {
                // Arrange
                Context.fillStyle = "red";

                // Act
                Context.fillRect(10, 10, 50, 50);

                // Assert
                AssertPixelColor(35, 35, 255, 0, 0, 255);
            }
            finally
            {
                TearDown();
            }
        }
    }
}
```

## Available Helpers

### Assertion Helpers

- `GetPixel(x, y)` - Returns `(r, g, b, a)` tuple
- `AssertPixelColor(x, y, r, g, b, a, tolerance)` - Assert exact color
- `AssertPixelTransparent(x, y, tolerance)` - Assert alpha ≈ 0
- `AssertPixelOpaque(x, y, tolerance)` - Assert alpha ≈ 255

### Configuration

Override these properties in your test class:

```csharp
// Custom canvas size (default: 200x200)
protected override (int width, int height) CanvasDimensions => (400, 400);

// Enable saving test outputs on failure (default: false)
protected override bool SaveTestImages => true;
```

## Test Coverage

### Arc Tests (ArcTests.cs)

Tests the `arc()` method:
- ✅ Simple circles, half circles, quarter circles
- ✅ Clockwise and anticlockwise directions
- ✅ Full circles (>= 2π)
- ✅ Zero and negative radius handling
- ✅ Empty vs. existing path behavior
- ✅ Transform integration
- ✅ Stroke vs. fill rendering

### ArcTo Tests (ArcToTests.cs)

Tests the `arcTo()` method:
- ✅ Basic rounded corners
- ✅ Various radii (zero, small, large)
- ✅ Right angle, acute, and obtuse corners
- ✅ Collinear points handling
- ✅ Edge cases (same points, negative radius)
- ✅ Multiple consecutive calls (rounded rectangles)
- ✅ Transform integration

## Backend Implementations

### Currently Supported

1. **SkiaSharp** (`SkiaContextProvider`)
   - Cross-platform (Linux, macOS, Windows)
   - Modern implementation
   - Primary test target

### Future Support

2. **System.Drawing** (planned for Windows)
   - Windows-only legacy backend
   - Requires Windows-specific conditional compilation
   - Will enable parity testing

## Debugging Failed Tests

When a test fails, you can:

1. **Enable test image output**:
   ```csharp
   protected override bool SaveTestImages => true;
   ```

2. **Check console output** for pixel values:
   ```
   Red channel mismatch at (100, 100). Expected: 255, Actual: 250
   ```

3. **Inspect saved images** in `TestOutputs/` directory

## Implementation Notes

### Arc Implementation Analysis

**SkiaSharp Context** (`SkiaCanvasRenderingContext2DBase.cs:546-584`):
- ✅ Proper angle conversion (radians → degrees)
- ✅ Correct anticlockwise handling
- ✅ Follows HTML5 Canvas spec for moveTo/lineTo
- ✅ Clean implementation using `SKPath.AddArc`

**System.Drawing Context** (`CanvasRenderingContext2D.cs:1124-1153`):
- ⚠️ Complex angle calculation
- ⚠️ Manual handling of multiple angle cases
- ⚠️ More potential for edge case bugs
- 📝 Requires thorough testing

### ArcTo Implementation Analysis

**SkiaSharp Context** (`SkiaCanvasRenderingContext2DBase.cs:541-544`):
- ✅ Simple delegation to `SKPath.ArcTo`
- ✅ SkiaSharp handles tangent point calculations
- ✅ Minimal code, minimal bugs

**System.Drawing Context** (`CanvasRenderingContext2D.cs:1052-1096`):
- ⚠️ Manual tangent point calculation
- ⚠️ Complex trigonometry
- ⚠️ Custom helper methods
- 📝 Higher complexity, requires verification

## See Also

- `/UNIFIED_TESTING_STRATEGY.md` - Comprehensive strategy documentation
- `/TODO.md` - Project roadmap
- [HTML5 Canvas Specification](https://html.spec.whatwg.org/multipage/canvas.html) - Official spec
