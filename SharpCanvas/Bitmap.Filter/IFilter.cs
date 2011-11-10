using System.Drawing;

namespace SharpCanvas.StandardFilter.FilterSet
{
    public interface IFilter
    {
        Bitmap ApplyFilter(Bitmap bmp);
        Bitmap ApplyFilter(Bitmap bmp, Rectangle r);
    }
}