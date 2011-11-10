using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class SaturateFilter : BaseFilter
    {
        public SaturateFilter(double n)
        {
            SetValue(n);
        }

        public SaturateFilter()
        {
        }

        public void SetValue(double n)
        {
            double R = (1 - n)*lumConstants[0], G = (1 - n)*lumConstants[1], B = (1 - n)*lumConstants[2];

            currentMtx = new double[4*5]
                             {
                                 R + n, G, B, 0, 0,
                                 R, G + n, B, 0, 0,
                                 R, G, B + n, 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}