using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17_Sharp.Services.Implementation;

public class ProbeService : IProbeService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public ProbeService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProbeDto>> GetAllAsync()
    {
        var probes = await _context.Probes
            .Include(p => p.MesuresEtalonnage)
            .OrderBy(p => p.Nom)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ProbeDto>>(probes);
    }

    public async Task<ProbeDto?> GetByIdAsync(int id)
    {
        var probe = await _context.Probes
            .Include(p => p.MesuresEtalonnage.OrderByDescending(m => m.DateEtalonnage))
            .FirstOrDefaultAsync(p => p.Id == id);
        return probe == null ? null : _mapper.Map<ProbeDto>(probe);
    }

    public async Task<ProbeDto?> GetByNumeroSerieAsync(string numeroSerie)
    {
        var probe = await _context.Probes
            .Include(p => p.MesuresEtalonnage)
            .FirstOrDefaultAsync(p => p.NumeroSerie == numeroSerie);
        return probe == null ? null : _mapper.Map<ProbeDto>(probe);
    }

    public async Task<ProbeDto> CreateAsync(ProbeCreateDto dto)
    {
        if (await _context.Probes.AnyAsync(p => p.NumeroSerie == dto.NumeroSerie))
            throw new ArgumentException($"Une sonde avec le numéro de série '{dto.NumeroSerie}' existe déjà.");

        var probe = _mapper.Map<Probe>(dto);
        _context.Probes.Add(probe);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProbeDto>(probe);
    }

    public async Task<ProbeDto?> UpdateAsync(int id, ProbeUpdateDto dto)
    {
        var probe = await _context.Probes.FindAsync(id);
        if (probe == null) return null;

        if (dto.NumeroSerie != probe.NumeroSerie &&
            await _context.Probes.AnyAsync(p => p.NumeroSerie == dto.NumeroSerie))
            throw new ArgumentException($"Une sonde avec le numéro de série '{dto.NumeroSerie}' existe déjà.");

        _mapper.Map(dto, probe);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var probe = await _context.Probes
            .Include(p => p.MesuresEtalonnage)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (probe == null) return false;

        // Vérifier si la sonde est utilisée dans des essais
        var estUtilisee = await _context.EssaisExpansion.AnyAsync(e => e.ProbeId == id);
        if (estUtilisee)
            throw new InvalidOperationException(
                "Impossible de supprimer une sonde utilisée dans des essais d'expansion.");

        _context.Probes.Remove(probe);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MesuresEtalonnageDto>> GetEtalonnagesAsync(int probeId)
    {
        var etalonnages = await _context.MesuresEtalonnage
            .Where(m => m.ProbeId == probeId)
            .OrderByDescending(m => m.DateEtalonnage)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MesuresEtalonnageDto>>(etalonnages);
    }

    public async Task<MesuresEtalonnageDto> AddEtalonnageAsync(int probeId, MesuresEtalonnageCreateDto dto)
    {
        var probeExists = await _context.Probes.AnyAsync(p => p.Id == probeId);
        if (!probeExists)
            throw new ArgumentException($"Sonde avec l'ID {probeId} introuvable.");

        var etalonnage = _mapper.Map<MesuresEtalonnage>(dto);
        etalonnage.ProbeId = probeId;

        _context.MesuresEtalonnage.Add(etalonnage);
        await _context.SaveChangesAsync();

        return _mapper.Map<MesuresEtalonnageDto>(etalonnage);
    }
}