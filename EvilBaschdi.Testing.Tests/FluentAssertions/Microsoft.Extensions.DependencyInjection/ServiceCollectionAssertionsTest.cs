using EvilBaschdi.Testing.FluentAssertions.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

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
        services.AddSingleton<ISingleton>(provider => new Singleton());
        services.Should()
                .HaveService<ISingleton>()
                .WithFactory()
                .AsSingleton();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Transient_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransient>(provider => new Transient());
        services.Should()
                .HaveService<ITransient>()
                .WithFactory()
                .AsTransient();
    }

    [Fact]
    public void ServiceCollection_Should_Contain_Scoped_With_Factory()
    {
        var services = new ServiceCollection();
        services.AddScoped<IScoped>(provider => new Scoped());
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
        services.AddSingleton<ISingleton>(provider => new Singleton());
        services.AddTransient<ITransient>(provider => new Transient());
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

    #region Test Helpers

    public interface ISingleton
    {
    }

    public interface ITransient
    {
    }

    public interface IScoped
    {
    }

    public class Singleton : ISingleton
    {
    }

    public class SingletonOther : ISingleton
    {
    }

    public class Transient : ITransient
    {
    }

    public class Scoped : IScoped
    {
    }

    #endregion
}