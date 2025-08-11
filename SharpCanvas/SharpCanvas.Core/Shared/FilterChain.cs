#if WINDOWS
using Bitmap = System.Drawing.Bitmap;
#else
using Bitmap = SkiaSharp.SKBitmap;
#endif
using System.Collections.Generic;
using SharpCanvas.Shared;

namespace SharpCanvas.Shared
{
    public class FilterChain
    {
        private readonly List<IFilter> filters = new List<IFilter>();

        public int Count
        {
            get { return filters.Count; }
        }

        public List<IFilter> GetFilters()
        {
            return filters;
        }

        public void AddFilter(IFilter filter)
        {
            filters.Add(filter);
        }

        public void RemoveFilter(IFilter filter)
        {
            filters.Remove(filter);
        }

        public void RemoveFilterAt(int index)
        {
            filters.RemoveAt(index);
        }

        public void Clear()
        {
            filters.Clear();
        }

        public Bitmap? ApplyFilters(Bitmap? source)
        {
            Bitmap? result = source;
            if (filters.Count > 0)
            {
                foreach (IFilter filter in filters)
                {
                    result = filter.ApplyFilter(result);
                }
            }
            return result;
        }
    }
}