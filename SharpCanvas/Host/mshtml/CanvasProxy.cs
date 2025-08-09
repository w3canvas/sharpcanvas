using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
// using SharpCanvas.Prototype.HTMLPainter
using SharpCanvas.Host.Prototype;
using SharpCanvas.Interop;
using SharpCanvas.Core.Shared;
using _BEHAVIOR_LAYOUT_INFO = SharpCanvas.Interop._BEHAVIOR_LAYOUT_INFO;
using _BEHAVIOR_LAYOUT_MODE = SharpCanvas.Interop._BEHAVIOR_LAYOUT_MODE;
using _HTML_PAINTER_INFO = SharpCanvas.Interop._HTML_PAINTER_INFO;
using Convert = System.Convert;
using IElementBehavior = SharpCanvas.Interop.IElementBehavior;
using IElementBehaviorLayout = SharpCanvas.Interop.IElementBehaviorLayout;
using IElementBehaviorSite = SharpCanvas.Interop.IElementBehaviorSite;
using IHTMLDocument2 = SharpCanvas.Interop.IHTMLDocument2;
using IHTMLElement = SharpCanvas.Interop.IHTMLElement;
using IHTMLObjectElement = SharpCanvas.Interop.IHTMLObjectElement;
using tagPOINT = SharpCanvas.Interop.tagPOINT;
using tagRECT = SharpCanvas.Interop.tagRECT;
using tagSIZE = SharpCanvas.Interop.tagSIZE;

namespace SharpCanvas.Host.mshtml
{
    [Guid("27664326-55d8-4a73-9362-4f5fd5ec7d18")]
    [ComVisible(true)]
    [ComDefaultInterface(typeof(global::SharpCanvas.Shared.IHTMLCanvasElement))]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    internal class CanvasProxy : // HTMLPainterWithPrototype
        ObjectWithPrototype, 
        IElementBehavior,
        IElementBehaviorLayout,
        IHTMLPainter,
        global::SharpCanvas.Shared.IHTMLCanvasElement
    {
        #region Internet Explorer Integration Interfaces

        private IHTMLElement _canvasElt;
        private global::SharpCanvas.Shared.IHTMLCanvasElement _canvasWindow;
        private IElementBehaviorSite _site;
        private IHTMLElement _windowElt;

        #endregion

        #region ObjectWithPrototype Constructor
        // ObjectWithPrototype
        private object _init;

        public object init
        {
            get { return _init; }
            set { _init = value; }
        }

        public CanvasProxy()
            : base(Guid.Empty)
        {
        }

        public CanvasProxy(Guid scope)
            : base(scope)
        {
        }
        #endregion Constructor

        #region Layout Properties

        private int _height;

        private int _left;
        private int _top;
        private int _width;

        #endregion

        #region IElementBehavior Members

        void IElementBehavior.Detach()
        {
            _site = null;
        }

        void IElementBehavior.Init(IElementBehaviorSite pBehaviorSite)
        {
            _site = pBehaviorSite;
        }

        void IElementBehavior.Notify(int lEvent, IntPtr pVar)
        {
            // We're notified of a couple of interesting event while parsing the document or leaving the page
            // Here we figure out the events of interest
            switch ((_BEHAVIOR_EVENT)lEvent)
            {
                // At various points during the loading of the HTML document, IE calls Notify(), each time with a different lEvent flag. 
                // (Actually, only calls with the FIRST and DOCUMENTREADY event flags occur in this example.) 
                // The crucial call for us is when the document has been loaded, when lEvent equals BEHAVIOREVENT_DOCUMENTREADY. 
                // At this point we know that the document, element, and parent windows are available, and references to these can be saved for future use.
                case _BEHAVIOR_EVENT.BEHAVIOREVENT_CONTENTREADY:

                    if (_site != null)
                    {
                        // Store the canvas element that's our serialized form for future reference
                        _canvasElt = _site.GetElement();

                        //////////
                        // Add all members with dispids from the concrete type
                        //////////
                        base.AddIntrinsicMembers();

                        RegisterHtmlAttributesAsExpandoItems();

                        // use the element to get the height and width of the canvas
                        GetAttributeValue("width", out _width);
                        GetAttributeValue("height", out _height);
                        GetAttributeValue("left", out _left);
                        GetAttributeValue("top", out _top);

                        // Create the real forms control by inserting an object tag as child. This
                        // prevents interferance between the MSHTML COM hosting interfaces and the
                        // .NET hosting (ControlAxSourcingSite and ieexec).
                        _windowElt = ((IHTMLDocument2)_canvasElt.document).createElement("object");
                        _windowElt.setAttribute("width", _width.ToString(), 0);
                        _windowElt.setAttribute("height", _height.ToString(), 0);

                        //// Drawing to absolute point locations 
                        //// requires absolute position style.
                        _canvasElt.style.setAttribute("position", "absolute", 0);
                        _canvasElt.style.setAttribute("display", "block", 0);

                        //apply appropriate behavior to the child object
                        Bootstrapper.Initialize((IHTMLObjectElement)_windowElt);
                        //add newly created object to the DOM
                        var eltNode = (IHTMLDOMNode)_canvasElt;
                        eltNode.appendChild((IHTMLDOMNode)_windowElt);

                        // The object tag allows us to touch the underlying object, which is the ax
                        // host. ControlAxSourcingSite is a generic proxy that allows itself to be
                        // cast to its target.
                        _canvasWindow =
                            ((IHTMLObjectElement)_windowElt).@object as global::SharpCanvas.Shared.IHTMLCanvasElement;
                        _canvasWindow.PaintSite = (IHTMLPaintSite)_site;
                        _canvasWindow.width = _width;
                        _canvasWindow.height = _height;
                        //add back reference to document element - needed for events workaround (bubbling)
                        //if (_canvasWindow is HTMLElement)
                        //{
                        //    ((HTMLElement) _canvasWindow).document =
                        //        (IHTMLDocument4) _canvasElt.document;
                        //}
                        // FIXME: We need a reference to the window object for events and the window.document
                        // tag for reflection activity.
                    }

                    break;

                case _BEHAVIOR_EVENT.BEHAVIOREVENT_DOCUMENTREADY:

                    // Change the style of the canvas element to set size and visibility
                    _canvasElt.style.pixelWidth = _width;
                    _canvasElt.style.pixelHeight = _height;
                    if (_canvasWindow != null)
                    {
                        _canvasWindow.width = _width;
                        _canvasWindow.height = _height;
                    }
                    _canvasElt.style.visibility = "visible";

                    /*Old version
					if (this._canvasElt != null)
                    {
                        // Change the style of the canvas element to set size and visibility
                        this._canvasElt.style.width = this._width;
                        this._canvasElt.style.height = this._height;
                        if (this._canvasWindow != null)
                        {
                            this._canvasWindow.width = this._width;
                            this._canvasWindow.height = this._height;
                        }
                        this._canvasElt.style.visibility = "visible";
                    }*/
                    break;
            }
        }

        #endregion

        #region IElementBehaviorLayout Members

        int IElementBehaviorLayout.GetLayoutInfo()
        {
            return (int)_BEHAVIOR_LAYOUT_INFO.BEHAVIORLAYOUTINFO_MODIFYNATURAL;
        }

        void IElementBehaviorLayout.GetPosition(int lFlags, ref tagPOINT pptTopLeft)
        {
            return;
        }

        void IElementBehaviorLayout.GetSize(int dwFlags, tagSIZE sizeContent, ref tagPOINT pptTranslateBy,
                                            ref tagPOINT pptTopLeft, ref tagSIZE psizeProposed)
        {
            var layoutMode = (_BEHAVIOR_LAYOUT_MODE)dwFlags;

            psizeProposed.cx = _width;
            psizeProposed.cy = _height;
            sizeContent.cx = _width;
            sizeContent.cy = _height;
            pptTopLeft.x = _left;
            pptTopLeft.y = _top;
        }

        void IElementBehaviorLayout.MapSize(ref tagSIZE psizeIn, out tagRECT prcOut)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IHTMLCanvasElement Members

        public string Identifier { get; set; }

        public int ZIndex
        {
            get { return _canvasWindow.style.zIndex; }
            set { _canvasWindow.style.zIndex = value; }
        }

        public bool IsVisible
        {
            get { return _canvasWindow.style.display != "none"; }
        }

        public ICanvasProxy GetProxy()
        {
            throw new NotImplementedException();
        }

        public void RequestDraw()
        {
            _canvasWindow.RequestDraw();
        }

        public int width
        {
            get { return _width; }
            set
            {
                _width = value;

                if (_windowElt != null)
                {
                    _windowElt.style.width = _width.ToString();
                }

                if (_canvasWindow != null)
                {
                    _canvasWindow.width = _width;
                }
            }
        }

        public int height
        {
            get { return _height; }
            set
            {
                _height = value;

                if (_windowElt != null)
                {
                    _windowElt.style.height = _height.ToString();
                }

                if (_canvasWindow != null)
                {
                    _canvasWindow.height = _height;
                }
            }
        }

        public string toDataURL(string type, params object[] args)
        {
            if (_canvasWindow != null)
            {
                return _canvasWindow.toDataURL(type, args);
            }

            return null;
        }

        [DispId(12)]
        public object getContext(string contextId)
        {
            if (_canvasWindow != null)
            {
                return _canvasWindow.getContext(contextId);
            }

            return null;
        }

        [DispId(13)]
        public void setAttribute(string name, object value)
        {
            if (_canvasWindow != null)
            {
                ((IStyleSupported)_canvasWindow).setAttribute(name, value, _canvasElt);
            }
        }

        public ICSSStyleDeclaration style
        {
            get { throw new NotImplementedException(); }
        }

        [DispId(14)]
        public void addEventListener(string type, Delegate listener, bool useCapture)
        {
            if (_canvasWindow != null)
            {
                (_canvasWindow).addEventListener(type, listener, useCapture);
            }
        }

        public object PaintSite { get; set; }

        public ICanvasRenderingContext2D getCanvas()
        {
            return null;
        }

        /// <summary>
        /// Returns the number of pixels that the upper left corner of the current element is offset to the left within the offsetParent node.
        /// </summary>
        public int offsetLeft
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        public int offsetTop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        // FIXME: Delete IHTMLPainter region once the Painter class is in use.
        #region IHTMLPainter Members

        void IHTMLPainter.Draw(tagRECT rcBounds, tagRECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            //if child object - surface to draw - is ready,then continue drawing on the specified surface
            //NOTE: in IE environment drawing occurs in several steps, each drawing step draws stripe with height ~ 150px and with widht = 100%
            //thats why we have to consider rcUpdate region
            if (_canvasWindow != null)
            {
                ((IHTMLPainter)_canvasWindow).Draw(rcBounds, rcUpdate, lDrawFlags, hdc, pvDrawObject);
            }

            if (init != null) //run only once
            {
                _init.GetType().InvokeMember("", BindingFlags.InvokeMethod, null, _init,
                                             new object[] { });
                _init = null;
            }
        }

        void IHTMLPainter.GetPainterInfo(out _HTML_PAINTER_INFO pInfo)
        {
            pInfo = new _HTML_PAINTER_INFO();
            pInfo.lFlags = (int)(_HTML_PAINTER.HTMLPAINTER_OPAQUE | _HTML_PAINTER.HTMLPAINTER_HITTEST);
            pInfo.lZOrder = (int)_HTML_PAINT_ZORDER.HTMLPAINT_ZORDER_REPLACE_CONTENT;
        }

        void IHTMLPainter.HitTestPoint(tagPOINT pt, out int pbHit, out int plPartID)
        {
            plPartID = 0;
            pbHit = 0;

            if (_canvasWindow == null)
            {
                return;
            }

            if (pt.x >= 0 && pt.x < _width && pt.y >= 0 && pt.y < _height)
            {
                pbHit = 1;
            }

            return;
        }

        //Implemented just for IHTMLPainer interface consistence
        public void ReDraw()
        {
            throw new NotImplementedException();
        }

        void IHTMLPainter.OnResize(tagSIZE size)
        {
            // Rule out invalide params, custom tags start with null size
            if (size.cx > 0 && size.cy > 0)
            {
                _height = size.cy;
                _width = size.cx;
            }

            return;
        }

        #endregion

        // IE Utilities, mainly, expandos.
        #region Utility members

        private void GetAttributeValue<T>(string attrName, out T value)
        {
            object objValue = _canvasElt.getAttribute(attrName, 0);
            value = default(T);

            if (objValue == null || objValue is DBNull)
            {
                return;
            }

            value = (T)Convert.ChangeType(objValue, typeof(T));
        }

        private void RegisterHtmlAttributesAsExpandoItems()
        {
            // Use xml (the HTML parser documentation states that all elements will be properly closed)
            // to find out about our own attributes. Going through the html interfaces will cause IE to
            // invoke our expando implementation. And since we don't know the attributes yet, that is
            // guaranteed to fail.

            // We need to advance to the real canvas element, because the HTML parser works with its own
            // idea of what it considers a valid PI.

            // Find the namespace prefix
            string canvasPrefix = ((IHTMLElement2)_canvasElt).scopeName;

            // Find the namespace used for the mapping
            string canvasNamespace = ((IHTMLElement2)_canvasElt).tagUrn;

            // The full name of the canvas element
            string canvasElementName = string.Format("{0}:canvas", canvasPrefix);

            // Get the html describing the canvas element
            string outerHtml = _canvasElt.outerHTML;

            // Advance to the real canvas element, skipping al PI's and comments
            int canvasIndex = outerHtml.IndexOf(canvasElementName);
            if (canvasIndex < 0)
            {
                throw new ArgumentException(
                    string.Format("Expected an HTML document with a canvas element of '{0}', got '{1}'",
                                  canvasElementName, outerHtml));
            }

            // Skip all comments and PI's -- they add nothing and the PI that IE uses is not XML 1.0 + NS
            // compliant
            outerHtml = outerHtml.Substring(canvasIndex - 1); // -1 for the starting '<'

            // Replace prefix:canvas by canvas. The prefix mapping is missing and no XML parser in .NET
            // can cope with a fragment like that
            outerHtml = outerHtml.Replace(canvasElementName, "canvas");

            //////////
            // Another surprise: the html parser returns outer html with all attributes in XML compliant
            // form EXCEPT the id attribute. The value is unquoted. Need to fix that in the string as
            // the XML parser will choke on it ...
            //////////
            outerHtml = Regex.Replace(outerHtml, @"(\w+)=(\w+)", m =>
            {
                // Index 0 is the complete match. First group starts at 1
                string attribute = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                // Return attribute="value"
                return string.Format("{0}=\"{1}\"", attribute,
                                     value);
            });

            // Create a text reader based on the outer html
            var htmlReader = new StringReader(outerHtml);

            // Create an xml reader on top of the canvas html element
            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.None;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader readerForAttributes = XmlReader.Create(htmlReader, settings);

            // Read all attributes and values
            readerForAttributes.Read();
            readerForAttributes.MoveToElement();
            if (readerForAttributes.MoveToFirstAttribute())
            {
                do
                {
                    PropertyInfo piAttr = null;
                    if (MemberExists(readerForAttributes.Name))
                    {
                        piAttr = GetProperty(readerForAttributes.Name, BindingFlags.Public | BindingFlags.Instance);
                    }
                    else
                    {
                        piAttr = AddProperty(readerForAttributes.Name);
                    }

                    if (!piAttr.PropertyType.IsAssignableFrom(typeof(string)))
                    {
                        piAttr.SetValue(this,
                                        Convert.ChangeType(readerForAttributes.Value, piAttr.PropertyType),
                                        BindingFlags.Default, null, null, null);
                    }
                    else
                    {
                        piAttr.SetValue(this, readerForAttributes.Value, BindingFlags.Default, null, null, null);
                    }
                    Debug.WriteLine(string.Format("{0}: {1}", readerForAttributes.Name, readerForAttributes.Value));
                } while (readerForAttributes.MoveToNextAttribute());
            }
        }

        /// <summary>
        /// Expose current type's properties
        /// </summary>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>

        public override void GetMember<T>(string name, out T member)
        {
            //search members in current object first
            MemberInfo[] foundMembers = typeof(CanvasProxy).GetMember(name);
            if (foundMembers.Length > 0)
            {
                member = (T)foundMembers[0];
                return;
            }
            if (name.Contains("[DISPID="))
            //in some cases IE gives us only dispid instead of name, so here is a chance to find the member
            {
                int dispid = base.GetDispid(name);
                // Get metadata by dispid
                foreach (MemberInfo m in typeof(CanvasProxy).GetMembers())
                {
                    // Get the dispid attributes defined on this member
                    object[] dispidAttrs = m.GetCustomAttributes(typeof(DispIdAttribute), true);
                    if (dispidAttrs != null && dispidAttrs.Length > 0)
                    {
                        if (((DispIdAttribute)dispidAttrs[0]).Value == dispid)
                        {
                            member = (T)m;
                            return;
                        }
                    }
                }
            }
            base.GetMember(name, out member);
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            var properties = new List<PropertyInfo>();
            //get Canvas real properties
            properties.AddRange(typeof(CanvasProxy).GetProperties(bindingAttr));
            //get Expando properties
            properties.AddRange(base.GetProperties(bindingAttr));
            return properties.ToArray();
        }
        #endregion
    }
}