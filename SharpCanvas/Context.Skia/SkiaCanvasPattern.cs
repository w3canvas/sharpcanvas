#nullable enable
using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class SkiaCanvasPattern
    {
        private readonly SKBitmap _bitmap;
        private readonly SKShaderTileMode _tileX;
        private readonly SKShaderTileMode _tileY;

        public SkiaCanvasPattern(SKBitmap bitmap, string repetition)
        {
            _bitmap = bitmap;
            repetition = repetition.ToLower();

            switch (repetition)
            {
                case "repeat":
                    _tileX = SKShaderTileMode.Repeat;
                    _tileY = SKShaderTileMode.Repeat;
                    break;
                case "repeat-x":
                    _tileX = SKShaderTileMode.Repeat;
                    _tileY = SKShaderTileMode.Decal;
                    break;
                case "repeat-y":
                    _tileX = SKShaderTileMode.Decal;
                    _tileY = SKShaderTileMode.Repeat;
                    break;
                case "no-repeat":
                    _tileX = SKShaderTileMode.Decal;
                    _tileY = SKShaderTileMode.Decal;
                    break;
                default: // Also "repeat"
                    _tileX = SKShaderTileMode.Repeat;
                    _tileY = SKShaderTileMode.Repeat;
                    break;
            }
        }

        public SKShader GetShader()
        {
            return SKShader.CreateBitmap(_bitmap, _tileX, _tileY);
        }
    }
}
