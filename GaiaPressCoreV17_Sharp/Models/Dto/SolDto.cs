namespace GaiaPressCoreV17.Models.Dto
{
    public class SolDto
    {
        public string refSol { get; set; }
        public string nomSol { get; set; }
        public string zoneGeo { get; set; }
        public string famille { get; set; }
        public string ClasseSol { get; set; }
        public int SousClasse { get; set; }
        public string classeInférieure { get; set; }
        public int sousClasseInférieure { get; set; }
        public string nomSolNormalise { get; set; }

        public float dmax { get; set; }
        public float tamisat50mmdmax { get; set; }
        public float tamisat2mm { get; set; }
        public float tamisat80microns { get; set; }
        public float VBS { get; set; }
        public float IP { get; set; }
        public float Es { get; set; }
        public float phiMu { get; set; }
    }
}