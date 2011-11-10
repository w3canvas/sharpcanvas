using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ConvFilter
{
    [ComVisible(true)]
    public class GaussianBlurFilter : IFilter
    {
        private ConvolutionFilter filter;
        private double sigma;

        public GaussianBlurFilter()
        {
        }

        public GaussianBlurFilter(double sig) : this(sig, 5)
        {
        }

        public GaussianBlurFilter(double sig, int size)
        {
            SetValue(size, sig);
        }

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap srcBmp)
        {
            return filter.ApplyFilter(srcBmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
        }


        public Bitmap ApplyFilter(Bitmap srcBmp, Rectangle r)
        {
            return filter.ApplyFilter(srcBmp, r);
        }

        #endregion

        public void SetValue(int size, double sig)
        {
            if (size%2 == 1)
            {
                sigma = sig;
                var mtx = new double[size,size];
                int radius = size/2;
                int x = -radius;
                for (int i = 0; i < size; i++, x++)
                {
                    int y = -radius;
                    for (int j = 0; j < size; y++, j++)
                    {
                        mtx[i, j] = fn(x, y);
                    }
                }

                filter = new ConvolutionFilter(mtx);
            }
            else
                throw new ArgumentException("Matrix Dimension should be Odd...");
        }

        private double fn(int x, int y)
        {
            return Math.Exp((x*x + y*y)/(-2*sigma*sigma))/(2*Math.PI*sigma*sigma);
        }
    }
}