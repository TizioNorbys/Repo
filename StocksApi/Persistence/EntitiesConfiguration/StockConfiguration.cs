using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StocksApi.Persistence.Entities;
using StocksApi.Persistence.ValueConverters;

namespace StocksApi.Persistence.EntitiesConfiguration;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.DateTime)
            .HasConversion<CustomDateTimeConverter>();

        builder.HasIndex(s => new { s.Symbol, s.DateTime })
            .IsUnique();
    }
}