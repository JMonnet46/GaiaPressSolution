using EssaiModel = GaiaPressCoreV17.Models.Essai_Forage;

namespace GaiaPressBlazorWebAPP.Services
{
    public interface IEssaiService
    {
        Task<EssaiModel?> GetEssaiAsync(string refEssai);
        Task<List<EssaiModel>> GetEssaisByForageAsync(string refForage);
        Task<List<EssaiModel>> GetEssaisByChantierAsync(string refChantier);
        Task<string> GenerateNewRefEssaiAsync(string refForage);
        Task SaveEssaiAsync(EssaiModel essai);
        Task DeleteEssaiAsync(string refEssai);
    }
}