namespace GaiaPressCoreV17.Models.Dto
{
    public class MesurePerteDeChargeDto
    {
        public int Ind { get; set; }
        public string refEssai { get; set; }
        public int indiceMesure { get; set; }
        public float debitLitreMin { get; set; }
        public float debitLitreHeure { get; set; }
        public float pi { get; set; }
        public float pp { get; set; }
    }
}