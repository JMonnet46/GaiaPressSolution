using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressBlazorWebAPP.Models
{
    [Table("Forages")]
    public class ForageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RefForage { get; set; } = string.Empty;

        public string RefChantier { get; set; }
        public int NumeroForage { get; set; }
        public string NomForage { get; set; }
        public DateTime DateForage { get; set; }
        public string Materiel { get; set; }
        public string Outil { get; set; }  // Corrigé : plus de "system."

        public float AbscisseX { get; set; }
        public float OrdonneeY { get; set; }
        public float AltitudeZ { get; set; }
        public float CoteInit { get; set; }
        public float CoteFinale { get; set; }

        public bool EventsSuspendus { get; set; }
        public bool IsSaved { get; set; }

        public string FichierChantier { get; set; }
    }  // Accolade fermante, PAS "End Namespace"
}