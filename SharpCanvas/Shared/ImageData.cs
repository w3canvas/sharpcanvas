﻿using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas
{
    /// <summary>
    /// The CanvasPixelArray object provides ordered, indexed access to the color components of each pixel of the image data. 
    /// The data must be represented in left-to-right order, row by row top to bottom, starting with the top left, with each pixel's red, green, blue, 
    /// and alpha components being given in that order for each pixel. Each component of each device pixel represented in this array must be 
    /// in the range 0..255, representing the 8 bit value for that component. The components must be assigned consecutive indices 
    /// starting with 0 for the top left pixel's red component.
    /// </summary>
    [ComVisible(true),
     ComSourceInterfaces(typeof (IImageData))]
    public class ImageData : IImageData
    {
        private Microsoft.JScript.ArrayObject _data;

        public ImageData()
        {
        }

        public ImageData(uint sw, uint sh)
        {
            height = sh;
            width = sw;
        }

        #region IImageData Members

        public Microsoft.JScript.ArrayObject data
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

        public void applyFilters(FilterChain chain)
        {
            //get current data as byte array
            byte[] data = Utils.ConvertJSArrayToByteArray(_data);
            var arr = new List<object>();
            var bmp = new Bitmap((int) width, (int) height);
            //for (int i = 0; i < data.Length; i += 4)
            //{
            //    int k = i/4;
            //    int x = k%(int)width;
            //    int y = k / (int)width;
            //    bmp.SetPixel(x, y, Color.FromArgb(data[i+3], data[i], data[i+1], data[i+2]));
            //}
            Utils.CopyBytesToBitmap(data, (int) width, (int) height, ref bmp);
            Bitmap filtered = chain.ApplyFilters(bmp);
            byte[] bytes = Utils.CopyBitmapToBytes(0, 0, (int) width, (int) height, filtered);
            var objects = new object[bytes.Length];
            bytes.CopyTo(objects, 0);
            //store result as array
            _data = Utils.ConvertArrayToJSArray(objects);
        }
    }
}