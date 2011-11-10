using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class HueFilter : BaseFilter
    {
        public HueFilter(double n)
        {
            SetValue(n);
        }

        public HueFilter()
        {
        }

        public void SetValue(double n)
        {
            double R = lumConstants[0], G = lumConstants[1], B = lumConstants[2];
            n *= Math.PI/180;
            double cos = Math.Cos(n), sin = Math.Sin(n);

            currentMtx = new double[4*5]
                             {
                                 (R + (cos*(1 - R))) + (sin*-R), (G + (cos*-G)) + (sin*-G),
                                 (B + (cos*-B)) + (sin*(1 - B)), 0, 0,
                                 (R + (cos*-R)) + (sin*0.143), (G + (cos*(1 - G))) + (sin*0.14),
                                 (B + (cos*-B)) + (sin*-0.283), 0, 0,
                                 (R + (cos*-R)) + (sin*-(1 - R)), (G + (cos*-G)) + (sin*G),
                                 (B + (cos*(1 - B))) + (sin*B), 0, 0,
                                 0, 0, 0, 1, 0
                             };
        }
    }
}