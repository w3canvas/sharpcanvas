//Displacement Filters
//November 2009, by st0le


using System.Drawing;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    public class FlipFilter : IFilter
    {
        private readonly BaseDisplacementFilter filter = new BaseDisplacementFilter();

        private bool bHorizontal;
        private bool bVertical;

        public FlipFilter(bool bVert, bool bHor)
        {
            SetValue(bVert, bHor);
        }

        public FlipFilter()
        {
        }

        #region IFilter Members

        public Bitmap ApplyFilter(Bitmap bmp)
        {
            return ApplyFilter(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }

        public Bitmap ApplyFilter(Bitmap bmp, Rectangle r)
        {
            filter.inputBmp = bmp.Clone(r, bmp.PixelFormat);

            int nWidth = filter.inputBmp.Width;
            int nHeight = filter.inputBmp.Height;
            var map = new Point[nWidth,nHeight];

            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    map[x, y].X = (bHorizontal) ? nWidth - (x + 1) : x;
                    map[x, y].Y = (bVertical) ? nHeight - (y + 1) : y;
                }
            }

            return filter.applyDisplacementMapAbsolute(new Rectangle(0, 0, nWidth, nHeight), map);
        }

        #endregion

        public void SetValue(bool bVert, bool bHor)
        {
            bVertical = bVert;
            bHorizontal = bHor;
        }

        public void setOrientation(bool bVert, bool bHor)
        {
            bVertical = bVert;
            bHorizontal = bHor;
        }
    }
}