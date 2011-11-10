using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SharpCanvas.Host.Prototype;
using SharpCanvas.Shared;

namespace SharpCanvas.Interop
{
    public interface ICanvasProxy
    {
        [DispId(1)]
        global::SharpCanvas.Interop.IHTMLCanvasElement RealObject { get; }

        /// <summary>
        /// Returns the number of pixels that the upper left corner of the current element is offset to the left within the offsetParent node.
        /// </summary>
        [DispId(2)]
        int offsetLeft { get; set; }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        [DispId(3)]
        int offsetTop { get; set; }
    }
}
