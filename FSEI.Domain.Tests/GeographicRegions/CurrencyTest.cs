using FSEI.Domain.GeographicRegions;

namespace FSEI.Domain.Tests;

public class CurrencyTests
{
    [Test]
    public void Validate_WhenCodeHasThreeCharacters_DoesNotThrow()
    {
        var currency = CreateCurrency("USD");

        Assert.DoesNotThrow(() => currency.Validate());
    }

    [TestCase("")]
    [TestCase("  ")]
    public void Validate_WhenCodeIsEmptyOrWhitespace_ThrowsInvalidOperationException(
        string code)
    {
        var currency = CreateCurrency(code);

        Assert.Throws<InvalidOperationException>(() => currency.Validate());
    }

    [TestCase("US")]
    [TestCase("USDD")]
    public void Validate_WhenCodeDoesNotHaveThreeCharacters_ThrowsInvalidOperationException(
        string code)
    {
        var currency = CreateCurrency(code);

        Assert.Throws<InvalidOperationException>(() => currency.Validate());
    }

    private static Currency CreateCurrency(string code)
    {
        return new Currency
        {
            CurrencyId = 1,
            Code = code,
            Name = "Test Currency"
        };
    }
}