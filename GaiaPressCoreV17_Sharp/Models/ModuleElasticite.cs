using System;
using GaiaPressCoreV17.Models;

namespace GaiaPressCoreV17.Services
{
    /// <summary>Calculs du module d’élasticité.</summary>
    public class ModuleElasticite
    {
        public static double Calculer(double pression, double volume) => volume == 0 ? 0 : pression / volume;
    }
}
