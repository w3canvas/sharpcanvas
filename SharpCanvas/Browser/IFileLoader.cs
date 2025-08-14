using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Browser
{
    public delegate void FileLoadedEventHandler(byte[] data);

    internal interface IFileLoader
    {
        void BeginLoad();
        byte[] Load();
    }
}
