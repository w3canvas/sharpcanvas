using System;

namespace SharpCanvas.Shared
{
    public interface IEventRegistration
    {
        object Target { get; }
        string Type { get; }
        Delegate Listener { get; }
        EventPhases ApplyToPhase { get; }
    }
}