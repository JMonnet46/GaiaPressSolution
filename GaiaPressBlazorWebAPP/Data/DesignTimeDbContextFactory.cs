using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GaiaPressBlazorWebAPP.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GaiaPressBlazorDbContext>
    {
        public GaiaPressBlazorDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GaiaPressBlazorDbContext>();

            // Chaîne de connexion locale (adapte selon ta config)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=GaiaPressDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
            );

            return new GaiaPressBlazorDbContext(optionsBuilder.Options);
        }
    }
}

