using SkiaSharp;
using System.Text.RegularExpressions;
using System.Reflection;

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

                // Load the font from the embedded resource, as per user instructions.
                // The resource name format is {DefaultNamespace}.{FolderPath}.{FileName}
                string resourceName = "SharpCanvas.Context.Skia.Fonts.DejaVuSans.ttf";
                var assembly = typeof(FontUtils).Assembly;

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        paint.Typeface = SKTypeface.FromStream(stream);
                    }
                    else
                    {
                        // Fallback if the resource cannot be found.
                        // This will likely fail to render text on Linux without a correctly configured fontconfig.
                        paint.Typeface = SKTypeface.FromFamilyName(family);
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
