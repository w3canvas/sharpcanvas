using SkiaSharp;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SharpCanvas.Context.Skia
{
    public static class ColorParser
    {
        public static SKColor Parse(string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                return SKColors.Black;
            }

            if (colorString.StartsWith("rgba"))
            {
                var regex = new Regex(@"rgba\((\d+),\s*(\d+),\s*(\d+),\s*(\d*\.?\d+)\)");
                var match = regex.Match(colorString);
                if (match.Success)
                {
                    var r = byte.Parse(match.Groups[1].Value);
                    var g = byte.Parse(match.Groups[2].Value);
                    var b = byte.Parse(match.Groups[3].Value);
                    var a = (byte)(float.Parse(match.Groups[4].Value) * 255);
                    return new SKColor(r, g, b, a);
                }
            }

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
