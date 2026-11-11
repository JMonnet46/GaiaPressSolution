using System.ComponentModel.DataAnnotations;

namespace GaiaPressCoreV17.Models.Dto
{
    public class MesureExpansionDto
    {
        // Définition de la clef primaire de la Table LesMesureExpansion
        [Key]
        public int Ind { get; set; }

        [Required]
        public string refEssai { get; set; }

        [Required]
        public int indiceMesure { get; set; }

        public float Pression { get; set; }

        public float Volume { get; set; }
    }
}