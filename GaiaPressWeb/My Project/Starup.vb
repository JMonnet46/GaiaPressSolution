Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting

Public Class StartUp
    Public Sub ConfigureServices(ByVal services As IServiceCollection)
        services.AddControllersWithViews()
    End Sub

    Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IWebHostEnvironment)
        If env.IsDevelopment() Then
            app.UseDeveloperExceptionPage()
        Else
            app.UseExceptionHandler("/Home/Error")
            app.UseHsts()
        End If

        app.UseHttpsRedirection()
        app.UseStaticFiles()
        app.UseRouting() ' <-- Obligatoire pour ASP.NET Core 3.1+
        app.UseAuthorization()

        app.UseEndpoints(Sub(endpoints)
                             endpoints.MapControllerRoute(
                                 name:="default",
                                 pattern:="{controller=Home}/{action=Index}/{id?}")
                             endpoints.Map("/", Sub(context)
                                                    context.Response.Redirect("/Menu_Principal.html")
                                                End Sub)
                         End Sub)
    End Sub
End Class