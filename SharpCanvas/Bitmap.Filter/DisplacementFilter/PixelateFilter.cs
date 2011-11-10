//Displacement Filters
//November 2009, by st0le


using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    [ComVisible(true)]
    public class PixelateFilter : IFilter
    {
        private readonly BaseDisplacementFilter filter = new BaseDisplacementFilter();
        private int gridLen;

        public PixelateFilter(int gLen)
        {
            gridLen = gLen;
        }

        public PixelateFilter()
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

            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    int x0 = gridLen - x%gridLen;
                    int y0 = gridLen - y%gridLen;

                    map[x, y].X = x0;
                    map[x, y].Y = y0;
                }
            }

            return filter.applyDisplacementMapRelative(new Rectangle(0, 0, nWidth, nHeight), map);
        }

        #endregion

        public void setGridLength(int len)
        {
            gridLen = len;
        }
    }
}