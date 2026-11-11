Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Design

Public Class DesignTimeDbContextFactory
    Implements IDesignTimeDbContextFactory(Of GaiaPressDbContext)

    Public Function CreateDbContext(args As String()) As GaiaPressDbContext _
        Implements IDesignTimeDbContextFactory(Of GaiaPressDbContext).CreateDbContext

        Dim optionsBuilder = New DbContextOptionsBuilder(Of GaiaPressDbContext)()
        ' Utilise ta chaîne de connexion locale (LocalDB ou SQL Server)
        optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=GaiaPressDb;Trusted_Connection=True;TrustServerCertificate=True")

        Return New GaiaPressDbContext(optionsBuilder.Options)
    End Function
End Class