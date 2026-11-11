using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GaiaPressCoreV17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesureExpansionController : ControllerBase
    {
        private readonly GaiaPressDbContext _dbContext;

        public MesureExpansionController(GaiaPressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Récupère toutes les mesures d'expansion.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMesuresExpansion()
        {
            try
            {
                var mesures = await _dbContext.ListeMesureExpansions
                    .Include(m => m.EssaiExpansion)
                    .ToListAsync();
                return Ok(mesures);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère une mesure d'expansion par son identifiant.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMesureExpansionById(int id)
        {
            try
            {
                var mesure = await _dbContext.ListeMesureExpansions
                    .Include(m => m.EssaiExpansion)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (mesure == null)
                {
                    return NotFound(new { Message = $"Mesure d'expansion avec l'ID {id} introuvable." });
                }
                return Ok(mesure);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère toutes les mesures d'expansion pour un essai d'expansion spécifique.
        /// </summary>
        [HttpGet("essai/{refEssai}")]
        public async Task<IActionResult> GetMesuresExpansionByEssai(string refEssai)
        {
            try
            {
                var mesures = await _dbContext.ListeMesureExpansions
                    .Include(m => m.EssaiExpansion)
                    .Where(m => m.EssaiExpansion.refEssai == refEssai)
                    .OrderBy(m => m.Profondeur)
                    .ToListAsync();

                if (!mesures.Any())
                {
                    return NotFound(new { Message = $"Aucune mesure trouvée pour l'essai {refEssai}." });
                }
                return Ok(mesures);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Crée une nouvelle mesure d'expansion.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateMesureExpansion([FromBody] MesureExpansionCreateDto mesureDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier que l'essai d'expansion existe
                var essaiExiste = await _dbContext.ListeEssaiExpansions
                    .AnyAsync(e => e.refEssai == mesureDto.refEssaiExpansion);

                if (!essaiExiste)
                {
                    return BadRequest(new { Message = $"L'essai d'expansion {mesureDto.refEssaiExpansion} n'existe pas." });
                }

                var mesure = new MesureExpansion
                {
                    refEssaiExpansion = mesureDto.refEssaiExpansion,
                    Profondeur = mesureDto.Profondeur,
                    Pression = mesureDto.Pression,
                    Volume = mesureDto.Volume,
                    Diametre = mesureDto.Diametre,
                    DateMesure = mesureDto.DateMesure ?? DateTime.Now
                };

                _dbContext.ListeMesureExpansions.Add(mesure);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetMesureExpansionById),
                    new { id = mesure.Id },
                    mesure);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Met à jour une mesure d'expansion existante.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMesureExpansion(int id, [FromBody] MesureExpansionUpdateDto mesureDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mesure = await _dbContext.ListeMesureExpansions.FindAsync(id);
                if (mesure == null)
                {
                    return NotFound(new { Message = $"Mesure d'expansion avec l'ID {id} introuvable." });
                }

                // Mise à jour des propriétés
                mesure.Profondeur = mesureDto.Profondeur;
                mesure.Pression = mesureDto.Pression;
                mesure.Volume = mesureDto.Volume;
                mesure.Diametre = mesureDto.Diametre;
                if (mesureDto.DateMesure.HasValue)
                {
                    mesure.DateMesure = mesureDto.DateMesure.Value;
                }

                await _dbContext.SaveChangesAsync();
                return Ok(new { Message = "Mesure d'expansion mise à jour avec succès.", Mesure = mesure });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Supprime une mesure d'expansion.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesureExpansion(int id)
        {
            try
            {
                var mesure = await _dbContext.ListeMesureExpansions.FindAsync(id);
                if (mesure == null)
                {
                    return NotFound(new { Message = $"Mesure d'expansion avec l'ID {id} introuvable." });
                }

                _dbContext.ListeMesureExpansions.Remove(mesure);
                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = $"Mesure d'expansion {id} supprimée avec succès." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère les statistiques des mesures d'expansion pour un essai.
        /// </summary>
        [HttpGet("essai/{refEssai}/statistiques")]
        public async Task<IActionResult> GetStatistiquesMesuresExpansion(string refEssai)
        {
            try
            {
                var mesures = await _dbContext.ListeMesureExpansions
                    .Where(m => m.EssaiExpansion.refEssai == refEssai)
                    .ToListAsync();

                if (!mesures.Any())
                {
                    return NotFound(new { Message = $"Aucune mesure trouvée pour l'essai {refEssai}." });
                }

                var statistiques = new
                {
                    NombreMesures = mesures.Count,
                    PressionMin = mesures.Min(m => m.Pression),
                    PressionMax = mesures.Max(m => m.Pression),
                    PressionMoyenne = mesures.Average(m => m.Pression),
                    VolumeMin = mesures.Min(m => m.Volume),
                    VolumeMax = mesures.Max(m => m.Volume),
                    VolumeMoyen = mesures.Average(m => m.Volume),
                    ProfondeurMin = mesures.Min(m => m.Profondeur),
                    ProfondeurMax = mesures.Max(m => m.Profondeur)
                };

                return Ok(statistiques);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur : {ex.Message}" });
            }
        }
    }
}