namespace EvilBaschdi.Testing.Tests;

public class NSubstituteOmitAutoPropertiesTrueAutoDataAttributeTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(NSubstituteOmitAutoPropertiesTrueAutoDataAttribute).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(NSubstituteOmitAutoPropertiesTrueAutoDataAttribute sut)
    {
        sut.Should().BeAssignableTo<AutoDataAttribute>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(NSubstituteOmitAutoPropertiesTrueAutoDataAttribute).GetMethods()
            .Where(method =>
                !method.IsAbstract &&
                method.DeclaringType == typeof(NSubstituteOmitAutoPropertiesTrueAutoDataAttribute)));
    }
}