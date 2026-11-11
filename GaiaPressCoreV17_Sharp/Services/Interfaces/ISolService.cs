using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface ISolService
{
    Task<IEnumerable<SolDto>> GetAllAsync();
    Task<SolDto?> GetByIdAsync(int id);
    Task<SolDto?> GetByCodeAsync(string code);
    Task<SolDto> CreateAsync(SolCreateDto dto);
    Task<SolDto?> UpdateAsync(int id, SolUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}