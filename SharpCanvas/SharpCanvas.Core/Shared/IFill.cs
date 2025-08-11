#if WINDOWS
using System.Drawing;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
#else
using SkiaSharp;
using Brush = SkiaSharp.SKPaint;
using Color = SkiaSharp.SKColor;
#endif
using System;

namespace SharpCanvas.Shared
{
    public interface IFill : ICloneable
    {
        Brush brush { get; set; }
        Color color { get; set; }
    }
}