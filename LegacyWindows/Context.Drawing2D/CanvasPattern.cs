using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpCanvas.Context.Drawing2D
{
    public class CanvasPattern
    {
        private readonly string _imagePath;
        private readonly string _repetition;
        private ICanvasRenderingContext2D _context2D;

        public CanvasPattern(string _repetition, string _imagePath)
        {
            this._repetition = _repetition;
            this._imagePath = _imagePath;
        }

        public CanvasPattern(string repetition, ICanvasRenderingContext2D context2D)
        {
            _repetition = repetition;
            _context2D = context2D;
        }

        public TextureBrush GetBrush(Matrix matrix)
        {
            Bitmap bmp;
            if (_context2D != null)
            {
                byte[] bytes = _context2D.GetBitmap();
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    bmp = new Bitmap(ms);
                }
            }
            else
            {
                bmp = new Bitmap(_imagePath);
            }
            WrapMode wm = WrapMode.Tile;
            switch (_repetition)
            {
                case "repeat":
                    wm = WrapMode.Tile;
                    break;
                case "no-repeat":
                    wm = WrapMode.Clamp;
                    break;
                case "repeat-x":
                    wm = WrapMode.TileFlipX;
                    break;
                case "repeat-y":
                    wm = WrapMode.TileFlipY;
                    break;
            }
            var brush = new TextureBrush(bmp, wm);
            brush.MultiplyTransform(matrix);
            return brush;
        }
    }
}