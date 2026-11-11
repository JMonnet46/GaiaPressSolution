using GaiaPressCoreV17.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaiaPressCoreV17.Data.Configurations;

public class EssaiExpansionConfiguration : IEntityTypeConfiguration<EssaiExpansion>
{
    public void Configure(EntityTypeBuilder<EssaiExpansion> builder)
    {
        builder.ToTable("EssaisExpansion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PressionLimite)
            .HasPrecision(18, 4);

        builder.Property(e => e.PressionFluage)
            .HasPrecision(18, 4);

        builder.HasOne(e => e.Forage)
            .WithMany(f => f.EssaisExpansion)
            .HasForeignKey(e => e.ForageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Probe)
            .WithMany(p => p.EssaisExpansion)
            .HasForeignKey(e => e.ProbeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Mesures)
            .WithOne(m => m.EssaiExpansion)
            .HasForeignKey(m => m.EssaiExpansionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}