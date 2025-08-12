using SkiaSharp;
using System.Text.RegularExpressions;

namespace SharpCanvas.Context.Skia
{
    public static class FontUtils
    {
        public static void ApplyFont(SkiaCanvasRenderingContext2DBase context, SKPaint paint)
        {
            var font = context.font;
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

            paint.IsAntialias = context.textRendering != "optimizeSpeed";
            paint.SubpixelText = context.textRendering == "optimizeLegibility";

            // The following properties are not available in this version of SkiaSharp.
            // paint.TextKerning = context.fontKerning == "normal";
            //
            // if (float.TryParse(context.letterSpacing.Replace("px", ""), out var letterSpacing))
            // {
            //     paint.LetterSpacing = letterSpacing;
            // }
            //
            // if (float.TryParse(context.wordSpacing.Replace("px", ""), out var wordSpacing))
            // {
            //     paint.WordSpacing = wordSpacing;
            // }

            // fontStretch is more complex, requiring specific typeface selection or transformations.
            // For simplicity, we can use TextScaleX, but a full implementation would need more logic.
            paint.TextScaleX = context.fontStretch switch
            {
                "ultra-condensed" => 0.5f,
                "extra-condensed" => 0.625f,
                "condensed" => 0.75f,
                "semi-condensed" => 0.875f,
                "normal" => 1.0f,
                "semi-expanded" => 1.125f,
                "expanded" => 1.25f,
                "extra-expanded" => 1.5f,
                "ultra-expanded" => 2.0f,
                _ => 1.0f,
            };
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
