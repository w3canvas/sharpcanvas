using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    [ComVisible(true)]
    public class Document : HTMLElement, IDocument
    {
        private IWindow _parentWindow;
        public Document(IWindow parentWindow):base("document", (UserControl)parentWindow)
        {
            _parentWindow = parentWindow;

            Visible = true;

            //init body
            body = new HTMLElement("body", this);
            //init sizes
            ((UserControl) body).Width = this.Width = parentWindow.innerWidth;
            ((UserControl) body).Height = this.Height = parentWindow.innerHeight;
            appendChild(body);
            //(body as UserControl).BackColor = Color.Yellow;

            Name = "document";
            this.ControlAdded += new ControlEventHandler(Document_ControlAdded);
            this.Resize += new EventHandler(Document_Resize);
        }

        void Document_Resize(object sender, EventArgs e)
        {
            ((UserControl) body).Width = this.Width;
            ((UserControl) body).Height = this.Height;
        }

        void Document_ControlAdded(object sender, ControlEventArgs e)
        {
            ((IDocument)this).defaultView.RedrawChildren();
        }

        public IWindow defaultView
        {
            get{ return _parentWindow;}
        }

        public object body { get; set; }

        public string title { get; set; }

        public Graphics Graphics { get; set; }

        public ILocation location
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