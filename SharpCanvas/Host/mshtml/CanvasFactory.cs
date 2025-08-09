using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using SharpCanvas.Interop;

[assembly: AllowPartiallyTrustedCallers]

namespace SharpCanvas.Host
{
    [Guid("06351772-f965-4b5e-8dd2-0c2cf8da6c6b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CanvasFactory :
        IElementNamespaceFactory,
        IElementBehaviorFactory,
        //global::SharpCanvas.Interop.IObjectSafety,
        IElementBehavior
    {
        //void IECanvasHost.global::Interop.IObjectSafety.GetInterfaceSafetyOptions(ref Guid riid, out uint pdwSupportedOptions, out uint pdwEnabledOptions)
        //{
        //    throw new NotImplementedException();
        //}

        //void IECanvasHost.global::SharpCanvas.Interop.IObjectSafety.SetInterfaceSafetyOptions(ref Guid riid, uint dwOptionSetMask, uint dwEnabledOptions)
        //{
        //    throw new NotImplementedException();
        //}

        #region IElementBehavior Members

        void IElementBehavior.Init(IElementBehaviorSite pBehaviorSite)
        {
            throw new NotImplementedException();
        }

        void IElementBehavior.Notify(int lEvent, IntPtr pVar)
        {
            throw new NotImplementedException();
        }

        void IElementBehavior.Detach()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IElementBehaviorFactory Members

        IElementBehavior IElementBehaviorFactory.FindBehavior(string bstrBehavior, string bstrBehaviorUrl,
                                                              IElementBehaviorSite pSite)
        {
            // Find behavior will be called several times during the object initialization, but
            // we have to create new proxy-object Canvas only in case when bstrBehavior is null
            if (bstrBehavior == null)
            {
                Guid scopeForCanvas = GetScope(pSite.GetElement().document);
                //return new Canvas(scopeForCanvas);
                return new mshtml.CanvasProxy();
            }
            return null;
        }

        #endregion

        #region Support for Scope detection

        public static Guid GetScope(object probe)
        {
            IBindCtx bindCtx = null;
            CreateBindCtx(0, out bindCtx);

            // Create objref moniker based on probe
            IMoniker pMk = null;
            if (0 == CreateObjrefMoniker(probe, out pMk))
            {
                // Get the monikers display name (objref:<base64 encode meow packet>)
                string scopeId = null;
                pMk.GetDisplayName(bindCtx, null, out scopeId);

                // Extract the base64 encoded meow packet
                string base64 = scopeId.Split(':')[1];

                // Turn the base64 encoded version into a raw meow packet
                byte[] rawObjRef = Convert.FromBase64String(base64);

                // OXID and OID are adjecent in the meow packet and start at index 32
                // See DCOM spec for details
                var oxidAndOid = new byte[16];
                Array.Copy(rawObjRef, 32, oxidAndOid, 0, 16);

                // Turn the 16 bytes containing OXID and OID into a guid for easy handling
                return new Guid(oxidAndOid);
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Creates a moniker which contains a MEOW packet (the marshaled form of a COM object)
        /// </summary>
        /// <remarks>
        /// MEOW packets contains enough information to uniquely identify both the apartment
        /// and the object. We use this information to turn the site that hosts the canvas
        /// into a unique scope id. We need this to prevent sharing of prototype state across
        /// different sites (e.g. one canvas hosted in a document and an other in a frame).
        /// </remarks>
        /// <param name="pUnk">Probe for the scope id.</param>
        /// <param name="ppMk">Moniker containing marshal packet</param>
        /// <returns>HRESULT</returns>
        [DllImport("ole32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern uint CreateObjrefMoniker(
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            out IMoniker ppMk);

        /// <summary>
        /// Creates a context that is needed to handle (e.g. combine) monikers
        /// </summary>
        /// <param name="reserved"></param>
        /// <param name="bindCtx">Context created</param>
        /// <returns>HRESULT</returns>
        [DllImport("ole32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern uint CreateBindCtx(
            uint reserved,
            out IBindCtx bindCtx);

        [DllImport("mshtml.dll", CallingConvention = CallingConvention.Winapi, PreserveSig = true, SetLastError = true,
            CharSet = CharSet.Unicode)]
        public static extern uint IERegisterXMLNS(string uri, Guid clsid, bool machine);

        #endregion

        #region IElementNamespaceFactory Members

        void IElementNamespaceFactory.Create(IElementNamespace pNamespace)
        {
            pNamespace.AddTag("canvas", 0);
        }

        #endregion

        [DllImport("urlmon.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        private static extern uint CoInternetIsFeatureEnabled(uint feature, uint flags);
    }
}