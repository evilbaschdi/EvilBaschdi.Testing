namespace EvilBaschdi.Testing;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
internal class OmitAutoPropertiesTrueCompositeCustomization(ICustomization customization) : CompositeCustomization(new OmitAutoPropertiesTrueCustomization(), customization)
{
}