using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.Shared
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHTMLCanvasElement
    {
        [DispId(-2147418107)]
        int width { get; set; }

        [DispId(-2147418106)]
        int height { get; set; }

        [DispId(4000000)]
        string toDataURL([Optional, In] string type, params object[] args);

        [DispId(4000001)]
        object getContext([In] string contextId);

        [DispId(4000002)]
        void addEventListener(string type, Delegate listener, bool capture);

        [DispId(4000003)]
        void setAttribute(string name, object value);

        [DispId(4000004)]
        ICSSStyleDeclaration style { get; }

        //we need this property in order to allow CanvasRenderingContext2D to refresh its container
        //when necessary internal event occurs. Probably we can replace this property by event subscription later
        object PaintSite { get; set; }

        ICanvasProxy GetProxy();

        // window overlap workaround
        void RequestDraw();

        // bitmap pointer
        ICanvasRenderingContext2D getCanvas();
    }
}