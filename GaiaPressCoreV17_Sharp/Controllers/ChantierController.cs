using GaiaPressCoreV17.Data;
using GaiaPressCoreV17.Logic;
using GaiaPressCoreV17.Models;
using GaiaPressCoreV17.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GaiaPressCoreV17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChantierController : ControllerBase
    {
        // Déclaration des dépendances avec injection de dépendances
        private readonly GaiaPressDbContext _context;
        private readonly ChantierLogic _chantierLogic;

        // Constructeur avec injection de dépendances
        private readonly ChantierService _chantierService;

        private readonly DataService _dataService;  // Ajoutez cette ligne
        private readonly PdfService _pdfService;  // ✅ Déclaration

        public ChantierController(GaiaPressDbContext context)
        {
            _context = context;
            _dataService = new DataService(_context);  // ✅ Passez _context à DataService
            _chantierService = new ChantierService(_dataService, _context);
        }

        // Récupérer la liste des chantiers
        [HttpGet]
        public IActionResult GetChantiers()
        {
            try
            {
                List<Chantier> chantiers = _chantierService.GetAllChantiers();  // ✅ Utilise l'instance
                if (chantiers == null)
                {
                    return NotFound("Aucun chantier trouvé.");
                }
                return Ok(chantiers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des chantiers: {ex.Message}");
            }
        }

        // Récupérer un chantier par son ID
        //[HttpGet("{id}")]
        //public IActionResult GetChantierById(string refChantier)
        //{
        //    try
        //    {
        //        Chantier chantier = _chantierService.GetById(int.Parse(c));  // ✅ Utilisez le paramètre `c` et la méthode `GetById`
        //        if (chantier == null)
        //        {
        //            return NotFound($"Le chantier avec l'ID {id} n'existe pas.");
        //        }
        //        return Ok(chantier);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Erreur lors de la récupération du chantier: {ex.Message}");
        //    }
        //}

        // Générer un PDF pour un chantier
        [HttpGet("{id}/pdf")]
        public IActionResult GeneratePDF(string refChantier)
        {
            try
            {
                Chantier chantier = _chantierService.GetChantier(refChantier);
                if (chantier == null)
                {
                    return NotFound($"Le chantier avec l'ID {refChantier} n'existe pas.");
                }

                byte[] pdfBytes = PdfService.GenerateChantierPDF(chantier);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return BadRequest("Impossible de générer le PDF.");
                }

                return File(pdfBytes, "application/pdf", $"Chantier_{refChantier}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la génération du PDF: {ex.Message}");
            }
        }

        // Recevoir une capture d'écran du client
        [HttpPost("screenshot")]
        public IActionResult SaveScreenshot([FromBody] ScreenshotModel model)
        {
            try
            {
                // Vérification des données reçues
                if (model == null || model.ImageData == null || model.ImageData.Length == 0)
                {
                    return BadRequest("Aucune donnée d'image reçue.");
                }

                // Sauvegarder l'image dans le dossier temporaire
                string tempPath = Path.Combine(Path.GetTempPath(), $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                System.IO.File.WriteAllBytes(tempPath, model.ImageData);

                return Ok(new { Message = "Capture d'écran enregistrée avec succès.", Path = tempPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'enregistrement de la capture d'écran: {ex.Message}");
            }
        }
    }
}