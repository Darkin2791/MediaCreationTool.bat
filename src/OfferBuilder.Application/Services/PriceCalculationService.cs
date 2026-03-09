using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Application.Services;

public class PriceCalculationService
{
    public decimal CalculateLineTotal(OfferItem item)
    {
        var gross = item.UnitPrice * item.Quantity;
        var discount = gross * (item.DiscountPercent / 100m);
        return decimal.Round(gross - discount, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateOfferTotal(IEnumerable<OfferItem> items) => items.Sum(CalculateLineTotal);
}
