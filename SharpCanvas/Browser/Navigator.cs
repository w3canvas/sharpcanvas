using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    [Guid("56F58DCD-6655-4dfb-824F-5A21D7F53062")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Navigator : INavigator
    {
        public string _userAgent = "SharpCanvas";
        public string appVersion = string.Empty;

        public string userAgent
        {
            get { return _userAgent; }
        }
    }
}
