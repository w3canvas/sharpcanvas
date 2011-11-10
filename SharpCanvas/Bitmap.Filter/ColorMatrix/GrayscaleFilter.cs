using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class GrayscaleFilter : BaseFilter
    {
        public GrayscaleFilter(double r, double g, double b)
        {
            SetValue(r, g, b);
        }

        public GrayscaleFilter()
        {
        }

        public void SetValue(double r, double g, double b)
        {
            currentMtx = new double[20]
                             {
                                 r, g, b, 0, 0,
                                 r, g, b, 0, 0,
                                 r, g, b, 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}