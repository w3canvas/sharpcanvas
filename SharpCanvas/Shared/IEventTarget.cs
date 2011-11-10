using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.JScript;

namespace SharpCanvas.Shared
{
    public interface IEventTarget
    {
        /// <summary>
        /// This method allows the registration of event listeners on the event target. If an EventListener is added to an EventTarget while it is processing an event, it will not be triggered by the current actions but may be triggered during a later stage of event flow, such as the bubbling phase.
        /// If multiple identical EventListeners are registered on the same EventTarget with the same parameters the duplicate instances are discarded. They do not cause the EventListener to be called twice and since they are discarded they do not need to be removed with the removeEventListener method.
        /// </summary>
        /// <param name="type">The event type for which the user is registering</param>
        /// <param name="listener">The listener parameter takes an interface implemented by the user which contains the methods to be called when the event occurs.</param>
        /// <param name="useCapture">If true, useCapture indicates that the user wishes to initiate capture. After initiating capture, all events of the specified type will be dispatched to the registered EventListener before being dispatched to any EventTargets beneath them in the tree. Events which are bubbling upward through the tree will not trigger an EventListener designated to use capture.</param>
        void addEventListener(string type, ScriptFunction listener, bool useCapture);
        
        /// <summary>
        /// This method allows the removal of event listeners from the event target. If an EventListener is removed from an EventTarget while it is processing an event, it will not be triggered by the current actions. EventListeners can never be invoked after being removed.
        /// Calling removeEventListener with arguments which do not identify any currently registered EventListener on the EventTarget has no effect.
        /// </summary>
        /// <param name="type">Specifies the event type of the EventListener being removed.</param>
        /// <param name="listener">The EventListener parameter indicates the EventListener to be removed.</param>
        /// <param name="useCapture">Specifies whether the EventListener being removed was registered as a capturing listener or not. If a listener was registered twice, one with capture and one without, each must be removed separately. Removal of a capturing listener does not affect a non-capturing version of the same listener, and vice versa.</param>
        void removeEventListener(string type, ScriptFunction listener, bool useCapture);
        
        /// <summary>
        /// This method allows the dispatch of events into the implementations event model. Events dispatched in this manner will have the same capturing and bubbling behavior as events dispatched directly by the implementation. The target of the event is the EventTarget on which dispatchEvent is called.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        //bool dispatchEvent(Event evt);

        #region Extension to DOM Level 2 IEventTarget interface

        Dictionary<string, List<IEventRegistration>> GetEventsCollection();

        #endregion
    }
}
