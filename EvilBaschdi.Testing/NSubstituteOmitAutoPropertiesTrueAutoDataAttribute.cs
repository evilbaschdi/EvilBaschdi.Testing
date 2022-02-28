using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

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