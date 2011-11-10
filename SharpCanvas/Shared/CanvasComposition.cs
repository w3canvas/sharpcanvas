using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpCanvas
{
    public enum CompositeMode
    {
        XOR = 0,
        SourceOver,
        DestinationOver,
        SourceOut,
        DestinationOut,
        Copy,
        SourceIn,
        DestinationIn,
        SourceATop,
        DestinationATop,
        Lighter,
        Darker,
        ChromeSourceIn,
        ChromeSourceOut,
        ChromeDestinationIn,
        ChromeDestinationATop
    } ;

    public class Compositer
    {
        private CompositeMode currentMode = CompositeMode.Copy;

        public Compositer()
        {
            currentMode = CompositeMode.Copy;
        }

        public void setCompositeMode(CompositeMode p)
        {
            currentMode = p;
        }

        public CompositeMode getCompositeMode()
        {
            return currentMode;
        }

        public void blendImages(Bitmap src1, Bitmap src2)
        {
            blendImages(src1, src2, new Rectangle(0, 0, src1.Width, src1.Height));
        }

        public void blendImages(Bitmap src1, Bitmap src2, Rectangle r)
        {
            if (
                (src1.PixelFormat == PixelFormat.Format32bppArgb) &&
                (src2.PixelFormat == PixelFormat.Format32bppArgb)
                )
            {
                if (r.X >= src1.Width || r.X < 0) return;
                if (r.Y >= src1.Height || r.Y < 0) return;

                r.Width = Math.Min(r.Width, src2.Width);
                r.Height = Math.Min(r.Height, src2.Height);

                if (r.X + r.Width > src1.Width) r.Width = src1.Width - r.X;
                if (r.Y + r.Height > src1.Height) r.Height = src1.Height - r.Y;

                unsafe
                {
                    BitmapData src1Data = src1.LockBits(r, ImageLockMode.ReadWrite, src1.PixelFormat);
                    var pSrc = (byte*) src1Data.Scan0;

                    BitmapData src2Data = src2.LockBits(r, ImageLockMode.ReadOnly, src2.PixelFormat);
                    var pSrc2 = (byte*) src2Data.Scan0;

                    int x, y;
                    int w = r.Width;
                    int h = r.Height;
                    int k = 0;
                    for (x = 0; x < w; x++)
                    {
                        for (y = 0; y < h; y++)
                        {
                            uint p1 = *(uint*) &pSrc[k];
                            uint p2 = *(uint*) &pSrc2[k];

                            uint p3 = blend(p1, p2);

                            *(uint*) &pSrc[k] = p3;

                            k += 4;
                        }
                    }
                    src1.UnlockBits(src1Data);
                    src2.UnlockBits(src2Data);
                } //unsafe
            }
        }

        private uint Lighten(uint col1, uint col2)
        {
            double a1 = (double) ((col1 >> 24) & 0xFF)/255.0f;
            var r1 = (byte) ((col1 >> 16) & 0xFF);
            var g1 = (byte) ((col1 >> 8) & 0xFF);
            var b1 = (byte) ((col1) & 0xFF);

            double a2 = (double) ((col2 >> 24) & 0xFF)/255.0f;
            var r2 = (byte) ((col2 >> 16) & 0xFF);
            var g2 = (byte) ((col2 >> 8) & 0xFF);
            var b2 = (byte) ((col2) & 0xFF);


            var a3 = (byte) ((a2 + a1*(1 - a2))*255.0f);
            byte r3 = (Math.Max(r1, r2));
            byte g3 = (Math.Max(g1, g2));
            byte b3 = (Math.Max(b1, b2));

            var ret = (uint) (a3 << 24 | r3 << 16 | g3 << 8 | b3);
            return ret;
        }


        private uint Darken(uint col1, uint col2)
        {
            double a1 = (double) ((col1 >> 24) & 0xFF)/255.0f;
            var r1 = (byte) ((col1 >> 16) & 0xFF);
            var g1 = (byte) ((col1 >> 8) & 0xFF);
            var b1 = (byte) ((col1) & 0xFF);

            double a2 = (double) ((col2 >> 24) & 0xFF)/255.0f;
            var r2 = (byte) ((col2 >> 16) & 0xFF);
            var g2 = (byte) ((col2 >> 8) & 0xFF);
            var b2 = (byte) ((col2) & 0xFF);


            var a3 = (byte) ((a2 + a1*(1 - a2))*255.0f);
            byte r3 = (Math.Min(r1, r2));
            byte g3 = (Math.Min(g1, g2));
            byte b3 = (Math.Min(b1, b2));

            var ret = (uint) (a3 << 24 | r3 << 16 | g3 << 8 | b3);
            return ret;
        }

        private uint MixColors(uint col1, uint col2)
        {
            double a1 = (double) ((col1 >> 24) & 0xFF)/255.0f;
            double r1 = (double) ((col1 >> 16) & 0xFF)/255.0f;
            double g1 = (double) ((col1 >> 8) & 0xFF)/255.0f;
            double b1 = (double) ((col1) & 0xFF)/255.0f;

            double a2 = (double) ((col2 >> 24) & 0xFF)/255.0f;
            double r2 = (double) ((col2 >> 16) & 0xFF)/255.0f;
            double g2 = (double) ((col2 >> 8) & 0xFF)/255.0f;
            double b2 = (double) ((col2) & 0xFF)/255.0f;


            double a3 = a2 + a1*(1 - a2);
            var r3 = (byte) (((r2*a2 + r1*a1*(1 - a2))*255.0f)/a3);
            var g3 = (byte) (((g2*a2 + g1*a1*(1 - a2))*255.0f)/a3);
            var b3 = (byte) (((b2*a2 + b1*a1*(1 - a2))*255.0f)/a3);

            var af = (byte) (a3*255.0f);
            var ret = (uint) (af << 24 | r3 << 16 | g3 << 8 | b3);
            return ret;
        }

        private uint blend(uint pixelValue1, uint pixelValue2)
        {
            var a1 = (byte) ((pixelValue1 >> 24) & 0xFF);
            var a2 = (byte) ((pixelValue2 >> 24) & 0xFF);
            double r1 = (double) ((pixelValue2 >> 16) & 0xFF)/255.0f;
            double g1 = (double) ((pixelValue2 >> 8) & 0xFF)/255.0f;
            double b1 = (double) ((pixelValue2) & 0xFF)/255.0f;
            bool transparent1 = (a1 <= 120.0f);
            bool transparent2 = (a2 <= 120.0f);

            switch (currentMode)
            {
                case CompositeMode.XOR:
                    if (!transparent1 && transparent2)
                        return pixelValue1;
                    else if (transparent1 && !transparent2)
                        return (pixelValue2);
                    else return 0;

                case CompositeMode.Copy:
                    if (!transparent1 && transparent2)
                        return (pixelValue1);
                    else return pixelValue2;

                case CompositeMode.Lighter:
                    if (!transparent1 && !transparent2)
                        return (Lighten(pixelValue1, pixelValue2));
                    else if (transparent1 && !transparent2)
                        return (pixelValue2);
                    else if (!transparent1 && transparent2)
                        return (pixelValue1);
                    else
                        return (0);

                case CompositeMode.Darker:
                    if (!transparent1 && !transparent2)
                        return (Darken(pixelValue1, pixelValue2));
                    else if (transparent1 && !transparent2)
                        return (pixelValue2);
                    else if (!transparent1 && transparent2)
                        return (pixelValue1);
                    else
                        return (0);

                case CompositeMode.SourceOver:
                    return MixColors(pixelValue1, pixelValue2);

                case CompositeMode.DestinationOver:
                    return MixColors(pixelValue2, pixelValue1);

                case CompositeMode.ChromeSourceIn:
                    if (!transparent1 && transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        if (!transparent1)
                        {
                            return (pixelValue2);
                        }
                        else
                        {
                            return (0);
                        }
                    }

                case CompositeMode.SourceIn:
                    if (!transparent1 && !transparent2)
                    {
                        return (pixelValue2);
                    }
                    else
                    {
                        return (0);
                    }

                case CompositeMode.ChromeDestinationIn:
                    if (!transparent1 && transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        if (!transparent2)
                            return (pixelValue1);
                        else
                            return (0);
                    }

                case CompositeMode.DestinationIn:
                    if (!transparent1 && !transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        return (0);
                    }

                case CompositeMode.ChromeSourceOut:
                    if (!transparent1 && transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        if (transparent1)
                            return (pixelValue2);
                        else
                            return (0);
                    }

                case CompositeMode.SourceOut:
                    if (transparent1)
                        return (pixelValue2);
                    else
                        return (0);

                case CompositeMode.DestinationOut:
                    if (transparent2)
                        return (pixelValue1);
                    else
                        return (0);

                case CompositeMode.SourceATop:
                    if (!transparent1)
                        return MixColors(pixelValue1, pixelValue2);
                    else
                        return (0);

                case CompositeMode.ChromeDestinationATop:
                    if (!transparent1 && transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        if (!transparent2)
                            return (MixColors(pixelValue2, pixelValue1));
                        else
                            return (0);
                    }

                case CompositeMode.DestinationATop:
                    if (!transparent1 && !transparent2)
                    {
                        return (pixelValue1);
                    }
                    else
                    {
                        if (!transparent2)
                            return (MixColors(pixelValue2, pixelValue1));
                        else
                            return (0);
                    }

                default:
                    return 0;
            }
        }
    }
}