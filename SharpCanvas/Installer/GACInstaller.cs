using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpCanvas.Installer
{
    public static class GACInstaller
    {
        public static void InstallAssemblyIfExists(string assemblyFile)
        {
            Console.WriteLine(string.Format("Looking for {0} assembly on the disk...", assemblyFile));
            if (File.Exists(assemblyFile))
            {
                try
                {
                    AssemblyCache.InstallAssembly(assemblyFile, null, AssemblyCommitFlags.Force);
//                    AssemblyCache.InstallAssembly(AppDomain.CurrentDomain.BaseDirectory + "//" + assemblyFile, null,
//                                                  AssemblyCommitFlags.Force);
                    Console.WriteLine("Installation completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error: {0}", ex.Message));
                }
            }
            else
            {
                Console.WriteLine("File doesn't exists.");
            }
        }

        public static void UninstallAssemblyIfExists(string assembly)
        {
            Console.WriteLine(string.Format("Checking assembly {0}", assembly));
            try
            {
                AssemblyCache.QueryAssemblyInfo(assembly);
                Console.WriteLine("Assembly found, trying to uninstall...");
                AssemblyCacheUninstallDisposition result;
                AssemblyCache.UninstallAssembly(assembly, null, out result);
                Console.WriteLine(string.Format("Result: {0}", Enum.GetName(typeof(AssemblyCacheUninstallDisposition), result)));
            }
            catch (Exception ex)
            {
                if(ex == null) Console.WriteLine(string.Format("Error: {0}", ex.Message));
            }
        }
    }
}
