using System;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpCanvas.Interop;

namespace SharpCanvas.Host
{
    public static class Bootstrapper
    {
        // Can Silverlight 3 use the WPF build?
        internal static readonly Guid CLSID_SilverlightCanvas = new Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5");

        /// <summary>
        /// Class ids (COM) for the diverse canvas implementations we know about. The bootstrapper will
        /// figure out on load time, what the best canvas implementation would be for the current environment.
        /// Since some implementations (WinForms) are more COM friendly that others (WPF), it may be necessary
        /// to inject a wrapper to handle activex hosting. Also, although silverlight manifests itself as
        /// a COM component, we will need to point it to the actual .NET assembly to load, meaning extra
        /// initialization may be needed. This is handled by passing the html object element to the
        /// bootstrapper rather than just asking the bootstrapper for the proper class id.
        /// </summary>
        internal static readonly Guid CLSID_WinFormsCanvas = new Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5");

        internal static readonly Guid CLSID_WPFCanvas = new Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5");//new Guid("35a22abc-5a67-4723-8da4-c1b00e0853f4");

        private static Guid _clsidForContext = Guid.Empty;

        /// <summary>
        /// Figure out the best canvas implementation to use. This will run at load time.
        /// </summary>
        static Bootstrapper()
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
                if (Assembly.LoadWithPartialName("WindowsBase") != null)
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
        /// This method should be called only in IE environment and the main intent of this method is to add necessary behavior to the object
        /// </summary>
        /// <param name="element"></param>
        internal static void Initialize(IHTMLObjectElement element)
        {
            string clsidString = string.Format("CLSID:{0}", _clsidForContext);
            ((IHTMLElement) element).setAttribute("classid", clsidString, 0);
        }
    }
}