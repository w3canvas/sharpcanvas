#nullable enable
using SharpCanvas.Shared;
using SkiaSharp;
using System.Collections.Generic;

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
            foreach (var filter in _filters)
            {
                source = filter.ApplyFilter(source);
            }
            return source;
        }
    }
}
