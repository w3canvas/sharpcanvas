using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SharpCanvas.Media
{
    public class CanvasPattern
    {
        private readonly string _imagePath;
        private readonly string _repetition;

        public CanvasPattern(string _repetition, string _imagePath)
        {
            this._repetition = _repetition;
            this._imagePath = _imagePath;
        }

        public ImageBrush GetBrush()
        {
            var brush = new ImageBrush();
            var source = new BitmapImage(new Uri(_imagePath, UriKind.RelativeOrAbsolute));
            brush.ImageSource = source;
            brush.Stretch = Stretch.None;

            switch (_repetition)
            {
                case "repeat":
                    brush.Viewport = new Rect(0, 0, source.Width, source.Height);
                    brush.ViewportUnits = BrushMappingMode.Absolute;
                    brush.TileMode = TileMode.Tile;
                    break;
                case "no-repeat":
                    brush.TileMode = TileMode.None;
                    break;
                case "repeat-x":
                    brush.TileMode = TileMode.FlipX;
                    break;
                case "repeat-y":
                    brush.TileMode = TileMode.FlipY;
                    break;
            }
            return brush;
        }
    }
}