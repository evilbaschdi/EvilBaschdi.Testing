namespace EvilBaschdi.Testing;

/// <inheritdoc />
// ReSharper disable once UnusedType.Global
public class NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute : InlineAutoDataAttribute
{
    /// <inheritdoc />
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="values"></param>
    public NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute(params object[] values)
        : base(() => new Fixture().Customize(new OmitAutoPropertiesTrueCompositeCustomization(new AutoNSubstituteCustomization())),
            values)
    {
    }
}