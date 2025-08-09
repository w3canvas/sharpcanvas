using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpCanvas.Host.Browser;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Interaction logic for Document.xaml
    /// </summary>
    public partial class Document : HTMLElement, IDocument
    {
        private IWindow _parentWindow;

        public Document(IWindow parentWindow)
        {
            InitializeComponent();
            body = new HTMLElement("body");

            //LineGeometry myLineGeometry = new LineGeometry();
            //myLineGeometry.StartPoint = new Point(10, 20);
            //myLineGeometry.EndPoint = new Point(100, 130);

            //Path myPath = new Path();
            //myPath.Stroke = Brushes.Black;
            //myPath.StrokeThickness = 1;
            //myPath.Data = myLineGeometry;
            //((Canvas)body).Children.Add(myPath);

            appendChild(body);

            _parentWindow = parentWindow;
            Name = "document";
        }

        public object body { get; set; }

        public string title { get; set; }

        public ILocation location
        {
            get
            {
                if (_parentWindow != null)
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
                        element = new Window(this);
                        break;
                    }
                default:
                    {
                        element = new HTMLElement(tagName, this);
                        ((HTMLElement)element).parent = this;
                        //this.appendChild((HTMLElement)element);
                        break;
                    }
            }

            return element;
        }

        /// <summary>
        /// Creates an element with the specified namespace URI and qualified name.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public object createElementNS(string ns, string tagName)
        {
            return createElement(tagName);
        }

        /// <summary>
        /// The defaultView IDL attribute of the HTMLDocument interface must return the Document's browsing context's WindowProxy object, if there is one, or null otherwise.
        /// </summary>
        public IWindow defaultView
        {
            get { throw new NotImplementedException(); }
        }
    }
}
