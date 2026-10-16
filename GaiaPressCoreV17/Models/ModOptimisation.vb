Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic

Module ModOptimisation

    'Fonction d'optimisation des paramètres d'essai pressiométrique
    Public Function Optimisation(Essai As Essai_Forage, ByVal U0 As Double,
          ByVal OptimiseE As Boolean, ByVal OptimiseC As Boolean,
          ByVal OptimisePhi As Boolean, ByVal OptimiseU0 As Boolean) As Optimise

        Dim Ndp, k As Integer
        Dim row As Integer
        Dim Absc, ordo As Integer
        Dim Pres, Def As Double
        Dim Sz, Dext, Nu, Eelast, Phimu, Coh, Phi, Phii, Phij, UaMax, Pmax As Double
        Dim Cohi, Cohj, dC As Double
        Dim A, K0, P0, Lambda, Mu, Lambdai, Mui, Lambdaj, Muj, Phie, N, Np As Double
        Dim Phiei, Phiej, Ni, Nj, Npi, Npj, U0i, U0j As Double
        Dim Sei, S2ei, Sci, S2ci, Spi, S2pi As Double
        Dim Sej, S2ej, Scj, S2cj, Spj, S2pj As Double
        Dim dPhi, Se, Seini, S2e, Sc, S2c, Sp, S2p As Double
        Dim UaMaxj, Pmaxj, UaMaxi, Pmaxi, PfTheo, dU0 As Double
        Dim dE, PasE, PasC, PasPhi, PasU0, PasMaxE, PasMaxC, PasMaxPhi, PasMaxU0 As Double
        Dim Theo As Boolean
        Dim Eelasti, Eelastj As Double
        Dim dSdJ, d2SdJ2 As Double
        Dim Theoriei As Courbe
        Dim Theorie As Courbe
        Dim Theoriej As Courbe

        Ndp = Essai.GetMesureCount

        'Paramétrage de la courbe
        With Essai
            Dext = MySonde.Ds
            Nu = .coefPoisson
            Eelast = .modElastique
            If Eelast = 0 Then Eelast = .modPressio
            Phimu = .MySol.Phimu
            Coh = .Cohesion
            Phi = .Frottement
        End With

        'Pas minimum de variation, à régler éventuellement
        PasE = 100  'en kPa
        PasPhi = 1  'en degré
        PasC = 10   'en kPa
        PasU0 = 0.01    'sans unité

        'Pas maximum de variation, à régler éventuellement
        PasMaxE = 5000  'en kPa
        PasMaxPhi = 10  'en degré
        PasMaxC = 100   'en kPa
        PasMaxU0 = 0.01    'sans unité

        'Optimisation en Contrainte/Déformation
        If (Ndp > 0) And (Eelast > 0) Then

            With Essai
                ' Contrainte/Déformation
                Absc = 5
                ordo = 4

                UaMax = 0
                Pmax = 0
                UaMaxi = 0
                Pmaxi = 0
                UaMaxj = 0
                Pmaxj = 0
                Theo = True

                'Préparation du calcul théorique
                A = Dext / 2000 * (1 + U0)
                K0 = Nu / (1 - Nu)

                Phie = Phi - Phimu
                ' GetMesures(Ligne, Colonne, valeur)
                Sz = .Mesures(8).ContrainteVerticale
                P0 = K0 * Sz
                N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
                Np = (1 - Math.Sin(DTOR(Phie))) / (1 + Math.Sin(DTOR(Phie)))

                Se = 0

                'Optimisation du décalage à l'origine
                If OptimiseU0 Then

                    Lambda = Eelast * Nu / (1 + Nu) / (1 - 2 * Nu)
                    Mu = Eelast / 2 / (1 + Nu)
                    For k = 0 To 500

                        'Initialisation du test de convergence
                        Seini = Se

                        'Initialisations
                        Sei = 0
                        S2ei = 0
                        Sci = 0
                        S2ci = 0
                        Spi = 0
                        S2pi = 0
                        Se = 0
                        S2e = 0
                        Sc = 0
                        S2c = 0
                        Sp = 0
                        S2p = 0
                        Sej = 0
                        S2ej = 0
                        Scj = 0
                        S2cj = 0
                        Spj = 0
                        S2pj = 0

                        'Jeux de paramètres
                        If U0 < 0 Then U0 = 0
                        U0i = U0 - PasU0
                        U0j = U0 + PasU0
                        Pmax = 0
                        Pmaxi = 0
                        Pmaxj = 0
                        UaMax = 0
                        UaMaxi = 0
                        UaMaxj = 0

                        For row = 1 To Ndp

                            'Pressions et déformations expérimentales

                            Pres = .Mesures(row - 1).ContrainteRadiale
                            Def = .Mesures(row - 1).Deformation

                            If Pres > Pmax Then
                                Pmax = Pres
                            End If
                            If Pres > Pmaxi Then
                                Pmaxi = Pres
                            End If
                            If Pres > Pmaxj Then
                                Pmaxj = Pres
                            End If


                            'Calcul pour le jeu de paramètres Debase
                            Theorie = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0, UaMax,
                            Pres, Pmax, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmax = Theorie.Pmax
                            UaMax = Theorie.UaMax
                            PfTheo = Theorie.Pf

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres <= PfTheo) Then
                                Se = Se + (Def - Theorie.Def) ^ 2
                            End If


                            'Calcul pour le jeu de paramètres i
                            Theoriei = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0i, UaMaxi,
                            Pres, Pmaxi, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxi = Theorie.Pmax
                            UaMaxi = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres <= PfTheo) Then
                                Sei = Sei + (Def - Theoriei.Def) ^ 2
                            End If

                            'Calcul pour le jeu de paramètres j
                            Theoriej = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0j, UaMaxj,
                            Pres, Pmaxj, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxj = Theorie.Pmax
                            UaMaxj = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres <= PfTheo) Then
                                Sej = Sej + (Def - Theoriej.Def) ^ 2
                            End If


                        Next row
                        dSdJ = (Sej - Sei) / (2 * PasE)
                        d2SdJ2 = (Sej - 2 * Se + Sei) / (PasE ^ 2)
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 >= 0) Then d2SdJ2 = 0.0000000001
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 < 0) Then d2SdJ2 = -0.0000000001

                        dU0 = dSdJ / d2SdJ2
                        If (Math.Abs(dU0) > PasMaxU0) And (dU0 > 0) Then dU0 = PasMaxU0
                        If (Math.Abs(dU0) > PasMaxU0) And (dU0 < 0) Then dU0 = -PasMaxU0

                        'Test de convergence
                        If (Seini < Se) And (k > 0) And (Pres <= PfTheo) Then
                            Optimisation.Erreur = True
                            Optimisation.U0 = Math.Round(U0, 5)
                            Exit For
                        End If

                        'Test de fin d'itération
                        If (Math.Abs(dSdJ / d2SdJ2) < 0.01) Then
                            Optimisation.Erreur = False
                            Optimisation.U0 = Math.Round(U0, 5)
                            'Call essai.SetU0(Math.Round(U0, 5))
                            Exit For
                        End If

                        'Prochaine valeur à calculer
                        U0 = U0 - dU0

                    Next k
                    If k > 500 Then
                        Optimisation.Erreur = True
                        Optimisation.U0 = Math.Round(U0, 5)
                    End If
                End If

                If OptimiseE Then
                    For k = 0 To 500

                        'Initialisation du test de convergence
                        Seini = Se

                        'Initialisations
                        Sei = 0
                        S2ei = 0
                        Sci = 0
                        S2ci = 0
                        Spi = 0
                        S2pi = 0
                        Se = 0
                        S2e = 0
                        Sc = 0
                        S2c = 0
                        Sp = 0
                        S2p = 0
                        Sej = 0
                        S2ej = 0
                        Scj = 0
                        S2cj = 0
                        Spj = 0
                        S2pj = 0

                        'Jeux de paramètres élastiques
                        Lambda = Eelast * Nu / (1 + Nu) / (1 - 2 * Nu)
                        Mu = Eelast / 2 / (1 + Nu)
                        Eelasti = Eelast - PasE
                        Lambdai = Eelasti * Nu / (1 + Nu) / (1 - 2 * Nu)
                        Mui = Eelasti / 2 / (1 + Nu)
                        Eelastj = Eelast + PasE
                        Lambdaj = Eelastj * Nu / (1 + Nu) / (1 - 2 * Nu)
                        Muj = Eelastj / 2 / (1 + Nu)
                        Pmax = 0
                        Pmaxi = 0
                        Pmaxj = 0
                        UaMax = 0
                        UaMaxi = 0
                        UaMaxj = 0


                        For row = 1 To Ndp

                            'Pressions et déformations expérimentales
                            ' GetMesures(Ligne, Colonne, valeur)
                            Pres = .Mesures(row - 1).ContrainteRadiale
                            Def = .Mesures(row - 1).Deformation

                            If Pres > Pmax Then
                                Pmax = Pres
                            End If
                            If Pres > Pmaxi Then
                                Pmaxi = Pres
                            End If
                            If Pres > Pmaxj Then
                                Pmaxj = Pres
                            End If


                            'Calcul pour le jeu de paramètres Debase
                            Theorie = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0, UaMax,
                            Pres, Pmax, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmax = Theorie.Pmax
                            UaMax = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If Math.Abs(Pres - Pmax) > 0.001 Then
                                Se = Se + (Def - Theorie.Def) ^ 2
                            End If


                            'Calcul pour le jeu de paramètres i
                            Theoriei = ComptPres(Dext, A, Nu, Eelasti, Sz, Phimu, Coh, Phi, U0, UaMaxi,
                            Pres, Pmaxi, K0, P0, Lambdai, Mui, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxi = Theorie.Pmax
                            UaMaxi = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If Math.Abs(Pres - Pmax) > 0.001 Then
                                Sei = Sei + (Def - Theoriei.Def) ^ 2
                            End If

                            'Calcul pour le jeu de paramètres j
                            Theoriej = ComptPres(Dext, A, Nu, Eelastj, Sz, Phimu, Coh, Phi, U0, UaMaxj,
                            Pres, Pmaxj, K0, P0, Lambdaj, Muj, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxj = Theorie.Pmax
                            UaMaxj = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If Math.Abs(Pres - Pmax) > 0.001 Then
                                Sej = Sej + (Def - Theoriej.Def) ^ 2
                            End If


                        Next row
                        dSdJ = (Sej - Sei) / (2 * PasE)
                        d2SdJ2 = (Sej - 2 * Se + Sei) / (PasE ^ 2)
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 >= 0) Then d2SdJ2 = 0.0000000001
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 < 0) Then d2SdJ2 = -0.0000000001

                        'Calcul du pas de variation
                        dE = dSdJ / d2SdJ2
                        If (Math.Abs(dE) > PasMaxE) And (dE > 0) Then dE = PasMaxE
                        If (Math.Abs(dE) > PasMaxE) And (dE < 0) Then dE = -PasMaxE

                        'Test de convergence
                        If (Seini < Se) And (k > 0) Then
                            Optimisation.Erreur = True
                            Optimisation.Eelast = Math.Round(Eelast, 1)
                            Exit For
                        End If

                        'Test de fin d'itération
                        If (Math.Abs(dSdJ / d2SdJ2) < 100) Then
                            Optimisation.Erreur = False
                            Optimisation.Eelast = Math.Round(Eelast, 1)
                            Essai.modElastique = Math.Round(Eelast, 1)
                            Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                            Exit For
                        End If

                        'Prochaine valeur à calculer
                        Eelast = Eelast - dE
                        If Eelast < 0 Then
                            Eelast = 0.001
                        End If

                    Next k
                    If k > 500 Then
                        Optimisation.Erreur = True
                        Optimisation.Eelast = Math.Round(Eelast, 1)
                        Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                    End If
                End If

                'Optimisation sur le frottement
                If OptimisePhi Then

                    Lambda = Eelast * Nu / (1 + Nu) / (1 - 2 * Nu)
                    Mu = Eelast / 2 / (1 + Nu)
                    For k = 0 To 500

                        'Initialisation du test de convergence
                        Seini = Se

                        'Initialisations
                        Sei = 0
                        S2ei = 0
                        Sci = 0
                        S2ci = 0
                        Spi = 0
                        S2pi = 0
                        Se = 0
                        S2e = 0
                        Sc = 0
                        S2c = 0
                        Sp = 0
                        S2p = 0
                        Sej = 0
                        S2ej = 0
                        Scj = 0
                        S2cj = 0
                        Spj = 0
                        S2pj = 0

                        'Jeux de paramètres
                        If Phi < Phimu Then Phi = Phimu
                        Phii = Phi - PasPhi
                        If Phii < Phimu Then Phii = Phimu
                        Phij = Phi + PasPhi
                        Phiei = Phii - Phimu
                        Phiej = Phij - Phimu
                        Ni = (1 - Math.Sin(DTOR(Phii))) / (1 + Math.Sin(DTOR(Phii)))
                        Npi = (1 - Math.Sin(DTOR(Phiei))) / (1 + Math.Sin(DTOR(Phiei)))
                        Nj = (1 - Math.Sin(DTOR(Phij))) / (1 + Math.Sin(DTOR(Phij)))
                        Npj = (1 - Math.Sin(DTOR(Phiej))) / (1 + Math.Sin(DTOR(Phiej)))
                        N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
                        Np = (1 - Math.Sin(DTOR(Phie))) / (1 + Math.Sin(DTOR(Phie)))
                        Pmax = 0
                        Pmaxi = 0
                        Pmaxj = 0
                        UaMax = 0
                        UaMaxi = 0
                        UaMaxj = 0

                        For row = 1 To Ndp

                            'Pressions et déformations expérimentales
                            ' GetMesures(Ligne, Colonne, valeur)
                            Pres = .Mesures(row - 1).ContrainteRadiale
                            Def = .Mesures(row - 1).Deformation

                            If Pres > Pmax Then
                                Pmax = Pres
                            End If
                            If Pres > Pmaxi Then
                                Pmaxi = Pres
                            End If
                            If Pres > Pmaxj Then
                                Pmaxj = Pres
                            End If


                            'Calcul pour le jeu de paramètres Debase
                            Theorie = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0, UaMax,
                            Pres, Pmax, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmax = Theorie.Pmax
                            UaMax = Theorie.UaMax
                            PfTheo = Theorie.Pf

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Se = Se + (Def - Theorie.Def) ^ 2
                            End If


                            'Calcul pour le jeu de paramètres i
                            Theoriei = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phii, U0, UaMaxi,
                            Pres, Pmaxi, K0, P0, Lambda, Mu, Phiei, Ni, Npi, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxi = Theorie.Pmax
                            UaMaxi = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Sei = Sei + (Def - Theoriei.Def) ^ 2
                            End If

                            'Calcul pour le jeu de paramètres j
                            Theoriej = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phij, U0, UaMaxj,
                            Pres, Pmaxj, K0, P0, Lambda, Mu, Phiej, Nj, Npj, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxj = Theorie.Pmax
                            UaMaxj = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Sej = Sej + (Def - Theoriej.Def) ^ 2
                            End If


                        Next row
                        dSdJ = (Sej - Sei) / (2 * PasE)
                        d2SdJ2 = (Sej - 2 * Se + Sei) / (PasE ^ 2)
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 >= 0) Then d2SdJ2 = 0.0000000001
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 < 0) Then d2SdJ2 = -0.0000000001

                        dPhi = dSdJ / d2SdJ2
                        If (Math.Abs(dPhi) > PasMaxPhi) And (dPhi > 0) Then dPhi = PasMaxPhi
                        If (Math.Abs(dPhi) > PasMaxPhi) And (dPhi < 0) Then dPhi = -PasMaxPhi

                        'Test de convergence
                        If (Seini < Se) And (k > 0) And (Pres > PfTheo) Then
                            Optimisation.Erreur = True
                            Optimisation.Phi = Math.Round(Phi, 1)
                            Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                            Exit For
                        End If

                        'Test de fin d'itération
                        If (Math.Abs(dSdJ / d2SdJ2) < 0.2) Then
                            Optimisation.Erreur = False
                            Optimisation.Phi = Math.Round(Phi, 1)
                            Essai.Frottement = Math.Round(Phi, 1)
                            Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                            Exit For
                        End If

                        'Prochaine valeur à calculer
                        Phi = Phi - dPhi
                        If Phi < 0 Then
                            Phi = 0
                        End If

                    Next k
                    If k > 500 Then
                        Optimisation.Erreur = True
                        Optimisation.Phi = Math.Round(Phi, 1)
                        Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                    End If
                End If

                'Optimisation sur la cohésion
                If OptimiseC Then

                    Lambda = Eelast * Nu / (1 + Nu) / (1 - 2 * Nu)
                    Mu = Eelast / 2 / (1 + Nu)
                    For k = 0 To 500

                        'Initialisation du test de convergence
                        Seini = Se

                        'Initialisations
                        Sei = 0
                        S2ei = 0
                        Sci = 0
                        S2ci = 0
                        Spi = 0
                        S2pi = 0
                        Se = 0
                        S2e = 0
                        Sc = 0
                        S2c = 0
                        Sp = 0
                        S2p = 0
                        Sej = 0
                        S2ej = 0
                        Scj = 0
                        S2cj = 0
                        Spj = 0
                        S2pj = 0

                        'Jeux de paramètres
                        Cohi = Coh - PasC
                        If Cohi < 0 Then Cohi = 0
                        Cohj = Coh + PasC
                        If Cohj < 0 Then Cohj = 0
                        Pmax = 0
                        Pmaxi = 0
                        Pmaxj = 0
                        UaMax = 0
                        UaMaxi = 0
                        UaMaxj = 0

                        For row = 1 To Ndp

                            'Pressions et déformations expérimentales
                            ' GetMesures(Ligne, Colonne, valeur)
                            Pres = .Mesures(row - 1).ContrainteRadiale
                            Def = .Mesures(row - 1).Deformation

                            If Pres > Pmax Then
                                Pmax = Pres
                            End If
                            If Pres > Pmaxi Then
                                Pmaxi = Pres
                            End If
                            If Pres > Pmaxj Then
                                Pmaxj = Pres
                            End If


                            'Calcul pour le jeu de paramètres Debase
                            Theorie = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0, UaMax,
                            Pres, Pmax, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmax = Theorie.Pmax
                            UaMax = Theorie.UaMax
                            PfTheo = Theorie.Pf

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Se = Se + (Def - Theorie.Def) ^ 2
                            End If


                            'Calcul pour le jeu de paramètres i
                            Theoriei = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Cohi, Phi, U0, UaMaxi,
                            Pres, Pmaxi, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxi = Theorie.Pmax
                            UaMaxi = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Sei = Sei + (Def - Theoriei.Def) ^ 2
                            End If

                            'Calcul pour le jeu de paramètres j
                            Theoriej = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Cohj, Phi, U0, UaMaxj,
                            Pres, Pmaxj, K0, P0, Lambda, Mu, Phie, N, Np, Theo)

                            'Mémorisation des pressions et déformations maximum
                            Pmaxj = Theorie.Pmax
                            UaMaxj = Theorie.UaMax

                            'Calcul de l'écart type sur le cycle
                            If (Math.Abs(Pres - Pmax) < 0.001) And (Pres >= PfTheo) Then
                                Sej = Sej + (Def - Theoriej.Def) ^ 2
                            End If


                        Next row
                        dSdJ = (Sej - Sei) / (2 * PasC)
                        d2SdJ2 = (Sej - 2 * Se + Sei) / (PasC ^ 2)
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 >= 0) Then d2SdJ2 = 0.0000000001
                        If (Math.Abs(d2SdJ2) < 0.0000000001) And (d2SdJ2 < 0) Then d2SdJ2 = -0.0000000001

                        dC = dSdJ / d2SdJ2
                        If (Math.Abs(dC) > PasMaxC) And (dC > 0) Then dC = PasMaxC
                        If (Math.Abs(dC) > PasMaxC) And (dC < 0) Then dC = -PasMaxC

                        'Test de convergence
                        If (Seini < Se) And (k > 0) And (Pres > PfTheo) Then
                            Optimisation.Erreur = True
                            Optimisation.Coh = Math.Round(Coh, 1)
                            Exit For
                        End If

                        'Test de fin d'itération
                        If (Math.Abs(dSdJ / d2SdJ2) < 0.2) Then
                            Optimisation.Erreur = False
                            Optimisation.Coh = Math.Round(Coh, 1)
                            Essai.Cohesion = Math.Round(Coh, 1)
                            Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                            Exit For
                        End If

                        'Prochaine valeur à calculer
                        Coh = Coh - dC
                        If Coh < 0 Then
                            Coh = 0
                        End If

                    Next k
                    If k > 500 Then
                        Optimisation.Erreur = True
                        Optimisation.Coh = Math.Round(Coh, 1)
                        Optimisation.Plim = Math.Round(Theorie.Plim, 1)
                    End If
                End If

            End With
        End If

    End Function

End Module
