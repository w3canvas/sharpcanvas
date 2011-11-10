using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.Interop
{
    [ComImport, ComVisible(true), Guid("3050f28b-98b5-11cf-bb82-00aa00bdce0b")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
    TypeLibType(TypeLibTypeFlags.FDispatchable)]
    public interface IHTMLScriptElement
    {
        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_SRC)]
        string src
        { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_HTMLFOR)]
        string htmlFor { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_EVENT)]
        string scriptevent { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_TEXT)]
        string text { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_DEFER)]
        bool defer { set; [return: MarshalAs(UnmanagedType.VariantBool)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_READYSTATE)]
        string readyState { [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_ONERROR)]
        object onerror { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLSCRIPTELEMENT_TYPE)]
        string type { set; [return: MarshalAs(UnmanagedType.BStr)] get; }
    }
}
