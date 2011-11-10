//Convolution Filters
//August 2009, by st0le


using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ConvFilter
{
    [ComVisible(true)]
    public class ConvolutionFilter : IFilter
    {
        private double divisor = 1.0f;
        private double[,] mtx;
        private int N;

        public ConvolutionFilter()
        {
        }

        public ConvolutionFilter(double[,] Mtx)
            : this(Mtx, 1, 0)
        {
            divisor = 0.0f;
            for (int i = 0; i < Mtx.GetLength(0); i++)
                for (int j = 0; j < Mtx.GetLength(1); j++)
                    divisor += mtx[i, j];
            if (divisor == 0)
                divisor = 1;
        }

        public ConvolutionFilter(double[,] Mtx, double Div, int Bias)
        {
            if ((Mtx.GetLength(0) == Mtx.GetLength(1)) //square matrix?
                && (Mtx.GetLength(0)%2 == 1) // of odd dimension
                && (Mtx.GetLength(0) >= 3)) //and atleast of size 3
            {
                mtx = Mtx;
                N = mtx.GetLength(0);
                divisor = Div;
                this.Bias = Bias;
            }
            else throw new ArgumentException("Invalid Convolution Matrix...");
        }

        public double Divisor
        {
            get { return divisor; }
            set
            {
                if (value == 0 || double.IsNaN(value))
                    throw new ArgumentException("Invalid Divisor value...");
                divisor = value;
            }
        }

        public int Bias { get; set; }

        public double[,] Mtx
        {
            get { return mtx; }
            set
            {
                if ((value.GetLength(0) == value.GetLength(1)) //square matrix?
                    && (value.GetLength(0)%2 == 1) // of odd dimension
                    && (value.GetLength(0) >= 3)) //and atleast of size 3
                {
                    mtx = value;
                    N = mtx.GetLength(0);
                }
                else throw new ArgumentException("Invalid Convolution Matrix...");
            }
        }

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap srcBmp)
        {
            return ApplyFilter(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
        }


        public Bitmap ApplyFilter(Bitmap srcBmp, Rectangle r)
        {
            if (srcBmp.PixelFormat == PixelFormat.Format24bppRgb || srcBmp.PixelFormat == PixelFormat.Format32bppArgb)
            {
                int step = (srcBmp.PixelFormat == PixelFormat.Format32bppArgb) ? 4 : 3;
                var result = new Bitmap(r.Width, r.Height, srcBmp.PixelFormat);

                unsafe
                {
                    BitmapData srcData = srcBmp.LockBits(r, ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                    var pSrc = (byte*) srcData.Scan0;

                    BitmapData dstData = result.LockBits(new Rectangle(0, 0, r.Width, r.Height), ImageLockMode.WriteOnly,
                                                         result.PixelFormat);
                    var pDst = (byte*) dstData.Scan0;

                    int w = r.Width;
                    int h = r.Height;
                    int len = w*h*step;
                    int radius = N/2;

                    int x, y, i, j, k;
                    double oR, oG, oB;


                    for (x = 0; x < w; x++)
                    {
                        for (y = 0; y < h; y++)
                        {
                            oR = oB = oG = 0;
                            for (i = 0; i < N; i++)
                            {
                                int ir = i - radius;
                                int _y = y + ir;

                                if (_y < 0) continue;
                                if (_y >= h) break;

                                for (j = 0; j < N; j++)
                                {
                                    int jr = j - radius;
                                    int _x = x + jr;

                                    if (_x < 0) continue;
                                    if (_x >= h) break;

                                    k = ((_y*w) + _x)*step;
                                    double m = mtx[i, j];
                                    oB += pSrc[k + 0]*m;
                                    oG += pSrc[k + 1]*m;
                                    oR += pSrc[k + 2]*m;
                                }
                            }
                            k = ((y*w) + x)*step;
                            oB = (int) (oB/divisor);
                            oG = (int) (oG/divisor);
                            oR = (int) (oR/divisor);

                            if (double.IsNaN(oB)) oB = pSrc[k + 0];
                            if (double.IsNaN(oG)) oG = pSrc[k + 1];
                            if (double.IsNaN(oR)) oR = pSrc[k + 2];


                            pDst[k + 0] = (byte) ((oB < 0) ? 0 : ((oB > 255) ? 255 : oB));
                            pDst[k + 1] = (byte) ((oG < 0) ? 0 : ((oG > 255) ? 255 : oG));
                            pDst[k + 2] = (byte) ((oR < 0) ? 0 : ((oR > 255) ? 255 : oR));
                            if (step == 4) pDst[k + 3] = pSrc[k + 3];
                        }
                    }

                    srcBmp.UnlockBits(srcData);
                    result.UnlockBits(dstData);
                } //unsafe ends
                return result;
            }
            else
                return null;
        }

        #endregion
    }
}