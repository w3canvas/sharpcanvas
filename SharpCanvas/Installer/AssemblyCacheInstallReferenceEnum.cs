﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SharpCanvas.Installer
{
    public class AssemblyCacheInstallReferenceEnum
    {
        public AssemblyCacheInstallReferenceEnum(String assemblyName)
        {
            IAssemblyName fusionName = null;

            int hr = Utils.CreateAssemblyNameObject(
                        out fusionName,
                        assemblyName,
                        CreateAssemblyNameObjectFlags.CANOF_PARSE_DISPLAY_NAME,
                        IntPtr.Zero);

            if (hr >= 0)
            {
                hr = Utils.CreateInstallReferenceEnum(out refEnum, fusionName, 0, IntPtr.Zero);
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        public InstallReference GetNextReference()
        {
            IInstallReferenceItem item = null;
            int hr = refEnum.GetNextInstallReferenceItem(out item, 0, IntPtr.Zero);
            if ((uint)hr == 0x80070103)
            {   // ERROR_NO_MORE_ITEMS
                return null;
            }

            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            IntPtr refData;
            InstallReference instRef = new InstallReference(Guid.Empty, String.Empty, String.Empty);

            hr = item.GetReference(out refData, 0, IntPtr.Zero);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            Marshal.PtrToStructure(refData, instRef);
            return instRef;
        }

        private IInstallReferenceEnum refEnum;
    }
}
