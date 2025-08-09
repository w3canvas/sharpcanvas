using System;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host
{
    public class Standalone : IStandaloneFactory
    {
        internal static readonly Guid CLSID_SilverlightCanvas = new Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5");

        /// <summary>
        /// Class ids (COM) for the diverse canvas implementations we know about. The bootstrapper will
        /// figure out on load time, what the best canvas implementation would be for the current environment.
        /// Since some implementations (WinForms) are more COM fiendly that others (WPF), it may be necessary
        /// to inject a wrapper to handle activex hosting. Also, although silverlight manifests itself as
        /// a COM component, we will need to point it to the actual .NET assembly to load, meaning extra
        /// initialization may be needed. This is handled by passing the html object element to the
        /// bootstrapper rather than just asking the bootstrapper for the proper class id.
        /// </summary>
        internal static readonly Guid CLSID_WinFormsCanvas = new Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5");

        internal static readonly Guid CLSID_WPFCanvas = new Guid("35a22abc-5a67-4723-8da4-c1b00e0853f4");

        private static Guid _clsidForContext = Guid.Empty;

        private static Type _currentTypeFactory = null;

        /// <summary>
        /// Figure out the best canvas implementation to use. This will run at load time.
        /// </summary>
        static Standalone()
        {
            //////////
            // Check the runtime version, just to be on the safe side
            //////////
            if (Environment.Version.Major == 2)
            {
                //////////
                // 2.0, 3.0, 3.5 or 3.5SP1 all have runtime version 2. we need to check for silverlight
                // and for WPF (which is present starting from 3.0)
                /////////

                //////////
                // Use the silverlight clsid if installed
                //////////
                //if (CheckSilverlightInstalled())
                //{
                //    Bootstrapper._clsidForContext = Bootstrapper.CLSID_SilverlightCanvas;
                //    return;
                //}

                //////////
                // Try to load any (the preferred) version of a core WPF assembly. If this succeeds, there
                // is no overload; we need it loaded anyway. If it fails, we don't have WPF installed.
                //////////
                //TODO: remove false from condition below. JUST FOR TESTING PURPOSES
                if (Assembly.LoadWithPartialName("WindowsBase") != null && false)
                {
                    // WPF installed, use that
                    _clsidForContext = CLSID_WPFCanvas;
                    return;
                }

                // WPF need 3 or higher, so use WinForms
                _clsidForContext = CLSID_WinFormsCanvas;
            }
            else
            {
                //////////
                // 4.0 and later, we know WPF is installed. Only need to check for silverlight.
                //////////

                //////////
                // Use the silverlight clsid if installed
                //////////
                if (CheckSilverlightInstalled())
                {
                    _clsidForContext = CLSID_SilverlightCanvas;
                    return;
                }

                //////////
                // WPF is a given
                //////////
                _clsidForContext = CLSID_WPFCanvas;
            }

            //_clsidForContext = CLSID_WinFormsCanvas;
            AssemblyLocator.Init();            
        }

        /// <summary>
        /// Checks wherever silverlight installed on the target machine
        /// </summary>
        /// <returns></returns>
        private static bool CheckSilverlightInstalled()
        {
            Guid clsidSilverlight;
            return CLSIDFromProgID("AgControl.AgControl.3.0", out clsidSilverlight) == 0;
        }

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        private static extern int CLSIDFromProgID(string progId, out Guid clsid);


        /// <summary>
        /// This function helps to support MSHTML.
        /// </summary>
        /// <param name="element"></param>
        internal static void Initialize(IHTMLObjectElement element)
        {
            // _clsidForHost = "SharpCanvas.Host.mshtml";
            string clsidString = string.Format("CLSID:{0}", _clsidForContext);
            ((IHTMLElement) element).setAttribute("classid", clsidString, 0);
        }

        /// <summary>
        /// Return namespace depending on the current .NET Framework version
        /// </summary>
        /// <returns></returns>
        private static string GetNamespace()
        {
            if (_clsidForContext == CLSID_WinFormsCanvas)
            {
                return "SharpCanvas.Browser.Forms";
            }
            else if (_clsidForContext == CLSID_WPFCanvas)
            {
                return "SharpCanvas.Browser.Media";
            }
            else if (_clsidForContext == CLSID_SilverlightCanvas)
            {
                return "SharpCanvas.Browser.Forms";
            }
            return string.Empty;
        }

        /// <summary>
        /// Load assembly depending on current .NET Framework version
        /// </summary>
        /// <returns></returns>
        private static Assembly GetAssembly()
        {
            //Namespace equal to assembly name in our case, so we can do this
            return Assembly.Load(GetNamespace());
        }

        /// <summary>
        /// Factory method returns appropriate HTMLCanvasElement instance, dependent on loaded framework version
        /// </summary>
        /// <returns></returns>
        public IHTMLCanvasElement CreateCanvasElement(ICanvasProxy proxy)
        {
            Assembly assembly = GetAssembly();
            if (assembly != null)
            {
                if (_currentTypeFactory == null)
                {
                    _currentTypeFactory = assembly.GetType(string.Format("{0}.HTMLCanvasElement", GetNamespace()));
                    if (_currentTypeFactory == null) return null;
                }
                //TODO: make proxy optional?
                IHTMLCanvasElement element = Activator.CreateInstance(_currentTypeFactory, proxy) as IHTMLCanvasElement;
                return element;
            }
            return null;
        }

        // FIXME: Handled by Window-specific instance in Browser; load browser via GetWindow().
        /// <summary>
        /// Facotry method returns appropriate Timeout instance, dependent on loaded framework version
        /// </summary>
        /// <returns></returns>
        public ITimeout CreateTimeout()
        {
            Assembly assembly = GetAssembly();
            if (assembly != null)
            {
                Type type = assembly.GetType(string.Format("{0}.Timeout", GetNamespace()));
                ITimeout element = Activator.CreateInstance(type) as ITimeout;
                return element;
            }
            return null;
        }

    }
}