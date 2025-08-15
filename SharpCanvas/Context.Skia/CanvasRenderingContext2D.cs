using SharpCanvas.Shared;
using SkiaSharp;

namespace SharpCanvas.Context.Skia
{
    public class CanvasRenderingContext2D : SkiaCanvasRenderingContext2DBase
    {
        public CanvasRenderingContext2D(SKSurface surface, IDocument document) : base(surface, document)
        {
        }
    }
}
