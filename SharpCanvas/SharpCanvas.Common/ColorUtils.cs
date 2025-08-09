using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SharpCanvas.Forms
{
    public static class ColorUtils
    {
        private const string HTML_COLOR_REGEX = @"#[\d\w]{3,6}";
        private static Regex _htmlColorRegex = new Regex(HTML_COLOR_REGEX);

        public static bool isValidColor(string value)
        {
            if (value.Contains("#"))
            {
                try
                {
                    ColorTranslator.FromHtml(value);
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
                    var regex = new Regex(@"rgba\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\s*,\s*(?<alpha>\d*.?\d*)\)");
                    if (regex.IsMatch(value))
                    {
                        Match match = regex.Match(value);
                        int r = Convert.ToInt32(match.Groups["r"].Value);
                        int g = Convert.ToInt32(match.Groups["g"].Value);
                        int b = Convert.ToInt32(match.Groups["b"].Value);
                        string s = match.Groups["alpha"].Value.Replace(',', '.');
                        if(s.Length > 0 && s[0] == '.')//handle .01 cases
                        {
                            s = '0' + s;
                        }
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
                else if (value.Contains("rgb"))
                {
                    var regex = new Regex(@"rgb\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\)");
                    if (regex.IsMatch(value))
                    {
                        Match match = regex.Match(value);
                        int r = Convert.ToInt32(match.Groups["r"].Value);
                        int g = Convert.ToInt32(match.Groups["g"].Value);
                        int b = Convert.ToInt32(match.Groups["b"].Value);
                        return r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255;
                    }
                }
            }

            Color c = Color.FromName(value);
            return !(c.R == 0 && c.G == 0 && c.B == 0 && c.A == 0);
        }

        public static Color ParseColor(string value)
        {
            if (_htmlColorRegex.IsMatch(value))
            {
                return ColorTranslator.FromHtml(value);
            }
            else
            {
                if (value.Contains("rgba"))
                {
                    var regex = new Regex(@"rgba\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\s*,\s*(?<alpha>\d*.?\d*)\)");
                    if (regex.IsMatch(value))
                    {
                        Match match = regex.Match(value);
                        int r = Convert.ToInt32(match.Groups["r"].Value);
                        int g = Convert.ToInt32(match.Groups["g"].Value);
                        int b = Convert.ToInt32(match.Groups["b"].Value);
                        string s = match.Groups["alpha"].Value.Replace(',', '.');
                        if (s.Length > 0 && s[0] == '.')//handle .01 cases
                        {
                            s = '0' + s;
                        }
                        double a = double.Parse(s, CultureInfo.InvariantCulture);
                        //About alpha value from the spec:
                        //if it has alpha equal to 1.0...
                        //Otherwise, the color value has alpha less than 1.0...
                        if (a > 1)
                            a = 1;
                        var alpha = (int) Math.Floor(a*255);
                        return Color.FromArgb(alpha, r, g, b);
                    }
                }
                else if (value.Contains("rgb"))
                {
                    var regex = new Regex(@"rgb\((?<r>\d+)\s*,\s*(?<g>\d+)\s*,\s*(?<b>\d+)\)");
                    if (regex.IsMatch(value))
                    {
                        Match match = regex.Match(value);
                        int r = Convert.ToInt32(match.Groups["r"].Value);
                        int g = Convert.ToInt32(match.Groups["g"].Value);
                        int b = Convert.ToInt32(match.Groups["b"].Value);
                        return Color.FromArgb(r, g, b);
                    }
                }
            }
            return Color.FromName(value);
        }
    }
}