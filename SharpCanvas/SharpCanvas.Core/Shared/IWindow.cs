using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCanvas.Shared
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IWindow : IGlobalScope
    {
        object? parentNode { get; }
        object childNodes { get; }
        void appendChild(object child);
        void removeChild(object child);
        object? ownerDocument { get; }

        // assigning this has special behavior in ECMAScript, but it is otherwise
        // read only. specifically, in ES a string URI can be assigned to location,
        // having the same effect as location.assign(URI)
        [DispId(2000000)]
        ILocation? location { get; set; }

        /// <summary>
        /// The value of the window attribute MUST be the same Window object that has the attribute: the attribute is a self-reference.
        /// </summary>
        IWindow self { get; }

        /// <summary>
        /// The value of the window attribute MUST be the same Window object that has the attribute: the attribute is a self-reference.
        /// </summary>
        IWindow window { get; }

        /// <summary>
        /// Returns a reference to the document that the window contains.
        /// </summary>
        IDocument document { get; }

        /// <summary>
        /// The value of the parent attribute of a Window object MUST be the parent document's Window object or the document's Window object 
        /// if there is no parent document.
        /// </summary>
        IDocument parent { get; set; }

        //object globalScope { get; set; }

        /// <summary>
        /// An event handler for the load event of a window.
        /// </summary>
        object onload { get; set; }
        
        /// <summary>
        /// Gets the height of the content area of the browser window including, if rendered, the horizontal scrollbar.
        /// </summary>
        int innerHeight { get; set; }
        
        /// <summary>
        /// Gets the width of the content area of the browser window including, if rendered, the vertical scrollbar.
        /// </summary>
        int innerWidth { get; set; }
        
        /// <summary>
        /// Whenever the src attribute is set, the user agent must resolve the value of that attribute, relative to the element, and if that is successful, 
        /// the nested browsing context must be navigated to the resulting absolute URL, with the frame element's document's browsing context as the source browsing context.
        /// </summary>
        string src { get; set; }

        /// <summary>
        /// An attribute containing a unique name used to refer to this Window object.
        /// Need to describe how this could come from a containing element. 
        /// </summary>
        string name { get; set; }

        /// <summary>
        /// An attribute containing a reference to the topmost Window object in the hierarchy that contains this object.
        /// </summary>
        object top { get; set; }

        /// <summary>
        /// referencing <html:frame>, <html:iframe>, <html:object>, <svg:foreignObject>,
        /// <svg:animation> or other embedding point, or null if none
        /// </summary>
        object frameElement { get; set; }

        /// <summary>
        /// Reference to parent window
        /// </summary>
        IWindow? parentWindow { get; set; }

        /// <summary>
        /// Left position of the control
        /// </summary>
        int Left { get; set; }

        /// <summary>
        /// Top position of the control
        /// </summary>
        int Top { get; set; }


        void setAttribute(object name, object value);

        /// <summary>
        /// Executes a code snippet or a function after specified delay.
        /// </summary>
        /// <param name="func">func is the function you want to execute after delay milliseconds</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the function call should be delayed by.</param>
        /// <returns>timeoutID is the ID of the timeout, which can be used with window.clearTimeout.</returns>
        int setTimeout(object func, object milliseconds);

        /// <summary>
        /// Clears the delay set by window.setTimeout().
        /// </summary>
        /// <param name="key">where key is the ID of the timeout you wish to clear, as returned by window.setTimeout().</param>
        void clearTimeout(int key);

        /// <summary>
        /// Calls a function repeatedly, with a fixed time delay between each call to that function.
        /// </summary>
        /// <param name="func">func is the function you want to be called repeatedly.</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the setInterval() function should wait before each call to func.</param>
        /// <returns>unique interval ID you can pass to clearInterval().</returns>
        int setInterval(object func, object milliseconds);

        /// <summary>
        /// Cancels repeated action which was set up using setInterval(). 
        /// </summary>
        /// <param name="key">is the identifier of the repeated action you want to cancel. This ID is returned from setInterval(). </param>
        void clearInterval(int key);

        /// <summary>
        /// Represents the identity and state of the user agent (the client), and allows Web pages to register themselves as potential protocol and content handlers.
        /// </summary>
        INavigator navigator { get; }

        /// <summary>
        /// Redraw visible only childrens (take in count z-index of the children)
        /// </summary>
        void RedrawChildren();

        IEventModel eventModel { get; }
    }
}
