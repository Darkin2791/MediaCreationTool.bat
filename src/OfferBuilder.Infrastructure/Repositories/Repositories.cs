using Microsoft.Data.Sqlite;
using OfferBuilder.Application.Interfaces;
using OfferBuilder.Domain.Entities;
using OfferBuilder.Domain.Enums;

namespace OfferBuilder.Infrastructure.Repositories;

public class OfferRepository(string connectionString) : IOfferRepository
{
    public async Task<IReadOnlyList<Offer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<Offer>();
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Number, DateCreated, ValidUntil, Title, Subtitle, PackageName, TotalAmount, GuaranteeText, Notes, Status, TemplateId FROM Offers ORDER BY Id DESC";
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new Offer
            {
                Id = reader.GetInt32(0), Number = reader.GetString(1), DateCreated = DateTime.Parse(reader.GetString(2)), ValidUntil = DateTime.Parse(reader.GetString(3)),
                Title = reader.GetString(4), Subtitle = reader.GetString(5), PackageName = reader.GetString(6), TotalAmount = reader.GetDecimal(7),
                GuaranteeText = reader.GetString(8), Notes = reader.GetString(9), Status = reader.GetString(10), TemplateId = reader.GetInt32(11)
            });
        }
        return result;
    }

    public async Task<Offer?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => (await GetAllAsync(cancellationToken)).FirstOrDefault(x => x.Id == id);

    public async Task<int> SaveAsync(Offer offer, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        if (offer.Id == 0)
        {
            command.CommandText = "INSERT INTO Offers (Number, DateCreated, ValidUntil, ClientId, ManagerProfileId, CompanyProfileId, Title, Subtitle, PackageName, TotalAmount, GuaranteeText, Notes, Status, TemplateId) VALUES ($n,$dc,$vu,0,0,0,$t,$st,$p,$ta,$g,$no,$s,0); SELECT last_insert_rowid();";
        }
        else
        {
            command.CommandText = "UPDATE Offers SET Number=$n, ValidUntil=$vu, Title=$t, Subtitle=$st, PackageName=$p, TotalAmount=$ta, GuaranteeText=$g, Notes=$no, Status=$s WHERE Id=$id; SELECT $id;";
            command.Parameters.AddWithValue("$id", offer.Id);
        }
        command.Parameters.AddWithValue("$n", offer.Number);
        command.Parameters.AddWithValue("$dc", offer.DateCreated.ToString("O"));
        command.Parameters.AddWithValue("$vu", offer.ValidUntil.ToString("O"));
        command.Parameters.AddWithValue("$t", offer.Title);
        command.Parameters.AddWithValue("$st", offer.Subtitle);
        command.Parameters.AddWithValue("$p", offer.PackageName);
        command.Parameters.AddWithValue("$ta", offer.TotalAmount);
        command.Parameters.AddWithValue("$g", offer.GuaranteeText);
        command.Parameters.AddWithValue("$no", offer.Notes);
        command.Parameters.AddWithValue("$s", offer.Status);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Offers WHERE Id=$id";
        command.Parameters.AddWithValue("$id", id);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<int> DuplicateAsync(int id, CancellationToken cancellationToken = default)
    {
        var offer = await GetByIdAsync(id, cancellationToken) ?? throw new InvalidOperationException();
        offer.Id = 0;
        offer.Number += "-COPY";
        return await SaveAsync(offer, cancellationToken);
    }

    public async Task<IReadOnlyList<OfferSection>> GetSectionsAsync(int offerId, CancellationToken cancellationToken = default)
    {
        var result = new List<OfferSection>();
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, SortOrder, SectionType, Title, Subtitle, ContentJson, IsVisible FROM OfferSections WHERE OfferId=$id ORDER BY SortOrder";
        command.Parameters.AddWithValue("$id", offerId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new OfferSection { Id = reader.GetInt32(0), OfferId = offerId, SortOrder = reader.GetInt32(1), SectionType = (SectionType)reader.GetInt32(2), Title = reader.GetString(3), Subtitle = reader.GetString(4), ContentJson = reader.GetString(5), IsVisible = reader.GetInt32(6) == 1 });
        }
        return result;
    }

    public async Task SaveSectionsAsync(int offerId, IReadOnlyList<OfferSection> sections, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var delete = connection.CreateCommand();
        delete.CommandText = "DELETE FROM OfferSections WHERE OfferId=$id";
        delete.Parameters.AddWithValue("$id", offerId);
        await delete.ExecuteNonQueryAsync(cancellationToken);
        foreach (var s in sections)
        {
            await using var insert = connection.CreateCommand();
            insert.CommandText = "INSERT INTO OfferSections (OfferId, SortOrder, SectionType, Title, Subtitle, ContentJson, IsVisible) VALUES ($o,$so,$st,$t,$sb,$c,$v)";
            insert.Parameters.AddWithValue("$o", offerId);
            insert.Parameters.AddWithValue("$so", s.SortOrder);
            insert.Parameters.AddWithValue("$st", (int)s.SectionType);
            insert.Parameters.AddWithValue("$t", s.Title);
            insert.Parameters.AddWithValue("$sb", s.Subtitle);
            insert.Parameters.AddWithValue("$c", s.ContentJson);
            insert.Parameters.AddWithValue("$v", s.IsVisible ? 1 : 0);
            await insert.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public Task<IReadOnlyList<OfferItem>> GetItemsAsync(int offerId, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<OfferItem>>(new List<OfferItem>());
    public Task SaveItemsAsync(int offerId, IReadOnlyList<OfferItem> items, CancellationToken cancellationToken = default) => Task.CompletedTask;
}

public class TemplateRepository(string connectionString) : ITemplateRepository
{
    public async Task<IReadOnlyList<Template>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<Template>();
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Description, PreviewImagePath, StructureJson, IsDefault, CreatedAt FROM Templates";
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            list.Add(new Template { Id = reader.GetInt32(0), Name = reader.GetString(1), Description = reader.GetString(2), PreviewImagePath = reader.IsDBNull(3) ? "" : reader.GetString(3), StructureJson = reader.GetString(4), IsDefault = reader.GetInt32(5) == 1, CreatedAt = DateTime.Parse(reader.GetString(6)) });
        return list;
    }
    public Task<int> SaveAsync(Template template, CancellationToken cancellationToken = default) => Task.FromResult(0);
}

public class CatalogRepository(string connectionString) : ICatalogRepository
{
    public async Task<IReadOnlyList<CatalogItem>> SearchAsync(string category, string query, CancellationToken cancellationToken = default)
    {
        var list = new List<CatalogItem>();
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Category, Name, Description, PriceModifier, Tags, IsFavorite, IsActive FROM CatalogItems WHERE Category LIKE $c AND Name LIKE $q";
        cmd.Parameters.AddWithValue("$c", $"%{category}%");
        cmd.Parameters.AddWithValue("$q", $"%{query}%");
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            list.Add(new CatalogItem { Id = reader.GetInt32(0), Category = reader.GetString(1), Name = reader.GetString(2), Description = reader.GetString(3), PriceModifier = reader.GetDecimal(4), Tags = reader.GetString(5), IsFavorite = reader.GetInt32(6) == 1, IsActive = reader.GetInt32(7) == 1 });
        return list;
    }
    public Task<IReadOnlyList<CatalogItem>> GetRecentAsync(string category, CancellationToken cancellationToken = default) => SearchAsync(category, string.Empty, cancellationToken);
}
