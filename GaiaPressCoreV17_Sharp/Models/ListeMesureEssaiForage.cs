using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeMesureEssaiForage.vb.</summary>
    public class ListeMesureEssaiForage : List<MesureEssaiForage>
    {
        public ListeMesureEssaiForage() { }
        public ListeMesureEssaiForage(IEnumerable<MesureEssaiForage> items) : base(items) { }
    }
}
