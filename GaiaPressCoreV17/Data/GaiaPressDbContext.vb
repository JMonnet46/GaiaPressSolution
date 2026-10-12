Imports Microsoft.EntityFrameworkCore
Imports GaiaPress.Core.V17.Models


Namespace GaiaPress.Core.V17

    Public Class GaiaPressDbContext
        Inherits DbContext

        Public Property EssaisForage As DbSet(Of Essai_Forage)
        Public Property MesuresEssaiForage As DbSet(Of MesuresEssaiForage)
        Public Property EssaisResistance As DbSet(Of Essai_Resistance)
        Public Property MesuresResistance As DbSet(Of MesuresResistance)
        Public Property EssaisExpansion As DbSet(Of Essai_Expansion)
        Public Property MesuresExpansion As DbSet(Of MesuresExpansion)
        Public Property Probes As DbSet(Of Probe)
        Public Property Sols As DbSet(Of Sol)

        ' =======================================================
        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

            ' =============================================
            ' Configuration pour Essai_Forage
            ' =============================================
            modelBuilder.Entity(Of Essai_Forage)().HasKey(Function(e) e.E_RefEssai)

            modelBuilder.Entity(Of Essai_Forage)() _
                .HasOne(Function(e) e.E_MySonde) _
                .WithMany(Function(p) p.P_MesEssaisForage) _
                .HasForeignKey(Function(e) e.E_refSonde)

            modelBuilder.Entity(Of Essai_Forage)() _
                .HasOne(Function(e) e.E_MySol) _
                .WithMany() _
                .HasForeignKey(Function(e) e.E_RefSol)

            modelBuilder.Entity(Of Essai_Forage)() _
                .HasMany(Function(e) e.E_Mesures) _
                .WithOne(Function(m) m.Essai_Forage) _
                .HasForeignKey(Function(m) m.RefEssai)

            ' =============================================
            ' Configuration pour Essai_Resistance
            ' =============================================
            modelBuilder.Entity(Of Essai_Resistance)().HasKey(Function(e) e.E_RefEssai)

            modelBuilder.Entity(Of Essai_Resistance)() _
                .HasOne(Function(e) e.E_MySonde) _
                .WithMany(Function(p) p.P_MaResistance) _
                .HasForeignKey(Function(e) e.E_RefSonde)

            ' =============================================
            ' Configuration pour Essai_Expansion
            ' =============================================
            modelBuilder.Entity(Of Essai_Expansion)().HasKey(Function(e) e.E_RefEssai)

            modelBuilder.Entity(Of Essai_Expansion)() _
                .HasOne(Function(e) e.E_MySonde) _
                .WithMany(Function(p) p.P_MaExpansion) _
                .HasForeignKey(Function(e) e.E_RefSonde)

            modelBuilder.Entity(Of Essai_Expansion)() _
                .HasMany(Function(e) e.Mesures_Expansion) _
                .WithOne() _
                .HasForeignKey(Function(m) m.RefEssai)

            ' =============================================
            ' Configuration pour Probe
            ' =============================================
            modelBuilder.Entity(Of Probe)().HasKey(Function(p) p.P_refSonde)

            ' =============================================
            ' Configuration pour Sol
            ' =============================================
            modelBuilder.Entity(Of Sol)().HasKey(Function(s) s.RefSol)
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.UseSqlServer("Server=(localdb)\MSSQLLocalDB;Database=GaiaPressDb;Trusted_Connection=True;")
        End Sub
    End Class
End Namespace