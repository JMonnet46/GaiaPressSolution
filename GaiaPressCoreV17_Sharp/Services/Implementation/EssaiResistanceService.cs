using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17_Sharp.Services.Implementation;

public class EssaiResistanceService : IEssaiResistanceService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public EssaiResistanceService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EssaiResistanceDto>> GetAllAsync()
    {
        var essais = await _context.EssaisResistance
            .Include(e => e.Forage)
            .Include(e => e.Mesures)
            .OrderByDescending(e => e.DateEssai)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiResistanceDto>>(essais);
    }

    public async Task<EssaiResistanceDto?> GetByIdAsync(int id)
    {
        var essai = await _context.EssaisResistance
            .Include(e => e.Forage)
            .Include(e => e.Mesures.OrderBy(m => m.Profondeur))
            .FirstOrDefaultAsync(e => e.Id == id);
        return essai == null ? null : _mapper.Map<EssaiResistanceDto>(essai);
    }

    public async Task<IEnumerable<EssaiResistanceDto>> GetByForageAsync(int forageId)
    {
        var essais = await _context.EssaisResistance
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiResistanceDto>>(essais);
    }

    public async Task<EssaiResistanceDto> CreateAsync(EssaiResistanceCreateDto dto)
    {
        var forageExists = await _context.Forages.AnyAsync(f => f.Id == dto.ForageId);
        if (!forageExists)
            throw new ArgumentException($"Forage avec l'ID {dto.ForageId} introuvable.");

        var essai = _mapper.Map<EssaiResistance>(dto);
        _context.EssaisResistance.Add(essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(essai.Id)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'essai créé.");
    }

    public async Task<EssaiResistanceDto?> UpdateAsync(int id, EssaiResistanceUpdateDto dto)
    {
        var essai = await _context.EssaisResistance.FindAsync(id);
        if (essai == null) return null;

        _mapper.Map(dto, essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var essai = await _context.EssaisResistance
            .Include(e => e.Mesures)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (essai == null) return false;

        _context.EssaisResistance.Remove(essai);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MesureResistanceDto>> GetMesuresAsync(int essaiId)
    {
        var mesures = await _context.MesuresResistance
            .Where(m => m.EssaiResistanceId == essaiId)
            .OrderBy(m => m.Profondeur)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MesureResistanceDto>>(mesures);
    }

    public async Task<MesureResistanceDto> AddMesureAsync(int essaiId, MesureResistanceCreateDto dto)
    {
        var essaiExists = await _context.EssaisResistance.AnyAsync(e => e.Id == essaiId);
        if (!essaiExists)
            throw new ArgumentException($"Essai de résistance avec l'ID {essaiId} introuvable.");

        var mesure = _mapper.Map<MesureResistance>(dto);
        mesure.EssaiResistanceId = essaiId;

        _context.MesuresResistance.Add(mesure);
        await _context.SaveChangesAsync();

        return _mapper.Map<MesureResistanceDto>(mesure);
    }
}