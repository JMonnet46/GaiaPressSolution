Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports System.IO

Public Module Program
    Public Sub Main(args As String())
        Dim builder As WebApplicationBuilder = WebApplication.CreateBuilder(args)

        ' Ajoute les services nécessaires
        builder.Services.AddControllersWithViews()

        Dim app As WebApplication = builder.Build()

        ' Configuration de l'application
        If app.Environment.IsDevelopment() Then
            app.UseDeveloperExceptionPage()
        Else
            app.UseExceptionHandler("/Home/Error")
            app.UseHsts()
        End If

        ' ✅ POINT IMPORTANT : Configure le chemin pour les fichiers statiques
        app.UseStaticFiles(New StaticFileOptions() With {
            .FileProvider = New Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                Path.Combine(AppContext.BaseDirectory, "wwwroot", "html"))
        })

        app.UseHttpsRedirection()
        app.UseRouting()

        app.UseEndpoints(Function(endpoints)
                             endpoints.MapControllerRoute(
                                 name:="default",
                                 pattern:="{controller=Home}/{action=Index}/{id?}")
                         End Function)

        app.Run()
    End Sub
End Module