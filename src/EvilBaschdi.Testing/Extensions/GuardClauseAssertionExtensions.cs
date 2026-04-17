using System.Reflection;
using AutoFixture.Kernel;

namespace EvilBaschdi.Testing.Extensions;

/// <summary>
///     Provides extension methods for verifying that all public asynchronous methods on a specified type have appropriate
///     null guards for their reference type parameters.
/// </summary>
/// <remarks>
///     Use these extensions in conjunction with GuardClauseAssertion to ensure that async methods on the
///     target type throw ArgumentNullException when passed null for reference type parameters. This helps enforce
///     defensive
///     programming practices and improves API reliability. The extensions are intended for use in automated unit
///     tests.
/// </remarks>
public static class GuardClauseAssertionExtensions
{
    /// <summary>
    ///     Verifies that all public async methods on type <typeparamref name="T" /> have null guards for their reference type
    ///     parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assertion"></param>
    /// <param name="methodInfos">The methods.</param>
    /// <exception cref="GuardClauseException"></exception>
    public static void VerifyTask<T>(this GuardClauseAssertion assertion, IEnumerable<MethodInfo> methodInfos)
    {
        ArgumentNullException.ThrowIfNull(assertion);
        ArgumentNullException.ThrowIfNull(methodInfos);

        var fixture = assertion.Builder;
        var context = new SpecimenContext(fixture);
        var instance = context.Resolve(typeof(T));

        var innerMethodInfos = methodInfos.Where(mi => typeof(Task).IsAssignableFrom(mi.ReturnType));

        foreach (var method in innerMethodInfos)
        {
            var parameters = method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;

                // Skip value types (including enums) as they cannot be null
                if (parameterType.IsValueType && Nullable.GetUnderlyingType(parameterType) == null)
                {
                    continue;
                }

                var args = parameters.Select(p => context.Resolve(p.ParameterType)).ToArray();
                args[i] = null!;

                try
                {
                    var task = (Task)method.Invoke(instance, args)!;
                    task.GetAwaiter().GetResult();
                }
                catch (TargetInvocationException ex) when (ex.InnerException is ArgumentNullException)
                {
                    // Expected behavior - null guard triggered
                    continue;
                }
                catch (ArgumentNullException)
                {
                    // Expected behavior - null guard triggered
                    continue;
                }
                catch (TargetInvocationException ex) when (ex.InnerException is NullReferenceException)
                {
                    throw new GuardClauseException(
                        $"Method '{method.Name}' parameter '{parameters[i].Name}' does not have a null guard. " +
                        $"A NullReferenceException was thrown instead of ArgumentNullException.", ex.InnerException);
                }
                catch (NullReferenceException ex)
                {
                    throw new GuardClauseException(
                        $"Method '{method.Name}' parameter '{parameters[i].Name}' does not have a null guard. " +
                        $"A NullReferenceException was thrown instead of ArgumentNullException.", ex);
                }

                throw new GuardClauseException($"Method '{method.Name}' parameter '{parameters[i].Name}' does not have a null guard.");
            }
        }
    }
}