using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCanvas.Installer
{
    public static class COMInstaller
    {
        private static RegistrationServices regSrv = new RegistrationServices();

        public static void UnRegisterAssembly(KeyValuePair<string, string> assembly)
        {
            Console.WriteLine(string.Format("Unregister assembly {0} with component object models (COM)", assembly.Key));
            Assembly a = Assembly.Load(assembly.Value);
            regSrv.UnregisterAssembly(a);
            Console.WriteLine("Unregistered.");
        }

        public static void RegisterAssembly(KeyValuePair<string, string> assembly)
        {
            Console.WriteLine(string.Format("Register assembly {0} with component object models (COM)", assembly.Key));
            Assembly a = Assembly.Load(assembly.Value);
            regSrv.RegisterAssembly(a, AssemblyRegistrationFlags.SetCodeBase);
            Console.WriteLine("Registered.");
        }
    }
}
