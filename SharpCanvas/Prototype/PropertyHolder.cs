using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas.Interop;

namespace SharpCanvas.Host.Prototype
{
    /// <summary>
    /// All expando members are generally Properties, in standalone environment
    /// </summary>
    public class PropertyHolder : PropertyInfo, IProvideDefaultValue, ICloneable
    {
        private readonly ValueHolder _defaultValue = new ValueHolder();
        private readonly DispIdAttribute _dispidAttr;
        private readonly string _name;
        //private object _value;
        private IExpandoProperty _parent;

        public PropertyHolder(string name, IExpandoProperty parent)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(DispidGenerator.Take());
            _parent = parent;
        }

        public PropertyHolder(string name)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(DispidGenerator.Take());
        }

        public PropertyHolder(string name, int dispId)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(dispId);
        }

        private PropertyHolder(string name, IExpandoProperty parent, DispIdAttribute dispIdAttribute, ValueHolder value)
        {
            _name = name;
            _dispidAttr = dispIdAttribute;
            _parent = parent;
            _defaultValue = value;
        }

        public IExpandoProperty parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override Type ReflectedType
        {
            // Can't do anything more concrete than this
            get { return typeof (ExpandoBase); }
        }

        public override Type DeclaringType
        {
            // Can't do anything more concrete than this
            get { return typeof (ExpandoBase); }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override PropertyAttributes Attributes
        {
            get { return PropertyAttributes.None; }
        }

        public override Type PropertyType
        {
            // Dispatch is a safe bet
            get { return typeof (object); }
        }

        #region ICloneable Members

        public object Clone()
        {
            return new PropertyHolder(_name, _parent, _dispidAttr, _defaultValue);
        }

        #endregion

        #region IProvideDefaultValue Members

        /// <summary>
        /// Create and return clone of the current PropertyHolder and add link to it as initial object (clone was made from)
        /// </summary>
        /// <returns></returns>
        //public PropertyHolder Clone(IExpandoProperty parent, IValueChanged initialProperty)
        //{
        //    PropertyHolder clone = new PropertyHolder(this._name, parent);
        //    clone.SetValue(clone, this._value, BindingFlags.Public, null, null, null);
        //    clone.InitialMember = initialProperty;
        //    return clone;
        //}
        public ValueHolder DefaultValue
        {
            get { return _defaultValue; }
        }

        #endregion

        /// <summary>
        /// Get value from the current instance of the property
        /// </summary>
        /// <param name="obj">This, must not be null.</param>
        /// <param name="invokeAttr">Flags that determine overload strategy etc. Can safely ignore here</param>
        /// <param name="binder">Ignore. For advanced method routing.</param>
        /// <param name="index">Indexes for indexed prop. This does not exist in script (the property type itself would be array).</param>
        /// <param name="culture">Ignore, concept does not exist in COM</param>
        /// <returns>Property value</returns>
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index,
                                        CultureInfo culture)
        {
            // If our this ptr does not derive from Prototype, things are rotten
            var instance = obj as ExpandoBase;
            if (instance == null)
            {
                throw new InvalidCastException("This does not implement prototype (at least not as we know it)");
            }

            // Get backing storage from the instance
            ValueHolder backingStore = instance.GetStorage(this);

            // If no storage found, check if this is an object with prototype
            if (backingStore == null && instance is ObjectWithPrototype)
            {
                backingStore = ((ObjectWithPrototype) instance)._prototype.GetStorage(this);
            }

            if (backingStore == null)
            {
                return null;
            }

            // Return the value found
            return backingStore.Value;
        }

        /// <summary>
        /// Set value on storage on the instance passed in
        /// </summary>
        /// <param name="obj">This, must not be null.</param>
        /// <param name="invokeAttr">Flags that determine overload strategy etc. Can safely ignore here</param>
        /// <param name="binder">Ignore. For advanced method routing.</param>
        /// <param name="index">Indexes for indexed prop. This does not exist in script (the property type itself would be array).</param>
        /// <param name="culture">Ignore, concept does not exist in COM</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index,
                                      CultureInfo culture)
        {
            // If out this ptr does not derive from Prototype, things are rotten
            var instance = obj as ExpandoBase;
            if (instance == null)
            {
                throw new InvalidCastException("This does not implement prototype (at least not ME)");
            }

            // Get backing storage from the instance
            ValueHolder backingStore = instance.GetStorage(this);

            // If no storage exists, create on the fly
            if (backingStore == null)
            {
                lock (instance)
                {
                    backingStore = instance.GetStorage(this);
                    if (backingStore == null)
                    {
                        backingStore = instance.AddStorage(this);
                    }
                }
            }

            if (backingStore == null)
            {
                throw new OutOfMemoryException("No storage created");
            }

            Type vt = value.GetType();
            if (vt.IsCOMObject)
            {
                var pDispatch = value as IDispatch;
                if (pDispatch != null)
                {
                    IntPtr pTI;
                    pDispatch.GetTypeInfo(0, 0, out pTI);

                    Type t = Marshal.GetTypeForITypeInfo(pTI);
                }
            }

            // Set new value
            backingStore.Value = value;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            // Javascipt does not do attributes
            return false;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            // Javascript does not do attributes
            return null;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            // Javascript does not do attributes
            return null;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            // May have indexes, but there's no way to set metadata about them
            return new ParameterInfo[0];
        }

        //public override MethodInfo GetSetMethod(bool nonPublic)
        //{
        //    // Bit of a kludge, but COM clients can actually call this, so need to return something that will work
        //    return this.GetType().GetMethod("SetValue");
        //}

        //public override MethodInfo GetGetMethod(bool nonPublic)
        //{
        //    // Bit of a kludge, but COM clients can actually call this, so need to return something that will work
        //    return this.GetType().GetMethod("GetValue");
        //}

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return new[] {GetGetMethod(nonPublic), GetSetMethod(nonPublic)};
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return (_parent).GetSetMethod(this);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return (_parent).GetGetMethod(this);
        }
    }
}