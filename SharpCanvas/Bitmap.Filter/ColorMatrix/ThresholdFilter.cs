using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class ThresholdFilter : BaseFilter
    {
        public ThresholdFilter(uint threshold, uint factor)
        {
            SetValue(factor, threshold);
        }

        public ThresholdFilter()
        {
        }

        public void SetValue(uint factor, uint threshold)
        {
            double R = lumConstants[0], G = lumConstants[1], B = lumConstants[2];

            currentMtx = new double[20]
                             {
                                 (R*factor), (G*factor), (B*factor), 0, (-(factor)*threshold),
                                 (R*factor), (G*factor), (B*factor), 0, (-(factor)*threshold),
                                 (R*factor), (G*factor), (B*factor), 0, (-(factor)*threshold),
                                 0, 0, 0, 1, 0
                             };
        }
    }
}