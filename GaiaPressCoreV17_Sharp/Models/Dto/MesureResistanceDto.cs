using System.ComponentModel.DataAnnotations;

namespace GaiaPressCoreV17.Models.Dto
{
    public class MesureResistanceDto
    {
        [Key]
        public int Ind { get; set; }

        public string refEssai { get; set; }

        public int indiceMesure { get; set; }

        public float Pression { get; set; }

        public float Volume { get; set; }
    }
}