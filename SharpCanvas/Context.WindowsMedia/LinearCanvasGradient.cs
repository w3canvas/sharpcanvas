using System;
using System.Windows;
using System.Windows.Media;
// using System.Linq;

namespace SharpCanvas.Media
{
    public class LinearCanvasGradient : ILinearCanvasGradient
    {
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string SYNTAX_ERR = "Syntax error";

        private readonly LinearGradientBrush _brush;

        public LinearCanvasGradient(double x0, double y0, double x1, double y1)
        {
            _brush = new LinearGradientBrush();
            _brush.StartPoint = new Point(x0, y0);
            _brush.EndPoint = new Point(x1, y1);
            _brush.MappingMode = BrushMappingMode.Absolute;
        }

        #region ILinearCanvasGradient Members

        public void addColorStop(double offset, string color)
        {
            if (!ColorUtils.isValidColor(color))
                throw new Exception(SYNTAX_ERR);
            if (offset < 0 || offset > 1)
                throw new Exception(INDEX_SIZE_ERR);
            Color parsedColor = ColorUtils.ParseColor(color);
            _brush.GradientStops.Add(new GradientStop(parsedColor, offset));
        }

        public object GetBrush()
        {
            return _brush;
        }

        #endregion
    }
}