using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpCanvas.Host.Browser
{
    /// <summary>
    /// Load specified assembly and all its dependencies (asynchronously?)
    /// </summary>
    public class AssemblyLoader
    {
        private Uri _uri;

        public AssemblyLoader(Uri uri)
        {
            _uri = uri;
        }

        public Assembly Load()
        {
            //load file
            IFileLoader fileLoader = LoaderFactory.GetLoader(_uri);
            byte[] data = fileLoader.Load();
            //get current domain
            AppDomain appDomain = AppDomain.CurrentDomain;
            //load assembly from received bytes
            appDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(Location_ResolveEventHandler);
            return appDomain.Load(data);
        }

        /// <summary>
        /// This handler is called only when the common language runtime tries to bind to the assembly and fails.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly Location_ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly loadedAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = _uri.AbsolutePath + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    Uri uri = new Uri(strTempAssmbPath);
                    AssemblyLoader dependentAssemblyLoader = new AssemblyLoader(uri);
                    return dependentAssemblyLoader.Load();
                }
            }
            return null;
        }
    }
}
