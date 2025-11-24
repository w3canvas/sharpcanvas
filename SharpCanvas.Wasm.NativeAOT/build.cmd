@echo off
REM Build script for SharpCanvas NativeAOT-LLVM WASI (EXPERIMENTAL)
REM
REM This script builds the NativeAOT project separately from the main solution.
REM Run this ONLY if you want to experiment with NativeAOT-LLVM compilation.

echo ========================================
echo SharpCanvas NativeAOT-LLVM Build
echo ========================================
echo.
echo This is an EXPERIMENTAL build using:
echo - .NET 9
echo - NativeAOT-LLVM compiler
echo - WASI target
echo - componentize-dotnet tooling
echo.

REM Check if .NET 9 is installed
dotnet --version | findstr /R "^9\." >nul
if errorlevel 1 (
    echo ERROR: .NET 9 is required
    echo Download from: https://dotnet.microsoft.com/download/dotnet/9.0
    exit /b 1
)

echo Step 1: Checking package sources...
dotnet nuget list source | findstr "dotnet-experimental" >nul
if errorlevel 1 (
    echo.
    echo WARNING: Experimental package source not found
    echo.
    echo Run this command to add it:
    echo dotnet nuget add source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json -n dotnet-experimental
    echo.
    set /p CONTINUE="Continue anyway? (y/n): "
    if /i not "%CONTINUE%"=="y" exit /b 1
) else (
    echo ✓ Experimental package source configured
)

echo.
echo Step 2: Restoring dependencies...
dotnet restore
if errorlevel 1 (
    echo ERROR: Restore failed
    exit /b 1
)

echo.
echo Step 3: Building project...
dotnet build -c Release
if errorlevel 1 (
    echo ERROR: Build failed
    exit /b 1
)

echo.
echo Step 4: Publishing to WASM...
dotnet publish -c Release
if errorlevel 1 (
    echo ERROR: Publish failed
    exit /b 1
)

echo.
echo ========================================
echo SUCCESS!
echo ========================================
echo.
echo Output location:
echo bin\Release\net9.0\wasi-wasm\publish\
echo.
echo To run with Wasmtime:
echo   cd bin\Release\net9.0\wasi-wasm\publish
echo   wasmtime run --dir=. SharpCanvas.Wasm.NativeAOT.wasm
echo.
