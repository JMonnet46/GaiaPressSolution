using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeResistance.vb.</summary>
    public class ListeResistance : List<EssaiResistance>
    {
        public ListeResistance() { }
        public ListeResistance(IEnumerable<EssaiResistance> items) : base(items) { }
    }
}
