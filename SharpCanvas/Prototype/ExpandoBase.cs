using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using CT = System.Runtime.InteropServices.ComTypes;

namespace SharpCanvas.Host.Prototype
{
    [ComVisible(true)]
    public abstract class ExpandoBase : IExpando, IExpandoProperty, INotifyPropertyChanged
    {
        #region Storage and Metadata contract

        /// <summary>
        /// Get backing storage for the member passed in as parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public abstract ValueHolder GetStorage<T>(T member) where T : MemberInfo;

        public abstract ValueHolder AddStorage<T>(T member) where T : MemberInfo;

        public abstract void GetMember<T>(string name, out T member) where T : MemberInfo;
        internal abstract void AddMember<T>(T member) where T : MemberInfo;
        internal abstract void DeleteMember(MemberInfo member);
        public abstract bool ExistsMember(string name);
        internal abstract IEnumerable<T> EnumerateMembers<T>() where T : MemberInfo;

        #endregion

        private readonly Queue<MemberInfo> _getValue = new Queue<MemberInfo>();

        /// <summary>
        /// In standalone environment access to the properties doesn't cause call of InvokeMember method.
        /// Therefore GetValue and SetValue methods should be belong not to the property itself, but for the object-container.
        /// We use thread-safe queue to process multiple calls of different properties.
        /// </summary>
        private readonly Queue<MemberInfo> _setValue = new Queue<MemberInfo>();

        private readonly object sync = new object();

        protected ExpandoBase _parent;

        #region IExpando Members

        public FieldInfo AddField(string name)
        {
            //////////
            // Adding a member with the same name is an error
            //////////
            //TODO: remove this line after testing - we shouldn't throw excpetion in case when member with the same name already exists, because of the spec
            //AssertMemberExistsNot(name);

            //////////
            // Add metadata for this field, if not already given (which would be the case
            // for intrinsics
            //////////
            var newField = new FieldHolder(name);

            AddMember(newField);

            //////////
            // Add storage for this field
            //////////
            AddStorage(newField);

            return newField;
        }

        public MethodInfo AddMethod(string name, Delegate method)
        {
            //////////
            // Adding a member with the same name is an error
            //////////
            AssertMemberExistsNot(name);

            //////////
            // Create a new method
            //////////
            var newMethod = new MethodHolder(name, method);

            AddMember(newMethod);

            return newMethod;
        }

        public PropertyInfo AddProperty(string name)
        {
            //Henk's comment
            //if (this.outerExpando != null)
            //{
            //    PropertyInfo result = this.outerExpando.AddProperty(name);
            //    return result;
            //}

            //////////
            // Adding a member with the same name is an error
            //////////
            //TODO: remove this line after testing - we shouldn't throw excpetion in case when member with the same name already exists, because of the spec
            //AssertMemberExistsNot(name);

            //////////
            // Create a new property
            //////////
            var newProperty = new PropertyHolder(name, this);

            AddMember(newProperty);

            //////////
            // Add storage for this property
            //////////
            AddStorage(newProperty);

            return newProperty;
        }

        public void RemoveMember(MemberInfo m)
        {
            //////////
            // Remove the member from metadata and storage
            //////////
            DeleteMember(m);
        }

        public FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            //////////
            // Get the field from metadata. Ignore the binding flags.
            //////////
            FieldInfo field = null;
            GetMember(name, out field);

            //////////
            // Return null if not found ..
            //////////
            return field;
        }

        public FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            //////////
            // Enumerate all fields from metadata
            //////////
            IEnumerable<FieldInfo> allFields = EnumerateMembers<FieldInfo>();

            //////////
            // Return as array
            //////////
            return EnumerableToArray(allFields);
        }

        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            //////////
            // Scripting doesn't do overloads, so name must be unique. Meaning:
            // we can have at most one member
            //////////
            var oneOrNoMembers = new List<MemberInfo>(1);

            //////////
            // Find the member
            //////////
            MemberInfo member = null;
            GetMember(name, out member);

            if (member != null)
            {
                oneOrNoMembers.Add(member);
            }

            //////////
            // Return as array
            //////////
            return oneOrNoMembers.ToArray();
        }

        public MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            //////////
            // Get all members regardless of type
            //////////
            IEnumerable<MemberInfo> allMembers = EnumerateMembers<MemberInfo>();

            //////////
            // Return as array
            //////////
            return EnumerableToArray(allMembers);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
        {
            //////////
            // Get the method from metadata. Ignore the binding flags.
            //////////
            MethodInfo method = null;
            GetMember(name, out method);

            //////////
            // Return null if not found ..
            //////////
            return method;
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types,
                                    ParameterModifier[] modifiers)
        {
            //////////
            // All extra attributes do not apply to script
            //////////
            return GetMethod(name, bindingAttr);
        }

        public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            //////////
            // Enumerate all fields from metadata
            //////////
            IEnumerable<MethodInfo> allMethods = EnumerateMembers<MethodInfo>();

            //////////
            // Return as array
            //////////
            return EnumerableToArray(allMethods);
        }

        public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            //////////
            // Enumerate all fields from metadata
            //////////
            IEnumerable<PropertyInfo> allProperties = EnumerateMembers<PropertyInfo>();

            //////////
            // Return as array
            //////////
            return EnumerableToArray(allProperties);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType,
                                        Type[] types, ParameterModifier[] modifiers)
        {
            return GetProperty(name, bindingAttr);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            //////////
            // Get the field from metadata. Ignore the binding flags.
            //////////
            PropertyInfo property = null;
            GetMember(name, out property);

            //////////
            // Return null if not found ..
            //////////
            return property;
        }

        public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
                                   ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            // Member to invoke (can be either a property, field or method, so typed as base class)
            MemberInfo memberToInvoke = null;

            GetMember(name, out memberToInvoke);
            if (memberToInvoke == null)
            {
                throw new ApplicationException(string.Format("{0} not found", name));
            }

            // For invocation we need to know the concrete type
            BindingFlags invokeFlags = BindingFlags.GetField |
                                       BindingFlags.SetField |
                                       BindingFlags.GetProperty |
                                       BindingFlags.SetProperty |
                                       BindingFlags.PutDispProperty |
                                       BindingFlags.InvokeMethod;
            switch (invokeAttr & invokeFlags)
            {
                case BindingFlags.GetField:
                case BindingFlags.GetProperty:

                    //////////
                    // NOTE: there are cases where expando items are added as properties, but invoked
                    // as properties. This is something we cannot fix (the code that triggers this is
                    // beyond our reach) so we have to correct for it
                    //////////

                    //////////
                    // Find out the real type of member
                    //////////
                    if (memberToInvoke is PropertyInfo)
                    {
                        var propInfoGet = (PropertyInfo) memberToInvoke;
                        return propInfoGet.GetValue(target, null);
                    }

                    if (memberToInvoke is FieldInfo)
                    {
                        var fieldInfoGet = (FieldInfo) memberToInvoke;
                        return fieldInfoGet.GetValue(target);
                    }

                    break;

                case BindingFlags.SetProperty:
                case BindingFlags.PutDispProperty:
                case BindingFlags.SetField:

                    //////////
                    // NOTE: there are cases where expando items are added as fields, but invoked
                    // as properties. This is something we cannot fix (the code that triggers this is
                    // beyond our reach) so we have to correct for it. The thing is that javascript
                    // doesn't make a difference between fields and props and the marshaling layer
                    // (ExpandoToDispatchExMarshaler) in .NET seems to mix things up a bit.
                    //////////

                    //////////
                    // Find out the real type of member, if property info, invoke as property ..
                    //////////
                    if (memberToInvoke is PropertyInfo)
                    {
                        var propInfoSet = (PropertyInfo) memberToInvoke;
                        propInfoSet.SetValue(target, args[0], null);

                        FirePropertyChanged(name);

                        break;
                    }

                    //////////
                    // .. else invoke as field
                    //////////
                    if (memberToInvoke is FieldInfo)
                    {
                        var fieldInfoSet = (FieldInfo) memberToInvoke;
                        fieldInfoSet.SetValue(target, args[0]);

                        FirePropertyChanged(name);

                        break;
                    }

                    //////////
                    // It would be somewhat strange to end up here ...
                    //////////
                    throw new ApplicationException( // 0x80131600 equivalent HRESULT
                        string.Format("Unknown peroperty type: {0}", memberToInvoke.GetType().FullName));


                case BindingFlags.InvokeMethod:
                case BindingFlags.InvokeMethod | BindingFlags.GetProperty:

                    //////////
                    // NOTE: method invocations may come in from javascript as a combination of get property
                    // and invoke
                    //////////

                    //////////
                    // First we get the actual method to be executed: when a method call comes in on a method
                    // that was added on an expando, the member will not be stored as a method, but rather as
                    // a property. The property will point to a JScriptTypeInfo object, which within IE acts
                    // as the target for method who's target is not one of the intrinsic objects.
                    //////////
                    // This means we cannot rely on the fact that the member to invoke is a method info. It
                    // might just as well be a property of field, in which case we first need to get the value
                    // which would then typically be a COM object (JScriptTypeInfo).
                    //////////
                    object methodToInvoke = memberToInvoke;

                    //////////
                    // If the member to invoke is not a method info, get the prop/field value which will point
                    // to a method
                    //////////
                    if (!(memberToInvoke is MethodInfo))
                    {
                        //////////
                        // Handle a method added as expando property
                        //////////

                        //////////
                        // Get the field/property value
                        //////////
                        if (memberToInvoke is PropertyInfo)
                        {
                            methodToInvoke = ((PropertyInfo) memberToInvoke).GetValue(target, new object[0]);
                        }
                        else
                        {
                            methodToInvoke = ((FieldInfo) memberToInvoke).GetValue(target);
                        }
                    }

                    //////////
                    // Now we have a method to invoke, check how we should call
                    //////////
                    if (methodToInvoke is MethodInfo)
                    {
                        //////////
                        // Regular method
                        //////////
                        var methodInfo = (MethodInfo) memberToInvoke;
                        return methodInfo.Invoke(target, args);
                    }

                    if (methodToInvoke.GetType().IsCOMObject)
                    {
                        //////////
                        // Method added from script
                        //////////

                        //////////
                        // Function objects implement IDispatch, which we can use to dynamically invoke
                        //////////
                        var functionDispatch = (IReflect) methodToInvoke; // IReflect is managed IDispatch

                        //////////
                        // Create a parameter list, we need to manually add the this pointer
                        //////////
                        var parms = new List<object>(args);

                        //////////
                        // Inject this ptr as first parameter
                        //////////
                        parms.Insert(0, this);

                        //////////
                        // Invoke, the method name is 'call' for function objects from script
                        //////////
                        return functionDispatch.InvokeMember(
                            "call", invokeAttr, binder, methodToInvoke, parms.ToArray(), modifiers, culture,
                            namedParameters);
                    }

                    break;
            }

            return null;
        }

        public Type UnderlyingSystemType
        {
            get { return GetType(); }
        }

        #endregion

        #region IExpandoProperty Members

        /// <summary>
        /// Set value for the property at the bottom of the queue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual MemberInfo SetValue(object value)
        {
            lock (sync)
            {
                MemberInfo member = _setValue.Dequeue();
                if (member is PropertyInfo)
                {
                    ((PropertyInfo) member).SetValue(this, value, BindingFlags.Instance, null, null, null);
                }
                return member;
            }
        }

        /// <summary>
        /// Get value for the property at the bottom of the queue
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            lock (sync)
            {
                MemberInfo member = _getValue.Dequeue();
                if (member is PropertyInfo)
                {
                    return ((PropertyInfo) member).GetValue(this, BindingFlags.Instance, null, null, null);
                }
                return null;
            }
        }

        public MethodInfo GetGetMethod(MemberInfo member)
        {
            MethodInfo method = GetType().GetMethod("GetValue");
            //add request to get property's value to the GET queue
            _getValue.Enqueue(member);
            return method;
            //return ((PropertyHolder) member).GetGetMethod();
        }


        public MethodInfo GetSetMethod(MemberInfo member)
        {
            MethodInfo method = GetType().GetMethod("SetValue", new[]
                                                                    {
                                                                        typeof (object)
                                                                    });
            //add request to set property's value to the SET queue
            _setValue.Enqueue(member);
            return method;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //TODO:Do we really use this method???
        protected int GetDispid(string name)
        {
            // Filter out the dispid
            int idxStartDispid = name.IndexOf('=') + 1;
            int dispidLength = name.IndexOf(']') - idxStartDispid;

            int dispid;
            if (!int.TryParse(name.Substring(idxStartDispid, dispidLength), out dispid))
            {
                throw new FormatException(string.Format("Dispid in unknown format: {0}", name));
            }
            return dispid;
        }

        private void AssertMemberExists(string name)
        {
            //////////
            // A member with the given name must exist
            //////////
            if (!ExistsMember(name))
            {
                throw new ApplicationException(string.Format("{0}: member not found", name));
            }
        }

        private void AssertMemberExistsNot(string name)
        {
            //////////
            // A member with the given name must not exist yet
            //////////
            if (ExistsMember(name))
            {
                throw new ApplicationException(string.Format("{0}: a member with this name already exists", name));
            }
        }

        private T[] EnumerableToArray<T>(IEnumerable<T> enumerable)
        {
            //////////
            // Cannot assume linq
            //////////
            var list = new List<T>(enumerable);
            return list.ToArray();
        }

        private void AddMembersWithDispid(Type typeToCheck, Type reflectedType)
        {
            MemberInfo[] members =
                typeToCheck.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);

            InterfaceMapping map = default(InterfaceMapping);
            if (typeToCheck.IsInterface)
            {
                map = reflectedType.GetInterfaceMap(typeToCheck);
            }

            foreach (MemberInfo member in members)
            {
                MemberInfo metadataTocheck = member;

                if (metadataTocheck is MethodInfo && typeToCheck.IsInterface)
                {
                    for (int i = 0; i < map.TargetMethods.Length; i++)
                    {
                        if (map.InterfaceMethods[i].Equals(metadataTocheck))
                        {
                            metadataTocheck = map.TargetMethods[i];
                            break;
                        }
                    }
                }

                //////////
                // If this is a method declared on an interface, 
                object[] dispidAttrs = metadataTocheck.GetCustomAttributes(typeof (DispIdAttribute), true);
                if (dispidAttrs != null && dispidAttrs.Length > 0 && !ExistsMember(member.Name))
                {
                    // Add this member to metadata
                    AddMember(member);
                }
            }
        }

        protected void AddIntrinsicMembers()
        {
            Type reflectedType = GetType();

            //////////
            // Run through all members that may have a dispid attribute on
            //////////
            AddMembersWithDispid(reflectedType, null);

            ////////////
            // Run through all interface implementations
            //////////
            foreach (Type interfaceType in reflectedType.GetInterfaces())
            {
                AddMembersWithDispid(interfaceType, reflectedType);
            }
        }

        private void FirePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}