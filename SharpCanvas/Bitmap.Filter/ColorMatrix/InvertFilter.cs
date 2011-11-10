using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class InvertFilter : BaseFilter
    {
        public InvertFilter()
        {
            currentMtx = new double[20]
                             {
                                 -1, 0, 0, 0, 255,
                                 0, -1, 0, 0, 255,
                                 0, 0, -1, 0, 255,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}