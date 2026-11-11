using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GaiaPressCoreV17.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des sols.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SolController : ControllerBase
    {
        // ✅ Déclarer les dépendances
        public readonly GaiaPressDbContext _context;
        public readonly ChantierService _ChantierService;

        /// <summary>
        /// Constructeur avec injection de dépendances.
        /// </summary>
        public SolController(GaiaPressDbContext context, ChantierService chantierService)
        {
            _context = context;
            _ChantierService = chantierService;
        }

        // GET: api/Sol
        [HttpGet]
        public async Task<IActionResult> GetAllSols()
        {
            try
            {
                var sols = await _context.ListeSolDb.ToListAsync();
                return Ok(sols);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des sols: {ex.Message}");
            }
        }

        // ======================
        // POST: api/Sol
        // ======================
        /// <summary>
        /// Ajoute un nouveau sol.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSol(Sol sol)
        {
            try
            {
                if (sol == null)
                {
                    return BadRequest("Le sol ne peut pas être null.");
                }

                _context.ListeSolDb.Add(sol);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSolById), new { id = sol.refSol }, sol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la création du sol: {ex.Message}");
            }
        }

        // GET: api/Sol/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolById(string id)
        {
            try
            {
                var sol = await _context.ListeSolDb.FirstOrDefaultAsync(s => s.refSol == id);
                if (sol == null)
                {
                    return NotFound($"Le sol avec l'ID {id} n'existe pas.");
                }
                return Ok(sol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération du sol: {ex.Message}");
            }
        }

        // ======================
        // DELETE: api/Sol/{id}
        // ======================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSol(string id)
        {
            try
            {
                var sol = await _context.ListeSolDb.FirstOrDefaultAsync(s => s.refSol == id);
                if (sol == null)
                {
                    return NotFound($"Le sol avec l'ID {id} n'existe pas.");
                }

                _context.ListeSolDb.Remove(sol);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la suppression du sol: {ex.Message}");
            }
        }

        // ======================
        // PUT: api/Sol/{id}
        // ======================
        /// <summary>
        /// Met à jour un sol existant.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSol(string id, Sol sol)
        {
            try
            {
                if (id != sol.refSol)
                {
                    return BadRequest("L'ID du sol ne correspond pas à l'ID dans l'URL.");
                }

                var existingSol = await _context.ListeSolDb.FirstOrDefaultAsync(s => s.refSol == id);
                if (existingSol == null)
                {
                    return NotFound($"Le sol avec l'ID {id} n'existe pas.");
                }

                _context.Entry(existingSol).CurrentValues.SetValues(sol);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la mise à jour du sol: {ex.Message}");
            }
        }

        // ======================
        // GET: api/Sol/ByChantier/{chantierId}
        // ======================
        /// <summary>
        /// Récupère tous les sols associés à un chantier.
        /// </summary>
        [HttpGet("ByChantier/{chantierId}")]
        public async Task<IActionResult> GetSolsByChantier(string chantierId)
        {
            try
            {
                // ✅ Syntaxe correcte pour votre modèle (ListSols, refChantier)
                var chantier = await _context.LesChantiersDb
                    .Include(c => c.ListSols)
                    .FirstOrDefaultAsync(c => c.refChantier == chantierId);

                if (chantier == null)
                {
                    return NotFound($"Le chantier avec l'ID {chantierId} n'existe pas.");
                }

                return Ok(chantier.ListSols);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des sols du chantier: {ex.Message}");
            }
        }
    }
}