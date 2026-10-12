Imports GaiaPressCoreV17.Models
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Data

    Public Class GaiaPressDbContext
        Inherits DbContext
        Public Property LesChantiers As DbSet(Of Chantier)
        Public Property ListeForageDb As DbSet(Of Forage)
        Public Property ListeEssaiForageDb As DbSet(Of Essai_Forage)
        Public Property ListeMesureEssaiForageDb As DbSet(Of MesureEssaiForage)
        Public Property ListeEssaiResistancesDb As DbSet(Of EssaiResistance)
        Public Property ListeMesureResistanceDb As DbSet(Of MesureResistance)
        Public Property ListeEssaiExpansionsDb As DbSet(Of EssaiExpansion)
        Public Property ListeMesureExpansionsDb As DbSet(Of MesureExpansion)
        Public Property ListeSolDb As DbSet(Of Sol)
        Public Property ListeSondeDb As DbSet(Of Probe)

        '---------------------------------------------------------------------------------------------------------------------------------
        '  Mapping des entités variable associées aux tables de la base de donnée
        '  Seules les clefs sont définies ici
        '  les autres propriétés ou variables ou champs sont définies comme identiques aux colonnes de la table de BD correspondante
        ' ---------------------------------------------------------------------------------------------------------------------------------
        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)

            modelBuilder.Entity(Of MesureExpansion)().
                ToTable("LesMesuresExpansionPropre").
                HasKey(Function(m) m.Ind)

            modelBuilder.Entity(Of MesureResistance)().
                ToTable("LesMesuresResistancePropre").
                HasKey(Function(m) m.Ind)

            ' Conservez ici le reste de votre configuration existante.

            modelBuilder.Entity(Of Chantier)().
                ToTable("LesChantiers").
                HasKey(Function(c) c.refChantier)

            modelBuilder.Entity(Of Forage)().
                ToTable("LesForages").
                HasKey(Function(f) f.RefForage)

            modelBuilder.Entity(Of Probe)().
                ToTable("LesSondes").
                HasKey(Function(p) p.RefSonde)

            modelBuilder.Entity(Of Sol)().
                ToTable("LesSols").
                HasKey(Function(s) s.RefSol)

            modelBuilder.Entity(Of EssaiExpansion)().
                ToTable("LesEssaiExpansion").
                HasKey(Function(e) e.RefEssai)

            modelBuilder.Entity(Of EssaiResistance)().
                ToTable("LesEssaisResistance").
                HasKey(Function(e) e.RefEssai)

            modelBuilder.Entity(Of Essai_Forage)().
                HasKey(Function(e) e.refEssai)

            modelBuilder.Entity(Of MesureEssaiForage)().
                HasKey(Function(m) m.Ind)

            modelBuilder.Entity(Of MesureResistance)().
                HasKey(Function(m) m.Ind)

            modelBuilder.Entity(Of MesureExpansion)().
                HasKey(Function(m) m.Ind)

        End Sub
    End Class

End Namespace