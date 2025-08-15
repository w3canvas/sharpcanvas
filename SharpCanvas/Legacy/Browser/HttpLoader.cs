using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Browser
{
    internal class HttpLoader: IFileLoader
    {
        private Uri _uri;

        public HttpLoader(Uri uri)
        {
            _uri = uri;
        }

        #region Implementation of IFileLoader

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
