using System.ComponentModel.DataAnnotations;

namespace GaiaPressBlazorWebAPP.Models
{
    public class ConstructionSiteDto
    {
        [Required(ErrorMessage = "La référence du chantier est obligatoire.")]
        public string RefChantier { get; set; }

        [Required(ErrorMessage = "Le nom du chantier est obligatoire.")]
        public string Nom { get; set; }

        // Ajoutez ici d'autres propriétés si nécessaire
        // Exemple :
        // public string Adresse { get; set; }
        // public DateTime DateDebut { get; set; }
    }
}