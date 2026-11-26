Imports GaiaPressCoreV17.Data
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Design

Public Class DesignTimeDbContextFactory
    Implements IDesignTimeDbContextFactory(Of GaiaPress_Db_Context)

    Public Function CreateDbContext(args As String()) As GaiaPress_Db_Context _
        Implements IDesignTimeDbContextFactory(Of GaiaPress_Db_Context).CreateDbContext

        Dim optionsBuilder = New DbContextOptionsBuilder(Of GaiaPress_Db_Context)()
        ' Utilise ta chaîne de connexion locale (LocalDB ou SQL Server)
        optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=GaiaPressDb;Trusted_Connection=True;TrustServerCertificate=True")

        Return New GaiaPress_Db_Context(optionsBuilder.Options)
    End Function
End Class