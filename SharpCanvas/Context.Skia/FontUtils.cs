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

                var assembly = typeof(FontUtils).Assembly;
                var resourceNames = assembly.GetManifestResourceNames();
                System.Console.WriteLine("Available resources: " + string.Join(", ", resourceNames));
                var resourceName = "Context.Skia.DejaVuSans.ttf";
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new System.Exception($"Failed to load embedded resource: {resourceName}");
                }
                paint.Typeface = SKTypeface.FromStream(stream);
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
                    return -metrics.Top;
                case "hanging":
                    // Not directly supported by SkiaSharp metrics, but Ascent is a good approximation
                    return -metrics.Ascent;
                case "middle":
                    return -(metrics.Top + metrics.Bottom) / 2;
                case "alphabetic":
                    return 0; // The default
                case "ideographic":
                    // Not directly supported, but Descent is a good approximation
                    return -metrics.Descent;
                case "bottom":
                    return -metrics.Bottom;
                default:
                    return 0;
            }
        }
    }
}
