using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpCanvas.Host.Prototype
{
    /// <summary>
    /// Object with expando members support as well as prototype property
    /// </summary>
    [ComVisible(true)]
    public class ObjectWithPrototype : ExpandoBase
    {
        private readonly Dictionary<string, MemberInfo> _metadata = new Dictionary<string, MemberInfo>();
        private readonly Dictionary<string, ValueHolder> _storage = new Dictionary<string, ValueHolder>();
        internal Prototype _prototype;

        public ObjectWithPrototype(Guid scope)
        {
            _prototype = new Prototype(scope);
        }

        [DispId(1)]
        public object prototype
        {
            get { return _prototype; }
            set { _prototype = (Prototype) value; }
        }

        public bool MemberExists(string name)
        {
            return ExistsMember(name);
        }

        internal override void AddMember<T>(T member)
        {
            if (!_metadata.ContainsKey(member.Name))
            {
                _metadata.Add(member.Name, member);
            }
            //base.AddMember(member);
        }

        public override bool ExistsMember(string name)
        {
            if (!_metadata.ContainsKey(name))
            {
                return _prototype.ExistsMember(name);
            }

            return true;
        }

        internal override void DeleteMember(MemberInfo member)
        {
            if (!ExistsMember(member.Name))
            {
                throw new InvalidOperationException(string.Format("{0} cannot be deleted, it doesn't exist", member.Name));
            }

            if (!_metadata.ContainsKey(member.Name))
            {
                _prototype.DeleteMember(member);
            }
            else
            {
                _metadata.Remove(member.Name);
            }

            if (member is FieldHolder || member is PropertyHolder)
            {
                if (_storage.ContainsKey(member.Name))
                {
                    _storage.Remove(member.Name);
                }
            }
        }

        internal override IEnumerable<T> EnumerateMembers<T>()
        {
            foreach (MemberInfo m in _metadata.Values)
            {
                if (m is T)
                {
                    yield return (T) m;
                }
            }

            foreach (MemberInfo m in _prototype.EnumerateMembers<T>())
            {
                if (m is T && !_metadata.ContainsKey(m.Name))
                {
                    yield return (T) m;
                }
            }

            yield break;
        }

        public override void GetMember<T>(string name, out T member)
        {
            member = null;
            //search member in metadata
            if (_metadata.ContainsKey(name))
            {
                MemberInfo memberFound = _metadata[name];

                if (!typeof (T).IsAssignableFrom(memberFound.GetType()))
                {
                    throw new ArgumentException(string.Format("{0} exists, but is not a {1}", name,
                                                              member.GetType().Name));
                }

                member = (T) memberFound;
            }
            else
            {
                _prototype.GetMember(name, out member);
                if (member != null && member is PropertyHolder && member is ICloneable)
                {
                    member = ((ICloneable) member).Clone() as T;
                    //the member info was found in prototype, but search was initiated in instance, so we need 
                    //to "replace" member's parent for this instance. This means that from now and till end of the member processing
                    //the member will belong to the current instance
                    (member as PropertyHolder).parent = this;
                }
            }
        }

        public override ValueHolder AddStorage<T>(T member)
        {
            MemberInfo m = member;

            if (_storage.ContainsKey(m.Name))
            {
                throw new ArgumentException(string.Format("{0} already has storage", m.Name));
            }

            object defaultValue = null;
            if (member is IProvideDefaultValue)
            {
                defaultValue = ((IProvideDefaultValue) member).DefaultValue;
            }

            var vh = new ValueHolder(defaultValue);

            _storage.Add(m.Name, vh);

            return vh;
        }

        public override ValueHolder GetStorage<T>(T member)
        {
            MemberInfo m = member;

            if (!_storage.ContainsKey(m.Name))
            {
                return null;
                //this._storage.Add(m.Name, this._prototype.GetStorage(member));
            }

            return _storage[m.Name];
        }
    }
}