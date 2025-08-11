using SkiaSharp;
using System.Text.RegularExpressions;

namespace SharpCanvas.Context.Skia
{
    public static class FontUtils
    {
        public static void ApplyFont(string font, SKPaint paint)
        {
            var regex = new Regex(@"(?<size>\d+)(?<metric>\w+)\W+(?<font>[\w\s]+.*)");
            if (regex.IsMatch(font))
            {
                Match match = regex.Match(font);
                string size = match.Groups["size"].Value;
                string family = match.Groups["font"].Value;

                if (float.TryParse(size, out float sizeValue))
                {
                    paint.TextSize = sizeValue;
                }

                paint.Typeface = SKTypeface.FromFamilyName(family);
            }
        }

        public static float GetYOffset(string textBaseline, SKPaint paint)
        {
            var metrics = paint.FontMetrics;
            switch (textBaseline)
            {
                case "top":
                    return -metrics.Ascent;
                case "hanging":
                    return -metrics.CapHeight;
                case "middle":
                    return (-metrics.Ascent - metrics.Descent) / 2;
                case "alphabetic":
                    return 0;
                case "ideographic":
                    return -metrics.Descent;
                case "bottom":
                    return -metrics.Descent;
                default:
                    return 0;
            }
        }
    }
}
