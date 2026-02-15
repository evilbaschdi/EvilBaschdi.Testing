using EvilBaschdi.Testing.Extensions;

// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

namespace EvilBaschdi.Testing.Tests.Extensions;

public class ReflectionHelperTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReflectionHelper).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReflectionHelper).GetMethods().Where(method => !method.IsAbstract));
    }

    public class BaseClass(string innerAttribute1, string innerAttribute2)
    {
        private readonly string _innerAttribute1 = innerAttribute1 ?? throw new ArgumentNullException(nameof(innerAttribute1));
        private readonly string _innerAttribute2 = innerAttribute2 ?? throw new ArgumentNullException(nameof(innerAttribute2));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public class InheritedClass(string attribute) : BaseClass("inner1", "inner2")
    {
        private readonly string _attribute1 = attribute ?? throw new ArgumentNullException(nameof(attribute));
    }

    [Fact]
    public void GetPrivateFieldInfo()
    {
        // Arrange
        var sut = new InheritedClass("test");

        // Act
        var result1 = sut.GetPrivateMemberInfo("_attribute1");
        var result2 = sut.GetPrivateMemberInfo("_innerAttribute2", typeof(BaseClass));

        // Assert
        result1.Should().Be("test");
        result2.Should().Be("inner2");
    }
}