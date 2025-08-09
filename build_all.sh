#!/bin/bash
set -e
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Core/SharpCanvas.Core.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Common/SharpCanvas.Common.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Interop/SharpCanvas.Interop.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Prototype/SharpCanvas.Prototype.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Host/SharpCanvas.Host.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Bitmap.Filter/SharpCanvas.Bitmap.Filter.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Context.Drawing2D/SharpCanvas.Context.Drawing2D.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Browser/SharpCanvas.Browser.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Browser.WinForms/SharpCanvas.Browser.WinForms.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.csproj
