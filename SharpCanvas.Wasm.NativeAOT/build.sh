#!/bin/bash
# Build script for SharpCanvas NativeAOT (EXPERIMENTAL)
#
# This script builds the NativeAOT project separately from the main solution.
# Run this ONLY if you want to experiment with NativeAOT compilation.

echo "========================================"
echo "SharpCanvas NativeAOT Build"
echo "========================================"
echo ""
echo "This is an EXPERIMENTAL build using:"
echo "- .NET 8"
echo "- NativeAOT compiler (PublishAot=true)"
echo "- Native executable output (not WASM)"
echo ""

# Check if .NET 8 or later is installed
DOTNET_VERSION=$(dotnet --version)
if [[ ! $DOTNET_VERSION =~ ^[89]\. ]]; then
    echo "ERROR: .NET 8 or later is required (found $DOTNET_VERSION)"
    echo "Download from: https://dotnet.microsoft.com/download/dotnet"
    exit 1
fi

# Determine runtime identifier based on OS
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS - detect architecture
    ARCH=$(uname -m)
    if [[ "$ARCH" == "arm64" ]]; then
        RID="osx-arm64"
    else
        RID="osx-x64"
    fi
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    RID="linux-x64"
else
    echo "ERROR: Unsupported OS: $OSTYPE"
    exit 1
fi

echo "Step 1: Restoring dependencies..."
dotnet restore || {
    echo "ERROR: Restore failed"
    exit 1
}

echo ""
echo "Step 2: Publishing with NativeAOT ($RID)..."
dotnet publish -c Release -r $RID || {
    echo "ERROR: Publish failed"
    echo ""
    echo "This may fail if:"
    echo "- SkiaSharp is not compatible with NativeAOT trimming"
    echo "- Required code was trimmed"
    echo "- Native dependencies cannot be bundled"
    exit 1
}

echo ""
echo "========================================"
echo "SUCCESS!"
echo "========================================"
echo ""
echo "Output location:"
echo "bin/Release/net8.0/$RID/publish/"
echo ""
echo "To run:"
echo "  cd bin/Release/net8.0/$RID/publish"
echo "  ./SharpCanvas.Wasm.NativeAOT"
echo ""
