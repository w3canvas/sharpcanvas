using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class ExposureFilter : BaseFilter
    {
        public ExposureFilter(double n)
        {
            SetValue(n);
        }

        public ExposureFilter()
        {
        }

        public void SetValue(double n)
        {
            n = n + 1;
            if (n < 1) n = (n + 1.5)/3.0;
            else n = 1 + ((n - 1)*1/2);

            currentMtx = new double[20]
                             {
                                 n, 0, 0, 0, 0,
                                 0, n, 0, 0, 0,
                                 0, 0, n, 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}