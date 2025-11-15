# Edge Case Tests - Quick Reference

## 1. Bezier Stroke Rendering (6 failures)

### Minimal Test Case
```csharp
var surface = SKSurface.Create(new SKImageInfo(200, 200));
var context = new CanvasRenderingContext2D(surface, mockDocument);

context.strokeStyle = "blue";
context.lineWidth = 2;
context.beginPath();
context.moveTo(20, 20);
context.quadraticCurveTo(100, 20, 100, 100);
context.stroke();

// Check pixel (70, 40) - should have blue stroke
var pixel = GetPixel(70, 40);
Assert: pixel.Alpha > 0
```

**Expected**: Blue stroke visible at test pixel
**Actual**: Alpha = 0 (transparent)
**Note**: Filled curves work, TestCubicBezierSShape passes

### Failing Tests
- TestQuadraticCurveBasic
- TestCubicBezierBasic
- TestCubicBezierComplex
- TestMixedCurvesPath
- TestQuadraticCurveFromLine
- TestBezierWithTransform

## 2. Path2D Bezier Curves (3 failures)

### Minimal Test Case
```csharp
var path = new Path2D();
path.moveTo(20, 20);
path.quadraticCurveTo(100, 20, 100, 100);

context.strokeStyle = "red";
context.lineWidth = 2;
context.stroke(path);

// Check pixel (70, 40)
Assert: pixel.Alpha > 0
```

**Expected**: Stroke visible
**Actual**: Alpha = 0
**Note**: Same issue as #1 but in Path2D

### Failing Tests
- TestPath2DQuadraticCurve
- TestPath2DBezierCurve
- TestPath2DComplexShape

## 3. isPointInStroke (1 failure)

### Minimal Test Case
```csharp
context.beginPath();
context.moveTo(50, 50);
context.lineTo(150, 50);
context.lineWidth = 10;

var result = context.isPointInStroke(50, 55);
// Point is 5px away from line, lineWidth=10, so radius=5
// Point should be inside stroke bounds

Assert: result == true
```

**Expected**: true (point 5px from line with width=10)
**Actual**: false
**File**: SkiaCanvasRenderingContext2DBase.cs, isPointInStroke method

## 4. Radial Gradient Off-Center (1 failure)

### Minimal Test Case
```csharp
var gradient = context.createRadialGradient(75, 75, 10, 100, 100, 50);
gradient.addColorStop(0, "white");
gradient.addColorStop(1, "black");

context.fillStyle = gradient;
context.fillRect(0, 0, 200, 200);

var pixel = GetPixel(75, 75); // Inner circle center
Assert: pixel.Red > 150 (should be white/light)
```

**Expected**: Inner circle center is light (white)
**Actual**: R=0 (black)
**File**: SkiaRadialCanvasGradient.cs, GetShader method

## 5. Arc Anticlockwise (1 failure)

### Minimal Test Case
```csharp
// Red clockwise semicircle
context.fillStyle = "red";
context.beginPath();
context.arc(100, 100, 50, 0, Math.PI, false);
context.fill();

// Blue anticlockwise semicircle
context.fillStyle = "blue";
context.beginPath();
context.arc(100, 100, 50, 0, Math.PI, true);
context.fill();

// Should see both colors
Assert: Red pixel exists
```

**Expected**: Both semicircles visible
**Actual**: No red pixels found
**File**: SkiaCanvasRenderingContext2DBase.cs, arc method
