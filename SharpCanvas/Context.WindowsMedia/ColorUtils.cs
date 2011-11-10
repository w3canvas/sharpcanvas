using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
//using System.Linq;

namespace SharpCanvas.Media
{
    public static class ColorUtils
    {
        private static readonly Dictionary<string, Color> _colorCache = new Dictionary<string, Color>();
        private static readonly Regex rgb = new Regex(@"rgb\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\)");

        private static readonly Regex rgba =
            new Regex(@"rgba\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\s*,\s*(?<alpha>\d+.?\d*)\)");

        private static readonly object syncRgb = new object();
        private static readonly object syncRgba = new object();

        public static bool isValidColor(string value)
        {
            //check cache for the color
            lock (syncRgba)
            {
                if (_colorCache.ContainsKey(value))
                {
                    return true;
                }
            }
            //validate
            if (value.Contains("#"))
            {
                try
                {
                    ColorConverter.ConvertFromString(value);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex == null) return false;
                    return false;
                }
            }
            else
            {
                if (value.Contains("rgba"))
                {
                    lock (syncRgba) //sync rgba shared object
                    {
                        if (rgba.IsMatch(value))
                        {
                            Match match = rgba.Match(value);
                            int r = Convert.ToInt32(match.Groups["r"].Value);
                            int g = Convert.ToInt32(match.Groups["g"].Value);
                            int b = Convert.ToInt32(match.Groups["b"].Value);
                            string s = match.Groups["alpha"].Value.Replace(',', '.');
                            double a = double.Parse(s, CultureInfo.InvariantCulture);
                            //About alpha value from the spec:
                            //if it has alpha equal to 1.0...
                            //Otherwise, the color value has alpha less than 1.0...
                            if (a > 1)
                                a = 1;
                            var alpha = (int) Math.Floor(a*255);
                            return r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255 && a >= 0 && a <= 1;
                        }
                    }
                }
                else if (value.Contains("rgb"))
                {
                    lock (syncRgb) //sync rgb shared object
                    {
                        if (rgb.IsMatch(value))
                        {
                            Match match = rgb.Match(value);
                            int r = Convert.ToInt32(match.Groups["r"].Value);
                            int g = Convert.ToInt32(match.Groups["g"].Value);
                            int b = Convert.ToInt32(match.Groups["b"].Value);
                            return r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255;
                        }
                    }
                }
            }

            var c = (Color) ColorConverter.ConvertFromString(value);
            return !(c.R == 0 && c.G == 0 && c.B == 0 && c.A == 0);
        }

        public static Color ParseColor(string value)
        {
            //check cache for the color
            lock (syncRgba)
            {
                if (_colorCache.ContainsKey(value))
                {
                    return _colorCache[value];
                }
            }
            if (value.Contains("#"))
            {
                var fromString = (Color) ColorConverter.ConvertFromString(value);
                _colorCache.Add(value, fromString);
                return fromString;
            }
            else
            {
                if (value.Contains("rgba"))
                {
                    lock (syncRgba)
                    {
                        if (rgba.IsMatch(value))
                        {
                            Match match = rgba.Match(value);
                            byte r = Convert.ToByte(match.Groups["r"].Value);
                            byte g = Convert.ToByte(match.Groups["g"].Value);
                            byte b = Convert.ToByte(match.Groups["b"].Value);
                            string s = match.Groups["alpha"].Value.Replace(',', '.');
                            double a = double.Parse(s, CultureInfo.InvariantCulture);
                            //About alpha value from the spec:
                            //if it has alpha equal to 1.0...
                            //Otherwise, the color value has alpha less than 1.0...
                            if (a > 1)
                                a = 1;
                            var alpha = (byte) Math.Floor(a*255);
                            Color argb = Color.FromArgb(alpha, r, g, b);
                            _colorCache.Add(value, argb);
                            return argb;
                        }
                    }
                }
                else if (value.Contains("rgb"))
                {
                    lock (syncRgb)
                    {
                        if (rgb.IsMatch(value))
                        {
                            Match match = rgb.Match(value);
                            byte r = Convert.ToByte(match.Groups["r"].Value);
                            byte g = Convert.ToByte(match.Groups["g"].Value);
                            byte b = Convert.ToByte(match.Groups["b"].Value);
                            Color fromRgb = Color.FromRgb(r, g, b);
                            _colorCache.Add(value, fromRgb);
                            return fromRgb;
                        }
                    }
                }
            }
            var fromString2 = (Color) ColorConverter.ConvertFromString(value);
            _colorCache.Add(value, fromString2);
            return fromString2;
        }
    }
}