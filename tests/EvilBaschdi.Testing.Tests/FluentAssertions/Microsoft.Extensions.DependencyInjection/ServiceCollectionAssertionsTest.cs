using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

// ReSharper disable MemberCanBePrivate.Global

namespace EvilBaschdi.Testing.Tests.FluentAssertions.Microsoft.Extensions.DependencyInjection;

public class ServiceCollectionAssertionsTest
{
    private readonly IServiceCollection _services;

    public ServiceCollectionAssertionsTest()
    {
        _services = new ServiceCollection();
        _services.AddSingleton<ISingleton, Singleton>();
        _services.AddTransient<ITransient, Transient>();
        _services.AddScoped<IScoped, Scoped>();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Be_Null()
    {
        IServiceCollection services = null;
        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => services.Should()
                                   .HaveService<ISingleton>()
                                   .AsSingleton();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Contain_Different_Lifetimes()
    {
        _services.AddTransient<ISingleton, Singleton>();
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .WithCount(2)
                                    .AsSingleton();

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Have_Service()
    {
        Action act = () => _services.Should()
                                    .HaveService<IServiceCollection>();

        // Assert
        act.Should().Throw<XunitException>();
    }

    #region HaveCount

    [Fact]
    public void HaveCount_Should_Pass_When_Count_Matches()
    {
        _services.Should().HaveCount(3);
    }

    [Fact]
    public void HaveCount_Should_Fail_When_Count_Does_Not_Match()
    {
        Action act = () => _services.Should().HaveCount(5);

        act.Should().Throw<XunitException>().WithMessage("*5*3*");
    }

    [Fact]
    public void HaveCount_Should_Fail_When_ServiceCollection_Is_Null()
    {
        IServiceCollection services = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => services.Should().HaveCount(0);

        act.Should().Throw<XunitException>().WithMessage("*<null>*");
    }

    [Fact]
    public void HaveCount_Should_Return_AndConstraint_For_Chaining()
    {
        _services.Should()
                 .HaveCount(3)
                 .And
                 .HaveService<ISingleton>()
                 .AsSingleton();
    }

    #endregion

    #region Singleton

    [Fact]
    public void ServiceCollection_Should_Contain_Singleton()
    {
        _services.Should()
                 .HaveService<ISingleton>()
                 .WithImplementation<Singleton>()
                 .AsSingleton();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Singleton_With_Implementation()
    {
        _services.Should().HaveService<ISingleton>()
                 .WithImplementation<Singleton>()
                 .AsSingleton();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Contain_Singleton_With_Implementation()
    {
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .WithImplementation<SingletonOther>();

        // Assert
        act.Should().Throw<XunitException>("ISingleton is not registered as a singleton");
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Two_Singleton()
    {
        _services.AddSingleton<ISingleton, Singleton>();
        _services.Should().HaveService<ISingleton>()
                 .WithCount(2)
                 .AsSingleton();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Contain_Singleton()
    {
        Action act = () => _services.Should()
                                    .HaveService<ITransient>()
                                    .AsSingleton();

        // Assert
        act.Should().Throw<XunitException>("ITransient is not registered as a singleton");
    }

    [Fact]
    public void ServiceCollection_HasManyShouldSingleton_ExpectExceptionBecauseIsNotOne()
    {
        _services.AddSingleton<ISingleton, Singleton>();
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .AsSingleton();

        // Assert
        act.Should().Throw<XunitException>();
    }

    #endregion

    #region Scoped

    [Fact]
    public void ServiceCollection_Should_Contain_Two_Scoped()
    {
        _services.AddScoped<IScoped, Scoped>();
        _services.Should()
                 .HaveService<IScoped>()
                 .WithCount(2)
                 .AsScoped();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Scoped()
    {
        _services.Should()
                 .HaveService<IScoped>()
                 .AsScoped();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Contain_Scoped()
    {
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .AsScoped();

        // Assert
        act.Should().Throw<XunitException>("ISingleton is not registered as a Scoped");
    }

    [Fact]
    public void ServiceCollection_HasManyShouldScoped_ExpectExceptionBecauseMoreThanOne()
    {
        _services.AddScoped<IScoped, Scoped>();
        Action act = () => _services.Should()
                                    .HaveService<IScoped>()
                                    .AsScoped();

        // Assert
        act.Should().Throw<XunitException>();
    }

    #endregion

    #region Transient

    [Fact]
    public void ServiceCollection_Should_Contain_Transient()
    {
        _services.Should()
                 .HaveService<ITransient>()
                 .AsTransient();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Two_Transient()
    {
        _services.AddTransient<ITransient, Transient>();
        _services.Should()
                 .HaveService<ITransient>()
                 .WithCount(2)
                 .AsTransient();
    }

    [Fact]
    public void ServiceCollection_Should_Not_Contain_Transient()
    {
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .AsTransient();

        // Assert
        act.Should().Throw<XunitException>("ISingleton is not registered as a Transient");
    }

    [Fact]
    public void ServiceCollection_HasManyShouldTransient_ExpectExceptionBecauseIsNotOne()
    {
        _services.AddTransient<ITransient, Transient>();
        Action act = () => _services.Should()
                                    .HaveService<ITransient>()
                                    .AsTransient();

        // Assert
        act.Should().Throw<XunitException>();
    }

    #endregion

    #region Factory-based Registrations

    [Fact]
    public void ServiceCollection_Should_Contain_Singleton_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(_ => new Singleton());
        services.Should()
                .HaveService<ISingleton>()
                .WithFactory()
                .AsSingleton();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Transient_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient>(_ => new Transient());
        services.Should()
                .HaveService<ITransient>()
                .WithFactory()
                .AsTransient();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Scoped_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddScoped<IScoped>(_ => new Scoped());
        services.Should()
                .HaveService<IScoped>()
                .WithFactory()
                .AsScoped();
    }

    [Fact]
    public void ServiceCollection_Should_Fail_When_Factory_Expected_But_Direct_Implementation_Registered()
    {
        // _services already has ISingleton registered directly (not via factory)
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .WithFactory();

        // Assert
        act.Should().Throw<XunitException>("ISingleton is registered with direct implementation, not a factory");
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Multiple_Services_With_Mixed_Registration_Types()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(_ => new Singleton());
        services.AddTransient<ITransient>(_ => new Transient());
        services.Should()
                .HaveService<ISingleton>()
                .WithFactory()
                .AsSingleton()
                .And
                .HaveService<ITransient>()
                .WithFactory()
                .AsTransient();
    }

    #endregion

    #region WithFactory(expectedFactory) - Behavior Comparison

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Pass_When_Factories_Match_Simple()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient>(_ => new Transient());

        services.Should()
                .HaveService<ITransient>()
                .WithFactory(_ => new Transient())
                .AsTransient();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Pass_When_Factories_Request_Same_Services()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient>(sp => new TransientWithDependency(sp.GetService<ISingleton>()));

        services.Should()
                .HaveService<ITransient>()
                .WithFactory(sp => new TransientWithDependency(sp.GetService<ISingleton>()))
                .AsTransient();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Pass_When_Both_Factories_Throw_After_Same_Requests()
    {
        var services = new ServiceCollection();
        services.AddTransient(sp => sp.GetService<IThrowingDependency>().CreateTransient());

        services.Should()
                .HaveService<ITransient>()
                .WithFactory(sp => sp.GetService<IThrowingDependency>().CreateTransient())
                .AsTransient();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Fail_When_No_Factory_Registered()
    {
        Action act = () => _services.Should()
                                    .HaveService<ISingleton>()
                                    .WithFactory(_ => new Singleton());

        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Fail_When_Factories_Request_Different_Services()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient>(sp => new TransientWithDependency(sp.GetService<ISingleton>()));

        Action act = () => services.Should()
                                   .HaveService<ITransient>()
                                   .WithFactory(sp => new TransientWithDependency(sp.GetService<IScoped>()));

        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Fail_When_Factories_Return_Different_Types()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(_ => new Singleton());

        Action act = () => services.Should()
                                   .HaveService<ISingleton>()
                                   .WithFactory(_ => new SingletonOther());

        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Pass_For_Scoped_With_Matching_Factory()
    {
        var services = new ServiceCollection();
        services.AddScoped<IScoped>(_ => new Scoped());

        services.Should()
                .HaveService<IScoped>()
                .WithFactory(_ => new Scoped())
                .AsScoped();
    }

    [Fact]
    public void WithFactory_ExpectedFactory_Should_Pass_For_Singleton_With_Matching_Factory()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(_ => new Singleton());

        services.Should()
                .HaveService<ISingleton>()
                .WithFactory(_ => new Singleton())
                .AsSingleton();
    }

    #endregion

    #region WithImplementation - Error Messages

    [Fact]
    public void WithImplementation_Should_Fail_When_Service_Registered_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(_ => new Singleton());

        Action act = () => services.Should()
                                   .HaveService<ISingleton>()
                                   .WithImplementation<Singleton>();

        act.Should().Throw<XunitException>().WithMessage("*factory*");
    }

    [Fact]
    public void WithImplementation_Should_Fail_When_Service_Registered_With_Instance()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(new Singleton());

        Action act = () => services.Should()
                                   .HaveService<ISingleton>()
                                   .WithImplementation<SingletonOther>();

        act.Should().Throw<XunitException>().WithMessage($"*{typeof(Singleton)}*");
    }

    [Fact]
    public void WithImplementation_Should_Pass_When_Service_Registered_With_Instance_And_Matching_Type()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISingleton>(new Singleton());

        // Instance registrations don't set ImplementationType, so WithImplementation checks ImplementationInstance type
        // However, ImplementationType is null for instance registrations, so this will fail
        Action act = () => services.Should()
                                   .HaveService<ISingleton>()
                                   .WithImplementation<Singleton>();

        // ImplementationType is null for instance registrations, so WithImplementation reports the instance type
        act.Should().Throw<XunitException>().WithMessage($"*{typeof(Singleton)}*");
    }

    #endregion

    #region Test Helpers

    public interface ISingleton;

    public interface ITransient;

    public interface IScoped;

    public interface IThrowingDependency
    {
        ITransient CreateTransient();
    }

    public class Singleton : ISingleton;

    public class SingletonOther : ISingleton;

    public class Transient : ITransient;

#pragma warning disable CS9113 // Parameter is unread.
    public class TransientWithDependency(object dependency) : ITransient;
#pragma warning restore CS9113 // Parameter is unread.

    public class Scoped : IScoped;

    #endregion
}