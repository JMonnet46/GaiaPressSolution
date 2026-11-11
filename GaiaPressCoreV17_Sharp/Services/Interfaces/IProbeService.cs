
namespace GaiaPressCoreV17.Services.Interfaces;

public interface IProbeService
{
    Task<IEnumerable<ProbeDto>> GetAllAsync();
    Task<ProbeDto?> GetByIdAsync(int id);
    Task<ProbeDto?> GetByNumeroSerieAsync(string numeroSerie);
    Task<ProbeDto> CreateAsync(ProbeCreateDto dto);
    Task<ProbeDto?> UpdateAsync(int id, ProbeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MesuresEtalonnageDto>> GetEtalonnagesAsync(int probeId);
    Task<MesuresEtalonnageDto> AddEtalonnageAsync(int probeId, MesuresEtalonnageCreateDto dto);
}