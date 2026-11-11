using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GaiaPressCoreV17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EssaisResistanceController : ControllerBase
    {
        private readonly GaiaPressDbContext _dbContext;

        public EssaisResistanceController(GaiaPressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Récupère tous les essais de résistance.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEssaisResistanceDb()
        {
            try
            {
                var essais = await _dbContext.ListeEssaiResistancesDb.ToListAsync();
                return Ok(essais);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère un essai de résistance par sa référence.
        /// </summary>
        [HttpGet("{refEssai}")]
        public async Task<IActionResult> GetEssaiResistanceByRef(string refEssai)
        {
            try
            {
                // ✅ CORRECTION: Utilisation de la bonne propriété (refEssai doit exister dans EssaiResistance)
                var essai = await _dbContext.ListeEssaiResistancesDb
                    .FirstOrDefaultAsync(e => e.GetrefEssai == refEssai);

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
    }
}