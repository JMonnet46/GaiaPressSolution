Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Data  ' ✅ Correct : GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Services  ' ✅ Correct : GaiaPressCoreV17.Services
Imports Microsoft.Extensions.Configuration

Namespace GaiaPressCoreV17
    Public Class Program

        Public Shared Sub Main(args As String())
            Dim builder = WebApplication.CreateBuilder(args)

            ' ✅ Configuration du DbContext avec la bonne chaîne de connexion
            builder.Services.AddDbContext(Of GaiaPressDbContext)(
                Sub(options)
                    options.UseSqlServer(builder.Configuration.GetConnectionString("GaiaPressDbConnection"))
                End Sub)

            ' ✅ Configuration des services (corrigé : Services au pluriel)
            builder.Services.AddControllers()

            ' ✅ Ajout des services métiers
            builder.Services.AddScoped(Of ChantierService)()

            ' ✅ Construction de l'application
            Dim app = builder.Build()

            ' Configuration du pipeline HTTP
            app.UseHttpsRedirection()
            app.UseAuthorization()
            app.MapControllers()

            app.Run()
        End Sub

    End Class
End Namespace