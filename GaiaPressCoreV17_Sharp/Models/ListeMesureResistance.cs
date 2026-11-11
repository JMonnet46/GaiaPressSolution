using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeMesureResistance.vb.</summary>
    public class ListeMesureResistance : List<MesureResistance>
    {
        public ListeMesureResistance() { }
        public ListeMesureResistance(IEnumerable<MesureResistance> items) : base(items) { }
    }
}
