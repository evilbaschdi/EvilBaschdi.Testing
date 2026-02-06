using System.Reflection;
using EvilBaschdi.Testing.Extensions;

// ReSharper disable All

namespace EvilBaschdi.Testing.Tests.Extensions;

public class GuardClauseAssertionExtensionsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuardClauseAssertionExtensions).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuardClauseAssertionExtensions).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenAllAsyncMethodsHaveGuards_DoesNotThrow(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithGuards).GetMethods().Where(m => m.DeclaringType == typeof(AsyncClassWithGuards));

        var act = () => assertion.VerifyTask<AsyncClassWithGuards>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenAsyncMethodIsMissingGuard_ThrowsGuardClauseException(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassMissingGuard).GetMethods().Where(m => m.DeclaringType == typeof(AsyncClassMissingGuard));

        var act = () => assertion.VerifyTask<AsyncClassMissingGuard>(methods);

        act.Should().Throw<GuardClauseException>().WithMessage("*does not have a null guard*");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenAsyncMethodThrowsNullReference_ThrowsGuardClauseException(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassThrowsNullReference).GetMethods().Where(m => m.DeclaringType == typeof(AsyncClassThrowsNullReference));

        var act = () => assertion.VerifyTask<AsyncClassThrowsNullReference>(methods);

        act.Should().Throw<GuardClauseException>().WithMessage("*A NullReferenceException was thrown instead of ArgumentNullException*");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenAsyncMethodHasValueTypes_IgnoresValueTypes(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithValueTypes).GetMethods().Where(m => m.DeclaringType == typeof(AsyncClassWithValueTypes));

        var act = () => assertion.VerifyTask<AsyncClassWithValueTypes>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_IgnoresNonAsyncMethods(GuardClauseAssertion assertion)
    {
        var methods = typeof(ClassWithNonAsyncMethods).GetMethods().Where(m => m.DeclaringType == typeof(ClassWithNonAsyncMethods));

        var act = () => assertion.VerifyTask<ClassWithNonAsyncMethods>(methods);

        act.Should().NotThrow();
    }

    [Fact]
    public void VerifyTask_WhenAssertionIsNull_ThrowsArgumentNullException()
    {
        GuardClauseAssertion assertion = null!;
        var methods = typeof(AsyncClassWithGuards).GetMethods();

        var act = () => assertion.VerifyTask<AsyncClassWithGuards>(methods);

        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("assertion");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodInfosIsNull_ThrowsArgumentNullException(GuardClauseAssertion assertion)
    {
        IEnumerable<MethodInfo> methods = null!;

        var act = () => assertion.VerifyTask<AsyncClassWithGuards>(methods);

        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("methodInfos");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenEmptyMethodInfos_DoesNotThrow(GuardClauseAssertion assertion)
    {
        var methods = Enumerable.Empty<MethodInfo>();

        var act = () => assertion.VerifyTask<AsyncClassWithGuards>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodHasMultipleParameters_VerifiesAllParameters(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithMultipleParametersMissingGuard).GetMethods()
                                                                          .Where(m => m.DeclaringType == typeof(AsyncClassWithMultipleParametersMissingGuard));

        var act = () => assertion.VerifyTask<AsyncClassWithMultipleParametersMissingGuard>(methods);

        act.Should().Throw<GuardClauseException>().WithMessage("*second*does not have a null guard*");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodHasMultipleParametersWithGuards_DoesNotThrow(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithMultipleParametersWithGuards).GetMethods()
                                                                        .Where(m => m.DeclaringType == typeof(AsyncClassWithMultipleParametersWithGuards));

        var act = () => assertion.VerifyTask<AsyncClassWithMultipleParametersWithGuards>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodHasEnumParameter_IgnoresEnumType(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithEnumParameter).GetMethods()
                                                         .Where(m => m.DeclaringType == typeof(AsyncClassWithEnumParameter));

        var act = () => assertion.VerifyTask<AsyncClassWithEnumParameter>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodHasNullableValueTypeMissingGuard_ThrowsGuardClauseException(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithNullableValueTypeMissingGuard).GetMethods()
                                                                         .Where(m => m.DeclaringType == typeof(AsyncClassWithNullableValueTypeMissingGuard));

        var act = () => assertion.VerifyTask<AsyncClassWithNullableValueTypeMissingGuard>(methods);

        act.Should().Throw<GuardClauseException>().WithMessage("*does not have a null guard*");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodHasNoParameters_DoesNotThrow(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassWithNoParameters).GetMethods()
                                                        .Where(m => m.DeclaringType == typeof(AsyncClassWithNoParameters));

        var act = () => assertion.VerifyTask<AsyncClassWithNoParameters>(methods);

        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenMethodReturnsTaskOfT_VerifiesNullGuards(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassReturnsTaskOfTMissingGuard).GetMethods()
                                                                  .Where(m => m.DeclaringType == typeof(AsyncClassReturnsTaskOfTMissingGuard));

        var act = () => assertion.VerifyTask<AsyncClassReturnsTaskOfTMissingGuard>(methods);

        act.Should().Throw<GuardClauseException>().WithMessage("*does not have a null guard*");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VerifyTask_WhenAsyncMethodThrowsDirectNullReference_ThrowsGuardClauseException(GuardClauseAssertion assertion)
    {
        var methods = typeof(AsyncClassThrowsDirectNullReference).GetMethods()
                                                                 .Where(m => m.DeclaringType == typeof(AsyncClassThrowsDirectNullReference));

        var act = () => assertion.VerifyTask<AsyncClassThrowsDirectNullReference>(methods);

        act.Should().Throw<GuardClauseException>()
           .WithMessage("*A NullReferenceException was thrown instead of ArgumentNullException*");
    }

    public class AsyncClassWithGuards
    {
        public Task MethodAsync(string arg)
        {
            ArgumentNullException.ThrowIfNull(arg);

            return Task.CompletedTask;
        }

        public Task<string> MethodWithResultAsync(string arg)
        {
            ArgumentNullException.ThrowIfNull(arg);

            return Task.FromResult(arg);
        }
    }

    public class AsyncClassMissingGuard
    {
        public Task MethodAsync(string arg) => Task.CompletedTask;
    }

    public class AsyncClassThrowsNullReference
    {
        public Task MethodAsync(string arg) => Task.FromResult(arg.Length.ToString());
    }

    public class AsyncClassWithValueTypes
    {
        public Task MethodAsync(int arg) => Task.CompletedTask;

        public Task MethodWithNullableAsync(int? arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            return Task.CompletedTask;
        }
    }

    public class ClassWithNonAsyncMethods
    {
        public void SyncMethod(string arg)
        {
        }
    }

    public class AsyncClassWithMultipleParametersMissingGuard
    {
        public Task MethodAsync(string first, string second)
        {
            ArgumentNullException.ThrowIfNull(first);

            // Missing guard for second parameter
            return Task.CompletedTask;
        }
    }

    public class AsyncClassWithMultipleParametersWithGuards
    {
        public async Task MethodAsync(string first, string second)
        {
            ArgumentNullException.ThrowIfNull(first);

            ArgumentNullException.ThrowIfNull(second);

            await Task.CompletedTask;
        }
    }

    public enum TestEnum
    {
        Value1,
        Value2
    }

    public class AsyncClassWithEnumParameter
    {
        public Task MethodAsync(TestEnum enumValue) => Task.CompletedTask;
    }

    public class AsyncClassWithNullableValueTypeMissingGuard
    {
        public Task MethodAsync(int? nullableValue) => Task.CompletedTask;
    }

    public class AsyncClassWithNoParameters
    {
        public Task MethodAsync() => Task.CompletedTask;
    }

    public class AsyncClassReturnsTaskOfTMissingGuard
    {
        public Task<int> MethodAsync(string arg) => Task.FromResult(0);
    }

    public class AsyncClassThrowsDirectNullReference
    {
        public async Task MethodAsync(string arg)
        {
            // NullReferenceException thrown before first await - will be direct, not wrapped
            _ = arg.Length;
            await Task.CompletedTask;
        }
    }
}