using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>
    /// Représente une mesure de l'essai d'expansion (Pressiomètre Ménard).
    /// </summary>
    [Table("MesuresExpansion")]
    public class MesureExpansion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EssaiExpansionId { get; set; }

        [ForeignKey("EssaiExpansionId")]
        public virtual EssaiExpansion EssaiExpansion { get; set; }

        public int Ordre { get; set; }
        public double Pression { get; set; }               // kPa
        public double Volume { get; set; }                 // cm³
        public double? DiametreSonde { get; set; }          // mm
        public double? LongueurSonde { get; set; }          // mm

        // Valeurs calculées
        public double? ModulePressiometrique { get; set; }  // MPa (EM)
        public double? PressionLimite { get; set; }         // kPa (PL)
        public double? PressionFluage { get; set; }         // kPa (PF)

        [StringLength(200)]
        public string Observation { get; set; }
    }
}