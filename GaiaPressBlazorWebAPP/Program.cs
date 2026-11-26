using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using GaiaPressBlazorWebAPP.Components;
using GaiaPressBlazorWebAPP.Services;
using GaiaPressCoreV17.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// Services Blazor (Server interactif)
// ---------------------------------------------------------------
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = builder.Environment.IsDevelopment();
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(60);
    });

// SignalR (utilisé par Blazor Server)
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.MaximumReceiveMessageSize = 1024 * 1024; // 1 Mo
});

// ---------------------------------------------------------------
// Azure Key Vault (production uniquement)
// ---------------------------------------------------------------
if (!builder.Environment.IsDevelopment())
{
    try
    {
        builder.Configuration.AddAzureKeyVault(
            new Uri("https://gaiatech-keyvault.vault.azure.net/"),
            new AzureCliCredential());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur KeyVault (prod) : {ex.Message}");
    }
}

// ---------------------------------------------------------------
// Services applicatifs
// ---------------------------------------------------------------
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ChantierState>();
builder.Services.AddScoped<IEssaiService, EssaiService>();
builder.Services.AddScoped<IForageService, ForageService>();
builder.Services.AddLocalization();

// ---------------------------------------------------------------
// Base de données
// ---------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "La chaîne de connexion 'DefaultConnection' est manquante dans appsettings.json.");

Console.WriteLine($"Chaîne de connexion utilisée : {connectionString[..Math.Min(20, connectionString.Length)]}...");

Action<DbContextOptionsBuilder> configureDb = options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null));

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
};

builder.Services.AddDbContextFactory<GaiaPress_Db_Context>(configureDb);

var app = builder.Build();

// ---------------------------------------------------------------
// Pipeline HTTP
// ---------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Fichiers statiques (une seule fois)
app.UseStaticFiles();

// Localisation
var supportedCultures = new[] { "fr-FR", "en-GB" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("fr-FR")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseAntiforgery();

// ---------------------------------------------------------------
// Mapping Blazor : UNE SEULE fois
// ---------------------------------------------------------------
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();