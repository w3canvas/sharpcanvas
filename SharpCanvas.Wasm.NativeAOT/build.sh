#!/bin/bash
# Build script for SharpCanvas NativeAOT-LLVM WASI (EXPERIMENTAL)
#
# This script builds the NativeAOT project separately from the main solution.
# Run this ONLY if you want to experiment with NativeAOT-LLVM compilation.

echo "========================================"
echo "SharpCanvas NativeAOT-LLVM Build"
echo "========================================"
echo ""
echo "This is an EXPERIMENTAL build using:"
echo "- .NET 9"
echo "- NativeAOT-LLVM compiler"
echo "- WASI target"
echo "- componentize-dotnet tooling"
echo ""

# Check if .NET 9 is installed
DOTNET_VERSION=$(dotnet --version)
if [[ ! $DOTNET_VERSION =~ ^9\. ]]; then
    echo "ERROR: .NET 9 is required (found $DOTNET_VERSION)"
    echo "Download from: https://dotnet.microsoft.com/download/dotnet/9.0"
    exit 1
fi

echo "Step 1: Checking package sources..."
if ! dotnet nuget list source | grep -q "dotnet-experimental"; then
    echo ""
    echo "WARNING: Experimental package source not found"
    echo ""
    echo "Run this command to add it:"
    echo "dotnet nuget add source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json -n dotnet-experimental"
    echo ""
    read -p "Continue anyway? (y/n): " CONTINUE
    if [[ ! $CONTINUE =~ ^[Yy]$ ]]; then
        exit 1
    fi
else
    echo "✓ Experimental package source configured"
fi

echo ""
echo "Step 2: Restoring dependencies..."
dotnet restore || {
    echo "ERROR: Restore failed"
    exit 1
}

echo ""
echo "Step 3: Building project..."
dotnet build -c Release || {
    echo "ERROR: Build failed"
    exit 1
}

echo ""
echo "Step 4: Publishing to WASM..."
dotnet publish -c Release || {
    echo "ERROR: Publish failed"
    exit 1
}

echo ""
echo "========================================"
echo "SUCCESS!"
echo "========================================"
echo ""
echo "Output location:"
echo "bin/Release/net9.0/wasi-wasm/publish/"
echo ""
echo "To run with Wasmtime:"
echo "  cd bin/Release/net9.0/wasi-wasm/publish"
echo "  wasmtime run --dir=. SharpCanvas.Wasm.NativeAOT.wasm"
echo ""
