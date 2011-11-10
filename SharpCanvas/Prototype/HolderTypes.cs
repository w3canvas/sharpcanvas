using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace SharpCanvas.Host.Prototype
{

    #region DISPID generation

    public static class DispidGenerator
    {
        private const int MaxDispidExpando = 0x3FFFFFF;
        private const int MinDispidExpando = 0x3000000;

        private static int nextDispid = MinDispidExpando;

        public static int Take()
        {
            int next = Interlocked.Increment(ref nextDispid);

            if (next > MaxDispidExpando)
            {
                throw new InvalidOperationException("Maximum number of dynamic properties exceeded");
            }

            return next;
        }
    }

    #endregion

    #region Holder Types

    public class MethodHolder : MethodInfo
    {
        private readonly DispIdAttribute _dispidAttr;
        private readonly string _name;
        private Delegate _target;

        public MethodHolder(string name)
        {
            _name = name;
            _dispidAttr = new DispIdAttribute(DispidGenerator.Take());
        }

        public MethodHolder(string name, Delegate target)
        {
            _name = name;
            _target = target;
            _dispidAttr = new DispIdAttribute(DispidGenerator.Take());
        }

        internal MethodHolder(string name, Delegate target, int dispId)
        {
            _name = name;
            _target = target;
            _dispidAttr = new DispIdAttribute(dispId);
        }

        public Delegate Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public override Type ReflectedType
        {
            // Can't do anything more concrete than this
            get { return GetType(); }
        }

        public override Type DeclaringType
        {
            // Can't do anything more concrete than this
            get { return GetType(); }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override MethodAttributes Attributes
        {
            get
            {
                // Javascript only does public
                return MethodAttributes.Public;
            }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            // Is only ever needed for reflection
            get { return new RuntimeMethodHandle(); }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            // No custom attributes on code defined in javascript
            get { return null; }
        }

        /// <summary>
        /// Invoke using the target set on creation and the parameters passed in
        /// </summary>
        /// <param name="obj">This, must not be null.</param>
        /// <param name="invokeAttr">Flags that determine overload strategy etc. Can safely ignore here</param>
        /// <param name="binder">Ignore. For advanced method routing.</param>
        /// <param name="parameters">Parameters passed ot the method. This is NOT the first parameter ...</param>
        /// <param name="culture">Ignore, concept does not exist in COM</param>
        /// <returns></returns>
        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters,
                                      CultureInfo culture)
        {
            // Sanity check
            if (_target == null)
            {
                throw new InvalidOperationException("No target defined");
            }

            // Make sure we ignore the target of the delegate. That could very well be the prototype,
            // should always invoke on the this ptr passed in.
            return _target.Method.Invoke(obj, parameters);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            // Javascipt does not do attributes
            return false;
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            // if asked for dispid attribute, return it
            if (attributeType == typeof (DispIdAttribute))
            {
                return new object[] {_dispidAttr};
            }

            // Javascript itself does not do attributes
            return null;
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            // We use a dispid attribute to map name to dispid
            return new object[] {_dispidAttr};
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            // Implemented by script
            return MethodImplAttributes.Unmanaged;
        }

        public override ParameterInfo[] GetParameters()
        {
            // There's may be parameters, but we don't know about them
            return null;
        }

        public override MethodInfo GetBaseDefinition()
        {
            // No inheritance support
            return null;
        }
    }

    public class ValueHolder : IProvideDefaultValue
    {
        private object _value;

        internal ValueHolder()
        {
        }

        internal ValueHolder(object value)
        {
            _value = value;
        }

        internal virtual object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #region IProvideDefaultValue Members

        public ValueHolder DefaultValue
        {
            get { return null; }
        }

        #endregion
    }

    public interface IProvideDefaultValue
    {
        ValueHolder DefaultValue { get; }
    }

    #endregion
}