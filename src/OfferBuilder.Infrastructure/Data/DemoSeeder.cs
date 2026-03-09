using Microsoft.Data.Sqlite;

namespace OfferBuilder.Infrastructure.Data;

public static class DemoSeeder
{
    public static void Seed(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM Templates";
        if ((long)check.ExecuteScalar()! > 0) return;

        using var tx = connection.BeginTransaction();
        var cmd = connection.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = @"
INSERT INTO Templates (Name, Description, StructureJson, IsDefault, CreatedAt) VALUES
('Стандартный бизнес', 'Классический шаблон КП', '[{""type"":""Header""},{""type"":""PriceHighlight""},{""type"":""FinalAdvantages""}]', 1, datetime('now')),
('Визуальное КП по строительству дома', 'С изображениями и тех. блоками', '[{""type"":""Cover""},{""type"":""Plan""},{""type"":""Construction""}]', 0, datetime('now'));
INSERT INTO CatalogItems (Category, Name, Description, PriceModifier, Tags, IsFavorite, IsActive) VALUES
('Отделка фасада', 'Штукатурка + покраска', 'Декоративная штукатурка и влагостойкая краска', 250000, 'фасад,классика', 1, 1),
('Кровля', 'Металлочерепица', 'Кровля с утеплением 200 мм', 180000, 'крыша,металл', 0, 1),
('Инженерия', 'Пакет Комфорт', 'Электрика, отопление, водоснабжение', 450000, 'инженерия,комфорт', 1, 1),
('Продажи', 'Преимущества', 'Гарантия 5 лет, фиксированная смета, поэтапная оплата', 0, 'cta,sales', 0, 1);
";
        cmd.ExecuteNonQuery();
        tx.Commit();
    }
}
