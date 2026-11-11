using GaiaPressCoreV17.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaiaPressCoreV17.Models
{
    /// <summary>Entité convertie depuis le modèle VB.NET d’origine.</summary>
    public class Chantier
    {
        /// <summary>Valeur métier refChantier.</summary>
        public string refChantier { get; set; }
        /// <summary>Valeur métier nomChantier.</summary>
        public string nomChantier { get; set; }
        /// <summary>Valeur métier descriptifChantier.</summary>
        public string descriptifChantier { get; set; }
        /// <summary>Valeur métier refClient.</summary>
        public int? refClient { get; set; }
        /// <summary>Valeur métier entreprise.</summary>
        public string entreprise { get; set; }
        /// <summary>Valeur métier adresse.</summary>
        public string adresse { get; set; }
        /// <summary>Valeur métier telephone.</summary>
        public string telephone { get; set; }
        /// <summary>Valeur métier reference.</summary>
        public string reference { get; set; }
        /// <summary>Valeur métier ListForages.</summary>
        public New List(Of Forage) ListForages { get; set; }
        /// <summary>Valeur métier ListEssaiForages.</summary>
        public New List(Of EssaiForage) ListEssaiForages { get; set; }
        /// <summary>Valeur métier ListMesuresForage.</summary>
        public New List(Of MesureEssaiForage) ListMesuresForage { get; set; }
        /// <summary>Valeur métier ListSols.</summary>
        public New List(Of Sol) ListSols { get; set; }
        /// <summary>Valeur métier ListSondes.</summary>
        public New List(Of Probe) ListSondes { get; set; }
        /// <summary>Valeur métier IsSaved.</summary>
        public Boolean = False IsSaved { get; set; }
        /// <summary>Valeur métier FilePath.</summary>
        public string FilePath { get; set; }
        /// <summary>Valeur métier FileTitle.</summary>
        public string FileTitle { get; set; }
        /// <summary>Valeur métier EventsSupendus.</summary>
        public bool EventsSupendus { get; set; }
        /// <summary>Valeur métier FichierChantier.</summary>
        public string FichierChantier { get; set; }
        /// <summary>Valeur métier MyFenetreChantier.</summary>
        public ChantierLogic MyFenetreChantier { get; set; }
        /// <summary>Valeur métier MyFile.</summary>
        public string MyFile { get; set; }
        /// <summary>Valeur métier MyFileTitle.</summary>
        public string MyFileTitle { get; set; }
        /// <summary>Valeur métier DataSetName.</summary>
        public string DataSetName { get; set; }
        [Key]
        /// <summary>Valeur métier Ind.</summary>
        public int Ind { get; set; }
        public List<Forage> ListForages { get; set; } = new();
        public List<EssaiForage> ListEssaiForages { get; set; } = new();
        public List<MesureEssaiForage> ListMesuresForage { get; set; } = new();
        public List<Sol> ListSols { get; set; } = new();
        public List<Probe> ListSondes { get; set; } = new();
        public event EventHandler? HasBeenModified;
        public Forage? GetForageParRef(string reference) => ListForages.Find(x => x.refForage == reference);
        public void AddForage(Forage forage) { if (forage != null) ListForages.Add(forage); HasBeenModified?.Invoke(this, EventArgs.Empty); }
        public void DeleteForage(string reference) { var item = GetForageParRef(reference); if (item != null) ListForages.Remove(item); }
    }
}
