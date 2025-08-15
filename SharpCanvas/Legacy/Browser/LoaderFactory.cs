using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SharpCanvas.Browser
{
    internal static class LoaderFactory
    {
        internal static IFileLoader GetLoader(Uri uri)
        {
            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                return new HttpLoader(uri);
            }
            else if (uri.Scheme == Uri.UriSchemeFile)
            {
                return new LFSLoader(uri);
            }
            throw new Exception(string.Format("Path format {0} is not supported yet.", uri.Scheme));
        }
    }
}
