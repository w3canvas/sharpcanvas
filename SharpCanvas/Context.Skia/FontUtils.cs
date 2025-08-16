using System.IO;
using System.Linq;
using SharpCanvas.Shared;
using SkiaSharp;
using System.Text.RegularExpressions;
using System.Reflection;
using HarfBuzzSharp;
using System.Collections.Generic;

namespace SharpCanvas.Context.Skia
{
    public static class FontUtils
    {
        private static Feature[] GetFontFeatures(string fontVariantCaps)
        {
            var features = new List<Feature>();
            var values = fontVariantCaps.Split(' ');
            foreach (var value in values)
            {
                switch (value)
                {
                    case "small-caps":
                        features.Add(new Feature(new Tag('s', 'm', 'c', 'p'), 1, 0, uint.MaxValue));
                        break;
                    case "all-small-caps":
                        features.Add(new Feature(new Tag('c', '2', 's', 'c'), 1, 0, uint.MaxValue));
                        features.Add(new Feature(new Tag('s', 'm', 'c', 'p'), 1, 0, uint.MaxValue));
                        break;
                    case "petite-caps":
                        features.Add(new Feature(new Tag('p', 'c', 'a', 'p'), 1, 0, uint.MaxValue));
                        break;
                    case "all-petite-caps":
                        features.Add(new Feature(new Tag('c', '2', 'p', 'c'), 1, 0, uint.MaxValue));
                        features.Add(new Feature(new Tag('p', 'c', 'a', 'p'), 1, 0, uint.MaxValue));
                        break;
                    case "unicase":
                        features.Add(new Feature(new Tag('u', 'n', 'i', 'c'), 1, 0, uint.MaxValue));
                        break;
                    case "titling-caps":
                        features.Add(new Feature(new Tag('t', 'i', 't', 'l'), 1, 0, uint.MaxValue));
                        break;
                }
            }
            return features.ToArray();
        }

        internal static bool ApplyFont(SkiaCanvasRenderingContext2DBase context, SKFont font)
        {
            var fontString = context.font;
            var regex = new Regex(@"(?<size>\d+)(?<metric>\w+)\W+(?<font>[\w\s]+.*)");
            if (regex.IsMatch(fontString))
            {
                Match match = regex.Match(fontString);
                string size = match.Groups["size"].Value;
                string family = match.Groups["font"].Value;

                if (float.TryParse(size, out float sizeValue))
                {
                    font.Size = sizeValue;
                }

                var fontFace = ((FontFaceSet)context.fonts).values().FirstOrDefault(f => f.family == family);
                if (fontFace != null)
                {
                    if (fontFace.status == "loaded")
                    {
                        var fontData = fontFace.GetDataAsync().Result;
                        var data = SKData.Create(new MemoryStream(fontData));
                        font.Typeface = SKTypeface.FromData(data);
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
                        font.Typeface = SKTypeface.FromFile(fontPath);
                    }
                    else
                    {
                        // Fallback to system fonts if the file doesn't exist
                        font.Typeface = SKTypeface.FromFamilyName(family.Trim());
                    }
                }
            }

            font.Subpixel = context.textRendering == "optimizeLegibility";

            font.ScaleX = context.fontStretch switch
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
            context._fontFeatures = GetFontFeatures(context.fontVariantCaps);
            return true;
        }

        public static float GetYOffset(string textBaseline, SKFont font)
        {
            var metrics = font.Metrics;
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
