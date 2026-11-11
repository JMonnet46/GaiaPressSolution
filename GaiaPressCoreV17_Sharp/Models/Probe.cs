using GaiaPressCoreV17.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace GaiaPressCoreV17.Models
{
    /// <summary>
    /// Représente une sonde de mesure géotechnique.
    /// Correspond à la table 'LesSondes' en base de données.
    /// </summary>
    [Table("LesSondes")]
    public class Probe
    {
        // ============================================
        // CHAMPS PERSISTÉS EN BD (table LesSondes)
        // ============================================

        [Key]
        [Column("refSonde")]
        [StringLength(50)]
        public string RefSonde { get; set; }

        [Column("typeSonde")]
        public int TypeSonde { get; set; } = 0;

        [Column("typeDeformee")]
        public int TypeDeformee { get; set; } = 0;

        [Column("typeContrainte")]
        public int TypeContrainte { get; set; } = 0;

        [Column("ls")]
        public float Ls { get; set; }

        [Column("lg")]
        public float Lg { get; set; }

        [Column("lt")]
        public float Lt { get; set; }

        [Column("longInjectPompage")]
        public float LongInjectPompage { get; set; }

        [Column("Vs")]
        public float Vs { get; set; }

        [Column("ds")]
        public float Ds { get; set; }

        [Column("Dcalib")]
        public float Dcalib { get; set; } = 65f; // Valeur par défaut : 65 mm

        [Column("ClassPrecis")]
        public int ClassPrecis { get; set; }

        [Column("epMembraneGarde")]
        public float EpMembraneGarde { get; set; }

        [Column("PrEpMembraneGarde")]
        public float PrEpMembraneGarde { get; set; }

        [Column("epMembraneMesure")]
        public float EpMembraneMesure { get; set; }

        [Column("PrEpMembraneMesure")]
        public float PrEpMembraneMesure { get; set; }

        [Column("epTubeFendu")]
        public float EpTubeFendu { get; set; }

        [Column("PrEpTubeFendu")]
        public float PrEpTubeFendu { get; set; }

        [Column("epaisInitMembrane")]
        public float EpaisInitMembrane { get; set; }

        [Column("PrEpaisInitMembrane")]
        public float PrEpaisInitMembrane { get; set; }

        [Column("PrLong")]
        public float PrLong { get; set; }

        [Column("PrDiam")]
        public float PrDiam { get; set; }

        [Column("PrP")]
        public float PrP { get; set; }

        [Column("PrV")]
        public float PrV { get; set; }

        [Column("PrPV")]
        public float PrPV { get; set; }

        [Column("PrZ")]
        public float PrZ { get; set; }

        [Column("PrT")]
        public float PrT { get; set; }

        [Column("Pm")]
        public float Pm { get; set; }

        // ============================================
        // PROPRIÉTÉS MÉTIER (non persistées en BD)
        // ============================================

        [NotMapped]
        public string NomSonde { get; set; }

        [NotMapped]
        public List<EssaiResistance> Resistance { get; set; } = new List<EssaiResistance>();

        [NotMapped]
        public List<EssaiExpansion> Expansion { get; set; } = new List<EssaiExpansion>();

        [NotMapped]
        public EssaiExpansion MaExpansion { get; set; }

        [NotMapped]
        public EssaiResistance MaResistance { get; set; }

        [NotMapped]
        public ListeSonde MyListe { get; set; } // Référence vers la liste parente (ex-MyListeSonde/MaListeSonde)

        [NotMapped]
        public bool IsSaved { get; set; } = true;

        [NotMapped]
        public bool EventsSuspendus { get; private set; } = false;

        // ============================================
        // PROPRIÉTÉS CALCULÉES
        // ============================================

        [NotMapped]
        public int ResistanceCount => Resistance?.Count ?? 0;

        [NotMapped]
        public int ExpansionCount => Expansion?.Count ?? 0;

        // ============================================
        // ÉVÉNEMENTS
        // ============================================

        [NotMapped]
        public event EventHandler HasBeenModified;

        // ============================================
        // CONSTRUCTEUR
        // ============================================

        public Probe()
        {
            // Valeurs par défaut
            RefSonde = string.Empty;
            TypeSonde = 0;
            TypeDeformee = 0;
            TypeContrainte = 0;
            Dcalib = 65f;
            IsSaved = true;
        }

        // ============================================
        // MÉTHODES MÉTIER
        // ============================================

        /// <summary>
        /// Convertit une chaîne en Single (équivalent de Valeurs() en VB)
        /// </summary>
        public static float ConvertirEnSingle(string valeur)
        {
            return Convert.ToSingle(valeur, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Réinitialise la sonde avec les valeurs par défaut
        /// </summary>
        public void ResetToDefaults()
        {
            TypeSonde = 0;
            TypeDeformee = 0;
            TypeContrainte = 0;
            Ls = 0;
            Lg = 0;
            Lt = 0;
            LongInjectPompage = 0;
            Vs = 0;
            Ds = 0;
            Dcalib = 65f;
            ClassPrecis = 0;
            EpMembraneGarde = 0;
            PrEpMembraneGarde = 0;
            EpMembraneMesure = 0;
            PrEpMembraneMesure = 0;
            EpTubeFendu = 0;
            PrEpTubeFendu = 0;
            EpaisInitMembrane = 0;
            PrEpaisInitMembrane = 0;
            PrLong = 0;
            PrDiam = 0;
            PrP = 0;
            PrV = 0;
            PrPV = 0;
            PrZ = 0;
            PrT = 0;
            Pm = 0;
            IsSaved = false;
            IHaveBeenModified();
        }

        /// <summary>
        /// Calcule le volume de la sonde (équivalent de CalculeVol en VB)
        /// </summary>
        public void CalculeVol()
        {
            // Formule de calcul du volume à adapter selon la logique métier
            // Exemple : Vs = f(Ls, Lg, Ds, etc.)
            IsSaved = false;
            IHaveBeenModified();
        }

        /// <summary>
        /// Calcule le volume non filtré (équivalent de CalculeVolNF en VB)
        /// </summary>
        public void CalculeVolNF()
        {
            // Formule spécifique à définir
            IsSaved = false;
            IHaveBeenModified();
        }

        // ============================================
        // GETTERS (compatibilité avec l'ancien code VB)
        // ============================================

        public string GetRefSonde() => RefSonde;
        public float GetPrEpMembraneGarde() => PrEpMembraneGarde;
        public int GetTypeDeformee() => TypeDeformee;
        public int GetTypeContrainte() => TypeContrainte;
        public float GetDs() => Ds;
        public ListeSonde GetMyListe() => MyListe;
        public bool GetIsSaved() => IsSaved;

        public int GetEssaiCount(int typeEssai)
        {
            return typeEssai switch
            {
                0 => ResistanceCount,  // EssaiCalib.Resistance
                1 => ExpansionCount,   // EssaiCalib.Expansion
                _ => 0
            };
        }

        // ============================================
        // SETTERS (avec notification de modification)
        // ============================================

        public void SetRefSonde(string refSonde)
        {
            RefSonde = refSonde;
            IHaveBeenModified();
        }

        public void SetPrDiam(float pr)
        {
            PrDiam = pr;
            IHaveBeenModified();
        }

        public void SetPrEpMembraneGarde(float pr)
        {
            PrEpMembraneGarde = pr;
            IHaveBeenModified();
        }

        public void SetPrEpTubeFendu(float pr)
        {
            PrEpTubeFendu = pr;
            IHaveBeenModified();
        }

        public void SetPrEpMembraneMesure(float pr)
        {
            PrEpMembraneMesure = pr;
            IHaveBeenModified();
        }

        public void SetClassPrecis(int p)
        {
            ClassPrecis = p;
            IHaveBeenModified();
        }

        public void SetMyListe(ListeSonde liste)
        {
            MyListe = liste;
        }

        // ============================================
        // GESTION DES ÉVÉNEMENTS
        // ============================================

        public void IHaveBeenModified()
        {
            if (!EventsSuspendus)
            {
                IsSaved = false;
                MyListe?.IHaveBeenModified();
                HasBeenModified?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SuspendreEvents()
        {
            EventsSuspendus = true;
        }

        public void RetablirEvents()
        {
            EventsSuspendus = false;
        }

        // ============================================
        // MÉTHODES DE NOMENCLATURE (i18n)
        // ============================================

        public string GetNomTypeSonde(int lang = 1)
        {
            // lang: 0 = Anglais, 1 = Français
            return TypeSonde switch
            {
                0 => lang == 0 ? "Soft Type E" : "Souple Type E",
                1 => lang == 0 ? "Long Slotted Tube" : "Tube fendu, longue",
                _ => lang == 0 ? "Unknown" : "Inconnu"
            };
        }

        public string GetNomTypeDeformee(int lang = 1)
        {
            return TypeDeformee switch
            {
                0 => lang == 0 ? "Rugby ball" : "Ballon Rugby",
                1 => lang == 0 ? "Slotted tube" : "Tube Lanterné",
                _ => lang == 0 ? "Unknown" : "Inconnu"
            };
        }

        // ============================================
        // MÉTHODES DE GESTION DES ESSAIS
        // ============================================

        public void GetListeResistanceParRef(string refSonde)
        {
            // À implémenter avec le service de lecture
            // Resistance = _readService.GetEssaisResistanceBySonde(refSonde);
        }

        public void GetListeExpansionParRef(string refSonde)
        {
            // À implémenter avec le service de lecture
            // Expansion = _readService.GetEssaisExpansionBySonde(refSonde);
        }

        public void GetListeResistanceExpansionParRef(string refSonde, int typeEssai)
        {
            switch (typeEssai)
            {
                case 0: // Resistance
                    GetListeResistanceParRef(refSonde);
                    break;
                case 1: // Expansion
                    GetListeExpansionParRef(refSonde);
                    break;
            }
        }

        public object SetEssai(int typeEssai, object essai, int indice)
        {
            // À implémenter selon la logique métier
            IHaveBeenModified();
            return essai;
        }

        public void DeleteEssai(int typeEssai, int index)
        {
            switch (typeEssai)
            {
                case 0: // Resistance
                    if (index >= 0 && index < Resistance.Count)
                        Resistance.RemoveAt(index);
                    break;
                case 1: // Expansion
                    if (index >= 0 && index < Expansion.Count)
                        Expansion.RemoveAt(index);
                    break;
            }
            IHaveBeenModified();
        }

        public string GiveMeANewRefEssai(int typeEssai)
        {
            string prefix = typeEssai == 0 ? "Resistance" : "Expansion";
            int k = 1;
            string newRef;
            do
            {
                newRef = $"{prefix}{k:D3}";
                k++;
            } while (MyListe != null && !MyListe.IsRefEssaiUnique(newRef, typeEssai));

            return newRef;
        }

        // ============================================
        // MÉTHODES POUR L'AFFICHAGE HTML
        // ============================================

        public List<Dictionary<string, object>> GetEssaisForHtmlDisplay(int typeEssai)
        {
            var essais = new List<Dictionary<string, object>>();

            var liste = typeEssai == 0 ? Resistance.Cast<object>().ToList() : Expansion.Cast<object>().ToList();

            foreach (var essai in liste)
            {
                var dict = new Dictionary<string, object>();
                // Remplir selon le type d'essai
                essais.Add(dict);
            }

            return essais;
        }
    }
}