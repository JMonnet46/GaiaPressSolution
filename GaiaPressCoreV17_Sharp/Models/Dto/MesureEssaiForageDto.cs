namespace GaiaPressCoreV17.Models.Dto
{
    public class MesureEssaiForageDto
    {
        public int Ind { get; set; }
        public string refEssai { get; set; }
        public int indiceMesure { get; set; }
        public float pression { get; set; }
        public float volume15 { get; set; }
        public float volume30 { get; set; }
        public float volume60 { get; set; }
        public float contrainteRadiale { get; set; }
        public float contrainteVerticale { get; set; }
        public float deformation { get; set; }
        public float PNF { get; set; }
        public float VNF { get; set; }
        public float Qi { get; set; }
        public float Pi { get; set; }
        public float Qp { get; set; }
        public float Pp { get; set; }


        // ← RIEN DE PLUS !
    }
}