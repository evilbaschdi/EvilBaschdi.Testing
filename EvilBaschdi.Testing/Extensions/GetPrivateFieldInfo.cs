using System.Reflection;

namespace EvilBaschdi.Testing.Extensions;

/// <summary>
///     Class for reflection helpers
/// </summary>
// ReSharper disable once UnusedType.Global
public static class ReflectionHelper
{
    /// <summary>
    ///     Returns private member property from given object or base type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public static object GetPrivateFieldInfo(this object obj, string propertyName, Type baseType = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(propertyName);

        var objType = baseType ?? obj.GetType();
        var memberInfo = objType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                .FirstOrDefault(x => x.Name == propertyName);
        return memberInfo == null
            ? throw new ArgumentOutOfRangeException(nameof(propertyName),
                $"Couldn't find property {propertyName} in type {objType.FullName}")
            : ((FieldInfo)memberInfo).GetValue(obj);
    }
}