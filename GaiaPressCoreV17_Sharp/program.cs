using GaiaPressCoreV17.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration du DbContext selon le provider choisi
var provider = builder.Configuration["DatabaseProvider"];
var connectionString = builder.Configuration.GetConnectionString($"GaiaPress{provider}");

builder.Services.AddDbContext<GaiaPressDbContext>(options =>
{
    switch (provider?.ToLower())
    {
        case "sqlserver":
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
            });
            break;

        case "postgresql":
            options.UseNpgsql(connectionString);
            break;

        case "mysql":
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            break;

        case "access":
            // Pour la migration uniquement
            options.UseJet(connectionString);
            break;

        default:
            throw new InvalidOperationException($"Provider '{provider}' non supporté.");
    }
});

// ... reste de la configuration