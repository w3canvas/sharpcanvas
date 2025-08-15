#pragma warning disable SYSLIB0014
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Convert=System.Convert;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    public class Image : HTMLElement, IImage
    {
        private Bitmap _bitmap = null!;
//        private Graphics _graphics;
        private int _height;
        private string _src = string.Empty;
        private int _width;

        public Image()
        {
            //Paint += Image_Paint;
            //Name = "image";
            onload = delegate { };
        }

        #region IImage Members

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int height
        {
            get { return _height; }
            set { _height = value; }
        }

        public string src
        {
            get { return _src; }
            set
            {
                _src = value;
                //load image to memory
                string prefix = "image/png;base64,";
                if (!string.IsNullOrEmpty(_src))
                {
                    int startIndex = _src.IndexOf(prefix) + prefix.Length;
                    if (startIndex > 16)
                    {
                        string data = _src.Substring(startIndex);
                        byte[] imageData = Convert.FromBase64String(data);
                        var stream = new MemoryStream(imageData);
                        _bitmap = new Bitmap(stream);
                    }
                    else if (_src.Contains("http:"))
                    {
                        var uriObj = new Uri(_src);
                        var request = (HttpWebRequest) WebRequest.CreateDefault(uriObj);
                        request.Timeout = 5000; // 5 seconds in milliseconds
                        request.ReadWriteTimeout = 20000; // allow up to 20 seconds to elapse
                        var response = (HttpWebResponse)
                                       request.GetResponse();

                        _bitmap = (Bitmap) System.Drawing.Image.FromStream(response.GetResponseStream());
                    }
                    else
                    {
                        _bitmap = (Bitmap) System.Drawing.Image.FromFile(_src);
                    }
                    if (_bitmap != null)
                    {
                        _width = _bitmap.Width;
                        _height = _bitmap.Height;
                    }
                    //fire onload
                    if (onload != null)
                    {
                        ((Action<object>) onload).Invoke(this);
                    }
                }
            }
        }

        public Action<object> onload { get; set; } //what type should it be? maybe it should be delegate?
/*
        public void drawImage(object image)
        {
            var img = image as Bitmap;
            _graphics.DrawImage(img, 0, 0);
        }
*/
        public object getImage()
        {
            return _bitmap;
        }

        #endregion

        private void Image_Paint(object? sender, PaintEventArgs e)
        {
            //_graphics = e.Graphics;            
            //if (_bitmap != null) //draw image
            //{
            //    this.ClientSize = new System.Drawing.Size(_bitmap.Width, _bitmap.Height);
            //    drawImage(_bitmap);                
            //}
        }
    }
}