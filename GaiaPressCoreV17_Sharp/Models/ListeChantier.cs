using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeChantier.vb.</summary>
    public class ListeChantier : List<Chantier>
    {
        public ListeChantier() { }
        public ListeChantier(IEnumerable<Chantier> items) : base(items) { }
    }
}
