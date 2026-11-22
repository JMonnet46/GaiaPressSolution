using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using GaiaPressBlazorWebAPP.Components;
using GaiaPressBlazorWebAPP.Data;
using GaiaPressBlazorWebAPP.Services;
using GaiaPressCoreV17.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Ajout du support pour Azure Key Vault
if (builder.Environment.IsDevelopment())
{
    try
    {
        builder.Configuration.AddAzureKeyVault(new Uri("https://gaiatech-keyvault.vault.azure.net/"), new AzureCliCredential());
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur KeyVault (dev) : " + ex);
    }
}

// Ajout des services au conteneur
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Ajout des services personnalisés
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddLocalization();
builder.Services.AddScoped<ChantierState>();
builder.Services.AddScoped<IEssaiService, EssaiService>();

// Au démarrage de l'app - réchauffe la connexion Azure SQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        using (var warmup = new SqlConnection(connectionString))
        {
            warmup.Open(); // force le réveil Azure
            Console.WriteLine("Connexion Azure SQL établie (warmup).");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Warmup échoué (sera retenté par EF) : " + ex.Message);
    }
}

// Ajout du contexte de base de données (avec logs EF)
Action<DbContextOptionsBuilder> gaiaPressOptions = options =>
{
    options
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        .EnableSensitiveDataLogging();

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("GaiaPressBlazorWebAPP");
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null); // retry sur timeout aussi
        });
};

builder.Services.AddDbContextFactory<ClasseGaiaPressDbContext>(gaiaPressOptions);

builder.Services.AddScoped<IForageService, ForageService>();

var app = builder.Build();

// Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var supportedCultures = new[] { "fr-FR", "en-GB" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("fr-FR")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);   // ← ici
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();