using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Shared
{
    public interface IDocument
    {
        /// <summary>
        /// The body attribute, on getting, must return the body element of the document (either a body element, a frameset element, or null). On setting, the following algorithm must be run:
        /// If the new value is not a body or frameset element, then raise a HIERARCHY_REQUEST_ERR exception and abort these steps.
        /// Otherwise, if the new value is the same as the body element, do nothing. Abort these steps.
        /// Otherwise, if the body element is not null, then replace that element with the new value in the DOM, as if the root element's replaceChild() method had been called with the new value and the incumbent body element as its two arguments respectively, then abort these steps.
        /// Otherwise, the body element is null. Append the new value to the root element.
        /// </summary>
        object body { get; set; }

        /// <summary>
        /// Returns the title of the current document.
        /// </summary>
        string title { get; set; }

        /// <summary>
        /// The location attribute of the HTMLDocument interface must return the Location object for that Document object, if it is in a browsing context, and null otherwise.
        /// </summary>
        ILocation location { get; set; }

        /// <summary>
        /// Creates an element with the specified tag name defaulting namespace depending on the document.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        object createElement(string tagName);

        /// <summary>
        /// Creates an element with the specified namespace URI and qualified name.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        object createElementNS(string ns, string tagName);

        /// <summary>
        /// The defaultView IDL attribute of the HTMLDocument interface must return the Document's browsing context's WindowProxy object, if there is one, or null otherwise.
        /// </summary>
        IWindow defaultView { get; }
    }
}
