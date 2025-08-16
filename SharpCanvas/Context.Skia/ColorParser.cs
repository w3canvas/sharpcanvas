using SkiaSharp;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace SharpCanvas.Context.Skia
{
    public static class ColorParser
    {
        private static readonly Dictionary<string, SKColor> NamedColors = new Dictionary<string, SKColor>(StringComparer.OrdinalIgnoreCase)
        {
            { "black", SKColors.Black },
            { "white", SKColors.White },
            { "red", SKColors.Red },
            { "green", SKColors.Green },
            { "blue", SKColors.Blue },
            { "transparent", SKColors.Transparent },
        };

        public static SKColor Parse(string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                return SKColors.Black;
            }

            if (NamedColors.TryGetValue(colorString, out var namedColor))
            {
                return namedColor;
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

            return SKColors.Black; // Default color if parsing fails
        }
    }
}
