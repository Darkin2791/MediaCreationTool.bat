using Microsoft.Data.Sqlite;

namespace OfferBuilder.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"
CREATE TABLE IF NOT EXISTS Offers (Id INTEGER PRIMARY KEY AUTOINCREMENT, Number TEXT, DateCreated TEXT, ValidUntil TEXT, ClientId INTEGER, ManagerProfileId INTEGER, CompanyProfileId INTEGER, Title TEXT, Subtitle TEXT, PackageName TEXT, TotalAmount REAL, GuaranteeText TEXT, Notes TEXT, Status TEXT, TemplateId INTEGER);
CREATE TABLE IF NOT EXISTS OfferSections (Id INTEGER PRIMARY KEY AUTOINCREMENT, OfferId INTEGER, SortOrder INTEGER, SectionType INTEGER, Title TEXT, Subtitle TEXT, ContentJson TEXT, IsVisible INTEGER);
CREATE TABLE IF NOT EXISTS OfferItems (Id INTEGER PRIMARY KEY AUTOINCREMENT, OfferId INTEGER, Title TEXT, Description TEXT, Category TEXT, Quantity REAL, Unit TEXT, UnitPrice REAL, DiscountPercent REAL, TotalPrice REAL, CatalogItemId INTEGER);
CREATE TABLE IF NOT EXISTS Templates (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Description TEXT, PreviewImagePath TEXT, StructureJson TEXT, IsDefault INTEGER, CreatedAt TEXT);
CREATE TABLE IF NOT EXISTS CatalogItems (Id INTEGER PRIMARY KEY AUTOINCREMENT, Category TEXT, Name TEXT, Description TEXT, PriceModifier REAL, Tags TEXT, IsFavorite INTEGER, IsActive INTEGER);
";
        command.ExecuteNonQuery();
    }
}
