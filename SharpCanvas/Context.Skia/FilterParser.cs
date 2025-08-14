using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpCanvas.Context.Skia
{
    public static class FilterParser
    {
        public static SKImageFilter Parse(string filter)
        {
            if (string.IsNullOrEmpty(filter) || filter == "none")
            {
                return null;
            }

            var filters = new List<SKImageFilter>();
            var filterParts = filter.Split(')', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim() + ")");

            foreach (var part in filterParts)
            {
                var match = Regex.Match(part, @"(\w+)\((.*)\)");
                if (match.Success)
                {
                    var function = match.Groups[1].Value;
                    var args = match.Groups[2].Value.Split(' ').Select(s => s.Trim()).ToArray();

                    switch (function)
                    {
                        case "blur":
                            if (args.Length == 1 && TryParsePx(args[0], out var blurValue))
                            {
                                filters.Add(SKImageFilter.CreateBlur(blurValue, blurValue));
                            }
                            break;
                        case "drop-shadow":
                            if (args.Length >= 2)
                            {
                                TryParsePx(args[0], out var dx);
                                TryParsePx(args[1], out var dy);
                                var shadowBlur = args.Length > 2 && TryParsePx(args[2], out var b) ? b : 0;
                                var color = args.Length > 3 ? ColorParser.Parse(args[3]) : SKColors.Black;
                                filters.Add(SKImageFilter.CreateDropShadow(dx, dy, shadowBlur, shadowBlur, color));
                            }
                            break;
                        case "grayscale":
                            if (args.Length == 1 && TryParsePercentage(args[0], out var grayAmount))
                            {
                                var grayMatrix = new float[]
                                {
                                    (float)(0.2126 + 0.7874 * (1 - grayAmount)), (float)(0.7152 - 0.7152 * (1 - grayAmount)), (float)(0.0722 - 0.0722 * (1 - grayAmount)), 0, 0,
                                    (float)(0.2126 - 0.2126 * (1 - grayAmount)), (float)(0.7152 + 0.2848 * (1 - grayAmount)), (float)(0.0722 - 0.0722 * (1 - grayAmount)), 0, 0,
                                    (float)(0.2126 - 0.2126 * (1 - grayAmount)), (float)(0.7152 - 0.7152 * (1 - grayAmount)), (float)(0.0722 + 0.9278 * (1 - grayAmount)), 0, 0,
                                    0, 0, 0, 1, 0
                                };
                                filters.Add(SKImageFilter.CreateColorFilter(SKColorFilter.CreateColorMatrix(grayMatrix)));
                            }
                            break;
                        case "sepia":
                            if (args.Length == 1 && TryParsePercentage(args[0], out var sepiaAmount))
                            {
                                var sepiaMatrix = new float[]
                                {
                                    (float)(0.393 + 0.607 * (1 - sepiaAmount)), (float)(0.769 - 0.769 * (1 - sepiaAmount)), (float)(0.189 - 0.189 * (1 - sepiaAmount)), 0, 0,
                                    (float)(0.349 - 0.349 * (1 - sepiaAmount)), (float)(0.686 + 0.314 * (1 - sepiaAmount)), (float)(0.168 - 0.168 * (1 - sepiaAmount)), 0, 0,
                                    (float)(0.272 - 0.272 * (1 - sepiaAmount)), (float)(0.534 - 0.534 * (1 - sepiaAmount)), (float)(0.131 + 0.869 * (1 - sepiaAmount)), 0, 0,
                                    0, 0, 0, 1, 0
                                };
                                filters.Add(SKImageFilter.CreateColorFilter(SKColorFilter.CreateColorMatrix(sepiaMatrix)));
                            }
                            break;
                    }
                }
            }

            if (filters.Count == 0)
            {
                return null;
            }
            if (filters.Count == 1)
            {
                return filters[0];
            }
            return SKImageFilter.CreateMerge(filters.ToArray());
        }

        private static bool TryParsePx(string value, out float result)
        {
            if (value.EndsWith("px"))
            {
                return float.TryParse(value.Substring(0, value.Length - 2), out result);
            }
            result = 0;
            return false;
        }

        private static bool TryParsePercentage(string value, out float result)
        {
            if (value.EndsWith("%"))
            {
                if (float.TryParse(value.Substring(0, value.Length - 1), out var percentage))
                {
                    result = percentage / 100f;
                    return true;
                }
            }
            else if (float.TryParse(value, out result))
            {
                return true;
            }
            result = 0;
            return false;
        }
    }
}
