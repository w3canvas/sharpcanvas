#!/bin/bash
set -e

echo "=== SharpCanvas Linux Warm Boot Script ==="

# 1. Setup .NET Environment
if ! command -v dotnet &> /dev/null; then
    echo "dotnet command not found. Checking default install location..."
    if [ -f "$HOME/.dotnet/dotnet" ]; then
        echo "Found dotnet at $HOME/.dotnet/dotnet"
        export PATH="$HOME/.dotnet:$PATH"
    else
        echo "dotnet not found. Attempting to install using dotnet-install.sh..."
        if [ -f "./dotnet-install.sh" ]; then
            ./dotnet-install.sh
            export PATH="$HOME/.dotnet:$PATH"
        else
            echo "Error: dotnet-install.sh not found. Please install .NET 8 SDK manually."
            exit 1
        fi
    fi
fi

# Verify dotnet version
echo "Using .NET SDK:"
dotnet --version

# 2. Check Dependencies
echo "Checking system dependencies..."
MISSING_DEPS=0

check_dep() {
    if command -v dpkg &> /dev/null; then
        if dpkg -s "$1" &> /dev/null; then
            echo "  [OK] $1"
        else
            echo "  [MISSING] $1"
            MISSING_DEPS=1
        fi
    else
        echo "  [UNKNOWN] $1 (dpkg not found, skipping check)"
    fi
}

# Check for FontConfig (Critical for SkiaSharp)
check_dep libfontconfig1

if [ $MISSING_DEPS -eq 1 ]; then
    echo ""
    echo "WARNING: Some dependencies are missing."
    echo "Please run: sudo apt-get update && sudo apt-get install -y libfontconfig1"
    echo ""
    # We continue, but warn.
fi

# 3. Restore Solution
echo "Restoring NuGet packages..."
dotnet restore SharpCanvas/SharpCanvas.sln

echo ""
echo "=== Warm boot complete! ==="
echo "To build the project, run: dotnet build SharpCanvas/SharpCanvas.sln"
echo "To run tests, run: dotnet test SharpCanvas/SharpCanvas.sln"
echo ""
echo "Note: If you installed .NET via this script, you may need to add it to your PATH permanently:"
echo "export PATH=\"\$HOME/.dotnet:\$PATH\""
