using System;
using System.Drawing;

namespace SharpCanvas.Forms
{
    public interface IFill : ICloneable
    {
        Brush brush { get; set; }
        Color color { get; set; }
    }
}