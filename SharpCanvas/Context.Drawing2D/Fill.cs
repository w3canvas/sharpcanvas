using System;
using System.Drawing;

namespace SharpCanvas.Forms
{
    public class Fill : IFill
    {
        private Brush _brush;
        private Color _color;

        public Fill(Color color)
        {
            _brush = new SolidBrush(color);
            _color = color;
        }

        public Brush brush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                _brush = new SolidBrush(_color);
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            var clone = new Fill(_color);
            return clone;
        }

        #endregion
    }
}