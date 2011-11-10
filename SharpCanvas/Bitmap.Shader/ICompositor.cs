using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace SharpCanvas.ShaderFilter
{
    public interface ICompositor
    {
        Brush Input2 { get; set; }
    }
}
