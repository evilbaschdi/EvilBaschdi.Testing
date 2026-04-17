namespace EvilBaschdi.Testing.Tests;

public class OmitAutoPropertiesTrueCompositeCustomizationTests
{
    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    //{
    //   assertion.Verify(typeof(OmitAutoPropertiesTrueCompositeCustomization).GetConstructors());
    //}

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    internal void Constructor_ReturnsInterfaceName(OmitAutoPropertiesTrueCompositeCustomization sut)
    {
        sut.Should().BeAssignableTo<CompositeCustomization>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OmitAutoPropertiesTrueCompositeCustomization).GetMethods()
                                                                             .Where(method => !method.IsAbstract &&
                                                                                              method.DeclaringType == typeof(OmitAutoPropertiesTrueCompositeCustomization)));
    }
}