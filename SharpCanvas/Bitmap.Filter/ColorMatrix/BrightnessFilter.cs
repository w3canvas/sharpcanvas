using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class BrightnessFilter : BaseFilter
    {
        public BrightnessFilter(double n)
        {
            SetValue(n);
        }

        public BrightnessFilter()
        {
        }

        public void SetValue(double n)
        {
            currentMtx = new double[20]
                             {
                                 1, 0, 0, 0, n,
                                 0, 1, 0, 0, n,
                                 0, 0, 1, 0, n,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}