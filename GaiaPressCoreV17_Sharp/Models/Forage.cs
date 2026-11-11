using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GaiaPressCoreV17.Models
{
    public class Forage
    {
        public List<EssaiForage> ListEssais { get; set; } = new List<EssaiForage>();
        public string refChantier { get; set; }
        [Key] public string refForage { get; set; }
        public int numeroForage { get; set; }
        public string NomForage { get; set; }
        public DateTime dateForage { get; set; }
        public string materiel { get; set; }
        public string outil { get; set; }
        public float abscisseX { get; set; }
        public float ordonneeY { get; set; }
        public float altitudeZ { get; set; }
        public float coteInit { get; set; }
        public float coteFinale { get; set; }
        public bool EventsSuspendus { get; set; }
        public bool IsSaved { get; set; }
        public string FichierChantier { get; set; }
        public Chantier Chantier { get; set; }

        public event EventHandler HasBeenModified;

        public void IHaveBeenModified()
        {
            if (!EventsSuspendus)
            {
                HasBeenModified?.Invoke(this, EventArgs.Empty);
                IsSaved = false;
            }
        }

        public string GetrefForage() => refForage;
        public int GetNumeroForage() => numeroForage;
        public string GetNomForage() => NomForage;
        public DateTime GetDateForage() => dateForage;
        public string GetMateriel() => materiel;
        public string GetOutil() => outil;
        public float GetabscisseX() => abscisseX;
        public float GetOrdonneeY() => ordonneeY;
        public float GetAltitudeZ() => altitudeZ;
        public float GetCoteInit() => coteInit;
        public float GetCoteFinale() => coteFinale;

        public Forage GetForageParRef(string refForage)
        {
            return Chantier == null ? null : Chantier.GetForageParRef(refForage);
        }

        public void SetNomForage(string nom) { NomForage = nom; IHaveBeenModified(); }
        public void SetrefForage(string @ref) { refForage = @ref; }
        public void SetNumeroForage(int numero) { numeroForage = numero; IHaveBeenModified(); }
        public void SetDateForage(string dateFora) { dateForage = DateTime.Parse(dateFora); IHaveBeenModified(); }
        public void SetMateriel(string typeFora) { materiel = typeFora; IHaveBeenModified(); }
        public void SetOutil(string outi) { outil = outi; IHaveBeenModified(); }
        public void SetAbscisseX(float abscisse) { abscisseX = abscisse; IHaveBeenModified(); }
        public void SetOrdonneeY(float ordonnee) { ordonneeY = ordonnee; IHaveBeenModified(); }
        public void SetAltitudeZ(float altitude) { altitudeZ = altitude; IHaveBeenModified(); }
        public void SetCoteInit(float cote) { coteInit = cote; IHaveBeenModified(); }
        public void SetCoteFinale(float cote) { coteFinale = cote; IHaveBeenModified(); }

        public void UpdateEssaiForage(EssaiForage essai)
        {
            for (int i = 0; i < ListEssais.Count; i++)
            {
                if (ListEssais[i].refEssai == essai.refEssai)
                {
                    ListEssais[i] = essai;
                    break;
                }
            }
        }
    }
}
