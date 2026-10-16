using GaiaPressBlazorWebAPP.Components;
using GaiaPressBlazorWebAPP.Services;

namespace GaiaPressBlazorWebAPP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ============================================
        // SERVICES PERSONNALISÉS - À AJOUTER ICI
        // ============================================

        // Service de localisation (déjà existant)
        builder.Services.AddScoped<LocalizationService>();

        // Service de gestion des fichiers (localStorage)
        builder.Services.AddScoped<FileService>();

        // Service de boîtes de dialogue (confirm, alert, prompt)
        builder.Services.AddScoped<DialogService>();

        // ============================================
        // SERVICES BLAZOR (ne pas modifier)
        // ============================================

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        // ============================================
        // PIPELINE HTTP (ne pas modifier l'ordre)
        // ============================================

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found");
        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
