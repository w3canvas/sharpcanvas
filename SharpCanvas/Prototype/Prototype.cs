using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpCanvas.Host.Prototype
{
    [ComVisible(true)]
    public class Prototype : ExpandoBase
    {
        /// <summary>
        /// Static metadata, kept per scope
        /// </summary>
        private static readonly Dictionary<Guid, Dictionary<string, MemberInfo>> s_metadataPerScope
            = new Dictionary<Guid, Dictionary<string, MemberInfo>>();

        private Prototype _prototype;

        /// <summary>
        /// The scope this instance is in. This determines which metadata will be used.
        /// The scope id is a combination of the apartment (technically the OXID) the
        /// IElementBehaviorSite that is hosting us is in and the object id (OID)of the
        /// object implementing IElementBehaviorSite (e.g. document or frame).
        /// </summary>
        private Guid _scopeId = Guid.Empty;

        internal Prototype(Guid scope)
        {
            _scopeId = scope;

            if (GetType() != typeof (Prototype))
            {
                _prototype = new Prototype(scope);
            }
            else
            {
                Initialize(scope);
            }

            //////////
            // Add all members with dispids from the concrete type
            //////////
            base.AddIntrinsicMembers();
        }

        protected Dictionary<string, MemberInfo> MetadataForCurrentScope
        {
            get { return GetMetadataStorageForCurrentScope(); }
        }

        /// <summary>
        /// Because the actual scope is not known at instance creation time, we need to
        /// lazily initialize the prototype
        /// </summary>
        /// <param name="scopeId">The scope id of the scope this prototype is in</param>
        protected void Initialize(Guid scopeId)
        {
            //////////
            // Remember the scope id
            //////////
            _scopeId = scopeId;

            //////////
            // Create storage for the metadata of this scope
            //////////

            //////////
            // Lock on metadata storage. This is on static scope, so we need to get
            // rid of the lock asap
            //////////
            lock (s_metadataPerScope)
            {
                if (!s_metadataPerScope.ContainsKey(_scopeId))
                {
                    s_metadataPerScope.Add(_scopeId, new Dictionary<string, MemberInfo>());
                }
            }
        }

        public override ValueHolder AddStorage<T>(T member)
        {
            //////////
            // For prototype the storage is always one-on-one with the member itself.
            // So, we can just use the storage for the default value on the member.
            //////////
            return GetStorage(member);
        }

        public override ValueHolder GetStorage<T>(T member)
        {
            //////////
            // For prototype the storage is always one-on-one with the member itself.
            // So, we can just use the storage for the default value on the member.
            //////////

            //////////
            // Get the member from prototype scope
            //////////
            if (member is IProvideDefaultValue)
            {
                return ((IProvideDefaultValue) member).DefaultValue;
            }

            throw new InvalidOperationException("Unsupported member type");
        }

        internal override void AddMember<T>(T member)
        {
            //////////
            // Get the metadata for the current scope
            //////////
            Dictionary<string, MemberInfo> metadata = MetadataForCurrentScope;

            //////////
            // We can now lock per scope
            //////////
            lock (metadata)
            {
                //////////
                // Check if the member exists, but only on prototype level
                //////////
                if (ExistsMemberInternal(member.Name))
                {
                    throw new ArgumentException(string.Format("{0}: member already exsists", member.Name));
                }

                //////////
                // Add the member to metadata storage
                //////////
                metadata.Add(member.Name, member);
            }
        }

        public override void GetMember<T>(string name, out T member)
        {
            member = null;

            //////////
            // Get the metadata for the current scope
            //////////
            Dictionary<string, MemberInfo> metadata = MetadataForCurrentScope;

            //////////
            // We can now lock per scope
            //////////
            lock (metadata)
            {
                //////////
                // If the member exists, get it
                //////////
                if (ExistsMemberInternal(name))
                {
                    MemberInfo memberFound = metadata[name];

                    //////////
                    // Only if it is of the expected type, assign to out parameter
                    //////////
                    if (memberFound is T)
                    {
                        member = (T) memberFound;
                    }
                }
            }
        }

        public override bool ExistsMember(string name)
        {
            //////////
            // Check if the member exists on the prototype
            //////////
            return ExistsMemberInternal(name);
        }

        internal override void DeleteMember(MemberInfo member)
        {
            //////////
            // Get the metadata for the current scope
            //////////
            Dictionary<string, MemberInfo> metadata = MetadataForCurrentScope;

            //////////
            // Lock for the duration of delete
            //////////
            lock (metadata)
            {
                //////////
                // Delete the member if it exists
                //////////
                if (ExistsMemberInternal(member.Name))
                {
                    metadata.Remove(member.Name);
                }
            }
        }

        internal override IEnumerable<T> EnumerateMembers<T>()
        {
            //////////
            // Get the metadata for the current scope
            //////////
            Dictionary<string, MemberInfo> metadata = MetadataForCurrentScope;

            //////////
            // Iterate
            //////////
            lock (metadata)
            {
                foreach (MemberInfo member in metadata.Values)
                {
                    if (member is T)
                    {
                        yield return (T) member;
                    }
                }
            }

            yield break;
        }

        protected IEnumerable<T> BaseEnumerateMembers<T>() where T : MemberInfo
        {
            return EnumerateMembers<T>();
        }

        private Dictionary<string, MemberInfo> GetMetadataStorageForCurrentScope()
        {
            //////////
            // Static data, need to lock for the duration of the lookup
            //////////
            Dictionary<string, MemberInfo> metadataForCurrentScope = null;
            lock (s_metadataPerScope)
            {
                if (!s_metadataPerScope.TryGetValue(_scopeId, out metadataForCurrentScope))
                {
                    throw new OutOfMemoryException(string.Format("{0}; No metadata storage exists for this scope",
                                                                 _scopeId));
                }
            }

            return metadataForCurrentScope;
        }

        private bool ExistsMemberInternal(string name)
        {
            //////////
            // Lock metadata for the duration of the lookup
            //////////
            Dictionary<string, MemberInfo> metadataForCurrentScope
                = GetMetadataStorageForCurrentScope();
            lock (metadataForCurrentScope)
            {
                return metadataForCurrentScope.ContainsKey(name);
            }
        }
    }
}