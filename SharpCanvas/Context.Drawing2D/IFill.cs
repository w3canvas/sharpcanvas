using System;
using System.Drawing;

namespace SharpCanvas.Context.Drawing2D
{
    public interface IFill : ICloneable
    {
        Brush brush { get; set; }
        Color color { get; set; }
    }
}
