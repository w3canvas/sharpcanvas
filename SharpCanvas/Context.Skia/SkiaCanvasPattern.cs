#nullable enable
using SkiaSharp;
using SharpCanvas.Shared;

namespace SharpCanvas.Context.Skia
{
    public class SkiaCanvasPattern
    {
        private readonly SKBitmap _bitmap;
        private readonly SKShaderTileMode _tileX;
        private readonly SKShaderTileMode _tileY;
        private SKMatrix _localMatrix;

        public SkiaCanvasPattern(SKBitmap bitmap, string repetition)
        {
            _bitmap = bitmap;
            repetition = repetition.ToLower();
            _localMatrix = SKMatrix.CreateIdentity();

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

        public void setTransform(DOMMatrix matrix)
        {
            if (matrix == null)
            {
                _localMatrix = SKMatrix.CreateIdentity();
                return;
            }

            _localMatrix = new SKMatrix
            {
                ScaleX = (float)matrix.a,
                SkewX = (float)matrix.c,
                TransX = (float)matrix.e,
                SkewY = (float)matrix.b,
                ScaleY = (float)matrix.d,
                TransY = (float)matrix.f,
                Persp0 = 0,
                Persp1 = 0,
                Persp2 = 1
            };
        }

        public SKShader GetShader()
        {
            return SKShader.CreateBitmap(_bitmap, _tileX, _tileY).WithLocalMatrix(_localMatrix);
        }
    }
}
