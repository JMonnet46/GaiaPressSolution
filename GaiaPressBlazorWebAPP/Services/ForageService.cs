using GaiaPressCoreV17.Data;
using Microsoft.EntityFrameworkCore;
using ForageModel = GaiaPressCoreV17.Models.Forage;

namespace GaiaPressBlazorWebAPP.Services
{
    public class ForageService : IForageService
    {
        private readonly ConnectionService _connectionService;

        public ForageService(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        // Crée un nouveau DbContext avec la chaîne de connexion de l'utilisateur connecté
        private GaiaPress_Db_Context CreateContext()
        {
            if (!_connectionService.IsConnected || string.IsNullOrEmpty(_connectionService.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Non connecté à la base de données. " +
                    "Veuillez vous connecter via le menu ConstructionSite > Se connecter à la Bd.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<GaiaPress_Db_Context>();
            optionsBuilder.UseSqlServer(_connectionService.ConnectionString);

            return new GaiaPress_Db_Context(optionsBuilder.Options);
        }

        public async Task<ForageModel?> GetForageAsync(string refForage)
        {
            using var context = CreateContext();
            return await context.ListeForageDb
                .FirstOrDefaultAsync(f => f.RefForage == refForage);
        }

        public async Task<List<ForageModel>> GetAllForagesAsync()
        {
            using var context = CreateContext();
            return await context.ListeForageDb.ToListAsync();
        }

        public async Task SaveForageAsync(ForageModel forage)
        {
            using var context = CreateContext();

            var existing = await context.ListeForageDb
                .FirstOrDefaultAsync(f => f.RefForage == forage.RefForage);

            if (existing == null)
            {
                context.ListeForageDb.Add(forage);
            }
            else
            {
                context.Entry(existing).CurrentValues.SetValues(forage);
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteForageAsync(string refForage)
        {
            using var context = CreateContext();

            var forage = await context.ListeForageDb
                .FirstOrDefaultAsync(f => f.RefForage == refForage);

            if (forage != null)
            {
                context.ListeForageDb.Remove(forage);
                await context.SaveChangesAsync();
            }
        }
    }
}