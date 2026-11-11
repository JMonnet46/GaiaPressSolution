using 
using System;
[PROJET].Models;
using [PROJET].Logic;

namespace [PROJET].Logic
{
    public static class ModCohesion
    {
        /// <summary>
        /// Convertit les degrés en radians
        /// </summary>
        private static float DTOR(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }

        /// <summary>
        /// Calcule la pression limite (Plim) et la pression de fluage (Pf)
        /// </summary>
        private static Cisaillement CalculPlim(
            float pente,
            float nu,
            float cTanPhi,
            float eelast,
            float p0,
            float phimu,
            float dext0,
            float k0,
            float sz,
            float coh,
            float pmax,
            int ndp)
        {
            float uap, ub, phi;
            float plimTheo, pfTheo, ubsB, c1, mu, a, b;

            var result = new Cisaillement();

            a = dext0 / 200f;
            mu = eelast / 2f / (1f + nu);

            // Note : En VB, 'Phi' n'est jamais assigné avant le test (bug potentiel)
            // En C#, on initialise phi = 0 pour éviter l'avertissement
            phi = 0f;

            if ((eelast > 0) && (phi == 0) && (p0 > 0) && (a > 0) && (sz < 0)
                && (pmax > 0) && (coh > 0))
            {
                // Argile deux zones plastiques
                if (coh <= (k0 - 1) * sz)
                {
                    c1 = -coh / mu;
                    b = a * Math.Exp((sz - pmax) / (2 * coh));
                    ub = (p0 - sz) / 2f / mu * b;
                    ubsB = (p0 - sz) / 2f / mu;
                    uap = a * (Math.Exp((sz - pmax) / coh) * (ubsB - c1 / 2f) + c1 / 2f);
                    // x1 = uap / dext0 * 2000;
                    pfTheo = -sz * (2 * k0 - 1) + 2 * coh;
                    plimTheo = -sz + coh * Math.Log((2 * mu * ((float)Math.Sqrt(2) - 1) + coh) / ((k0 - 1) * sz + coh));
                }
                else
                {
                    // Argile une zone plastique
                    // c1p = 0;
                    uap = a / (1f / (Math.Exp(-pmax / coh) * Math.Exp(p0 / coh - 1))) * (coh / (2 * mu));
                    // x1 = uap / dext0 * 2000 + u0;
                    pfTheo = -k0 * sz + coh;
                    plimTheo = -k0 * sz + coh * (1 + Math.Log(2 * mu * ((float)Math.Sqrt(2) - 1) / coh));
                }

                result.Plim = plimTheo;
                result.Pf = pfTheo;
                result.Erreur = false;
            }
            else
            {
                result.Erreur = true;
            }

            return result;
        }

        /// <summary>
        /// Module principal de calcul de cohésion
        /// </summary>
        public static Cisaillement ModuleCohe(float Pfexp, EssaiForage Essai)
        {
            int i, j, k;
            float[] x = new float[11];  // 0 à 10
            float[] y = new float[11];  // 0 à 10
            float pres, pmax, def, n, phir, pfTheo, sz, k0, p0;
            float plim, phi, phimu, cTanPhi, a0L, a1L, nu;
            float dext, coh, hSonde, hNappe, gammaDej, gamma, eelast;
            int ndp;
            bool erreur;
            Cisaillement result;
            Regression regres;

            var moduleResult = new Cisaillement();

            j = 10;
            // En C#, les tableaux sont initialisés à 0 par défaut
            // Pas besoin de ReDim, on gère la taille dynamiquement

            hSonde = essai.ProfSonde;
            hNappe = essai.ProfNappe;
            gamma = essai.PoidsVol;
            gammaDej = essai.PoidsVolDejauge;
            eelast = essai.ModElastique;
            nu = essai.CoefPoisson;
            nu = 0.343f;  // Valeur forcée (comme en VB)
            // dext = essai.MySonde.Ds;
            dext = essai.MySonde.Getds();

            if (hSonde >= hNappe)
                sz = -(hSonde - hNappe) * gammaDej - (hNappe * gamma);
            if (hSonde < hNappe)
                sz = -hSonde * gamma;

            k0 = nu / (1 - nu);
            p0 = -k0 * sz;

            ndp = essai.Mesures.Count;

            k = -1;
            pmax = 0;

            if (ndp > 0)
            {
                for (i = 0; i < ndp; i++)
                {
                    // GetMesures(Ligne, Colonne, valeur)
                    pres = essai.Mesures[i].ContrainteRadiale;
                    def = essai.Mesures[i].Deformation;

                    if (pmax < pres)
                    {
                        pmax = pres;
                    }

                    if ((Math.Abs(pmax - pres) < 0.0001) && (pres >= pfexp)
                        && (def > 0) && (pres > 0))
                    {
                        k = k + 1;

                        // Agrandissement dynamique du tableau si nécessaire
                        if (k > j)
                        {
                            j = j + 10;
                            Array.Resize(ref x, j + 1);
                            Array.Resize(ref y, j + 1);
                        }

                        x[k] = (float)Math.Log(def);
                        y[k] = pres;
                    }
                }

                if (k > 0)
                {
                    // Appel à la fonction de régression linéaire
                    regres = RegresLine(k, x, y);

                    if (!regres.Erreur)
                    {
                        a1L = regres.A;
                        coh = a1L;
                        moduleResult.Coh = coh;
                        moduleResult.Phi = 0;
                        a0L = regres.B;  // ou regres.b selon ta classe
                        erreur = regres.Erreur;

                        result = CalculPlim(a1L, nu, cTanPhi, eelast, p0, phimu, dext,
                                           k0, sz, coh, pmax, ndp);

                        if (!result.Erreur)
                        {
                            plim = result.Plim;
                            pfTheo = result.Pf;

                            // Note : 'Phi' et 'Phir' ne sont jamais assignés dans le code VB
                            // Ce bloc est probablement mort ou incomplet
                            phi = 0f;   // Initialisation pour éviter l'avertissement
                            phir = 0f;  // Idem

                            if (phi > 0)
                            {
                                n = (1 - (float)Math.Sin(DTOR(phi))) / (1 + (float)Math.Sin(DTOR(phi)));
                                if (k0 < 1 / (1 + (float)Math.Sin(DTOR(phi))))
                                    pfTheo = (2 * k0 - n) * sz;
                                if (k0 >= 1 / (1 + (float)Math.Sin(DTOR(phir))))
                                    pfTheo = -2 * k0 * sz / (1 + n);
                                moduleResult.Pf = pfTheo;
                                moduleResult.Plim = plim;
                                moduleResult.Erreur = false;
                            }

                            moduleResult.Pf = pfTheo;
                            moduleResult.Plim = plim;
                            moduleResult.Erreur = false;

                            // Mémorisation du frottement
                            essai.Cohesion = (float)Math.Round(coh, 1);
                        }
                        else
                        {
                            moduleResult.Erreur = true;
                        }
                    }
                    else
                    {
                        moduleResult.Erreur = true;
                    }
                }
                else
                {
                    moduleResult.Erreur = true;
                }
            }
            else
            {
                moduleResult.Erreur = true;
            }

            return moduleResult;
        }

        /// <summary>
        /// Régression linéaire simple (à implémenter ou importer)
        /// </summary>
        private static Regression RegresLine(int n, float[] x, float[] y)
        {
            // TODO : Implémenter la régression linéaire
            // ou appeler ta classe existante
            var reg = new Regression();

            // Calcul standard de régression linéaire y = A*x + B
            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            float meanX, meanY;

            for (int i = 0; i <= n; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXY += x[i] * y[i];
                sumX2 += x[i] * x[i];
            }

            int count = n + 1;
            meanX = sumX / count;
            meanY = sumY / count;

            float denominator = sumX2 - sumX * meanX;
            if (Math.Abs(denominator) < 1e-10)
            {
                reg.Erreur = true;
                return reg;
            }

            reg.A = (sumXY - sumX * meanY) / denominator;
            reg.B = meanY - reg.A * meanX;
            reg.Erreur = false;

            return reg;
        }
    }
}using System;
using [PROJET].Models;
using [PROJET].Logic;

namespace [PROJET].Logic
{
    public static class ModCohesion
    {
        /// <summary>
        /// Convertit les degrés en radians
        /// </summary>
        private static float DTOR(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }

        /// <summary>
        /// Calcule la pression limite (Plim) et la pression de fluage (Pf)
        /// </summary>
        private static Cisaillement CalculPlim(
            float pente,
            float nu,
            float cTanPhi,
            float eelast,
            float p0,
            float phimu,
            float dext0,
            float k0,
            float sz,
            float coh,
            float pmax,
            int ndp)
        {
            float uap, ub, phi;
            float plimTheo, pfTheo, ubsB, c1, mu, a, b;

            var result = new Cisaillement();

            a = dext0 / 200f;
            mu = eelast / 2f / (1f + nu);

            // Note : En VB, 'Phi' n'est jamais assigné avant le test (bug potentiel)
            // En C#, on initialise phi = 0 pour éviter l'avertissement
            phi = 0f;

            if ((eelast > 0) && (phi == 0) && (p0 > 0) && (a > 0) && (sz < 0)
                && (pmax > 0) && (coh > 0))
            {
                // Argile deux zones plastiques
                if (coh <= (k0 - 1) * sz)
                {
                    c1 = -coh / mu;
                    b = a * Math.Exp((sz - pmax) / (2 * coh));
                    ub = (p0 - sz) / 2f / mu * b;
                    ubsB = (p0 - sz) / 2f / mu;
                    uap = a * (Math.Exp((sz - pmax) / coh) * (ubsB - c1 / 2f) + c1 / 2f);
                    // x1 = uap / dext0 * 2000;
                    pfTheo = -sz * (2 * k0 - 1) + 2 * coh;
                    plimTheo = -sz + coh * Math.Log((2 * mu * ((float)Math.Sqrt(2) - 1) + coh) / ((k0 - 1) * sz + coh));
                }
                else
                {
                    // Argile une zone plastique
                    // c1p = 0;
                    uap = a / (1f / (Math.Exp(-pmax / coh) * Math.Exp(p0 / coh - 1))) * (coh / (2 * mu));
                    // x1 = uap / dext0 * 2000 + u0;
                    pfTheo = -k0 * sz + coh;
                    plimTheo = -k0 * sz + coh * (1 + Math.Log(2 * mu * ((float)Math.Sqrt(2) - 1) / coh));
                }

                result.Plim = plimTheo;
                result.Pf = pfTheo;
                result.Erreur = false;
            }
            else
            {
                result.Erreur = true;
            }

            return result;
        }

        /// <summary>
        /// Module principal de calcul de cohésion
        /// </summary>
        public static Cisaillement ModuleCohe(float Pfexp, EssaiForage Essai)
        {
            int i, j, k;
            float[] x = new float[11];  // 0 à 10
            float[] y = new float[11];  // 0 à 10
            float pres, pmax, def, n, phir, pfTheo, sz, k0, p0;
            float plim, phi, phimu, cTanPhi, a0L, a1L, nu;
            float dext, coh, hSonde, hNappe, gammaDej, gamma, eelast;
            int ndp;
            bool erreur;
            Cisaillement result;
            Regression regres;

            var moduleResult = new Cisaillement();

            j = 10;
            // En C#, les tableaux sont initialisés à 0 par défaut
            // Pas besoin de ReDim, on gère la taille dynamiquement

            hSonde = essai.ProfSonde;
            hNappe = essai.ProfNappe;
            gamma = essai.PoidsVol;
            gammaDej = essai.PoidsVolDejauge;
            eelast = essai.ModElastique;
            nu = essai.CoefPoisson;
            nu = 0.343f;  // Valeur forcée (comme en VB)
            // dext = essai.MySonde.Ds;
            dext = essai.MySonde.GetDs();

            if (hSonde >= hNappe)
                sz = -(hSonde - hNappe) * gammaDej - (hNappe * gamma);
            if (hSonde < hNappe)
                sz = -hSonde * gamma;

            k0 = nu / (1 - nu);
            p0 = -k0 * sz;

            ndp = essai.Mesures.Count;

            k = -1;
            pmax = 0;

            if (ndp > 0)
            {
                for (i = 0; i < ndp; i++)
                {
                    // GetMesures(Ligne, Colonne, valeur)
                    pres = essai.Mesures[i].ContrainteRadiale;
                    def = essai.Mesures[i].Deformation;

                    if (pmax < pres)
                    {
                        pmax = pres;
                    }

                    if ((Math.Abs(pmax - pres) < 0.0001) && (pres >= pfexp)
                        && (def > 0) && (pres > 0))
                    {
                        k = k + 1;

                        // Agrandissement dynamique du tableau si nécessaire
                        if (k > j)
                        {
                            j = j + 10;
                            Array.Resize(ref x, j + 1);
                            Array.Resize(ref y, j + 1);
                        }

                        x[k] = (float)Math.Log(def);
                        y[k] = pres;
                    }
                }

                if (k > 0)
                {
                    // Appel à la fonction de régression linéaire
                    regres = RegresLine(k, x, y);

                    if (!regres.Erreur)
                    {
                        a1L = regres.A;
                        coh = a1L;
                        moduleResult.Coh = coh;
                        moduleResult.Phi = 0;
                        a0L = regres.B;  // ou regres.b selon ta classe
                        erreur = regres.Erreur;

                        result = CalculPlim(a1L, nu, cTanPhi, eelast, p0, phimu, dext,
                                           k0, sz, coh, pmax, ndp);

                        if (!result.Erreur)
                        {
                            plim = result.Plim;
                            pfTheo = result.Pf;

                            // Note : 'Phi' et 'Phir' ne sont jamais assignés dans le code VB
                            // Ce bloc est probablement mort ou incomplet
                            phi = 0f;   // Initialisation pour éviter l'avertissement
                            phir = 0f;  // Idem

                            if (phi > 0)
                            {
                                n = (1 - (float)Math.Sin(DTOR(phi))) / (1 + (float)Math.Sin(DTOR(phi)));
                                if (k0 < 1 / (1 + (float)Math.Sin(DTOR(phi))))
                                    pfTheo = (2 * k0 - n) * sz;
                                if (k0 >= 1 / (1 + (float)Math.Sin(DTOR(phir))))
                                    pfTheo = -2 * k0 * sz / (1 + n);
                                moduleResult.Pf = pfTheo;
                                moduleResult.Plim = plim;
                                moduleResult.Erreur = false;
                            }

                            moduleResult.Pf = pfTheo;
                            moduleResult.Plim = plim;
                            moduleResult.Erreur = false;

                            // Mémorisation du frottement
                            essai.Cohesion = (float)Math.Round(coh, 1);
                        }
                        else
                        {
                            moduleResult.Erreur = true;
                        }
                    }
                    else
                    {
                        moduleResult.Erreur = true;
                    }
                }
                else
                {
                    moduleResult.Erreur = true;
                }
            }
            else
            {
                moduleResult.Erreur = true;
            }

            return moduleResult;
        }

        /// <summary>
        /// Régression linéaire simple (à implémenter ou importer)
        /// </summary>
        private static Regression RegresLine(int n, float[] x, float[] y)
        {
            // TODO : Implémenter la régression linéaire
            // ou appeler ta classe existante
            var reg = new Regression();

            // Calcul standard de régression linéaire y = A*x + B
            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            float meanX, meanY;

            for (int i = 0; i <= n; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXY += x[i] * y[i];
                sumX2 += x[i] * x[i];
            }

            int count = n + 1;
            meanX = sumX / count;
            meanY = sumY / count;

            float denominator = sumX2 - sumX * meanX;
            if (Math.Abs(denominator) < 1e-10)
            {
                reg.Erreur = true;
                return reg;
            }

            reg.A = (sumXY - sumX * meanY) / denominator;
            reg.B = meanY - reg.A * meanX;
            reg.Erreur = false;

            return reg;
        }
    }
}