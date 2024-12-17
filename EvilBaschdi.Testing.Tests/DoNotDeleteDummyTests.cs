#pragma warning disable MFA001
#pragma warning disable FAA0002

namespace EvilBaschdi.Testing.Tests;

/// <summary>
///     Do not delete this class.
///     NCrunch needs at least one test method in witch <see cref="Assert" />.Equal() is used.
/// </summary>
// ReSharper disable once TestFileNameWarning
public class DoNotDeleteDummyTests
{
    [Fact]
    public void Value_ToEnableUnitTests_Asserts1Equals1()
    {
        // Arrange
        // Act
        // Assert
        Assert.Equal(1, 1);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_ForProvidedValues_ReturnsSum(
        int dummyS1,
        int dummyS2)
    {
        // Arrange
        // Act
        var result = dummyS1 + dummyS2;

        // Assert
        Assert.Equal(dummyS1 + dummyS2, result);
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(1, 1, 2)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(2, 2, 4)]
    public void Value_ForProvidedInlineValues_ReturnsSum(
        int dummyS1,
        int dummyS2,
        int expected)
    {
        // Arrange
        // Act
        var result = dummyS1 + dummyS2;

        // Assert
        Assert.Equal(expected, result);
    }
}