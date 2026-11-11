using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface IEssaiResistanceService
{
    Task<IEnumerable<EssaiResistanceDto>> GetAllAsync();
    Task<EssaiResistanceDto?> GetByIdAsync(int id);
    Task<IEnumerable<EssaiResistanceDto>> GetByForageAsync(int forageId);
    Task<EssaiResistanceDto> CreateAsync(EssaiResistanceCreateDto dto);
    Task<EssaiResistanceDto?> UpdateAsync(int id, EssaiResistanceUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MesureResistanceDto>> GetMesuresAsync(int essaiId);
    Task<MesureResistanceDto> AddMesureAsync(int essaiId, MesureResistanceCreateDto dto);
}