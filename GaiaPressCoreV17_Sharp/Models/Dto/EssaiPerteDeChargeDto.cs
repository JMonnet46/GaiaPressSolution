namespace GaiaPressCoreV17.Models.Dto
{
    public class EssaiPerteDeChargeDto
    {
        public string refEssai { get; set; }
        public string refSonde { get; set; }
        public string nomEssai { get; set; }
        public DateTime DateEssai { get; set; }
        public float difPresEauAir { get; set; }
        public float hauteurCpv { get; set; }
        public float b0 { get; set; }
        public float b1 { get; set; }
        public float b2 { get; set; }
        public float b3 { get; set; }
    }
}