
// GaiaPressDbContext.cs mis à jour (multi-provider)
using GaiaPressCoreV17.Models;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17.Data
{
    public class GaiaPressDbContext : DbContext
    {
        // ========== DbSets ==========
        public DbSet<Chantier> LesChantiers { get; set; }
        public DbSet<Forage> LesForages { get; set; }
        public DbSet<Sol> LesSols { get; set; }
        public DbSet<Probe> LesSondes { get; set; }
        public DbSet<EssaiForage> LesEssaisForage { get; set; }
        public DbSet<EssaiResistance> LesEssaisResistance { get; set; }
        public DbSet<EssaiExpansion> LesEssaisExpansion { get; set; }
        public DbSet<EssaiPerteDeCharge> LesEssaisPerteDeCharge { get; set; }
        public DbSet<MesureEssaiForage> LesMesuresEssaiForage { get; set; }
        public DbSet<MesureResistance> LesMesuresResistance { get; set; }
        public DbSet<MesureExpansion> LesMesuresExpansion { get; set; }
        public DbSet<MesurePerteDeCharge> LesMesuresPerteDeCharge { get; set; }

        // ========== Constructeurs ==========

        /// <summary>
        /// Constructeur pour l'injection de dépendances (recommandé pour le web)
        /// </summary>
        public GaiaPressDbContext(DbContextOptions<GaiaPressDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Constructeur sans paramètre pour la migration depuis Access (legacy)
        /// </summary>
        public GaiaPressDbContext()
        {
        }

        // ========== Configuration de la connexion ==========

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ⚠️ CONFIGURATION PAR DÉFAUT : SQL Server (base cloud cible)
                // À remplacer par la chaîne de connexion réelle en production
                var connectionString = "Server=tcp:gaia-press.database.windows.net,1433;" +
                                       "Initial Catalog=GaiaPressDB;" +
                                       "Persist Security Info=False;" +
                                       "User ID=gaia_admin;" +
                                       "Password={your_password};" +
                                       "MultipleActiveResultSets=False;" +
                                       "Encrypt=True;" +
                                       "TrustServerCertificate=False;" +
                                       "Connection Timeout=30;";

                optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                });
            }
        }

        // ========== Configuration du modèle ==========

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureChantier(modelBuilder);
            ConfigureForage(modelBuilder);
            ConfigureSol(modelBuilder);
            ConfigureProbe(modelBuilder);
            ConfigureEssaiForage(modelBuilder);
            ConfigureEssaiResistance(modelBuilder);
            ConfigureEssaiExpansion(modelBuilder);
            ConfigureEssaiPerteDeCharge(modelBuilder);
            ConfigureMesures(modelBuilder);
        }

        // ========== Configuration par entité ==========

        private void ConfigureChantier(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chantier>(entity =>
            {
                entity.ToTable("LesChantiers");
                entity.HasKey(e => e.RefChantier);

                entity.Property(e => e.RefChantier)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.NomChantier)
                    .HasMaxLength(200);

                entity.Property(e => e.Adresse)
                    .HasMaxLength(500);

                // Relation 1-N avec Forage
                entity.HasMany(e => e.ListeForages)
                    .WithOne(f => f.Chantier)
                    .HasForeignKey(f => f.RefChantier)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureForage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Forage>(entity =>
            {
                entity.ToTable("LesForages");
                entity.HasKey(e => e.RefForage);

                entity.Property(e => e.RefForage)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.NomForage)
                    .HasMaxLength(200);

                // Relation N-1 avec Chantier
                entity.HasOne(e => e.Chantier)
                    .WithMany(c => c.ListeForages)
                    .HasForeignKey(e => e.RefChantier)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relation N-1 avec Sol
                entity.HasOne(e => e.Sol)
                    .WithMany(s => s.ListeForages)
                    .HasForeignKey(e => e.RefSol)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relation 1-N avec EssaiForage
                entity.HasMany(e => e.ListeEssais)
                    .WithOne(ee => ee.Forage)
                    .HasForeignKey(ee => ee.RefForage)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureSol(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sol>(entity =>
            {
                entity.ToTable("LesSols");
                entity.HasKey(e => e.RefSol);

                entity.Property(e => e.RefSol)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.NomSol)
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
            });
        }

        private void ConfigureProbe(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Probe>(entity =>
            {
                entity.ToTable("LesSondes");
                entity.HasKey(e => e.RefSonde);

                entity.Property(e => e.RefSonde)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.NomSonde)
                    .HasMaxLength(200);

                // Propriétés numériques avec précision
                entity.Property(e => e.Ls).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Lg).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Lt).HasColumnType("decimal(18,4)");
                entity.Property(e => e.LongInjectPompage).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Vs).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Ds).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Dcalib).HasColumnType("decimal(18,4)").HasDefaultValue(65m);
                entity.Property(e => e.EpMembraneGarde).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrEpMembraneGarde).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EpMembraneMesure).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrEpMembraneMesure).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EpTubeFendu).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrEpTubeFendu).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EpaisInitMembrane).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrEpaisInitMembrane).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrLong).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrDiam).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrP).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrV).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrPV).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrZ).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrT).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Pm).HasColumnType("decimal(18,4)");

                // Relations 1-N avec les essais
                entity.HasMany(e => e.Resistance)
                    .WithOne(er => er.MySonde)
                    .HasForeignKey(er => er.RefSonde)
                    .HasPrincipalKey(e => e.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Expansion)
                    .WithOne(ee => ee.MySonde)
                    .HasForeignKey(ee => ee.RefSonde)
                    .HasPrincipalKey(e => e.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.PerteDeCharge)
                    .WithOne(ep => ep.MySonde)
                    .HasForeignKey(ep => ep.RefSonde)
                    .HasPrincipalKey(e => e.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureEssaiForage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EssaiForage>(entity =>
            {
                entity.ToTable("LesEssaisForage");
                entity.HasKey(e => e.RefEssai);

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.RefForage)
                    .HasMaxLength(50);

                entity.Property(e => e.NomEssai)
                    .HasMaxLength(200);

                // Relation N-1 avec Forage
                entity.HasOne(e => e.Forage)
                    .WithMany(f => f.ListeEssais)
                    .HasForeignKey(e => e.RefForage)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation 1-N avec MesureEssaiForage
                entity.HasMany(e => e.Mesures)
                    .WithOne(m => m.Essai)
                    .HasForeignKey(m => m.RefEssai)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureEssaiResistance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EssaiResistance>(entity =>
            {
                entity.ToTable("LesEssaisResistanceDbPropre");
                entity.HasKey(e => e.RefEssai);

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.RefSonde)
                    .HasMaxLength(50);

                entity.Property(e => e.NomEssai)
                    .HasMaxLength(200);

                entity.Property(e => e.DateEssai)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DifPresEauAir).HasColumnType("decimal(18,4)");
                entity.Property(e => e.HauteurCpv).HasColumnType("decimal(18,4)");
                entity.Property(e => e.A0).HasColumnType("decimal(18,4)");
                entity.Property(e => e.A1).HasColumnType("decimal(18,4)");
                entity.Property(e => e.A2).HasColumnType("decimal(18,4)");

                // Relation N-1 avec Probe
                entity.HasOne(e => e.MySonde)
                    .WithMany(p => p.Resistance)
                    .HasForeignKey(e => e.RefSonde)
                    .HasPrincipalKey(p => p.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation 1-N avec MesureResistance
                entity.HasMany(e => e.MesuresResistanceDb)
                    .WithOne(m => m.EssaiResistance)
                    .HasForeignKey(m => m.RefEssai)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureEssaiExpansion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EssaiExpansion>(entity =>
            {
                entity.ToTable("LesEssaisExpansionPropre");
                entity.HasKey(e => e.RefEssai);

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.RefSonde)
                    .HasMaxLength(50);

                entity.Property(e => e.NomEssai)
                    .HasMaxLength(200);

                entity.Property(e => e.DateEssai)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DifPresEauAir).HasColumnType("decimal(18,4)");
                entity.Property(e => e.HauteurCpv).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Di).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Vc).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Vs).HasColumnType("decimal(18,4)");
                entity.Property(e => e.EpMembrane).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Prp).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrV).HasColumnType("decimal(18,4)");
                entity.Property(e => e.CoefficientA).HasColumnType("decimal(18,4)");
                entity.Property(e => e.CoefficientB).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrecisVc).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrecisVs).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrEpMembrane).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrecisA).HasColumnType("decimal(18,4)");
                entity.Property(e => e.PrecisB).HasColumnType("decimal(18,4)");

                // Relation N-1 avec Probe
                entity.HasOne(e => e.MySonde)
                    .WithMany(p => p.Expansion)
                    .HasForeignKey(e => e.RefSonde)
                    .HasPrincipalKey(p => p.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation 1-N avec MesureExpansion
                entity.HasMany(e => e.MesuresExpansionDb)
                    .WithOne(m => m.EssaiExpansion)
                    .HasForeignKey(m => m.RefEssai)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureEssaiPerteDeCharge(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EssaiPerteDeCharge>(entity =>
            {
                entity.ToTable("LesEssaisPerteDeCharge");
                entity.HasKey(e => e.RefEssai);

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.RefSonde)
                    .HasMaxLength(50);

                entity.Property(e => e.NomEssai)
                    .HasMaxLength(200);

                entity.Property(e => e.DateEssai)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DifPresEauAir).HasColumnType("decimal(18,4)");
                entity.Property(e => e.HauteurCpv).HasColumnType("decimal(18,4)");
                entity.Property(e => e.B0).HasColumnType("decimal(18,4)");
                entity.Property(e => e.B1).HasColumnType("decimal(18,4)");
                entity.Property(e => e.B2).HasColumnType("decimal(18,4)");
                entity.Property(e => e.B3).HasColumnType("decimal(18,4)");

                // Relation N-1 avec Probe
                entity.HasOne(e => e.MySonde)
                    .WithMany(p => p.PerteDeCharge)
                    .HasForeignKey(e => e.RefSonde)
                    .HasPrincipalKey(p => p.RefSonde)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation 1-N avec MesurePerteDeCharge
                entity.HasMany(e => e.MesuresPerteDeCharge)
                    .WithOne(m => m.EssaiPerteDeCharge)
                    .HasForeignKey(m => m.RefEssai)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureMesures(ModelBuilder modelBuilder)
        {
            // --- MesureEssaiForage ---
            modelBuilder.Entity<MesureEssaiForage>(entity =>
            {
                entity.ToTable("LesMesuresEssaiForage");
                entity.HasKey(e => e.Ind);

                entity.Property(e => e.Ind)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50);

                entity.Property(e => e.IndiceMesure);
                entity.Property(e => e.Pression).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,4)");
            });

            // --- MesureResistance ---
            modelBuilder.Entity<MesureResistance>(entity =>
            {
                entity.ToTable("LesMesuresResistanceDbPropre");
                entity.HasKey(e => e.Ind);

                entity.Property(e => e.Ind)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50);

                entity.Property(e => e.IndiceMesure);
                entity.Property(e => e.Pression).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,4)");
            });

            // --- MesureExpansion ---
            modelBuilder.Entity<MesureExpansion>(entity =>
            {
                entity.ToTable("LesMesuresExpansionPropre");
                entity.HasKey(e => e.Ind);

                entity.Property(e => e.Ind)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50);

                entity.Property(e => e.IndiceMesure);
                entity.Property(e => e.Pression).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,4)");
            });

            // --- MesurePerteDeCharge ---
            modelBuilder.Entity<MesurePerteDeCharge>(entity =>
            {
                entity.ToTable("LesMesuresPerteDeCharge");
                entity.HasKey(e => e.Ind);

                entity.Property(e => e.Ind)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.RefEssai)
                    .HasMaxLength(50);

                entity.Property(e => e.IndiceMesure);
                entity.Property(e => e.DebitLitreMin).HasColumnType("decimal(18,4)");
                entity.Property(e => e.DebitLitreHeure).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Pi).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Pp).HasColumnType("decimal(18,4)");
            });
        }
    }
}
