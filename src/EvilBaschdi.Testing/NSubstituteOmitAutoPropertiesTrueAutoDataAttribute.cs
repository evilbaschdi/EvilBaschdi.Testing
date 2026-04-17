namespace EvilBaschdi.Testing;

/// <inheritdoc />
public class NSubstituteOmitAutoPropertiesTrueAutoDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new OmitAutoPropertiesTrueCompositeCustomization(new AutoNSubstituteCustomization())));