using SharpCanvas.Interop;

namespace SharpCanvas.Interop
{
    public interface IStyleSupported
    {
        //we're using this method to set canvas position on the page
        //and to maintain canvas visibility, should be called before getContext method
        //TODO: add ability to call the method each time it needed and perform appropriate actions
        void setAttribute(string name, object value, IHTMLElement element);
    }
}