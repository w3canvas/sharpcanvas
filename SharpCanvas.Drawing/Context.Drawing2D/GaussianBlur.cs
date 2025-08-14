using System.Drawing;

namespace SharpCanvas.Forms
{
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    public class GaussianBlur
    {
        private readonly int _radius = 6;
        private BlurType _blurType = BlurType.Both;
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;

        public GaussianBlur()
        {
            PreCalculateSomeStuff();
        }

        public GaussianBlur(int radius)
        {
            _radius = radius;
            PreCalculateSomeStuff();
        }

        private void PreCalculateSomeStuff()
        {
            int sz = _radius*2 + 1;
            _kernel = new int[sz];
            _multable = new int[sz,256];
            for (int i = 1; i <= _radius; i++)
            {
                int szi = _radius - i;
                int szj = _radius + i;
                _kernel[szj] = _kernel[szi] = (szi + 1)*(szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (int j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj]*j;
                }
            }
            _kernel[_radius] = (_radius + 1)*(_radius + 1);
            _kernelSum += _kernel[_radius];
            for (int j = 0; j < 256; j++)
            {
                _multable[_radius, j] = _kernel[_radius]*j;
            }
        }

        public Bitmap ProcessImage(System.Drawing.Image inputImage, Color color)
        {
            var origin = new Bitmap(inputImage);
            var blurred = new Bitmap(inputImage.Width, inputImage.Height);

            using (var src = new RawBitmap(origin))
            {
                using (var dest = new RawBitmap(blurred))
                {
                    int pixelCount = src.Width*src.Height;
                    var b = new int[pixelCount];
                    var g = new int[pixelCount];
                    var r = new int[pixelCount];

                    var b2 = new int[pixelCount];
                    var g2 = new int[pixelCount];
                    var r2 = new int[pixelCount];

                    int offset = src.GetOffset();
                    int index = 0;
                    unsafe
                    {
                        byte* ptr = src.Begin;
                        for (int i = 0; i < src.Height; i++)
                        {
                            for (int j = 0; j < src.Width; j++)
                            {
                                if (*(ptr + 3) != 255 || *(ptr + 2) != 255 || *(ptr + 1) != 255 || *(ptr) != 255)
                                {
                                    b[index] = color.B;
                                    g[index] = color.G;
                                    r[index] = color.R;
                                    ptr += 3;
                                }
                                else
                                {
                                    b[index] = *ptr;
                                    ptr++;
                                    g[index] = *ptr;
                                    ptr++;
                                    r[index] = *ptr;
                                    ptr++;
                                }

                                ++index;
                            }
                            ptr += offset;
                        }

                        int bsum;
                        int gsum;
                        int rsum;
                        int sum;
                        int read;
                        int start = 0;
                        index = 0;
                        if (_blurType != BlurType.VerticalOnly)
                        {
                            for (int i = 0; i < src.Height; i++)
                            {
                                for (int j = 0; j < src.Width; j++)
                                {
                                    bsum = gsum = rsum = sum = 0;
                                    read = index - _radius;

                                    for (int z = 0; z < _kernel.Length; z++)
                                    {
                                        if (read >= start && read < start + src.Width)
                                        {
                                            bsum += _multable[z, b[read]];
                                            gsum += _multable[z, g[read]];
                                            rsum += _multable[z, r[read]];
                                            sum += _kernel[z];
                                        }
                                        ++read;
                                    }

                                    b2[index] = (bsum/sum);
                                    g2[index] = (gsum/sum);
                                    r2[index] = (rsum/sum);

                                    if (_blurType == BlurType.HorizontalOnly)
                                    {
                                        byte* pcell = dest[j, i];
                                        pcell[0] = (byte) (bsum/sum);
                                        pcell[1] = (byte) (gsum/sum);
                                        pcell[2] = (byte) (rsum/sum);
                                    }

                                    ++index;
                                }
                                start += src.Width;
                            }
                        }
                        if (_blurType == BlurType.HorizontalOnly)
                        {
                            return blurred;
                        }

                        int tempy;
                        for (int i = 0; i < src.Height; i++)
                        {
                            int y = i - _radius;
                            start = y*src.Width;
                            for (int j = 0; j < src.Width; j++)
                            {
                                bsum = gsum = rsum = sum = 0;
                                read = start + j;
                                tempy = y;
                                for (int z = 0; z < _kernel.Length; z++)
                                {
                                    if (tempy >= 0 && tempy < src.Height)
                                    {
                                        if (_blurType == BlurType.VerticalOnly)
                                        {
                                            bsum += _multable[z, b[read]];
                                            gsum += _multable[z, g[read]];
                                            rsum += _multable[z, r[read]];
                                        }
                                        else
                                        {
                                            bsum += _multable[z, b2[read]];
                                            gsum += _multable[z, g2[read]];
                                            rsum += _multable[z, r2[read]];
                                        }
                                        sum += _kernel[z];
                                    }
                                    read += src.Width;
                                    ++tempy;
                                }

                                byte* pcell = dest[j, i];
                                pcell[0] = (byte) (bsum/sum);
                                pcell[1] = (byte) (gsum/sum);
                                pcell[2] = (byte) (rsum/sum);
                            }
                        }
                    }
                }
            }

            return blurred;
        }
    }
}