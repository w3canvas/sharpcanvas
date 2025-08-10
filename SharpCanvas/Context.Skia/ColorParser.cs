using SkiaSharp;
using System.Drawing;

namespace SharpCanvas.Context.Skia
{
    public static class ColorParser
    {
        public static SKColor Parse(string colorString)
        {
            if (SKColor.TryParse(colorString, out var color))
            {
                return color;
            }

            var drawingColor = Color.FromName(colorString);
            if (drawingColor.A > 0 || drawingColor.R > 0 || drawingColor.G > 0 || drawingColor.B > 0)
            {
                return new SKColor(drawingColor.R, drawingColor.G, drawingColor.B, drawingColor.A);
            }

            return SKColors.Black; // Default color if parsing fails
        }
    }
}
