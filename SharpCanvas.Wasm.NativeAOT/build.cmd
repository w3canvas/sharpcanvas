@echo off
REM Build script for SharpCanvas NativeAOT (EXPERIMENTAL)
REM
REM This script builds the NativeAOT project separately from the main solution.
REM Run this ONLY if you want to experiment with NativeAOT compilation.

echo ========================================
echo SharpCanvas NativeAOT Build
echo ========================================
echo.
echo This is an EXPERIMENTAL build using:
echo - .NET 8
echo - NativeAOT compiler (PublishAot=true)
echo - Native executable output (not WASM)
echo.

REM Check if .NET 8 or later is installed
dotnet --version | findstr /R "^[89]\." >nul
if errorlevel 1 (
    echo ERROR: .NET 8 or later is required
    echo Download from: https://dotnet.microsoft.com/download/dotnet
    exit /b 1
)

echo Step 1: Restoring dependencies...
dotnet restore
if errorlevel 1 (
    echo ERROR: Restore failed
    exit /b 1
)

echo.
echo Step 2: Publishing with NativeAOT (win-x64)...
dotnet publish -c Release -r win-x64
if errorlevel 1 (
    echo ERROR: Publish failed
    echo.
    echo This may fail if:
    echo - SkiaSharp is not compatible with NativeAOT trimming
    echo - Required code was trimmed
    echo - Native dependencies cannot be bundled
    exit /b 1
)

echo.
echo ========================================
echo SUCCESS!
echo ========================================
echo.
echo Output location:
echo bin\Release\net8.0\win-x64\publish\
echo.
echo To run:
echo   cd bin\Release\net8.0\win-x64\publish
echo   SharpCanvas.Wasm.NativeAOT.exe
echo.
