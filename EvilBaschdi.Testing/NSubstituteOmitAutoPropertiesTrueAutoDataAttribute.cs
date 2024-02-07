namespace EvilBaschdi.Testing;

/// <inheritdoc />
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