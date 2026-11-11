using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models.Dto
{
    public class EssaiForageDto
    {
        public string refChantier { get; set; }
        public string refForage { get; set; }
        public string refEssai { get; set; }
        public string refSonde { get; set; }
        public string refEssaiResistancePropre { get; set; }
        public string refEssaiExpansionPropre { get; set; }
        public string refEssaiPerteDeCharge { get; set; 
        public string refSol { get; set; }
        public string Enregistreur { get; set; }
        public string Operateur { get; set; }
        public float VolSonde { get; set; }
        public int ClassPrecis { get; set; }
        public float PrecisVolSonde { get; set; }
        public string nomEssai { get; set; }
        public DateTime dateEssai { get; set; }
        public DateTime HeureTime { get; set; }
        public float difPresEauAir { get; set; }
        public float hauteurCpv { get; set; }
        public float presP0 { get; set; }
        public float presFluage { get; set; }
        public float presLimite { get; set; }
        public float presLimiteNf { get; set; }
        public float presLimiteTheo { get; set; }
        public float PrecisVolume { get; set; }
        public float PrecisPression { get; set; }
        public float poidsVol { get; set; }
        public float poidsVolDejauge { get; set; }
        public float coefPoisson { get; set; }
        public float modPressio { get; set; }
        public float modPressioNf  { get; set; }
        public float modElastique { get; set; }
        public float modElastiqueNf { get; set; }
        public float cohesion { get; set; }
        public float frottement { get; set; }
        public float frottementintergranulaire { get; set; }
        public float U0 { get; set; }
        public float profNappe { get; set; }
        public float profSonde { get; set; }
        public float coteZeroGeomeca { get; set; }

        // ← RIEN DE PLUS !***
    }
}