using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Host.Browser
{
    public delegate void FileLoadedEventHandler(byte[] data);

    internal interface IFileLoader
    {
        event FileLoadedEventHandler FileLoaded;
        void BeginLoad();
        byte[] Load();
    }
}
