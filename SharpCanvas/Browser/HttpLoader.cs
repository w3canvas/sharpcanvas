using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Host.Browser
{
    internal class HttpLoader: IFileLoader
    {
        private Uri _uri;

        public HttpLoader(Uri uri)
        {
            _uri = uri;
        }

        #region Implementation of IFileLoader

        public event FileLoadedEventHandler FileLoaded;

        public void BeginLoad()
        {
            throw new NotImplementedException();
        }

        public byte[] Load()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
