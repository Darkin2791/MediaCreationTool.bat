using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Application.Services;

public class SectionOrderingService
{
    public IReadOnlyList<OfferSection> Reorder(IReadOnlyList<OfferSection> sections, int sourceIndex, int targetIndex)
    {
        var list = sections.ToList();
        var moved = list[sourceIndex];
        list.RemoveAt(sourceIndex);
        list.Insert(targetIndex, moved);
        for (var i = 0; i < list.Count; i++)
            list[i].SortOrder = i;
        return list;
    }
}
