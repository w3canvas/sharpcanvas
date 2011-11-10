using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpCanvas.Host.Prototype
{
    /// <summary>
    /// At IE environment all expando members are generally Fields
    /// </summary>
    public class FieldHolder : FieldInfo, IProvideDefaultValue, ICloneable
    {
        private readonly ValueHolder _defaultValue = new ValueHolder();
        private readonly DispIdAttribute _dispidAttr;
        private readonly string _name;
// FIXME: Never used?
//        private object _value;

        //public event OnValueChangedHandler OnValueChanged;

        //public IValueChanged InitialMember
        //{
        //    get; set;
        //}

        public FieldHolder(string name)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(DispidGenerator.Take());
        }

        public FieldHolder(string name, int dispId)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(dispId);
        }

        private FieldHolder(string name, DispIdAttribute dispIdAttribute, ValueHolder value)
        {
            _name = name;
            _dispidAttr = dispIdAttribute;
            _defaultValue = value;
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

        public override FieldAttributes Attributes
        {
            // Only public scope supported in javascript
            get { return FieldAttributes.Public; }
        }

        public override Type FieldType
        {
            // Don't know type. Return dispatch
            get { return typeof (object); }
        }

        public override RuntimeFieldHandle FieldHandle
        {
            // This make no sense, but there's no alternative. This is reflection only anyway.
            get { return new RuntimeFieldHandle(); }
        }

        #region ICloneable Members

        public object Clone()
        {
            return new FieldHolder(_name, _dispidAttr, _defaultValue);
        }

        #endregion

        #region IProvideDefaultValue Members

        /// <summary>
        /// Create and return clone of the current FieldHolder and add link to it as initial object (clone object was made from)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        //public FieldHolder Clone(IValueChanged field)
        //{
        //    FieldHolder clone = new FieldHolder(this._name);
        //    clone.SetValue(clone, this._value);
        //    clone.InitialMember = field;
        //    return clone;
        //}
        public ValueHolder DefaultValue
        {
            get { return _defaultValue; }
        }

        #endregion

        /// <summary>
        /// Get value from the storage on the instance passed in
        /// </summary>
        /// <param name="obj">This, must not be null.</param>
        /// <returns>Field value</returns>
        public override object GetValue(object obj)
        {
            // If out this ptr does not derive from Prototype, things are rotten
            var instance = obj as ExpandoBase;
            if (instance == null)
            {
                throw new InvalidCastException("This does not implement prototype (at least not ME)");
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
        /// <param name="culture">Ignore, concept does not exist in COM</param>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder,
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
    }
}