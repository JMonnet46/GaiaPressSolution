using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    public class MesureEssaiForage
    {
        [Key] public int Ind { get; set; }
        [Required] public string refEssai { get; set; }
        [Required] public int indiceMesure { get; set; }
        public float? Pression { get; set; }
        public float? Volume15 { get; set; }
        public float? Volume30 { get; set; }
        public float? Volume60 { get; set; }
        public float? ContrainteRadiale { get; set; }
        public float? ContrainteVerticale { get; set; }
        public float? Deformation { get; set; }
        public float? PNF { get; set; }
        public float? VNF { get; set; }
        public float? Qi { get; set; }
        public float? Pi { get; set; }
        public float? Qp { get; set; }
        public float? Pp { get; set; }
        [ForeignKey(nameof(refEssai))] public virtual EssaiForage EssaiForage { get; set; }
        public MesureEssaiForage() { }
        public MesureEssaiForage(string refEssai, int indiceMesure) { refEssai = refEssai; indiceMesure = indiceMesure; }
    }
}
