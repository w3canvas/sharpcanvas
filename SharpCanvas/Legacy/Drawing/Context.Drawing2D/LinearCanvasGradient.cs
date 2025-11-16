using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpCanvas.Common;

namespace SharpCanvas.Context.Drawing2D
{
    /// <summary>
    /// Represents a linear gradient that can be used as a fill or stroke style in a 2D canvas context.
    /// Linear gradients are defined by a start point and an end point, with colors interpolated along the line between them.
    /// </summary>
    /// <remarks>
    /// This is the System.Drawing implementation of linear gradients. For SkiaSharp-based implementation, see SkiaLinearCanvasGradient.
    /// Use addColorStop() to add color stops to define the gradient appearance.
    /// </remarks>
        public class LinearCanvasGradient : ILinearCanvasGradient
    {
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string SYNTAX_ERR = "Syntax error";
        private List<Color> _colors = new List<Color>();

        private PointF _end;
        private List<float> _positions = new List<float>();
        private PointF _start;

        public LinearCanvasGradient(PointF _start, PointF _end)
        {
            this._start = _start;
            this._end = _end;
        }

        /// <summary>
        /// Initializes a new instance of the LinearCanvasGradient class with specified start and end points and a graphics path.
        /// </summary>
        /// <param name="_start">The starting point of the gradient</param>
        /// <param name="_end">The ending point of the gradient</param>
        /// <param name="path">The graphics path to apply the gradient to</param>
                public LinearCanvasGradient(PointF _start, PointF _end, GraphicsPath path)
        {
            this._start = _start;
            this._end = _end;
            this.path = path;
        }

        public LinearCanvasGradient()
        {
        }

        public List<float> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        public List<Color> Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        public GraphicsPath path { get; set; }

        public PointF start
        {
            get { return _start; }
            set { _start = value; }
        }

        public PointF end
        {
            get { return _end; }
            set { _end = value; }
        }

        #region ILinearCanvasGradient Members

        /// <summary>
        /// If multiple stops are added at the same offset on a gradient, they must be placed in the order added, with the first one closest to the start of the
        /// gradient, and each subsequent one infinitesimally further along towards the end point (in effect causing all but the first and last stop added at each point
        /// to be ignored).
        /// ...Between each such stop, the colors and the alpha component must be linearly interpolated over the RGBA space without premultiplying the alpha value to 
        /// find the color to use at that offset. Before the first stop, the color must be the color of the first stop. After the last stop, the color must be the color
        /// of the last stop. When there are no stops, the gradient is transparent black.
        /// </summary>
        /// <param name="offset">Must be between 0..1</param>
        /// <param name="color">Valid Color</param>
        public void addColorStop(double offset, string color)
        {
            if (!ColorUtils.isValidColor(color))
                throw new Exception(SYNTAX_ERR);
            if (offset < 0 || offset > 1)
                throw new Exception(INDEX_SIZE_ERR);
            Color parsedColor = ColorUtils.ParseColor(color);
            _colors.Add(parsedColor);
            _positions.Add((float) offset);
        }

        /// <summary>
        /// Creates and returns a LinearGradientBrush based on the gradient's color stops, start, and end points.
        /// This method is called internally when the gradient is used as a fill or stroke style.
        /// </summary>
        /// <returns>A LinearGradientBrush object configured with the gradient's color stops</returns>
        /// <remarks>
        /// If the first color stop is not at position 0.0, a stop at 0.0 is automatically added with the first stop's color.
        /// If the last color stop is not at position 1.0, a stop at 1.0 is automatically added with the last stop's color.
        /// This ensures the gradient covers the full range from start to end.
        /// </remarks>
                public object GetBrush()
        {
            //if last point is not equal to 1.0 then add such point
            if (_positions[_positions.Count - 1] != 1.0)
            {
                _positions.Add(1.0f);
                _colors.Add(_colors[_colors.Count - 1]);
            }
            //if first point is not equal to 0.0 then add such point
            if (_positions[0] != 0.0)
            {
                _positions.Insert(0, 0.0f);
                _colors.Insert(0, _colors[0]);
            }
            var brush = new LinearGradientBrush(_start, _end, _colors[0], _colors[_colors.Count - 1]);
            //PathGradientBrush brush = new PathGradientBrush(new PointF[]{_start, _end, new PointF(_end.X, _start.Y), new PointF(_start.X, _end.Y)  }, WrapMode.Clamp);
            var colorBlend = new ColorBlend(_colors.Count);
            colorBlend.Colors = _colors.ToArray();
            colorBlend.Positions = _positions.ToArray();
            brush.InterpolationColors = colorBlend;
            return brush;
        }

        #endregion
    }
}