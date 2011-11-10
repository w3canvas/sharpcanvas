using System;
using System.Windows;
using System.Windows.Media;
//using System.Linq;

namespace SharpCanvas.Media
{
    public class PathCanvasGradient : ILinearCanvasGradient
    {
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string SYNTAX_ERR = "Syntax error";

        private readonly RadialGradientBrush _brush;
        private double _innerRadius;
        private double _outerRadius;

        public PathCanvasGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            _brush = new RadialGradientBrush();
            _brush.GradientOrigin = new Point(x0, y0);
            _brush.Center = new Point(x1, y1);
            _brush.MappingMode = BrushMappingMode.Absolute;
            _brush.RadiusX = r1;
            _brush.RadiusY = r1;
            _brush.SpreadMethod = GradientSpreadMethod.Pad;
            _innerRadius = r0;
            _outerRadius = r1;
            double rMin = Math.Min(r0, r1);
            double rMax = Math.Max(r0, r1);
            OffsetMinimum = rMin/rMax;
            OffsetMultiplier = (rMax - rMin)/rMax;
        }

        // OffsetMinimum/OffsetMultiplier are used by createRadialGradient to
        // more easily implement its starting/ending radius behavior
        internal double OffsetMinimum { get; set; }
        internal double OffsetMultiplier { get; set; }

        #region ILinearCanvasGradient Members

        public void addColorStop(double offset, string color)
        {
            if (!ColorUtils.isValidColor(color))
                throw new Exception(SYNTAX_ERR);
            if (offset < 0 || offset > 1)
                throw new Exception(INDEX_SIZE_ERR);
            _brush.GradientStops.Add(new GradientStop(ColorUtils.ParseColor(color),
                                                      OffsetMinimum + (offset*OffsetMultiplier)));
        }

        public object GetBrush()
        {
            //List<GradientStop> stopsToAdd = new List<GradientStop>();
            //for (int i = 1; i < _brush.GradientStops.Count; i++)
            //{
            //    GradientStop stop = _brush.GradientStops[i];
            //    GradientStop prevStop = _brush.GradientStops[i - 1];
            //    if (prevStop.Offset < _innerRadius / _outerRadius)
            //    {
            //        GradientStop stopToAdd = new GradientStop(prevStop.Color,
            //                                                  prevStop.Offset + stop.Offset * _innerRadius/_outerRadius);
            //        stopsToAdd.Add(stopToAdd);
            //    }
            //}
            //foreach (GradientStop add in stopsToAdd)
            //{
            //    _brush.GradientStops.Add(add);
            //}
            return _brush;
        }

        #endregion
    }
}