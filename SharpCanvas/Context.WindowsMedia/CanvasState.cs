using System.Windows.Media;
// using System.Linq;

namespace SharpCanvas.Media
{
    public class CanvasState
    {
        public CanvasState(CanvasStyle style, MatrixTransform transformation)
        {
            Style = style;
            Transformation = transformation;
        }

        public CanvasStyle Style { get; set; }

        public MatrixTransform Transformation { get; set; }
    }
}