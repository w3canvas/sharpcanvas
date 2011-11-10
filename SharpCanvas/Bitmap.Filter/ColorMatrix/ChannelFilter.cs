using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class ChannelFilter : BaseFilter
    {
        public ChannelFilter(byte r, byte g, byte b, byte a)
        {
            SetValue(r, g, b, a);
        }

        public ChannelFilter()
        {
        }

        public void SetValue(byte r, byte g, byte b, byte a)
        {
            double R = ((r & 1) == 1 ? 1 : 0) + ((r & 2) == 2 ? 1 : 0) + ((r & 4) == 4 ? 1 : 0) + ((r & 8) == 8 ? 1 : 0);
            R = (R > 0) ? 1/R : R;
            double G = ((g & 1) == 1 ? 1 : 0) + ((g & 2) == 2 ? 1 : 0) + ((g & 4) == 4 ? 1 : 0) + ((g & 8) == 8 ? 1 : 0);
            G = (G > 0) ? 1/G : G;
            double B = ((b & 1) == 1 ? 1 : 0) + ((b & 2) == 2 ? 1 : 0) + ((b & 4) == 4 ? 1 : 0) + ((b & 8) == 8 ? 1 : 0);
            B = (B > 0) ? 1/B : B;
            double A = ((a & 1) == 1 ? 1 : 0) + ((a & 2) == 2 ? 1 : 0) + ((a & 4) == 4 ? 1 : 0) + ((a & 8) == 8 ? 1 : 0);
            A = (A > 0) ? 1/A : A;

            currentMtx = new double[20]
                             {
                                 (r & 1) == 1 ? R : 0, (r & 2) == 2 ? R : 0, (r & 4) == 4 ? R : 0, (r & 8) == 8 ? R : 0,
                                 0,
                                 (g & 1) == 1 ? G : 0, (g & 2) == 2 ? G : 0, (g & 4) == 4 ? G : 0, (g & 8) == 8 ? G : 0,
                                 0,
                                 (b & 1) == 1 ? B : 0, (b & 2) == 2 ? B : 0, (b & 4) == 4 ? B : 0, (b & 8) == 8 ? B : 0,
                                 0,
                                 (a & 1) == 1 ? A : 0, (a & 2) == 2 ? A : 0, (a & 4) == 4 ? A : 0, (a & 8) == 8 ? A : 0,
                                 0
                             };
        }
    }
}