#!/bin/bash
/home/jules/.dotnet/dotnet clean SharpCanvas/SharpCanvas.sln
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.sln > build_solution.log 2>&1
