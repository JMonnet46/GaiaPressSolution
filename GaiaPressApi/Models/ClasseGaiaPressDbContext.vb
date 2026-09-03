Namespace GaiaPress.core.V17.Datas

    Public Class GaiaPressDbContext
        Inherits DbContext

        ' 🔹 Déclarez vos DbSet (tables de la base de données)
        ' Définissez ici tous vos DbSet
        ' Déclarez vos DbSet pour chaque table
        'Public Property LesChantiers As DbSet(Of Models.LesChantiers)
        'Public Property Forages As DbSet(Of Models.Forage)

        ' Propriété pour EssaiForage
        Public Property EssaisForage As DbSet(Of Essai_Forage)
        Public Property MesuresEssaiForage As DbSet(Of Models.MesuresEssaiForage)
        Public Property Sols As DbSet(Of Models.Sol)
        Public Property Sondes As DbSet(Of Models.Probe)
        Public Property EssaisExpansion As DbSet(Of Models.Essai_Expansion)
        Public Property EssaisResistance As DbSet(Of Models.Essai_Resistance) ' 1ere instance
        Public Property MesuresExpansion As DbSet(Of MesuresExpansion) ' ✅ Corrigé ici
        Public Property MesuresResistance As DbSet(Of MesuresResistance) ' ✅ Corrigé ici
        ' Ajoutez d'autres DbSet selon vos besoins
        Public Property Forages As DbSet(Of Forage)

        ' Ajoutez d'autres DbSet pour vos entités

        ' 🔹 Configuration de la connexion à la base de données
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            ' MODIFIEZ CETTE LIGNE AVEC VOTRE CHAÎNE DE CONNEXION !
            ' Exemple pour SQL Server local :
            optionsBuilder.UseSqlServer("Server=.;Database=GaiaPressDB;Trusted_Connection=True;TrustServerCertificate=True")
            'optionsBuilder.UseSqlServer(ChaîneConnexion)
            ' Exemple pour SQL Server distant
            ' optionsBuilder.UseSqlServer(ChaîneConnexion)
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            ' Exemple : Configurer une clé primaire
            modelBuilder.Entity(Of Essai_Resistance)().HasKey(Function(e) e.Id)

            ' Exemple : Configurer une relation 1-N
            modelBuilder.Entity(Of Essai_Resistance)() _
                .HasOne(Function(e) e.Forage) _
                .WithMany(Function(f) f.EssaisResistance) _
                .HasForeignKey(Function(e) e.ForageId)
        End Sub
        ' ===================================================================================================================================================


        ' Constructeur pour configurer la connexion
        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub

        'Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        '    optionsBuilder.UseSqlServer("Server=.;Database=GaiaPress;Trusted_Connection=True;")
        'End Sub

    End Class
End Namespace