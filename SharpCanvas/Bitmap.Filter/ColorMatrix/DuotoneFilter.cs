using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class DuotoneFilter : BaseFilter
    {
        public DuotoneFilter(uint o)
        {
            SetValue(o);
        }

        public DuotoneFilter()
        {
        }

        public void SetValue(uint o)
        {
            double a = (o >> 24)/255.0f, r = (o >> 16 & 0xFF)/255.0f, g = (o >> 8 & 0xFF)/255.0f, b = (o & 0xFF)/255.0f;

            double R = lumConstants[0], G = lumConstants[1], B = lumConstants[2];

            currentMtx = new double[20]
                             {
                                 (1 - a) + a*r*R, a*r*G, a*r*B, 0, 0,
                                 a*g*R, (1 - a) + a*g*G, a*g*B, 0, 0,
                                 a*b*R, a*b*G, (1 - a) + a*b*B, 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}