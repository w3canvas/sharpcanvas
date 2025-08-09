using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Convert = System.Convert;

namespace SharpCanvas
{
    public class Utils
    {
        private static readonly Regex httpRegex = new Regex(@"http://.*");


        public static byte[] CopyBitmapToBytes(int x, int y, int width, int height, Bitmap bmp)
        {
            PixelFormat pxf = bmp.PixelFormat;

            // Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, width - x, height - y);
            Bitmap clone = bmp;
            if (bmp.Width != width || bmp.Height != height)
            {
                clone = bmp.Clone(new Rectangle(x, y, width, height), bmp.PixelFormat);
            }
            BitmapData bmpData =
                clone.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int numBytes = width*height*4;
            //int numBytes = bmpData.Stride * height;
            var rgbValues = new byte[numBytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Unlock the bits.
            clone.UnlockBits(bmpData);
            return rgbValues;
        }

        /// <summary>
        /// Updates a bitmap from a byte array
        /// </summary>
        /// <param name="srcData">Should be 32 or 24 bits per pixel (ARGB or RGB format)</param>
        /// <param name="srcDataWidth">Width of the image srcData represents</param>
        /// <param name="srcDataHeight">Height of the image srcData represents</param>
        /// <param name="destBitmap">Bitmap to copy to. Will be recreated if necessary to copy to the array.</param>
        public static void CopyBytesToBitmap(byte[] srcData, int srcDataWidth, int srcDataHeight, ref Bitmap destBitmap)
        {
            int bytesPerPixel = srcData.Length/(srcDataWidth*srcDataHeight);
            //if (destBitmap == null
            //    || destBitmap.Width != srcDataWidth
            //    || destBitmap.Height != srcDataHeight
            //    || (destBitmap.PixelFormat == PixelFormat.Format32bppArgb && bytesPerPixel == 3)
            //    || (destBitmap.PixelFormat == PixelFormat.Format32bppRgb && bytesPerPixel == 3)
            //    || (destBitmap.PixelFormat == PixelFormat.Format24bppRgb && bytesPerPixel == 4))
            //{
            //    if (bytesPerPixel == 3)
            //        destBitmap = new Bitmap(srcDataWidth, srcDataHeight, PixelFormat.Format24bppRgb);
            //    else
            //        destBitmap = new Bitmap(srcDataWidth, srcDataHeight, PixelFormat.Format32bppRgb);
            //}
            BitmapData bmpData = null;
            //try
            //{
            if (bytesPerPixel == 3)
                bmpData = destBitmap.LockBits(new Rectangle(0, 0, srcDataWidth, srcDataHeight), ImageLockMode.WriteOnly,
                                              PixelFormat.Format24bppRgb);
            else
                bmpData = destBitmap.LockBits(new Rectangle(0, 0, srcDataWidth, srcDataHeight), ImageLockMode.WriteOnly,
                                              PixelFormat.Format32bppRgb);

            Marshal.Copy(srcData, 0, bmpData.Scan0, srcData.Length);
            destBitmap.UnlockBits(bmpData);
            //}
            //catch (Exception)
            //{
            //}
        }

        public static Bitmap GetBitmapFromUrl(string imageData)
        {
            if (httpRegex.IsMatch(imageData))
            {
                WebRequest request = WebRequest.Create(imageData);
                request.Timeout = 10000;
                var response = (HttpWebResponse) request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                return new Bitmap(dataStream);
            }
            else //get bitmap from hard disk
            {
                if(File.Exists(imageData))
                {
                    return new Bitmap(imageData);
                }
            }
            return new Bitmap(1,1);//if image data is invalid - return empty bitmap
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static ImageFormat ImageFormatFromMediaType(string type)
        {
            switch (type)
            {
                case "image/png":
                case "image/x-png":
                    return ImageFormat.Png;
                    break;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                    break;
                case "image/gif":
                    return ImageFormat.Gif;
                    break;
                case "image/bmp":
                    return ImageFormat.Bmp;
                    break;
                default:
                    return ImageFormat.Png;
            }
        }

        public static object ConvertArrayToJSArray(object[] arr)
        {
            return arr;
        }

        public static byte[] ConvertJSArrayToByteArray(object arr)
        {
            if (arr is object[] objArray)
            {
                byte[] byteArr = new byte[objArray.Length];
                for (int i = 0; i < objArray.Length; i++)
                {
                    byteArr[i] = Convert.ToByte(objArray[i]);
                }
                return byteArr;
            }
            return new byte[0];
        }
    }
}