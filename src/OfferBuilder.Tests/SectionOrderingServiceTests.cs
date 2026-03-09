using OfferBuilder.Application.Services;
using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Tests;

public class SectionOrderingServiceTests
{
    [Fact]
    public void Reorder_MovesItemAndReindexes()
    {
        var service = new SectionOrderingService();
        var sections = new List<OfferSection>
        {
            new() { Title = "A", SortOrder = 0 },
            new() { Title = "B", SortOrder = 1 },
            new() { Title = "C", SortOrder = 2 }
        };

        var result = service.Reorder(sections, 2, 0).ToList();

        Assert.Equal("C", result[0].Title);
        Assert.Equal(0, result[0].SortOrder);
        Assert.Equal(2, result[2].SortOrder);
    }
}
