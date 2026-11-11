namespace GaiaPressCoreV17.Models.Dto
{
    public class EssaiResistanceDto
    {
        public string refEssai { get; set; }
        public string refSonde { get; set; }
        public int ClassPrecis { get; set; }
        public string nomEssai { get; set; }
        public DateTime dateEssai { get; set; }
        public float difPresEauAir { get; set; }
        public float hauteurCpv { get; set; }
        public float a0 { get; set; }
        public float a1 { get; set; }
        public float a2 { get; set; }
        public float a3 { get; set; }

        // ← RIEN DE PLUS !
    }
}