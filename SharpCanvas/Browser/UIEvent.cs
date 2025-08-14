using System;
using System.Collections.Generic;
using System.Text;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    public class UIEvent: Event
    {
        public UIEvent(string type, IEventTarget target, EventArgs args, bool bubbles, bool cancelable) : base(type, target, args, bubbles, cancelable)
        {
        }
    }
}
