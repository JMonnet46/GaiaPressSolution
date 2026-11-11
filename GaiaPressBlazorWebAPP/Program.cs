using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using GaiaPressBlazorWebAPP.Data;
using GaiaPressBlazorWebAPP.Services;
using GaiaPressCoreV17.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Ajout du support pour Azure Key Vault
if (builder.Environment.IsDevelopment())
{
    try
    {
        Console.WriteLine("Début AddAzureKeyVault...");
        builder.Configuration.AddAzureKeyVault(new Uri("https://gaiapress-key-vault.vault.azure.net/"), new AzureCliCredential());
        Console.WriteLine("Fin AddAzureKeyVault.");
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
builder.Services.AddDbContext<GaiaPressDbContext>(options =>
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
});

builder.Services.AddScoped<IForageService, ForageService>();

var app = builder.Build();

// Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<GaiaPressBlazorWebAPP.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();