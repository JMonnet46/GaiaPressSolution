using GaiaPressCoreV17.Models.Dto;

namespace GaiaPressCoreV17.Services.Interfaces;

public interface IChantierService
{
    Task<IEnumerable<ChantierDto>> GetAllAsync();
    Task<ChantierDto?> GetByIdAsync(int id);
    Task<ChantierDto?> GetByReferenceAsync(string reference);
    Task<ChantierDto> CreateAsync(ChantierCreateDto dto);
    Task<ChantierDto?> UpdateAsync(int id, ChantierUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ForageDto>> GetForagesAsync(int chantierId);
}