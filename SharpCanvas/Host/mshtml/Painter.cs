using System;
using System.Collections.Generic;
using SharpCanvas.Interop;
using IHTMLPainter = SharpCanvas.Interop.IHTMLPainter;
using System.Drawing;
using SharpCanvas.Host.Prototype;

namespace SharpCanvas.Host.mshtml
{
    class Painter : ObjectWithPrototype, IHTMLPainter
    {
        private IHTMLPaintSite _paintSite;
        // protected IAnimatableElement _element;
        private Graphics _graphics;

        /// <summary>
        /// Using ObjectWithPrototype
        /// </summary>
        private object _init;
        public object init
        {
            get { return _init; }
            set { _init = value; }
        }
        

        public Painter()
            : base(Guid.Empty)
        {
        }

        public Painter(Guid scope)
            : base(scope)
        {
        }

        /// <summary>
        /// Pointer to destination surface.
        /// </summary>      

        public IHTMLPaintSite PaintSite
        {
            get { return _paintSite; }
            set { _paintSite = value; }
        }

        protected int _height;
        protected int _width;
//      FIXME: Belongs in HTMLCanvasElement?
//      private bool _isChanged;
        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
            }
        }
        public int height
        {
            get { return _height; }
            set
            {
                _height = value;
            }
        }

        #region Implementation of IHTMLPainter

        /// <summary>
        /// IE will, in fact, call Draw() more than once in drawing the element, as it paints the element rectangle in successive horizontal bands. 
        /// For this reason, it is best to prepare an off-screen bitmap image before element invalidation, so that successive calls to Draw() 
        /// deal with an unchanging graphic. When IE calls Draw(), the drawing requirements dictated by IE are obtained through GetDrawInfo(), 
        /// and any necessary image transformation and clipping can then be applied before rendering with Graphics.DrawImage().
        /// </summary>
        /// <param name="rcBounds"></param>
        /// <param name="rcUpdate"></param>
        /// <param name="lDrawFlags"></param>
        /// <param name="hdc"></param>
        /// <param name="pvDrawObject"></param>
        // FIXME: Move this to an IE specific HTMLCanvasElement implementation.
        public void Draw(tagRECT rcBounds, tagRECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            // _element.Paint -= _element.HTMLCanvasElement_Paint;

            _graphics = Graphics.FromHdc(hdc);
            _graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            // Apply any scaling, etc. to the output.
            _HTML_PAINT_DRAW_INFO info;
            _paintSite.GetDrawInfo(
                (int)_HTML_PAINT_DRAW_INFO_FLAGS.HTMLPAINT_DRAWINFO_XFORM |
                (int)_HTML_PAINT_DRAW_INFO_FLAGS.HTMLPAINT_DRAWINFO_UPDATEREGION,
                out info);
            var xform = new System.Drawing.Drawing2D.Matrix(
                info.xform.eM11, info.xform.eM12,
                info.xform.eM21, info.xform.eM22,
                info.xform.eDx - rcBounds.left, info.xform.eDy - rcBounds.top);
            _graphics.Transform = xform;

            // Update clipping region.
            var clip = new Region();
            if (info.hrgnUpdate != IntPtr.Zero)
            {
                Region updateclip = Region.FromHrgn(info.hrgnUpdate);
                clip.Intersect(updateclip);
                clip.Translate(rcBounds.left, rcBounds.top);
            }
            _graphics.SetClip(clip, System.Drawing.Drawing2D.CombineMode.Replace);

            if (init != null) //run only once
            {
                _init.GetType().InvokeMember("", System.Reflection.BindingFlags.InvokeMethod, null, _init,
                                             new object[] { });
                _init = null;
            }

            // _element.Draw(_graphics, rcBounds, rcUpdate);
            _graphics.Dispose();
        }


        /// <summary>
        /// Called by CanvasRenderingContext2D in order to notify parent objects 
        /// that changes were made, so redrawing should be performed if necessary.
        /// </summary>
        public void ReDraw()
        {
            //if this is IE
            if (_paintSite != null)
            {
                var rect = new tagRECT();
                rect.top = 0;
                rect.bottom = height; // + _element.Top;
                rect.left = 0;
                rect.right = width; // + _element.Left;
                _paintSite.InvalidateRect(ref rect);
            }
            // belongs in HTMLCanvasElement?
            // _isChanged = true;
        }

        public void GetPainterInfo(out _HTML_PAINTER_INFO pInfo)
        {
            pInfo.lFlags = (int)(_HTML_PAINTER.HTMLPAINTER_TRANSPARENT |
                                  _HTML_PAINTER.HTMLPAINTER_NOPHYSICALCLIP
                                  | _HTML_PAINTER.HTMLPAINTER_SUPPORTS_XFORM | _HTML_PAINTER.HTMLPAINTER_NOSAVEDC);
            // Possibly also: | _HTML_PAINTER.HTMLPAINTER_HITTEST.

            pInfo.lZOrder = (int)_HTML_PAINT_ZORDER.HTMLPAINT_ZORDER_REPLACE_ALL;
            pInfo.iidDrawObject = Guid.Empty; // No drawing object; using GDI+.
            pInfo.rcExpand.left = pInfo.rcExpand.right =
                                  pInfo.rcExpand.top = pInfo.rcExpand.bottom = 0;
/*
            pInfo = new _HTML_PAINTER_INFO();
            pInfo.lFlags = (int)(_HTML_PAINTER.HTMLPAINTER_OPAQUE | _HTML_PAINTER.HTMLPAINTER_HITTEST);
            pInfo.lZOrder = (int)_HTML_PAINT_ZORDER.HTMLPAINT_ZORDER_REPLACE_CONTENT;
 */
        }

        // FIXME: Why are there two of these?
        public void onresize(tagSIZE size)
        {
            _paintSite.InvalidateRegion(IntPtr.Zero);
        }

        /// <summary>
        /// Refresh image on resize event.
        /// </summary>
        /// <param name="size"></param>
        public void OnResize(tagSIZE size)
        {
            _paintSite.InvalidateRegion(IntPtr.Zero);
            // tags start with null size
            if (size.cx > 0 && size.cy > 0)
            {
                _height = size.cy;
                _width = size.cx;
            }
            /*
             else {
              _height = _element.height;
               _width = _element.width;
             }
            */
            return;
        }

        /// <summary>
        /// Not called unless pInfo.lFlags included _HTML_PAINTER.HTMLPAINTER_HITTEST.
        /// </summary>
        public void HitTestPoint(tagPOINT pt, out int pbHit, out int plPartID)
        {
            plPartID = 0;
            pbHit = 0;

            // if (_element == null) return;

            if (pt.x >= 0 && pt.x < _width && pt.y >= 0 && pt.y < _height)
            {
                pbHit = 1;
            }

        }

        #endregion


    }
}
