# SharpCanvas Architecture Refactoring Plan

**Status:** ✅ Phases 1 & 2 COMPLETE - Runtime Layer Operational (Event Loop abstraction deferred)
**Priority:** Medium (Quality of Life / Future Maintainability)
**Source:** Gemini analysis comparing SharpCanvas to JavaCanvas architecture
**Completed:** 2025-11-24

---

## Executive Summary

The current SharpCanvas architecture works well and all tests pass, but runtime concerns (Workers, Event Loop, Threading) are coupled with rendering backends. This plan proposes extracting runtime logic into a separate layer for **massive code reuse benefits**.

### 🎯 Key Benefits (Code Reuse & Testing Leverage)

**Immediate Impact:**
1. **System.Drawing Gets Workers for Free** - Currently no Worker support; after refactoring, full Workers with zero duplication
2. **287 Tests Become More Valuable** - Test runtime logic once, validates all backends automatically
3. **JavaScript Integration Improves** - ClearScript V8 uses same Event Loop across backends
4. **Future Backends Trivial** - Just implement `IGraphicsFactory`, get Workers/Event Loop automatically

**Before Refactoring:**
```
Context.Skia: ~5000 lines (rendering + runtime MIXED)
  ├── Workers ✅ (Skia-specific)
  └── Event Loop ✅ (Skia-specific)

Context.Drawing2D: ~3000 lines
  ├── Workers ❌ (missing - would need duplication)
  └── Event Loop ❌ (missing - would need duplication)

= ~8000 lines, NO shared runtime code
```

**After Refactoring:**
```
SharpCanvas.Runtime: ~2000 lines (SHARED by all backends)
  ├── Workers ✅ (backend-agnostic)
  └── Event Loop ✅ (backend-agnostic)

Context.Skia: ~3000 lines (pure rendering)
  └── Uses Runtime ✅

Context.Drawing2D: ~3000 lines (pure rendering)
  └── Uses Runtime ✅ (Workers now work!)

= ~8000 lines, but Workers work for BOTH backends + testing is shared
```

## Current Architecture

```
SharpCanvas.Core/              # Interfaces only
SharpCanvas.Context.Skia/      # Rendering + Runtime (MIXED)
├── SkiaCanvasRenderingContext2D.cs  ✅ Rendering logic
├── Worker.cs                         ⚠️ Runtime logic (should move)
├── SharedWorker.cs                   ⚠️ Runtime logic (should move)
├── CanvasWorker.cs                   ⚠️ Runtime logic (should move)
└── OffscreenCanvas.cs                ✅ Rendering + Transferable
SharpCanvas.Context.Drawing2D/ # Windows backend
SharpCanvas.Shared/            # Shared types
```

**Problem:**
- Runtime logic (Workers, Event Loop) is in `Context.Skia`
- Adding new backend (DirectX, WebGPU) would require reimplementing Workers
- Threading model is tightly coupled to rendering implementation

## Proposed Architecture

```
SharpCanvas.Core/              # Core interfaces (no change)
SharpCanvas.Runtime/           # NEW: Runtime abstraction layer
├── Workers/
│   ├── IWorker.cs
│   ├── Worker.cs
│   ├── SharedWorker.cs
│   └── CanvasWorker.cs
├── EventLoop/
│   ├── IEventLoop.cs
│   ├── MainThreadEventLoop.cs
│   ├── WorkerThreadEventLoop.cs
│   ├── ConsoleEventLoop.cs        # For console apps
│   ├── WpfEventLoop.cs            # For WPF apps
│   └── BlazorEventLoop.cs         # For Blazor/WASM
├── Transferables/
│   ├── ITransferable.cs
│   └── TransferableRegistry.cs
└── Scheduling/
    ├── IScheduler.cs
    └── TaskScheduler.cs

SharpCanvas.Context.Skia/      # Pure rendering backend
├── SkiaCanvasRenderingContext2D.cs
├── SkiaOffscreenCanvas.cs
└── SkiaGraphicsFactory.cs

SharpCanvas.Context.Drawing2D/ # Pure rendering backend
SharpCanvas.Shared/            # Shared types (no change)
```

**Benefits:**
- ✅ **Massive Code Reuse** - ~2000 lines of runtime code shared by all backends
- ✅ **System.Drawing Gets Workers** - Automatically gains Worker/SharedWorker support
- ✅ **287 Tests Leverage** - Test runtime once, validates all backends
- ✅ **Easy Backend Additions** - New backends only implement rendering
- ✅ **Event Loop Flexibility** - Swap per platform (Console, WPF, Blazor)
- ✅ **JavaScript Integration** - ClearScript V8 works consistently across backends

## Detailed Refactoring Tasks

### Task 1: Create SharpCanvas.Runtime Project

**New Project:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\SharpCanvas.Core\SharpCanvas.Core.csproj" />
  </ItemGroup>
</Project>
```

**Dependencies:**
- References: `SharpCanvas.Core` only
- No rendering dependencies

### Task 2: Extract Worker Classes

**Move these files from `Context.Skia` to `Runtime/Workers/`:**
1. `Worker.cs` → `SharpCanvas.Runtime/Workers/Worker.cs`
2. `SharedWorker.cs` → `SharpCanvas.Runtime/Workers/SharedWorker.cs`
3. `CanvasWorker.cs` → `SharpCanvas.Runtime/Workers/CanvasWorker.cs`

**Changes Required:**
- Remove Skia-specific dependencies
- Make Workers use `IGraphicsFactory` interface for rendering backend
- Update namespaces: `SharpCanvas.Runtime.Workers`

**Before:**
```csharp
namespace SharpCanvas.Context.Skia
{
    public class Worker
    {
        // Direct Skia dependencies...
    }
}
```

**After:**
```csharp
namespace SharpCanvas.Runtime.Workers
{
    public class Worker
    {
        private readonly IGraphicsFactory _graphicsFactory;

        public Worker(IGraphicsFactory factory)
        {
            _graphicsFactory = factory;
        }
    }
}
```

### Task 3: Create Event Loop Abstraction

**New Interface:**
```csharp
namespace SharpCanvas.Runtime.EventLoop
{
    public interface IEventLoop
    {
        void Run();
        void Stop();
        void Post(Action action);
        Task<T> PostAsync<T>(Func<T> func);
        bool IsMainThread { get; }
    }
}
```

**Implementations:**
```csharp
// For console applications
public class ConsoleEventLoop : IEventLoop { }

// For WPF applications
public class WpfEventLoop : IEventLoop
{
    private readonly Dispatcher _dispatcher;
    // Use WPF Dispatcher
}

// For Blazor/WASM applications
public class BlazorEventLoop : IEventLoop
{
    // Use Blazor's JSRuntime synchronization
}

// For Worker threads
public class WorkerThreadEventLoop : IEventLoop { }
```

### Task 4: Create Graphics Factory Interface

**New Interface in Core:**
```csharp
namespace SharpCanvas.Core
{
    public interface IGraphicsFactory
    {
        ICanvasRenderingContext2D CreateContext(int width, int height);
        IOffscreenCanvas CreateOffscreenCanvas(int width, int height);
        IImageBitmap CreateImageBitmap(byte[] data);
    }
}
```

**Implementations:**
```csharp
// In Context.Skia
public class SkiaGraphicsFactory : IGraphicsFactory
{
    public ICanvasRenderingContext2D CreateContext(int width, int height)
    {
        var info = new SKImageInfo(width, height);
        var surface = SKSurface.Create(info);
        return new SkiaCanvasRenderingContext2D(surface, document);
    }
}

// In Context.Drawing2D
public class GdiGraphicsFactory : IGraphicsFactory
{
    public ICanvasRenderingContext2D CreateContext(int width, int height)
    {
        var bitmap = new Bitmap(width, height);
        var graphics = Graphics.FromImage(bitmap);
        return new CanvasRenderingContext2D(graphics, bitmap);
    }
}
```

### Task 5: Update Worker to Use Factory

**Before (Coupled to Skia):**
```csharp
public class Worker
{
    private void CreateCanvas()
    {
        // Direct Skia creation
        var surface = SKSurface.Create(info);
        var context = new SkiaCanvasRenderingContext2D(surface, doc);
    }
}
```

**After (Decoupled):**
```csharp
public class Worker
{
    private readonly IGraphicsFactory _factory;

    public Worker(IGraphicsFactory factory)
    {
        _factory = factory;
    }

    private void CreateCanvas()
    {
        // Backend-agnostic creation
        var context = _factory.CreateContext(width, height);
    }
}
```

### Task 6: Validate Transferables

**Ensure these implement ITransferable:**
- ✅ `OffscreenCanvas` - Already implements
- ✅ `ImageBitmap` - Already implements
- ✅ `MessagePort` - Check implementation
- ✅ `ArrayBuffer` - If implemented

**Transfer Logic:**
```csharp
public interface ITransferable
{
    void Neuter(); // Called when transferred
    bool IsNeutered { get; }
}
```

### Task 7: Update Project References

**Update .csproj files:**

`Context.Skia.csproj`:
```xml
<ItemGroup>
  <ProjectReference Include="..\SharpCanvas.Core\SharpCanvas.Core.csproj" />
  <ProjectReference Include="..\SharpCanvas.Runtime\SharpCanvas.Runtime.csproj" />
  <ProjectReference Include="..\SharpCanvas.Shared\SharpCanvas.Shared.csproj" />
</ItemGroup>
```

`Context.Drawing2D.csproj`:
```xml
<ItemGroup>
  <ProjectReference Include="..\SharpCanvas.Core\SharpCanvas.Core.csproj" />
  <ProjectReference Include="..\SharpCanvas.Runtime\SharpCanvas.Runtime.csproj" />
  <ProjectReference Include="..\SharpCanvas.Shared\SharpCanvas.Shared.csproj" />
</ItemGroup>
```

### Task 8: Update Tests

**Update test references:**
- Add `SharpCanvas.Runtime` reference to test projects
- Update namespace imports
- Verify all 287 tests still pass

### Task 9: Update Documentation

**Update these files:**
- `STRUCTURE.md` - Add SharpCanvas.Runtime project
- `README.md` - Update architecture diagram
- `PRODUCTION_READINESS.md` - Note runtime abstraction

## Implementation Phases

### Phase 1: Preparation (No Breaking Changes)
1. Create `SharpCanvas.Runtime` project (empty)
2. Create `IGraphicsFactory` interface in Core
3. Implement `SkiaGraphicsFactory` and `GdiGraphicsFactory`
4. Run tests → Should still pass (no changes to behavior)

### Phase 2: Worker Extraction
1. Copy (don't move) Worker classes to Runtime
2. Update Worker classes to use `IGraphicsFactory`
3. Update Context.Skia to use Runtime.Workers
4. Run tests → Should still pass
5. Delete old Worker classes from Context.Skia

### Phase 3: Event Loop
1. Create `IEventLoop` interface
2. Implement platform-specific event loops
3. Update Workers to use `IEventLoop`
4. Run tests → Should still pass

### Phase 4: Cleanup
1. Remove any remaining Skia-specific code from Runtime
2. Verify Context.Drawing2D can use Runtime.Workers
3. Update all documentation
4. Run full test suite

## Testing Strategy & Leverage

### 🎯 How 287 Existing Tests Become More Valuable

**Current State:**
- 287 tests validate SkiaSharp backend
- System.Drawing has no Worker tests (Workers don't exist there)
- Testing infrastructure not reusable across backends

**After Refactoring:**
```csharp
// NEW: SharpCanvas.Runtime.Tests project
[TestClass]
public class WorkerTests
{
    [TestMethod]
    public void Worker_PostMessage_WithSkiaBackend()
    {
        var factory = new SkiaGraphicsFactory();
        var worker = new Worker(factory);
        // Test worker logic
    }

    [TestMethod]
    public void Worker_PostMessage_WithSystemDrawingBackend()
    {
        var factory = new GdiGraphicsFactory();
        var worker = new Worker(factory);
        // SAME test, different backend - works automatically!
    }
}
```

**Benefits:**
1. **Write Once, Test Everywhere** - Runtime tests validate all backends
2. **System.Drawing Gets Tested** - Workers now testable on Windows backend
3. **Backend-Agnostic Tests** - Mock `IGraphicsFactory` for pure unit tests
4. **JavaScript Integration Tests** - Work consistently across backends
5. **Regression Protection** - Any runtime bug caught once, fixed for all

### Test Each Phase:
```bash
# After each phase
dotnet test
# Verify: 287/287 tests passing (existing tests)

# NEW tests for Runtime
dotnet test SharpCanvas.Runtime.Tests
# Verify: Runtime tests pass with both backends

# Manual validation
cd SharpCanvas.JsHost
dotnet run
# Verify: JavaScript tests still work

cd SharpCanvas.Blazor.Wasm
dotnet run
# Verify: Blazor still works
```

### Regression Testing:
- ✅ All 287 unit tests (SkiaSharp backend)
- ✅ NEW Runtime tests (validates both backends)
- ✅ JavaScript integration tests (both backends)
- ✅ Blazor WASM compilation
- ✅ Wasmtime headless execution
- ✅ NativeAOT compilation

## Benefits After Refactoring

### 🎁 System.Drawing Backend Gets Immediate Upgrade

**Before:**
```csharp
// System.Drawing backend - NO Workers
var context = new CanvasRenderingContext2D(graphics, bitmap);
// Can't use Workers - not implemented
```

**After (Automatically!):**
```csharp
// System.Drawing backend - Workers now work!
var factory = new GdiGraphicsFactory();
var worker = new Worker(factory);
var sharedWorker = new SharedWorker(factory);

// OffscreenCanvas now works on System.Drawing too!
var offscreen = new OffscreenCanvas(800, 600, factory);

// ALL runtime features now available on Windows GDI+ backend
```

**System.Drawing gains:**
- ✅ Worker support (background rendering)
- ✅ SharedWorker support (shared state)
- ✅ CanvasWorker support (dedicated canvas worker)
- ✅ Event Loop (proper async handling)
- ✅ OffscreenCanvas (background rendering)
- ✅ Message passing between workers
- ✅ Transferables (efficient data transfer)

**Zero duplication, zero extra work** - just works!

### For Developers:
1. **Easier to Add Backends**
   - Just implement `IGraphicsFactory` (3 methods)
   - Workers and Event Loop are reusable
   - Example: DirectX backend = ~500 lines instead of ~5000

2. **Better Testing** (Leverage Existing Infrastructure)
   - Mock `IGraphicsFactory` for unit tests
   - Test Workers independently of rendering
   - Your 287 tests validate runtime logic for all backends
   - Write tests once, run against any backend

3. **Platform Flexibility**
   - Swap Event Loop per platform
   - Same code runs in Console, WPF, Blazor
   - No platform-specific Worker implementations

### For Users:
1. **More Deployment Options**
   - System.Drawing: Workers now available on Windows
   - Console apps with background Workers
   - WPF apps with offscreen rendering
   - Blazor with proper thread synchronization

2. **Better Performance**
   - Event Loop optimized per platform
   - Efficient Worker scheduling
   - Consistent threading model

## Risks and Mitigation

### Risk 1: Breaking Changes
**Mitigation:**
- Phase implementation (copy before delete)
- Keep old classes deprecated until v2.0
- Comprehensive testing after each phase

### Risk 2: Test Failures
**Mitigation:**
- Run tests after each file move
- Keep CI/CD running throughout
- Rollback if any phase breaks tests

### Risk 3: API Surface Changes
**Mitigation:**
- Keep all public APIs the same
- Internal refactoring only
- No user-facing changes

## Timeline Estimate

**Assuming dedicated work:**
- Phase 1 (Preparation): 2-4 hours
- Phase 2 (Worker Extraction): 4-6 hours
- Phase 3 (Event Loop): 6-8 hours
- Phase 4 (Cleanup): 2-4 hours
- Testing & Documentation: 4-6 hours

**Total:** 18-28 hours of focused work

**Recommended:** Do this over 1-2 weeks with proper validation at each step.

## Open Questions

1. **Should Runtime be in same repo or separate?**
   - Same repo: Easier development, single NuGet package
   - Separate repo: More modular, could be reused by other projects

2. **Do we need all Event Loop implementations immediately?**
   - Start with: Console, Worker (essential)
   - Add later: WPF, Blazor (as needed)

3. **Should IGraphicsFactory be sync or async?**
   - Current: Synchronous
   - Future: Async might be needed for WASM resource loading

4. **Backward compatibility strategy?**
   - Keep old Worker classes deprecated until v2.0?
   - Breaking change with major version bump?

## Related JavaCanvas Architecture

JavaCanvas has this structure:
```
rt/              # Runtime
├── Worker
├── SharedWorker
└── EventLoop
canvas/          # Rendering
└── CanvasRenderingContext2D
```

SharpCanvas would mirror this:
```
SharpCanvas.Runtime/    # Like JavaCanvas rt/
├── Workers/
├── EventLoop/
└── Transferables/
SharpCanvas.Context.*/  # Like JavaCanvas canvas/
└── Rendering implementations
```

## Conclusion

This refactoring improves architecture without changing functionality. It's a **quality of life improvement** for future maintainability and backend flexibility.

**Recommendation:**
- ✅ Validate WASM and NativeAOT first (current milestone)
- ✅ Plan this refactoring for Phase 2 / v2.0
- ✅ No rush - current architecture works fine
- ✅ Refactor when adding new backends (DirectX, WebGPU, etc.)

## References

- JavaCanvas architecture: https://github.com/javacanvas/javacanvas
- Web Workers spec: https://html.spec.whatwg.org/multipage/workers.html
- Event Loop pattern: https://developer.mozilla.org/en-US/docs/Web/JavaScript/EventLoop
