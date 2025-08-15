using System.IO;
using System.Linq;
using SharpCanvas.Shared;
using SkiaSharp;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SharpCanvas.Context.Skia
{
    public static class FontUtils
    {
        internal static bool ApplyFont(SkiaCanvasRenderingContext2DBase context, SKPaint paint)
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

                var fontFace = ((FontFaceSet)context.fonts).values().FirstOrDefault(f => f.family == family);
                if (fontFace != null)
                {
                    if (fontFace.status == "loaded")
                    {
                        var fontData = fontFace.GetDataAsync().Result;
                        var data = SKData.Create(new MemoryStream(fontData));
                        paint.Typeface = SKTypeface.FromData(data);
                    }
                    else
                    {
                        // font-display: block behavior - don't draw if font is not loaded
                        return false;
                    }
                }
                else
                {
                    var fontPath = System.IO.Path.Combine("Fonts", family.Trim() + ".ttf");
                    if (System.IO.File.Exists(fontPath))
                    {
                        paint.Typeface = SKTypeface.FromFile(fontPath);
                    }
                    else
                    {
                        // Fallback to system fonts if the file doesn't exist
                        paint.Typeface = SKTypeface.FromFamilyName(family.Trim());
                    }
                }
            }

            paint.IsAntialias = context.textRendering != "optimizeSpeed";
            paint.SubpixelText = context.textRendering == "optimizeLegibility";

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
            return true;
        }

        public static float GetYOffset(string textBaseline, SKPaint paint)
        {
            var metrics = paint.FontMetrics;
            switch (textBaseline)
            {
                case "top":
                    return -metrics.Top;
                case "hanging":
                    return -metrics.Ascent;
                case "middle":
                    return -(metrics.Top + metrics.Bottom) / 2;
                case "alphabetic":
                    return 0;
                case "ideographic":
                    return -metrics.Descent;
                case "bottom":
                    return -metrics.Bottom;
                default:
                    return 0;
            }
        }
    }
}
