Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Models

Namespace GaiaPressCoreV17.Data
    Public Class GaiaPressDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub

        Public Property LesChantiersDb As DbSet(Of Chantier)
        Public Property ListeForageDb As DbSet(Of Forage)
        Public Property ListeEssaiForageDb As DbSet(Of Essai_Forage)
        Public Property ListeMesureEssaiForageDb As DbSet(Of MesureEssaiForage)
        Public Property ListeEssaiResistancesDb As DbSet(Of EssaiResistance)
        Public Property ListeMesureResistanceDb As DbSet(Of MesureResistance)
        Public Property ListeEssaiExpansionsDb As DbSet(Of EssaiExpansion)
        Public Property ListeMesureExpansionsDb As DbSet(Of MesureExpansion)
        Public Property ListeSondeDb As DbSet(Of Probe)
        Public Property ListeSolDb As DbSet(Of Sol)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Chantier)().HasKey(Function(c) c.refChantier)
            modelBuilder.Entity(Of Essai_Forage)().HasKey(Function(e) e.refEssai)
            modelBuilder.Entity(Of EssaiResistance)().HasKey(Function(e) e.RefEssai)
            modelBuilder.Entity(Of EssaiExpansion)().HasKey(Function(e) e.RefEssai)
            modelBuilder.Entity(Of Probe)().HasKey(Function(p) p.RefSonde)
            modelBuilder.Entity(Of Sol)().HasKey(Function(s) s.RefSol)
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.UseSqlServer("Server=(localdb)\MSSQLLocalDB;Database=GaiaPressDb;Trusted_Connection=True;")
        End Sub
    End Class
End Namespace
