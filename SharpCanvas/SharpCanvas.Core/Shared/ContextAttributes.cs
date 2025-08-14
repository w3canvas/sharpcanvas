namespace SharpCanvas.Shared
{
    public interface IContextAttributes
    {
        bool alpha { get; }
        string colorSpace { get; }
        bool desynchronized { get; }
        bool willReadFrequently { get; }
    }

    public class ContextAttributes : IContextAttributes
    {
        public bool alpha { get; set; }
        public string colorSpace { get; set; } = "srgb";
        public bool desynchronized { get; set; }
        public bool willReadFrequently { get; set; }
    }
}
