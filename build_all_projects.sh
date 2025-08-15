#!/bin/bash
export PATH="/home/jules/.dotnet:$PATH"
/home/jules/.dotnet/dotnet build SharpCanvas.Tests/Tests.Skia/Tests.Skia.Minimal.csproj > build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas.Tests/Tests.Skia/Tests.Skia.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas.Tests/Tests.Skia.Modern/SharpCanvas.Tests.Skia.Modern.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Host/SharpCanvas.Host.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Context.Skia/Context.Skia.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Core/SharpCanvas.Core.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Interop/SharpCanvas.Interop.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Bitmap.Shader/SharpCanvas.Bitmap.Shader.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Prototype/SharpCanvas.Prototype.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Browser.WindowsMedia/SharpCanvas.Browser.WindowsMedia.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Context.WindowsMedia/SharpCanvas.Context.WindowsMedia.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Installer/SharpCanvas.Installer.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/SharpCanvas.Common/SharpCanvas.Common.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Bitmap.Filter/SharpCanvas.Bitmap.Filter.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Legacy/Browser/SharpCanvas.Browser.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Legacy/Drawing/Context.Drawing2D/SharpCanvas.Context.Drawing2D.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Legacy/Drawing/Tests/SharpCanvas.Tests.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Legacy/Drawing/Tests.Drawing2D/SharpCanvas.Tests.Drawing2D.csproj >> build_log.txt
/home/jules/.dotnet/dotnet build SharpCanvas/Legacy/Drawing/Browser.WinForms/SharpCanvas.Browser.WinForms.csproj >> build_log.txt
