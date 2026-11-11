using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeEssaiForage.vb.</summary>
    public class ListeEssaiForage : List<EssaiForage>
    {
        public ListeEssaiForage() { }
        public ListeEssaiForage(IEnumerable<EssaiForage> items) : base(items) { }
    }
}
