namespace GaiaPressCoreV17.Models.Dto
{
    public class EssaiExpansionDto
    {
        public string refEssai { get; set; }
        public string refSonde { get; set; }
        public int ClassPrecis { get; set; }
        public string nomEssai { get; set; }
        public DateTime dateEssai { get; set; }
        public float difPresEauAir { get; set;
        public float hauteurCpv { get; set; }
        public float Di { get; set; }
        public float Vc { get; set; }
        public float Vs { get; set; }
        public float EpMembrane { get; set; }
        public float Prp { get; set; }
        public float PrV { get; set; }
        public float coefficientA { get; set; }
        public float coefficientB { get; set; }
        public float PrecisVc { get; set; }
        public float PrecisVs { get; set; }
        public float PrEpMembrane { get; set; }
        public float PrecisA { get; set; }
        public float PrecisB { get; set; }

        // ← RIEN DE PLUS !
    }
}