using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    [Serializable]
    public class Location : ILocation
    {
        private const string base64PDF = "data:application/pdf;base64,";
        //private static Regex _urlExpression = new Regex(@"(?:(?<protocol>http(?:s?)|ftp)(?:\:\/\/))(?:(?<usrpwd>\w+\:\w+)(?:\@))?(?<domain>[^/\r\n\:]+)?(?<port>\:\d+)?(?<path>(?:\/.*)*\/)?(?<filename>.*?\.(?<ext>\w{2,4}))?(?<qrystr>\??(?:\w+\=[^\#]+)(?:\&?\w+\=\w+)*)*(?<bkmrk>\#.*)?");

        #region Private Fields
        
        private string _href;

        public event OnSaveFileHandler OnSaveFile;

        private Uri _uri;
        private IFileLoader _dllLoader;
        private Assembly _assembly;

        #endregion

        #region Public Properties

        public Assembly assembly
        {
            get { return _assembly; }
        }

        public object hash { get; set; }

        /// <summary>
        /// This attribute represents the network host of the Location's URI. If the port attribute is not null then the host attribute's value is 
        /// the concatenation of the hostname attribute, a colon (:) and the port attribute. If the port attribute is null then the host attribute's value 
        /// is identical to the hostname attribute.
        /// </summary>
        public object host { get; set; }

        /// <summary>
        /// This attribute represents the name or IP address of the network location without any port number.
        /// </summary>
        public object hostname { get; set; }

        /// <summary>
        /// The value of the href attribute MUST be the absolute URI reference.
        /// </summary>
        public string href
        {
            get { return _href; }
            set
            {
                _href = value;
                bool isPDF = _href.IndexOf(base64PDF) != -1;
                if (isPDF)
                {
                    OpenPDF();
                }

                if(Uri.TryCreate(_href, UriKind.RelativeOrAbsolute, out _uri))
                {
                    //?hash
                    host = _uri.Authority;
                    hostname = _uri.Host;
                    pathname = _uri.AbsolutePath;
                    port = _uri.Port;
                    protocol = _uri.Scheme;
                    search = _uri.Query;
                    if(_uri.IsFile)
                    {
                        AssemblyLoader assemblyLoader = new AssemblyLoader(_uri);
                        _assembly = assemblyLoader.Load();
                    }
                }
            }
        }

        private void OpenPDF()
        {
            int i = _href.IndexOf(base64PDF);
            //JVBERi0xLjMKMyAwIG9iago8PC9UeXBlIC9QYWdlCi9QYXJlbnQgMSAwIFIKL1Jlc291cmNlcyAyIDAgUgovQ29udGVudHMgNCAwIFI+Pgpl
            //bmRvYmoKNCAwIG9iago8PC9MZW5ndGggMTQ0Pj4Kc3RyZWFtCjAuNTcgdwpCVCAvRjEgMTYuMDAgVGYgRVQKQlQgNTYuNjkgNzg1LjIwIFRk
            //IChIZWxsbyB3b3JsZCEpIFRqIEVUCkJUIDU2LjY5IDc1Ni44NSBUZCAoVGhpcyBpcyBjbGllbnQtc2lkZSBKYXZhc2NyaXB0LCBwdW1waW5nIG
            //91dCBhIFBERi4pIFRqIEVUCgplbmRzdHJlYW0KZW5kb2JqCjUgMCBvYmoKPDwvVHlwZSAvUGFnZQovUGFyZW50IDEgMCBSCi9SZXNvdXJjZXMg
            //    MiAwIFIKL0NvbnRlbnRzIDYgMCBSPj4KZW5kb2JqCjYgMCBvYmoKPDwvTGVuZ3RoIDcxPj4Kc3RyZWFtCjAuNTcgdwpCVCAvRjEgMTYuMD
            //AgVGYgRVQKQlQgNTYuNjkgNzg1LjIwIFRkIChEbyB5b3UgbGlrZSB0aGF0PykgVGogRVQKCmVuZHN0cmVhbQplbmRvYmoKMSAwIG9iago8PC9U
            //    eXBlIC9QYWdlcwovS2lkcyBbMyAwIFIgNSAwIFIgXQovQ291bnQgMgovTWVkaWFCb3ggWzAgMCA1OTUuMjggODQxLjg5XQo+PgplbmRvYm
            //oKNyAwIG9iago8PC9UeXBlIC9Gb250Ci9CYXNlRm9udCAvSGVsdmV0aWNhCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNv
            //    ZGluZwo+PgplbmRvYmoKMiAwIG9iago8PAovUHJvY1NldCBbL1BERiAvVGV4dCAvSW1hZ2VCIC9JbWFnZUMgL0ltYWdlSV0KL0ZvbnQgPDw
            //KL0YxIDcgMCBSCj4+Ci9YT2JqZWN0IDw8Cj4+Cj4+CmVuZG9iago4IDAgb2JqCjw8Ci9Qcm9kdWNlciAoanNQREYgMjAwOTA1MDQpCi9DcmVhdG
            //lvbkRhdGUgKEQ6MjAxMDAxMTcyMjIyNTUpCj4+CmVuZG9iago5IDAgb2JqCjw8Ci9UeXBlIC9DYXRhbG9nCi9QYWdlcyAxIDAgUgovT3BlbkFjdG
            //lvbiBbMyAwIFIgL0ZpdEggbnVsbF0KL1BhZ2VMYXlvdXQgL09uZUNvbHVtbgo+PgplbmRvYmoKeHJlZgowIDEwCjAwMDAwMDAwMDAgNjU1MzUgZi
            //AKMDAwMDAwMDQ3NyAwMDAwMCBuIAowMDAwMDAwNjY2IDAwMDAwIG4gCjAwMDAwMDAwMDkgMDAwMDAgbiAKMDAwMDAwMDA4NyAwMDAwMCBuIAowMD
            //    AwMDAwMjgwIDAwMDAwIG4gCjAwMDAwMDAzNTggMDAwMDAgbiAKMDAwMDAwMDU3MCAwMDAwMCBuIAowMDAwMDAwNzcwIDAwMDAwIG4gCjAwM
            //DAwMDA4NTEgMDAwMDAgbiAKdHJhaWxlcgo8PAovU2l6ZSAxMAovUm9vdCA5IDAgUgovSW5mbyA4IDAgUgo+PgpzdGFydHhyZWYKOTU0CiUlRU9GCg==

            string base64String = _href.Substring(i + base64PDF.Length);
            Byte[] pdfBytes = Convert.FromBase64String(base64String);
            if (OnSaveFile != null)
            {
                OnSaveFile(pdfBytes);
            }
        }

        /// <summary>
        /// This attribute represents the path component of the Location's URI which consists of everything after the host and port up to and excluding the first question mark (?) or hash mark (#).
        /// </summary>
        public object pathname { get; set; }

        /// <summary>
        /// This attribute represents the port number of the network location.
        /// </summary>
        public object port { get; set; }

        /// <summary>
        /// This attribute represents the scheme of the URI including the trailing colon (:)
        /// </summary>
        public object protocol { get; set; }

        /// <summary>
        /// This attribute represents the query portion of a URI. It consists of everything after the pathname up to and excluding the first hash mark (#).
        /// </summary>
        public object search { get; set; }

        public void assign(string url)
        {
            throw new NotImplementedException();
        }

        public void replace(string url)
        {
            throw new NotImplementedException();
        }

        public void reload()
        {
            throw new NotImplementedException();
        }

        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                = Encoding.Default.GetBytes(toEncode);
            string returnValue
                = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        #endregion

        #region Load dynamic dll

        //public bool IsDLLReference()
        //{
        //    Uri 
        //    if(_urlExpression.IsMatch(this._href))
        //    {
        //        Match match = _urlExpression.Match(this._href);
        //        string ext = match.Groups["ext"].Value;
        //        return !string.IsNullOrEmpty(ext) && ext == "dll";
        //    }
        //    Path. 
        //    return false;
        //}

        

        #endregion
    }
}