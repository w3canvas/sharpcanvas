using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.JScript;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    public class EventRegistration : IEventRegistration
    {
        private UserControl _target;
        private string _type;
        private ScriptFunction _listener;
        private EventPhases _applyToPhase;

        public EventRegistration(UserControl target, string type, ScriptFunction listener, EventPhases applyToPhase)
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

        public ScriptFunction Listener
        {
            get { return _listener; }
        }

        public EventPhases ApplyToPhase
        {
            get { return _applyToPhase; }
        }
    }
}
