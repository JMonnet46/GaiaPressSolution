using AutoMapper;
using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GaiaPressCoreV17_Sharp.Services.Implementation;

public class SolService : ISolService
{
    private readonly GaiaPressDbContext _context;
    private readonly IMapper _mapper;

    public SolService(GaiaPressDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SolDto>> GetAllAsync()
    {
        var sols = await _context.Sols
            .OrderBy(s => s.Code)
            .ToListAsync();
        return _mapper.Map<IEnumerable<SolDto>>(sols);
    }

    public async Task<SolDto?> GetByIdAsync(int id)
    {
        var sol = await _context.Sols.FindAsync(id);
        return sol == null ? null : _mapper.Map<SolDto>(sol);
    }

    public async Task<SolDto?> GetByCodeAsync(string code)
    {
        var sol = await _context.Sols
            .FirstOrDefaultAsync(s => s.Code == code);
        return sol == null ? null : _mapper.Map<SolDto>(sol);
    }

    public async Task<SolDto> CreateAsync(SolCreateDto dto)
    {
        // Vérifier l'unicité du code
        if (await _context.Sols.AnyAsync(s => s.Code == dto.Code))
            throw new ArgumentException($"Un sol avec le code '{dto.Code}' existe déjà.");

        var sol = _mapper.Map<Sol>(dto);
        _context.Sols.Add(sol);
        await _context.SaveChangesAsync();
        return _mapper.Map<SolDto>(sol);
    }

    public async Task<SolDto?> UpdateAsync(int id, SolUpdateDto dto)
    {
        var sol = await _context.Sols.FindAsync(id);
        if (sol == null) return null;

        // Vérifier l'unicité du code si modifié
        if (dto.Code != sol.Code && await _context.Sols.AnyAsync(s => s.Code == dto.Code))
            throw new ArgumentException($"Un sol avec le code '{dto.Code}' existe déjà.");

        _mapper.Map(dto, sol);
        await _context.SaveChangesAsync();
        return _mapper.Map<SolDto>(sol);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sol = await _context.Sols
            .Include(s => s.Forages)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sol == null) return false;

        if (sol.Forages?.Any() == true)
            throw new InvalidOperationException(
                "Impossible de supprimer un sol utilisé par des forages.");

        _context.Sols.Remove(sol);
        await _context.SaveChangesAsync();
        return true;
    }
}