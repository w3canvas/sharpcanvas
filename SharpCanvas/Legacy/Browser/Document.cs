using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas.Interop;
using SharpCanvas.Shared;
using SharpCanvas.Host.Prototype;
using SharpCanvas.Host.Browser;

namespace SharpCanvas.Browser
{
    [ComVisible(true)]
    public class Document : Node, IDocument
    {
        private IWindow _parentWindow;
        public Document(IWindow parentWindow)
        {
            _parentWindow = parentWindow;

            //init body
            body = new HTMLElement("body", this);
            //init sizes
            //((UserControl) body).Width = this.Width = parentWindow.innerWidth;
            //((UserControl) body).Height = this.Height = parentWindow.innerHeight;
            appendChild(body);

            name = "document";
        }


        public FontFaceSet fonts { get; } = new FontFaceSet();

        public IWindow defaultView
        {
            get{ return _parentWindow;}
        }

        public object body { get; set; }

        public string title { get; set; } = string.Empty;

        public Graphics Graphics { get; set; } = null!;

        public ILocation? location
        {
            get
            {
                if(_parentWindow != null)
                {
                    return _parentWindow.location;
                }
                return null;
            }
            set
            {
                if (_parentWindow != null)
                {
                    _parentWindow.location = value;
                }
            }
        }

        /// <summary>
        /// Creates an element with the specified namespace URI and qualified name.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public object createElementNS(string ns, string tagName)
        {
            return this.createElement(tagName);
        }

        public override void appendChild(object child)
        {
            if (child is INode node)
            {
                _childNodes.Add(node);
            }
        }

        public override void removeChild(object child)
        {
            if (child is INode node)
            {
                _childNodes.Remove(node);
            }
        }

        public object createElement(string tagName)
        {
            tagName = tagName.ToUpper();
            object element;
            switch (tagName)
            {
                case "CANVAS":
                    {
                        element = new CanvasProxy();
                        //CloneEvents(element, this._events);
                        break;
                    }
                case "WINDOW":
                    {
                        element = new WindowProxy(this);
                        break;
                    }
                case "IFRAME":
                    {
                        element = new IFrame(this._parentWindow);
                        break;
                    }
                default:
                    {
                        element = new HTMLElement(tagName, this);
                        ((HTMLElement)element).SetParent(this);
                        //this.appendChild((HTMLElement)element);
                        break;
                    }
            }
            return element;
        }
    }
}