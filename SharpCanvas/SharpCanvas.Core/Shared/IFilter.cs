#if WINDOWS
using Bitmap = System.Drawing.Bitmap;
using Rectangle = System.Drawing.Rectangle;
#else
using Bitmap = SkiaSharp.SKBitmap;
using Rectangle = SkiaSharp.SKRectI;
#endif

namespace SharpCanvas.Shared
{
    public interface IFilter
    {
        Bitmap? ApplyFilter(Bitmap? source);
        Bitmap? ApplyFilter(Bitmap? source, Rectangle area);
    }
}