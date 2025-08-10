using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class OffscreenCanvasRenderingContext2D : SkiaCanvasRenderingContext2DBase
    {
        public OffscreenCanvasRenderingContext2D(SKSurface surface) : base(surface)
        {
        }
    }
}
