using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17_Sharp.Services.Implementation;

public class EssaiExpansionService : IEssaiExpansionService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public EssaiExpansionService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EssaiExpansionDto>> GetAllAsync()
    {
        var essais = await _context.EssaisExpansion
            .Include(e => e.Forage)
            .Include(e => e.Mesures)
            .OrderByDescending(e => e.DateEssai)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiExpansionDto>>(essais);
    }

    public async Task<EssaiExpansionDto?> GetByIdAsync(int id)
    {
        var essai = await _context.EssaisExpansion
            .Include(e => e.Forage)
            .Include(e => e.Mesures.OrderBy(m => m.Pression))
            .FirstOrDefaultAsync(e => e.Id == id);
        return essai == null ? null : _mapper.Map<EssaiExpansionDto>(essai);
    }

    public async Task<IEnumerable<EssaiExpansionDto>> GetByForageAsync(int forageId)
    {
        var essais = await _context.EssaisExpansion
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiExpansionDto>>(essais);
    }

    public async Task<EssaiExpansionDto> CreateAsync(EssaiExpansionCreateDto dto)
    {
        var forageExists = await _context.Forages.AnyAsync(f => f.Id == dto.ForageId);
        if (!forageExists)
            throw new ArgumentException($"Forage avec l'ID {dto.ForageId} introuvable.");

        var essai = _mapper.Map<EssaiExpansion>(dto);
        _context.EssaisExpansion.Add(essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(essai.Id)
            ?? throw new InvalidOperationException("Erreur lors de la récupération de l'essai créé.");
    }

    public async Task<EssaiExpansionDto?> UpdateAsync(int id, EssaiExpansionUpdateDto dto)
    {
        var essai = await _context.EssaisExpansion.FindAsync(id);
        if (essai == null) return null;

        _mapper.Map(dto, essai);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var essai = await _context.EssaisExpansion
            .Include(e => e.Mesures)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (essai == null) return false;

        _context.EssaisExpansion.Remove(essai);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MesureExpansionDto>> GetMesuresAsync(int essaiId)
    {
        var mesures = await _context.MesuresExpansion
            .Where(m => m.EssaiExpansionId == essaiId)
            .OrderBy(m => m.Pression)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MesureExpansionDto>>(mesures);
    }

    public async Task<MesureExpansionDto> AddMesureAsync(int essaiId, MesureExpansionCreateDto dto)
    {
        var essaiExists = await _context.EssaisExpansion.AnyAsync(e => e.Id == essaiId);
        if (!essaiExists)
            throw new ArgumentException($"Essai d'expansion avec l'ID {essaiId} introuvable.");

        var mesure = _mapper.Map<MesureExpansion>(dto);
        mesure.EssaiExpansionId = essaiId;

        _context.MesuresExpansion.Add(mesure);
        await _context.SaveChangesAsync();

        return _mapper.Map<MesureExpansionDto>(mesure);
    }

    public async Task<double?> CalculerModulePressiometriqueAsync(int essaiId)
    {
        var essai = await _context.EssaisExpansion
            .Include(e => e.Mesures)
            .FirstOrDefaultAsync(e => e.Id == essaiId);

        if (essai?.Mesures == null || essai.Mesures.Count < 2)
            return null;

        // Calcul simplifié du module pressiométrique (Em)
        // Em = 2 * (1 + ν) * Vm * ΔP / ΔV
        // Nécessite les paramètres de la sonde et du sol

        var mesures = essai.Mesures.OrderBy(m => m.Pression).ToList();

        // Phase pseudo-élastique : entre 0.3*Pf et 0.7*Pf (approximation)
        var pf = essai.PressionLimite ?? mesures.Max(m => m.Pression);
        var pDebut = 0.3 * pf;
        var pFin = 0.7 * pf;

        var mesuresPhase = mesures
            .Where(m => m.Pression >= pDebut && m.Pression <= pFin)
            .ToList();

        if (mesuresPhase.Count < 2)
            return null;

        var deltaP = mesuresPhase.Last().Pression - mesuresPhase.First().Pression;
        var deltaV = mesuresPhase.Last().Volume - mesuresPhase.First().Volume;
        var vMoyen = mesuresPhase.Average(m => m.Volume);

        if (deltaV == 0) return null;

        // ν (coefficient de Poisson) typique pour sol : 0.3
        const double nu = 0.3;
        var em = 2 * (1 + nu) * vMoyen * (deltaP / deltaV);

        return Math.Round(em, 2);
    }
}