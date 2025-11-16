using System;
using System.Reflection;
using SharpCanvas.Interop;
using SharpCanvas;
using SharpCanvas.Shared;

namespace SharpCanvas.Host
{
    public interface IStandaloneFactory
    {
        IHTMLCanvasElement CreateCanvasElement(ICanvasProxy proxy);
        // DEPRECATED: This is part of the legacy standalone hosting model. The Window-specific
        // instances in the Browser libraries are the correct implementation.
        // FIXME: Event loop should be CreateWindowElement();
        ITimeout CreateTimeout();
    }

    public class StandaloneBootstrapper
    {

        // Qualified names of the assembled WPF and WinForms factory implementations
        // System.Forms and Windows.Media are the two most widely implemented namespaces.
        // Shaders are [pre-]compiled into a different bytecode than filters.

        private const string wpfFactoryTypename = "SharpCanvas.Browser.Media+HTMLCanvasElement" + ", " +
            "SharpCanvas.Browser.Media, Version=1.0.0.1, PublicKeyToken=60da03464eb3376b, ProcessorArchitecture=MSIL";
        private const string winformsFactoryName = "SharpCanvas.Host.Standalone, SharpCanvas.Host, "+
            "Version=1.0.0.1, Culture=neutral, PublicKeyToken=90f724f2197bb455";

        private const string shaderFactoryTypename = "SharpCanvas.ShaderFilter+Compositor" + ", " +
            "SharpCanvas.ShaderFilter, Version=1.0.0.1, Culture=neutral, PublicKeyToken=e0fd62732abd8834, ProcessorArchitecture=MSIL";

        private const string filterFactoryTypename = "SharpCanvas.StandardFilter+Compositor" + ", " +
            "SharpCanvas.StandardFilter, Version=1.0.0.1, PublicKeyToken=4ca95407b2e14a6a, ProcessorArchitecture=MSIL";

        private static IStandaloneFactory _factory;

        /// <summary>
        /// Figure out the best implementation to use. This will run at load time.
        /// </summary>
        static StandaloneBootstrapper()
        {
            Type factoryType = null;
            Type libFactoryType = null;

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
                // Try to load any (the preferred) version of a core WPF assembly. If this succeeds, there
                // is no overhead; we need it loaded anyway. If it fails, we don't have WPF installed.
                //////////
                //TODO: remove false from condition below. JUST FOR TESTING PURPOSES
                if (Assembly.Load("WindowsBase") != null && false)
                {
                    // WPF installed, use that
                    factoryType = Type.GetType(StandaloneBootstrapper.wpfFactoryTypename);
                }
                else
                {
                    // WPF needs 3 or higher, so use WinForms
                    factoryType = Type.GetType(StandaloneBootstrapper.winformsFactoryName);
                }
            }
            else
            {
                //////////
                // WPF is a given
                //////////
                factoryType = Type.GetType(StandaloneBootstrapper.wpfFactoryTypename);
            }

            if (Assembly.Load("System.Windows.Media.Effects") != null)
            {
                libFactoryType = Type.GetType(StandaloneBootstrapper.shaderFactoryTypename);
            }
            else
            {
                libFactoryType = Type.GetType(StandaloneBootstrapper.filterFactoryTypename);
            }

            // TODO: Call the assembly loader.
            //if (factoryType == null) throw new System.Exception("Required WPF and WinForms libraries were not loaded");
            if (factoryType != null)
            {
                StandaloneBootstrapper._factory = (IStandaloneFactory) Activator.CreateInstance(factoryType);
            }
        }

        public static IStandaloneFactory Factory
        {
            get
            {
                return StandaloneBootstrapper._factory;
            }
        }
    }
}
