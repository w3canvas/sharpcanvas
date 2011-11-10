using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class AlphaFilter : BaseFilter
    {
        public AlphaFilter(double n)
        {
            SetValue(n);
        }

        public AlphaFilter()
        {
        }

        public void SetValue(double n)
        {
            currentMtx = new double[20]
                             {
                                 1, 0, 0, 0, 0,
                                 0, 1, 0, 0, 0,
                                 0, 0, 1, 0, 0,
                                 0, 0, 0, n, 0
                             };
        }
    }
}