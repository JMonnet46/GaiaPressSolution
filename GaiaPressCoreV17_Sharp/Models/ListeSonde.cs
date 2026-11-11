using System;
using System.Collections.Generic;
using System.Linq;

namespace GaiaPressCoreV17.Models
{
    /// <summary>
    /// Collection métier de sondes (équivalent de ClasseListeSonde en VB)
    /// </summary>
    public class ListeSonde : List<Probe>
    {
        // ============================================
        // PROPRIÉTÉS
        // ============================================

        public bool IsSaved { get; private set; } = true;
        public bool EventsSuspendus { get; private set; } = false;

        public event EventHandler HasBeenModified;

        // ============================================
        // INDEXEUR (accès par index, comme en VB)
        // ============================================

        public new Probe this[int index]
        {
            get => base[index];
            set
            {
                base[index] = value;
                value.MyListe = this; // Lie la sonde à cette liste
                IHaveBeenModified();
            }
        }

        // ============================================
        // CONSTRUCTEUR
        // ============================================

        public ListeSonde() : base()
        {
        }

        public ListeSonde(IEnumerable<Probe> sondes) : base(sondes)
        {
            foreach (var sonde in this)
            {
                sonde.MyListe = this;
            }
        }

        // ============================================
        // MÉTHODES DE GESTION
        // ============================================

        public void Ajouter(Probe sonde)
        {
            sonde.MyListe = this;
            Add(sonde);
            IHaveBeenModified();
        }

        public void Supprimer(Probe sonde)
        {
            Remove(sonde);
            IHaveBeenModified();
        }

        public void SupprimerParIndex(int index)
        {
            if (index >= 0 && index < Count)
            {
                RemoveAt(index);
                IHaveBeenModified();
            }
        }

        // ============================================
        // RECHERCHE
        // ============================================

        public Probe GetByRef(string refSonde)
        {
            return this.FirstOrDefault(p => p.RefSonde == refSonde);
        }

        public Probe GetByIndex(int index)
        {
            return (index >= 0 && index < Count) ? this[index] : null;
        }

        public int GetIndexByRef(string refSonde)
        {
            return FindIndex(p => p.RefSonde == refSonde);
        }

        // ============================================
        // VALIDATION
        // ============================================

        public bool IsRefSondeUnique(string refSonde)
        {
            return !this.Any(p => p.RefSonde == refSonde);
        }

        public bool IsRefEssaiUnique(string refEssai, int typeEssai)
        {
            return !this.SelectMany(p => typeEssai == 0 ? p.Resistance.Cast<object>() : p.Expansion.Cast<object>())
                        .Any(e => GetRefEssai(e) == refEssai);
        }

        private string GetRefEssai(object essai)
        {
            // À adapter selon le type réel
            return essai?.ToString() ?? string.Empty;
        }

        // ============================================
        // GESTION DES ÉVÉNEMENTS
        // ============================================

        public void IHaveBeenModified()
        {
            if (!EventsSuspendus)
            {
                IsSaved = false;
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
        // SAUVEGARDE (déléguée au service)
        // ============================================

        public void MarquerToutesSauvegardees()
        {
            foreach (var sonde in this)
            {
                sonde.IsSaved = true;
            }
            IsSaved = true;
        }
    }
}