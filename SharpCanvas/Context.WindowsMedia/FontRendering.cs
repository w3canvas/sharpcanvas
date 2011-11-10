using System;
using System.Windows;
using System.Windows.Media;

namespace SharpCanvas.Media
{
    /// <summary>
    /// Class for combining font and other text related properties. 
    /// (Typeface, Alignment, Decorations, etc)
    /// </summary>
    public class FontRendering
    {
        #region Constructors

        public FontRendering(
            double emSize,
            TextAlignment alignment,
            TextDecorationCollection decorations,
            Brush textColor,
            Typeface face)
        {
            _fontSize = emSize;
            TextAlignment = alignment;
            TextDecorations = decorations;
            TextColor = textColor;
            Typeface = face;
        }

        public FontRendering()
        {
            _fontSize = 12.0f;
            TextAlignment = TextAlignment.Left;
            TextDecorations = new TextDecorationCollection();
            TextColor = Brushes.Black;
            Typeface = new Typeface(new FontFamily("Arial"),
                                    FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        #endregion

        #region Properties

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Parameter Must Be Greater Than Zero.");
                if (double.IsNaN(value))
                    throw new ArgumentOutOfRangeException("value", "Parameter Cannot Be NaN.");
                _fontSize = value;
            }
        }

        public TextAlignment TextAlignment { get; set; }

        public TextDecorationCollection TextDecorations { get; set; }

        public Brush TextColor { get; set; }

        public Typeface Typeface { get; set; }

        #endregion

        #region Private Fields

        private double _fontSize;

        #endregion
    }
}