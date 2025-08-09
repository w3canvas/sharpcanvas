using System;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas.Host.Prototype;
using SharpCanvas.Interop;
using SharpCanvas.Core.Shared;
using Convert = System.Convert;

namespace SharpCanvas.Host.Browser
{
    /// <summary>
    /// Proxy implementation of IHTMLCanvasElement.
    /// </summary>
    public class CanvasProxy : ObjectWithPrototype, ICanvasProxy, Shared.IHTMLCanvasElement
    {
        private readonly global::SharpCanvas.Shared.IHTMLCanvasElement _realObject;

        public CanvasProxy()
            : base(Guid.NewGuid())
        {
            // Call a global constructor.
            _realObject = SharpCanvas.Host.StandaloneBootstrapper.Factory.CreateCanvasElement(this);
            //////////
            // Add all members with dispids from the concrete type
            //////////
            base.AddIntrinsicMembers();
        }

        #region HTMLCanvasElement methods and properties

        public ICanvasProxy GetProxy()
        {
            return this;
        }

        public void RequestDraw()
        {
            throw new NotImplementedException();
        }

        [DispId(3)]
        public int width
        {
            get { return _realObject.width; }
            set { _realObject.width = value; }
        }

        [DispId(4)]
        public int height
        {
            get { return _realObject.height; }
            set { _realObject.height = value; }
        }

        [DispId(5)]
        public string toDataURL(string type, params object[] args)
        {
            return _realObject.toDataURL(type, args);
        }

        [DispId(6)]
        public object getContext(string contextId)
        {
            return _realObject.getContext(contextId);
        }

        [DispId(7)]
        public void addEventListener(string type, Delegate listener, bool useCapture)
        {
            (_realObject).addEventListener(type, listener, useCapture);
        }

        [DispId(8)]
        public void setAttribute(string name, object value)
        {
            _realObject.setAttribute(name, value);
        }

        [DispId(9)]
        public ICSSStyleDeclaration style
        {
            get { return _realObject.style; }
        }

        #endregion

        #region Utils

        [DispId(10)]
        public global::SharpCanvas.Shared.IHTMLCanvasElement RealObject
        {
            get { return _realObject; }
        }

        /// <summary>
        /// Returns the number of pixels that the upper left corner of the current element is offset to the left within the offsetParent node.
        /// </summary>
        [DispId(11)]
        public int offsetLeft
        {
            get { return (_realObject as IWindow).Left; }
            set { (_realObject as IWindow).Left = value; }
        }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        [DispId(12)]
        public int offsetTop
        {
            get { return (_realObject as IWindow).Top; }
            set { (_realObject as IWindow).Top = value; }
        }

        public ICanvasRenderingContext2D getCanvas()
        {
            return _realObject.getCanvas();
        }

        public object PaintSite
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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