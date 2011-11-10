using System;
using System.Collections.Generic;
using System.Text;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    public class IFrame
    {
        Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private IWindow _parentWindow;

        public void setAttribute(string key, string value)
        {
            if(_attributes.ContainsKey(key))
            {
                _attributes[key] = value;
            }
            else
            {
                _attributes.Add(key, value);
            }
        }

        public T getAttribute<T>(string key)
            where T: IConvertible
        {
            if (_attributes.ContainsKey(key))
            {
                string value = _attributes[key];
                return Parse<T>(value);
            }
            return default(T);
        }

        public T Parse<T>(string sourceValue) where T : IConvertible
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }

        public IFrame(IWindow parent)
        {
            this._parentWindow = parent;
        }

        public IWindow ParentWindow
        {
            get { return _parentWindow; }
        }
    }
}
