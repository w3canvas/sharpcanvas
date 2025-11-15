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
            // Basic colors
            { "black", SKColors.Black },
            { "white", SKColors.White },
            { "red", SKColors.Red },
            { "green", SKColors.Green },
            { "blue", SKColors.Blue },
            { "transparent", SKColors.Transparent },

            // Extended CSS named colors
            { "aliceblue", new SKColor(240, 248, 255) },
            { "antiquewhite", new SKColor(250, 235, 215) },
            { "aqua", new SKColor(0, 255, 255) },
            { "aquamarine", new SKColor(127, 255, 212) },
            { "azure", new SKColor(240, 255, 255) },
            { "beige", new SKColor(245, 245, 220) },
            { "bisque", new SKColor(255, 228, 196) },
            { "blanchedalmond", new SKColor(255, 235, 205) },
            { "blueviolet", new SKColor(138, 43, 226) },
            { "brown", new SKColor(165, 42, 42) },
            { "burlywood", new SKColor(222, 184, 135) },
            { "cadetblue", new SKColor(95, 158, 160) },
            { "chartreuse", new SKColor(127, 255, 0) },
            { "chocolate", new SKColor(210, 105, 30) },
            { "coral", new SKColor(255, 127, 80) },
            { "cornflowerblue", new SKColor(100, 149, 237) },
            { "cornsilk", new SKColor(255, 248, 220) },
            { "crimson", new SKColor(220, 20, 60) },
            { "cyan", new SKColor(0, 255, 255) },
            { "darkblue", new SKColor(0, 0, 139) },
            { "darkcyan", new SKColor(0, 139, 139) },
            { "darkgoldenrod", new SKColor(184, 134, 11) },
            { "darkgray", new SKColor(169, 169, 169) },
            { "darkgreen", new SKColor(0, 100, 0) },
            { "darkkhaki", new SKColor(189, 183, 107) },
            { "darkmagenta", new SKColor(139, 0, 139) },
            { "darkolivegreen", new SKColor(85, 107, 47) },
            { "darkorange", new SKColor(255, 140, 0) },
            { "darkorchid", new SKColor(153, 50, 204) },
            { "darkred", new SKColor(139, 0, 0) },
            { "darksalmon", new SKColor(233, 150, 122) },
            { "darkseagreen", new SKColor(143, 188, 143) },
            { "darkslateblue", new SKColor(72, 61, 139) },
            { "darkslategray", new SKColor(47, 79, 79) },
            { "darkturquoise", new SKColor(0, 206, 209) },
            { "darkviolet", new SKColor(148, 0, 211) },
            { "deeppink", new SKColor(255, 20, 147) },
            { "deepskyblue", new SKColor(0, 191, 255) },
            { "dimgray", new SKColor(105, 105, 105) },
            { "dodgerblue", new SKColor(30, 144, 255) },
            { "firebrick", new SKColor(178, 34, 34) },
            { "floralwhite", new SKColor(255, 250, 240) },
            { "forestgreen", new SKColor(34, 139, 34) },
            { "fuchsia", new SKColor(255, 0, 255) },
            { "gainsboro", new SKColor(220, 220, 220) },
            { "ghostwhite", new SKColor(248, 248, 255) },
            { "gold", new SKColor(255, 215, 0) },
            { "goldenrod", new SKColor(218, 165, 32) },
            { "gray", new SKColor(128, 128, 128) },
            { "greenyellow", new SKColor(173, 255, 47) },
            { "honeydew", new SKColor(240, 255, 240) },
            { "hotpink", new SKColor(255, 105, 180) },
            { "indianred", new SKColor(205, 92, 92) },
            { "indigo", new SKColor(75, 0, 130) },
            { "ivory", new SKColor(255, 255, 240) },
            { "khaki", new SKColor(240, 230, 140) },
            { "lavender", new SKColor(230, 230, 250) },
            { "lavenderblush", new SKColor(255, 240, 245) },
            { "lawngreen", new SKColor(124, 252, 0) },
            { "lemonchiffon", new SKColor(255, 250, 205) },
            { "lightblue", new SKColor(173, 216, 230) },
            { "lightcoral", new SKColor(240, 128, 128) },
            { "lightcyan", new SKColor(224, 255, 255) },
            { "lightgoldenrodyellow", new SKColor(250, 250, 210) },
            { "lightgray", new SKColor(211, 211, 211) },
            { "lightgreen", new SKColor(144, 238, 144) },
            { "lightpink", new SKColor(255, 182, 193) },
            { "lightsalmon", new SKColor(255, 160, 122) },
            { "lightseagreen", new SKColor(32, 178, 170) },
            { "lightskyblue", new SKColor(135, 206, 250) },
            { "lightslategray", new SKColor(119, 136, 153) },
            { "lightsteelblue", new SKColor(176, 196, 222) },
            { "lightyellow", new SKColor(255, 255, 224) },
            { "lime", new SKColor(0, 255, 0) },
            { "limegreen", new SKColor(50, 205, 50) },
            { "linen", new SKColor(250, 240, 230) },
            { "magenta", new SKColor(255, 0, 255) },
            { "maroon", new SKColor(128, 0, 0) },
            { "mediumaquamarine", new SKColor(102, 205, 170) },
            { "mediumblue", new SKColor(0, 0, 205) },
            { "mediumorchid", new SKColor(186, 85, 211) },
            { "mediumpurple", new SKColor(147, 112, 219) },
            { "mediumseagreen", new SKColor(60, 179, 113) },
            { "mediumslateblue", new SKColor(123, 104, 238) },
            { "mediumspringgreen", new SKColor(0, 250, 154) },
            { "mediumturquoise", new SKColor(72, 209, 204) },
            { "mediumvioletred", new SKColor(199, 21, 133) },
            { "midnightblue", new SKColor(25, 25, 112) },
            { "mintcream", new SKColor(245, 255, 250) },
            { "mistyrose", new SKColor(255, 228, 225) },
            { "moccasin", new SKColor(255, 228, 181) },
            { "navajowhite", new SKColor(255, 222, 173) },
            { "navy", new SKColor(0, 0, 128) },
            { "oldlace", new SKColor(253, 245, 230) },
            { "olive", new SKColor(128, 128, 0) },
            { "olivedrab", new SKColor(107, 142, 35) },
            { "orange", new SKColor(255, 165, 0) },
            { "orangered", new SKColor(255, 69, 0) },
            { "orchid", new SKColor(218, 112, 214) },
            { "palegoldenrod", new SKColor(238, 232, 170) },
            { "palegreen", new SKColor(152, 251, 152) },
            { "paleturquoise", new SKColor(175, 238, 238) },
            { "palevioletred", new SKColor(219, 112, 147) },
            { "papayawhip", new SKColor(255, 239, 213) },
            { "peachpuff", new SKColor(255, 218, 185) },
            { "peru", new SKColor(205, 133, 63) },
            { "pink", new SKColor(255, 192, 203) },
            { "plum", new SKColor(221, 160, 221) },
            { "powderblue", new SKColor(176, 224, 230) },
            { "purple", new SKColor(128, 0, 128) },
            { "rebeccapurple", new SKColor(102, 51, 153) },
            { "rosybrown", new SKColor(188, 143, 143) },
            { "royalblue", new SKColor(65, 105, 225) },
            { "saddlebrown", new SKColor(139, 69, 19) },
            { "salmon", new SKColor(250, 128, 114) },
            { "sandybrown", new SKColor(244, 164, 96) },
            { "seagreen", new SKColor(46, 139, 87) },
            { "seashell", new SKColor(255, 245, 238) },
            { "sienna", new SKColor(160, 82, 45) },
            { "silver", new SKColor(192, 192, 192) },
            { "skyblue", new SKColor(135, 206, 235) },
            { "slateblue", new SKColor(106, 90, 205) },
            { "slategray", new SKColor(112, 128, 144) },
            { "snow", new SKColor(255, 250, 250) },
            { "springgreen", new SKColor(0, 255, 127) },
            { "steelblue", new SKColor(70, 130, 180) },
            { "tan", new SKColor(210, 180, 140) },
            { "teal", new SKColor(0, 128, 128) },
            { "thistle", new SKColor(216, 191, 216) },
            { "tomato", new SKColor(255, 99, 71) },
            { "turquoise", new SKColor(64, 224, 208) },
            { "violet", new SKColor(238, 130, 238) },
            { "wheat", new SKColor(245, 222, 179) },
            { "whitesmoke", new SKColor(245, 245, 245) },
            { "yellow", new SKColor(255, 255, 0) },
            { "yellowgreen", new SKColor(154, 205, 50) },
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
