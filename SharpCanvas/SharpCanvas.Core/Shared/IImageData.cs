namespace SharpCanvas.Shared
{
    public interface IImageData
    {
        object data { get; set; }
        string alt { get; set; }
        string src { get; set; }
        string useMap { get; set; }
        bool isMap { get; set; }
        uint width { get; set; }
        uint height { get; set; }
    }
}