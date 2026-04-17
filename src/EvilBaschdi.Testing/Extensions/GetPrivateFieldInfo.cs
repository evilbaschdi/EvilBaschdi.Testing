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
    /// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public static object GetPrivateMemberInfo(this object obj, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(propertyName);

        var objType = obj.GetType();
        var memberInfos = objType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var memberInfo = memberInfos.FirstOrDefault(x => x.Name == propertyName);

        if (memberInfo == null)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName), $"Couldn't find property {propertyName} in type {objType.FullName}");
        }

        return memberInfo.MemberType switch
        {
            MemberTypes.Property => ((PropertyInfo)memberInfo).GetValue(obj),
            MemberTypes.Field => ((FieldInfo)memberInfo).GetValue(obj),
            _ => throw new ArgumentException($"Member {propertyName} in type {objType.FullName} is not a property or field")
        };
    }

    /// <summary>
    ///     Returns private member property from given object or base type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="objType"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public static object GetPrivateMemberInfo(this object obj, string propertyName, Type objType)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(objType);

        var memberInfos = objType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var memberInfo = memberInfos.FirstOrDefault(x => x.Name == propertyName);

        if (memberInfo == null)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName), $"Couldn't find property {propertyName} in type {objType.FullName}");
        }

        return memberInfo.MemberType switch
        {
            MemberTypes.Property => ((PropertyInfo)memberInfo).GetValue(obj),
            MemberTypes.Field => ((FieldInfo)memberInfo).GetValue(obj),
            _ => throw new ArgumentException($"Member {propertyName} in type {objType.FullName} is not a property or field")
        };
    }
}