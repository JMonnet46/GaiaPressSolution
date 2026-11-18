namespace GaiaPressBlazorWebAPP.Models
{
    public class TestDetailModel
    {
        // Identification
        public string Reference { get; set; } = string.Empty;
        public string NomEssai { get; set; } = string.Empty;
        public DateTime DateEssai { get; set; } = DateTime.Today;
        public DateTime HeureEssai { get; set; } = DateTime.Now;

        // Caractéristiques
        public float DiffPression { get; set; }
        public float HauteurControleur { get; set; }

        // Paramètres sol
        public float P0 { get; set; }
        public float ProfondeurNappe { get; set; }
        public float ProfondeurSonde { get; set; }
        public float PoidsVolumique { get; set; }
        public float PoidsVolumiqueDejauge { get; set; }
        public float CoefficientPoisson { get; set; }

        // Opérateurs
        public string Enregistreur { get; set; } = string.Empty;
        public string Operateur { get; set; } = string.Empty;

        // Précision
        public string ClassePrecision { get; set; } = "B";
        public float PrecisionLongueur { get; set; }
        public float PrecisionDiametre { get; set; }
        public float PrecisionPression { get; set; }
        public float PrecisionEpaisseur { get; set; }
        public float PrecisionVolume { get; set; }
        public float PrecisionPoidsVol { get; set; }
        public float PrecisionProfondeur { get; set; }
        public float PrecisionTemps { get; set; }
    }
}