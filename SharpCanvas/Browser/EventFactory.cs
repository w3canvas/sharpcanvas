using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SharpCanvas.Core.Shared;

namespace SharpCanvas.Host.Browser
{
    public static class EventFactory
    {
        public static Event CreateEvent(string type, IEventTarget target, EventArgs args, bool bubbles, bool cancelable)
        {
            if(args is MouseEventArgs)
            {
                return new MouseEvent(type, target, args, bubbles, cancelable);
            }
            return new Event(type, target, args, bubbles, cancelable);
        }
    }
}
