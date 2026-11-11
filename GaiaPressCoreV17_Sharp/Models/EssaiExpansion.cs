using GaiaPressCoreV17.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>Entité convertie depuis le modèle VB.NET d’origine.</summary>
    public class EssaiExpansion
    {
        [Key]
        /// <summary>Valeur métier refEssai.</summary>
        public string refEssai { get; set; }
        /// <summary>Valeur métier refSonde.</summary>
        public string refSonde { get; set; }
        /// <summary>Valeur métier ClassPrecis.</summary>
        public int ClassPrecis { get; set; }
        /// <summary>Valeur métier typeEssai.</summary>
        public string typeEssai { get; set; }
        /// <summary>Valeur métier nomEssai.</summary>
        public string nomEssai { get; set; }
        /// <summary>Valeur métier dateEssai.</summary>
        public DateTime dateEssai { get; set; }
        /// <summary>Valeur métier difPresEauAir.</summary>
        public float difPresEauAir { get; set; }
        /// <summary>Valeur métier hauteurCpv.</summary>
        public float hauteurCpv { get; set; }
        /// <summary>Valeur métier Di.</summary>
        public float Di { get; set; }
        /// <summary>Valeur métier Vc.</summary>
        public float Vc { get; set; }
        /// <summary>Valeur métier Vs.</summary>
        public float Vs { get; set; }
        /// <summary>Valeur métier EpMembrane.</summary>
        public float EpMembrane { get; set; }
        /// <summary>Valeur métier Prp.</summary>
        public float Prp { get; set; }
        /// <summary>Valeur métier PrV.</summary>
        public float PrV { get; set; }
        /// <summary>Valeur métier coefficientA.</summary>
        public float coefficientA { get; set; }
        /// <summary>Valeur métier coefficientB.</summary>
        public float coefficientB { get; set; }
        /// <summary>Valeur métier PrecisVc.</summary>
        public float PrecisVc { get; set; }
        /// <summary>Valeur métier PrecisVs.</summary>
        public float PrecisVs { get; set; }
        /// <summary>Valeur métier PrEpMembrane.</summary>
        public float PrEpMembrane { get; set; }
        /// <summary>Valeur métier PrecisA.</summary>
        public float PrecisA { get; set; }
        /// <summary>Valeur métier PrecisB.</summary>
        public float PrecisB { get; set; }
        /// <summary>Valeur métier MesureCount.</summary>
        public int MesureCount { get; set; }
        /// <summary>Valeur métier MySonde.</summary>
        public Probe MySonde { get; set; }
        /// <summary>Valeur métier ListeMesureExpansion.</summary>
        public List<esureExpansion) = New List(Of MesureExpansion)(> ListeMesureExpansion { get; set; } = new List<esureExpansion) = New List(Of MesureExpansion)(>();
        public List<MesureExpansion> ListeMesureExpansion { get; set; } = new();
        public int GetMesureCount() => ListeMesureExpansion.Count;
        public void AddMesureExpansion(MesureExpansion mesure) { if (mesure != null) ListeMesureExpansion.Add(mesure); }
    }
}
