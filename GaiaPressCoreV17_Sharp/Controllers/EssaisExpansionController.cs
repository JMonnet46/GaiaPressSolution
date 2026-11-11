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
    public class EssaisExpansionController : ControllerBase
    {
        public readonly GaiaPressDbContext _dbContext;

        public EssaisExpansionController(GaiaPressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Récupère tous les essais d'expansion.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEssaisExpansion()
        {
            try
            {
                // ✅ CORRECTION: Retourne une IActionResult avec la liste
                var essais = await _dbContext.ListeEssaiExpansionsDb.ToListAsync();
                return Ok(essais);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }
    }
}