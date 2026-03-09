using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Application.Interfaces;

public interface IOfferRepository
{
    Task<IReadOnlyList<Offer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Offer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(Offer offer, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> DuplicateAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OfferSection>> GetSectionsAsync(int offerId, CancellationToken cancellationToken = default);
    Task SaveSectionsAsync(int offerId, IReadOnlyList<OfferSection> sections, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OfferItem>> GetItemsAsync(int offerId, CancellationToken cancellationToken = default);
    Task SaveItemsAsync(int offerId, IReadOnlyList<OfferItem> items, CancellationToken cancellationToken = default);
}

public interface ITemplateRepository
{
    Task<IReadOnlyList<Template>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> SaveAsync(Template template, CancellationToken cancellationToken = default);
}

public interface ICatalogRepository
{
    Task<IReadOnlyList<CatalogItem>> SearchAsync(string category, string query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CatalogItem>> GetRecentAsync(string category, CancellationToken cancellationToken = default);
}

public interface IPdfRenderService
{
    Task<string> RenderAsync(Offer offer, IReadOnlyList<OfferSection> sections, string outputPath, CancellationToken cancellationToken = default);
}

public interface IPreviewRenderService
{
    Task<string> RenderPreviewAsync(Offer offer, IReadOnlyList<OfferSection> sections, string outputFolder, CancellationToken cancellationToken = default);
}
