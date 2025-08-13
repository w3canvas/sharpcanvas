#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;
using SkiaSharp.Views.Desktop;

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
                var bitmap = source?.ToBitmap();
                var filteredBitmap = filter.ApplyFilter(bitmap);
                source = filteredBitmap?.ToSKBitmap();
            }
#endif
            return source;
        }
    }
}
