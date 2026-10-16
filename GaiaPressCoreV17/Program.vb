Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Services
Imports Microsoft.Extensions.Configuration

Namespace GaiaPressCoreV17
    Public Class Program

        Public Shared Sub Main(args As String())
            Dim builder = WebApplication.CreateBuilder(args)

            ' DbContext avec chaîne de connexion
            builder.Services.AddDbContext(Of GaiaPressDbContext)(
                Sub(options)
                    options.UseSqlServer(builder.Configuration.GetConnectionString("GaiaPressDbConnection"))
                End Sub)

            ' Services métiers
            builder.Services.AddScoped(Of DataService)()
            builder.Services.AddScoped(Of ChantierService)()

            ' Controllers
            builder.Services.AddControllers()

            ' Build et configuration
            Dim app = builder.Build()

            app.UseHttpsRedirection()
            app.UseAuthorization()
            app.MapControllers()

            app.Run()
        End Sub

    End Class
End Namespace