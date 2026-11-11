using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeMesureExpansion.vb.</summary>
    public class ListeMesureExpansion : List<MesureExpansion>
    {
        public ListeMesureExpansion() { }
        public ListeMesureExpansion(IEnumerable<MesureExpansion> items) : base(items) { }
    }
}
