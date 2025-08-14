using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpCanvas.Forms
{
    public unsafe class RawBitmap : IDisposable
    {
        private readonly byte* _begin;
        private readonly BitmapData _bitmapData;

        public RawBitmap(Bitmap originBitmap)
        {
            OriginBitmap = originBitmap;
            _bitmapData = OriginBitmap.LockBits(new Rectangle(0, 0, OriginBitmap.Width, OriginBitmap.Height),
                                                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            _begin = (byte*) (void*) _bitmapData.Scan0;
        }

        public byte* Begin
        {
            get { return _begin; }
        }

        public byte* this[int x, int y]
        {
            get { return _begin + y*(_bitmapData.Stride) + x*3; }
        }

        public byte* this[int x, int y, int offset]
        {
            get { return _begin + y*(_bitmapData.Stride) + x*3 + offset; }
        }

        public int Stride
        {
            get { return _bitmapData.Stride; }
        }

        public int Width
        {
            get { return _bitmapData.Width; }
        }

        public int Height
        {
            get { return _bitmapData.Height; }
        }

        public Bitmap OriginBitmap { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            OriginBitmap.UnlockBits(_bitmapData);
        }

        #endregion

        public void SetColor(int x, int y, Color color)
        {
            byte* p = this[x, y];
            p[0] = color.B;
            p[1] = color.G;
            p[2] = color.R;
        }

        public Color GetColor(int x, int y)
        {
            byte* p = this[x, y];

            return Color.FromArgb
                (
                p[2],
                p[1],
                p[0]
                );
        }

        public int GetOffset()
        {
            return _bitmapData.Stride - _bitmapData.Width*3;
        }
    }
}