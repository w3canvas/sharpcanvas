using System;
using System.Runtime.InteropServices;

namespace SharpCanvas.Interop
{
    [ComVisible(true), Guid("3050f69a-98b5-11cf-bb82-00aa00bdce0b"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
    TypeLibType(TypeLibTypeFlags.FDispatchable)
    ]
    public interface IHTMLDocument4
    {
        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_FOCUS)]
        void focus();

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_HASFOCUS)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool hasFocus();

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
        object onselectionchange { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_NAMESPACES)] //IHTMLNamespaceCollection
        object namespaces { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL)]
        IHTMLDocument2 createDocumentFromUrl([In, MarshalAs(UnmanagedType.BStr)] String bstrUrl, [In, MarshalAs(UnmanagedType.BStr)] String bstrOptions);

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_MEDIA)]
        String media { set; [return: MarshalAs(UnmanagedType.BStr)] get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_CREATEEVENTOBJECT)]
        IHTMLEventObj CreateEventObject();

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_FIREEVENT)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        bool FireEvent([In, MarshalAs(UnmanagedType.BStr)] String bstrEventName, [In] object pvarEventObject);

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_CREATERENDERSTYLE)]
        [return: MarshalAs(UnmanagedType.Interface)]
        IHTMLRenderStyle createRenderStyle(IntPtr v);
        //Must be set to NULL
        //[In, MarshalAs(UnmanagedType.BStr)] String v);

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_ONCONTROLSELECT)]
        object oncontrolselect { set; get; }

        [DispId(HTMLDispIDs.DISPID_IHTMLDOCUMENT4_URLUNENCODED)]
        String URLUnencoded { [return: MarshalAs(UnmanagedType.BStr)] get; }
    }
}
