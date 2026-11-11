using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17.Services;

public class ForageService : IForageService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public ForageService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ForageDto>> GetAllAsync()
    {
        var forages = await _context.Forages
            .Include(f => f.Chantier)
            .Include(f => f.Sol)
            .OrderByDescending(f => f.DateRealisation)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ForageDto>>(forages);
    }

    public async Task<ForageDto?> GetByIdAsync(int id)
    {
        var forage = await _context.Forages
            .Include(f => f.Chantier)
            .Include(f => f.Sol)
            .Include(f => f.EssaisForage)
                .ThenInclude(e => e.Mesures)
            .Include(f => f.EssaisExpansion)
                .ThenInclude(e => e.Mesures)
            .Include(f => f.EssaisResistance)
                .ThenInclude(e => e.Mesures)
            .FirstOrDefaultAsync(f => f.Id == id);

        return forage == null ? null : _mapper.Map<ForageDto>(forage);
    }

    public async Task<IEnumerable<ForageDto>> GetByChantierAsync(int chantierId)
    {
        var forages = await _context.Forages
            .Where(f => f.ChantierId == chantierId)
            .Include(f => f.Sol)
            .Include(f => f.EssaisForage)
            .Include(f => f.EssaisExpansion)
            .Include(f => f.EssaisResistance)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ForageDto>>(forages);
    }

    public async Task<ForageDto> CreateAsync(ForageCreateDto dto)
    {
        // Vérifier que le chantier existe
        var chantierExists = await _context.Chantiers.AnyAsync(c => c.Id == dto.ChantierId);
        if (!chantierExists)
            throw new ArgumentException($"Chantier avec l'ID {dto.ChantierId} introuvable.");

        var forage = _mapper.Map<Forage>(dto);
        forage.DateCreation = DateTime.UtcNow;

        _context.Forages.Add(forage);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(forage.Id)
            ?? throw new InvalidOperationException("Erreur lors de la récupération du forage créé.");
    }

    public async Task<ForageDto?> UpdateAsync(int id, ForageUpdateDto dto)
    {
        var forage = await _context.Forages.FindAsync(id);
        if (forage == null) return null;

        _mapper.Map(dto, forage);
        forage.DateModification = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var forage = await _context.Forages
            .Include(f => f.EssaisForage)
            .Include(f => f.EssaisExpansion)
            .Include(f => f.EssaisResistance)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (forage == null) return false;

        // Vérifier s'il y a des essais associés
        if (forage.EssaisForage?.Any() == true ||
            forage.EssaisExpansion?.Any() == true ||
            forage.EssaisResistance?.Any() == true)
        {
            throw new InvalidOperationException(
                "Impossible de supprimer un forage contenant des essais. Supprimez d'abord les essais associés.");
        }

        _context.Forages.Remove(forage);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EssaiForageDto>> GetEssaisForageAsync(int forageId)
    {
        var essais = await _context.EssaisForage
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiForageDto>>(essais);
    }

    public async Task<IEnumerable<EssaiExpansionDto>> GetEssaisExpansionAsync(int forageId)
    {
        var essais = await _context.EssaisExpansion
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiExpansionDto>>(essais);
    }

    public async Task<IEnumerable<EssaiResistanceDto>> GetEssaisResistanceAsync(int forageId)
    {
        var essais = await _context.EssaisResistance
            .Where(e => e.ForageId == forageId)
            .Include(e => e.Mesures)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EssaiResistanceDto>>(essais);
    }
}