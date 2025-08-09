using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SharpCanvas.Core.Shared;

namespace SharpCanvas.Host.Browser
{
    public class MouseEvent: UIEvent
    {
        public int pageX { get; set; }
        public int pageY { get; set; }
        public int keyCode { get; set; }

        public MouseEvent(string type, IEventTarget target, EventArgs e, bool bubbles, bool cancelable) : 
            base(type, target, e, bubbles, cancelable)
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            if (e is MouseEventArgs)
            {
                x = (e as MouseEventArgs).X;
                y = (e as MouseEventArgs).Y;
            }
            pageX = x;
            pageY = y;
            if (e is KeyEventArgs)
            {
                keyCode = (int)((KeyEventArgs)e).KeyCode;
            }
        }
    }
}
