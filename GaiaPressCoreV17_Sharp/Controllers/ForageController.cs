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
    public class ForageController : ControllerBase
    {
        public readonly GaiaPressDbContext _dbContext;

        public ForageController(GaiaPressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Ajoute un nouveau forage.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddForage(Forage forage)
        {
            try
            {
                if (forage == null)
                {
                    return BadRequest(new { Message = "Le modèle de forage est vide." });
                }

                _dbContext.ListeForageDb.Add(forage);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetForageByRef), new { refForage = forage.refForage }, forage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur lors de l'ajout : {ex.Message}" });
            }
        }

        // Méthode référencée par CreatedAtAction - à ajouter si elle n'existe pas ailleurs
        [HttpGet("{refForage}")]
        public async Task<IActionResult> GetForageByRef(string refForage)
        {
            try
            {
                var forage = await _dbContext.ListeForageDb
                    .FirstOrDefaultAsync(f => f.refForage == refForage);

                if (forage == null)
                {
                    return NotFound(new { Message = $"Forage avec la référence {refForage} introuvable." });
                }
                return Ok(forage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }
    }
}