This is a C# project that uses .NET 8. To build the project, you will need the .NET 8 SDK installed.

## Building the project

To build the project, run the following commands from the root directory:

```bash
dotnet restore SharpCanvas/SharpCanvas.sln
dotnet build SharpCanvas/SharpCanvas.sln
```

This will restore all necessary NuGet packages and then build the entire solution.

## Guiding Principles

### Supporting Legacy Systems

When modernizing a codebase, it's important to consider the needs of users who may be on legacy systems. While it's tempting to apply "best practices" universally, a more inclusive approach is to provide choice and support for older systems where appropriate.

In this project, we have chosen to support both `System.Drawing` for Windows-specific builds and `SkiaSharp` for cross-platform builds. This allows us to modernize the API while still providing a path for users on legacy Windows systems. This is achieved through multi-targeting and conditional compilation in the `.csproj` file and C# code.
