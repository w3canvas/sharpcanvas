//Displacement Filters
//November 2009, by st0le

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    [ComVisible(true)]
    public class BaseDisplacementFilter : IFilter
    {
        public Bitmap inputBmp;

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap bmp)
        {
            return null;
        }

        public Bitmap ApplyFilter(Bitmap bmp, Rectangle r)
        {
            return null;
        }

        #endregion

        public Bitmap applyDisplacementMapRelative(Rectangle r, Point[,] map)
        {
            //Bitmap outputBmp = (Bitmap)inputBmp.Clone();
            var outputBmp = new Bitmap(inputBmp.Width, inputBmp.Height, inputBmp.PixelFormat);

            BitmapData srcData = inputBmp.LockBits(r, ImageLockMode.ReadWrite, inputBmp.PixelFormat);
            BitmapData dstData = outputBmp.LockBits(r, ImageLockMode.ReadWrite, outputBmp.PixelFormat);

            unsafe
            {
                var pSrc = (byte*) srcData.Scan0;
                var pDst = (byte*) dstData.Scan0;

                int step = (inputBmp.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                //int step = 3;
                int scanline = srcData.Stride;
                int mOffset = srcData.Stride - inputBmp.Width*step;

                int nWidth = r.Width;
                int nHeight = r.Height;

                int x0, y0;

                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        x0 = map[x, y].X;
                        y0 = map[x, y].Y;

                        if ((y + y0) >= 0 && (y + y0) < nHeight && (x + x0) >= 0 && (x + x0) < nWidth)
                        {
                            pDst[0] = pSrc[((y + y0)*scanline) + ((x + x0)*step)];
                            pDst[1] = pSrc[((y + y0)*scanline) + ((x + x0)*step) + 1];
                            pDst[2] = pSrc[((y + y0)*scanline) + ((x + x0)*step) + 2];
                            if (step == 4) pDst[3] = pSrc[((y + y0)*scanline) + ((x + x0)*step) + 3];
                        }
                        pDst += step;
                    }
                    pDst += mOffset;
                }
            } //unsafe

            inputBmp.UnlockBits(srcData);
            outputBmp.UnlockBits(dstData);
            return outputBmp;
        }

        public Bitmap applyDisplacementMapAbsolute(Rectangle r, Point[,] map)
        {
            //Bitmap outputBmp = (Bitmap)inputBmp.Clone();
            var outputBmp = new Bitmap(inputBmp.Width, inputBmp.Height, inputBmp.PixelFormat);

            BitmapData srcData = inputBmp.LockBits(r, ImageLockMode.ReadWrite, inputBmp.PixelFormat);
            BitmapData dstData = outputBmp.LockBits(r, ImageLockMode.ReadWrite, outputBmp.PixelFormat);

            unsafe
            {
                var pSrc = (byte*) srcData.Scan0;
                var pDst = (byte*) dstData.Scan0;

                int step = (inputBmp.PixelFormat == PixelFormat.Format24bppRgb) ? 3 : 4;
                //int step = 3;
                int scanline = srcData.Stride;
                int mOffset = srcData.Stride - inputBmp.Width*step;

                int nWidth = r.Width;
                int nHeight = r.Height;

                int x0, y0;

                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        x0 = map[x, y].X;
                        y0 = map[x, y].Y;

                        if ((y0) >= 0 && (y0) < nHeight && (x0) >= 0 && (x0) < nWidth)
                        {
                            pDst[0] = pSrc[((y0)*scanline) + ((x0)*step)];
                            pDst[1] = pSrc[((y0)*scanline) + ((x0)*step) + 1];
                            pDst[2] = pSrc[((y0)*scanline) + ((x0)*step) + 2];
                            if (step == 4) pDst[3] = pSrc[((y0)*scanline) + ((x0)*step) + 3];
                        }
                        pDst += step;
                    }
                    pDst += mOffset;
                }
            } //unsafe

            inputBmp.UnlockBits(srcData);
            outputBmp.UnlockBits(dstData);
            return outputBmp;
        }
    }
}