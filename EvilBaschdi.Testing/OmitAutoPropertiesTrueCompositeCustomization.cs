using AutoFixture;

namespace EvilBaschdi.Testing
{
    /// <inheritdoc />
    internal class OmitAutoPropertiesTrueCompositeCustomization : CompositeCustomization
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public OmitAutoPropertiesTrueCompositeCustomization(ICustomization customization)
            : base(new OmitAutoPropertiesTrueCustomization(), customization)
        {
        }
    }
}