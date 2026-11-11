using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface IEssaiExpansionService
{
    Task<IEnumerable<EssaiExpansionDto>> GetAllAsync();
    Task<EssaiExpansionDto?> GetByIdAsync(int id);
    Task<IEnumerable<EssaiExpansionDto>> GetByForageAsync(int forageId);
    Task<EssaiExpansionDto> CreateAsync(EssaiExpansionCreateDto dto);
    Task<EssaiExpansionDto?> UpdateAsync(int id, EssaiExpansionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MesureExpansionDto>> GetMesuresAsync(int essaiId);
    Task<MesureExpansionDto> AddMesureAsync(int essaiId, MesureExpansionCreateDto dto);
    Task<double?> CalculerModulePressiometriqueAsync(int essaiId);
}