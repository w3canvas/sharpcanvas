using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas
{
    [ComVisible(true)]
    public class FilterChain
    {
        private readonly List<IFilter> filterChain = new List<IFilter>();

        public FilterChain()
        {
            filterChain.Clear();
        }

        public FilterChain(IFilter filter)
        {
            filterChain.Add(filter);
        }

        public void resetChain()
        {
            filterChain.Clear();
        }

        public int AddFilter(IFilter filter)
        {
            filterChain.Add(filter);
            return filterChain.Count;
        }

        public IFilter AddFilterFromString(string filterName)
        {
            IFilter filter = null;
            switch (filterName.ToLowerInvariant())
            {
                default:
                    new Exception(string.Format("Filter '{0}' doesn't exists.", filterName));
                    break;
            }
            filterChain.Add(filter);
            return filter;
        }

        public int GetFilterCount()
        {
            return filterChain.Count;
        }

        public Bitmap ApplyFilters(Bitmap bmp)
        {
            Bitmap result = null;
            for (int i = 0; i < filterChain.Count; i++)
            {
                result = filterChain[i].ApplyFilter(bmp);
                bmp = result;
            }
            return result;
        }
    }
}