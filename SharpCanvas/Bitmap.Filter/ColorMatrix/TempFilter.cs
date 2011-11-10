using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class TempFilter : BaseFilter
    {
        public TempFilter(double n)
        {
            SetValue(n);
        }

        public TempFilter()
        {
        }

        public void SetValue(double n)
        {
            currentMtx = new double[20]
                             {
                                 1 + (n/256), 0, 0, 0, 0,
                                 0, 1, 0, 0, 0,
                                 0, 0, 1 + (-n/256), 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}