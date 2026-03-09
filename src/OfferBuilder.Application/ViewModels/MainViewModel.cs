using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OfferBuilder.Application.Interfaces;
using OfferBuilder.Domain.Entities;

namespace OfferBuilder.Application.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IOfferRepository _offerRepository;
    private readonly IPdfRenderService _pdfRenderService;
    private readonly IPreviewRenderService _previewRenderService;

    [ObservableProperty] private List<Offer> offers = new();
    [ObservableProperty] private Offer? selectedOffer;
    [ObservableProperty] private List<OfferSection> sections = new();
    [ObservableProperty] private string previewPath = string.Empty;

    public MainViewModel(IOfferRepository offerRepository, IPdfRenderService pdfRenderService, IPreviewRenderService previewRenderService)
    {
        _offerRepository = offerRepository;
        _pdfRenderService = pdfRenderService;
        _previewRenderService = previewRenderService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        Offers = (await _offerRepository.GetAllAsync()).ToList();
        SelectedOffer = Offers.FirstOrDefault();
        if (SelectedOffer is not null)
            Sections = (await _offerRepository.GetSectionsAsync(SelectedOffer.Id)).OrderBy(x => x.SortOrder).ToList();
    }

    [RelayCommand]
    public async Task NewOfferAsync()
    {
        var offer = new Offer
        {
            Number = $"KP-{DateTime.Now:yyyyMMddHHmmss}",
            DateCreated = DateTime.Now,
            ValidUntil = DateTime.Now.AddDays(14)
        };
        offer.Id = await _offerRepository.SaveAsync(offer);
        await LoadAsync();
        SelectedOffer = Offers.FirstOrDefault(x => x.Id == offer.Id);
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (SelectedOffer is null) return;
        await _offerRepository.SaveAsync(SelectedOffer);
        await _offerRepository.SaveSectionsAsync(SelectedOffer.Id, Sections);
    }

    [RelayCommand]
    public async Task PreviewAsync()
    {
        if (SelectedOffer is null) return;
        PreviewPath = await _previewRenderService.RenderPreviewAsync(SelectedOffer, Sections, "preview");
    }

    [RelayCommand]
    public async Task ExportPdfAsync()
    {
        if (SelectedOffer is null) return;
        await _pdfRenderService.RenderAsync(SelectedOffer, Sections, Path.Combine("exports", $"{SelectedOffer.Number}.pdf"));
    }
}
