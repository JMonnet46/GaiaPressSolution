using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17.Services;

public class ChantierService : IChantierService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public ChantierService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChantierDto>> GetAllAsync()
    {
        var chantiers = await _context.Chantiers
            .Include(c => c.Forages)
            .OrderByDescending(c => c.DateCreation)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ChantierDto>>(chantiers);
    }

    public async Task<ChantierDto?> GetByIdAsync(int id)
    {
        var chantier = await _context.Chantiers
            .Include(c => c.Forages)
            .FirstOrDefaultAsync(c => c.Id == id);
        return chantier == null ? null : _mapper.Map<ChantierDto>(chantier);
    }

    public async Task<ChantierDto?> GetByReferenceAsync(string reference)
    {
        var chantier = await _context.Chantiers
            .Include(c => c.Forages)
            .FirstOrDefaultAsync(c => c.Reference == reference);
        return chantier == null ? null : _mapper.Map<ChantierDto>(chantier);
    }

    public async Task<ChantierDto> CreateAsync(ChantierCreateDto dto)
    {
        var chantier = _mapper.Map<Chantier>(dto);
        chantier.DateCreation = DateTime.UtcNow;

        _context.Chantiers.Add(chantier);
        await _context.SaveChangesAsync();

        return _mapper.Map<ChantierDto>(chantier);
    }

    public async Task<ChantierDto?> UpdateAsync(int id, ChantierUpdateDto dto)
    {
        var chantier = await _context.Chantiers.FindAsync(id);
        if (chantier == null) return null;

        _mapper.Map(dto, chantier);
        chantier.DateModification = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return _mapper.Map<ChantierDto>(chantier);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var chantier = await _context.Chantiers
            .Include(c => c.Forages)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chantier == null) return false;

        // Vérifier s'il y a des forages associés
        if (chantier.Forages?.Any() == true)
        {
            throw new InvalidOperationException(
                "Impossible de supprimer un chantier contenant des forages. Supprimez d'abord les forages associés.");
        }

        _context.Chantiers.Remove(chantier);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ForageDto>> GetForagesAsync(int chantierId)
    {
        var forages = await _context.Forages
            .Where(f => f.ChantierId == chantierId)
            .Include(f => f.EssaisForage)
            .Include(f => f.EssaisExpansion)
            .Include(f => f.EssaisResistance)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ForageDto>>(forages);
    }
}