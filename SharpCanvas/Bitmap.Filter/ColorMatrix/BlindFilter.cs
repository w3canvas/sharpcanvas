using System.Runtime.InteropServices;

namespace SharpCanvas.StandardFilter.FilterSet.ColorMatrix
{
    [ComVisible(true)]
    public class BlindFilter : BaseFilter
    {
        #region BlindnessConstants enum

        public enum BlindnessConstants
        {
            Protanopia = 0,
            Protanomaly,
            Deuteranopia,
            Deuteranomaly,
            Tritanopia,
            Tritanomaly,
            Achromatopsia,
            Achromatomaly
        } ;

        #endregion

        public BlindFilter(BlindnessConstants constant)
        {
            SetValue((int) constant);
        }

        public BlindFilter()
        {
        }

        public void SetValue(int value)
        {
            switch ((BlindnessConstants) value)
            {
                case BlindnessConstants.Protanopia:
                    currentMtx = new double[20]
                                     {
                                         0.567, 0.433, 0, 0, 0, 0.558, 0.442, 0, 0, 0, 0, 0.242, 0.758, 0, 0, 0, 0, 0, 1, 0
                                     };
                    break;
                case BlindnessConstants.Protanomaly:
                    currentMtx = new double[20]
                                     {
                                         0.817, 0.183, 0, 0, 0, 0.333, 0.667, 0, 0, 0, 0, 0.125, 0.875, 0, 0, 0, 0, 0, 1, 0
                                     };
                    break;
                case BlindnessConstants.Deuteranopia:
                    currentMtx = new double[20]
                                     {0.625, 0.375, 0, 0, 0, 0.7, 0.3, 0, 0, 0, 0, 0.3, 0.7, 0, 0, 0, 0, 0, 1, 0};
                    break;
                case BlindnessConstants.Deuteranomaly:
                    currentMtx = new double[20]
                                     {0.8, 0.2, 0, 0, 0, 0.258, 0.742, 0, 0, 0, 0, 0.142, 0.858, 0, 0, 0, 0, 0, 1, 0};
                    break;
                case BlindnessConstants.Tritanopia:
                    currentMtx = new double[20]
                                     {0.95, 0.05, 0, 0, 0, 0, 0.433, 0.567, 0, 0, 0, 0.475, 0.525, 0, 0, 0, 0, 0, 1, 0};
                    break;
                case BlindnessConstants.Tritanomaly:
                    currentMtx = new double[20]
                                     {
                                         0.967, 0.033, 0, 0, 0, 0, 0.733, 0.267, 0, 0, 0, 0.183, 0.817, 0, 0, 0, 0, 0, 1, 0
                                     };
                    break;
                case BlindnessConstants.Achromatopsia:
                    currentMtx = new double[20]
                                     {
                                         0.299, 0.587, 0.114, 0, 0, 0.299, 0.587, 0.114, 0, 0, 0.299, 0.587, 0.114, 0, 0, 0
                                         , 0, 0, 1, 0
                                     };
                    break;
                case BlindnessConstants.Achromatomaly:
                    currentMtx = new double[20]
                                     {
                                         0.618, 0.320, 0.062, 0, 0, 0.163, 0.775, 0.062, 0, 0, 0.163, 0.320, 0.516, 0, 0, 0
                                         , 0, 0, 1, 0
                                     };
                    break;
            }
        }
    }
}