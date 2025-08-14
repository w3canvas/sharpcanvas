using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpCanvas.Shared
{

    #region Delegates

    public delegate void OnSaveFileHandler(byte[] data);

    #endregion


    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILocation
    {
        // pieces of the URI, per the generic URI syntax
        [DispId(3000000)]
        object hash { get; set; }
        //This attribute represents the network host of the Location's URI. 
        // If the port attribute is not null then the host attribute's value is the concatenation of the hostname attribute, 
        // a colon (:) and the port attribute. If the port attribute is null then the host attribute's value is identical to the hostname attribute.
        [DispId(3000001)]
        object host { get; set; }
        // This attribute represents the name or IP address of the network location without any port number.
        [DispId(3000002)]
        object hostname { get; set; }
        // that is the Location's current location. When the href attribute is set, 
        // the Location's window MUST navigate to the newly set value.
        [DispId(3000003)]
        string href { get; set; }
        // This attribute represents the path component of the Location's URI which consists of everything after the host and port up to 
        // and excluding the first question mark (?) or hash mark (#).
        [DispId(3000003)]
        object pathname { get; set; }
        // This attribute represents the port number of the network location.
        [DispId(3000004)]
        object port { get; set; }
        // This attribute represents the scheme of the URI including the trailing colon (:)
        [DispId(3000005)]
        object protocol { get; set; }
        // This attribute represents the query portion of a URI. It consists of everything after the pathname up to and excluding the first hash mark (#).
        [DispId(3000006)]
        object search { get; set; }

        Assembly? assembly { get; }

        void assign(string url);
        void replace(string url);
        void reload();

        //fires when some PDF file loaded
        event OnSaveFileHandler OnSaveFile;
    }
}