using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace EvilBaschdi.Testing.FluentAssertions.Microsoft.Extensions.DependencyInjection;

/// <inheritdoc />
/// <summary>
///     Contains a number of methods to assert that an
///     <see cref="T:IServiceCollection" /> has registered expected services.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class ServiceCollectionAssertions : ReferenceTypeAssertions<IServiceCollection, ServiceCollectionAssertions>
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="subject"></param>
    internal ServiceCollectionAssertions(IServiceCollection subject)
        : base(subject)
    {
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override string Identifier => "services";

    /// <summary>
    ///     Asserts that the number of items in the collection matches the supplied <paramref name="expected" /> amount.
    /// </summary>
    /// <param name="expected">The expected number of items in the collection.</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public AndConstraint<ServiceCollectionAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
    {
        //had to take the HaveCount function from fluent asserts library since IServiceCollection is not explicitlly 
        //Ienumerable<TService> see: https://github.com/fluentassertions/fluentassertions/blob/develop/Src/FluentAssertions/Collections/NonGenericCollectionAssertions.cs
        if (Subject is null)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to contain {0} item(s){reason}, but found <null>.", expected);
        }
        else
        {
            var actualCount = Subject.Count;

            Execute.Assertion
                   .ForCondition(actualCount == expected)
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to contain {0} item(s){reason}, but found {1}.", expected, actualCount);
        }

        return new AndConstraint<ServiceCollectionAssertions>(this);
    }

    /// <summary>
    ///     Asserts that the service collection has the service
    /// </summary>
    /// <typeparam name="TService">The service to check</typeparam>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public ServiceAssertions<TService> HaveService<TService>(string because = "", params object[] becauseArgs)
    {
        NotBeNull();

        var services = Subject.Where(descriptor => descriptor.ServiceType == typeof(TService));

        //check that there is a service
        var serviceDescriptors = services.ToList();
        if (!serviceDescriptors.Any())
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to have a service of type {0} registered, but found none.",
                       typeof(TService));
        }

        return new ServiceAssertions<TService>(Subject, serviceDescriptors, 1);
    }

    #region Helpers

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void NotBeNull()
    {
        if (Subject is null)
        {
            throw new ArgumentNullException(nameof(Subject), "cannot not assert on null service collection");
        }
    }

    #endregion
}