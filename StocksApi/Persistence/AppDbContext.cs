using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksApi.Persistence.Entities;
using StocksApi.Persistence.EntitiesConfiguration;
using StocksApi.Persistence.ValueConverters;

namespace StocksApi.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }

    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);
    }
}