using OfferBuilder.Application.Interfaces;
using OfferBuilder.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OfferBuilder.Infrastructure.Services;

public class PdfRenderService : IPdfRenderService
{
    public Task<string> RenderAsync(Offer offer, IReadOnlyList<OfferSection> sections, string outputPath, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(25);
                page.Header().Text($"{offer.Title} — {offer.Number}").SemiBold().FontSize(16);
                page.Content().Column(col =>
                {
                    col.Item().Text(offer.Subtitle).FontSize(12);
                    col.Item().Text($"Пакет: {offer.PackageName}");
                    col.Item().Text($"Стоимость: {offer.TotalAmount:N0} ₽").Bold().FontSize(14).FontColor(Colors.Red.Darken2);
                    foreach (var section in sections.Where(s => s.IsVisible).OrderBy(x => x.SortOrder))
                    {
                        col.Item().PaddingTop(10).Text(section.Title).SemiBold();
                        col.Item().Text(section.Subtitle);
                        col.Item().Text(section.ContentJson);
                    }
                });
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Страница ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf(outputPath);

        return Task.FromResult(outputPath);
    }
}

public class PreviewRenderService(IPdfRenderService pdfRenderService) : IPreviewRenderService
{
    public Task<string> RenderPreviewAsync(Offer offer, IReadOnlyList<OfferSection> sections, string outputFolder, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(outputFolder);
        var path = Path.Combine(outputFolder, $"preview_{offer.Number}.pdf");
        return pdfRenderService.RenderAsync(offer, sections, path, cancellationToken);
    }
}
