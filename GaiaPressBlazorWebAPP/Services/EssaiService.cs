using Microsoft.EntityFrameworkCore;
using EssaiModel = GaiaPressCoreV17.Models.Essai_Forage;
using GaiaPressCoreV17.Data;   // adapter au namespace de votre DbContext

namespace GaiaPressBlazorWebAPP.Services
{
    public class EssaiService : IEssaiService
    {
        private readonly GaiaPress_Db_Context _db;   // adapter au nom de votre DbContext

        public EssaiService(GaiaPress_Db_Context db)
        {
            _db = db;
        }

        public async Task<string> GenerateNewRefEssaiAsync(string refForage)
        {
            var existants = await _db.ListeEssaiForageDb
                .Where(e => e.RefForage == refForage)
                .Select(e => e.RefEssai)
                .ToListAsync();

            int k = 1;
            while (existants.Contains($"{refForage}_{k}")) k++;
            return $"{refForage}_{k}";
        }

        public async Task<EssaiModel?> GetEssaiAsync(string refEssai)
        {
            return await _db.ListeEssaiForageDb.FirstOrDefaultAsync(e => e.RefEssai == refEssai);
        }

        public async Task<List<EssaiModel>> GetEssaisByForageAsync(string refForage)
        {
            return await _db.ListeEssaiForageDb
                .Where(e => e.RefForage == refForage)
                .ToListAsync();
        }

        public async Task<List<EssaiModel>> GetEssaisByChantierAsync(string refChantier)
        {
            return await _db.ListeEssaiForageDb
                .Where(e => e.RefChantier == refChantier)
                .ToListAsync();
        }

        public async Task SaveEssaiAsync(EssaiModel essai)
        {
            var existant = await _db.ListeEssaiForageDb.FirstOrDefaultAsync(e => e.RefEssai == essai.RefEssai);
            if (existant == null)
                _db.ListeEssaiForageDb.Add(essai);
            else
                _db.Entry(existant).CurrentValues.SetValues(essai);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteEssaiAsync(string refEssai)
        {
            var essai = await _db.ListeEssaiForageDb.FirstOrDefaultAsync(e => e.RefEssai == refEssai);
            if (essai != null)
            {
                _db.ListeEssaiForageDb.Remove(essai);
                await _db.SaveChangesAsync();
            }
        }
    }
}