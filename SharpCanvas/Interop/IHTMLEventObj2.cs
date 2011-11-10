using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.Interop
{
    #region IHTMLEventObj2 Interface
    [ComVisible(true), ComImport()]
    [TypeLibType((short)4160)] //TypeLibTypeFlags.FDispatchable
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3050f48B-98b5-11cf-bb82-00aa00bdce0b")]
    public interface IHTMLEventObj2
    {
        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SETATTRIBUTE)]
        void setAttribute([MarshalAs(UnmanagedType.BStr)] string strAttributeName, object AttributeValue, int lFlags);

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_GETATTRIBUTE)]
        object getAttribute([MarshalAs(UnmanagedType.BStr)] string strAttributeName, int lFlags);

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_REMOVEATTRIBUTE)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool removeAttribute([MarshalAs(UnmanagedType.BStr)] string strAttributeName, int lFlags);

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_PROPERTYNAME)]
        string propertyName { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_BOOKMARKS)]
        object bookmarks { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_RECORDSET)]
        object recordset { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_DATAFLD)]
        string dataFld { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_BOUNDELEMENTS)]
        object boundElements { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_REPEAT)]
        bool repeat { set; [return: MarshalAs(UnmanagedType.VariantBool)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SRCURN)]
        string srcUrn { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SRCELEMENT)]
        IHTMLElement SrcElement { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_ALTKEY)]
        bool AltKey { set; get; }
        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_CTRLKEY)]
        bool CtrlKey { set; get; }
        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SHIFTKEY)]
        bool ShiftKey { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_FROMELEMENT)]
        IHTMLElement FromElement { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_TOELEMENT)]
        IHTMLElement ToElement { set; [return: MarshalAs(UnmanagedType.Interface)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_BUTTON)]
        int Button { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_TYPE)]
        string EventType { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_QUALIFIER)]
        string Qualifier { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_REASON)]
        int reason { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_X)]
        int x { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_Y)]
        int y { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_CLIENTX)]
        int clientX { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_CLIENTY)]
        int clientY { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_OFFSETX)]
        int offsetX { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_OFFSETY)]
        int offsetY { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SCREENX)]
        int screenX { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SCREENY)]
        int screenY { get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_SRCFILTER)]
        object srcFilter { [return: MarshalAs(UnmanagedType.IDispatch)] get; set; }

        [DispId(HTMLDispIDs.DISPID_IHTMLEVENTOBJ2_DATATRANSFER)]
        object dataTransfer { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
    }

    #endregion
}