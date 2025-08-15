using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    public class Event
    {

        #region Fields

        string _type;
        IEventTarget _target;
        IEventTarget _currentTarget;
        EventPhases _eventPhase;
        bool _bubbles;
        bool _cancelable;
        DateTime _timeStamp;
        private EventArgs _args;

        #endregion

        #region Properties

        public EventArgs args
        {
            get { return _args; }
        }

        /// <summary>
        /// The name of the event (case-insensitive). The name must be an XML name.
        /// </summary>
        public string type
        {
            get { return _type; }
        }

        /// <summary>
        /// Used to indicate the EventTarget to which the event was originally dispatched.
        /// </summary>
        public IEventTarget target
        {
            get { return _target; }
        }

        /// <summary>
        /// Used to indicate whether or not an event is a bubbling event. If the event can bubble the value is true, else the value is false
        /// </summary>
        public bool bubbles
        {
            get { return _bubbles; }
        }

        /// <summary>
        /// Used to indicate whether or not an event can have its default action prevented. If the default action can be prevented the value is true, else the value is false.
        /// </summary>
        public bool cancelable
        {
            get { return _cancelable; }
        }

        /// <summary>
        /// Used to indicate the EventTarget whose EventListeners are currently being processed. This is particularly useful during capturing and bubbling.
        /// </summary>
        public IEventTarget currentTarget
        {
            get { return _currentTarget; }
            set { _currentTarget = value; }
        }

        /// <summary>
        /// Used to indicate which phase of event flow is currently being evaluated.
        /// </summary>
        public EventPhases eventPhase
        {
            get { return _eventPhase; }
            set { _eventPhase = value; }
        }

        /// <summary>
        /// Used to specify the time (in milliseconds relative to the epoch) at which the event was created. Due to the fact that some systems may not provide this information the value of timeStamp may be not available for all events. When not available, a value of 0 will be returned. Examples of epoch time are the time of the system start or 0:0:0 UTC 1st January 1970.
        /// </summary>
        public DateTime timeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }



        #endregion

        #region Constructors

        public Event(string type, IEventTarget target, EventArgs args, bool bubbles, bool cancelable)
        {
            _type = type;
            _target = target;
            _bubbles = bubbles;
            _cancelable = cancelable;
            _args = args;
            _currentTarget = null!;
        }

        #endregion

        void stopPropagation()
        {
            
        }
        void preventDefault()
        {
            
        }

        void initEvent(string eventTypeArg, bool canBubbleArg, bool cancelableArg)
        {
            throw new NotImplementedException();
        }
    }
}
