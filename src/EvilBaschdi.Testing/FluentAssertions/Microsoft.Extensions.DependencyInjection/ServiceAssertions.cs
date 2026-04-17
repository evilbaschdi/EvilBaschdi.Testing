using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace EvilBaschdi.Testing.FluentAssertions.Microsoft.Extensions.DependencyInjection;
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
/// <summary>
///     Collection of assertions to check the registered services
/// </summary>
/// <typeparam name="TService">The registered service to check</typeparam>
public class ServiceAssertions<TService>
{
    private readonly IServiceCollection _services;
    private readonly IReadOnlyList<ServiceDescriptor> _filteredServices;
    private int _count;

    internal ServiceAssertions(IServiceCollection services, IEnumerable<ServiceDescriptor> filteredServices, int count)
    {
        _services = services;
        _filteredServices = (filteredServices ?? []).ToList();
        _count = count;
    }

    /// <summary>
    ///     Asserts that the service collection has the expected number of services
    /// </summary>
    /// <param name="expected">
    ///     the expected number of services
    /// </param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public ServiceAssertions<TService> WithCount(int expected, string because = "", params object[] becauseArgs)
    {
        _count = expected;
        CheckCount(because, becauseArgs);
        return this;
    }

    /// <summary>
    ///     Asserts that the service collection has a service registered with an implementation. To check if multiple
    ///     implementations are registered, simply chain method
    /// </summary>
    /// <typeparam name="TImplementation">The Implementation to check</typeparam>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public ServiceAssertions<TService> WithImplementation<TImplementation>(string because = "", params object[] becauseArgs)
        where TImplementation : TService
    {
        // ReSharper disable once SimplifyLinqExpressionUseAll
        if (_filteredServices.Any(service => service.ImplementationType == typeof(TImplementation)))
        {
            return this;
        }

        var registered = _filteredServices.Count > 0 ? _filteredServices[0] : null;
        object found;

        if (registered == null)
        {
            found = "<none>";
        }
        else if (registered.ImplementationType != null)
        {
            found = registered.ImplementationType;
        }
        else if (registered.ImplementationInstance != null)
        {
            found = registered.ImplementationInstance.GetType();
        }
        else if (registered.ImplementationFactory != null)
        {
            found = "factory";
        }
        else
        {
            found = "<unknown>";
        }

        Execute.Assertion
               .BecauseOf(because, becauseArgs)
               .FailWith("Expected {context:services} to have an implementation of type {0} registered, but found {1}.",
                   typeof(TImplementation),
                   found);

        return this;
    }

    /// <summary>
    ///     Asserts that the service collection has a service registered with a factory function.
    ///     This is used for services registered via AddSingleton(provider => ...), AddScoped(provider => ...), etc.
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public ServiceAssertions<TService> WithFactory(string because = "", params object[] becauseArgs)
    {
        // ReSharper disable once SimplifyLinqExpressionUseAll
        if (!_filteredServices.Any(service => service.ImplementationFactory != null))
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to have a factory-based implementation registered for {0}, but found none.",
                       typeof(TService));
        }

        return this;
    }

    /// <summary>
    ///     Asserts that the service collection has a service registered with a factory function that behaves equivalently
    ///     to the expected factory. Comparison is done by verifying that both factories request the same services from the
    ///     <see cref="IServiceProvider" /> and produce the same result type, without requiring real dependency resolution.
    /// </summary>
    /// <param name="expectedFactory">
    ///     A factory function whose behavior (requested services and result type) is compared against the registered factory.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public ServiceAssertions<TService> WithFactory(Func<IServiceProvider, TService> expectedFactory, string because = "", params object[] becauseArgs)
    {
        // ReSharper disable once SimplifyLinqExpressionUseAll
        if (!_filteredServices.Any(service => service.ImplementationFactory != null))
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to have a factory-based implementation registered for {0}, but found none.",
                       typeof(TService));
            return this;
        }

        var registeredService = _filteredServices.First(s => s.ImplementationFactory != null);

        var registeredRecorder = new ServiceRequestRecorder();
        var expectedRecorder = new ServiceRequestRecorder();

        var (registeredResultType, registeredError) = InvokeFactory(sp => registeredService.ImplementationFactory!(sp), registeredRecorder);
        var (expectedResultType, expectedError) = InvokeFactory(sp => expectedFactory(sp), expectedRecorder);

        if (!registeredRecorder.RequestedServiceTypes.SequenceEqual(expectedRecorder.RequestedServiceTypes))
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} factory for {0} to request services [{1}], but it requested [{2}].",
                       typeof(TService),
                       string.Join(", ", expectedRecorder.RequestedServiceTypes.Select(t => t.FullName)),
                       string.Join(", ", registeredRecorder.RequestedServiceTypes.Select(t => t.FullName)));
        }

        if (registeredError == null && expectedError == null && registeredResultType != expectedResultType)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} factory for {0} to return type {1}, but it returned {2}.",
                       typeof(TService),
                       expectedResultType,
                       registeredResultType);
        }

        return this;
    }

    private static (Type ResultType, Exception Error) InvokeFactory(Func<IServiceProvider, object> factory, IServiceProvider provider)
    {
        try
        {
            var result = factory(provider);
            return (result?.GetType(), null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private sealed class ServiceRequestRecorder : IServiceProvider
    {
        private readonly List<Type> _requestedServiceTypes = [];

        public IReadOnlyList<Type> RequestedServiceTypes => _requestedServiceTypes;

        public object GetService(Type serviceType)
        {
            _requestedServiceTypes.Add(serviceType);
            return null;
        }
    }

    /// <summary>
    ///     Asserts that the service collection <see cref="TService" /> are of lifespan Singleton
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public AndConstraint<ServiceCollectionAssertions> AsSingleton(string because = "", params object[] becauseArgs)
    {
        //check count for one service if count has not been specified
        CheckCount("Should only have one service");
        CheckLifetime(ServiceLifetime.Singleton, because, becauseArgs);
        return new AndConstraint<ServiceCollectionAssertions>(new ServiceCollectionAssertions(_services));
    }

    /// <summary>
    ///     Asserts that the service collection <see cref="TService" /> are of lifespan Scoped
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public AndConstraint<ServiceCollectionAssertions> AsScoped(string because = "", params object[] becauseArgs)
    {
        //check count for one service if count has not been specified
        CheckCount("Should only have one service");
        CheckLifetime(ServiceLifetime.Scoped, because, becauseArgs);
        return new AndConstraint<ServiceCollectionAssertions>(new ServiceCollectionAssertions(_services));
    }

    /// <summary>
    ///     Asserts that the service collection <see cref="TService" /> are of lifespan Transient
    /// </summary>
    /// <param name="because">
    ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <see cref="because" />.
    /// </param>
    public AndConstraint<ServiceCollectionAssertions> AsTransient(string because = "", params object[] becauseArgs)
    {
        //check count for one service if count has not been specified
        CheckCount("Should only have one service");
        CheckLifetime(ServiceLifetime.Transient, because, becauseArgs);
        return new AndConstraint<ServiceCollectionAssertions>(new ServiceCollectionAssertions(_services));
    }

    private void CheckLifetime(ServiceLifetime lifetime, string because, params object[] becauseArgs)
    {
        var mismatch = _filteredServices.FirstOrDefault(x => x.Lifetime != lifetime);
        if (mismatch != null)
        {
            var service = mismatch;
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to have a {0} of type {1} registered, but found {2}.",
                       lifetime,
                       service.ServiceType,
                       service.Lifetime);
        }
    }

    private void CheckCount(string because, params object[] becauseArgs)
    {
        //check service count
        if (_filteredServices.Count != _count)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:services} to have {0} service(s) of type {1} registered, but found {2}.",
                       _count,
                       typeof(TService),
                       _filteredServices.Count);
        }
    }
}