//Displacement Filters
//November 2009, by st0le


using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    [ComVisible(true)]
    public class JitterFilter : IFilter
    {
        private readonly BaseDisplacementFilter filter = new BaseDisplacementFilter();
        private int amount;

        public JitterFilter(int amt)
        {
            amount = amt;
        }

        public JitterFilter()
        {
        }

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap bmp)
        {
            return ApplyFilter(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }

        public Bitmap ApplyFilter(Bitmap bmp, Rectangle rect)
        {
            filter.inputBmp = bmp.Clone(rect, bmp.PixelFormat);

            int nWidth = filter.inputBmp.Width;
            int nHeight = filter.inputBmp.Height;
            var map = new Point[nWidth,nHeight];

            var r = new Random();

            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    int dx = r.Next(amount) - amount/2;
                    map[x, y].X = dx;
                    int dy = r.Next(amount) - amount/2;
                    map[x, y].Y = dy;
                }
            }

            return filter.applyDisplacementMapRelative(new Rectangle(0, 0, nWidth, nHeight), map);
        }

        #endregion

        public void setAmount(int amt)
        {
            amount = amt;
        }
    }
}