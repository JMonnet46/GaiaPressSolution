using GaiaPressCoreV17.Models;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressBlazorWebAPP.Data
{
    public class GaiaPressBlazorDbContext : DbContext
    {
        public GaiaPressBlazorDbContext(
            DbContextOptions<GaiaPressBlazorDbContext> options)
            : base(options)
        {
        }

        public DbSet<ForageEntity> ForageDb { get; set; }
        public DbSet<ConstructionSite> ConstructionSites { get; set; }
        public DbSet<Essai_Forage> ListeEssaiForageDb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Essai_Forage>(entity =>
            {
                entity.ToTable("ListeEssaiForageDb", "dbo");
                entity.HasKey(e => e.RefEssai);
            });
        }
    }
}
