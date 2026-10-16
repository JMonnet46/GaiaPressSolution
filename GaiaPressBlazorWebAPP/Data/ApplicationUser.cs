using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace GaiaPressBlazorWebAPP.Data
{                                          // ← Ouverture du namespace
    public class ApplicationUser : IdentityUser
    {                                      // ← Ouverture de la classe
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }                                      // ← Fermeture de la classe
}                                          // ← Fermeture du namespace
