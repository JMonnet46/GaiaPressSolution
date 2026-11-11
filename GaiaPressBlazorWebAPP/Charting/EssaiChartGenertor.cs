using System;
using System.Collections.Generic;
using System.Linq;
using GaiaPressCoreV17.Models;

namespace GaiaPressBlazorWebAPP.Charting  // ← Vérifier la casse exacte (APP ou App)
{
    /// <summary>
    /// Génère des données de graphique pour les Tests de forage (Pressiomètre).
    /// </summary>
    public class TestChartGenerator
    {
        /// <summary>
        /// Génère les données pour le graphique Pression vs Volume.
        /// </summary>
        public Dictionary<string, List<double>> GeneratePressureVolumeChart(Essai_Forage Test)
        {
            var data = new Dictionary<string, List<double>>();

            // Série Pression (bar ou kPa selon l'unité de l'Test)
            data.Add("Pression (bar)", new List<double>
            {
                Test.presP0,        // Pression initiale
                Test.presFluage,    // Pression de fluage
                Test.presLimite     // Pression limite
            });

            // Série Volume (cm³ ou m³ selon l'unité)
            data.Add("Volume (cm³)", new List<double>
            {
                Test.VolSonde,           // Volume initial sonde
                Test.VolSonde * 1.2,     // Volume à 20% de plus (exemple)
                Test.VolSonde * 1.5      // Volume à 50% de plus (exemple)
            });

            return data;
        }

        /// <summary>
        /// Génère les données pour la courbe de fluage (Volume en fonction du temps).
        /// </summary>
        /// <returns>Tuple contenant les valeurs X (temps en secondes) et Y (volume en cm³)</returns>
        public (double[] xValues, double[] yValues) GenerateCreepCurve(Essai_Forage Test)
        {
            // TODO: Remplacer par les vraies mesures de l'Test si disponibles
            // Exemple avec données simulées basées sur la pression initiale

            var xValues = new double[] { 0, 15, 30, 60, 120, 180 };  // Temps en secondes (15s, 30s, 60s, 2min, 3min)

            var yValues = new double[]
            {
                Test.VolSonde,                    // Volume initial (t=0)
                Test.VolSonde * 1.05,             // +5% à 15s
                Test.VolSonde * 1.10,             // +10% à 30s
                Test.VolSonde * 1.15,             // +15% à 60s
                Test.VolSonde * 1.20,             // +20% à 2min
                Test.VolSonde * 1.25              // +25% à 3min
            };

            return (xValues, yValues);
        }

        /// <summary>
        /// Génère les données pour le graphique Pression vs Volume corrigé (NF).
        /// </summary>
        public Dictionary<string, List<double>> GeneratePressureVolumeNFChart(Essai_Forage Test)
        {
            var data = new Dictionary<string, List<double>>();

            // TODO: Ajouter les propriétés NF si elles existent sur Test_Forage
            // Exemple:
            // data.Add("Pression NF (kPa)", new List<double> { Test.E_PNF_0, Test.E_PNF_fluage, Test.E_PNF_limite });
            // data.Add("Volume NF (cm³)", new List<double> { Test.E_VNF_0, Test.E_VNF_fluage, Test.E_VNF_limite });

            return data;
        }
    }
}
