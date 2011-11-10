using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpCanvas.StandardFilter.FilterSet.ColorMatrix;
using SharpCanvas.StandardFilter.FilterSet.ConvFilter;
using SharpCanvas.StandardFilter.FilterSet.DisplacementFilter;

namespace SharpCanvas.StandardFilter.FilterSet
{
    [ComVisible(true)]
    public class FilterChain
    {
        private readonly List<IFilter> filterChain = new List<IFilter>();

        public FilterChain()
        {
            filterChain.Clear();
        }

        public FilterChain(IFilter filter)
        {
            filterChain.Add(filter);
        }

        public void resetChain()
        {
            filterChain.Clear();
        }

        public int AddFilter(IFilter filter)
        {
            filterChain.Add(filter);
            return filterChain.Count;
        }

        public IFilter AddFilterFromString(string filterName)
        {
            IFilter filter = null;
            switch (filterName.ToLowerInvariant())
            {
                case "alpha":
                    filter = new AlphaFilter();
                    break;
                case "blind":
                    filter = new BlindFilter();
                    break;
                case "brightness":
                    filter = new BrightnessFilter();
                    break;
                case "channel":
                    filter = new ChannelFilter();
                    break;
                case "contrast":
                    filter = new ContrastFilter();
                    break;
                case "duotone":
                    filter = new DuotoneFilter();
                    break;
                case "exposure":
                    filter = new ExposureFilter();
                    break;
                case "grayscale":
                    filter = new GrayscaleFilter();
                    break;
                case "hue":
                    filter = new HueFilter();
                    break;
                case "invert":
                    filter = new InvertFilter();
                    break;
                case "saturate":
                    filter = new SaturateFilter();
                    break;
                case "temp":
                    filter = new TempFilter();
                    break;
                case "threshold":
                    filter = new ThresholdFilter();
                    break;
                case "tint":
                    filter = new TintFilter();
                    break;
                case "convolution":
                    filter = new ConvolutionFilter();
                    break;
                case "edgedetect":
                    filter = new EdgeDetectFilter();
                    break;
                case "emboss":
                    filter = new EmbossFilter();
                    break;
                case "gaussian":
                    filter = new GaussianBlurFilter();
                    break;
                case "relief":
                    filter = new ReliefFilter();
                    break;
                case "sharpness":
                    filter = new SharpnessFilter();
                    break;
                case "basedisplacement":
                    filter = new BaseDisplacementFilter();
                    break;
                case "flip":
                    filter = new FlipFilter();
                    break;
                case "jitter":
                    filter = new JitterFilter();
                    break;
                case "pixelate":
                    filter = new PixelateFilter();
                    break;
                case "sphere":
                    filter = new SphereFilter();
                    break;
                case "timewrap":
                    filter = new TimeWrapFilter();
                    break;
                case "water":
                    filter = new WaterFilter();
                    break;
                default:
                    new Exception(string.Format("Filter '{0}' doesn't exists.", filterName));
                    break;
            }
            filterChain.Add(filter);
            return filter;
        }

        public int GetFilterCount()
        {
            return filterChain.Count;
        }

        public Bitmap ApplyFilters(Bitmap bmp)
        {
            Bitmap result = null;
            for (int i = 0; i < filterChain.Count; i++)
            {
                result = filterChain[i].ApplyFilter(bmp);
                bmp = result;
            }
            return result;
        }
    }
}