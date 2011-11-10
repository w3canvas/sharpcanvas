using System.Windows.Forms;
using Microsoft.JScript;

namespace SharpCanvas.Interop
{
    public delegate void ControlsTreeChangeHandler();

    public interface IHTMLElementBase : INode
    {
        event ControlsTreeChangeHandler ControlsTreeChange;
        /// <summary>
        /// Flag to determine wherever image on the current surface was changed or not
        /// </summary>
        bool IsChanged { get; set; }

        /// <summary>
        /// An object representing the declarations of an element's style attributes.
        /// </summary>
        object style { get; set; }

        /// <summary>
        /// Gets/sets the name attribute of an element.
        /// </summary>
        string name { get; set; }

        /// <summary>
        /// Workaround for IE env. - uses instead of native id property
        /// </summary>
        string Identifier { get; set; }

        /// <summary>
        /// Reference to the direct parent object
        /// </summary>
        object parent { get; }

        /// <summary>
        /// Reference to the native document object
        /// </summary>
        IHTMLDocument4 document { get; set; }

        void Redraw();

        /// <summary>
        /// Returns the element whose ID is specified. 
        /// </summary>
        /// <param name="id">id is a case-sensitive string representing the unique ID of the element being sought. </param>
        /// <returns></returns>
        object getElementById(string id);

        /// <summary>
        /// Returns a boolean indicating whether the object has the specified property.
        /// </summary>
        /// <param name="propName">properpty name</param>
        /// <returns></returns>
        bool hasOwnProperty(string propName);

        /// <summary>
        /// Returns the number of pixels that the upper left corner of the current element is offset to the left within the offsetParent node.
        /// </summary>
        int offsetLeft { get; set; }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        int offsetTop { get; set; }

        /// <summary>
        /// Assign parent for the element if element doesn't have any parent yet
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(object parent);
    }
}