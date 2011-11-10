//Displacement Filters
//November 2009, by st0le


using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    [ComVisible(true)]
    public class SphereFilter : IFilter
    {
        private readonly BaseDisplacementFilter filter = new BaseDisplacementFilter();

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
            var r = new Rectangle(0, 0, nWidth, nHeight);
            int centerX = r.X + nWidth/2;
            int centerY = r.Y + nHeight/2;

            int centerMax = Math.Max(centerX, centerY);

            double angle, rad;

            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    int xcoord = x - centerX;
                    int ycoord = y - centerY;

                    angle = Math.Atan2(ycoord, xcoord);
                    rad = Math.Sqrt(xcoord*xcoord + ycoord*ycoord);

                    double rad2 = rad*rad/centerMax;

                    map[x, y].X = (int) (centerX + (rad2*Math.Cos(angle)));
                    map[x, y].Y = (int) (centerY + (rad2*Math.Sin(angle)));
                }
            }
            return filter.applyDisplacementMapAbsolute(r, map);
        }

        #endregion
    }
}