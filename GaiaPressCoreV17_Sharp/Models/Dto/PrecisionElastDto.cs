using System.ComponentModel;

namespace GaiaPressCoreV17.Models.Dto
{
    /// <summary>
    /// DTO pour les variables de précision dans les calculs élastiques
    /// </summary>
    public class PrecisionElastDto
    {
        /// <summary>
        /// Précision maximale pour la pression (P)
        /// </summary>
        [Description("Précision maximale pour la pression (P)")]
        public float PMax { get; set; }

        [Description("Précision minimale pour la pression (P)")]
        public float PMin { get; set; }

        [Description("Précision maximale pour le volume Max (D)")]
        public float VMax { get; set; }

        [Description("Précision minimale pour le volume Min (D)")]
        public float VMin { get; set; }

        [Description("??")]
        public float DEm_Vs { get; set; }

        [Description("Module Ménard par rapport à V")]
        public float DEm_V { get; set; }

        [Description("??")]
        public float DEm_P { get; set; }

        [Description("Volume Limite")]
        public float VL { get; set; }

        [Description("Précision sur Volume Limite")]
        public float PrVl { get; set; }
    }
}