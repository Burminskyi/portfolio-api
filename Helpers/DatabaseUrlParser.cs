namespace PortfolioApi.Helpers;

/// <summary>
/// Converts Render-style DATABASE_URL (postgres://user:pass@host:port/db)
/// into an Npgsql connection string for EF Core.
/// </summary>
public static class DatabaseUrlParser
{
    public static string? TryParse(string? databaseUrl)
    {
        if (string.IsNullOrWhiteSpace(databaseUrl))
        {
            return null;
        }

        if (!databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) &&
            !databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
        {
            // Already a standard connection string
            return databaseUrl;
        }

        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var database = uri.AbsolutePath.TrimStart('/');

        // Render DATABASE_URL often omits the port; Uri.Port is then -1.
        var port = uri.IsDefaultPort || uri.Port <= 0 ? 5432 : uri.Port;

        var sslMode = "Require";
        if (!string.IsNullOrEmpty(uri.Query))
        {
            foreach (var part in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var pair = part.Split('=', 2);
                if (pair.Length == 2 &&
                    pair[0].Equals("sslmode", StringComparison.OrdinalIgnoreCase))
                {
                    sslMode = Uri.UnescapeDataString(pair[1]);
                    break;
                }
            }
        }

        return $"Host={uri.Host};Port={port};Database={database};Username={username};Password={password};SSL Mode={sslMode};Trust Server Certificate=true";
    }
}
