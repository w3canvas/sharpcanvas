using System.Drawing;

namespace SharpCanvas
{
    public interface IFilter
    {
        Bitmap ApplyFilter(Bitmap bmp);
        Bitmap ApplyFilter(Bitmap bmp, Rectangle r);
    }
}