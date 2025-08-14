#!/bin/bash
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Core/SharpCanvas.Core.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Common/SharpCanvas.Common.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Interop/SharpCanvas.Interop.csproj -f net8.0-windows
/home/jules/.dotnet/dotnet build SharpCanvas/Prototype/SharpCanvas.Prototype.csproj -f net8.0-windows
/home/jules/.dotnet/dotnet build SharpCanvas/Host/SharpCanvas.Host.csproj -f net8.0-windows
/home/jules/.dotnet/dotnet build SharpCanvas/Bitmap.Filter/SharpCanvas.Bitmap.Filter.csproj
/home/jules/.dotnet/dotnet build SharpCanvas.Drawing/Context.Drawing2D/SharpCanvas.Context.Drawing2D.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/Browser/SharpCanvas.Browser.csproj -f net8.0-windows
/home/jules/.dotnet/dotnet build SharpCanvas.Drawing/Browser.WinForms/SharpCanvas.Browser.WinForms.csproj
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.csproj -f net8.0-windows
