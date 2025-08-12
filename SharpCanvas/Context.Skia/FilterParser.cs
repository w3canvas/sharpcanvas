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
    }
}
