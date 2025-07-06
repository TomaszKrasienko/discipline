using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.users.domain.unitTests.Subscriptions.ValueObjects;

public sealed class PriceTests
{
    [Fact]
    public void GivenValidPricePerYearAndPerMonth_WhenCreate_ThenReturnPriceWithPerYearAndPerMonthValues()
    {
        // Arrange
        const decimal pricePerYear = 234m;
        const decimal pricePerMonth = 32m;
        var currency = Currency.Pln;
        
        // Act
        var result = Price.Create(pricePerMonth, pricePerYear, currency);
        
        // Assert
        result.PerMonth.ShouldBe(pricePerMonth);
        result.PerYear.ShouldBe(pricePerYear);
        result.Currency.ShouldBe(currency);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidPrice))]
    public void GivenPriceLessThanZero_WhenCreate_ThenThrowsDomainExceptionWithCode(decimal perMonth, decimal perYear,
        string code)
    {
        // Act
        var exception = Record.Exception(() => Price.Create(perMonth, perYear, Currency.Pln));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvalidPrice()
    {
        yield return [-1m, 3m, "Subscription.Price.PerMonthLessThanZero"];
        yield return [1m, -1m, "Subscription.Price.PerYearLessThanZero"];
    }
}