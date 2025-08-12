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

                // Try to find the specified font, then try common fallbacks.
                var typeface = SKTypeface.FromFamilyName(family);
                if (typeface == null)
                {
                    var genericFamily = family.ToLower();
                    if (genericFamily.Contains("sans-serif"))
                    {
                        typeface = SKTypeface.FromFamilyName("DejaVu Sans");
                    }
                    else if (genericFamily.Contains("serif"))
                    {
                        typeface = SKTypeface.FromFamilyName("DejaVu Serif");
                    }
                    else if (genericFamily.Contains("monospace"))
                    {
                        typeface = SKTypeface.FromFamilyName("DejaVu Sans Mono");
                    }
                }

                paint.Typeface = typeface ?? SKTypeface.Default;
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
