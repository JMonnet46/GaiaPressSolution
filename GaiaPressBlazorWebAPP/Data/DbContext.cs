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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // RIEN d'autre pour Forage - les attributs gèrent tout
        }
    }
}