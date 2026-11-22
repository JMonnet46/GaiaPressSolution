Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Models
Imports GaiaPressBlazorWebAPP.Models

Namespace GaiaPressCoreV17.Data

    Public Class GaiaPressDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub

        Public Property ChantiersDb As DbSet(Of Chantier)

        Public Property ListeForageDb As DbSet(Of GaiaPressCoreV17.Models.Forage)
        Public Property ListeEssaiForageDb As DbSet(Of Essai_Forage)
        Public Property ListeMesureEssaiForageDb As DbSet(Of MesureEssaiForage)
        Public Property ListeEssaiResistancesDb As DbSet(Of EssaiResistance)
        Public Property ListeMesureResistanceDb As DbSet(Of MesureResistance)
        Public Property ListeEssaiExpansionsDb As DbSet(Of EssaiExpansion)
        Public Property ListeMesureExpansionsDb As DbSet(Of MesureExpansion)
        Public Property ListeSondeDb As DbSet(Of Probe)
        Public Property ListeSolDb As DbSet(Of Sol)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)


            modelBuilder.Entity(Of Chantier)(Sub(entity)
                                                 entity.ToTable("LesChantiersDb", "dbo")

                                                 entity.HasKey(Function(c) c.RefChantier)

                                                 ' Clé primaire
                                                 entity.Property(Function(c) c.RefChantier).
                                                             HasColumnName("RefChantier")

                                                 ' Colonnes
                                                 entity.Property(Function(c) c.NomChantier).
                                                             HasColumnName("NomChantier")

                                                 entity.Property(Function(c) c.DescriptifChantier).
                                                             HasColumnName("DescriptifChantier")

                                                 entity.Property(Function(c) c.DateChantier).
                                                             HasColumnName("DateChantier")

                                                 entity.Property(Function(c) c.Reference).
                                                             HasColumnName("Reference").IsRequired().HasMaxLength(50)

                                                 entity.Property(Function(c) c.RefClient).
                                                             HasColumnName("RefClient").HasMaxLength(50)

                                                 entity.Property(Function(c) c.Entreprise).
                                                             HasColumnName("Entreprise").HasMaxLength(200)

                                                 entity.Property(Function(c) c.Adresse).
                                                             HasColumnName("Adresse").HasMaxLength(500)

                                                 entity.Property(Function(c) c.Telephone).
                                                             HasColumnName("Telephone").HasMaxLength(20)

                                             End Sub)

            ' Configuration de l'entité Forage
            modelBuilder.Entity(Of Forage)(Sub(entity)
                                               entity.ToTable("LesForagesDb", "dbo")

                                               ' Clé primaire
                                               entity.HasKey(Function(e) e.RefForage)
                                               entity.Property(Function(e) e.RefForage).
                                                    HasColumnName("refForage")

                                               ' CORRECTION : mapper la propriété VB vers la vraie colonne SQL
                                               ' Clé étrangère vers Chantier
                                               entity.Property(Function(f) f.RefChantier).
                                                    HasColumnName("refChantier").IsRequired()

                                               ' Définir explicitement la relation pour éviter la colonne fantôme
                                               entity.Property(Function(f) f.numeroForage).
                                                    HasColumnName("numeroForage").IsRequired()
                                               entity.Property(Function(f) f.NomForage).
                                                    HasColumnName("nomForage").HasMaxLength(100)
                                               entity.Property(Function(f) f.dateForage).
                                                    HasColumnName("dateForage")
                                               entity.Property(Function(f) f.materiel).
                                                    HasColumnName("materiel").HasMaxLength(100)
                                               entity.Property(Function(f) f.outil).
                                                    HasColumnName("outil").HasMaxLength(100)
                                               entity.Property(Function(f) f.abscisseX).
                                                    HasColumnName("abscisseX")
                                               entity.Property(Function(f) f.ordonneeY).
                                                    HasColumnName("ordonneeY")
                                               entity.Property(Function(f) f.altitudeZ).
                                                    HasColumnName("altitudeZ")
                                               entity.Property(Function(f) f.coteInit).
                                                    HasColumnName("coteinit")
                                               entity.Property(Function(f) f.coteFinale).
                                                    HasColumnName("coteFinale")

                                               ' Relation de navigation
                                               entity.HasOne(Function(f) f.Chantier).
                                                      WithMany(Function(c) c.ListForages).
                                                      HasForeignKey(Function(f) f.RefChantier)

                                               entity.Ignore(Function(c) c.EventsSuspendus)
                                               entity.Ignore(Function(c) c.FichierChantier)
                                               entity.Ignore(Function(c) c.IsSaved)

                                           End Sub)

            modelBuilder.Entity(Of Essai_Forage)(Sub(entity)
                                                     entity.ToTable("ListeListeEssaiForageDb", "dbo")
                                                     entity.HasKey(Function(e) e.RefEssai)

                                                     entity.HasOne(Of Forage)().
                                                    WithMany(Function(f) f.ListEssais).
                                                    HasForeignKey(Function(e) e.RefForage).
                                                    OnDelete(DeleteBehavior.Restrict)

                                                     entity.Ignore(Function(e) e.EventsSuspendus)
                                                     entity.Ignore(Function(e) e.IsSaved)
                                                 End Sub)

            modelBuilder.Entity(Of MesureEssaiForage)(Sub(entity)
                                                          entity.ToTable("ListeMesureEssaiForageDb", "dbo")
                                                      End Sub)

            modelBuilder.Entity(Of EssaiResistance)().
                HasKey(Function(e) e.RefEssai)


            modelBuilder.Entity(Of EssaiExpansion)().
                HasKey(Function(e) e.RefEssai)

            modelBuilder.Entity(Of Probe)().
                HasKey(Function(p) p.RefSonde)

            modelBuilder.Entity(Of Sol)().
                HasKey(Function(s) s.RefSol)

        End Sub

    End Class

End Namespace