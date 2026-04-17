namespace EvilBaschdi.Testing;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="values"></param>
// ReSharper disable once UnusedType.Global
public class NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute(
    params object[] values)
    : InlineAutoDataAttribute(() => new Fixture().Customize(new OmitAutoPropertiesTrueCompositeCustomization(new AutoNSubstituteCustomization())), values);