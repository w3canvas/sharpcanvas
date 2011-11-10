using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas;
using System.Drawing;
using SharpCanvas.Forms;
using SharpCanvas.Host.Prototype;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser.Forms
{
    [ComDefaultInterface(typeof(global::SharpCanvas.Interop.IHTMLCanvasElement))]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class ContextProxy: ObjectWithPrototype, ICanvasRenderingContext2D, IReflect, IContextProxy
    {
        #region Fields

        // ProxyTarget
        private ICanvasRenderingContext2D _target;
        private ICanvasProxy _canvasElement;

        #endregion

        #region Constructors

        public ContextProxy(Graphics s, Bitmap bitmap, Pen stroke, IFill fill, bool visible, ICanvasProxy canvasElement)
            : base(Guid.NewGuid())
        {
            _target = new CanvasRenderingContext2D(s, bitmap, stroke, fill, visible, canvasElement);
            _target.OnPartialDraw += new OnPartialDrawHanlder(_target_OnPartialDraw);
            //////////
            // Add all members with dispids from the concrete type
            //////////
            base.AddIntrinsicMembers();
            _canvasElement = canvasElement;
        }

        void _target_OnPartialDraw()
        {
            if(OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }
       
        #endregion

        #region CanvasRenderingContext2D

        // Fix
        [DispId(1)]
        new public object prototype() { return null; }

        [DispId(2)]
        public object __proto__
        {
            get { return null; }
            set { return; }
        }

        // State
        [DispId(5)]
        public void save() { _target.save(); }

        [DispId(6)]
        public void restore() { _target.restore(); }

        // Transforms
        [DispId(10)]
        public void scale([In] double x, [In] double y) { _target.scale(x,y); }
        [DispId(11)]
        public void rotate([In] double angle) { _target.rotate(angle); }

        [DispId(12)]
        public void translate([In] double x, [In] double y) { _target.translate(x,y); }

        [DispId(13)]
        public void transform([In] double m11, [In] double m12, [In] double m21, [In] double m22, [In] double dx,
                       [In] double dy) { _target.transform(m11,m12,m21,m22,dx,dy); }

        [DispId(14)]
        public void setTransform([In] double m11, [In] double m12, [In] double m21, [In] double m22, [In] double dx,
                          [In] double dy) { _target.setTransform(m11,m12,m21,m22,dx,dy); }

        // properties
        [DispId(20)]
        public double globalAlpha
        {
            get { return _target.globalAlpha; }
            set { _target.globalAlpha = value; }
        }

        [DispId(21)]
        public string globalCompositeOperation
        {
            get { return _target.globalCompositeOperation; }
            set { _target.globalCompositeOperation = value; }
        }

        [DispId(22)]
        public object strokeStyle
        {
            get { return _target.strokeStyle; }
            set { _target.strokeStyle = value; }
        }

        [DispId(23)]
        public object fillStyle
        {
            get { return _target.fillStyle; }
            set { _target.fillStyle = value; }
        }


        [DispId(24)]
        public double lineWidth
        {
            get { return _target.lineWidth; }
            set { _target.lineWidth = value; }
        }

        [DispId(25)]
        public string lineCap
        {
            get { return _target.lineCap; }
            set { _target.lineCap = value; }
        }

        [DispId(26)]
        public string lineJoin
        {
            get { return _target.lineJoin; }
            set { _target.lineJoin = value; }
        }

        [DispId(27)]
        public double miterLimit
        {
            get { return _target.miterLimit; }
            set { _target.miterLimit = value; }
        }

        // shadow properties
        [DispId(28)]
        public double shadowOffsetX
        {
            get { return _target.shadowOffsetX; }
            set { _target.shadowOffsetX = value; }
        }

        [DispId(29)]
        public double shadowOffsetY
        {
            get { return _target.shadowOffsetY; }
            set { _target.shadowOffsetY = value; }
        }

        [DispId(30)]
        public double shadowBlur
        {
            get { return _target.shadowBlur; }
            set { _target.shadowBlur = value; }
        }

        [DispId(31)]
        public string shadowColor
        {
            get { return _target.shadowColor; }
            set { _target.shadowColor = value; }
        }

        // rectangles
        [DispId(50)]
        public void clearRect([In] double x, [In] double y, [In] double w, [In] double h) { _target.clearRect(x,y,w,h); }

        [DispId(51)]
        public void fillRect([In] double x, [In] double y, [In] double w, [In] double h) { _target.fillRect(x, y, w, h); }

        [DispId(52)]
        public void strokeRect([In] double x, [In] double y, [In] double w, [In] double h) { _target.strokeRect(x, y, w, h); }

        // path API
        [DispId(60)]
        public void beginPath() { _target.beginPath(); }

        [DispId(61)]
        public void closePath() { _target.closePath(); }

        [DispId(62)]
        public void moveTo([In] double x, [In] double y) { _target.moveTo(x,y); }

        [DispId(63)]
        public void lineTo([In] double x, [In] double y) { _target.lineTo(x,y); }

        [DispId(64)]
        public void quadraticCurveTo([In] double cpx, [In] double cpy, [In] double x, [In] double y) { _target.quadraticCurveTo(cpx, cpy, x, y); }

        [DispId(65)]
        public void bezierCurveTo([In] double cp1x, [In] double cp1y, [In] double cp2x, [In] double cp2y, [In] double x,
                           [In] double y) { _target.bezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y); }

        [DispId(66)]
        public void arcTo([In] double x1, [In] double y1, [In] double x2, [In] double y2, [In] double radius) {
            _target.arcTo(x1, y1, x2, y2, radius); }

        [DispId(67)]
        public void arc([In] double x, [In] double y, [In] double r, [In] double startAngle, [In] double endAngle,
                 [In] bool clockwise) { _target.arc(x, y, r, startAngle, endAngle, clockwise); }

        [DispId(68)]
        public void rect([In] double x, [In] double y, [In] double w, [In] double h) { _target.rect(x, y, w, h); }

        // core drawIng
        [DispId(70)]
        public void fill() { _target.fill(); }

        [DispId(71)]
        public void stroke() { _target.stroke(); }

        [DispId(72)]
        public void clip() { _target.clip(); }

        // text API
        [DispId(80)]
        public string font
        {
            get { return _target.font; }
            set { _target.font = value; }
        }

        [DispId(81)]
        public string textAlign
        {
            get { return _target.textAlign; }
            set { _target.textAlign = value; }
        }

        [DispId(82)]
        public string textBaseLine
        {
            get { return _target.textBaseLine; }
            set { _target.textBaseLine = value; }
        }

        [DispId(83)]
        public void fillText([In] string text, [In] double x, [In] double y) { _target.fillText(text, x, y); }

        [DispId(84)]
        public void strokeText([In] string text, [In] double x, [In] double y) { _target.strokeText(text, x, y); }

        //[DispId(85)] void MeasureText([In] string text, [Out] ICanvasTextMetrics** pResult);

        // images
        [DispId(90)]
        public void drawImage([In] object image, [In] double sx, [In] double sy, [In] double sw, [In] double sh, [In] double dx,
                       [In] double dy, [In] double dw, [In] double dh) { _target.drawImage(image, sx, sy, sw, sh, dx, dy, dw, dh); }

        [DispId(91)]
        public void drawImage([In] object pImg, [In] double dx, [In] double dy, [In] double dw, [In] double dh) {
               _target.drawImage(pImg, dx, dy, dw, dh); }

        [DispId(92)]
        public void drawImage([In] object pImg, [In] double dx, [In] double dy) { _target.drawImage(pImg, dx, dy); }

        // poInt-membership test
        [DispId(100)]
        public bool isPointInPath([In] double x, [In] double y) { return _target.isPointInPath(x, y); }

        // pixel manipulation
        [DispId(101)]
        public object createLinearGradient([In] double x0, [In] double y0, [In] double x1, [In] double y1) { return _target.createLinearGradient(x0, y0, x1, y1); }

        [DispId(102)]
        public object createPattern([In] object pImg, [In] string repeat) { return _target.createPattern(pImg, repeat); }

        [DispId(103)]
        public object createRadialGradient([In] double x0, [In] double y0, [In] double r0, [In] double x1, [In] double y1,
                                    [In] double r1) { return _target.createRadialGradient(x0, y0, r0, x1, y1, r1); }

        [DispId(104)]
        public object measureText([In] string text) { return _target.measureText(text); }

        [DispId(105)]
        public object getImageData([In] double sx, [In] double sy, [In] double sw, [In] double sh) { return _target.getImageData(sx, sy, sw, sh); }

        [DispId(106)]
        public object createImageData([In] double sw, [In] double sh) { return _target.createImageData(sw, sh); }

        [DispId(107)]
        public void putImageData([In] object pData, [In] double dx, [In] double dy) { _target.putImageData(pData, dx, dy); }

        [DispId(108)]
        public void putImageData([In] object imagedata, [In] double dx, [In] double dy, [In] double dirtyX, [In] double dirtyY,
                          [In] double dirtyWidth, [In] double dirtyHeight) {
                              _target.putImageData(imagedata, dx, dy, dirtyX, dirtyY, dirtyWidth, dirtyHeight);
                         }


        [DispId(111)]
        public object createFilterChain() { return _target.createFilterChain(); }

        /// <summary>
        /// WPF: commits current geometry set to the surface
        /// WinForm: do nothing
        /// </summary>
        [DispId(112)]
        public void commit() { _target.commit(); }

        /// <summary>
        /// back-reference to the canvas
        /// </summary>
        [DispId(113)]
        public object canvas
        {
            get { return _target.canvas; }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Provides with latest image from the Canvas
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap() { return _target.GetBitmap(); }

        /// <summary>
        /// Change size of canvas and underlying controls.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ChangeSize(int width, int height, bool reset)
        {
            _target.ChangeSize(width, height, reset);
        }

        /// <summary>
        /// Return current height of the surface
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return _target.GetHeight();
        }

        /// <summary>
        /// Return current width of the surface
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return _target.GetWidth();
        }

        /// <summary>
        /// Indicates wherever it visible or not (use in WinForm env.)
        /// </summary>
        public bool IsVisible
        {
            get { return _target.IsVisible; }
            set { return; }
        }
        /// <summary>
        /// Occurs when some part of image was commited to the surface
        /// </summary>
        public event OnPartialDrawHanlder OnPartialDraw;

        //TODO: unify GetRealObject methods and put it into IProxy interface
        public ICanvasRenderingContext2D GetRealObject()
        {
            return _target;
        }

        /// <summary>
        /// Workaround for overloaded drawImage methods
        /// </summary>
        /// <param name="name"></param>
        /// <param name="invokeAttr"></param>
        /// <param name="binder"></param>
        /// <param name="target"></param>
        /// <param name="args"></param>
        /// <param name="modifiers"></param>
        /// <param name="culture"></param>
        /// <param name="namedParameters"></param>
        /// <returns></returns>
        public new object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
                                   ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            if (name != "drawImage")
            {
                return base.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
            }
            else
            {
                switch (args.Length)
                {
                    case 3:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]));
                        break;
                    case 5:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]),
                                  Convert.ToDouble(args[3]), Convert.ToDouble(args[4]));
                        break;
                    case 9:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]),
                                  Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]),
                                  Convert.ToDouble(args[6]), Convert.ToDouble(args[7]), Convert.ToDouble(args[8]));
                        break;
                }
            }
            return null;
        }

        public new MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            //we do support overloading of drawImage method
            if(name == "drawImage")
            {
                return typeof (ContextProxy).GetMember(name, bindingAttr);
            }
            return base.GetMember(name, bindingAttr);
        }

        #endregion

        #region Indexed Properties

        public string this[string key]
        {
            get
            {
                if (ExistsMember(key))
                {
                    MemberInfo member = null;
                    GetMember(key, out member);
                    if (member != null)
                    {
                        if (member is PropertyInfo)
                        {
                            PropertyInfo prop = member as PropertyInfo;
                            prop.GetValue(this, BindingFlags.GetProperty | BindingFlags.Public, null, new object[] { },
                                          null);
                            return key;
                        }
                    }
                }
                throw new ArgumentException(string.Format("Member {0} doesn't exists in {1} class or it's prototype.", key, this.GetType().Name));
            }
            set
            {
                //Debug.WriteLine(string.Format("{0}={1}", key, value));
                if (ExistsMember(key))
                {
                    AssignValue(value, key);
                }
                else
                {
                    AddProperty(key);
                    AssignValue(value, key);
                    //throw new ArgumentException(string.Format("Member {0} doesn't exists in {1} class or it's prototype.", key, this.GetType().Name));
                }
            }
        }

        private void AssignValue(string value, string key)
        {
            object _value = value;
            MemberInfo member = null;
            GetMember(key, out member);
            if (member != null)
            {
                if (member is PropertyInfo)
                {
                    PropertyInfo prop = member as PropertyInfo;
                    Type propertyType = prop.PropertyType;
                    if (propertyType != value.GetType())
                    {
                        if (TryConvert(value, propertyType) != null)
                        {
                            _value = TryConvert(value, propertyType);
                        }
                        else
                        {
                            throw new ArgumentException(
                                string.Format(
                                    "Member {0} from {1} has type {2}, but passed argument has type {3}.", key,
                                    this.GetType().Name, propertyType, value.GetType()));
                        }
                    }
                    prop.SetValue(this, _value, BindingFlags.SetProperty | BindingFlags.Public, null, new object[] { }, null);
                }
            }
        }

        private object TryConvert(object value, Type type)
        {
            try
            {
                return Convert.ChangeType(value, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
