using Microsoft.JScript;

namespace SharpCanvas.Shared
{
    public interface IEventRegistration
    {
        object Target { get; }
        string Type { get; }
        ScriptFunction Listener { get; }
        EventPhases ApplyToPhase { get; }
    }
}