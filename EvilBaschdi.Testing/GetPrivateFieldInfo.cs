using System.Reflection;

namespace EvilBaschdi.Testing;

/// <summary>
///     Class for reflection helpers
/// </summary>
// ReSharper disable once UnusedType.Global
public static class GetPrivateFieldInfo
{
    /// <summary>
    ///     Returns private member property from given object or base type
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public static object ValueFor(this object obj, string propertyName, Type baseType = null)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var objType = baseType ?? obj.GetType();
        var memberInfo = objType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                .FirstOrDefault(x => x.Name == propertyName);
        if (memberInfo == null)
        {
            throw new ArgumentOutOfRangeException(nameof(propertyName),
                $"Couldn't find property {propertyName} in type {objType.FullName}");
        }

        return ((FieldInfo)memberInfo).GetValue(obj);
    }
}