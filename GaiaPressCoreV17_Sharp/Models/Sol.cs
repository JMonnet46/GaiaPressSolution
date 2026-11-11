using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>
    /// Représente un sol géotechnique avec ses caractéristiques physiques.
    /// </summary>
    [Table("Sols")]
    public class Sol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Designation { get; set; }

        public double? MasseVolumique { get; set; }      // g/cm³
        public double? TeneurEnEau { get; set; }          // %
        public double? LimiteLiquidite { get; set; }      // %
        public double? LimitePlasticite { get; set; }     // %
        public double? IndicePlasticite { get; set; }
        public double? DensiteGrains { get; set; }

        // Classification
        [StringLength(10)]
        public string ClasseUSCS { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        // Relation avec Chantier (si un sol appartient à un chantier)
        public int? ChantierId { get; set; }

        [ForeignKey("ChantierId")]
        public virtual Chantier Chantier { get; set; }

        // Navigation vers les essais
        public virtual ICollection<EssaiForage> EssaisForage { get; set; }
        public virtual ICollection<EssaiExpansion> EssaisExpansion { get; set; }
        public virtual ICollection<EssaiResistance> EssaisResistance { get; set; }

        public Sol()
        {
            EssaisForage = new HashSet<EssaiForage>();
            EssaisExpansion = new HashSet<EssaiExpansion>();
            EssaisResistance = new HashSet<EssaiResistance>();
        }
    }
}
