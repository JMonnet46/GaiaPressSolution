//. MesuresEssaiForageController.vb → MesuresEssaiForageController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Services;
using Microsoft.AspNetCore.Mvc;

namespace GaiaPressCoreV17.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des essais de forage.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MesuresEssaiForageDbController : ControllerBase
    {
        // Déclaration des dépendances avec injection de dépendances
        public readonly EssaiForageService _essaiForageService;

        /// <summary>
        /// Constructeur avec injection de dépendances.
        /// </summary>
        public MesuresEssaiForageDbController(EssaiForageService essaiForageService)
        {
            _essaiForageService = essaiForageService;
        }

        // ======================
        // ESSAIS DE FORAGE
        // ======================

        /// <summary>
        /// Récupère tous les essais de forage.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllEssaiForages()
        {
            try
            {
                var essais = _essaiForageService.GetAllEssaiForages();
                return Ok(essais);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur lors de la récupération des essais de forage : {ex.Message}" });
            }
        }

        /// <summary>
        /// Récupère un essai de forage par sa référence.
        /// </summary>
        [HttpGet("{refEssai}")]
        public IActionResult GetEssaiForageByRef(string refEssai)
        {
            try
            {
                var essai = _essaiForageService.GetEssaiForageByrefEssai(refEssai);
                if (essai == null)
                {
                    return NotFound(new { Message = $"Essai de forage avec la référence {refEssai} introuvable." });
                }
                return Ok(essai);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur lors de la récupération de l'essai de forage : {ex.Message}" });
            }
        }

        /// <summary>
        /// Ajoute un nouvel essai de forage.
        /// </summary>
        // ✅ Correction
        [HttpPost]
        public async Task<IActionResult> AddEssaiForage([FromBody] Essai_Forage essaiModel)
        {
            try
            {
                if (essaiModel == null)
                {
                    return BadRequest(new { Message = "Le modèle d'essai de forage est vide." });
                }

                await _essaiForageService.AddEssaiForage(essaiModel);  // ✅ Await ajouté
                return CreatedAtAction(nameof(GetEssaiForageByRef), new { refEssai = essaiModel.refEssai }, essaiModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur lors de l'ajout de l'essai de forage : {ex.Message}" });
            }
        }

        /// <summary>
        /// Met à jour un essai de forage existant.
        /// </summary>
        [HttpPut("{refEssai}")]
        public IActionResult UpdateEssaiForage(string refEssai, [FromBody] Essai_Forage essaiModel)
        {
            try
            {
                if (essaiModel == null || refEssai != essaiModel.refEssai)
                {
                    return BadRequest(new { Message = "Données d'essai de forage invalides." });
                }

                var existingEssai = _essaiForageService.GetEssaiForageByrefEssai(refEssai);
                if (existingEssai == null)
                {
                    return NotFound(new { Message = $"Essai de forage avec la référence {refEssai} introuvable." });
                }

                _essaiForageService.UpdateEssaiForage(essaiModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erreur lors de la mise à jour de l'essai de forage : {ex.Message}" });
            }
        }
    }
}

2