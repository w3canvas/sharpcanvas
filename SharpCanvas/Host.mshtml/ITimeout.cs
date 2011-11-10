namespace SharpCanvas.Host.Browser
{
    /// <summary>
    /// Define the set of methods neccessary for timeout support
    /// </summary>
    public interface ITimeout
    {
        int setTimeout(object func, object milliseconds);
        void clearTimeout(int key);

        int setInterval(object func, object milliseconds);
        void clearInterval(int key);
    }
}