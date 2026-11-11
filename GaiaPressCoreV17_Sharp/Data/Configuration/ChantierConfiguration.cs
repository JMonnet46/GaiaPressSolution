using GaiaPressCoreV17.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaiaPressCoreV17.Data.Configurations;

public class ChantierConfiguration : IEntityTypeConfiguration<Chantier>
{
    public void Configure(EntityTypeBuilder<Chantier> builder)
    {
        builder.ToTable("Chantiers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Reference)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Reference)
            .IsUnique();

        builder.Property(c => c.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Localisation)
            .HasMaxLength(500);

        builder.Property(c => c.DateCreation)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(c => c.Forages)
            .WithOne(f => f.Chantier)
            .HasForeignKey(f => f.ChantierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}