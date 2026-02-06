using System.Diagnostics.CodeAnalysis;
using EvilBaschdi.Testing.Extensions;

namespace EvilBaschdi.Testing.Tests.Extensions;

public class ReflectionHelperTests
{
    public class BaseClass
    {
        public string InnerAttribute { get; }

        public BaseClass(string innerAttribute)
        {
            InnerAttribute = innerAttribute ?? throw new ArgumentNullException(nameof(innerAttribute));
        }
    }

    public class InheritedClass : BaseClass
    {
        public string Attribute { get; }

        public InheritedClass(string attribute)
            : base("inner")
        {
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReflectionHelper).GetConstructors());
    }

    [Fact]
    [SuppressMessage("Design", "MFA001:Replace Xunit assertion with Fluent Assertions equivalent", Justification = "<Pending>")]
    public void GetPrivateFieldInfo_NullGuards()
    {
        // Arrange
        var bla = new BaseClass("dasdasd");
        // Act
        Assert.Throws<ArgumentNullException>(() => bla.GetPrivateFieldInfo(null));

        bla = null;
        Assert.Throws<ArgumentNullException>(() => bla.GetPrivateFieldInfo(null));

        // Assert
    }

    [Fact]
    [SuppressMessage("Design", "MFA001:Replace Xunit assertion with Fluent Assertions equivalent", Justification = "<Pending>")]
    public void GetPrivateFieldInfo()
    {
        // Arrange

        var sut = new InheritedClass("test");
        // Act
        var result = sut.GetPrivateFieldInfo("_attribute");
        var innerResult = sut.GetPrivateFieldInfo("_innerAttribute", typeof(BaseClass));
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetPrivateFieldInfo("_innerAttribute"));

        // Assert
        result.Should().Be("test");
        innerResult.Should().Be("inner");
        exception.Message.Should().StartWith("Couldn't find property _innerAttribute in type");
    }
}