using AutoFixture.Xunit2;

namespace EvilBaschdi.Testing
{
    /// <inheritdoc />
    // ReSharper disable once UnusedType.Global
    public class NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="values"></param>
        public NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute(params object[] values)
            : base(new NSubstituteOmitAutoPropertiesTrueAutoDataAttribute(), values)
        {
        }
    }
}