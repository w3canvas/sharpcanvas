#nullable enable
using System;
using System.Reflection;
using SharpCanvas.Shared;
using System.Drawing;

namespace SharpCanvas.Host.Window
{
    public class FilterProxy : IFilter
    {
        private object? _filter;
        private MethodInfo? _method;
        private MethodInfo? _methodWithRect;

        public FilterProxy(string name, params object[] args)
        {
            Assembly assembly = Assembly.Load("SharpCanvas.StandardFilter");
            Type? type = assembly.GetType(string.Format("SharpCanvas.StandardFilter.FilterSet.{0}Filter", name), true, true);
            if (type != null)
            {
                _filter = Activator.CreateInstance(type, args);
                _method = type.GetMethod("ApplyFilter", new Type[] { typeof(Bitmap) });
                _methodWithRect = type.GetMethod("ApplyFilter", new Type[] { typeof(Bitmap), typeof(Rectangle) });
            }
        }

        public Bitmap? ApplyFilter(Bitmap? source)
        {
            if (_method != null && _filter != null)
            {
                return (Bitmap?)_method.Invoke(_filter, new object?[] { source });
            }
            return null;
        }

        public Bitmap? ApplyFilter(Bitmap? source, Rectangle area)
        {
            if (_methodWithRect != null && _filter != null)
            {
                return (Bitmap?)_methodWithRect.Invoke(_filter, new object?[] { source, area });
            }
            return null;
        }
    }
}