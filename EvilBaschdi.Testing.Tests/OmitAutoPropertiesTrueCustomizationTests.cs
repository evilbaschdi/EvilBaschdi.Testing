namespace EvilBaschdi.Testing.Tests;

public class OmitAutoPropertiesTrueCustomizationTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OmitAutoPropertiesTrueCustomization).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    internal void Constructor_ReturnsInterfaceName(OmitAutoPropertiesTrueCustomization sut)
    {
        sut.Should().BeAssignableTo<ICustomization>();
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(OmitAutoPropertiesTrueCustomization).GetMethods().Where(method => !method.IsAbstract));
    //}
}