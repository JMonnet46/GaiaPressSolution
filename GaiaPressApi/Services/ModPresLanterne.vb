Namespace GaiaPress.core.V17.Services

    Module ModPresLanterne

        Public Function PresLanterne(Essai As Essai_Forage)

            Dim indice1 As Integer
            Dim PmembMax, Psol, Rsol, DRsol, Pair As Single
            Dim RextMax, PsolMax, PsolMin, Fsol, Fsolt, Fcel As Single
            Dim Heau, Feau As Single
            Dim Pg, DPSolmax, Smes, Fmes As Single
            Dim Sz, P0, K0 As Single
            Dim Pres, V60, FeauMes, dFeauMes As Single
            Dim Elast As Boolean
            Dim Lame As Boolean
            Dim result As Inertie

            ' Variable de la classe Sonde
            ' Public Enum typeDeSonde
            '   SoupleTypeE = 0
            '   SoupleTypeG = 1
            '   TubeFenduCourte = 2
            '   TubeFenduLongue = 3
            '   Geomecametre = 4
            '   Autre = 5
            'End Enum
            '
            'Public Enum typeDeDeformee
            '   Nue = 0
            '   Lanternee = 1
            'End Enum
            '
            'Public Enum typeDeContrainte
            '   Moyenne = 0
            '   Lame = 1
            'End Enum


            ' Variables pour les caractéristiques de l'objet
            Dim Vext0, P0i, Lmes, Lcel, Lt, Rext0, EpAir, EpEau, EpTube, EpTotCentre, EpTotCentre0, EpTotExtre As Single
            Dim dPInert, Phimu, Phi, Gamma, Gammadej, Nu, Zc, Zs, Zw, Pcap, A, B, VolNF As Single
            Dim TypeDefo, TypeCont As Integer
            Dim Ndp As Integer
            Dim Precision, Pmemb, Pdh, Peau, Coef1, Coef2, Coef3, Coef4, R0p, Vint0 As Single
            Dim MesuresEssaiCalcule As MesuresEssaiForage

            ' Initialisation
            PresLanterne = 0

            'Précision du contrôle d'équilibre statique
            Precision = 0.1
            MesuresEssaiCalcule = New MesuresEssaiForage

            'chargement des variables
            Ndp = Essai.E_Mesures.Count


            With Essai.GetMyEssaiCalib(EssaiCalib.Resistance)
                Coef1 = .GetA0
                Coef2 = .GetA1
                Coef3 = .GetA2
                Coef4 = .GetA3
            End With

            With Essai
                A = .GetMyEssaiCalib(EssaiCalib.Expansion).GetCoefficientA
                B = .GetMyEssaiCalib(EssaiCalib.Expansion).GetCoefficientB
                TypeDefo = .MySonde.GetTypeDeformee
                TypeCont = .MySonde.GetTypeContrainte

                ' Passage des dimensions en cm
                Lt = .MySonde.Getlt / 10
                Lcel = .MySonde.Getlg / 10
                Lmes = .MySonde.Getls / 10
                Vext0 = .MySonde.GetVs
                Lcel = 2 * Lcel + Lmes
                Rext0 = .MySonde.Getds / 20

                ' Passage des épaisseurs en cm
                EpEau = .MySonde.GetEpMembraneMesure / 10
                EpAir = .MySonde.GetEpMembraneGarde / 10
                EpTube = .MySonde.GetEpaisTubeFendu / 10
                EpTotCentre0 = .MySonde.P_EpInitMembrane / 10

                dPInert = .E_difPresEauAir

                P0i = .E_presP0
                'On annule P0i
                'P0i = 0

                Gamma = .E_poidsVol
                Gammadej = .E_poidsVolDejauge
                Nu = .E_coefPoisson
                If MySol Is Nothing Then
                    Select Case Lang
                        Case 0 : MsgBox("You must select a soil")
                        Case 1 : MsgBox("Vous devez sélectionner un sol")
                    End Select
                    Exit Function
                End If
                Phimu = MySol.GetPhiMu
                Phi = .E_Frottement
                If Phi < Phimu Then Phi = Phimu
                Zc = .E_hauteurCpv
                Zw = .E_profNappe
                Zs = .E_profSonde

                'Calcul du volume intérieur initial de la sonde
                EpTotCentre = EpEau + EpAir + EpTube
                EpTotExtre = EpAir + EpTube

            End With


            result = PresMembMax(Coef1, Coef2, Coef3, Coef4)
            PmembMax = result.PresInertie

            ' Pression de membrane d'eau
            Pmemb = Essai.MySonde.GetPm
            EpTotCentre = EpEau + EpAir + EpTube
            EpTotExtre = EpAir + EpTube
            R0p = Math.Sqrt(Vext0 / (Pi * Lmes))   ' Rayon externe de la sonde
            Vint0 = Pi * (R0p - EpTotCentre) ^ 2 * Lmes       ' Volume interne initial de la sonde
            If Essai.E_Mesures Is Nothing Then Exit Function
            'If MyEssai.E_Mesures(Ndp - 1).RefEssai = Nothing Then
            '    ReDim Preserve MyEssai.E_Mesures(Ndp - 1).
            '    Ndp = Ndp - 1
            'End If
            For indice1 = 0 To Ndp - 1

                ' GetMesures( Ligne, Colonne, Valeur)
                Pres = Essai.E_Mesures(indice1).Pression
                V60 = Essai.E_Mesures(indice1).Volume60
                MesuresEssaiCalcule.RefEssai = Essai.E_Mesures(indice1).RefEssai
                MesuresEssaiCalcule.IndiceMesure = indice1
                MesuresEssaiCalcule.Pression = Essai.E_Mesures(indice1).Pression
                MesuresEssaiCalcule.Volume15 = Essai.E_Mesures(indice1).Volume15
                MesuresEssaiCalcule.Volume30 = Essai.E_Mesures(indice1).Volume30
                MesuresEssaiCalcule.Volume60 = Essai.E_Mesures(indice1).Volume60
                If CompMemb Then
                    EpTotCentre = EpEau + EpAir + EpTube + B * Pres / 10    ' Ecrasement des membranes
                    EpTotCentre = EpTotCentre0 + B * Pres / 1
                Else
                    EpTotCentre = EpEau + EpAir + EpTube ' Pas d'écrasement des membranes
                End If
                'Calcul du volume corrigé de l'expansion
                VolNF = V60 - A * Pres
                If VolNF < 0 Then VolNF = 0

                ' Prise en compte de la compression des membranes
                'EpTotCentre = EpEau + EpAir + EpTube + B * Pres
                'EpTotCentre = EpEau + EpAir + EpTube

                Elast = False

                'Calcul de la pression d'inertie à partir des mesures brutes
                result = PresMembrane(Coef1, Coef2, Coef3, Coef4, PmembMax, V60)
                Pcap = result.PresInertie

                Lame = (TypeCont = 1)

                ' Calcul du rayon maximum de la lanterne
                ' Formule p.29.3 du manuscrit
                'AA = 256 * Pi * IntCarre(Lt, Lmes / 2, -Lmes / 2)
                'BB = (Rext0 - EpTotCentre) * 32 * Pi * Inte(Lt, Lmes / 2, -Lmes / 2)
                'CC = -VolNF

                'solu = RACINE2(AA, BB, CC)

                'If solu.Erreur = 0 Then
                '    DRmax = solu.X1
                'End If

                'If DRmax < 0 Then
                '    DRmax = 0
                'End If

                '' Valeur du rayon maximum de la lanterne -----
                'RextMax = Rext0 + DRmax
                If CompMemb Then
                    RextMax = RMembMaxTubeFendu(Vext0, V60, Lt, Lmes, Rext0, EpTotCentre)
                Else
                    RextMax = RMembMaxTubeFendu(Vext0, VolNF, Lt, Lmes, Rext0, EpTotCentre)
                End If

                'RextMax = RMembMaxTubeFendu(Vint0, V60, Lt, Lmes, Rext0, EpTotCentre)

                ' Calcul du rayon moyen du sol au contact de la lanterne
                ' Formule p.26 du manuscrit
                ' Rsol = 16 * DRmax * Inte(Lt, Lmes / 2, -Lmes / 2) / Lmes + Rext0
                Rsol = QGRayon(-Lmes / 2, Lmes / 2, Rext0, RextMax, Lt) / Lmes

                'calcul de la déformation radiale moyenne
                DRsol = (Rsol - Rext0) / Rext0
                MesuresEssaiCalcule.deformation = DRsol

                ' Calcul de la surface de mesure au contact du sol
                If Not Lame Then
                    Smes = 2 * Pi * Rsol * Lmes
                Else
                    Smes = 2 * Pi * Rext0 * Lmes
                End If

                ' calcul de l'etat de contrainte vertical et Horizontal
                If Zs >= Zw Then
                    Sz = (Zs - Zw) * Gammadej + (Zw * Gamma)
                Else
                    Sz = Zs * Gamma
                End If
                MesuresEssaiCalcule.contrainteVerticale = Sz

                'Sécurité sur Nu
                If Nu = 0 Then
                    Nu = 0.3
                End If

                ' Calcul de K0
                K0 = Nu / (1 - Nu)

                ' Calcul de la pression
                P0 = K0 * Sz

                If P0 < P0i Then
                    P0i = P0
                End If

                'Calcul de la pression de l'eau au milieu de la sonde
                Pg = 9.81 * (Zs - Zw)
                If Pg < 0 Then
                    Pg = 0
                End If

                'Calcul de la pression d'eau statique à l'intérieur de l'appareil
                Heau = (Zc + Zs)
                Pdh = 9.81 * Heau
                Peau = Pres - Pcap + Pdh
                If Peau < 0 Then
                    Peau = 0
                End If

                'Pair = Pres - Pcap - dPInert + Pmemb
                Pair = Pres - Pcap - dPInert
                If Pair < 0 Then
                    Pair = 0
                End If


                Fcel = FCelLanterne(Lt, Lmes, Lcel, RextMax, Rext0, Pair, Peau, EpTotExtre, EpTotCentre)

                ' Formule p.10 du manuscrit
                Fmes = 2 * Pi * Peau * QGRayon(-Lmes / 2, Lmes / 2, Rext0 - EpTotExtre, RextMax - EpTotCentre, Lt)

                ' Force totale correspondant à la contrepression hydraulique sur la lanterne
                If Not Lame Then

                    ' Formule p.20 du manuscrit pour une surface totale autour de la lanterne
                    ' Feau = 16 * Pi * Pg * DRmax * Lt / 15 + 2 * Pi * Pg * Rext0 * Lt

                    Feau = 2 * Pi * Pg * QGRayon(-Lt / 2, Lt / 2, Rext0, RextMax, Lt)
                Else

                    ' Formule pour une surface extérieure qui ne change pas quand
                    ' on calcule uniquement sur les lames
                    ' Feau = 2 * Pi * Pg * Rext0 * Lt
                    ' Feau = 32 * Pi * Pg * DRmax * Inte(Lt, Lcel / 2, -Lcel / 2) + 2 * Pi * Pg * Rext0 * Lt

                    Feau = 2 * Pi * Pg * Rext0 * Lt
                End If

                ' force sur la partie mesure due à la contrepression hydraulique
                ' sur la cellule de mesure
                ' Formule p.20 du manuscrit
                ' dFeauMes = Pg * ((Rext0 - RextMax) / 3 + RextMax) * Lmes
                dFeauMes = Pg * QGRayon(-Lmes / 2, Lmes / 2, Rext0, RextMax, Lt)

                FeauMes = 2 * Pi * dFeauMes

                Feau = FNappeLanterne(Lt, Lmes, Lcel, RextMax, Rext0, Pg)

                ' Calcul de la pression de réaction élastique maximum du sol
                ' avec une surface de contact égale à lenveloppe de la lanterne

                PsolMax = PressionSolMax(RextMax, Rext0, Lt, Fcel, Feau)
                '

                ' Recherche de la pression moyenne initiale sur la lanterne
                Psol = PressionSolMoy(RextMax, Rext0, Lt, Lmes, PsolMax)
                MesuresEssaiCalcule.contrainteRadiale = Psol

                ' Initialisation du calcul de vérification
                Fsol = FSolLanterneElast(Lt, RextMax, Rext0, PsolMax)
                Fsolt = FSolLanterneElastGauss(Lt, RextMax, Rext0, PsolMax)


                If Math.Abs((Fsol - Fsolt) / Fsol) > 0.01 Then MsgBox("Erreur d'équilibrage élastique du sol")

                'PsolMax = Pres - Pcap
                '        PSolMin = P0i
                PsolMin = 0

                ' L'équilibre élastique de la lanterne est atteint

                If Fsolt <= 0 Then
                    Fsolt = 0.0001
                    PsolMax = 0
                End If

                ' Calcul de Psol Moyen
                Psol = PressionSolMoy(RextMax, Rext0, Lt, Lmes, PsolMax)
                MesuresEssaiCalcule.contrainteRadiale = Psol

                DPSolmax = PsolMax - PsolMin
                'Fsolt = 2 * Pi * Rext0 * QGPression(-Lt / 2, Lt / 2, 0, PsolMax, Lt)
                Fsolt = FSolLanterneElast(Lt, RextMax, Rext0, PsolMax)

                If PsolMax < PsolMin Then
                    ' PsolMax = P0i
                    DPSolmax = 0
                    Elast = True
                End If


                If (((Fcel - Feau) / Fsolt > (1 + Precision)) _
               Or (Fcel - Feau) / Fsolt < (1 - Precision)) _
               And (Psol > P0) And (Fcel > Feau) Then MsgBox("Erreur d'équilibrage élastique")

                ' Limitation de PSol à PNF.Cep
                If Psol > Peau * (Rext0 - EpTotCentre) / Rext0 Then Psol = Peau * (Rext0 - EpTotCentre) / Rext0
                MesuresEssaiCalcule.contrainteRadiale = Psol

                If indice1 = 0 Then
                    Psol = 0
                    DRsol = 0
                End If

                ' suppression des déformations négatives
                If DRsol < 0 Then DRsol = 0
                MesuresEssaiCalcule.deformation = DRsol

                'Mémorisation de la pression moyenne sur la cellule de mesure
                '' SetMesures( Ligne, Colonne, Valeur)
                'Call Essai.SetMesures(indice1, 7, Math.Round(Psol, 1))
                ''Mémorisation de la déformation moyenne sur la cellule de mesure
                'Call Essai.SetMesures(indice1, 9, Math.Round(DRsol, 7))
                ''Mémorisation de la contrainte verticale sur la cellule de mesure
                'Call Essai.SetMesures(indice1, 8, Math.Round(Sz, 1))
                ''Call MyEssai.SetMesures(indice1, 7, Math.Round(Psol, 1))
                ''Call MyEssai.SetMesures(indice1, 9, Math.Round(DRsol, 7))
                ''Call MyEssai.SetMesures(indice1, 8, Math.Round(Sz, 1))
                Essai.UpdateMesuresEssaiForage(indice1, MesuresEssaiCalcule)

            Next indice1


        End Function

        '**********************************************************************************************
        ' Calcul de la force totale de réaction élastique du sol
        ' sur la lanterne et pour la longueur de la cellule de mesure
        ' Lt est la longueur totale de la sonde
        ' Lmes est la longueur de la cellule de mesure
        ' X3 est l'abscisse du début de la zone plastique
        '*********************************************************************************************

        Public Function FSolLanterne(ByVal Lt As Single, ByVal Lmes As Single, ByVal X3 As Single, ByVal DRmax As Single, ByVal Rext0 As Single, ByVal DPSolmax As Single, ByVal PsolPlast As Single,
                    ByVal PsolMin As Single, ByVal Lame As Boolean) As Single
            Dim Fsol As Single

            If Not Lame Then

                If X3 = 0 Then

                    ' Formule p.37.2 du manuscrit
                    ' Sol en élasticité
                    Fsol = 512 * Pi * DPSolmax * DRmax * IntCarre(Lt, Lmes / 2, -Lmes / 2) +
                      32 * Pi * (PsolMin * DRmax + Rext0 * DPSolmax) * Inte(Lt, Lmes / 2, -Lmes / 2) +
                      2 * Pi * PsolMin * Rext0 * Lmes
                Else
                    ' Formule p.3.6.1 du manuscrit "Cas du tube fendu"
                    ' Sol en élasto-plasticité
                    Fsol = 1024 * Pi * DPSolmax * DRmax * IntCarre(Lt, -X3, -Lmes / 2) _
                      + 64 * Pi * (PsolMin * DRmax + Rext0 * DPSolmax) * Inte19(Lt, -X3, -Lmes / 2) _
                      + 2 * Pi * PsolMin * Rext0 * (Lmes - 2 * X3) _
                      + 32 * Pi * PsolPlast * DRmax * Inte20(Lt, X3, -X3) _
                      + 4 * Pi * PsolPlast * Rext0 * X3

                End If

            End If 'not lame

            If Lame Then

                If X3 = 0 Then
                    Fsol = 32 * Pi * Rext0 * DPSolmax * Inte(Lt, Lmes / 2, -Lmes / 2) +
                      2 * Pi * PsolMin * Rext0 * Lmes
                Else
                    Fsol = 64 * Pi * Rext0 * DPSolmax * Inte(Lt, -X3, -Lmes / 2) _
                      + 2 * Pi * PsolMin * Rext0 * (Lmes - 2 * X3) _
                      + 4 * Pi * PsolPlast * Rext0 * X3
                End If

            End If
            FSolLanterne = Fsol
        End Function

        '**********************************************************************************************
        ' Calcul de la force totale de réaction élastique du sol
        ' sur la lanterne et pour la longueur de la cellule de mesure
        ' Lt est la longueur totale de la sonde
        ' Lmes est la longueur de la cellule de mesure
        ' X3 est l'abscisse du début de la zone plastique
        '*********************************************************************************************

        Public Function FSolLanterneElast(ByVal Lt As Single, ByVal Rmax As Single, ByVal Rext0 As Single, ByVal Pmax As Single) As Single
            Dim Fsol, dRmax As Single

            dRmax = Rmax - Rext0

            ' Relation 196, p.20 Formulaire Tube fendu
            Fsol = 512 / 630 * Pi * Pmax * dRmax * Lt + 32 / 30 * Pi * Pmax * Rext0 * Lt
            FSolLanterneElast = Fsol
        End Function


        '**********************************************************************************************
        ' Calcul de la force totale de réaction élastique du sol
        ' sur la lanterne et pour la longueur de la cellule de mesure
        ' Lt est la longueur totale de la sonde
        ' Lmes est la longueur de la cellule de mesure
        ' X3 est l'abscisse du début de la zone plastique
        '*********************************************************************************************

        Public Function FSolLanterneElastGauss(ByVal Lt As Single, ByVal Rmax As Single, ByVal Rext0 As Single, ByVal Pmax As Single) As Single
            Dim Fsol, Y, Z, Bsup, Binf As Single
            Dim H(5), A(5) As Single
            Dim i As Integer

            ' Sol en élasticité
            ' La Formule 163, p.16 formulaire Tube Fendu du manuscrit ne marche pas

            ' L'intégration numérique par la méthode de la quadrature de Gauss
            ' Coefficients de Gauss
            A(1) = -0.906179845938664
            A(2) = -0.538469310105683
            A(3) = 0
            A(4) = 0.538469310105683
            A(5) = 0.906179845938664
            H(1) = 0.236926885056189
            H(2) = 0.478628670499366
            H(3) = 0.568888888888889
            H(4) = 0.478628670499366
            H(5) = 0.236926885056189

            Fsol = 0
            Bsup = Lt / 2
            Binf = -Lt / 2
            For i = 1 To 5

                'Z = A(i) * Lt / 2
                'Fsol = Fsol + H(i) * Pi * Lt * ForcePressionLanterne(Lt, Rmax, Rext0, Pmax, Z)
                ' Changement des bornes d'intégration pour passer dans l'intervalle -1 / +1
                Y = (Bsup + Binf) / 2 + A(i) * (Bsup - Binf) / 2
                Z = 2 * Pi * PressionLanterne(0, Pmax, Lt, Y) * DeformeeLanterne(Rext0, Rmax, Lt, Y)

                ' Sommation de Gauss
                Fsol = Fsol + (Bsup - Binf) / 2 * H(i) * Z

            Next i

            ''Fsol = Pi * PSolmax * (256 / 315 * DRmax * Lt + 64 / 3 * Rext0 * Lt)
            'Ssol = Pi * (1 / 630 * DRmax * Lt + 1 / 30 * Rext0 * Lt)
            'Fsol = PSolmax * Ssol

            FSolLanterneElastGauss = Fsol
        End Function

        ' ***************************************************************************************
        ' Fonction Produit Pression Surface élémentaire
        ' ***************************************************************************************
        Public Function ForcePressionLanterne(ByVal Lt As Single, ByVal Rmax As Single, ByVal Rext0 As Single, ByVal Pmax As Single, ByVal X As Single) As Single
            Dim R, dRMax, P, Coef0 As Single

            dRMax = Rmax - Rext0
            Coef0 = (2 * X + Lt) ^ 2 / (4 * Lt ^ 2)
            Coef0 = Coef0 - (2 * X + Lt) ^ 3 / (4 * Lt ^ 3)
            Coef0 = Coef0 + (2 * X + Lt) ^ 4 / (16 * Lt ^ 4)
            R = 16 * dRMax * Coef0 + Rext0
            P = 16 * Pmax * Coef0

            ForcePressionLanterne = 2 * Pi * R * P
        End Function

        '**********************************************************************************************
        ' Calcul de la force totale de réaction élastique du sol
        ' sur la lanterne et pour la longueur de la cellule de mesure
        ' Lt est la longueur totale de la sonde
        ' Lmes est la longueur de la cellule de mesure
        ' X3 est l'abscisse du début de la zone plastique
        '*********************************************************************************************

        Public Function FCelLanterne(ByVal Lt As Single, ByVal Lmes As Single, ByVal Lcel As Single, ByVal Rmax As Single, ByVal Rext0 As Single,
                                 ByVal Pair As Single, ByVal PEau As Single, ByVal Epair As Single, ByVal EpEau As Single) As Single
            Dim I15, I16, I17, I18, I19, I20, dRmax, FAir, FEau, SAir, SEau As Single

            I15 = 1 / (24 * Lt ^ 2) * ((Lt - Lmes) ^ 3 - (Lt - Lcel) ^ 3)
            I16 = 1 / (32 * Lt ^ 3) * ((Lt - Lmes) ^ 4 - (Lt - Lcel) ^ 4)
            I17 = 1 / (160 * Lt ^ 4) * ((Lt - Lmes) ^ 5 - (Lt - Lcel) ^ 5)
            I18 = 1 / (24 * Lt ^ 2) * ((Lt + Lmes) ^ 3 - (Lt - Lmes) ^ 3)
            I19 = 1 / (32 * Lt ^ 3) * ((Lt + Lmes) ^ 4 - (Lt - Lmes) ^ 4)
            I20 = 1 / (160 * Lt ^ 4) * ((Lt + Lmes) ^ 5 - (Lt - Lmes) ^ 5)
            dRmax = Rmax - Rext0

            ' Pression des cellules de garde, eq.98 p.10 Formulaire Tube Fendu
            SAir = 64 * Pi * dRmax * (I15 - I16 + I17) + 2 * Pi * (Rext0 - Epair) * (Lcel - Lmes)
            FAir = Pair * SAir

            ' Pression de la cellule de mesure eq.98 p.10 Formulaire Tube Fendu
            SEau = 32 * Pi * dRmax * (I18 - I19 + I20) + 2 * Pi * (Rext0 - EpEau) * Lmes
            FEau = PEau * SEau

            FCelLanterne = FAir + FEau
        End Function

        '**********************************************************************************************
        ' Calcul de la force totale de réaction élastique du sol
        ' sur la lanterne et pour la longueur de la cellule de mesure
        ' Lt est la longueur totale de la sonde
        ' Lmes est la longueur de la cellule de mesure
        ' X3 est l'abscisse du début de la zone plastique
        '*********************************************************************************************

        Public Function FNappeLanterne(ByVal Lt As Single, ByVal Lmes As Single, ByVal Lcel As Single, ByVal Rmax As Single, ByVal Rext0 As Single, ByVal PEau As Single) As Single
            Dim dRmax, Feau As Single

            dRmax = Rmax - Rext0
            ' Pression de l'eau eq.119 p.12 Formulaire Tube Fendu
            Feau = 16 / 15 * Pi * PEau * dRmax * Lt + 2 * Pi * PEau * Rext0 * Lt
            FNappeLanterne = Feau
        End Function


        ' Formule p.2 du manuscrit
        ' Intégration de la forme en poutre encastrée de la lanterne
        ' Calcul d'intégrale entre les bornes Bsup et Binf de la relation
        ' {[(2.x+L)^2/(4.L^2) - (2.x+L)^3/(4.L^3) + (2.x+L)^4/(16.L^4)]}dx
        '
        Public Function Inte(ByVal L As Single, ByVal Bsup As Single, ByVal Binf As Single) As Single

            Dim Inte1, Inte2, Inte3 As Single
            Dim Debut, fin As Single

            Debut = (2 * Bsup + L) / 2
            fin = (2 * Binf + L) / 2

            ' Relation I.19.1 p. 3.6.4 du manuscrit
            '  Inte1 = ((Debut ^ 3) - (fin ^ 3)) / (24 * (L ^ 2))
            Inte1 = 1 / (3 * L ^ 2) * (Debut ^ 3 - fin ^ 3)   ' I6_3
            If Inte1 < 0 Then
                Inte1 = 0
            End If

            ' Relation p. 3.3 du manuscrit
            '  Inte2 = -((Debut ^ 4) - (fin ^ 4)) / (32 * (L ^ 3))
            Inte2 = -2 / (4 * L ^ 3) * (Debut ^ 4 - fin ^ 4)  ' I6_2
            If Inte2 > 0 Then
                Inte2 = 0
            End If

            ' relation p. 3.3 du manuscrit
            '  Inte3 = ((Debut ^ 5) - (fin ^ 5)) / (160 * (L ^ 4))
            Inte3 = 1 / (5 * L ^ 4) * (Debut ^ 5 - fin ^ 5)   ' I6_1
            If Inte3 < 0 Then
                Inte3 = 0
            End If
            Inte = Inte1 + Inte2 + Inte3

        End Function

        ' ***************************************************
        ' Function de calcul du Rayon maximum sonde TubeFendu
        ' ***************************************************
        ' Eq. page 29.7 manuscrit
        Public Function RMembMaxTubeFendu(ByVal Vext0 As Single, ByVal dV As Single, ByVal Lt As Single, ByVal Ls As Single,
                             ByVal R0 As Single, ByVal Ep As Single) As Single
            Dim Coef0, Coef1, Coef2, Pi, I2, I3, I4, I5, I6, I10, I11, R0p As Single
            Dim Resol As Solution

            Pi = 3.141592654

            ' Coef de puissance 2
            I2 = 1 / (160 * Lt ^ 4) * ((Lt + Ls) ^ 5 - (Lt - Ls) ^ 5)   ' Correspond à I7.1 du doc. papier

            'I3 = 1 / (224 * Lt ^ 6) * ((Lt + Ls) ^ 7 - (Lt - Ls) ^ 7)  ' Correspond à I7.2 du doc. papier 
            I3 = 3 / (448 * Lt ^ 6) * ((Lt + Ls) ^ 7 - (Lt - Ls) ^ 7)

            I4 = 1 / (4608 * Lt ^ 8) * ((Lt + Ls) ^ 9 - (Lt - Ls) ^ 9)  ' Correspond à I7.3 du doc. papier
            I5 = 1 / (192 * Lt ^ 5) * ((Lt + Ls) ^ 6 - (Lt - Ls) ^ 6)   ' Correspond à 1/2*I7.4 du doc. papier
            I6 = 1 / (1024 * Lt ^ 7) * ((Lt + Ls) ^ 8 - (Lt - Ls) ^ 8)  ' Correspond à 1/2*I7.5 du doc. papier
            'I7 = 1 / (896 * Lt ^ 6) * ((Lt + Ls) ^ 7 - (Lt - Ls) ^ 7)

            ' Coef de Puissance 1
            I10 = 1 / (24 * Lt ^ 2) * ((Lt + Ls) ^ 3 - (Lt - Ls) ^ 3)
            I11 = 1 / (32 * Lt ^ 3) * ((Lt + Ls) ^ 4 - (Lt - Ls) ^ 4)
            'I12 = 1 / (160 * Lt ^ 4) * ((Lt + Ls) ^ 5 - (Lt - Ls) ^ 5)

            ' Equations 55 formulaire Tube Fendu
            'Coef2 = 256 * Pi * (I2 + I3 + I4 - 2 * I5 - 2 * I6 + 2 * I7)
            Coef2 = 256 * Pi * (I2 + I3 + I4 - 2 * I5 - 2 * I6)

            ' Equations 69 formulaire Tube Fendu
            'Coef1 = 32 * Pi * (R0 - Ep) * (I10 - I11 + I12)
            Coef1 = 32 * Pi * (R0 - Ep) * (I10 - I11 + I2)

            ' Vint0 est le Vinterne de la sonde
            ' Equations 70 formulaire Tube Fendu
            R0p = (Vext0 / (Pi * Ls)) ^ 0.5
            Coef0 = Pi * (R0p - Ep) ^ 2 * Ls + Pi * Ep * (2 * R0p - Ep) * Ls - Vext0 - dV

            Resol = RACINE2(Coef2, Coef1, Coef0)
            RMembMaxTubeFendu = Resol.X1 + R0

        End Function

        ' Formule p.3.6.1 du manuscrit
        ' Intégration du carré de la forme en poutre encastrée de la lanterne
        ' Calcul d'intégrale entre les bornes Bsup et Binf de la relation
        ' {[(2.x+L)^2/(4.L^2) - (2.x+L)^3/(4.L^3) + (2.x+L)^4/(16.L^4)]}^2.dx
        '
        Public Function IntCarre(ByVal L As Single, ByVal Bsup As Single, ByVal Binf As Single) As Single


            Dim Int1, Int2, Int3, Int4, Int5, Int6, Int7 As Single
            Dim Debut, fin As Single
            Debut = (2 * Bsup + L) / 2
            fin = (2 * Binf + L) / 2

            ' Relation I.18.1 p.3.6.2 du manuscrit
            '  Int1 = ((Debut ^ 5) - (fin ^ 5)) / (160 * (L ^ 4))
            Int1 = 1 / (5 * L ^ 4) * (Debut ^ 5 - fin ^ 5)   ' I7_3
            If Int1 < 0 Then
                Int1 = 0
            End If

            ' Relation I.18.2 p.3.6.2 du manuscrit
            '  Int2 = ((Debut ^ 7) - (fin ^ 7)) / (224 * (L ^ 6))
            Int2 = 4 / (7 * L ^ 6) * (Debut ^ 7 - fin ^ 7)    ' I7_2
            If Int2 < 0 Then
                Int2 = 0
            End If

            ' Relation I.18.3 p.3.6.2 du manuscrit
            'Int3 = ((Debut ^ 9) - (fin ^ 9)) / (4608 * (L ^ 8))
            Int3 = 1 / (9 * L ^ 8) * (Debut ^ 9 - fin ^ 9)    ' I7_1
            If Int3 < 0 Then
                Int3 = 0
            End If

            ' Relation I.18.4 p.3.6.3 du manuscrit
            '  Int4 = -((Debut ^ 6) - (fin ^ 6)) / (96 * (L ^ 5))
            Int4 = -4 / (6 * L ^ 5) * (Debut ^ 6 - fin ^ 6)   '  I7_5
            If Int4 > 0 Then
                Int4 = 0
            End If

            ' Relation I.18.5 p.3.6.3 du manuscrit
            '  Int5 = -((Debut ^ 8) - (fin ^ 8)) / (512 * (L ^ 7))
            Int5 = -4 / (8 * L ^ 7) * (Debut ^ 8 - fin ^ 8)   ' I7_4
            If Int5 > 0 Then
                Int5 = 0
            End If

            ' Relation I.18.6 p.3.6.4 du manuscrit
            '  Int6 = ((Debut ^ 7) - (fin ^ 7)) / (448 * (L ^ 6))
            Int6 = 2 / (7 * L ^ 6) * (Debut ^ 7 - fin ^ 7)    '  I7_6
            If Int6 < 0 Then
                Int6 = 0
            End If

            ' Relation I.18 p.3.6.2 du manuscrit
            Int7 = Int1 + Int2 + Int3 + Int4 + Int5 + Int6
            IntCarre = Int7

        End Function

        ' Formule p.3.6 du manuscrit
        ' Intégration de la forme en poutre encastrée de la lanterne
        ' Calcul d'intégrale entre les bornes Bsup et Binf de la relation
        ' {[(2.x+L)^2/(4.L^2) - (2.x+L)^3/(4.L^3) + (2.x+L)^4/(16.L^4)]}dx
        '
        Public Function Inte19(ByVal L As Single, ByVal Bsup As Single, ByVal Binf As Single) As Single

            Dim Inte1, Inte2, Inte3 As Single
            Dim Debut, fin As Single

            Debut = 2 * Bsup + L
            fin = 2 * Binf + L

            ' Relation I.19.1 p. 3.6.4 du manuscrit
            Inte1 = ((Debut ^ 3) - (fin ^ 3)) / (24 * (L ^ 2))
            If Inte1 < 0 Then
                Inte1 = 0
            End If

            ' Relation I.19.2 p. 3.6.5 du manuscrit
            Inte2 = -((Debut ^ 4) - (fin ^ 4)) / (32 * (L ^ 3))
            If Inte2 > 0 Then
                Inte2 = 0
            End If

            ' relation I.19.3 p. 3.6.5 du manuscrit
            Inte3 = ((Debut ^ 5) - (fin ^ 5)) / (160 * (L ^ 4))
            If Inte3 < 0 Then
                Inte3 = 0
            End If
            Inte19 = Inte1 + Inte2 + Inte3

        End Function

        ' Formule p.3.6 du manuscrit
        ' Intégration de la forme en poutre encastrée de la lanterne
        ' Calcul d'intégrale entre les bornes Bsup et Binf de la relation
        ' {[(2.x+L)^2/(4.L^2) - (2.x+L)^3/(4.L^3) + (2.x+L)^4/(16.L^4)]}dx
        '
        Public Function Inte20(ByVal L As Single, ByVal Bsup As Single, ByVal Binf As Single) As Single

            Dim Inte1, Inte2, Inte3 As Single
            Dim Debut, fin As Single

            Debut = L + 2 * Bsup
            fin = L - 2 * Binf

            ' Relation I.20.1 p. 3.6.4 du manuscrit
            Inte1 = ((Debut ^ 3) - (fin ^ 3)) / (24 * (L ^ 2))
            If Inte1 < 0 Then
                Inte1 = 0
            End If

            ' Relation I.19.2 p. 3.6.5 du manuscrit
            Inte2 = -((Debut ^ 4) - (fin ^ 4)) / (32 * (L ^ 3))
            If Inte2 > 0 Then
                Inte2 = 0
            End If

            ' relation I.19.3 p. 3.6.5 du manuscrit
            Inte3 = ((Debut ^ 5) - (fin ^ 5)) / (160 * (L ^ 4))
            If Inte3 < 0 Then
                Inte3 = 0
            End If
            Inte20 = Inte1 + Inte2 + Inte3

        End Function

        Public Function QGPression(ByVal Binf As Single, ByVal Bsup As Single,
           ByVal P0 As Single, ByVal Pmax As Single, ByVal L As Single) As Single
            ' Intégration selon la formule de la quadrature de Gauss
            ' pour une fonction de type degré 3, soit 2*n-1 =5

            Dim A(3) As Single
            Dim H(3) As Single
            Dim x, Y, Z, Somme, Poids As Single
            Dim i As Integer


            If Binf > Bsup Then
                MsgBox("La borne inférieure est supérieure à la borne inférieure")
            End If

            A(1) = 0.906179845938664
            A(2) = 0.538469310105683
            A(3) = 0
            H(1) = 0.236926885056189
            H(2) = 0.478628670499366
            H(3) = 0.568888888888889

            Somme = 0

            'il faut passer en paramètres p0, pmax....
            For i = 1 To 5

                ' calcul des paramètres d'intégration de -1 à +1
                If i < 3 Then
                    x = -A(i)
                    Poids = H(i)
                Else
                    x = A(6 - i)
                    Poids = H(6 - i)
                End If

                ' Changement des bornes d'intégration pour passer dans l'intervalle -1 / +1
                Y = (Bsup + Binf) / 2 + x * (Bsup - Binf) / 2
                Z = PressionLanterne(P0, Pmax, L, Y)

                ' Sommation de Gauss
                Somme = Somme + (Bsup - Binf) / 2 * Poids * Z
            Next i

            ' Résultat final
            QGPression = Somme
        End Function

        Public Function QGRayon(ByVal Binf As Single, ByVal Bsup As Single,
           ByVal R0 As Single, ByVal RMAX As Single, ByVal L As Single) As Single
            ' Intégration selon la formule de la quadrature de Gauss
            ' pour une fonction de type degré 3, soit 2*n-1 =5

            Dim A(3) As Single
            Dim H(3) As Single
            Dim x, Y, Z, Somme, Poids As Single
            Dim i As Integer

            If Binf > Bsup Then
                MsgBox("La borne inférieure est supérieure à la borne inférieure")
            End If

            ' Abscisses
            A(1) = 0.906179845938664
            A(2) = 0.538469310105683
            A(3) = 0

            ' Poids
            H(1) = 0.236926885056189
            H(2) = 0.478628670499366
            H(3) = 0.568888888888889

            Somme = 0

            For i = 1 To 5

                ' calcul des paramètres d'intégration de -1 à +1
                If i < 3 Then
                    x = -A(i)
                    Poids = H(i)
                Else
                    x = A(6 - i)
                    Poids = H(6 - i)
                End If

                ' Changement des bornes d'intégration pour passer dans l'intervalle -1 / +1
                Y = (Bsup + Binf) / 2 + x * (Bsup - Binf) / 2
                Z = DeformeeLanterne(R0, RMAX, L, Y)

                ' Sommation de Gauss
                Somme = Somme + (Bsup - Binf) / 2 * Poids * Z
            Next i

            ' Résultat final
            QGRayon = Somme
        End Function

        Public Function QGPressionRayon(ByVal Binf As Single, ByVal Bsup As Single,
           ByVal P0 As Single, ByVal Pmax As Single,
           ByVal R0 As Single, ByVal RMAX As Single,
           ByVal L As Single) As Single
            ' Intégration selon la formule de la quadrature de Gauss
            ' pour une fonction de type degré 3, soit 2*n-1 =5

            Dim A(3) As Single
            Dim H(3) As Single
            Dim x, Y, Z, Somme, Poids As Single
            Dim i As Integer


            If Binf > Bsup Then
                MsgBox("La borne inférieure est supérieure à la borne inférieure")
            End If

            A(1) = 0.906179845938664
            A(2) = 0.538469310105683
            A(3) = 0
            H(1) = 0.236926885056189
            H(2) = 0.478628670499366
            H(3) = 0.568888888888889

            Somme = 0

            'il faut passer en paramètres p0, pmax....
            For i = 1 To 5

                ' calcul des paramètres d'intégration de -1 à +1
                If i < 3 Then
                    x = -A(i)
                    Poids = H(i)
                Else
                    x = A(6 - i)
                    Poids = H(6 - i)
                End If

                ' Changement des bornes d'intégration pour passer dans l'intervalle -1 / +1
                Y = (Bsup + Binf) / 2 + x * (Bsup - Binf) / 2
                Z = PressionLanterne(P0, Pmax, L, Y) * DeformeeLanterne(R0, RMAX, L, Y)

                ' Sommation de Gauss
                Somme = Somme + (Bsup - Binf) / 2 * Poids * Z
            Next i

            ' Résultat final
            QGPressionRayon = Somme
        End Function


        Public Function DeformeeLanterne(ByVal R0 As Single, ByVal RMAX As Single,
            ByVal L As Single, ByVal x As Single) As Single
            ' calcul de la déformée de la sonde lanternée
            Dim X1 As Single

            ' X1 varie de -L/2 à +L/2
            If (x > L / 2) Or (x < -L / 2) Then
                MsgBox("Les bornes de l'intégration ne sont pas dans les limites admissibles")
            End If
            X1 = 2 * x + L

            ' relation p.2 du manuscrit
            DeformeeLanterne = 16 * (RMAX - R0) * (X1 ^ 2 / (4 * L ^ 2) - X1 ^ 3 / (4 * L ^ 3) +
                           X1 ^ 4 / (16 * L ^ 4)) + R0

        End Function

        Public Function PressionSolMax(ByVal RextMax As Single, ByVal Rext0 As Single, ByVal Lt As Single,
                                   ByVal FCell As Single, ByVal Fw As Single) As Single
            ' calcul de pression de réaction maximum élastique du sol
            Dim Psol, dRmax As Single

            ' relation 169 p.17 du formulaire Tube Fendu
            dRmax = RextMax - Rext0
            Psol = (FCell - Fw) / (Pi * (512 / 630 * dRmax * Lt + 32 / 30 * Rext0 * Lt))
            If Psol < 0 Then Psol = 0

            PressionSolMax = Psol
        End Function

        Public Function PressionSolMoy(ByVal RextMax As Single, ByVal Rext0 As Single, ByVal Lt As Single, ByVal Lmes As Single, ByVal dPMax As Single) As Single
            ' calcul de pression de réaction maximum élastique du sol
            Dim Psol, dRmax As Single
            Dim I34, I35, I36, I37, I38, I39, I40, I41, I42 As Single

            ' Eq. 174 à 190, p.18-19 Formulaire Tube Fendu
            I34 = 1 / (160 * Lt ^ 4) * ((Lt + Lmes) ^ 5 - (Lt - Lmes) ^ 5)
            I35 = 1 / (224 * Lt ^ 6) * ((Lt + Lmes) ^ 7 - (Lt - Lmes) ^ 7)
            I36 = 1 / (4608 * Lt ^ 8) * ((Lt + Lmes) ^ 9 - (Lt - Lmes) ^ 9)
            I37 = 1 / (192 * Lt ^ 5) * ((Lt + Lmes) ^ 6 - (Lt - Lmes) ^ 6)
            I38 = 1 / (1024 * Lt ^ 7) * ((Lt + Lmes) ^ 8 - (Lt - Lmes) ^ 8)
            I39 = 1 / (896 * Lt ^ 6) * ((Lt + Lmes) ^ 7 - (Lt - Lmes) ^ 7)
            I40 = 1 / (24 * Lt ^ 2) * ((Lt + Lmes) ^ 3 - (Lt - Lmes) ^ 3)
            I41 = 1 / (32 * Lt ^ 3) * ((Lt + Lmes) ^ 4 - (Lt - Lmes) ^ 4)
            I42 = 1 / (160 * Lt ^ 4) * ((Lt + Lmes) ^ 5 - (Lt - Lmes) ^ 5)

            ' relation 191 p.19 du formulaire Tube Fendu
            dRmax = RextMax - Rext0
            Psol = (256 * dPMax * dRmax * (I34 + I35 + I36 - 2 * I37 - 2 * I38 + 2 * I39) + 16 * dPMax * Rext0 * (I40 - I41 + I42)) _
                / (16 * dRmax * (I40 - I41 + I42) + Rext0 * Lmes)
            If Psol < 0 Then Psol = 0
            PressionSolMoy = Psol
        End Function

        Public Function PressionLanterne(ByVal P0 As Single, ByVal Pmax As Single,
            ByVal L As Single, ByVal x As Single) As Single
            ' calcul de la déformée de la sonde lanternée
            Dim X1 As Single

            ' X varie de -L/2 à +L/2
            If (x > L / 2) Or (x < -L / 2) Then
                MsgBox("Les bornes de l'intégration ne sont pas dans les limites admissibles")
            End If
            X1 = 2 * x + L
            ' relation p.1 du manuscrit
            PressionLanterne = 16 * (Pmax - P0) * (X1 ^ 2 / (4 * L ^ 2) - X1 ^ 3 / (4 * L ^ 3) +
                           X1 ^ 4 / (16 * L ^ 4)) + P0
        End Function


    End Module
End Namespace