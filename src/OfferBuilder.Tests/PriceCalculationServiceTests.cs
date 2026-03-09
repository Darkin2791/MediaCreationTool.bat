using OfferBuilder.Application.Services;
using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Tests;

public class PriceCalculationServiceTests
{
    [Fact]
    public void CalculateLineTotal_AppliesDiscount()
    {
        var service = new PriceCalculationService();
        var item = new OfferItem { UnitPrice = 1000m, Quantity = 2, DiscountPercent = 10m };

        var total = service.CalculateLineTotal(item);

        Assert.Equal(1800m, total);
    }
}
