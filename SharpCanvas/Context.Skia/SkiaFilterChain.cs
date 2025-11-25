#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;
#if WINDOWS
using System.Drawing;
using System.Drawing.Imaging;
#endif

namespace SharpCanvas.Context.Skia
{
    public class SkiaFilterChain
    {
        private readonly List<IFilter> _filters = new List<IFilter>();

        public void add(IFilter filter)
        {
            _filters.Add(filter);
        }

        public SKBitmap? ApplyFilter(SKBitmap? source)
        {
#if WINDOWS
            foreach (var filter in _filters)
            {
                if (source == null)
                {
                    return null;
                }
                var sourceInfo = source.Info;
                var bitmap = new Bitmap(sourceInfo.Width, sourceInfo.Height, sourceInfo.RowBytes, PixelFormat.Format32bppPArgb, source.GetPixels());

                var filteredBitmap = filter.ApplyFilter(bitmap);

                if (filteredBitmap == null)
                {
                    return null;
                }
                var data = filteredBitmap.LockBits(new Rectangle(0, 0, filteredBitmap.Width, filteredBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
                var info = new SKImageInfo(filteredBitmap.Width, filteredBitmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                var skBitmap = new SKBitmap(info);
                skBitmap.InstallPixels(info, data.Scan0, data.Stride, null, null);
                filteredBitmap.UnlockBits(data);
                source = skBitmap;
            }
#endif
            return source;
        }
    }
}
