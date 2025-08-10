namespace SharpCanvas
{
    public interface IImage
    {
        int width { get; set; }
        int height { get; set; }
        string src { get; set; }
        System.Action<object> onload { get; set; }
//        void drawImage(object image);
        object getImage();
    }
}