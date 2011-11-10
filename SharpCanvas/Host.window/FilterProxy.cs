using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpCanvas.Host.Prototype;

namespace SharpCanvas
{
    [ComVisible(true)]
    public class FilterProxy : ObjectWithPrototype
    {

        // ProxyTarget
        private FilterProxy _target;

        // ObjectWithPrototype
        private object _init;

        public object init
        {
            get { return _init; }
            set { _init = value; }
        }

        public FilterProxy()
            : base(Guid.Empty)
        {
        }

        public FilterProxy(Guid scope)
            : base(scope)
        {
        }

        public void resetChain()
        {
            _target.resetChain();
        }

        public int AddFilter(IFilter filter)
        {
            return _target.AddFilter(filter);
        }

        public IFilter AddFilterFromString(string filterName)
        {
            return _target.AddFilterFromString(filterName);
        }

        public int GetFilterCount()
        {
            return _target.GetFilterCount();
        }

        public Bitmap ApplyFilters(Bitmap bmp)
        {
            return _target.ApplyFilters(bmp);
        }
    }
}