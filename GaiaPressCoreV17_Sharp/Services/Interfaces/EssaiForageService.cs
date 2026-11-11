using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17.Services;

public class EssaiForageService : IEssaiForageService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public EssaiForageService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EssaiForageDto>> GetAllAsync()
    {
        var essais = await _context.EssaisForage
            .Include(e => e.Forage)
            .Include(e => e.Mesures)
            .OrderByDescending(e => e.DateEssai)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiForageDto>>(essais);
    }

    public async Task<EssaiForageDto?> GetByIdAsync(int id)
    {
        var essai = await _context.EssaisForage
            .Include(e => e.Forage)
            .Include(e => e.Mesures.OrderBy(m => m.Profondeur))
            .FirstOrDefaultAsync(e => e.Id == id);
        return essai == null ? null : _mapper.Map<EssaiForageDto>(essai);
    }

    public async Task<IEnumerable<EssaiForageDto>> GetByForageAsync(int forageId)
    {
        var essais = await _context.EssaisForage
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiForageDto>>(essais);
    }

    public async Task<EssaiForageDto> CreateAsync(EssaiForageCreateDto dto)
    {
        var forageExists = await _context.Forages.AnyAsync(f => f.Id == dto.ForageId);
        if (!forageExists)
            throw new ArgumentException($"Forage avec l'ID {dto.ForageId} introuvable.");

        var essai = _mapper.Map<EssaiForage>(dto);
        _context.EssaisForage.Add(essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(essai.Id)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'essai créé.");
    }

    public async Task<EssaiForageDto?> UpdateAsync(int id, EssaiForageUpdateDto dto)
    {
        var essai = await _context.EssaisForage.FindAsync(id);
        if (essai == null) return null;

        _mapper.Map(dto, essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var essai = await _context.EssaisForage
            .Include(e => e.Mesures)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (essai == null) return false;

        _context.EssaisForage.Remove(essai); // Cascade delete sur les mesures
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MesureEssaiForageDto>> GetMesuresAsync(int essaiId)
    {
        var mesures = await _context.MesuresEssaiForage
            .Where(m => m.EssaiForageId == essaiId)
            .OrderBy(m => m.Profondeur)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MesureEssaiForageDto>>(mesures);
    }

    public async Task<MesureEssaiForageDto> AddMesureAsync(int essaiId, MesureEssaiForageCreateDto dto)
    {
        var essaiExists = await _context.EssaisForage.AnyAsync(e => e.Id == essaiId);
        if (!essaiExists)
            throw new ArgumentException($"Essai de forage avec l'ID {essaiId} introuvable.");

        var mesure = _mapper.Map<MesureEssaiForage>(dto);
        mesure.EssaiForageId = essaiId;

        _context.MesuresEssaiForage.Add(mesure);
        await _context.SaveChangesAsync();

        return _mapper.Map<MesureEssaiForageDto>(mesure);
    }
}