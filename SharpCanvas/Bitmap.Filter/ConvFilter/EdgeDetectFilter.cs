//Convolution Filters
//August 2009, by st0le

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ConvFilter
{
    [ComVisible(true)]
    public class EdgeDetectFilter : IFilter
    {
        private ConvolutionFilter filter;

        public EdgeDetectFilter(double v)
        {
            SetValue(v);
        }

        public EdgeDetectFilter()
        {
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

        public void SetValue(double v)
        {
            double r = (v/360*Math.PI)*2, dr = Math.PI/4;

            var mtx = new[,]
                          {
                              {Math.Cos(r + dr), Math.Cos(r + 2*dr), Math.Cos(r + 3*dr)},
                              {Math.Cos(r), 0, Math.Cos(r + 4*dr)},
                              {Math.Cos(r - dr), Math.Cos(r - 2*dr), Math.Cos(r - 3*dr)}
                          };
            filter = new ConvolutionFilter(mtx, 1, 0);
        }
    }
}