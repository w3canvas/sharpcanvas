using SharpCanvas.Shared;
using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class OffscreenCanvasRenderingContext2D : SkiaCanvasRenderingContext2DBase
    {
        public OffscreenCanvasRenderingContext2D(SKSurface surface, IDocument document, object canvas = null) : base(surface, document, canvas)
        {
        }
    }
}
