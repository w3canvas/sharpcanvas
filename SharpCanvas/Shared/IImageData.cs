namespace SharpCanvas
{
    public interface IImageData
    {
        Microsoft.JScript.ArrayObject data { get; set; }
        string alt { get; set; }
        string src { get; set; }
        string useMap { get; set; }
        bool isMap { get; set; }
        uint width { get; set; }
        uint height { get; set; }
    }
}