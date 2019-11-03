using AutoFixture;

namespace EvilBaschdi.Testing
{
    /// <inheritdoc />
    internal class OmitAutoPropertiesTrueCustomization : ICustomization
    {
        public void Customize(IFixture fixture) => fixture.OmitAutoProperties = true;
    }
}