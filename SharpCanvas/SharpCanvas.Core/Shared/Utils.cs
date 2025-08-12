#if WINDOWS
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using Bitmap = System.Drawing.Bitmap;
using Image = System.Drawing.Image;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
#else
using SkiaSharp;
using System.Net.Http;
using Bitmap = SkiaSharp.SKBitmap;
using ImageFormat = SkiaSharp.SKEncodedImageFormat;
#endif
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Convert = System.Convert;

namespace SharpCanvas
{
    public class Utils
    {
        private static readonly Regex httpRegex = new Regex(@"http://.*");

#if WINDOWS
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

        public static void CopyBytesToBitmap(byte[] srcData, int srcDataWidth, int srcDataHeight, ref Bitmap destBitmap)
        {
            int bytesPerPixel = srcData.Length/(srcDataWidth*srcDataHeight);
            BitmapData bmpData = null;
            if (bytesPerPixel == 3)
                bmpData = destBitmap.LockBits(new Rectangle(0, 0, srcDataWidth, srcDataHeight), ImageLockMode.WriteOnly,
                                              PixelFormat.Format24bppRgb);
            else
                bmpData = destBitmap.LockBits(new Rectangle(0, 0, srcDataWidth, srcDataHeight), ImageLockMode.WriteOnly,
                                              PixelFormat.Format32bppRgb);

            Marshal.Copy(srcData, 0, bmpData.Scan0, srcData.Length);
            destBitmap.UnlockBits(bmpData);
        }

        public static Bitmap GetBitmapFromUrl(string imageData)
        {
            if (httpRegex.IsMatch(imageData))
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    try
                    {
                        var stream = httpClient.GetStreamAsync(imageData).Result;
                        return new Bitmap(stream);
                    }
                    catch (System.Net.Http.HttpRequestException)
                    {
                        return new Bitmap(1, 1);
                    }
                }
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
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
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
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Png;
            }
        }
#else
        public static byte[] CopyBitmapToBytes(int x, int y, int width, int height, Bitmap bmp)
        {
            var info = new SKImageInfo(width, height);
            using (var subset = new Bitmap(info))
            {
                if (bmp.ExtractSubset(subset, new SKRectI(x, y, x + width, y + height)))
                {
                    return subset.Bytes;
                }
                return bmp.Bytes; // or throw an exception
            }
        }

        public static void CopyBytesToBitmap(byte[] srcData, int srcDataWidth, int srcDataHeight, ref Bitmap destBitmap)
        {
            var info = new SKImageInfo(srcDataWidth, srcDataHeight, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            var gcHandle = GCHandle.Alloc(srcData, GCHandleType.Pinned);
            destBitmap.InstallPixels(info, gcHandle.AddrOfPinnedObject(), info.RowBytes, (address, context) => ((GCHandle)context).Free(), gcHandle);
        }

        public static Bitmap GetBitmapFromUrl(string imageData)
        {
            if (httpRegex.IsMatch(imageData))
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = httpClient.GetAsync(imageData).Result;
                        response.EnsureSuccessStatusCode();
                        using (var stream = response.Content.ReadAsStreamAsync().Result)
                        {
                            return Bitmap.Decode(stream);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        return new Bitmap(1, 1);
                    }
                }
            }
            else
            {
                if (File.Exists(imageData))
                {
                    return Bitmap.Decode(imageData);
                }
            }
            return new Bitmap(1, 1);
        }

        public static string ImageToBase64(SKBitmap image, ImageFormat format)
        {
            using (var skImage = SKImage.FromBitmap(image))
            using (var data = skImage.Encode(format, 100))
            {
                return Convert.ToBase64String(data.ToArray());
            }
        }

        public static ImageFormat ImageFormatFromMediaType(string type)
        {
            switch (type)
            {
                case "image/png":
                case "image/x-png":
                    return ImageFormat.Png;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Png;
            }
        }
#endif

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