using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpCanvas.Interop
{
    [ComVisible(true), ComImport()]
    [TypeLibType(TypeLibTypeFlags.FDispatchable)]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3050f6ae-98b5-11cf-bb82-00aa00bdce0b")]
    public interface IHTMLRenderStyle
    {
        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTLINETHROUGHSTYLE), displaybind, bindable] HRESULT textLineThroughStyle([in] BSTR v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTLINETHROUGHSTYLE)]
        string textLineThroughStyle { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTUNDERLINESTYLE), displaybind, bindable] HRESULT textUnderlineStyle([in] BSTR v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTUNDERLINESTYLE)]
        string textUnderlineStyle { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTEFFECT), displaybind, bindable] HRESULT textEffect([in] BSTR v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTEFFECT)]
        string textEffect { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTCOLOR), displaybind, bindable] HRESULT textColor([in] VARIANT v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTCOLOR)]
        object textColor { set; get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTBACKGROUNDCOLOR), displaybind, bindable] HRESULT textBackgroundColor([in] VARIANT v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTBACKGROUNDCOLOR)]
        object textBackgroundColor { set; get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTDECORATIONCOLOR), displaybind, bindable] HRESULT textDecorationColor([in] VARIANT v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTDECORATIONCOLOR)]
        object textDecorationColor { set; get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_RENDERINGPRIORITY), displaybind, bindable] HRESULT renderingPriority([in] long v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_RENDERINGPRIORITY)]
        int renderingPriority { set; get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_DEFAULTTEXTSELECTION), displaybind, bindable] HRESULT defaultTextSelection([in] BSTR v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_DEFAULTTEXTSELECTION)]
        string defaultTextSelection { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        //    [propput, id(DISPID_IHTMLRENDERSTYLE_TEXTDECORATION), displaybind, bindable] HRESULT textDecoration([in] BSTR v);
        [DispId(HTMLDispIDs.DISPID_IHTMLRENDERSTYLE_TEXTDECORATION)]
        string textDecoration { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

    }

}