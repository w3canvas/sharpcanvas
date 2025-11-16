#!/bin/bash
# This script builds the entire SharpCanvas solution.
# It restores dependencies and then builds the solution.

echo "Restoring dependencies..."
/home/jules/.dotnet/dotnet restore SharpCanvas/SharpCanvas.sln

echo "Building solution..."
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.sln
