using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface IEssaiForageService
{
    Task<IEnumerable<EssaiForageDto>> GetAllAsync();
    Task<EssaiForageDto?> GetByIdAsync(int id);
    Task<IEnumerable<EssaiForageDto>> GetByForageAsync(int forageId);
    Task<EssaiForageDto> CreateAsync(EssaiForageCreateDto dto);
    Task<EssaiForageDto?> UpdateAsync(int id, EssaiForageUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MesureEssaiForageDto>> GetMesuresAsync(int essaiId);
    Task<MesureEssaiForageDto> AddMesureAsync(int essaiId, MesureEssaiForageCreateDto dto);
}