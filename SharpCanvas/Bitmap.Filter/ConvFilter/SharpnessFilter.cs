//Convolution Filters
//August 2009, by st0le

using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ConvFilter
{
    [ComVisible(true)]
    public class SharpnessFilter : IFilter
    {
        private ConvolutionFilter filter;

        public SharpnessFilter() : this(1)
        {
        }

        public SharpnessFilter(double v)
        {
            SetValue(v);
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
            var mtx = new[,]
                          {
                              {-v, -v, -v},
                              {-v, 8*v + 1, -v},
                              {-v, -v, -v}
                          };
            filter = new ConvolutionFilter(mtx, 1, 0);
        }
    }
}