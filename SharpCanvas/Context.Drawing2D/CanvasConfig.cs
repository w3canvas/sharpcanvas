using System.Drawing;

namespace SharpCanvas.Context.Drawing2D
{
    public class CanvasConfig
    {
        private readonly Fill _fill;
        private readonly Pen _stroke;

        public CanvasConfig(Pen stroke, Fill fill)
        {
            _stroke = stroke;
            _fill = fill;
        }

        public Pen Stroke
        {
            get { return (Pen) _stroke.Clone(); }
        }

        public Fill Fill
        {
            get { return (Fill) _fill.Clone(); }
        }
    }
}