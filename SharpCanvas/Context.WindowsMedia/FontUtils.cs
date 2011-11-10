using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
//using System.Linq;

namespace SharpCanvas.Media
{
    public static class FontUtils
    {
        private const string FONT_REGEX = @"(?<size>\d+)(?<metric>\w+)\W+(?<font>\w+.*)";

        public static double ConvertPtToPixel(double pt, double dpi)
        {
            return pt*dpi/72f;
        }

        public static double ConvertPixelToPt(double pixel, double dpi)
        {
            return pixel*72f/dpi;
        }

        public static Point ApplyBaseLine(Point point, string baseLine, double baseline, double ext, double height)
        {
            switch (baseLine)
            {
                case "top": //The top of the em square
                    return point;
                case "hanging": //The hanging baseline
                    return new Point(point.X, point.Y + baseline*0.1);
                case "middle": //The middle of the em square
                    return new Point(point.X, point.Y - height/2);
                case "alphabetic":
                    return new Point(point.X, point.Y - baseline);
                case "ideographic":
                    return new Point(point.X, point.Y - height - ext);
                case "bottom": //The bottom of the em square
                    return new Point(point.X, point.Y - height);
            }
            return point;
        }

        public static TextMetrics MeasureText(string text, string fontFamily, double size)
        {
            var typeface = new Typeface(new FontFamily(fontFamily),
                                        FontStyles.Normal,
                                        FontWeights.Normal,
                                        FontStretches.Normal);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                throw new InvalidOperationException("No glyphtypeface found");

            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            double totalWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                char key = text[n];
                if (glyphTypeface.CharacterToGlyphMap.ContainsKey(key)) //may cause incorrect width value!
                {
                    ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[key];
                    glyphIndexes[n] = glyphIndex;

                    double width = glyphTypeface.AdvanceWidths[glyphIndex]*size;
                    advanceWidths[n] = width;

                    totalWidth += width;
                }
            }
            //todo: measure height
            return new TextMetrics((int) Math.Truncate(totalWidth), 0);
        }

        public static Point ApplyTextAlign(Point point, string align, string text, TextMetrics size)
        {
            switch (align)
            {
                case "left":
                    return point;
                case "center":
                    return new Point(point.X - size.width/2f, point.Y);
                case "right":
                    return new Point(point.X - size.width, point.Y);
            }
            return point;
        }


        public static string ExtractFontName(string font)
        {
            var regex = new Regex(FONT_REGEX);
            if (regex.IsMatch(font))
            {
                Match match = regex.Match(font);
                return match.Groups["font"].Value;
            }
            return string.Empty;
        }

        public static double ExtractFontEmSize(string font, double dpi)
        {
            var regex = new Regex(FONT_REGEX);
            double sizeDouble = 0;
            if (regex.IsMatch(font))
            {
                Match match = regex.Match(font);
                string size = match.Groups["size"].Value;
                string metric = match.Groups["metric"].Value;
                double.TryParse(size, out sizeDouble);
                switch (metric)
                {
                    case "pt": //according to http://alanle.com/2009/02/27/font-size-tip-for-silverlight/
                        //Traditionally, a point is 1/72 of an inch. A Silverlight pixel renders at 1/96 of an inch. 
                        //To convert a point to a pixel, I need to multiply the point by 96/72 or 1.333
                        sizeDouble = ConvertPtToPixel(sizeDouble, dpi);
                        break;
                }
            }
            return sizeDouble;
        }
    }
}