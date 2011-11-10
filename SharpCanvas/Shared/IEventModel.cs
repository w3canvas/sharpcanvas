using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Shared
{
    public interface IEventModel
    {
        void scroll(object sender, EventArgs e);
        void mousewheel(object sender, EventArgs e);
        void mouseout(object sender, EventArgs e);
        void mousemove(object sender, EventArgs e);
        void mouseup(object sender, EventArgs e);
        void mousedown(object sender, EventArgs e);
        void mouseover(object sender, EventArgs e);
        void load(object sender, EventArgs e);
        void keyup(object sender, EventArgs e);
        void keypress(object sender, EventArgs e);
        void keydown(object sender, EventArgs e);
        void focus(object sender, EventArgs e);
        void dblclick(object sender, EventArgs e);
        void dragover(object sender, EventArgs e);
        void dragleave(object sender, EventArgs e);
        void dragenter(object sender, EventArgs e);
        void drag(object sender, EventArgs e);
        void click(object sender, EventArgs e);
    }

}
