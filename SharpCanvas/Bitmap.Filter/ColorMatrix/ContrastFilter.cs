using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class ContrastFilter : BaseFilter
    {
        public ContrastFilter(double n)
        {
            SetValue(n);
        }

        public ContrastFilter()
        {
        }

        public void SetValue(double n)
        {
            currentMtx = new double[20]
                             {
                                 ++ n, 0, 0, 0, 128*(1 - n),
                                 0, n, 0, 0, 128*(1 - n),
                                 0, 0, n, 0, 128*(1 - n),
                                 0, 0, 0, 1, 0
                             };
        }
    }
}