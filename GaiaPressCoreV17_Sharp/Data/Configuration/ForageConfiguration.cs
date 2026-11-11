using GaiaPressCoreV17.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaiaPressCoreV17.Data.Configurations;

public class ForageConfiguration : IEntityTypeConfiguration<Forage>
{
    public void Configure(EntityTypeBuilder<Forage> builder)
    {
        builder.ToTable("Forages");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Nom)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ProfondeurPrevue)
            .HasPrecision(10, 2);

        builder.Property(f => f.ProfondeurAtteinte)
            .HasPrecision(10, 2);

        builder.HasOne(f => f.Chantier)
            .WithMany(c => c.Forages)
            .HasForeignKey(f => f.ChantierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Sol)
            .WithMany(s => s.Forages)
            .HasForeignKey(f => f.SolId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(f => f.EssaisForage)
            .WithOne(e => e.Forage)
            .HasForeignKey(e => e.ForageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.EssaisExpansion)
            .WithOne(e => e.Forage)
            .HasForeignKey(e => e.ForageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.EssaisResistance)
            .WithOne(e => e.Forage)
            .HasForeignKey(e => e.ForageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}