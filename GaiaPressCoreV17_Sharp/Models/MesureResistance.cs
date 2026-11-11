using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>
    /// Représente une mesure de résistance pressiométrique.
    /// </summary>
    public class MesureResistance
    {
        [Key]
        public int Ind { get; set; }

        [Required]
        public string refEssai { get; set; }

        [Required]
        public int indiceMesure { get; set; }

        public float Pression { get; set; }
        public float Volume { get; set; }

        // Navigation vers l'essai parent
        [ForeignKey("refEssai")]
        public virtual EssaiResistance EssaiResistance { get; set; }
    }
}
