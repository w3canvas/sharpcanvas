using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Shared
{
    public interface IContextProxy
    {
        ICanvasRenderingContext2D GetRealObject();
    }
}
