using System;
using System.Collections.Generic;
using System.Linq;
using GaiaPressCoreV17.Models;

namespace GaiaPressBlazorWebAPP.Charting  // ← Vérifier la casse exacte (APP ou App)
{
    /// <summary>
    /// Génère des données de graphique pour les essais de forage (Pressiomètre).
    /// </summary>
    public class EssaiChartGenerator
    {
        /// <summary>
        /// Génère les données pour le graphique Pression vs Volume.
        /// </summary>
        public Dictionary<string, List<double>> GeneratePressureVolumeChart(Essai_Forage essai)
        {
            var data = new Dictionary<string, List<double>>();

            // Série Pression (bar ou kPa selon l'unité de l'essai)
            data.Add("Pression (bar)", new List<double>
            {
                essai.presP0,        // Pression initiale
                essai.presFluage,    // Pression de fluage
                essai.presLimite     // Pression limite
            });

            // Série Volume (cm³ ou m³ selon l'unité)
            data.Add("Volume (cm³)", new List<double>
            {
                essai.VolSonde,           // Volume initial sonde
                essai.VolSonde * 1.2,     // Volume à 20% de plus (exemple)
                essai.VolSonde * 1.5      // Volume à 50% de plus (exemple)
            });

            return data;
        }

        /// <summary>
        /// Génère les données pour la courbe de fluage (Volume en fonction du temps).
        /// </summary>
        /// <returns>Tuple contenant les valeurs X (temps en secondes) et Y (volume en cm³)</returns>
        public (double[] xValues, double[] yValues) GenerateCreepCurve(Essai_Forage essai)
        {
            // TODO: Remplacer par les vraies mesures de l'essai si disponibles
            // Exemple avec données simulées basées sur la pression initiale

            var xValues = new double[] { 0, 15, 30, 60, 120, 180 };  // Temps en secondes (15s, 30s, 60s, 2min, 3min)

            var yValues = new double[]
            {
                essai.VolSonde,                    // Volume initial (t=0)
                essai.VolSonde * 1.05,             // +5% à 15s
                essai.VolSonde * 1.10,             // +10% à 30s
                essai.VolSonde * 1.15,             // +15% à 60s
                essai.VolSonde * 1.20,             // +20% à 2min
                essai.VolSonde * 1.25              // +25% à 3min
            };

            return (xValues, yValues);
        }

        /// <summary>
        /// Génère les données pour le graphique Pression vs Volume corrigé (NF).
        /// </summary>
        public Dictionary<string, List<double>> GeneratePressureVolumeNFChart(Essai_Forage essai)
        {
            var data = new Dictionary<string, List<double>>();

            // TODO: Ajouter les propriétés NF si elles existent sur Essai_Forage
            // Exemple:
            // data.Add("Pression NF (kPa)", new List<double> { essai.E_PNF_0, essai.E_PNF_fluage, essai.E_PNF_limite });
            // data.Add("Volume NF (cm³)", new List<double> { essai.E_VNF_0, essai.E_VNF_fluage, essai.E_VNF_limite });

            return data;
        }
    }
}
