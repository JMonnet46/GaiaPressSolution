Imports Microsoft.EntityFrameworkCore

Namespace Data
    Public Class GaiaPressDbContext
        Inherits DbContext

        ' 3/ DbSet pour chaque table de votre base de données
        Public Property LesChantiers As DbSet(Of LesChantiers)
        Public Property LesForages As DbSet(Of LesForages)
        Public Property LesSols As DbSet(Of LesSols)
        Public Property LesSondes As DbSet(Of LesSondes)
        Public Property LesEssaisForage As DbSet(Of LesEssaisForage)
        Public Property LesMesuresForage As DbSet(Of LesMesuresForage)
        Public Property LesEssaisExpansionPropre As DbSet(Of LesEssaisExpansionPropre)
        Public Property LesEssaisResistancePropre As DbSet(Of LesEssaisResistancePropre)
        Public Property LesEssaisPertedeCharge As DbSet(Of LesEssaisPertedeCharge)
        Public Property LesMesuresResistancePropre As DbSet(Of LesMesuresResistancePropre)
        Public Property LesMesuresExpansionPropre As DbSet(Of LesMesuresExpansionPropre)
        Public Property LesMesuresPertedeCharge As DbSet(Of LesMesuresPertedeCharge)

        ' Constructeur pour configurer le contexte avec les options de connexion
        Public Sub New(options As DbContextOptions(Of GaiaPressDbContext))
            MyBase.New(options)
        End Sub

        ' Méthode pour configurer les relations entre les tables (optionnel)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            ' Exemple : Configurer une relation entre LesForages et LesChantiers
            modelBuilder.Entity(Of LesForages)().HasOne(Of LesChantiers)().WithMany().HasForeignKey(Function(f) f.refChantier)

            ' Ajoutez ici les autres relations entre vos tables
            modelBuilder.Entity(Of LesEssaisForage)().HasOne(Of LesForages)().WithMany().HasForeignKey(Function(e) e.refForage)
            modelBuilder.Entity(Of LesEssaisForage)().HasOne(Of LesSols)().WithMany().HasForeignKey(Function(e) e.refSol)
            modelBuilder.Entity(Of LesMesuresForage)().HasOne(Of LesEssaisForage)().WithMany().HasForeignKey(Function(m) m.refEssai)

            ' Relations pour les tables liées aux sondes
            modelBuilder.Entity(Of LesEssaisResistancePropre)().HasOne(Of LesSondes)().WithMany().HasForeignKey(Function(e) e.refSonde)
            modelBuilder.Entity(Of LesEssaisExpansionPropre)().HasOne(Of LesSondes)().WithMany().HasForeignKey(Function(e) e.refSonde)
            modelBuilder.Entity(Of LesEssaisPertedeCharge)().HasOne(Of LesSondes)().WithMany().HasForeignKey(Function(e) e.refSonde)

            modelBuilder.Entity(Of LesMesuresResistancePropre)().HasOne(Of LesEssaisResistancePropre)().WithMany().HasForeignKey(Function(m) m.refEssai)
            modelBuilder.Entity(Of LesMesuresExpansionPropre)().HasOne(Of LesEssaisExpansionPropre)().WithMany().HasForeignKey(Function(m) m.refEssai)
            modelBuilder.Entity(Of LesMesuresPertedeCharge)().HasOne(Of LesEssaisPertedeCharge)().WithMany().HasForeignKey(Function(m) m.refEssai)
        End Sub
    End Class
End Namespace