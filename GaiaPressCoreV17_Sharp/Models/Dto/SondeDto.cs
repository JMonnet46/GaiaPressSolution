namespace GaiaPressCoreV17.Models.Dto
{
    public class SondeDto
    {
        public string refSonde { get; set; }
        public int typeSonde { get; set; }
        public int typeDeformee { get; set; }
        public int typeContrainte { get; set; }
        public float ls { get; set; }
        public float lg { get; set; }
        public float lt { get; set; }
        public float longInjectPompage { get; set; }
        public float Vs { get; set; }
        public float ds { get; set; }
        public int ClassPrecis { get; set; }
        public float epMembraneGarde { get; set; }
        public float PrEpMembraneGarde { get; set; }
        public float epMembraneMesure { get; set; }
        public float PrEpMembraneMesure { get; set; }
        public float epTubeFendu { get; set; }
        public float PrEpTubeFendu { get; set; }
        public float epaisInitMembrane { get; set; }
        public float PrEpaisInitMembrane { get; set; }
        public float PrLong { get; set; }
        public float PrDiam { get; set; }
        public float PrP { get; set; }
        public float PrV { get; set; }
        public float PrPV { get; set; }
        public float PrZ { get; set; }
        public float PrT { get; set; }

        // ← RIEN DE PLUS !
    }
}