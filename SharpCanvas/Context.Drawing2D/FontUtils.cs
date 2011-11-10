using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SharpCanvas.Forms
{
    public static class FontUtils
    {
        public static double ConvertPtToPixel(double pt, double dpi)
        {
            return pt*dpi/72f;
        }

        public static double ConvertPixelToPt(double pixel, double dpi)
        {
            return pixel*72f/dpi;
        }

        public static float ConvertEmToPixels(float em, GraphicsUnit unit, double dpi)
        {
            switch (unit)
            {
                case GraphicsUnit.Point: //pt
                    return (float) ConvertPtToPixel(em/72f, dpi);
                default:
                    return em;
            }
        }

        public static PointF ApplyBaseLine(PointF point, Font font, string baseLine, double dpi)
        {
            float emHeight = font.FontFamily.GetEmHeight(FontStyle.Regular)/2f;
            int descent = font.FontFamily.GetCellDescent(FontStyle.Regular);
            int ascent = font.FontFamily.GetCellAscent(FontStyle.Regular);
            float extra = emHeight*2f - descent - ascent - (float) dpi;
            //gives error for one dpi, so we need to substract dpi
            switch (baseLine)
            {
                case "top": //The top of the em square
                    return point;
                case "hanging": //The hanging baseline
                    return point;
                    //return new PointF(point.X, point.Y + ConvertEmToPixels(ascent * 0.1f, font.Unit, dpi));
                case "middle": //The middle of the em square
                    return new PointF(point.X, point.Y - ConvertEmToPixels(emHeight/2f, font.Unit, dpi));
                case "alphabetic":
                    return new PointF(point.X, point.Y - ConvertEmToPixels(emHeight - descent - extra, font.Unit, dpi));
                case "ideographic":
                    return new PointF(point.X, point.Y - ConvertEmToPixels(emHeight - extra/2, font.Unit, dpi));
                case "bottom": //The bottom of the em square
                    return new PointF(point.X, point.Y - ConvertEmToPixels(emHeight + (float) dpi, font.Unit, dpi));
            }
            return point;
        }

        public static TextMetrics MeasureText(string text, Graphics surface, Font font)
        {
            SizeF sizeF = surface.MeasureString(text, font, new PointF(0, 0), StringFormat.GenericTypographic);
            return new TextMetrics((int) Math.Truncate(ConvertPixelToPt(sizeF.Width, surface.DpiX)), (int) sizeF.Height);
        }

        public static PointF ApplyTextAlign(PointF point, string align, string text, Graphics surface, Font font)
        {
            TextMetrics size = MeasureText(text, surface, font);
            switch (align)
            {
                case "left":
                    return point;
                case "center":
                    return new PointF(point.X - size.width/2f, point.Y);
                case "right":
                    return new PointF(point.X - size.width, point.Y);
            }
            return point;
        }

        public static Font ParseFont(string _font, double dpi)
        {
            var regex = new Regex(@"(?<size>\d+)(?<metric>\w+)\W+(?<font>[\w\s]+.*)");
            if (regex.IsMatch(_font))
            {
                Match match = regex.Match(_font);
                string size = match.Groups["size"].Value;
                string metric = match.Groups["metric"].Value;
                double sizeDouble = 0;
                double.TryParse(size, out sizeDouble);
                switch (metric)
                {
                    case "pt": //according to http://alanle.com/2009/02/27/font-size-tip-for-silverlight/
                        //Traditionally, a point is 1/72 of an inch. A Silverlight pixel renders at 1/96 of an inch. 
                        //To convert a point to a pixel, I need to multiply the point by 96/72 or 1.333
                        sizeDouble = ConvertPtToPixel(sizeDouble, dpi);
                        break;
                }
                return new Font(match.Groups["font"].Value, (float) (sizeDouble));
            }
            return null;
        }
    }
}