using ForageModel = GaiaPressCoreV17.Models.Forage;

namespace GaiaPressBlazorWebAPP.Services
{
    public interface IForageService
    {
        Task<ForageModel?> GetForageAsync(string refForage);

        Task<List<ForageModel>> GetAllForagesAsync();

        Task SaveForageAsync(ForageModel forage);

        Task DeleteForageAsync(string refForage);

    }
}