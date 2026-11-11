using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>Entité convertie depuis le modèle VB.NET d’origine.</summary>
    public class EssaiResistance
    {
        [Key]
        /// <summary>Valeur métier refEssai.</summary>
        public string refEssai { get; set; }
        /// <summary>Valeur métier refSonde.</summary>
        public string refSonde { get; set; }
        /// <summary>Valeur métier ClassPrecis.</summary>
        public int ClassPrecis { get; set; }
        /// <summary>Valeur métier nomEssai.</summary>
        public string nomEssai { get; set; }
        /// <summary>Valeur métier dateEssai.</summary>
        public DateTime dateEssai { get; set; }
        /// <summary>Valeur métier typeEssai.</summary>
        public float typeEssai { get; set; }
        /// <summary>Valeur métier difPresEauAir.</summary>
        public float difPresEauAir { get; set; }
        /// <summary>Valeur métier hauteurCpv.</summary>
        public float hauteurCpv { get; set; }
        /// <summary>Valeur métier A0.</summary>
        public float A0 { get; set; }
        /// <summary>Valeur métier A1.</summary>
        public float A1 { get; set; }
        /// <summary>Valeur métier A2.</summary>
        public float A2 { get; set; }
        /// <summary>Valeur métier A3.</summary>
        public float A3 { get; set; }
        /// <summary>Valeur métier Pel.</summary>
        public float Pel { get; set; }
        /// <summary>Valeur métier Pe.</summary>
        public float Pe { get; set; }
        /// <summary>Valeur métier MySondeResistance.</summary>
        public Probe MySondeResistance { get; set; }
        /// <summary>Valeur métier MesureCount.</summary>
        public int MesureCount { get; set; }
        /// <summary>Valeur métier MySonde.</summary>
        public Probe MySonde { get; set; }
        /// <summary>Valeur métier MesuresResistanceDb.</summary>
        public New List(Of MesureResistance) MesuresResistanceDb { get; set; }
        /// <summary>Valeur métier Mesures.</summary>
        public List<esureResistance> Mesures { get; set; } = new List<esureResistance>();
        /// <summary>Valeur métier MesuresResistanceDb.</summary>
        public ICollection(Of MesureResistance) = New List(Of MesureResistance) MesuresResistanceDb { get; set; }
        public List<MesureResistance> MesuresResistanceDb { get; set; } = new();
        public List<MesureResistance> Mesures => MesuresResistanceDb;
        public int GetMesureCount() => MesuresResistanceDb.Count;
        public void AddMesure(MesureResistance mesure) { if (mesure != null) MesuresResistanceDb.Add(mesure); }
    }
}
