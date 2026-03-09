using OfferBuilder.Domain.Enums;

namespace OfferBuilder.Domain.Entities;

public class CompanyProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LogoPath { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TaxInfo { get; set; } = string.Empty;
    public string BankDetails { get; set; } = string.Empty;
    public string SignatureName { get; set; } = string.Empty;
    public string SignatureTitle { get; set; } = string.Empty;
}

public class ManagerProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public string Telegram { get; set; } = string.Empty;
    public string WhatsApp { get; set; } = string.Empty;
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class Offer
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public DateTime ValidUntil { get; set; }
    public int ClientId { get; set; }
    public int ManagerProfileId { get; set; }
    public int CompanyProfileId { get; set; }
    public string Title { get; set; } = "Коммерческое предложение";
    public string Subtitle { get; set; } = "Комплектация под ключ";
    public string PackageName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string GuaranteeText { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft";
    public int TemplateId { get; set; }
}

public class OfferSection
{
    public int Id { get; set; }
    public int OfferId { get; set; }
    public int SortOrder { get; set; }
    public SectionType SectionType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ContentJson { get; set; } = "{}";
    public bool IsVisible { get; set; } = true;
}

public class Template
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PreviewImagePath { get; set; } = string.Empty;
    public string StructureJson { get; set; } = "[]";
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CatalogItem
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PriceModifier { get; set; }
    public string Tags { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public bool IsActive { get; set; } = true;
}

public class OfferItem
{
    public int Id { get; set; }
    public int OfferId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 1;
    public string Unit { get; set; } = "шт";
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TotalPrice { get; set; }
    public int? CatalogItemId { get; set; }
}
