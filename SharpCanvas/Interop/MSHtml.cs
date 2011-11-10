using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

namespace SharpCanvas.Interop
{
    [ComImport]
    [Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHTMLElement
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417611)]
        void setAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                          [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue,
                          [In, Optional, DefaultParameterValue(1)] int lFlags);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417610)]
        object getAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                            [In, Optional, DefaultParameterValue(0)] int lFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417609)]
        bool removeAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                             [In, Optional, DefaultParameterValue(1)] int lFlags);

        [DispId(-2147417111)]
        string className { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 4),
         DispId(-2147417111)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417111),
         TypeLibFunc((short) 4)]
        get; }

        [DispId(-2147417110)]
        string id { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417110),
         TypeLibFunc((short) 4)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 4),
         DispId(-2147417110)]
        get; }

        [DispId(-2147417108)]
        string tagName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417108)]
        get; }

        [DispId(-2147418104)]
        IHTMLElement parentElement { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418104)]
        get; }

        [DispId(-2147418038)]
        IHTMLStyle style { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418038),
         TypeLibFunc((short) 0x400)]
        get; }

        [DispId(-2147412099)]
        object onhelp { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412099),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412099),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412104)]
        object onclick { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412104),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412104)]
        get; }

        [DispId(-2147412103)]
        object ondblclick { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412103),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412103),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412107)]
        object onkeydown { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412107)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412107),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412106)]
        object onkeyup { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412106),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412106),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412105)]
        object onkeypress { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412105),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412105),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412111)]
        object onmouseout { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412111),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412111),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412112)]
        object onmouseover { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412112)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412112),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412108)]
        object onmousemove { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412108),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412108),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412110)]
        object onmousedown { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412110),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412110)]
        get; }

        [DispId(-2147412109)]
        object onmouseup { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412109),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412109)]
        get; }

        [DispId(-2147417094)]
        object document { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417094)]
        get; }

        [DispId(-2147418043)]
        string title { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418043)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418043),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413012)]
        string language { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413012),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413012)]
        get; }

        [DispId(-2147412075)]
        object onselectstart { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412075),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412075)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417093)]
        void scrollIntoView([In, Optional, MarshalAs(UnmanagedType.Struct)] object varargStart);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417092)]
        bool contains([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement pChild);

        [DispId(-2147417088)]
        int sourceIndex { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417088),
                           TypeLibFunc((short) 4)]
        get; }

        [DispId(-2147417087)]
        object recordNumber { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417087)]
        get; }

        [DispId(-2147413103)]
        string lang { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413103)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413103)]
        get; }

        [DispId(-2147417104)]
        int offsetLeft { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417104)]
        get; }

        [DispId(-2147417103)]
        int offsetTop { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417103)]
        get; }

        [DispId(-2147417102)]
        int offsetWidth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417102)]
        get; }

        [DispId(-2147417101)]
        int offsetHeight { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417101)]
        get; }

        [DispId(-2147417100)]
        IHTMLElement offsetParent { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417100)]
        get; }

        [DispId(-2147417086)]
        string innerHTML { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417086)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417086)]
        get; }

        [DispId(-2147417085)]
        string innerText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417085)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417085)]
        get; }

        [DispId(-2147417084)]
        string outerHTML { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417084)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417084)]
        get; }

        [DispId(-2147417083)]
        string outerText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417083)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417083)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417082)]
        void insertAdjacentHTML([In, MarshalAs(UnmanagedType.BStr)] string where,
                                [In, MarshalAs(UnmanagedType.BStr)] string html);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417081)]
        void insertAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where,
                                [In, MarshalAs(UnmanagedType.BStr)] string text);

        [DispId(-2147417080)]
        IHTMLElement parentTextEdit { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417080)]
        get; }

        [DispId(-2147417078)]
        bool isTextEdit { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417078)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417079)]
        void click();

        [DispId(-2147417077)]
        IHTMLFiltersCollection filters { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417077)]
        get; }

        [DispId(-2147412077)]
        object ondragstart { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412077),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412077)]
        get; }

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417076)]
        string toString();

        [DispId(-2147412091)]
        object onbeforeupdate { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412091),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412091)]
        get; }

        [DispId(-2147412090)]
        object onafterupdate { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412090)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412090),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412074)]
        object onerrorupdate { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412074),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412074)]
        get; }

        [DispId(-2147412094)]
        object onrowexit { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412094),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412094)]
        get; }

        [DispId(-2147412093)]
        object onrowenter { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412093)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412093),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412072)]
        object ondatasetchanged { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412072),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412072),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412071)]
        object ondataavailable { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412071),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412071),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412070)]
        object ondatasetcomplete { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412070),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412070)]
        get; }

        [DispId(-2147412069)]
        object onfilterchange { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412069),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412069)]
        get; }

        [DispId(-2147417075)]
        object children { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417075)]
        get; }

        [DispId(-2147417074)]
        object all { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417074)]
        get; }
    }

    [ComImport]
    [Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHTMLElement2
    {
        [DispId(-2147417073)]
        string scopeName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417073)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417072)]
        void setCapture([In, Optional, DefaultParameterValue(true)] bool containerCapture);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417071)]
        void releaseCapture();

        [DispId(-2147412066)]
        object onlosecapture { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412066)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412066)]
        get; }

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417070)]
        string componentFromPoint([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417069)]
        void doScroll([In, Optional, MarshalAs(UnmanagedType.Struct)] object component);

        [DispId(-2147412081)]
        object onscroll { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412081)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412081)]
        get; }

        [DispId(-2147412063)]
        object ondrag { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412063),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412063),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412062)]
        object ondragend { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412062)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412062)]
        get; }

        [DispId(-2147412061)]
        object ondragenter { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412061)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412061)]
        get; }

        [DispId(-2147412060)]
        object ondragover { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412060),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412060)]
        get; }

        [DispId(-2147412059)]
        object ondragleave { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412059),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412059)]
        get; }

        [DispId(-2147412058)]
        object ondrop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412058)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412058)]
        get; }

        [DispId(-2147412054)]
        object onbeforecut { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412054),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412054)]
        get; }

        [DispId(-2147412057)]
        object oncut { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412057),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412057)]
        get; }

        [DispId(-2147412053)]
        object onbeforecopy { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412053),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412053)]
        get; }

        [DispId(-2147412056)]
        object oncopy { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412056),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412056)]
        get; }

        [DispId(-2147412052)]
        object onbeforepaste { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412052),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412052)]
        get; }

        [DispId(-2147412055)]
        object onpaste { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412055),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412055)]
        get; }

        [DispId(-2147417105)]
        IHTMLCurrentStyle currentStyle { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417105),
         TypeLibFunc((short) 0x400)]
        get; }

        [DispId(-2147412065)]
        object onpropertychange { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412065)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412065),
         TypeLibFunc((short) 20)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417068)]
        IHTMLRectCollection getClientRects();

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417067)]
        IHTMLRect getBoundingClientRect();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417608)]
        void setExpression([In, MarshalAs(UnmanagedType.BStr)] string propname,
                           [In, MarshalAs(UnmanagedType.BStr)] string expression,
                           [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string language);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417607)]
        object getExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417606)]
        bool removeExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

        [DispId(-2147418097)]
        short tabIndex { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418097),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418097),
              TypeLibFunc((short) 20)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416112)]
        void focus();

        [DispId(-2147416107)]
        string accessKey { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416107),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416107),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412097)]
        object onblur { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412097),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412097),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412098)]
        object onfocus { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412098),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412098),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412076)]
        object onresize { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412076),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412076),
         TypeLibFunc((short) 20)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416110)]
        void blur();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416095)]
        void addFilter([In, MarshalAs(UnmanagedType.IUnknown)] object pUnk);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416094)]
        void removeFilter([In, MarshalAs(UnmanagedType.IUnknown)] object pUnk);

        [DispId(-2147416093)]
        int clientHeight { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            TypeLibFunc((short) 20), DispId(-2147416093)]
        get; }

        [DispId(-2147416092)]
        int clientWidth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416092),
                           TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147416091)]
        int clientTop { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         TypeLibFunc((short) 20), DispId(-2147416091)]
        get; }

        [DispId(-2147416090)]
        int clientLeft { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147416090),
                          TypeLibFunc((short) 20)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417605)]
        bool attachEvent([In, MarshalAs(UnmanagedType.BStr)] string @event,
                         [In, MarshalAs(UnmanagedType.IDispatch)] object pdisp);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417604)]
        void detachEvent([In, MarshalAs(UnmanagedType.BStr)] string @event,
                         [In, MarshalAs(UnmanagedType.IDispatch)] object pdisp);

        [DispId(-2147412996)]
        object readyState { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412996)]
        get; }

        [DispId(-2147412087)]
        object onreadystatechange { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412087)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412087),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412050)]
        object onrowsdelete { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412050)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412050)]
        get; }

        [DispId(-2147412049)]
        object onrowsinserted { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412049),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412049)]
        get; }

        [DispId(-2147412048)]
        object oncellchange { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412048)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412048),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412995)]
        string dir { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412995)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412995),
         TypeLibFunc((short) 20)]
        get; }

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417056)]
        object createControlRange();

        [DispId(-2147417055)]
        int scrollHeight { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417055),
                            TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417054)]
        int scrollWidth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           TypeLibFunc((short) 20), DispId(-2147417054)]
        get; }

        [DispId(-2147417053)]
        int scrollTop { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417053),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147417053)]
        get; }

        [DispId(-2147417052)]
        int scrollLeft { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417052),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147417052)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417050)]
        void clearAttributes();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417049)]
        void mergeAttributes([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement mergeThis);

        [DispId(-2147412047)]
        object oncontextmenu { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412047),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412047),
         TypeLibFunc((short) 20)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417043)]
        IHTMLElement insertAdjacentElement([In, MarshalAs(UnmanagedType.BStr)] string where,
                                           [In, MarshalAs(UnmanagedType.Interface)] IHTMLElement insertedElement);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417047)]
        IHTMLElement applyElement([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement apply,
                                  [In, MarshalAs(UnmanagedType.BStr)] string where);

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417042)]
        string getAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where);

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417041)]
        string replaceAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where,
                                   [In, MarshalAs(UnmanagedType.BStr)] string newText);

        [DispId(-2147417040)]
        bool canHaveChildren { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417040)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417032)]
        int addBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
                        [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvarFactory);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417031)]
        bool removeBehavior([In] int cookie);

        [DispId(-2147417048)]
        IHTMLStyle runtimeStyle { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x400)
        , DispId(-2147417048)]
        get; }

        [DispId(-2147417030)]
        object behaviorUrns { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417030)]
        get; }

        [DispId(-2147417029)]
        string tagUrn { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417029)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417029)]
        get; }

        [DispId(-2147412043)]
        object onbeforeeditfocus { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412043)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412043),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417028)]
        int readyStateValue { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417028),
                               TypeLibFunc((short) 0x41)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417027)]
        IHTMLElementCollection getElementsByTagName([In, MarshalAs(UnmanagedType.BStr)] string v);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3050F563-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface DispHTMLGenericElement
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417611)]
        void setAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                          [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue,
                          [In, Optional, DefaultParameterValue(1)] int lFlags);

        [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417610)]
        object getAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                            [In, Optional, DefaultParameterValue(0)] int lFlags);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417609)]
        bool removeAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                             [In, Optional, DefaultParameterValue(1)] int lFlags);

        [DispId(-2147417111)]
        string className { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 4), DispId(-2147417111)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417111), TypeLibFunc((short) 4)]
        get; }

        [DispId(-2147417110)]
        string id { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 4), DispId(-2147417110)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 4), DispId(-2147417110)]
        get; }

        [DispId(-2147417108)]
        string tagName { [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417108)]
        get; }

        [DispId(-2147418104)]
        IHTMLElement parentElement { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147418104)]
        get; }

        [DispId(-2147418038)]
        IHTMLStyle style { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147418038), TypeLibFunc((short) 0x400)]
        get; }

        [DispId(-2147412099)]
        object onhelp { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412099), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412099)]
        get; }

        [DispId(-2147412104)]
        object onclick { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412104)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412104), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412103)]
        object ondblclick { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412103)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412103), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412107)]
        object onkeydown { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412107), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412107), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412106)]
        object onkeyup { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412106), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412106)]
        get; }

        [DispId(-2147412105)]
        object onkeypress { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412105)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412105)]
        get; }

        [DispId(-2147412111)]
        object onmouseout { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412111), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412111)]
        get; }

        [DispId(-2147412112)]
        object onmouseover { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412112), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412112)]
        get; }

        [DispId(-2147412108)]
        object onmousemove { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412108)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412108)]
        get; }

        [DispId(-2147412110)]
        object onmousedown { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412110), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412110)]
        get; }

        [DispId(-2147412109)]
        object onmouseup { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412109), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412109), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417094)]
        object document { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417094)]
        get; }

        [DispId(-2147418043)]
        string title { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147418043), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147418043), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413012)]
        string language { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147413012), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147413012), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412075)]
        object onselectstart { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412075), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412075), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417093)]
        void scrollIntoView([In, Optional, MarshalAs(UnmanagedType.Struct)] object varargStart);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417092)]
        bool contains([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement pChild);

        [DispId(-2147417088)]
        int sourceIndex { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           TypeLibFunc((short) 4), DispId(-2147417088)]
        get; }

        [DispId(-2147417087)]
        object recordNumber { [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417087)]
        get; }

        [DispId(-2147413103)]
        string lang { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147413103)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147413103)]
        get; }

        [DispId(-2147417104)]
        int offsetLeft { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                          DispId(-2147417104)]
        get; }

        [DispId(-2147417103)]
        int offsetTop { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         DispId(-2147417103)]
        get; }

        [DispId(-2147417102)]
        int offsetWidth { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           DispId(-2147417102)]
        get; }

        [DispId(-2147417101)]
        int offsetHeight { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147417101)]
        get; }

        [DispId(-2147417100)]
        IHTMLElement offsetParent { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417100)]
        get; }

        [DispId(-2147417086)]
        string innerHTML { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417086)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417086)]
        get; }

        [DispId(-2147417085)]
        string innerText { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417085)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417085)]
        get; }

        [DispId(-2147417084)]
        string outerHTML { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417084)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417084)]
        get; }

        [DispId(-2147417083)]
        string outerText { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417083)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417083)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417082)]
        void insertAdjacentHTML([In, MarshalAs(UnmanagedType.BStr)] string where,
                                [In, MarshalAs(UnmanagedType.BStr)] string html);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417081)]
        void insertAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where,
                                [In, MarshalAs(UnmanagedType.BStr)] string text);

        [DispId(-2147417080)]
        IHTMLElement parentTextEdit { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417080)]
        get; }

        [DispId(-2147417078)]
        bool isTextEdit { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           DispId(-2147417078)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417079)]
        void click();

        [DispId(-2147417077)]
        IHTMLFiltersCollection filters { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417077)]
        get; }

        [DispId(-2147412077)]
        object ondragstart { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412077), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412077), TypeLibFunc((short) 20)]
        get; }

        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417076)]
        string toString();

        [DispId(-2147412091)]
        object onbeforeupdate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412091), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412091), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412090)]
        object onafterupdate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412090), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412090), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412074)]
        object onerrorupdate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412074), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412074), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412094)]
        object onrowexit { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412094)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412094)]
        get; }

        [DispId(-2147412093)]
        object onrowenter { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412093), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412093)]
        get; }

        [DispId(-2147412072)]
        object ondatasetchanged { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412072)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412072), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412071)]
        object ondataavailable { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412071)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412071)]
        get; }

        [DispId(-2147412070)]
        object ondatasetcomplete { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412070)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412070)]
        get; }

        [DispId(-2147412069)]
        object onfilterchange { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412069), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412069)]
        get; }

        [DispId(-2147417075)]
        object children { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417075)]
        get; }

        [DispId(-2147417074)]
        object all { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417074)]
        get; }

        [DispId(-2147417073)]
        string scopeName { [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417073)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417072)]
        void setCapture([In, Optional, DefaultParameterValue(true)] bool containerCapture);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417071)]
        void releaseCapture();

        [DispId(-2147412066)]
        object onlosecapture { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412066), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412066)]
        get; }

        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417070)]
        string componentFromPoint([In] int x, [In] int y);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417069)]
        void doScroll([In, Optional, MarshalAs(UnmanagedType.Struct)] object component);

        [DispId(-2147412081)]
        object onscroll { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412081)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412081), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412063)]
        object ondrag { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412063)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412063), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412062)]
        object ondragend { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412062), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412062), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412061)]
        object ondragenter { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412061)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412061), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412060)]
        object ondragover { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412060)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412060)]
        get; }

        [DispId(-2147412059)]
        object ondragleave { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412059)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412059)]
        get; }

        [DispId(-2147412058)]
        object ondrop { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412058), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412058)]
        get; }

        [DispId(-2147412054)]
        object onbeforecut { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412054), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412054), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412057)]
        object oncut { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412057)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412057), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412053)]
        object onbeforecopy { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412053), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412053), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412056)]
        object oncopy { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412056), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412056)]
        get; }

        [DispId(-2147412052)]
        object onbeforepaste { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412052)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412052)]
        get; }

        [DispId(-2147412055)]
        object onpaste { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412055)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412055)]
        get; }

        [DispId(-2147417105)]
        IHTMLCurrentStyle currentStyle { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 0x400), DispId(-2147417105)]
        get; }

        [DispId(-2147412065)]
        object onpropertychange { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412065)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412065)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417068)]
        IHTMLRectCollection getClientRects();

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417067)]
        IHTMLRect getBoundingClientRect();

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417608)]
        void setExpression([In, MarshalAs(UnmanagedType.BStr)] string propname,
                           [In, MarshalAs(UnmanagedType.BStr)] string expression,
                           [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string language);

        [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417607)]
        object getExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417606)]
        bool removeExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

        [DispId(-2147418097)]
        short tabIndex { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                          DispId(-2147418097), TypeLibFunc((short) 20)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              DispId(-2147418097), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416112)]
        void focus();

        [DispId(-2147416107)]
        string accessKey { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416107), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147416107)]
        get; }

        [DispId(-2147412097)]
        object onblur { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412097), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412097)]
        get; }

        [DispId(-2147412098)]
        object onfocus { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412098)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412098), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412076)]
        object onresize { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412076)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412076), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416110)]
        void blur();

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416095)]
        void addFilter([In, MarshalAs(UnmanagedType.IUnknown)] object pUnk);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416094)]
        void removeFilter([In, MarshalAs(UnmanagedType.IUnknown)] object pUnk);

        [DispId(-2147416093)]
        int clientHeight { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147416093), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147416092)]
        int clientWidth { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           TypeLibFunc((short) 20), DispId(-2147416092)]
        get; }

        [DispId(-2147416091)]
        int clientTop { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         TypeLibFunc((short) 20), DispId(-2147416091)]
        get; }

        [DispId(-2147416090)]
        int clientLeft { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                          TypeLibFunc((short) 20), DispId(-2147416090)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417605)]
        bool attachEvent([In, MarshalAs(UnmanagedType.BStr)] string @event,
                         [In, MarshalAs(UnmanagedType.IDispatch)] object pdisp);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417604)]
        void detachEvent([In, MarshalAs(UnmanagedType.BStr)] string @event,
                         [In, MarshalAs(UnmanagedType.IDispatch)] object pdisp);

        [DispId(-2147412996)]
        object readyState { [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412996)]
        get; }

        [DispId(-2147412087)]
        object onreadystatechange { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412087)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412087), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412050)]
        object onrowsdelete { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412050), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412050), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412049)]
        object onrowsinserted { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412049)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412049)]
        get; }

        [DispId(-2147412048)]
        object oncellchange { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412048), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412048)]
        get; }

        [DispId(-2147412995)]
        string dir { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412995)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412995), TypeLibFunc((short) 20)]
        get; }

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417056)]
        object createControlRange();

        [DispId(-2147417055)]
        int scrollHeight { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147417055), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417054)]
        int scrollWidth { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           TypeLibFunc((short) 20), DispId(-2147417054)]
        get; }

        [DispId(-2147417053)]
        int scrollTop { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         TypeLibFunc((short) 20), DispId(-2147417053)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              DispId(-2147417053), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417052)]
        int scrollLeft { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                          TypeLibFunc((short) 20), DispId(-2147417052)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147417052)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417050)]
        void clearAttributes();

        [DispId(-2147412047)]
        object oncontextmenu { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412047)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412047)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417043)]
        IHTMLElement insertAdjacentElement([In, MarshalAs(UnmanagedType.BStr)] string where,
                                           [In, MarshalAs(UnmanagedType.Interface)] IHTMLElement insertedElement);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417047)]
        IHTMLElement applyElement([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement apply,
                                  [In, MarshalAs(UnmanagedType.BStr)] string where);

        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417042)]
        string getAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where);

        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417041)]
        string replaceAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where,
                                   [In, MarshalAs(UnmanagedType.BStr)] string newText);

        [DispId(-2147417040)]
        bool canHaveChildren { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                                DispId(-2147417040)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417032)]
        int addBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
                        [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvarFactory);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417031)]
        bool removeBehavior([In] int cookie);

        [DispId(-2147417048)]
        IHTMLStyle runtimeStyle { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417048), TypeLibFunc((short) 0x400)]
        get; }

        [DispId(-2147417030)]
        object behaviorUrns { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417030)]
        get; }

        [DispId(-2147417029)]
        string tagUrn { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417029)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417029)]
        get; }

        [DispId(-2147412043)]
        object onbeforeeditfocus { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412043)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412043), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417028)]
        int readyStateValue { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                               TypeLibFunc((short) 0x41), DispId(-2147417028)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417027)]
        IHTMLElementCollection getElementsByTagName([In, MarshalAs(UnmanagedType.BStr)] string v);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417016)]
        void mergeAttributes([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement mergeThis,
                             [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvarFlags);

        [DispId(-2147417015)]
        bool isMultiLine { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147417015)]
        get; }

        [DispId(-2147417014)]
        bool canHaveHTML { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147417014)]
        get; }

        [DispId(-2147412039)]
        object onlayoutcomplete { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412039)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412039)]
        get; }

        [DispId(-2147412038)]
        object onpage { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412038), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412038)]
        get; }

        [DispId(-2147417012)]
        bool inflateBlock { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                             DispId(-2147417012), TypeLibFunc((short) 0x441)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              DispId(-2147417012), TypeLibFunc((short) 0x441)]
        get; }

        [DispId(-2147412035)]
        object onbeforedeactivate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412035)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412035), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417011)]
        void setActive();

        [DispId(-2147412950)]
        string contentEditable { [param: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412950)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412950), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417010)]
        bool isContentEditable { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                                  DispId(-2147417010)]
        get; }

        [DispId(-2147412949)]
        bool hideFocus { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                          DispId(-2147412949), TypeLibFunc((short) 20)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              DispId(-2147412949), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418036)]
        bool disabled { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         TypeLibFunc((short) 20), DispId(-2147418036)]
        set; [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147418036)]
        get; }

        [DispId(-2147417007)]
        bool isDisabled { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                           DispId(-2147417007)]
        get; }

        [DispId(-2147412034)]
        object onmove { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412034)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412034)]
        get; }

        [DispId(-2147412033)]
        object oncontrolselect { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412033), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412033), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417006)]
        bool FireEvent([In, MarshalAs(UnmanagedType.BStr)] string bstrEventName,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvarEventObject);

        [DispId(-2147412029)]
        object onresizestart { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412029)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412029)]
        get; }

        [DispId(-2147412028)]
        object onresizeend { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412028), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412028)]
        get; }

        [DispId(-2147412031)]
        object onmovestart { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412031), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412031)]
        get; }

        [DispId(-2147412030)]
        object onmoveend { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412030)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412030), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412027)]
        object onmouseenter { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412027), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412027)]
        get; }

        [DispId(-2147412026)]
        object onmouseleave { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412026), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412026)]
        get; }

        [DispId(-2147412025)]
        object onactivate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412025)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412025), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412024)]
        object ondeactivate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412024), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412024), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417005)]
        bool dragDrop();

        [DispId(-2147417004)]
        int glyphMode { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                         TypeLibFunc((short) 0x441), DispId(-2147417004)]
        get; }

        [DispId(-2147412036)]
        object onmousewheel { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412036)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412036), TypeLibFunc((short) 20)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417000)]
        void normalize();

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417003)]
        IHTMLDOMAttribute getAttributeNode([In, MarshalAs(UnmanagedType.BStr)] string bstrName);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417002)]
        IHTMLDOMAttribute setAttributeNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMAttribute pattr);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417001)]
        IHTMLDOMAttribute removeAttributeNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMAttribute pattr);

        [DispId(-2147412022)]
        object onbeforeactivate { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412022)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 20), DispId(-2147412022)]
        get; }

        [DispId(-2147412021)]
        object onfocusin { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412021), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412021), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412020)]
        object onfocusout { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412020), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147412020), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417058)]
        int uniqueNumber { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                            DispId(-2147417058), TypeLibFunc((short) 0x40)]
        get; }

        [DispId(-2147417057)]
        string uniqueID { [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 0x40), DispId(-2147417057)]
        get; }

        [DispId(-2147417066)]
        int nodeType { [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
                        DispId(-2147417066)]
        get; }

        [DispId(-2147417065)]
        IHTMLDOMNode parentNode { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417065)]
        get; }

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417064)]
        bool hasChildNodes();

        [DispId(-2147417063)]
        object childNodes { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417063)]
        get; }

        [DispId(-2147417062)]
        object attributes { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417062)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417061)]
        IHTMLDOMNode insertBefore([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild,
                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object refChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417060)]
        IHTMLDOMNode removeChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417059)]
        IHTMLDOMNode replaceChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild,
                                  [In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417051)]
        IHTMLDOMNode cloneNode([In] bool fDeep);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417046)]
        IHTMLDOMNode removeNode([In, Optional, DefaultParameterValue(false)] bool fDeep);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417044)]
        IHTMLDOMNode swapNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode otherNode);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417045)]
        IHTMLDOMNode replaceNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode replacement);

        [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417039)]
        IHTMLDOMNode appendChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild);

        [DispId(-2147417038)]
        string nodeName { [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417038)]
        get; }

        [DispId(-2147417037)]
        object nodeValue { [param: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417037)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417037)]
        get; }

        [DispId(-2147417036)]
        IHTMLDOMNode firstChild { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417036)]
        get; }

        [DispId(-2147417035)]
        IHTMLDOMNode lastChild { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417035)]
        get; }

        [DispId(-2147417034)]
        IHTMLDOMNode previousSibling { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417034)]
        get; }

        [DispId(-2147417033)]
        IHTMLDOMNode nextSibling { [return: MarshalAs(UnmanagedType.Interface)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147417033)]
        get; }

        [DispId(-2147416999)]
        object ownerDocument { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         DispId(-2147416999)]
        get; }

        [DispId(0x3e9)]
        object recordset { [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
         TypeLibFunc((short) 0x40), DispId(0x3e9)]
        get; }

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)
        ]
        object namedRecordset([In, MarshalAs(UnmanagedType.BStr)] string dataMember,
                              [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object hierarchy);
    }

    [ComImport]
    [Guid("3050F4B0-98B5-11CF-BB82-00AA00BDCE0B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHTMLDOMAttribute
    {
        [DispId(0x3e8)]
        string nodeName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e8)]
        get; }

        [DispId(0x3ea)]
        object nodeValue { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3e9)]
        bool specified { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }
    }

    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
    [ComVisible(true)]
    [ComImport]
    public interface IHTMLDocument
    {
        [DispId(0x3e9)]
        object Script { [return: MarshalAs(UnmanagedType.IDispatch)]
        get; }
    }

    [Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
    [ComImport]
    [TypeLibType((short)4160)]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IHTMLDocument2
    {
        /// <summary><para><c>write</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>write</c> method was the following:  <c>HRESULT write (SAFEARRAY() psarray)</c>;</para></remarks>
        // IDL: HRESULT write (SAFEARRAY() psarray);
        // VB6: Sub write (ByVal psarray() As Any)
        [DispId(1054)]
        void write([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] object psarray);

        /// <summary><para><c>writeln</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>writeln</c> method was the following:  <c>HRESULT writeln (SAFEARRAY() psarray)</c>;</para></remarks>
        // IDL: HRESULT writeln (SAFEARRAY() psarray);
        // VB6: Sub writeln (ByVal psarray() As Any)
        [DispId(1055)] //VarEnum.VT_VARIANT
        void writeln([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] object psarray);

        /// <summary><para><c>open</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>open</c> method was the following:  <c>HRESULT open ([optional, defaultvalue("text/html")] BSTR url, [optional] VARIANT name, [optional] VARIANT features, [optional] VARIANT replace, [out, retval] IDispatch** ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT open ([optional, defaultvalue("text/html")] BSTR url, [optional] VARIANT name, [optional] VARIANT features, [optional] VARIANT replace, [out, retval] IDispatch** ReturnValue);
        // VB6: Function open ([ByVal url As String = "text/html"], [ByVal name As Any], [ByVal features As Any], [ByVal replace As Any]) As IDispatch
        [DispId(1056)]
        [return: MarshalAs(UnmanagedType.IDispatch)]
        object open([MarshalAs(UnmanagedType.BStr)] string url, object name, object features, object replace);

        /// <summary><para><c>close</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>close</c> method was the following:  <c>HRESULT close (void)</c>;</para></remarks>
        // IDL: HRESULT close (void);
        // VB6: Sub close
        [DispId(1057)]
        void close();

        /// <summary><para><c>clear</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>clear</c> method was the following:  <c>HRESULT clear (void)</c>;</para></remarks>
        // IDL: HRESULT clear (void);
        // VB6: Sub clear
        [DispId(1058)]
        void clear();

        /// <summary><para><c>queryCommandSupported</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandSupported</c> method was the following:  <c>HRESULT queryCommandSupported (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandSupported (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function queryCommandSupported (ByVal cmdID As String) As Boolean
        [DispId(1059)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool queryCommandSupported([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>queryCommandEnabled</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandEnabled</c> method was the following:  <c>HRESULT queryCommandEnabled (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandEnabled (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function queryCommandEnabled (ByVal cmdID As String) As Boolean
        [DispId(1060)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool queryCommandEnabled([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>queryCommandState</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandState</c> method was the following:  <c>HRESULT queryCommandState (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandState (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function queryCommandState (ByVal cmdID As String) As Boolean
        [DispId(1061)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool queryCommandState([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>queryCommandIndeterm</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandIndeterm</c> method was the following:  <c>HRESULT queryCommandIndeterm (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandIndeterm (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function queryCommandIndeterm (ByVal cmdID As String) As Boolean
        [DispId(1062)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool queryCommandIndeterm([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>queryCommandText</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandText</c> method was the following:  <c>HRESULT queryCommandText (BSTR cmdID, [out, retval] BSTR* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandText (BSTR cmdID, [out, retval] BSTR* ReturnValue);
        // VB6: Function queryCommandText (ByVal cmdID As String) As String
        [DispId(1063)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string queryCommandText([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>queryCommandValue</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>queryCommandValue</c> method was the following:  <c>HRESULT queryCommandValue (BSTR cmdID, [out, retval] VARIANT* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT queryCommandValue (BSTR cmdID, [out, retval] VARIANT* ReturnValue);
        // VB6: Function queryCommandValue (ByVal cmdID As String) As Any
        [DispId(1064)]
        object queryCommandValue([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>execCommand</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>execCommand</c> method was the following:  <c>HRESULT execCommand (BSTR cmdID, [optional, defaultvalue(0)] VARIANT_BOOL showUI, [optional] VARIANT value, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT execCommand (BSTR cmdID, [optional, defaultvalue(0)] VARIANT_BOOL showUI, [optional] VARIANT value, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function execCommand (ByVal cmdID As String, [ByVal showUI As Boolean = False], [ByVal value As Any]) As Boolean
        [DispId(1065)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool execCommand([MarshalAs(UnmanagedType.BStr)] string cmdID, [MarshalAs(UnmanagedType.VariantBool)] bool showUI, object value);

        /// <summary><para><c>execCommandShowHelp</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>execCommandShowHelp</c> method was the following:  <c>HRESULT execCommandShowHelp (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT execCommandShowHelp (BSTR cmdID, [out, retval] VARIANT_BOOL* ReturnValue);
        // VB6: Function execCommandShowHelp (ByVal cmdID As String) As Boolean
        [DispId(1066)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool execCommandShowHelp([MarshalAs(UnmanagedType.BStr)] string cmdID);

        /// <summary><para><c>createElement</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>createElement</c> method was the following:  <c>HRESULT createElement (BSTR eTag, [out, retval] IHTMLElement** ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT createElement (BSTR eTag, [out, retval] IHTMLElement** ReturnValue);
        // VB6: Function createElement (ByVal eTag As String) As IHTMLElement
        [DispId(1067)]
        IHTMLElement createElement([MarshalAs(UnmanagedType.BStr)] string eTag);

        /// <summary><para><c>elementFromPoint</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>elementFromPoint</c> method was the following:  <c>HRESULT elementFromPoint (long x, long y, [out, retval] IHTMLElement** ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT elementFromPoint (long x, long y, [out, retval] IHTMLElement** ReturnValue);
        // VB6: Function elementFromPoint (ByVal x As Long, ByVal y As Long) As IHTMLElement
        [DispId(1068)]
        IHTMLElement elementFromPoint(int x, int y);

        /// <summary><para><c>toString</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>toString</c> method was the following:  <c>HRESULT toString ([out, retval] BSTR* ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT toString ([out, retval] BSTR* ReturnValue);
        // VB6: Function toString As String
        [DispId(1070)]
        [return: MarshalAs(UnmanagedType.BStr)]
        string toString();

        /// <summary><para><c>createStyleSheet</c> method of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>createStyleSheet</c> method was the following:  <c>HRESULT createStyleSheet ([optional, defaultvalue("")] BSTR bstrHref, [optional, defaultvalue(-1)] long lIndex, [out, retval] IHTMLStyleSheet** ReturnValue)</c>;</para></remarks>
        // IDL: HRESULT createStyleSheet ([optional, defaultvalue("")] BSTR bstrHref, [optional, defaultvalue(-1)] long lIndex, [out, retval] IHTMLStyleSheet** ReturnValue);
        // VB6: Function createStyleSheet ([ByVal bstrHref As String = ""], [ByVal lIndex As Long = -1]) As IHTMLStyleSheet
        [DispId(1071)]
        [return: MarshalAs(UnmanagedType.Interface)] //IHTMLStyleSheet
        object createStyleSheet([MarshalAs(UnmanagedType.BStr)] string bstrHref, int lIndex);

        /// <summary><para><c>activeElement</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>activeElement</c> property was the following:  <c>IHTMLElement* activeElement</c>;</para></remarks>
        // IDL: IHTMLElement* activeElement;
        // VB6: activeElement As IHTMLElement
        IHTMLElement activeElement
        {
            // IDL: HRESULT activeElement ([out, retval] IHTMLElement** ReturnValue);
            // VB6: Function activeElement As IHTMLElement
            [DispId(1005)]
            get;
        }

        /// <summary><para><c>alinkColor</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>alinkColor</c> property was the following:  <c>VARIANT alinkColor</c>;</para></remarks>
        // IDL: VARIANT alinkColor;
        // VB6: alinkColor As Any
        object alinkColor
        {
            // IDL: HRESULT alinkColor ([out, retval] VARIANT* ReturnValue);
            // VB6: Function alinkColor As Any
            [DispId(1022)]
            get;
            // IDL: HRESULT alinkColor (VARIANT value);
            // VB6: Sub alinkColor (ByVal value As Any)
            [DispId(1022)]
            set;
        }

        /// <summary><para><c>all</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>all</c> property was the following:  <c>IHTMLElementCollection* all</c>;</para></remarks>
        // IDL: IHTMLElementCollection* all;
        // VB6: all As IHTMLElementCollection
        object all
        {
            // IDL: HRESULT all ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function all As IHTMLElementCollection
            [DispId(1003)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>anchors</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>anchors</c> property was the following:  <c>IHTMLElementCollection* anchors</c>;</para></remarks>
        // IDL: IHTMLElementCollection* anchors;
        // VB6: anchors As IHTMLElementCollection
        object anchors
        {
            // IDL: HRESULT anchors ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function anchors As IHTMLElementCollection
            [DispId(1007)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>applets</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>applets</c> property was the following:  <c>IHTMLElementCollection* applets</c>;</para></remarks>
        // IDL: IHTMLElementCollection* applets;
        // VB6: applets As IHTMLElementCollection
        object applets
        {
            // IDL: HRESULT applets ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function applets As IHTMLElementCollection
            [DispId(1008)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>bgColor</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>bgColor</c> property was the following:  <c>VARIANT bgColor</c>;</para></remarks>
        // IDL: VARIANT bgColor;
        // VB6: bgColor As Any
        object bgColor
        {
            // IDL: HRESULT bgColor ([out, retval] VARIANT* ReturnValue);
            // VB6: Function bgColor As Any
            [DispId(-501)]
            get;
            // IDL: HRESULT bgColor (VARIANT value);
            // VB6: Sub bgColor (ByVal value As Any)
            [DispId(-501)]
            set;
        }

        /// <summary><para><c>body</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>body</c> property was the following:  <c>IHTMLElement* body</c>;</para></remarks>
        // IDL: IHTMLElement* body;
        // VB6: body As IHTMLElement
        IHTMLElement body
        {
            // IDL: HRESULT body ([out, retval] IHTMLElement** ReturnValue);
            // VB6: Function body As IHTMLElement
            [DispId(1004)]
            get;
        }

        /// <summary><para><c>charset</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>charset</c> property was the following:  <c>BSTR charset</c>;</para></remarks>
        // IDL: BSTR charset;
        // VB6: charset As String
        string charset
        {
            // IDL: HRESULT charset ([out, retval] BSTR* ReturnValue);
            // VB6: Function charset As String
            [DispId(1032)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT charset (BSTR value);
            // VB6: Sub charset (ByVal value As String)
            [DispId(1032)]
            set;
        }

        /// <summary><para><c>cookie</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>cookie</c> property was the following:  <c>BSTR cookie</c>;</para></remarks>
        // IDL: BSTR cookie;
        // VB6: cookie As String
        string cookie
        {
            // IDL: HRESULT cookie ([out, retval] BSTR* ReturnValue);
            // VB6: Function cookie As String
            [DispId(1030)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT cookie (BSTR value);
            // VB6: Sub cookie (ByVal value As String)
            [DispId(1030)]
            set;
        }

        /// <summary><para><c>defaultCharset</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>defaultCharset</c> property was the following:  <c>BSTR defaultCharset</c>;</para></remarks>
        // IDL: BSTR defaultCharset;
        // VB6: defaultCharset As String
        string defaultCharset
        {
            // IDL: HRESULT defaultCharset ([out, retval] BSTR* ReturnValue);
            // VB6: Function defaultCharset As String
            [DispId(1033)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT defaultCharset (BSTR value);
            // VB6: Sub defaultCharset (ByVal value As String)
            [DispId(1033)]
            set;
        }

        /// <summary><para><c>designMode</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>designMode</c> property was the following:  <c>BSTR designMode</c>;</para></remarks>
        // IDL: BSTR designMode;
        // VB6: designMode As String
        string designMode
        {
            // IDL: HRESULT designMode ([out, retval] BSTR* ReturnValue);
            // VB6: Function designMode As String
            [DispId(1014)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT designMode (BSTR value);
            // VB6: Sub designMode (ByVal value As String)
            [DispId(1014)]
            set;
        }

        /// <summary><para><c>domain</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>domain</c> property was the following:  <c>BSTR domain</c>;</para></remarks>
        // IDL: BSTR domain;
        // VB6: domain As String
        string domain
        {
            // IDL: HRESULT domain ([out, retval] BSTR* ReturnValue);
            // VB6: Function domain As String
            [DispId(1029)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT domain (BSTR value);
            // VB6: Sub domain (ByVal value As String)
            [DispId(1029)]
            set;
        }

        /// <summary><para><c>embeds</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>embeds</c> property was the following:  <c>IHTMLElementCollection* embeds</c>;</para></remarks>
        // IDL: IHTMLElementCollection* embeds;
        // VB6: embeds As IHTMLElementCollection
        object embeds
        {
            // IDL: HRESULT embeds ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function embeds As IHTMLElementCollection
            [DispId(1015)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>expando</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>expando</c> property was the following:  <c>VARIANT_BOOL expando</c>;</para></remarks>
        // IDL: VARIANT_BOOL expando;
        // VB6: expando As Boolean
        bool expando
        {
            // IDL: HRESULT expando ([out, retval] VARIANT_BOOL* ReturnValue);
            // VB6: Function expando As Boolean
            [DispId(1031)]
            [return: MarshalAs(UnmanagedType.VariantBool)]
            get;
            // IDL: HRESULT expando (VARIANT_BOOL value);
            // VB6: Sub expando (ByVal value As Boolean)
            [DispId(1031)]
            set;
        }

        /// <summary><para><c>fgColor</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>fgColor</c> property was the following:  <c>VARIANT fgColor</c>;</para></remarks>
        // IDL: VARIANT fgColor;
        // VB6: fgColor As Any
        object fgColor
        {
            // IDL: HRESULT fgColor ([out, retval] VARIANT* ReturnValue);
            // VB6: Function fgColor As Any
            [DispId(-2147413110)]
            get;
            // IDL: HRESULT fgColor (VARIANT value);
            // VB6: Sub fgColor (ByVal value As Any)
            [DispId(-2147413110)]
            set;
        }

        /// <summary><para><c>fileCreatedDate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>fileCreatedDate</c> property was the following:  <c>BSTR fileCreatedDate</c>;</para></remarks>
        // IDL: BSTR fileCreatedDate;
        // VB6: fileCreatedDate As String
        string fileCreatedDate
        {
            // IDL: HRESULT fileCreatedDate ([out, retval] BSTR* ReturnValue);
            // VB6: Function fileCreatedDate As String
            [DispId(1043)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>fileModifiedDate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>fileModifiedDate</c> property was the following:  <c>BSTR fileModifiedDate</c>;</para></remarks>
        // IDL: BSTR fileModifiedDate;
        // VB6: fileModifiedDate As String
        string fileModifiedDate
        {
            // IDL: HRESULT fileModifiedDate ([out, retval] BSTR* ReturnValue);
            // VB6: Function fileModifiedDate As String
            [DispId(1044)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>fileSize</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>fileSize</c> property was the following:  <c>BSTR fileSize</c>;</para></remarks>
        // IDL: BSTR fileSize;
        // VB6: fileSize As String
        string fileSize
        {
            // IDL: HRESULT fileSize ([out, retval] BSTR* ReturnValue);
            // VB6: Function fileSize As String
            [DispId(1042)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>fileUpdatedDate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>fileUpdatedDate</c> property was the following:  <c>BSTR fileUpdatedDate</c>;</para></remarks>
        // IDL: BSTR fileUpdatedDate;
        // VB6: fileUpdatedDate As String
        string fileUpdatedDate
        {
            // IDL: HRESULT fileUpdatedDate ([out, retval] BSTR* ReturnValue);
            // VB6: Function fileUpdatedDate As String
            [DispId(1045)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>forms</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>forms</c> property was the following:  <c>IHTMLElementCollection* forms</c>;</para></remarks>
        // IDL: IHTMLElementCollection* forms;
        // VB6: forms As IHTMLElementCollection
        object forms
        {
            // IDL: HRESULT forms ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function forms As IHTMLElementCollection
            [DispId(1010)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>frames</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>frames</c> property was the following:  <c>IHTMLFramesCollection2* frames</c>;</para></remarks>
        // IDL: IHTMLFramesCollection2* frames;
        // VB6: frames As IHTMLFramesCollection2
        object frames
        {
            // IDL: HRESULT frames ([out, retval] IHTMLFramesCollection2** ReturnValue);
            // VB6: Function frames As IHTMLFramesCollection2
            [DispId(1019)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLFramesCollection2
            get;
        }

        /// <summary><para><c>images</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>images</c> property was the following:  <c>IHTMLElementCollection* images</c>;</para></remarks>
        // IDL: IHTMLElementCollection* images;
        // VB6: images As IHTMLElementCollection
        object images
        {
            // IDL: HRESULT images ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function images As IHTMLElementCollection
            [DispId(1011)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>lastModified</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>lastModified</c> property was the following:  <c>BSTR lastModified</c>;</para></remarks>
        // IDL: BSTR lastModified;
        // VB6: lastModified As String
        string lastModified
        {
            // IDL: HRESULT lastModified ([out, retval] BSTR* ReturnValue);
            // VB6: Function lastModified As String
            [DispId(1028)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>linkColor</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>linkColor</c> property was the following:  <c>VARIANT linkColor</c>;</para></remarks>
        // IDL: VARIANT linkColor;
        // VB6: linkColor As Any
        object linkColor
        {
            // IDL: HRESULT linkColor ([out, retval] VARIANT* ReturnValue);
            // VB6: Function linkColor As Any
            [DispId(1024)]
            get;
            // IDL: HRESULT linkColor (VARIANT value);
            // VB6: Sub linkColor (ByVal value As Any)
            [DispId(1024)]
            set;
        }

        /// <summary><para><c>links</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>links</c> property was the following:  <c>IHTMLElementCollection* links</c>;</para></remarks>
        // IDL: IHTMLElementCollection* links;
        // VB6: links As IHTMLElementCollection
        object links
        {
            // IDL: HRESULT links ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function links As IHTMLElementCollection
            [DispId(1009)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>location</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>location</c> property was the following:  <c>IHTMLLocation* location</c>;</para></remarks>
        // IDL: IHTMLLocation* location;
        // VB6: location As IHTMLLocation
        object location
        {
            // IDL: HRESULT location ([out, retval] IHTMLLocation** ReturnValue);
            // VB6: Function location As IHTMLLocation
            [DispId(1026)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLLocation
            get;
        }

        /// <summary><para><c>mimeType</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>mimeType</c> property was the following:  <c>BSTR mimeType</c>;</para></remarks>
        // IDL: BSTR mimeType;
        // VB6: mimeType As String
        string mimeType
        {
            // IDL: HRESULT mimeType ([out, retval] BSTR* ReturnValue);
            // VB6: Function mimeType As String
            [DispId(1041)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>nameProp</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>nameProp</c> property was the following:  <c>BSTR nameProp</c>;</para></remarks>
        // IDL: BSTR nameProp;
        // VB6: nameProp As String
        string nameProp
        {
            // IDL: HRESULT nameProp ([out, retval] BSTR* ReturnValue);
            // VB6: Function nameProp As String
            [DispId(1048)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>onafterupdate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onafterupdate</c> property was the following:  <c>VARIANT onafterupdate</c>;</para></remarks>
        // IDL: VARIANT onafterupdate;
        // VB6: onafterupdate As Any
        object onafterupdate
        {
            // IDL: HRESULT onafterupdate ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onafterupdate As Any
            [DispId(-2147412090)]
            get;
            // IDL: HRESULT onafterupdate (VARIANT value);
            // VB6: Sub onafterupdate (ByVal value As Any)
            [DispId(-2147412090)]
            set;
        }

        /// <summary><para><c>onbeforeupdate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onbeforeupdate</c> property was the following:  <c>VARIANT onbeforeupdate</c>;</para></remarks>
        // IDL: VARIANT onbeforeupdate;
        // VB6: onbeforeupdate As Any
        object onbeforeupdate
        {
            // IDL: HRESULT onbeforeupdate ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onbeforeupdate As Any
            [DispId(-2147412091)]
            get;
            // IDL: HRESULT onbeforeupdate (VARIANT value);
            // VB6: Sub onbeforeupdate (ByVal value As Any)
            [DispId(-2147412091)]
            set;
        }

        /// <summary><para><c>onclick</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onclick</c> property was the following:  <c>VARIANT onclick</c>;</para></remarks>
        // IDL: VARIANT onclick;
        // VB6: onclick As Any
        object onclick
        {
            // IDL: HRESULT onclick ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onclick As Any
            [DispId(-2147412104)]
            get;
            // IDL: HRESULT onclick (VARIANT value);
            // VB6: Sub onclick (ByVal value As Any)
            [DispId(-2147412104)]
            set;
        }

        /// <summary><para><c>ondblclick</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>ondblclick</c> property was the following:  <c>VARIANT ondblclick</c>;</para></remarks>
        // IDL: VARIANT ondblclick;
        // VB6: ondblclick As Any
        object ondblclick
        {
            // IDL: HRESULT ondblclick ([out, retval] VARIANT* ReturnValue);
            // VB6: Function ondblclick As Any
            [DispId(-2147412103)]
            get;
            // IDL: HRESULT ondblclick (VARIANT value);
            // VB6: Sub ondblclick (ByVal value As Any)
            [DispId(-2147412103)]
            set;
        }

        /// <summary><para><c>ondragstart</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>ondragstart</c> property was the following:  <c>VARIANT ondragstart</c>;</para></remarks>
        // IDL: VARIANT ondragstart;
        // VB6: ondragstart As Any
        object ondragstart
        {
            // IDL: HRESULT ondragstart ([out, retval] VARIANT* ReturnValue);
            // VB6: Function ondragstart As Any
            [DispId(-2147412077)]
            get;
            // IDL: HRESULT ondragstart (VARIANT value);
            // VB6: Sub ondragstart (ByVal value As Any)
            [DispId(-2147412077)]
            set;
        }

        /// <summary><para><c>onerrorupdate</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onerrorupdate</c> property was the following:  <c>VARIANT onerrorupdate</c>;</para></remarks>
        // IDL: VARIANT onerrorupdate;
        // VB6: onerrorupdate As Any
        object onerrorupdate
        {
            // IDL: HRESULT onerrorupdate ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onerrorupdate As Any
            [DispId(-2147412074)]
            get;
            // IDL: HRESULT onerrorupdate (VARIANT value);
            // VB6: Sub onerrorupdate (ByVal value As Any)
            [DispId(-2147412074)]
            set;
        }

        /// <summary><para><c>onhelp</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onhelp</c> property was the following:  <c>VARIANT onhelp</c>;</para></remarks>
        // IDL: VARIANT onhelp;
        // VB6: onhelp As Any
        object onhelp
        {
            // IDL: HRESULT onhelp ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onhelp As Any
            [DispId(-2147412099)]
            get;
            // IDL: HRESULT onhelp (VARIANT value);
            // VB6: Sub onhelp (ByVal value As Any)
            [DispId(-2147412099)]
            set;
        }

        /// <summary><para><c>onkeydown</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onkeydown</c> property was the following:  <c>VARIANT onkeydown</c>;</para></remarks>
        // IDL: VARIANT onkeydown;
        // VB6: onkeydown As Any
        object onkeydown
        {
            // IDL: HRESULT onkeydown ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onkeydown As Any
            [DispId(-2147412107)]
            get;
            // IDL: HRESULT onkeydown (VARIANT value);
            // VB6: Sub onkeydown (ByVal value As Any)
            [DispId(-2147412107)]
            set;
        }

        /// <summary><para><c>onkeypress</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onkeypress</c> property was the following:  <c>VARIANT onkeypress</c>;</para></remarks>
        // IDL: VARIANT onkeypress;
        // VB6: onkeypress As Any
        object onkeypress
        {
            // IDL: HRESULT onkeypress ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onkeypress As Any
            [DispId(-2147412105)]
            get;
            // IDL: HRESULT onkeypress (VARIANT value);
            // VB6: Sub onkeypress (ByVal value As Any)
            [DispId(-2147412105)]
            set;
        }

        /// <summary><para><c>onkeyup</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onkeyup</c> property was the following:  <c>VARIANT onkeyup</c>;</para></remarks>
        // IDL: VARIANT onkeyup;
        // VB6: onkeyup As Any
        object onkeyup
        {
            // IDL: HRESULT onkeyup ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onkeyup As Any
            [DispId(-2147412106)]
            get;
            // IDL: HRESULT onkeyup (VARIANT value);
            // VB6: Sub onkeyup (ByVal value As Any)
            [DispId(-2147412106)]
            set;
        }

        /// <summary><para><c>onmousedown</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onmousedown</c> property was the following:  <c>VARIANT onmousedown</c>;</para></remarks>
        // IDL: VARIANT onmousedown;
        // VB6: onmousedown As Any
        object onmousedown
        {
            // IDL: HRESULT onmousedown ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onmousedown As Any
            [DispId(-2147412110)]
            get;
            // IDL: HRESULT onmousedown (VARIANT value);
            // VB6: Sub onmousedown (ByVal value As Any)
            [DispId(-2147412110)]
            set;
        }

        /// <summary><para><c>onmousemove</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onmousemove</c> property was the following:  <c>VARIANT onmousemove</c>;</para></remarks>
        // IDL: VARIANT onmousemove;
        // VB6: onmousemove As Any
        object onmousemove
        {
            // IDL: HRESULT onmousemove ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onmousemove As Any
            [DispId(-2147412108)]
            get;
            // IDL: HRESULT onmousemove (VARIANT value);
            // VB6: Sub onmousemove (ByVal value As Any)
            [DispId(-2147412108)]
            set;
        }

        /// <summary><para><c>onmouseout</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onmouseout</c> property was the following:  <c>VARIANT onmouseout</c>;</para></remarks>
        // IDL: VARIANT onmouseout;
        // VB6: onmouseout As Any
        object onmouseout
        {
            // IDL: HRESULT onmouseout ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onmouseout As Any
            [DispId(-2147412111)]
            get;
            // IDL: HRESULT onmouseout (VARIANT value);
            // VB6: Sub onmouseout (ByVal value As Any)
            [DispId(-2147412111)]
            set;
        }

        /// <summary><para><c>onmouseover</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onmouseover</c> property was the following:  <c>VARIANT onmouseover</c>;</para></remarks>
        // IDL: VARIANT onmouseover;
        // VB6: onmouseover As Any
        object onmouseover
        {
            // IDL: HRESULT onmouseover ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onmouseover As Any
            [DispId(-2147412112)]
            get;
            // IDL: HRESULT onmouseover (VARIANT value);
            // VB6: Sub onmouseover (ByVal value As Any)
            [DispId(-2147412112)]
            set;
        }

        /// <summary><para><c>onmouseup</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onmouseup</c> property was the following:  <c>VARIANT onmouseup</c>;</para></remarks>
        // IDL: VARIANT onmouseup;
        // VB6: onmouseup As Any
        object onmouseup
        {
            // IDL: HRESULT onmouseup ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onmouseup As Any
            [DispId(-2147412109)]
            get;
            // IDL: HRESULT onmouseup (VARIANT value);
            // VB6: Sub onmouseup (ByVal value As Any)
            [DispId(-2147412109)]
            set;
        }

        /// <summary><para><c>onreadystatechange</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onreadystatechange</c> property was the following:  <c>VARIANT onreadystatechange</c>;</para></remarks>
        // IDL: VARIANT onreadystatechange;
        // VB6: onreadystatechange As Any
        object onreadystatechange
        {
            // IDL: HRESULT onreadystatechange ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onreadystatechange As Any
            [DispId(-2147412087)]
            get;
            // IDL: HRESULT onreadystatechange (VARIANT value);
            // VB6: Sub onreadystatechange (ByVal value As Any)
            [DispId(-2147412087)]
            set;
        }

        /// <summary><para><c>onrowenter</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onrowenter</c> property was the following:  <c>VARIANT onrowenter</c>;</para></remarks>
        // IDL: VARIANT onrowenter;
        // VB6: onrowenter As Any
        object onrowenter
        {
            // IDL: HRESULT onrowenter ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onrowenter As Any
            [DispId(-2147412093)]
            get;
            // IDL: HRESULT onrowenter (VARIANT value);
            // VB6: Sub onrowenter (ByVal value As Any)
            [DispId(-2147412093)]
            set;
        }

        /// <summary><para><c>onrowexit</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onrowexit</c> property was the following:  <c>VARIANT onrowexit</c>;</para></remarks>
        // IDL: VARIANT onrowexit;
        // VB6: onrowexit As Any
        object onrowexit
        {
            // IDL: HRESULT onrowexit ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onrowexit As Any
            [DispId(-2147412094)]
            get;
            // IDL: HRESULT onrowexit (VARIANT value);
            // VB6: Sub onrowexit (ByVal value As Any)
            [DispId(-2147412094)]
            set;
        }

        /// <summary><para><c>onselectstart</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>onselectstart</c> property was the following:  <c>VARIANT onselectstart</c>;</para></remarks>
        // IDL: VARIANT onselectstart;
        // VB6: onselectstart As Any
        object onselectstart
        {
            // IDL: HRESULT onselectstart ([out, retval] VARIANT* ReturnValue);
            // VB6: Function onselectstart As Any
            [DispId(-2147412075)]
            get;
            // IDL: HRESULT onselectstart (VARIANT value);
            // VB6: Sub onselectstart (ByVal value As Any)
            [DispId(-2147412075)]
            set;
        }

        /// <summary><para><c>parentWindow</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>parentWindow</c> property was the following:  <c>IHTMLWindow2* parentWindow</c>;</para></remarks>
        // IDL: IHTMLWindow2* parentWindow;
        // VB6: parentWindow As IHTMLWindow2
        object parentWindow
        {
            // IDL: HRESULT parentWindow ([out, retval] IHTMLWindow2** ReturnValue);
            // VB6: Function parentWindow As IHTMLWindow2
            [DispId(1034)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLWindow2
            get;
        }

        /// <summary><para><c>plugins</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>plugins</c> property was the following:  <c>IHTMLElementCollection* plugins</c>;</para></remarks>
        // IDL: IHTMLElementCollection* plugins;
        // VB6: plugins As IHTMLElementCollection
        object plugins
        {
            // IDL: HRESULT plugins ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function plugins As IHTMLElementCollection
            [DispId(1021)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>protocol</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>protocol</c> property was the following:  <c>BSTR protocol</c>;</para></remarks>
        // IDL: BSTR protocol;
        // VB6: protocol As String
        string protocol
        {
            // IDL: HRESULT protocol ([out, retval] BSTR* ReturnValue);
            // VB6: Function protocol As String
            [DispId(1047)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>readyState</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>readyState</c> property was the following:  <c>BSTR readyState</c>;</para></remarks>
        // IDL: BSTR readyState;
        // VB6: readyState As String
        string readyState
        {
            // IDL: HRESULT readyState ([out, retval] BSTR* ReturnValue);
            // VB6: Function readyState As String
            [DispId(1018)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>referrer</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>referrer</c> property was the following:  <c>BSTR referrer</c>;</para></remarks>
        // IDL: BSTR referrer;
        // VB6: referrer As String
        string referrer
        {
            // IDL: HRESULT referrer ([out, retval] BSTR* ReturnValue);
            // VB6: Function referrer As String
            [DispId(1027)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>Script</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>Script</c> property was the following:  <c>IDispatch* Script</c>;</para></remarks>
        // IDL: IDispatch* Script;
        // VB6: Script As IDispatch
        object Script
        {
            // IDL: HRESULT Script ([out, retval] IDispatch** ReturnValue);
            // VB6: Function Script As IDispatch
            [DispId(1001)]
            [return: MarshalAs(UnmanagedType.IDispatch)]
            get;
        }

        /// <summary><para><c>scripts</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>scripts</c> property was the following:  <c>IHTMLElementCollection* scripts</c>;</para></remarks>
        // IDL: IHTMLElementCollection* scripts;
        // VB6: scripts As IHTMLElementCollection
        object scripts
        {
            // IDL: HRESULT scripts ([out, retval] IHTMLElementCollection** ReturnValue);
            // VB6: Function scripts As IHTMLElementCollection
            [DispId(1013)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLElementCollection
            get;
        }

        /// <summary><para><c>security</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>security</c> property was the following:  <c>BSTR security</c>;</para></remarks>
        // IDL: BSTR security;
        // VB6: security As String
        string security
        {
            // IDL: HRESULT security ([out, retval] BSTR* ReturnValue);
            // VB6: Function security As String
            [DispId(1046)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
        }

        /// <summary><para><c>selection</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>selection</c> property was the following:  <c>IHTMLSelectionObject* selection</c>;</para></remarks>
        // IDL: IHTMLSelectionObject* selection;
        // VB6: selection As IHTMLSelectionObject
        object selection
        {
            // IDL: HRESULT selection ([out, retval] IHTMLSelectionObject** ReturnValue);
            // VB6: Function selection As IHTMLSelectionObject
            [DispId(1017)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLSelectionObject
            get;
        }

        /// <summary><para><c>styleSheets</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>styleSheets</c> property was the following:  <c>IHTMLStyleSheetsCollection* styleSheets</c>;</para></remarks>
        // IDL: IHTMLStyleSheetsCollection* styleSheets;
        // VB6: styleSheets As IHTMLStyleSheetsCollection
        object styleSheets
        {
            // IDL: HRESULT styleSheets ([out, retval] IHTMLStyleSheetsCollection** ReturnValue);
            // VB6: Function styleSheets As IHTMLStyleSheetsCollection
            [DispId(1069)]
            [return: MarshalAs(UnmanagedType.Interface)] //IHTMLStyleSheetsCollection
            get;
        }

        /// <summary><para><c>title</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>title</c> property was the following:  <c>BSTR title</c>;</para></remarks>
        // IDL: BSTR title;
        // VB6: title As String
        string title
        {
            // IDL: HRESULT title ([out, retval] BSTR* ReturnValue);
            // VB6: Function title As String
            [DispId(1012)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT title (BSTR value);
            // VB6: Sub title (ByVal value As String)
            [DispId(1012)]
            set;
        }

        /// <summary><para><c>url</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>url</c> property was the following:  <c>BSTR url</c>;</para></remarks>
        // IDL: BSTR url;
        // VB6: url As String
        string url
        {
            // IDL: HRESULT url ([out, retval] BSTR* ReturnValue);
            // VB6: Function url As String
            [DispId(1025)]
            [return: MarshalAs(UnmanagedType.BStr)]
            get;
            // IDL: HRESULT url (BSTR value);
            // VB6: Sub url (ByVal value As String)
            [DispId(1025)]
            set;
        }

        /// <summary><para><c>vlinkColor</c> property of <c>IHTMLDocument2</c> interface.</para></summary>
        /// <remarks><para>An original IDL definition of <c>vlinkColor</c> property was the following:  <c>VARIANT vlinkColor</c>;</para></remarks>
        // IDL: VARIANT vlinkColor;
        // VB6: vlinkColor As Any
        object vlinkColor
        {
            // IDL: HRESULT vlinkColor ([out, retval] VARIANT* ReturnValue);
            // VB6: Function vlinkColor As Any
            [DispId(1023)]
            get;
            // IDL: HRESULT vlinkColor (VARIANT value);
            // VB6: Sub vlinkColor (ByVal value As Any)
            [DispId(1023)]
            set;
        }
    }
 

    [ComImport, DefaultMember("item"), Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLElementCollection : IEnumerable
    {
        [DispId(0x5dc)]
        int length { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dc)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dc)]
        get; }

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dd)]
        string toString();

        [return:
            MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "",
                MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4),
         TypeLibFunc((short) 0x41)]
        new IEnumerator GetEnumerator();

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, Optional, MarshalAs(UnmanagedType.Struct)] object name,
                    [In, Optional, MarshalAs(UnmanagedType.Struct)] object index);

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5de)]
        object tags([In, MarshalAs(UnmanagedType.Struct)] object tagName);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("3050F24F-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IHTMLObjectElement
    {
        [DispId(-2147415111)]
        object @object { [return: MarshalAs(UnmanagedType.IDispatch)]
        get; }

        [DispId(-2147415110)]
        string classid { [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415109)]
        string data { [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415107)]
        object recordset { [param: In, MarshalAs(UnmanagedType.IDispatch)]
        set; [return: MarshalAs(UnmanagedType.IDispatch)]
        get; }

        [DispId(-2147418039)]
        string align { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147418112)]
        string name { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415106)]
        string codeBase { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415105)]
        string codeType { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415104)]
        string code { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147418110)]
        string BaseHref { [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415103)]
        string type { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147416108)]
        object form { [return: MarshalAs(UnmanagedType.IDispatch)]
        get; }

        [DispId(-2147418107)]
        object width { [param: In, MarshalAs(UnmanagedType.Struct)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        get; }

        [DispId(-2147418106)]
        object height { [param: In, MarshalAs(UnmanagedType.Struct)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        get; }

        [DispId(-2147415102)]
        int readyState { get; }

        [DispId(-2147412087)]
        object onreadystatechange { [param: In, MarshalAs(UnmanagedType.Struct)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        get; }

        [DispId(-2147412083)]
        object onerror { [param: In, MarshalAs(UnmanagedType.Struct)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        get; }

        [DispId(-2147415101)]
        string altHtml { [param: In, MarshalAs(UnmanagedType.BStr)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        get; }

        [DispId(-2147415100)]
        int vspace { get; [param: In]
        set; }

        [DispId(-2147415099)]
        int hspace { get; [param: In]
        set; }
    }

    [ComImport, Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IHTMLStyle
    {
        [DispId(-2147413094)]
        string fontFamily { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413094)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413094)]
        get; }

        [DispId(-2147413088)]
        string fontStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413088)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413088), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413087)]
        string fontVariant { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413087)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413087)]
        get; }

        [DispId(-2147413085)]
        string fontWeight { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413085)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413085)]
        get; }

        [DispId(-2147413093)]
        object fontSize { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413093)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413093), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413071)]
        string font { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413071)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413071)]
        get; }

        [DispId(-2147413110)]
        object color { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413110), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413110), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413080)]
        string background { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413080)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413080)]
        get; }

        [DispId(-501)]
        object backgroundColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-501)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-501)]
        get; }

        [DispId(-2147413111)]
        string backgroundImage { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413111)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413111), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413068)]
        string backgroundRepeat { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413068), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413068)]
        get; }

        [DispId(-2147413067)]
        string backgroundAttachment { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413067), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413067), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413066)]
        string backgroundPosition { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413066), TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413066)]
        get; }

        [DispId(-2147413079)]
        object backgroundPositionX { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413079)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413079), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413078)]
        object backgroundPositionY { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413078), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413078), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413065)]
        object wordSpacing { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413065)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413065)]
        get; }

        [DispId(-2147413104)]
        object letterSpacing { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413104)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413104), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413077)]
        string textDecoration { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413077), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413077)]
        get; }

        [DispId(-2147413089)]
        bool textDecorationNone { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147413089)]
        set; [TypeLibFunc((short) 20), DispId(-2147413089)]
        get; }

        [DispId(-2147413091)]
        bool textDecorationUnderline { [param: In]
        [DispId(-2147413091), TypeLibFunc((short) 20)]
        set; [DispId(-2147413091), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413043)]
        bool textDecorationOverline { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147413043)]
        set; [TypeLibFunc((short) 20), DispId(-2147413043)]
        get; }

        [DispId(-2147413092)]
        bool textDecorationLineThrough { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147413092)]
        set; [TypeLibFunc((short) 20), DispId(-2147413092)]
        get; }

        [DispId(-2147413090)]
        bool textDecorationBlink { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147413090)]
        set; [TypeLibFunc((short) 20), DispId(-2147413090)]
        get; }

        [DispId(-2147413064)]
        object verticalAlign { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413064)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413064)]
        get; }

        [DispId(-2147413108)]
        string textTransform { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413108)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413108), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418040)]
        string textAlign { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147418040), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147418040), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413105)]
        object textIndent { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413105)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413105)]
        get; }

        [DispId(-2147413106)]
        object lineHeight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413106)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413106), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413075)]
        object marginTop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413075), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413075)]
        get; }

        [DispId(-2147413074)]
        object marginRight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413074)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413074), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413073)]
        object marginBottom { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413073), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413073), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413072)]
        object marginLeft { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413072)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413072), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413076)]
        string margin { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413076)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413076)]
        get; }

        [DispId(-2147413100)]
        object paddingTop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413100), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413100), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413099)]
        object paddingRight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413099), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413099), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413098)]
        object paddingBottom { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413098), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413098), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413097)]
        object paddingLeft { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413097), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413097), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413101)]
        string padding { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413101), TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413101), TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-2147413063)]
        string border { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413063), TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413063), TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-2147413062)]
        string borderTop { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413062), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413062)]
        get; }

        [DispId(-2147413061)]
        string borderRight { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413061), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413061), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413060)]
        string borderBottom { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413060)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413060)]
        get; }

        [DispId(-2147413059)]
        string borderLeft { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413059), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413059)]
        get; }

        [DispId(-2147413058)]
        string borderColor { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413058), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413058), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413057)]
        object borderTopColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413057), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413057), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413056)]
        object borderRightColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413056), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413056)]
        get; }

        [DispId(-2147413055)]
        object borderBottomColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413055)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413055), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413054)]
        object borderLeftColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413054)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413054), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413053)]
        string borderWidth { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413053), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413053)]
        get; }

        [DispId(-2147413052)]
        object borderTopWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413052)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413052), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413051)]
        object borderRightWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413051), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413051), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413050)]
        object borderBottomWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413050), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413050)]
        get; }

        [DispId(-2147413049)]
        object borderLeftWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413049)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413049), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413048)]
        string borderStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413048)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413048)]
        get; }

        [DispId(-2147413047)]
        string borderTopStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413047), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413047), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413046)]
        string borderRightStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413046), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413046), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413045)]
        string borderBottomStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413045)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413045), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413044)]
        string borderLeftStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413044), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413044)]
        get; }

        [DispId(-2147418107)]
        object width { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147418107)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147418107), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418106)]
        object height { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147418106), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147418106)]
        get; }

        [DispId(-2147413042)]
        string styleFloat { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413042), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413042)]
        get; }

        [DispId(-2147413096)]
        string clear { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413096), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413096), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413041)]
        string display { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413041)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413041)]
        get; }

        [DispId(-2147413032)]
        string visibility { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413032)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413032), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413040)]
        string listStyleType { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413040), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413040), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413039)]
        string listStylePosition { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413039), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413039), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413038)]
        string listStyleImage { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413038)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413038), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413037)]
        string listStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413037), TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413037)]
        get; }

        [DispId(-2147413036)]
        string whiteSpace { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413036), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413036), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418108)]
        object top { [param: In, MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147418108)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147418108), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418109)]
        object left { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147418109), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147418109)]
        get; }

        [DispId(-2147413022)]
        string position { [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413022)]
        get; }

        [DispId(-2147413021)]
        object zIndex { [param: In, MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147413021), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [TypeLibFunc((short) 20), DispId(-2147413021)]
        get; }

        [DispId(-2147413102)]
        string overflow { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413102), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413102), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413035)]
        string pageBreakBefore { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413035)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413035), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413034)]
        string pageBreakAfter { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413034), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413034)]
        get; }

        [DispId(-2147413013)]
        string cssText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413013)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 0x414), DispId(-2147413013)]
        get; }

        [DispId(-2147414112)]
        int pixelTop { [param: In]
        [TypeLibFunc((short) 0x54), DispId(-2147414112)]
        set; [DispId(-2147414112), TypeLibFunc((short) 0x54)]
        get; }

        [DispId(-2147414111)]
        int pixelLeft { [param: In]
        [TypeLibFunc((short) 0x54), DispId(-2147414111)]
        set; [DispId(-2147414111), TypeLibFunc((short) 0x54)]
        get; }

        [DispId(-2147414110)]
        int pixelWidth { [param: In]
        [DispId(-2147414110), TypeLibFunc((short) 0x54)]
        set; [TypeLibFunc((short) 0x54), DispId(-2147414110)]
        get; }

        [DispId(-2147414109)]
        int pixelHeight { [param: In]
        [DispId(-2147414109), TypeLibFunc((short) 0x54)]
        set; [DispId(-2147414109), TypeLibFunc((short) 0x54)]
        get; }

        [DispId(-2147414108)]
        float posTop { [param: In]
        [DispId(-2147414108), TypeLibFunc((short) 20)]
        set; [TypeLibFunc((short) 20), DispId(-2147414108)]
        get; }

        [DispId(-2147414107)]
        float posLeft { [param: In]
        [DispId(-2147414107), TypeLibFunc((short) 20)]
        set; [TypeLibFunc((short) 20), DispId(-2147414107)]
        get; }

        [DispId(-2147414106)]
        float posWidth { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147414106)]
        set; [TypeLibFunc((short) 20), DispId(-2147414106)]
        get; }

        [DispId(-2147414105)]
        float posHeight { [param: In]
        [TypeLibFunc((short) 20), DispId(-2147414105)]
        set; [DispId(-2147414105), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413010)]
        string cursor { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413010)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413010)]
        get; }

        [DispId(-2147413020)]
        string clip { [param: In, MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413020), TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413020)]
        get; }

        [DispId(-2147413030)]
        string filter { [param: In, MarshalAs(UnmanagedType.BStr)]
        [TypeLibFunc((short) 20), DispId(-2147413030)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147413030), TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147417611)]
        void setAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                          [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue,
                          [In, Optional, DefaultParameterValue(1)] int lFlags);

        [return: MarshalAs(UnmanagedType.Struct)]
        [DispId(-2147417610)]
        object getAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                            [In, Optional, DefaultParameterValue(0)] int lFlags);

        [DispId(-2147417609)]
        bool removeAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                             [In, Optional, DefaultParameterValue(1)] int lFlags);

        [return: MarshalAs(UnmanagedType.BStr)]
        [DispId(-2147414104)]
        string toString();
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F25A-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLSelectionObject
    {
        [DispId(0x3ec)]
        string type { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        object createRange();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        void empty();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        void clear();
    }

    [ComImport, Guid("3050F7F6-98B5-11CF-BB82-00AA00BDCE0B"), DefaultMember("item"), ClassInterface((short) 0)]
    public class FramesCollectionClass : IHTMLFramesCollection2, FramesCollection
    {
        // Methods

        #region IHTMLFramesCollection2 Members

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        public virtual extern object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);

        // Properties
        [DispId(0x3e9)]
        public virtual extern int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        #endregion
    }

    [ComImport, Guid("332C4426-26CB-11D0-B483-00C04FD90119"), CoClass(typeof (FramesCollectionClass))]
    public interface FramesCollection : IHTMLFramesCollection2
    {
    }

    [ComImport, DefaultMember("item"), Guid("332C4426-26CB-11D0-B483-00C04FD90119"), TypeLibType((short) 0x1040)]
    public interface IHTMLFramesCollection2
    {
        [DispId(0x3e9)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);
    }

    [ComImport, Guid("163BB1E0-6E00-11CF-837A-48DC04C10000"), TypeLibType((short) 0x1040), DefaultMember("href")]
    public interface IHTMLLocation
    {
        [DispId(0)]
        string href { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        get; }

        [DispId(1)]
        string protocol { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }

        [DispId(2)]
        string host { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        get; }

        [DispId(3)]
        string hostname { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        get; }

        [DispId(4)]
        string port { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        get; }

        [DispId(5)]
        string pathname { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        get; }

        [DispId(6)]
        string search { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        get; }

        [DispId(7)]
        string hash { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        void reload([In, Optional, DefaultParameterValue(false)] bool flag);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        void replace([In, MarshalAs(UnmanagedType.BStr)] string bstr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)]
        void assign([In, MarshalAs(UnmanagedType.BStr)] string bstr);

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        string toString();
    }

    [ComImport, DefaultMember("item"), Guid("332C4427-26CB-11D0-B483-00C04FD90119"), TypeLibType((short) 0x1040)]
    public interface IHTMLWindow2 : IHTMLFramesCollection2
    {
        [DispId(0x3e9)]
        new int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x44c)]
        FramesCollection frames { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x44c)]
        get; }

        [DispId(0x44d)]
        string defaultStatus { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x44d)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x44d)]
        get; }

        [DispId(0x44e)]
        string status { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x44e)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x44e)]
        get; }

        [DispId(0x465)]
        HTMLImageElementFactory Image { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x465)]
        get; }

        [DispId(14)]
        IHTMLLocation location { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)]
        get; }

        [DispId(2)]
        IOmHistory history { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        get; }

        [DispId(4)]
        object opener { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        get; }

        [DispId(5)]
        IOmNavigator navigator { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        get; }

        [DispId(11)]
        string name { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        get; }

        [DispId(12)]
        IHTMLWindow2 parent { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
        get; }

        [DispId(20)]
        IHTMLWindow2 self { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(20)]
        get; }

        [DispId(0x15)]
        IHTMLWindow2 top { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x15)]
        get; }

        [DispId(0x16)]
        IHTMLWindow2 window { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x16)]
        get; }

        [DispId(-2147412098)]
        object onfocus { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412098),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412098)]
        get; }

        [DispId(-2147412097)]
        object onblur { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412097)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412097)]
        get; }

        [DispId(-2147412080)]
        object onload { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412080),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412080),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412073)]
        object onbeforeunload { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412073)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412073),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412079)]
        object onunload { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412079)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412079)]
        get; }

        [DispId(-2147412099)]
        object onhelp { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412099),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412099)]
        get; }

        [DispId(-2147412083)]
        object onerror { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412083),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412083),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412076)]
        object onresize { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412076),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412076),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412081)]
        object onscroll { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412081)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412081)]
        get; }

        [DispId(0x47f)]
        IHTMLDocument2 document { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 2),
         DispId(0x47f)]
        get; }

        [DispId(0x480)]
        IHTMLEventObj @event { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x480)]
        get; }

        [DispId(0x481)]
        object _newEnum { [return: MarshalAs(UnmanagedType.IUnknown)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x481),
         TypeLibFunc((short) 0x41)]
        get; }

        [DispId(0x484)]
        IHTMLScreen screen { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x484)]
        get; }

        [DispId(0x485)]
        HTMLOptionElementFactory Option { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x485)]
        get; }

        [DispId(0x17)]
        bool closed { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x17)]
        get; }

        [DispId(0x489)]
        IOmNavigator clientInformation { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x489)]
        get; }

        [DispId(0x48c)]
        object offscreenBuffering { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48c)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48c)]
        get; }

        [DispId(0x491)]
        object external { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x491)]
        get; }

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        new object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x494)]
        int setTimeout([In, MarshalAs(UnmanagedType.BStr)] string expression, [In] int msec,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object language);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x450)]
        void clearTimeout([In] int timerID);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x451)]
        void alert([In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string message);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x456)]
        bool confirm([In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string message);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x457)]
        object prompt([In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string message,
                      [In, Optional, DefaultParameterValue("undefined"), MarshalAs(UnmanagedType.BStr)] string defstr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        void close();

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)]
        IHTMLWindow2 open([In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string url,
                          [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string name,
                          [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string features,
                          [In, Optional, DefaultParameterValue(false)] bool replace);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x19)]
        void navigate([In, MarshalAs(UnmanagedType.BStr)] string url);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x482)]
        object showModalDialog([In, MarshalAs(UnmanagedType.BStr)] string dialog,
                               [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object varArgIn,
                               [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object varOptions);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x483)]
        void showHelp([In, MarshalAs(UnmanagedType.BStr)] string helpURL,
                      [In, Optional, MarshalAs(UnmanagedType.Struct)] object helpArg,
                      [In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string features);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x486)]
        void focus();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x487)]
        void blur();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x488)]
        void scroll([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x495)]
        int setInterval([In, MarshalAs(UnmanagedType.BStr)] string expression, [In] int msec,
                        [In, Optional, MarshalAs(UnmanagedType.Struct)] ref object language);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48b)]
        void clearInterval([In] int timerID);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48d)]
        object execScript([In, MarshalAs(UnmanagedType.BStr)] string code,
                          [In, Optional, DefaultParameterValue("JScript"), MarshalAs(UnmanagedType.BStr)] string
                              language);

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48e)]
        string toString();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x48f)]
        void scrollBy([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x490)]
        void scrollTo([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        void moveTo([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        void moveBy([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        void resizeTo([In] int x, [In] int y);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        void resizeBy([In] int x, [In] int y);
    }

    [ComImport, DefaultMember("item"), Guid("3050F37E-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLStyleSheetsCollection : IEnumerable
    {
        [DispId(0x3e9)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [return:
            MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "",
                MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4),
         TypeLibFunc((short) 0x41)]
        new IEnumerator GetEnumerator();

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);
    }

    [ComImport, Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLStyleSheet
    {
        [DispId(0x3e9)]
        string title { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x3ea)]
        IHTMLStyleSheet parentStyleSheet { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        IHTMLElement owningElement { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        get; }

        [DispId(-2147418036)]
        bool disabled { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418036)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418036)]
        get; }

        [DispId(0x3ec)]
        bool readOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }

        [DispId(0x3ed)]
        IHTMLStyleSheetsCollection imports { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        get; }

        [DispId(0x3ee)]
        string href { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee)]
        get; }

        [DispId(0x3ef)]
        string type { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ef)]
        get; }

        [DispId(0x3f0)]
        string id { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f0)]
        get; }

        [DispId(0x3f5)]
        string media { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f5)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f5)]
        get; }

        [DispId(0x3f6)]
        string cssText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f6)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f6)]
        get; }

        [DispId(0x3f7)]
        IHTMLStyleSheetRulesCollection rules { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f7)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f1)]
        int addImport([In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
                      [In, Optional, DefaultParameterValue(-1)] int lIndex);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f2)]
        int addRule([In, MarshalAs(UnmanagedType.BStr)] string bstrSelector,
                    [In, MarshalAs(UnmanagedType.BStr)] string bstrStyle,
                    [In, Optional, DefaultParameterValue(-1)] int lIndex);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f3)]
        void removeImport([In] int lIndex);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f4)]
        void removeRule([In] int lIndex);
    }

    [ComImport, CoClass(typeof (HTMLImageElementFactoryClass)), Guid("3050F38E-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface HTMLImageElementFactory : IHTMLImageElementFactory
    {
    }

    [ComImport, Guid("3050F38F-98B5-11CF-BB82-00AA00BDCE0B"), ClassInterface((short) 0), DefaultMember("create")]
    public class HTMLImageElementFactoryClass : IHTMLImageElementFactory, HTMLImageElementFactory
    {
        // Methods

        #region IHTMLImageElementFactory Members

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        public virtual extern IHTMLImgElement create([In, Optional, MarshalAs(UnmanagedType.Struct)] object width,
                                                     [In, Optional, MarshalAs(UnmanagedType.Struct)] object height);

        #endregion
    }

    [ComImport, TypeLibType((short) 0x1040), DefaultMember("create"), Guid("3050F38E-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLImageElementFactory
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        IHTMLImgElement create([In, Optional, MarshalAs(UnmanagedType.Struct)] object width,
                               [In, Optional, MarshalAs(UnmanagedType.Struct)] object height);
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F240-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLImgElement
    {
        [DispId(0x7d2)]
        bool isMap { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x7d2)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7d2),
              TypeLibFunc((short) 20)]
        get; }

        [DispId(0x7d8)]
        string useMap { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7d8),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x7d8)]
        get; }

        [DispId(0x7da)]
        string mimeType { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7da)]
        get; }

        [DispId(0x7db)]
        string fileSize { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7db)]
        get; }

        [DispId(0x7dc)]
        string fileCreatedDate { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7dc)]
        get; }

        [DispId(0x7dd)]
        string fileModifiedDate { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7dd)]
        get; }

        [DispId(0x7de)]
        string fileUpdatedDate { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7de)]
        get; }

        [DispId(0x7df)]
        string protocol { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7df)]
        get; }

        [DispId(0x7e0)]
        string href { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7e0)]
        get; }

        [DispId(0x7e1)]
        string nameProp { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x7e1)]
        get; }

        [DispId(0x3ec)]
        object border { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3ed)]
        int vspace { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(0x3ed)]
        get; }

        [DispId(0x3ee)]
        int hspace { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3ee)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee),
              TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3ea)]
        string alt { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3ea)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        string src { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3eb)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3eb)]
        get; }

        [DispId(0x3ef)]
        string lowsrc { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3ef)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ef),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3f0)]
        string vrml { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f0)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f0)]
        get; }

        [DispId(0x3f1)]
        string dynsrc { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f1)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f1),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412996)]
        string readyState { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412996)]
        get; }

        [DispId(0x3f2)]
        bool complete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f2)]
        get; }

        [DispId(0x3f3)]
        object loop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f3)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f3)]
        get; }

        [DispId(-2147418039)]
        string align { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418039)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418039),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412080)]
        object onload { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412080),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412080),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412083)]
        object onerror { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412083),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412083)]
        get; }

        [DispId(-2147412084)]
        object onabort { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412084),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412084),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418112)]
        string name { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418112)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418112),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418107)]
        int width { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418107)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418107)]
        get; }

        [DispId(-2147418106)]
        int height { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418106)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418106)]
        get; }

        [DispId(0x3f5)]
        string Start { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f5),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3f5)]
        get; }
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("FECEAAA2-8405-11CF-8BA1-00AA00476DA6")]
    public interface IOmHistory
    {
        [DispId(1)]
        short length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        void back([In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvargdistance);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        void forward([In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvargdistance);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        void go([In, Optional, MarshalAs(UnmanagedType.Struct)] ref object pvargdistance);
    }

    [ComImport, Guid("FECEAAA5-8405-11CF-8BA1-00AA00476DA6"), TypeLibType((short) 0x1040)]
    public interface IOmNavigator
    {
        [DispId(1)]
        string appCodeName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }

        [DispId(2)]
        string appName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        get; }

        [DispId(3)]
        string appVersion { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        get; }

        [DispId(4)]
        string userAgent { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        get; }

        [DispId(7)]
        CMimeTypes mimeTypes { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        get; }

        [DispId(8)]
        IHTMLPluginsCollection plugins { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        get; }

        [DispId(9)]
        bool cookieEnabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        get; }

        [DispId(10)]
        COpsProfile opsProfile { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)]
        get; }

        [DispId(12)]
        string cpuClass { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
        get; }

        [DispId(13)]
        string systemLanguage { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)]
        get; }

        [DispId(14)]
        string browserLanguage { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x40),
         DispId(14)]
        get; }

        [DispId(15)]
        string userLanguage { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)]
        get; }

        [DispId(0x10)]
        string platform { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x10)]
        get; }

        [DispId(0x11)]
        string appMinorVersion { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x11)]
        get; }

        [DispId(0x12)]
        int connectionSpeed { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x12),
                               TypeLibFunc((short) 0x40)]
        get; }

        [DispId(0x13)]
        bool onLine { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x13)]
        get; }

        [DispId(20)]
        COpsProfile userProfile { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(20)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        bool javaEnabled();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        bool taintEnabled();

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        string toString();
    }

    [ComImport, Guid("3050F401-98B5-11CF-BB82-00AA00BDCE0B"), CoClass(typeof (COpsProfileClass))]
    public interface COpsProfile : IHTMLOpsProfile
    {
    }

    [ComImport, Guid("3050F402-98B5-11CF-BB82-00AA00BDCE0B"), ClassInterface((short) 0)]
    public class COpsProfileClass : IHTMLOpsProfile, COpsProfile
    {
        // Methods

        #region IHTMLOpsProfile Members

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        public virtual extern bool addReadRequest([In, MarshalAs(UnmanagedType.BStr)] string name,
                                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        public virtual extern bool addRequest([In, MarshalAs(UnmanagedType.BStr)] string name,
                                              [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        public virtual extern void clearRequest();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        public virtual extern bool commitChanges();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        public virtual extern void doReadRequest([In, MarshalAs(UnmanagedType.Struct)] object usage,
                                                 [In, Optional, MarshalAs(UnmanagedType.Struct)] object fname,
                                                 [In, Optional, MarshalAs(UnmanagedType.Struct)] object domain,
                                                 [In, Optional, MarshalAs(UnmanagedType.Struct)] object path,
                                                 [In, Optional, MarshalAs(UnmanagedType.Struct)] object expire,
                                                 [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        public virtual extern void doRequest([In, MarshalAs(UnmanagedType.Struct)] object usage,
                                             [In, Optional, MarshalAs(UnmanagedType.Struct)] object fname,
                                             [In, Optional, MarshalAs(UnmanagedType.Struct)] object domain,
                                             [In, Optional, MarshalAs(UnmanagedType.Struct)] object path,
                                             [In, Optional, MarshalAs(UnmanagedType.Struct)] object expire,
                                             [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        public virtual extern bool doWriteRequest();

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        public virtual extern string getAttribute([In, MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        public virtual extern bool setAttribute([In, MarshalAs(UnmanagedType.BStr)] string name,
                                                [In, MarshalAs(UnmanagedType.BStr)] string value,
                                                [In, Optional, MarshalAs(UnmanagedType.Struct)] object prefs);

        #endregion
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F401-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLOpsProfile
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        bool addRequest([In, MarshalAs(UnmanagedType.BStr)] string name,
                        [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        void clearRequest();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        void doRequest([In, MarshalAs(UnmanagedType.Struct)] object usage,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] object fname,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] object domain,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] object path,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] object expire,
                       [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        string getAttribute([In, MarshalAs(UnmanagedType.BStr)] string name);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        bool setAttribute([In, MarshalAs(UnmanagedType.BStr)] string name,
                          [In, MarshalAs(UnmanagedType.BStr)] string value,
                          [In, Optional, MarshalAs(UnmanagedType.Struct)] object prefs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
        bool commitChanges();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
        bool addReadRequest([In, MarshalAs(UnmanagedType.BStr)] string name,
                            [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)]
        void doReadRequest([In, MarshalAs(UnmanagedType.Struct)] object usage,
                           [In, Optional, MarshalAs(UnmanagedType.Struct)] object fname,
                           [In, Optional, MarshalAs(UnmanagedType.Struct)] object domain,
                           [In, Optional, MarshalAs(UnmanagedType.Struct)] object path,
                           [In, Optional, MarshalAs(UnmanagedType.Struct)] object expire,
                           [In, Optional, MarshalAs(UnmanagedType.Struct)] object reserved);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
        bool doWriteRequest();
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLEventObj
    {
        [DispId(0x3e9)]
        IHTMLElement srcElement { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x3ea)]
        bool altKey { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        bool ctrlKey { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        get; }

        [DispId(0x3ec)]
        bool shiftKey { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }

        [DispId(0x3ef)]
        object returnValue { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ef)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ef)]
        get; }

        [DispId(0x3f0)]
        bool cancelBubble { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f0)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f0)]
        get; }

        [DispId(0x3f1)]
        IHTMLElement fromElement { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f1)]
        get; }

        [DispId(0x3f2)]
        IHTMLElement toElement { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f2)]
        get; }

        [DispId(0x3f3)]
        int keyCode { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f3)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f3)]
        get; }

        [DispId(0x3f4)]
        int button { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f4)]
        get; }

        [DispId(0x3f5)]
        string type { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f5)]
        get; }

        [DispId(0x3f6)]
        string qualifier { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f6)]
        get; }

        [DispId(0x3f7)]
        int reason { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f7)]
        get; }

        [DispId(0x3ed)]
        int x { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        get; }

        [DispId(0x3ee)]
        int y { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee)]
        get; }

        [DispId(0x3fc)]
        int clientX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3fc)]
        get; }

        [DispId(0x3fd)]
        int clientY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3fd)]
        get; }

        [DispId(0x3fe)]
        int offsetX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3fe)]
        get; }

        [DispId(0x3ff)]
        int offsetY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ff)]
        get; }

        [DispId(0x400)]
        int screenX { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x400)]
        get; }

        [DispId(0x401)]
        int screenY { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x401)]
        get; }

        [DispId(0x402)]
        object srcFilter { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x402)]
        get; }
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F35C-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLScreen
    {
        [DispId(0x3e9)]
        int colorDepth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x3ea)]
        int bufferDepth { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        int width { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        get; }

        [DispId(0x3ec)]
        int height { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }

        [DispId(0x3ed)]
        int updateInterval { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        get; }

        [DispId(0x3ee)]
        int availHeight { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee)]
        get; }

        [DispId(0x3ef)]
        int availWidth { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ef)]
        get; }

        [DispId(0x3f0)]
        bool fontSmoothingEnabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f0)]
        get; }
    }

    [ComImport, Guid("3050F38C-98B5-11CF-BB82-00AA00BDCE0B"), CoClass(typeof (HTMLOptionElementFactoryClass))]
    public interface HTMLOptionElementFactory : IHTMLOptionElementFactory
    {
    }

    [ComImport, ClassInterface((short) 0), DefaultMember("create"), Guid("3050F38D-98B5-11CF-BB82-00AA00BDCE0B")]
    public class HTMLOptionElementFactoryClass : IHTMLOptionElementFactory, HTMLOptionElementFactory
    {
        // Methods

        #region IHTMLOptionElementFactory Members

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        public virtual extern IHTMLOptionElement create([In, Optional, MarshalAs(UnmanagedType.Struct)] object text,
                                                        [In, Optional, MarshalAs(UnmanagedType.Struct)] object value,
                                                        [In, Optional, MarshalAs(UnmanagedType.Struct)] object
                                                            defaultSelected,
                                                        [In, Optional, MarshalAs(UnmanagedType.Struct)] object selected);

        #endregion
    }

    [ComImport, DefaultMember("create"), TypeLibType((short) 0x1040), Guid("3050F38C-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLOptionElementFactory
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        IHTMLOptionElement create([In, Optional, MarshalAs(UnmanagedType.Struct)] object text,
                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object value,
                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object defaultSelected,
                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object selected);
    }

    [ComImport, Guid("3050F211-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLOptionElement
    {
        [DispId(0x3e9)]
        bool selected { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x3ea)]
        string value { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        bool defaultSelected { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        get; }

        [DispId(0x3ed)]
        int index { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        get; }

        [DispId(0x3ec)]
        string text { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }

        [DispId(0x3ee)]
        IHTMLFormElement form { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee)]
        get; }
    }

    [ComImport, Guid("3050F1D9-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLFontElement
    {
        [DispId(-2147413110)]
        object color { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413110)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413110)]
        get; }

        [DispId(-2147413094)]
        string face { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413094),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413094),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413093)]
        object size { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413093)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413093),
         TypeLibFunc((short) 20)]
        get; }
    }

    [ComImport, DefaultMember("item"), Guid("3050F2E5-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLStyleSheetRulesCollection
    {
        [DispId(0x3e9)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        IHTMLStyleSheetRule item([In] int index);
    }

    [ComImport, CoClass(typeof (CMimeTypesClass)), Guid("3050F3FC-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface CMimeTypes : IHTMLMimeTypesCollection
    {
    }

    [ComImport, ClassInterface((short) 0), Guid("3050F3FE-98B5-11CF-BB82-00AA00BDCE0B")]
    public class CMimeTypesClass : IHTMLMimeTypesCollection, CMimeTypes
    {
        // Properties

        #region IHTMLMimeTypesCollection Members

        [DispId(1)]
        public virtual extern int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }

        #endregion
    }

    [ComImport, Guid("3050F3FC-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLMimeTypesCollection
    {
        [DispId(1)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F3FD-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLPluginsCollection
    {
        [DispId(1)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        void refresh([In, Optional, DefaultParameterValue(false)] bool reload);
    }

    [ComImport, DefaultMember("item"), Guid("3050F1F7-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLFormElement : IEnumerable
    {
        [DispId(0x3e9)]
        string action { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3e9)]
        get; }

        [DispId(-2147412995)]
        string dir { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412995)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412995),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3eb)]
        string encoding { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3ec)]
        string method { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(0x3ed)]
        object elements { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ed)]
        get; }

        [DispId(0x3ee)]
        string target { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ee),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(0x3ee)]
        get; }

        [DispId(-2147418112)]
        string name { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418112)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418112),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412101)]
        object onsubmit { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412101)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412101)]
        get; }

        [DispId(-2147412100)]
        object onreset { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412100),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412100)]
        get; }

        [DispId(0x5dc)]
        int length { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dc)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dc)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f1)]
        void submit();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3f2)]
        void reset();

        [return:
            MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "",
                MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x41),
         DispId(-4)]
        new IEnumerator GetEnumerator();

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, Optional, MarshalAs(UnmanagedType.Struct)] object name,
                    [In, Optional, MarshalAs(UnmanagedType.Struct)] object index);

        [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5de)]
        object tags([In, MarshalAs(UnmanagedType.Struct)] object tagName);
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F357-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLStyleSheetRule
    {
        [DispId(0x3e9)]
        string selectorText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(-2147418038)]
        IHTMLRuleStyle style { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x400)
        , DispId(-2147418038)]
        get; }

        [DispId(0x3ea)]
        bool readOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }
    }

    [ComImport, Guid("3050F3CF-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLRuleStyle
    {
        [DispId(-2147413094)]
        string fontFamily { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413094),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413094)]
        get; }

        [DispId(-2147413088)]
        string fontStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413088)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413088)]
        get; }

        [DispId(-2147413087)]
        string fontVariant { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413087),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413087),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413085)]
        string fontWeight { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413085)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413085)]
        get; }

        [DispId(-2147413093)]
        object fontSize { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413093)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413093),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413071)]
        string font { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413071)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413071)]
        get; }

        [DispId(-2147413110)]
        object color { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413110),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413110)]
        get; }

        [DispId(-2147413080)]
        string background { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413080),
         TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413080),
         TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-501)]
        object backgroundColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-501)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-501),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413111)]
        string backgroundImage { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413111)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413111),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413068)]
        string backgroundRepeat { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413068)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413068)]
        get; }

        [DispId(-2147413067)]
        string backgroundAttachment { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413067),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413067)]
        get; }

        [DispId(-2147413066)]
        string backgroundPosition { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413066)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413066),
         TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-2147413079)]
        object backgroundPositionX { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413079),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413079),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413078)]
        object backgroundPositionY { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413078),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413078)]
        get; }

        [DispId(-2147413065)]
        object wordSpacing { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413065)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413065)]
        get; }

        [DispId(-2147413104)]
        object letterSpacing { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413104),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413104)]
        get; }

        [DispId(-2147413077)]
        string textDecoration { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413077),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413077)]
        get; }

        [DispId(-2147413089)]
        bool textDecorationNone { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413089),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413089),
              TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413091)]
        bool textDecorationUnderline { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413091),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147413091)]
        get; }

        [DispId(-2147413043)]
        bool textDecorationOverline { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413043),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413043),
              TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413092)]
        bool textDecorationLineThrough { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413092),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413092),
              TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413090)]
        bool textDecorationBlink { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413090),
         TypeLibFunc((short) 20)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
              TypeLibFunc((short) 20), DispId(-2147413090)]
        get; }

        [DispId(-2147413064)]
        object verticalAlign { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413064)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413064)]
        get; }

        [DispId(-2147413108)]
        string textTransform { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413108)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413108)]
        get; }

        [DispId(-2147418040)]
        string textAlign { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418040),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418040)]
        get; }

        [DispId(-2147413105)]
        object textIndent { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413105),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413105)]
        get; }

        [DispId(-2147413106)]
        object lineHeight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413106)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413106)]
        get; }

        [DispId(-2147413075)]
        object marginTop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413075)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413075)]
        get; }

        [DispId(-2147413074)]
        object marginRight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413074)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413074)]
        get; }

        [DispId(-2147413073)]
        object marginBottom { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413073)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413073),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413072)]
        object marginLeft { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413072),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413072),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413076)]
        string margin { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413076)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413076)]
        get; }

        [DispId(-2147413100)]
        object paddingTop { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413100),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413100),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413099)]
        object paddingRight { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413099)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413099)]
        get; }

        [DispId(-2147413098)]
        object paddingBottom { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413098)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413098)]
        get; }

        [DispId(-2147413097)]
        object paddingLeft { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413097),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413097)]
        get; }

        [DispId(-2147413101)]
        string padding { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413101)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413101),
         TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-2147413063)]
        string border { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413063)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413063)]
        get; }

        [DispId(-2147413062)]
        string borderTop { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413062),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413062),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413061)]
        string borderRight { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413061)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413061),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413060)]
        string borderBottom { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413060)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413060)]
        get; }

        [DispId(-2147413059)]
        string borderLeft { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413059)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413059),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413058)]
        string borderColor { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413058)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413058)]
        get; }

        [DispId(-2147413057)]
        object borderTopColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413057),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413057),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413056)]
        object borderRightColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413056)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413056),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413055)]
        object borderBottomColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413055)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413055),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413054)]
        object borderLeftColor { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413054)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413054)]
        get; }

        [DispId(-2147413053)]
        string borderWidth { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413053)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413053),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413052)]
        object borderTopWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413052)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413052),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413051)]
        object borderRightWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413051),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413051),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413050)]
        object borderBottomWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413050)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413050)]
        get; }

        [DispId(-2147413049)]
        object borderLeftWidth { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413049)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413049)]
        get; }

        [DispId(-2147413048)]
        string borderStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413048),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413048)]
        get; }

        [DispId(-2147413047)]
        string borderTopStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413047),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413047)]
        get; }

        [DispId(-2147413046)]
        string borderRightStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413046),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413046),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413045)]
        string borderBottomStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413045),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413045)]
        get; }

        [DispId(-2147413044)]
        string borderLeftStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413044),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413044)]
        get; }

        [DispId(-2147418107)]
        object width { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418107),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418107),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418106)]
        object height { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418106),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418106),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413042)]
        string styleFloat { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413042)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413042)]
        get; }

        [DispId(-2147413096)]
        string clear { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413096),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413096),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413041)]
        string display { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413041),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413041)]
        get; }

        [DispId(-2147413032)]
        string visibility { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413032)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413032)]
        get; }

        [DispId(-2147413040)]
        string listStyleType { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413040),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413040)]
        get; }

        [DispId(-2147413039)]
        string listStylePosition { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413039),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413039)]
        get; }

        [DispId(-2147413038)]
        string listStyleImage { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413038)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413038)]
        get; }

        [DispId(-2147413037)]
        string listStyle { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413037)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413037),
         TypeLibFunc((short) 0x414)]
        get; }

        [DispId(-2147413036)]
        string whiteSpace { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413036)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413036)]
        get; }

        [DispId(-2147418108)]
        object top { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418108)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418108),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418109)]
        object left { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418109)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418109),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413022)]
        string position { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413022)]
        get; }

        [DispId(-2147413021)]
        object zIndex { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413021)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413021),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413102)]
        string overflow { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413102)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413102),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413035)]
        string pageBreakBefore { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413035)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413035),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413034)]
        string pageBreakAfter { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413034)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413034)]
        get; }

        [DispId(-2147413013)]
        string cssText { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413013),
         TypeLibFunc((short) 0x414)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 0x414)
        , DispId(-2147413013)]
        get; }

        [DispId(-2147413010)]
        string cursor { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413010),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413010),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413020)]
        string clip { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413020),
         TypeLibFunc((short) 20)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413020),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413030)]
        string filter { [param: In, MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413030)]
        set; [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413030)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417611)]
        void setAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                          [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue,
                          [In, Optional, DefaultParameterValue(1)] int lFlags);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417610)]
        object getAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                            [In, Optional, DefaultParameterValue(0)] int lFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417609)]
        bool removeAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                             [In, Optional, DefaultParameterValue(1)] int lFlags);
    }

    [ComImport, Guid("3050F5DA-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLDOMNode
    {
        [DispId(-2147417066)]
        int nodeType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417066)]
        get; }

        [DispId(-2147417065)]
        IHTMLDOMNode parentNode { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417065)]
        get; }

        [DispId(-2147417063)]
        object childNodes { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417063)]
        get; }

        [DispId(-2147417062)]
        object attributes { [return: MarshalAs(UnmanagedType.IDispatch)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417062)]
        get; }

        [DispId(-2147417038)]
        string nodeName { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417038)]
        get; }

        [DispId(-2147417037)]
        object nodeValue { [param: In, MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417037)]
        set; [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417037)]
        get; }

        [DispId(-2147417036)]
        IHTMLDOMNode firstChild { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417036)]
        get; }

        [DispId(-2147417035)]
        IHTMLDOMNode lastChild { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417035)]
        get; }

        [DispId(-2147417034)]
        IHTMLDOMNode previousSibling { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417034)]
        get; }

        [DispId(-2147417033)]
        IHTMLDOMNode nextSibling { [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417033)]
        get; }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417064)]
        bool hasChildNodes();

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417061)]
        IHTMLDOMNode insertBefore([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild,
                                  [In, Optional, MarshalAs(UnmanagedType.Struct)] object refChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417060)]
        IHTMLDOMNode removeChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417059)]
        IHTMLDOMNode replaceChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild,
                                  [In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417051)]
        IHTMLDOMNode cloneNode([In] bool fDeep);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417046)]
        IHTMLDOMNode removeNode([In, Optional, DefaultParameterValue(false)] bool fDeep);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417044)]
        IHTMLDOMNode swapNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode otherNode);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417045)]
        IHTMLDOMNode replaceNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode replacement);

        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417039)]
        IHTMLDOMNode appendChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild);
    }

    [ComImport, DefaultMember("item"), Guid("3050F3EE-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLFiltersCollection : IEnumerable
    {
        [DispId(0x3e9)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [return:
            MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "",
                MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4),
         TypeLibFunc((short) 0x41)]
        new IEnumerator GetEnumerator();

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);
    }

    [ComImport, Guid("3050F3DB-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLCurrentStyle
    {
        [DispId(-2147413022)]
        string position { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413022),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413042)]
        string styleFloat { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413042),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413110)]
        object color { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413110),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-501)]
        object backgroundColor { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-501)]
        get; }

        [DispId(-2147413094)]
        string fontFamily { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413094)]
        get; }

        [DispId(-2147413088)]
        string fontStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413088)]
        get; }

        [DispId(-2147413087)]
        string fontVariant { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413087),
         TypeLibFunc((short) 0x54)]
        get; }

        [DispId(-2147413085)]
        object fontWeight { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413085)]
        get; }

        [DispId(-2147413093)]
        object fontSize { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413093)]
        get; }

        [DispId(-2147413111)]
        string backgroundImage { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413111)]
        get; }

        [DispId(-2147413079)]
        object backgroundPositionX { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413079)]
        get; }

        [DispId(-2147413078)]
        object backgroundPositionY { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413078)]
        get; }

        [DispId(-2147413068)]
        string backgroundRepeat { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413068),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413054)]
        object borderLeftColor { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413054),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413057)]
        object borderTopColor { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413057)]
        get; }

        [DispId(-2147413056)]
        object borderRightColor { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413056)]
        get; }

        [DispId(-2147413055)]
        object borderBottomColor { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413055)]
        get; }

        [DispId(-2147413047)]
        string borderTopStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413047)]
        get; }

        [DispId(-2147413046)]
        string borderRightStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413046)]
        get; }

        [DispId(-2147413045)]
        string borderBottomStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413045),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413044)]
        string borderLeftStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413044),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413052)]
        object borderTopWidth { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413052),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413051)]
        object borderRightWidth { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413051),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413050)]
        object borderBottomWidth { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413050),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413049)]
        object borderLeftWidth { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413049)]
        get; }

        [DispId(-2147418109)]
        object left { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418109)]
        get; }

        [DispId(-2147418108)]
        object top { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418108),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418107)]
        object width { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418107)]
        get; }

        [DispId(-2147418106)]
        object height { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418106),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413097)]
        object paddingLeft { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413097),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413100)]
        object paddingTop { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413100)]
        get; }

        [DispId(-2147413099)]
        object paddingRight { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413099)]
        get; }

        [DispId(-2147413098)]
        object paddingBottom { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413098)]
        get; }

        [DispId(-2147418040)]
        string textAlign { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418040)]
        get; }

        [DispId(-2147413077)]
        string textDecoration { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413077),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413041)]
        string display { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413041),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413032)]
        string visibility { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413032)]
        get; }

        [DispId(-2147413021)]
        object zIndex { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413021)]
        get; }

        [DispId(-2147413104)]
        object letterSpacing { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413104)]
        get; }

        [DispId(-2147413106)]
        object lineHeight { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413106)]
        get; }

        [DispId(-2147413105)]
        object textIndent { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413105),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413064)]
        object verticalAlign { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413064)]
        get; }

        [DispId(-2147413067)]
        string backgroundAttachment { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413067),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413075)]
        object marginTop { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413075)]
        get; }

        [DispId(-2147413074)]
        object marginRight { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413074),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413073)]
        object marginBottom { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413073),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413072)]
        object marginLeft { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413072),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413096)]
        string clear { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413096)]
        get; }

        [DispId(-2147413040)]
        string listStyleType { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413040),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413039)]
        string listStylePosition { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413039)]
        get; }

        [DispId(-2147413038)]
        string listStyleImage { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413038)]
        get; }

        [DispId(-2147413019)]
        object clipTop { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413019),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413018)]
        object clipRight { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413018),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413017)]
        object clipBottom { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413017)]
        get; }

        [DispId(-2147413016)]
        object clipLeft { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413016)]
        get; }

        [DispId(-2147413102)]
        string overflow { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413102),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413035)]
        string pageBreakBefore { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413035),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413034)]
        string pageBreakAfter { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413034),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413010)]
        string cursor { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413010),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413014)]
        string tableLayout { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413014),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413028)]
        string borderCollapse { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413028),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412993)]
        string direction { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412993)]
        get; }

        [DispId(-2147412997)]
        string behavior { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412997)]
        get; }

        [DispId(-2147412994)]
        string unicodeBidi { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412994),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147418035)]
        object right { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147418035)]
        get; }

        [DispId(-2147418034)]
        object bottom { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147418034),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412992)]
        string imeMode { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412992)]
        get; }

        [DispId(-2147412991)]
        string rubyAlign { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412991),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412990)]
        string rubyPosition { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412990)]
        get; }

        [DispId(-2147412989)]
        string rubyOverhang { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412989)]
        get; }

        [DispId(-2147412980)]
        string textAutospace { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412980),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412979)]
        string lineBreak { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412979),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412978)]
        string wordBreak { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412978),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412977)]
        string textJustify { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412977),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412976)]
        string textJustifyTrim { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412976)]
        get; }

        [DispId(-2147412975)]
        object textKashida { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412975)]
        get; }

        [DispId(-2147412995)]
        string blockDirection { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412995)]
        get; }

        [DispId(-2147412985)]
        object layoutGridChar { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412985)]
        get; }

        [DispId(-2147412984)]
        object layoutGridLine { [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412984),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412983)]
        string layoutGridMode { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412983),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412982)]
        string layoutGridType { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412982),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413048)]
        string borderStyle { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413048),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413058)]
        string borderColor { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413058)]
        get; }

        [DispId(-2147413053)]
        string borderWidth { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413053)]
        get; }

        [DispId(-2147413101)]
        string padding { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413101)]
        get; }

        [DispId(-2147413076)]
        string margin { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147413076),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412965)]
        string accelerator { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147412965)]
        get; }

        [DispId(-2147412973)]
        string overflowX { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412973),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147412972)]
        string overflowY { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147412972),
         TypeLibFunc((short) 20)]
        get; }

        [DispId(-2147413108)]
        string textTransform { [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short) 20),
         DispId(-2147413108)]
        get; }

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-2147417610)]
        object getAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName,
                            [In, Optional, DefaultParameterValue(0)] int lFlags);
    }

    [ComImport, TypeLibType((short) 0x1040), Guid("3050F4A3-98B5-11CF-BB82-00AA00BDCE0B")]
    public interface IHTMLRect
    {
        [DispId(0x3e9)]
        int left { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3e9)]
        get; }

        [DispId(0x3ea)]
        int top { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ea)]
        get; }

        [DispId(0x3eb)]
        int right { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3eb)]
        get; }

        [DispId(0x3ec)]
        int bottom { [param: In]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        set; [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x3ec)]
        get; }
    }

    [ComImport, DefaultMember("item"), Guid("3050F4A4-98B5-11CF-BB82-00AA00BDCE0B"), TypeLibType((short) 0x1040)]
    public interface IHTMLRectCollection : IEnumerable
    {
        [DispId(0x5dc)]
        int length { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x5dc)]
        get; }

        [return:
            MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "",
                MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4),
         TypeLibFunc((short) 0x41)]
        new IEnumerator GetEnumerator();

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)]
        object item([In, MarshalAs(UnmanagedType.Struct)] ref object pvarIndex);
    }
}