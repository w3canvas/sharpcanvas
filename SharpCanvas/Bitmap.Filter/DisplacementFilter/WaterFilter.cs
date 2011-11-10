//Displacement Filters
//November 2009, by st0le

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.DisplacementFilter
{
    [ComVisible(true)]
    public class WaterFilter : IFilter
    {
        private readonly BaseDisplacementFilter filter = new BaseDisplacementFilter();
        private double waveMagnitude;

        public WaterFilter(double magnitude)
        {
            waveMagnitude = magnitude;
        }

        public WaterFilter()
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
            var r = new Rectangle(0, 0, nWidth, nHeight);

            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    var x_wave = (int) (waveMagnitude*Math.Cos(2*Math.PI*(y/(float) (nHeight/2))));
                    var y_wave = (int) (waveMagnitude*Math.Sin(2*Math.PI*(x/(float) (nWidth/2))));

                    map[x, y].X = x + x_wave;
                    map[x, y].Y = y + y_wave;
                }
            }
            return filter.applyDisplacementMapAbsolute(r, map);
        }

        #endregion

        public void setMagnitude(double m)
        {
            waveMagnitude = m;
        }
    }
}