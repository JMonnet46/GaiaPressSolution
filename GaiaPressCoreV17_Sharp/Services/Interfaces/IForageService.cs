using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface IForageService
{
    Task<IEnumerable<ForageDto>> GetAllAsync();
    Task<ForageDto?> GetByIdAsync(int id);
    Task<IEnumerable<ForageDto>> GetByChantierAsync(int chantierId);
    Task<ForageDto> CreateAsync(ForageCreateDto dto);
    Task<ForageDto?> UpdateAsync(int id, ForageUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EssaiForageDto>> GetEssaisForageAsync(int forageId);
    Task<IEnumerable<EssaiExpansionDto>> GetEssaisExpansionAsync(int forageId);
    Task<IEnumerable<EssaiResistanceDto>> GetEssaisResistanceAsync(int forageId);
}