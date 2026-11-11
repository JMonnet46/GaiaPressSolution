using System.Collections.Generic;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Collections
{
    /// <summary>Collection typée issue de ClasseListeSol.vb.</summary>
    public class ListeSol : List<Sol>
    {
        public ListeSol() { }
        public ListeSol(IEnumerable<Sol> items) : base(items) { }
    }
}
