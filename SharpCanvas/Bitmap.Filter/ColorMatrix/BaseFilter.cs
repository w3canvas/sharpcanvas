using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class BaseFilter : IFilter
    {
        private readonly double[] identityMtx = new double[]
                                                    {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0};

        protected double[] currentMtx = new double[4*5];

        protected double[] lumConstants = new[] {0.212671, 0.715160, 0.072169};

        public BaseFilter()
        {
            resetMatrix();
        }

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap srcBmp)
        {
            return ApplyFilter(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
        }


        public Bitmap ApplyFilter(Bitmap srcBmp, Rectangle rect)
        {
            if (srcBmp.PixelFormat == PixelFormat.Format32bppArgb)
            {
                var result = new Bitmap(rect.Width, rect.Height, srcBmp.PixelFormat);
                unsafe
                {
                    BitmapData srcData = srcBmp.LockBits(rect, ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                    var pSrc = (byte*) srcData.Scan0;

                    BitmapData dstData = result.LockBits(new Rectangle(0, 0, rect.Width, rect.Height),
                                                         ImageLockMode.WriteOnly, result.PixelFormat);
                    var pDst = (byte*) dstData.Scan0;

                    int n = rect.Width*rect.Height*4;

                    double a, r, g, b;
                    double A, R, G, B;
                    double[] m = currentMtx;
                    double m0 = m[0],
                           m1 = m[1],
                           m2 = m[2],
                           m3 = m[3],
                           m4 = m[4],
                           m5 = m[5],
                           m6 = m[6],
                           m7 = m[7],
                           m8 = m[8],
                           m9 = m[9],
                           m10 = m[10],
                           m11 = m[11],
                           m12 = m[12],
                           m13 = m[13],
                           m14 = m[14],
                           m15 = m[15],
                           m16 = m[16],
                           m17 = m[17],
                           m18 = m[18],
                           m19 = m[19];

                    for (int i = 0; i < n; i += 4)
                    {
                        r = pSrc[i];
                        g = pSrc[i + 1];
                        b = pSrc[i + 2];
                        a = pSrc[i + 3];

                        R = (r*m0) + (g*m1) + (b*m2) + (a*m3) + m4;
                        G = (r*m5) + (g*m6) + (b*m7) + (a*m8) + m9;
                        B = (r*m10) + (g*m11) + (b*m12) + (a*m13) + m14;
                        A = (r*m15) + (g*m16) + (b*m17) + (a*m18) + m19;


                        pDst[i + 0] = (byte) (R < 0 ? 0 : (R < 255 ? R : 255));
                        pDst[i + 1] = (byte) (G < 0 ? 0 : (G < 255 ? G : 255));
                        pDst[i + 2] = (byte) (B < 0 ? 0 : (B < 255 ? B : 255));
                        pDst[i + 3] = (byte) (A < 0 ? 0 : (A < 255 ? A : 255));
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

        public void resetMatrix()
        {
            for (int i = 0; i < 20; i++)
                currentMtx[i] = identityMtx[i];
        }
    }
}