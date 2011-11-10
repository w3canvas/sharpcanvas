using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SharpCanvas.Installer
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("1AA6D0FF-D8E7-4432-9C98-0472FC5C86AE")]
    internal interface IAssemblyEnum
    {
        [PreserveSig()]
        int GetNextAssembly(
                IntPtr pvReserved,
                out IAssemblyName ppName,
                int flags);
        [PreserveSig()]
        int Reset();
        [PreserveSig()]
        int Clone(out IAssemblyEnum ppEnum);
    }
}
