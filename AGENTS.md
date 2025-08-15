This is a C# project that uses .NET 8. To build the project, you will need the .NET 8 SDK installed.

## Building the project

To build the project, run the following commands from the root directory:

```bash
/home/jules/.dotnet/dotnet restore SharpCanvas/SharpCanvas.sln
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.sln
```

**Note:** The `dotnet` executable is installed in `/home/jules/.dotnet`. If `dotnet` is not in your `PATH`, you should use the full path.

This will restore all necessary NuGet packages and then build the entire solution.

## Guiding Principles

### Supporting Legacy Systems

When modernizing a codebase, it's important to consider the needs of users who may be on legacy systems. While it's tempting to apply "best practices" universally, a more inclusive approach is to provide choice and support for older systems where appropriate.

In this project, we have chosen to support both `System.Drawing` for Windows-specific builds and `SkiaSharp` for cross-platform builds. This allows us to modernize the API while still providing a path for users on legacy Windows systems. This is achieved through multi-targeting and conditional compilation in the `.csproj` file and C# code.

### Debugging Strategy for Build or API Issues

When facing persistent build errors or issues with understanding the correct usage of a third-party API (like SkiaSharp), a recommended debugging strategy is to isolate the problem. Instead of trying to fix the issue within the main project, which might have a complex configuration, follow these steps:

1.  **Create a Minimal Test Project**: Create a new, separate `.csproj` file within the relevant test directory (e.g., `Tests.Skia.Minimal.csproj`). This project should be as simple as possible and should not reference the main project, only the necessary dependencies (like NUnit and the third-party library).
2.  **Isolate the Code**: Create a new test file (e.g., `ApiUsageTests.cs`) that is only included in your minimal test project. This can be achieved by setting `<EnableDefaultCompileItems>false</EnableDefaultCompileItems>` in the minimal `.csproj` and then explicitly including only your new test file.
3.  **Write a Focused Test**: In the new test file, write a small, focused test case that uses the specific API or feature that is causing problems.
4.  **Iterate and Fix**: Build and run this minimal test project. Since it's isolated, it will be much easier to debug and get to a passing state. Iterate on the test until it passes, which proves you have found the correct API usage.
5.  **Apply the Fix**: Once the isolated test is passing, apply the now-verified correct code back to the main project.
6.  **Fix Forward**: If the main project still fails to build, do not revert your now-correct code. The error must lie elsewhere. Analyze the remaining build errors, knowing that the specific piece of code you tested is correct.

### Environment Dependencies

#### SkiaSharp on Linux

When working with `SkiaSharp` on Linux, be aware that it has native dependencies that must be present on the system for certain features to work.

Specifically for font rendering, `SkiaSharp` relies on the **Fontconfig** library. If `fontconfig` is not installed on the system, any attempts to render text will likely fail silently (i.e., no text will be drawn, and no exceptions will be thrown).

If you encounter issues with text rendering in SkiaSharp tests, ensure that the execution environment has the `fontconfig` package (or its equivalent) installed.

## Project Status and Known Issues

### `fontVariantCaps` Implementation

The implementation of the `fontVariantCaps` property in the Skia backend is more complex than initially anticipated. It requires the use of the `HarfBuzzSharp` library for text shaping to correctly handle OpenType features. The `SKShaper` class in `SkiaSharp.HarfBuzz` is the entry point for this, but a deeper integration is required to correctly apply the features and draw the resulting glyphs. This task has been deferred and is documented in `UNDONE.md`.

### Legacy Code (`System.Drawing`)

The legacy `System.Drawing` codebase has several `FIXME` comments listed in `UNDONE.md`. These comments are related to interop with JScript and the use of `IExpando`. After investigation, it has been determined that attempting to address these `FIXME`s without the original legacy host environment for testing is high-risk. A comment from the original author in the code suggests that the current implementation was left in place for compatibility. Therefore, these `FIXME`s should be considered deprecated unless a specific, reproducible bug is identified.
