using System.Reflection;

namespace SharpCanvas.Host.Prototype
{
    /// <summary>
    /// Identify family of objects which supports expando properties (important for WinForm env.)
    /// 
    /// In standalone environment access to the properties doesn't cause call of InvokeMember method.
    /// Therefore GetValue and SetValue methods should be belong not to the property itself, but for the object-container.
    /// We use thread-safe queue to process multiple calls of different properties.
    /// </summary>
    public interface IExpandoProperty
    {
        MemberInfo SetValue(object value);
        object GetValue();

        MethodInfo GetGetMethod(MemberInfo member);
        MethodInfo GetSetMethod(MemberInfo member);
    }
}