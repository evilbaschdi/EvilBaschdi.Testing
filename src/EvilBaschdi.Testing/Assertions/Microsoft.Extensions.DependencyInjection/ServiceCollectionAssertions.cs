using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace EvilBaschdi.Testing.Assertions.Microsoft.Extensions.DependencyInjection;

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
    private readonly AssertionChain _assertionChain;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceCollectionAssertions" /> class.
    /// </summary>
    /// <param name="subject">The service collection to assert.</param>
    /// <param name="assertionChain">The assertion chain used to execute the assertions.</param>
    internal ServiceCollectionAssertions(IServiceCollection subject, AssertionChain assertionChain)
        : base(subject, assertionChain)
    {
        _assertionChain = assertionChain;
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
    ///     Zero or more objects to format using the placeholders in "because".
    /// </param>
    public AndConstraint<ServiceCollectionAssertions> HaveCount(int expected, string because = "", params object[] becauseArgs)
    {
        if (Subject is null)
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:services} to contain {0} item(s){reason}, but found <null>.", expected);
        }
        else
        {
            var actualCount = Subject.Count;

            _assertionChain
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
    /// <param name="count">The expected number of services</param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in "because".
    /// </param>
    public ServiceAssertions<TService> HaveService<TService>(int count = 1, string because = "", params object[] becauseArgs)
    {
        NotBeNull();

        var services = Subject.Where(descriptor => descriptor.ServiceType == typeof(TService));

        //check that there is a service
        var serviceDescriptors = services.ToList();
        if (!serviceDescriptors.Any())
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith($"Expected {{context:services}} to have {count} service(s) of type {{0}} registered, but found none.",
                    typeof(TService));
        }

        return new ServiceAssertions<TService>(Subject, serviceDescriptors, count, _assertionChain);
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
