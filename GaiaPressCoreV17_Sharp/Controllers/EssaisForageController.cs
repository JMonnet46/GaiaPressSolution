using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace GaiaPressCoreV17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EssaisForageController : ControllerBase
    {
        private readonly GaiaPressDbContext _dbContext;

        public EssaisForageController(GaiaPressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Récupère un essai de forage par sa référence.
        /// </summary>
        [HttpGet("{refEssai}")]
        public async Task<IActionResult> GetEssaiForageByRef(string refEssai)
        {
            try
            {
                // ✅ Correction: Utilisation de FirstOrDefaultAsync avec CancellationToken
                var essai = await _dbContext.ListeEssaiForageDb.FirstOrDefaultAsync(e => e.refEssai == refEssai, CancellationToken.None);

                if (essai == null)
                {
                    return NotFound(new { Message = $"Essai avec la référence {refEssai} introuvable." });
                }
                return Ok(essai);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère un essai de forage par référence de forage.
        /// </summary>
        [HttpGet("byForage/{refForage}")]
        public async Task<IActionResult> GetEssaiForageByForageRef(string refForage)
        {
            try
            {
                // ✅ Correction: Utilisation de Where et ToListAsync
                var essais = await _dbContext.ListeEssaiForageDb
                    .Where(e => e.refForage == refForage)
                    .ToListAsync(CancellationToken.None);

                if (essais == null || essais.Count == 0)
                {
                    return NotFound(new { Message = $"Aucun essai trouvé pour le forage {refForage}." });
                }
                return Ok(essais);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }
    }
}