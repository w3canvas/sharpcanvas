using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCanvas.Installer
{
    class Program
    {
        /// <summary>
        /// List of libraries to install
        /// </summary>
        static Dictionary<string, string> assemblies = new Dictionary<string, string>()
        {
            {"SharpCanvas.dll", "SharpCanvas, Version=1.0.0.1, Culture=neutral, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Browser.dll","SharpCanvas.Browser, Version=1.0.0.1, Culture=neutral, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Browser.Forms.dll", "SharpCanvas.Browser.Forms, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Browser.Media.dll", "SharpCanvas.Browser.Media, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Forms.dll", "SharpCanvas.Forms, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Media.dll", "SharpCanvas.Media, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Host.dll", "SharpCanvas.Host, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Interop.dll", "SharpCanvas.Interop, Version=1.0.0.1, Culture=neutral, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.Prototype.dll", "SharpCanvas.Prototype, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.StandardFilter.dll", "SharpCanvas.StandardFilter, Version=1.0.0.1, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"},
            {"SharpCanvas.ShaderFilter.dll","SharpCanvas.ShaderFilter, Version=1.0.0.1, Culture=neutral, PublicKeyToken=92843221795deba4, ProcessorArchitecture=MSIL"}
        };

        static void Main(string[] args)
        {
            var path = @"C:\Users\Downchuck\Desktop\SharpCanvas\bin\Release\";
            // string[] copyPaths = new string[]{@"c:\Personal\SharpCanvas\SharpCanvas.Tests\CanvasAd\", @"c:\Personal\Darkroom\dist\js\build\"};
            foreach (KeyValuePair<string, string> assembly in assemblies)
            {
                string fileName = path + assembly.Key;
                var x = Assembly.LoadFile(fileName);
                var assemblyPath = fileName;
                var assemblyName = x.ToString()+", ProcessorArchitecture=MSIL";// GetName();
                Console.WriteLine("Working: " + assemblyName);
                //remove old library from the GAC
                GACInstaller.UninstallAssemblyIfExists(assembly.Value);
                //unregister library as COM lib
                COMInstaller.UnRegisterAssembly(assembly);                

                //install library to the GAC
                GACInstaller.InstallAssemblyIfExists(assemblyPath);
                //register library as COM lib
                COMInstaller.RegisterAssembly(assembly);
                //copy to work dirs
                /*foreach (string copyPath in copyPaths)
                {
                    Console.WriteLine(string.Format("Copy to: {0}", copyPath));
                    File.Copy(fileName, copyPath + assembly.Key, true);
                    File.Copy(fileName, copyPath + assembly.Key.Replace(".dll", ".pdb"), true);
                    Console.WriteLine();
                }*/
            }
            Console.WriteLine("Installation is ended.");
            Console.ReadLine();
        }
        
    }
}
