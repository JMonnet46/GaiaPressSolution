using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeForage.vb.</summary>
    public class ListeForage : List<Forage>
    {
        public ListeForage() { }
        public ListeForage(IEnumerable<Forage> items) : base(items) { }
    }
}
