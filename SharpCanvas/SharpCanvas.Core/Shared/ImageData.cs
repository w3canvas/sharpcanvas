#if WINDOWS
using Bitmap = System.Drawing.Bitmap;
#else
using Bitmap = SkiaSharp.SKBitmap;
#endif
using System.Runtime.InteropServices;
using SharpCanvas.Shared;

namespace SharpCanvas.Shared
{
    [ComVisible(true),
     ComSourceInterfaces(typeof (IImageData))]
    public class ImageData : IImageData
    {
        private object _data;

        public ImageData()
        {
            _data = new byte[0];
            alt = string.Empty;
            src = string.Empty;
            useMap = string.Empty;
        }

        public ImageData(uint sw, uint sh)
        {
            height = sh;
            width = sw;
            _data = new byte[0];
            alt = string.Empty;
            src = string.Empty;
            useMap = string.Empty;
        }

        #region IImageData Members

        public object data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string alt { get; set; }

        public string src { get; set; }

        public string useMap { get; set; }

        public bool isMap { get; set; }

        public uint width { get; set; }

        public uint height { get; set; }

        #endregion

#if WINDOWS
        public void applyFilters(FilterChain chain)
        {
            var bmp = new Bitmap((int) width, (int) height);
            Utils.CopyBytesToBitmap(Utils.ConvertJSArrayToByteArray(_data), (int) width, (int) height, ref bmp);
            Bitmap? filtered = chain.ApplyFilters(bmp);
            if(filtered != null)
                _data = Utils.CopyBitmapToBytes(0, 0, (int) width, (int) height, filtered);
        }
#else
        public void applyFilters(FilterChain chain)
        {
            var bmp = new Bitmap((int) width, (int) height);
            Utils.CopyBytesToBitmap(Utils.ConvertJSArrayToByteArray(_data), (int)width, (int)height, ref bmp);
            Bitmap? filtered = chain.ApplyFilters(bmp);
            if (filtered != null)
            {
                _data = Utils.CopyBitmapToBytes(0, 0, (int)width, (int)height, filtered);
            }
        }
#endif
    }
}