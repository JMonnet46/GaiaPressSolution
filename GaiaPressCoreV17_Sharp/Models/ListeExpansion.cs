using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeExpansion.vb.</summary>
    public class ListeExpansion : List<EssaiExpansion>
    {
        public ListeExpansion() { }
        public ListeExpansion(IEnumerable<EssaiExpansion> items) : base(items) { }
    }
}
