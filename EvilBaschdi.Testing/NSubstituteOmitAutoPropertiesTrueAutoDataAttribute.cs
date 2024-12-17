namespace EvilBaschdi.Testing;

/// <inheritdoc />
// ReSharper disable once UnusedType.Global
public class NSubstituteOmitAutoPropertiesTrueAutoDataAttribute : AutoDataAttribute
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public NSubstituteOmitAutoPropertiesTrueAutoDataAttribute()
        : base(() => new Fixture().Customize(new OmitAutoPropertiesTrueCompositeCustomization(new AutoNSubstituteCustomization())))
    {
    }
}