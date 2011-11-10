using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SharpCanvas.Installer
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("120FE119-0B3F-4d67-BBB7-499C6C78281D")]
    internal interface IAssemblyName
    {
        [PreserveSig()]
        int SetProperty(
                int PropertyId,
                IntPtr pvProperty,
                int cbProperty);

        [PreserveSig()]
        int GetProperty(
                int PropertyId,
                IntPtr pvProperty,
                ref int pcbProperty);

        [PreserveSig()]
        int Finalize();

        [PreserveSig()]
        int GetDisplayName(
                StringBuilder pDisplayName,
                ref int pccDisplayName,
                int displayFlags);

        [PreserveSig()]
        int Reserved(ref Guid guid,
            Object obj1,
            Object obj2,
            String string1,
            Int64 llFlags,
            IntPtr pvReserved,
            int cbReserved,
            out IntPtr ppv);

        [PreserveSig()]
        int GetName(
                ref int pccBuffer,
                StringBuilder pwzName);

        [PreserveSig()]
        int GetVersion(
                out int versionHi,
                out int versionLow);
        [PreserveSig()]
        int IsEqual(
                IAssemblyName pAsmName,
                int cmpFlags);

        [PreserveSig()]
        int Clone(out IAssemblyName pAsmName);
    }
}
