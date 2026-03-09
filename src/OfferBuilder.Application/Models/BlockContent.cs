namespace OfferBuilder.Application.Models;

public class BlockContent
{
    public string Text { get; set; } = string.Empty;
    public List<string> Bullets { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public string LayoutMode { get; set; } = "TextOnly";
}
