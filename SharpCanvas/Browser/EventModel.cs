using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    
    public class EventModel : IEventModel
    {
        public static object _sync = new object();
        public static Regex _eventRegex = new Regex(@"on(?<name>\w+)");
        public IWindow _window;
        private readonly List<string> _avoidBubblingEvents = new List<string> { "load" };//list of events which shouldn't be bubbled in IE
        

        /// <summary>
        /// This method should be shared between Window and any HTMLElement
        /// </summary>
        /// <param name="eventTarget"></param>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="useCapture"></param>
        public static void addEventListener(IEventTarget eventTarget, Dictionary<string, List<IEventRegistration>> events, string type, Delegate listener, bool useCapture)
        {
            //events always contain event's names without 'on' prefix
            type = NormalizeEventName(type);
            EventRegistration eventRegistration = new EventRegistration((UserControl)eventTarget, type, listener,
                                                                            useCapture ? EventPhases.CAPTURING_PHASE : EventPhases.BUBBLING_PHASE);
            if (!events.ContainsKey(type))
            {
                var eventRegistrations = new List<IEventRegistration>();
                eventRegistrations.Add(eventRegistration);
                events.Add(type, eventRegistrations);
            }
            else
            {
                bool alreadySubscribed = false;
                foreach (IEventRegistration existingRegistration in events[type])
                {
                    if(existingRegistration.Listener.ToString() == listener.ToString() && 
                        existingRegistration.Target == eventTarget)
                    {
                        alreadySubscribed = true;
                        break;
                    }
                }
                if (!alreadySubscribed)
                {
                    events[type].Add(eventRegistration);
                }
            }
        }

        private static string NormalizeEventName(string type)
        {
            if (_eventRegex.IsMatch(type))
            {
                type = _eventRegex.Match(type).Groups["name"].Value;
            }
            return type;
        }

        public static void removeEventListener(IEventTarget eventTarget, string type, Delegate listener, bool useCapture)
        {
            //events always contain event's names without 'on' prefix
            type = NormalizeEventName(type);
            Dictionary<string, List<IEventRegistration>> events = eventTarget.GetEventsCollection();
            EventPhases phase = useCapture ? EventPhases.CAPTURING_PHASE : EventPhases.BUBBLING_PHASE;
            if (events.ContainsKey(type))
            {
                IEventRegistration toRemove = null;
                foreach (IEventRegistration eventRegistration in events[type])
                {
                  if(eventRegistration.ApplyToPhase == phase)
                  {
                      toRemove = eventRegistration;
                      break;
                  }
                }
                if(toRemove != null)
                {
                    events[type].Remove(toRemove);
                }
            }
        }

        public EventModel(IWindow window)
        {
            _window = window;
        }

        #region Internal Event Handlers

        public void scroll(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "scroll");
        }

        public void mousewheel(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "mousewheel");
        }

        public void mouseout(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "mouseout");
        }

        public void mousemove(object? sender, EventArgs e)
        {
            if (sender == null) return;
            MouseEventArgs abse = new MouseEventArgs(((MouseEventArgs) e).Button, ((MouseEventArgs) e).Clicks,
                                                     ((MouseEventArgs) e).X + ((UserControl) sender).Left,
                                                     ((MouseEventArgs)e).Y + ((UserControl)sender).Top, ((MouseEventArgs)e).Delta);
            ProcessEvent(sender, abse, "mousemove");
        }

        public void mouseup(object? sender, EventArgs e)
        {
            if (sender == null) return;
            MouseEventArgs abse = new MouseEventArgs(((MouseEventArgs) e).Button, ((MouseEventArgs) e).Clicks,
                                                     ((MouseEventArgs) e).X + ((UserControl) sender).Left,
                                                     ((MouseEventArgs) e).Y + ((UserControl) sender).Top,
                                                     ((MouseEventArgs) e).Delta);
            ProcessEvent(sender, abse, "mouseup");
        }

        public void mousedown(object? sender, EventArgs e)
        {
            if (sender == null) return;
            MouseEventArgs abse = new MouseEventArgs(((MouseEventArgs) e).Button, ((MouseEventArgs) e).Clicks,
                                                     ((MouseEventArgs) e).X + ((UserControl) sender).Left,
                                                     ((MouseEventArgs) e).Y + ((UserControl) sender).Top,
                                                     ((MouseEventArgs) e).Delta);
            ProcessEvent(sender, abse, "mousedown");
        }

        public void mouseover(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "mouseover");
        }

        public void load(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "load");
        }

        public void keyup(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "keyup");
        }

        public void keypress(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "keypress");
        }

        public void keydown(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "keydown");
        }

        public void focus(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "focus");
        }

        public void dblclick(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "dblclick");
        }

        public void dragover(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "dragover");
        }

        public void dragleave(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "dragleave");
        }

        public void dragenter(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "dragenter");
        }

        public void drag(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "drag");
        }

        public void click(object? sender, EventArgs e)
        {
            ProcessEvent(sender, e, "click");
        }
        
        #endregion

        public void ProcessEvent(object? sender, EventArgs args, string eventName)
        {
            if (sender is IEventTarget)
            {
                IEventTarget eventTarget = (IEventTarget)sender;
                //if we're in IE, then propagate event into the native DOM
                if (sender is IHTMLElementBase)
                {
                    IHTMLDocument4? document = ((IHTMLElementBase)sender).document;
                    if (document != null && !_avoidBubblingEvents.Contains(eventName))
                    {
                        FireEvent(document, "on" + eventName, args);
                        return;
                    }
                }
                
                //Create Event
                Event e = EventFactory.CreateEvent(eventName, eventTarget, args, true, false);
                //Trigger mouse events for controls at the same coordinates
                List<INode> targetsXY = new List<INode>();
                if (e is MouseEvent)
                {
                    MouseEvent me = (MouseEvent)e;
                    IWindow window = sender is IWindow ? (IWindow)sender : ((IDocument)((IHTMLElementBase)sender).ownerDocument).defaultView;
                    BuildTargetsListByXY((INode)window, me.pageX, me.pageY, targetsXY);
                }

                //Broadcast event starting from target's node
                BroadcastEventFromNode((INode)sender, e, null);
                //Broadcast event from nodes which contains the same XY coordinate and in case of MouseEvent
                if(e is MouseEvent && targetsXY.Count > 0)
                {
                    //Build event targets chain
                    List<IEventTarget> avoidTargets = new List<IEventTarget>();
                    BuildEventTargetsChain((INode)sender, avoidTargets);
                    foreach (INode target in targetsXY)
                    {
                        BroadcastEventFromNode(target, e, avoidTargets);
                    }
                }
            }
        }

        private void BroadcastEventFromNode(INode sender, Event e, List<IEventTarget> avoidTargets)
        {
            //Build event targets chain
            List<IEventTarget> targets = new List<IEventTarget>();
            BuildEventTargetsChain(sender, targets);
            //Capture Event
            CaptureEvent(targets, e, avoidTargets);
            //Bubble Event
            BubbleEvent(targets, e, avoidTargets);
        }

        /// <summary>
        /// Return list of elements which are located at XY point and doesn't have any child elements at the same point
        /// </summary>
        /// <param name="node"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        private bool BuildTargetsListByXY(INode node, int x, int y, List<INode> targets)
        {
            List<INode> childAtXY = new List<INode>();
            foreach (INode childNode in node.childNodes)
            {
                if(childNode is UserControl)
                {
                    bool contains = ((UserControl) childNode).ClientRectangle.Contains(x, y);
                    if (contains)
                    {
                        childAtXY.Add(childNode);
                    }
                }
            }
            foreach (INode childXY in childAtXY)
            {
                if(childXY.childNodes.Count == 0 || !BuildTargetsListByXY(childXY, x, y, targets))//if child is leaf or it doesn't have any child which contain XY point
                {
                    targets.Add(childXY);
                }
            }
            return childAtXY.Count > 0;
        }

        private void BuildEventTargetsChain(INode node, List<IEventTarget> chain)
        {
            if (node is IEventTarget)
            {
                chain.Add((IEventTarget) node);
                if (node.parentNode != null)
                {
                    BuildEventTargetsChain(node.parentNode, chain);
                }
            }
        }

        private void CaptureEvent(List<IEventTarget> nodes, Event e, List<IEventTarget>? avoidNodes)
        {
            e.eventPhase = EventPhases.CAPTURING_PHASE;
            //reverse of the chain make it sorted in order to execute
            nodes.Reverse();
            BroadcastEvent(e, nodes, avoidNodes);
        }

        /// <summary>
        /// Bubbles event to the document object directly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public void BubbleEvent(List<IEventTarget> nodes, Event e, List<IEventTarget>? avoidNodes)
        {
            e.eventPhase = EventPhases.BUBBLING_PHASE;
            //reverse of the chain make it sorted in order to execute
            nodes.Reverse();
            BroadcastEvent(e, nodes, avoidNodes);
        }

        private void BroadcastEvent(Event e, List<IEventTarget> nodes, List<IEventTarget>? avoidNodes)
        {
            foreach (IEventTarget node in nodes)
            {
                if(avoidNodes != null && avoidNodes.Contains(node))
                {
                    continue;//skip nodes which are marked as avoided. Generally nodes which already handled the event are marked as avoided.
                }
                Dictionary<string, List<IEventRegistration>> events = node.GetEventsCollection();
                //process event in current node
                if (events.ContainsKey(e.type) && events[e.type].Count > 0)
                    //if there is at least one listener of this type of event
                {
                    List<IEventRegistration> eventRegistrations = events[e.type];
                    foreach (IEventRegistration eventRegistration in eventRegistrations)
                    {
                        if (eventRegistration.Listener != null && eventRegistration.ApplyToPhase == e.eventPhase) //if listener has correct type of eventPahse
                        {
                            FireEvent(eventRegistration.Listener, e, node);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Trigger some javascript event on the object
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public bool FireEvent(Delegate sf, Event e, object scope)
        {
            object result = sf.DynamicInvoke(scope, new object[] {e});
            //todo: check whether result is NULL?
            return result is bool ? (bool)result : false;
        }


        /// <summary>
        /// Fires event on the document object
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="eventName"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool FireEvent(IHTMLDocument4 doc, string eventName, EventArgs e)
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            if (e is MouseEventArgs)
            {
                x = (e as MouseEventArgs).X;
                y = (e as MouseEventArgs).Y;
            }
            //object dummy = null;
            //object oEvt = doc.CreateEventObject(ref dummy);
            object oEvt = doc.CreateEventObject();
            var evt = (IHTMLEventObj2)oEvt; //cast
            //set various properties of the event here.
            evt.clientX = x;
            evt.clientY = y;

            //fire
            return doc.FireEvent(eventName, oEvt);
        }

        public static Dictionary<string, List<IEventRegistration>> CloneDictionaryCloningValues(Dictionary<string, List<IEventRegistration>> original)
        {
            Dictionary<string, List<IEventRegistration>> ret =
                new Dictionary<string, List<IEventRegistration>>(original.Count, original.Comparer);
            foreach (KeyValuePair<string, List<IEventRegistration>> entry in original)
            {
                List<IEventRegistration> clone = new List<IEventRegistration>();
                foreach (IEventRegistration registration in entry.Value)
                {
                    clone.Add(registration);
                }
                ret.Add(entry.Key, clone);
            }
            return ret;
        }
    }
}
