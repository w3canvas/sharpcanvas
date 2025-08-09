using System;
using System.Collections.Generic;
using System.Text;
using SharpCanvas.Host.Prototype;

namespace SharpCanvas.Host.Browser
{
    public class WindowHold: ObjectWithPrototype
    {
        static readonly WindowHold _instance = new WindowHold(new Guid());

        public WindowHold(Guid scope) : base(scope)
        {
        }
    }
}
