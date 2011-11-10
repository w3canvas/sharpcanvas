using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.Interop
{
    [ComVisible(true), ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b")]
    public interface IHTMLPaintSite
    {
        //    [] HRESULT InvalidatePainterInfo();
        void InvalidatePainterInfo();

        //    [] HRESULT InvalidateRect([in] RECT* prcInvalid);
        void InvalidateRect(
            ref tagRECT prcInvalid
            );

        //    [] HRESULT InvalidateRegion([in] HRGN rgnInvalid);
        void InvalidateRegion(
            IntPtr rgnInvalid
            );

        //    [] HRESULT GetDrawInfo([in] LONG lFlags,[out] HTML_PAINT_DRAW_INFO* pDrawInfo);
        void GetDrawInfo(
            int lFlags,
            [Out] out _HTML_PAINT_DRAW_INFO pDrawInfo
            );

        //    [] HRESULT TransformGlobalToLocal([in] POINT ptGlobal,[out] POINT* pptLocal);
        void TransformGlobalToLocal(
            tagPOINT ptGlobal,
            [Out] out tagPOINT pptLocal
            );

        //    [] HRESULT TransformLocalToGlobal([in] POINT ptLocal,[out] POINT* pptGlobal);
        void TransformLocalToGlobal(
            tagPOINT ptLocal,
            [Out] out tagPOINT pptGlobal
            );

        //    [] HRESULT GetHitTestCookie([out] LONG* plCookie);
        void GetHitTestCookie(
            [Out] out int plCookie
            );
    }
}