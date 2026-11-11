using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PortfolioApi.Data;

/// <summary>
/// Used by `dotnet ef migrations` — targets PostgreSQL (production provider).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=portfolio;Username=postgres;Password=postgres");

        return new AppDbContext(optionsBuilder.Options);
    }
}
