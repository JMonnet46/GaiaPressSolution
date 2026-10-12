Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Hosting

Module Program
    Sub Main(args As String())
        ' ✅ Point d'entrée pour une application ASP.NET Core
        ' Crée un builder pour configurer l'application web
        Dim builder = WebApplication.CreateBuilder(args)

        ' ✅ Ajoutez vos services (DbContext, etc.)
        builder.Services.AddDbContext(Of GaiaPressDbContext)(Function(options)
                                                                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                                                             End Function)

        ' ✅ Ajoutez vos contrôleurs et services
        builder.Services.AddControllers()
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        ' ✅ Créez l'application web
        Dim app = builder.Build()

        ' ✅ Configurez le pipeline HTTP
        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI()
        End If

        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        ' ✅ Lancez l'application
        app.Run()
    End Sub
End Module