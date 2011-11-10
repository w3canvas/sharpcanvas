using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class TintFilter : BaseFilter
    {
        public TintFilter(double n)
        {
            SetValue(n);
        }

        public TintFilter()
        {
        }

        public void SetValue(double n)
        {
            currentMtx = new double[20]
                             {
                                 1 + (n/256), 0, 0, 0, 0,
                                 0, 1 + (-n/256), 0, 0, 0,
                                 0, 0, 1 + (n/256), 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}