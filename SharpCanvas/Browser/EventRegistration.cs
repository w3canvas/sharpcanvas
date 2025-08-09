using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    public class EventRegistration : IEventRegistration
    {
        private UserControl _target;
        private string _type;
        private Delegate _listener;
        private EventPhases _applyToPhase;

        public EventRegistration(UserControl target, string type, Delegate listener, EventPhases applyToPhase)
        {
            _target = target;
            _type = type;
            _listener = listener;
            _applyToPhase = applyToPhase;
        }

        public object Target
        {
            get { return _target; }
        }

        public string Type
        {
            get { return _type; }
        }

        public Delegate Listener
        {
            get { return _listener; }
        }

        public EventPhases ApplyToPhase
        {
            get { return _applyToPhase; }
        }
    }
}
