using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using GaiaPressCoreV17.Services;
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
    public class SondeController : ControllerBase
    {
        // ✅ Déclarer les dépendances
        public readonly GaiaPressDbContext _context;
        public readonly ChantierService _ChantierService;

        /// <summary>
        /// Constructeur avec injection de dépendances.
        /// </summary>
        public SondeController(GaiaPressDbContext context, ChantierService chantierService)
        {
            _context = context;
            _ChantierService = chantierService;
        }

        // ======================
        // GET: api/Sonde
        // ======================
        /// <summary>
        /// Récupère toutes les sondes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSondes()
        {
            try
            {
                // ✅ Utilise _context.ListeSondeDb (maintenant défini)
                var sondes = await _context.ListeSondeDb.ToListAsync();
                return Ok(sondes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des sondes: {ex.Message}");
            }
        }

        // ===================================
        // GET: api/Sonde/{id}
        // ===================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSondeById(string id)
        {
            try
            {
                // ✅ Utilise _context.ListeSondeDb avec FirstOrDefaultAsync
                var sonde = await _context.ListeSondeDb
                    .FirstOrDefaultAsync(s => s.refSonde == id);

                if (sonde == null)
                {
                    return NotFound($"La sonde avec l'ID {id} n'existe pas.");
                }

                return Ok(sonde);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération de la sonde: {ex.Message}");
            }
        }

        // ======================
        // POST: api/Sonde
        // ======================
        /// <summary>
        /// Ajoute une nouvelle sonde.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSonde(Probe sonde)
        {
            try
            {
                if (sonde == null)
                {
                    return BadRequest("La sonde ne peut pas être null.");
                }

                // ✅ Vérifie si la sonde existe déjà
                if (await _context.ListeSondeDb.AnyAsync(s => s.refSonde == sonde.refSonde))
                {
                    return Conflict($"Une sonde avec l'ID {sonde.refSonde} existe déjà.");
                }

                _context.ListeSondeDb.Add(sonde);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSondeById), new { id = sonde.refSonde }, sonde);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la création de la sonde: {ex.Message}");
            }
        }

        // ================================================
        // GET: api/Sonde/ByChantier/{chantierId}
        // ================================================
        /// <summary>
        /// Récupère toutes les sondes associées à un chantier.
        /// </summary>
        [HttpGet("ByChantier/{chantierId}")]
        public async Task<IActionResult> GetSondesByChantier(string chantierId)
        {
            try
            {
                var chantier = await _context.LesChantiersDb
                    .Include(c => c.ListSondes)
                    .FirstOrDefaultAsync(c => c.refChantier == chantierId);

                if (chantier == null)
                {
                    return NotFound($"Le chantier avec l'ID {chantierId} n'existe pas.");
                }

                return Ok(chantier.ListSondes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des sondes du chantier: {ex.Message}");
            }
        }

        // ═════════════════════════════════════════════════════════════
        // ✅ NOUVELLES MÉTHODES : ESSAIS D'EXPANSION
        // ═════════════════════════════════════════════════════════════

        /// <summary>
        /// Récupère tous les essais d'expansion pour une sonde.
        /// GET: api/sonde/{id}/expansion
        /// </summary>
        [HttpGet("{id}/expansion")]
        public async Task<IActionResult> GetEssaisExpansion(string id)
        {
            try
            {
                var essais = await _context.ListeEssaiExpansionsDb
                    .Where(e => e.refSonde == id)
                    .Select(e => new EssaiExpansionDto
                    {
                        refEssai = e.refEssai,
                        nomEssai = e.nomEssai,
                        dateEssai = e.dateEssai,
                        ClassPrecis = e.ClassPrecis,
                        refSonde = e.refSonde
                    })
                    .ToListAsync();

                return Ok(essais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des essais d'expansion: {ex.Message}");
            }
        }

        /// <summary>
        /// Récupère tous les essais de résistance pour une sonde.
        /// GET: api/sonde/{id}/resistance
        /// </summary>
        [HttpGet("{id}/resistance")]
        public async Task<IActionResult> GetEssaisResistance(string id)
        {
            try
            {
                // Charger les essais de résistance associés
                var essais = await _context.ListeEssaiResistancesDb
                    .Where(e => e.refSonde == id)
                    .Select(e => new EssaiResistanceDto
                    {
                        refEssai = e.refEssai,
                        nomEssai = e.nomEssai,
                        dateEssai = e.dateEssai,
                        ClassPrecis = e.ClassPrecis,
                        refSonde = e.refSonde
                    })
                    .ToListAsync();

                return Ok(essais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des essais de résistance: {ex.Message}");
            }
        }

        // ═════════════════════════════════════════════════════════════
        // ✅ NOUVELLES MÉTHODES : ESSAI UNIQUE
        // ═════════════════════════════════════════════════════════════

        /// <summary>
        /// Récupère un essai d'expansion spécifique.
        /// GET: api/sonde/{id}/expansion/{essaiId}
        /// </summary>
        [HttpGet("{id}/expansion/{essaiId}")]
        public async Task<IActionResult> GetEssaiExpansionById(string id, string essaiId)
        {
            try
            {
                var essai = await _context.ListeEssaiExpansionsDb
                    .Where(e => e.refSonde == id && e.refEssai == essaiId)
                    .FirstOrDefaultAsync();

                if (essai == null)
                {
                    return NotFound($"Essai d'expansion {essaiId} introuvable pour la sonde {id}.");
                }

                var essaiDto = new EssaiExpansionDto
                {
                    refEssai = essai.refEssai,
                    nomEssai = essai.nomEssai,
                    dateEssai = essai.dateEssai,
                    ClassPrecis = essai.ClassPrecis,
                    refSonde = essai.refSonde
                };

                return Ok(essaiDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur: {ex.Message}");
            }
        }

        /// <summary>
        /// Récupère un essai de résistance spécifique.
        /// GET: api/sonde/{id}/resistance/{essaiId}
        /// </summary>
        [HttpGet("{id}/resistance/{essaiId}")]
        public async Task<IActionResult> GetEssaiResistanceById(string id, string essaiId)
        {
            try
            {
                var essai = await _context.ListeEssaiResistancesDb
                    .Where(e => e.refSonde == id && e.refEssai == essaiId)
                    .FirstOrDefaultAsync();

                if (essai == null)
                {
                    return NotFound($"Essai de résistance {essaiId} introuvable pour la sonde {id}.");
                }

                var essaiDto = new EssaiResistanceDto
                {
                    refEssai = essai.refEssai,
                    nomEssai = essai.nomEssai,
                    dateEssai = essai.dateEssai,
                    ClassPrecis = essai.ClassPrecis,
                    refSonde = essai.refSonde
                };

                return Ok(essaiDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur: {ex.Message}");
            }
        }
    }
}