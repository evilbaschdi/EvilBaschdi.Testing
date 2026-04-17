namespace EvilBaschdi.Testing.Tests;

public class NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttributeTests
{
    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute).GetConstructors());
    //}

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute sut)
    {
        sut.Should().BeAssignableTo<InlineAutoDataAttribute>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute).GetMethods()
                                                                                         .Where(method => !method.IsAbstract && method.DeclaringType ==
                                                                                                    typeof(NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute)));
    }
}