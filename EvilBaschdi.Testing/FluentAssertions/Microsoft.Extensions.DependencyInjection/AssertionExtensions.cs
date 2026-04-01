using Microsoft.Extensions.DependencyInjection;

namespace EvilBaschdi.Testing.FluentAssertions.Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Provides extension methods for asserting the state of an IServiceCollection in unit tests.
/// </summary>
public static class AssertionExtensions
{
    /// <summary>
    ///     Returns a <see cref="ServiceCollectionAssertions" /> object that can be used to assert the state of the
    ///     <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to assert.</param>
    /// <returns>A <see cref="ServiceCollectionAssertions" /> object.</returns>
    public static ServiceCollectionAssertions Should(this IServiceCollection services) => new(services);
}