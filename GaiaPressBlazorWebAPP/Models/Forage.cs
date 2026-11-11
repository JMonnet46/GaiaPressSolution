using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GaiaPressCoreV17.Models;

namespace GaiaPressBlazorWebAPP.Models
{
    public class Forage
    {
        [Key]
        public string RefForage { get; set; } = "refBorehole";

        public int NumeroForage { get; set; } = 0;

        public string NomForage { get; set; } = "nameBorehole";

        public DateTime DateForage { get; set; } = DateTime.Now;

        public string Materiel { get; set; } = string.Empty;

        public string Outil { get; set; } = string.Empty;

        public float AbscisseX { get; set; } = 0f;

        public float OrdonneeY { get; set; } = 0f;

        public float AltitudeZ { get; set; } = 0f;

        public float CoteInit { get; set; } = 0f;

        public float CoteFinale { get; set; } = 0f;

        public string RefChantier { get; set; } = string.Empty;

        // Liste des essais associés (depuis GaiaPressCoreV17)
        public List<Essai_Forage> MesEssais { get; set; } = new List<Essai_Forage>();

        public void IHaveBeenModified()
        {
            Console.WriteLine($"Forage {RefForage} modifié.");
        }
    }
}
