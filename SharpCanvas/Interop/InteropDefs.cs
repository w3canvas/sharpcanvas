using System;
using System.Runtime.InteropServices;
using DISPPARAMS=System.Runtime.InteropServices.ComTypes.DISPPARAMS;
using EXCEPINFO=System.Runtime.InteropServices.ComTypes.EXCEPINFO;

namespace SharpCanvas.Interop
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F672-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementNamespaceFactory
    {
        void Create(IElementNamespace pNamespace);
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F671-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementNamespace
    {
        void AddTag(string bstrTagName, int lFlags);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F429-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementBehaviorFactory
    {
        IElementBehavior FindBehavior(string bstrBehavior, string bstrBehaviorUrl, IElementBehaviorSite pSite);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F427-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementBehaviorSite
    {
        IHTMLElement GetElement();
        void RegisterNotification(int lEvent);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct tagPOINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct tagSIZE
    {
        public int cx;
        public int cy;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct tagRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public enum _BEHAVIOR_LAYOUT_MODE
    {
        BEHAVIOR_LAYOUT_MODE_Max = 0x7fffffff,
        BEHAVIORLAYOUTMODE_FINAL_PERCENT = 0x8000,
        BEHAVIORLAYOUTMODE_MAXWIDTH = 4,
        BEHAVIORLAYOUTMODE_MEDIA_RESOLUTION = 0x4000,
        BEHAVIORLAYOUTMODE_MINWIDTH = 2,
        BEHAVIORLAYOUTMODE_NATURAL = 1
    }

    public enum _BEHAVIOR_LAYOUT_INFO
    {
        BEHAVIOR_LAYOUT_INFO_Max = 0x7fffffff,
        BEHAVIORLAYOUTINFO_FULLDELEGATION = 1,
        BEHAVIORLAYOUTINFO_MAPSIZE = 4,
        BEHAVIORLAYOUTINFO_MODIFYNATURAL = 2
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F6BA-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementBehaviorLayout
    {
        void GetSize([In] int dwFlags, [In] tagSIZE sizeContent, [In, Out] ref tagPOINT pptTranslateBy,
                     [In, Out] ref tagPOINT pptTopLeft, [In, Out] ref tagSIZE psizeProposed);

        int GetLayoutInfo();
        void GetPosition([In] int lFlags, [In, Out] ref tagPOINT pptTopLeft);
        void MapSize([In] ref tagSIZE psizeIn, out tagRECT prcOut);
    }

    public enum _BEHAVIOR_EVENT
    {
        BEHAVIOR_EVENT_Max = 0x7fffffff,
        BEHAVIOREVENT_APPLYSTYLE = 2,
        BEHAVIOREVENT_CONTENTREADY = 0,
        BEHAVIOREVENT_CONTENTSAVE = 4,
        BEHAVIOREVENT_DOCUMENTCONTEXTCHANGE = 3,
        BEHAVIOREVENT_DOCUMENTREADY = 1,
        BEHAVIOREVENT_FIRST = 0,
        BEHAVIOREVENT_LAST = 4
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F425-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IElementBehavior
    {
        void Init(IElementBehaviorSite pBehaviorSite);
        void Notify(int lEvent, IntPtr pVar);
        void Detach();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _HTML_PAINTER_INFO
    {
        public int lFlags;
        public int lZOrder;
        public Guid iidDrawObject;
        public tagRECT rcExpand;
    }

    public enum _HTML_PAINT_DRAW_INFO_FLAGS
    {
        HTMLPAINT_DRAWINFO_VIEWPORT = 1,
        HTMLPAINT_DRAWINFO_UPDATEREGION = 2,
        HTMLPAINT_DRAWINFO_XFORM = 4,
        HTML_PAINT_DRAW_INFO_FLAGS_Max = 2147483647
    }

    public struct _HTML_PAINT_DRAW_INFO
    {
        public tagRECT rcViewport;
        public IntPtr hrgnUpdate;
        public _HTML_PAINT_XFORM xform;
    }

    public struct _HTML_PAINT_XFORM
    {
        public float eM11;
        public float eM12;
        public float eM21;
        public float eM22;
        public float eDx;
        public float eDy;
    }

    public enum _HTML_PAINTER
    {
        HTML_PAINTER_Max = 0x7fffffff,
        HTMLPAINTER_3DSURFACE = 0x200,
        HTMLPAINTER_ALPHA = 4,
        HTMLPAINTER_COMPLEX = 8,
        HTMLPAINTER_EXPAND = 0x10000,
        HTMLPAINTER_HITTEST = 0x20,
        HTMLPAINTER_NOBAND = 0x400,
        HTMLPAINTER_NODC = 0x1000,
        HTMLPAINTER_NOPHYSICALCLIP = 0x2000,
        HTMLPAINTER_NOSAVEDC = 0x4000,
        HTMLPAINTER_NOSCROLLBITS = 0x20000,
        HTMLPAINTER_OPAQUE = 1,
        HTMLPAINTER_OVERLAY = 0x10,
        HTMLPAINTER_SUPPORTS_XFORM = 0x8000,
        HTMLPAINTER_SURFACE = 0x100,
        HTMLPAINTER_TRANSPARENT = 2
    }

    public enum _HTML_PAINT_ZORDER
    {
        HTML_PAINT_ZORDER_Max = 0x7fffffff,
        HTMLPAINT_ZORDER_ABOVE_CONTENT = 7,
        HTMLPAINT_ZORDER_ABOVE_FLOW = 6,
        HTMLPAINT_ZORDER_BELOW_CONTENT = 4,
        HTMLPAINT_ZORDER_BELOW_FLOW = 5,
        HTMLPAINT_ZORDER_NONE = 0,
        HTMLPAINT_ZORDER_REPLACE_ALL = 1,
        HTMLPAINT_ZORDER_REPLACE_BACKGROUND = 3,
        HTMLPAINT_ZORDER_REPLACE_CONTENT = 2,
        HTMLPAINT_ZORDER_WINDOW_TOP = 8
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3050F6A6-98B5-11CF-BB82-00AA00BDCE0B")]
    [ComVisible(true)]
    [ComImport]
    public interface IHTMLPainter
    {
        void Draw(tagRECT rcBounds, tagRECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject);
        void OnResize(tagSIZE size);
        void GetPainterInfo(out _HTML_PAINTER_INFO pInfo);
        void HitTestPoint(tagPOINT pt, out int pbHit, out int plPartID);
        //Invalidate ActiveX surface
        void ReDraw();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00020400-0000-0000-C000-000000000046")]
    [ComVisible(true)]
    [ComImport]
    public interface IDispatch
    {
        void GetTypeInfoCount(
            out uint pctinfo
            );

        void GetTypeInfo(
            uint iTInfo,
            uint lcid,
            out IntPtr ppTInfo
            );

        void GetIDsOfNames(
            ref Guid riid,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 2)] string[]
                rgszNames,
            uint cNames,
            uint lcid,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4, SizeParamIndex = 2)] uint[] rgDispId
            );

        void Invoke(
            uint dispIdMember,
            ref Guid riid,
            uint lcid,
            ushort wFlags,
            ref DISPPARAMS pDispParams,
            ref object pVarResult,
            IntPtr pExcepInfo,
            IntPtr puArgErr
            );
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3EEF9758-35FC-11D1-8CE4-00C04FC2B093")]
    [ComVisible(true)]
    [ComImport]
    public interface FunctionInstance
    {
        [DispId(0x000002bc)]
        object length { get; set; }

        [DispId(0x000002bd)]
        StringInstance toString();

        [DispId(0x000002be)]
        StringInstance toLocaleString();

        [DispId(0x000002bf)]
        FunctionInstance valueOf();

        [DispId(0x000002c0)]
        object apply(
            object thisValue,
            object argArray);

        [DispId(0x000002c1)]
        object call(object thisValue);

        [DispId(0x000002c2)]
        object hasOwnProperty(object propertyName);

        [DispId(0x000002c3)]
        object propertyIsEnumerable(object propertyName);

        [DispId(0x000002c4)]
        object isPrototypeOf(object obj);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3EEF9758-35FC-11D1-8CE4-00C04FC2B094")]
    [ComVisible(true)]
    [ComImport]
    public interface StringInstance
    {
        [DispId(0x00000320)]
        object length { get; set; }

        [DispId(0x00000321)]
        StringInstance toString();

        [DispId(0x00000322)]
        StringInstance valueOf();

        [DispId(0x00000323)]
        StringInstance anchor(object anchorString);

        [DispId(0x00000324)]
        StringInstance big();

        [DispId(0x00000325)]
        StringInstance blink();

        [DispId(0x00000326)]
        StringInstance bold();

        [DispId(0x00000327)]
        StringInstance charAt(object index);

        [DispId(0x00000328)]
        object charCodeAt(object index);

        [DispId(0x00000329)]
        StringInstance concat(object String);

        [DispId(0x0000032a)]
        StringInstance @fixed();

        [DispId(0x0000032b)]
        StringInstance fontcolor(object colorval);

        [DispId(0x0000032c)]
        StringInstance fontsize(object size);

        [DispId(0x0000032d)]
        object indexOf(
            object substring,
            object startindex);

        [DispId(0x0000032e)]
        StringInstance italics();

        [DispId(0x0000032f)]
        object lastIndexOf(
            object substring,
            object startindex);

        [DispId(0x00000330)]
        StringInstance link(object linkstring);

        [DispId(0x00000331)]
        object match(object RegExp);

        [DispId(0x00000332)]
        StringInstance replace(
            object RegExp,
            object replacetext);

        [DispId(0x00000333)]
        object search(object RegExp);

        [DispId(0x00000334)]
        StringInstance slice(
            object start,
            object end);

        [DispId(0x00000335)]
        StringInstance small();

        [DispId(0x00000336)]
        ArrayInstance split(object RegExp);

        [DispId(0x00000337)]
        StringInstance strike();

        [DispId(0x00000338)]
        StringInstance sub();

        [DispId(0x00000339)]
        StringInstance substring(
            object start,
            object end);

        [DispId(0x0000033a)]
        StringInstance substr(
            object start,
            object length);

        [DispId(0x0000033b)]
        StringInstance sup();

        [DispId(0x0000033c)]
        StringInstance toLowerCase();

        [DispId(0x0000033d)]
        StringInstance toUpperCase();

        [DispId(0x0000033e)]
        StringInstance toLocaleLowerCase();

        [DispId(0x0000033f)]
        StringInstance toLocaleUpperCase();

        [DispId(0x00000340)]
        object localeCompare(object that);

        [DispId(0x00000341)]
        object hasOwnProperty(object propertyName);

        [DispId(0x00000342)]
        object propertyIsEnumerable(object propertyName);

        [DispId(0x00000343)]
        object isPrototypeOf(object obj);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3EEF9758-35FC-11D1-8CE4-00C04FC2B092")]
    [ComImport]
    [ComVisible(true)]
    public interface ArrayInstance
    {
        [DispId(0x00000258)]
        object length { get; set; }

        [DispId(0x00000259)]
        ArrayInstance concat(object Array);

        [DispId(0x0000025a)]
        StringInstance join(object separator);

        [DispId(0x0000025b)]
        object pop();

        [DispId(0x0000025c)]
        object push(object value);

        [DispId(0x0000025d)]
        ArrayInstance reverse();

        [DispId(0x0000025e)]
        object shift();

        [DispId(0x0000025f)]
        ArrayInstance slice(
            object start,
            object end);

        [DispId(0x00000260)]
        ArrayInstance sort(object sortfunction);

        [DispId(0x00000261)]
        ArrayInstance splice(
            object start,
            object deletecount);

        [DispId(0x00000262)]
        StringInstance toString();

        [DispId(0x00000263)]
        StringInstance toLocaleString();

        [DispId(0x00000264)]
        ArrayInstance valueOf();

        [DispId(0x00000265)]
        object unshift(object value);

        [DispId(0x00000266)]
        object hasOwnProperty(object propertyName);

        [DispId(0x00000267)]
        object propertyIsEnumerable(object propertyName);

        [DispId(0x00000268)]
        object isPrototypeOf(object obj);
    }

    public enum SCRIPTSTATE
    {
        SCRIPTSTATE_UNINITIALIZED = 0,
        SCRIPTSTATE_INITIALIZED = 5,
        SCRIPTSTATE_STARTED = 1,
        SCRIPTSTATE_CONNECTED = 2,
        SCRIPTSTATE_DISCONNECTED = 3,
        SCRIPTSTATE_CLOSED = 4
    }

    public enum SCRIPTTHREADSTATE
    {
        SCRIPTTHREADSTATE_NOTINSCRIPT = 0,
        SCRIPTTHREADSTATE_RUNNING = 1
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BB1A2AE1-A4F9-11cf-8F20-00805F2CD064")]
    [ComImport]
    [ComVisible(true)]
    public interface IActiveScript
    {
        void SetScriptSite(
            [In] IntPtr pass
            );

        void GetScriptSite(
            [In] ref Guid riid,
            [Out] out IntPtr ppvObject
            );

        void SetScriptState(
            [In] SCRIPTSTATE ss
            );

        void GetScriptState(
            [Out] out SCRIPTSTATE pssState
            );

        void Close();

        void AddNamedItem(
            [In] string pstrName,
            [In] uint dwFlags
            );

        void AddTypeLib(
            [In] ref Guid rguidTypeLib,
            [In] uint dwMajor,
            [In] uint dwMinor,
            [In] uint dwFlags
            );

        void GetScriptDispatch(
            [In] string pstrItemName,
            [Out] out object ppdisp
            );

        void GetCurrentScriptThreadID(
            [Out] out uint pstidThread
            );

        void GetScriptThreadID(
            [In] uint dwWin32ThreadId,
            [Out] out uint pstidThread
            );

        void GetScriptThreadState(
            [In] uint stidThread,
            [Out] out SCRIPTTHREADSTATE pstsState
            );

        void InterruptScriptThread(
            [In] uint stidThread,
            [In] ref EXCEPINFO pexcepinfo,
            [In] uint dwFlags
            );
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("CB5BDC81-93C1-11cf-8F20-00805F2CD064")]
    [ComImport]
    [ComVisible(true)]
    public interface IObjectSafety
    {
        void GetInterfaceSafetyOptions(
            [In] ref Guid riid, // Interface that we want options for
            [Out] out uint pdwSupportedOptions, // Options meaningful on this interface
            [Out] out uint pdwEnabledOptions); // current option values on this interface

        void SetInterfaceSafetyOptions(
            [In] ref Guid riid, // Interface to set options for
            [In] uint dwOptionSetMask, // Options to change
            [In] uint dwEnabledOptions); // New option values
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000001-0000-0000-C000-000000000046")]
    [ComImport]
    public interface IClassFactory
    {
        void CreateInstance(
            [In] IntPtr pUnkOuter,
            [In] ref Guid riid,
            [Out] out IntPtr ppvObject);

        void LockServer(
            [In] bool fLock);
    }
}