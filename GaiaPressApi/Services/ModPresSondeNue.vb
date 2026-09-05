Namespace GaiaPress.core.V17.Services

    Module ModPresSondeNue

        PUBLIC Const GammaW = 9.81
        PUBLIC ReadOnly Pcap() As Single
        PUBLIC Ndp As Integer
        PUBLIC ReadOnly fin As Boolean
        PUBLIC ReadOnly X1, X2 As Single
        PUBLIC ReadOnly Erreur As Integer
        PUBLIC ReadOnly A0I, A1I, A2I, A3I As Single
        PUBLIC ReadOnly VIN As Single
        PUBLIC ReadOnly DVCEL() As Object
        PUBLIC ReadOnly PCEL() As Object
        PUBLIC ReadOnly x As Single
        PUBLIC ReadOnly Z(0 To 5) As Single

        PUBLIC ReadOnly XArray(0 To 30) As Single
        PUBLIC ReadOnly YArray(0 To 30) As Single

        '*********************************************
        'Module de calcul
        'Ecrit par F.BOURAHLA le 16/05/2002
        'Procédure de la fonction PresSondeNue
        '********************************************
        Public Sub Correction(Essai As Essai_Forage)
            If Essai.MySonde Is Nothing Then
                Select Case Lang
                    Case 0 : MsgBox("You must select a probe")
                        Exit Sub
                    Case 1 : MsgBox("Vous devez sélectionner une sonde")
                        Exit Sub
                End Select
            End If
            With Essai.MySonde
                If .P_typeDeformee = 0 Then ' Nue = 0
                    Call PresSondeNue(Essai)
                End If
                If .P_typeDeformee = 1 Then ' Lanternee = 1
                    Call PresLanterne(Essai)
                End If
                Call PressionNF(Essai)
            End With

        End Sub

        '*******************************************************************************************
        ' Fonction qui calcule la contrainte de réaction du sol le long de la cellule de mesure
        ' Cas de la sonde nue
        ' Manuscrit de référence "Répartition des pressions dans le sol pour une sonde nue"
        ' par J.Monnet, 1990
        '*******************************************************************************************
        Public Sub PresSondeNue(Essai As Essai_Forage)
            ' Zs remplace Hsonde
            ' Zw remplace Hnappe

            Dim PmembMax, Psol, DRsol, Rsol, Pair As Single
            Dim RextMax, Deno, PsolMax, PsolMin, DFcel As Single
            Dim Heau, DFeau, Feau, Pnappe As Single
            Dim PSolMaxElast, Smes As Single
            Dim RayonSol As ValeurPression
            Dim Sz, P0, K0 As Single
            Dim FeauMes, dFeauMes As Single
            Dim Pres, V60 As Single
            Dim Elast As Boolean
            Dim Lame As Boolean
            Dim solu As Solution
            Dim result As Inertie

            ' Variables pour les caractéristiques de l'objet

            Dim Vext0, Vint0, P0i, Lmes, Lcel, Lt, Rext0, EpAir, EpEau, EpTotale, EpTotale0, Phi, dVTheo, R0p As Single
            Dim A, VolNF, dPInert, Phimu, Gamma, Gammadej, Nu, Zc, Zs, Zw As Single
            Dim Precision As Single
            Dim TypeDefo, TypeCont As Integer
            Dim indice1 As Integer
            Dim Pmemb, Pdh, Peau, Coef1, Coef2, Coef3, Coef4, B As Single
            Dim MesureCorrigee As MesuresEssaiForage

            Dim Sonde As String


            'chargement des variables
            Ndp = Essai.E_Mesures.Count
            Lame = False
            MesureCorrigee = New MesuresEssaiForage

            'Pression de membrane d'eau
            Pmemb = Essai.MySonde.GetPm

            Dim Pcap, PCAPair As Single

            With Essai
                ' Gestion des types dedéformée et de sonde
                TypeDefo = .MySonde.GetTypeDeformee
                TypeCont = .MySonde.GetTypeContrainte

                ' Dimensions de lasonde  (cm)
                Lt = .MySonde.Getlt / 10
                Lcel = .MySonde.Getlg / 10
                Lmes = .MySonde.Getls / 10
                Lcel = 2 * Lcel + Lmes
                Rext0 = .MySonde.Getds / 20

                ' Epaisseur des membranes (mm)
                EpEau = .MySonde.GetEpMembraneMesure
                EpAir = .MySonde.GetEpMembraneGarde
                EpTotale0 = .MySonde.GetPrEpMembraneCentre
                dPInert = .E_difPresEauAir
                P0i = .E_presP0
                Gamma = .E_poidsVol
                Gammadej = .E_poidsVolDejauge
                Nu = .E_coefPoisson
                Phimu = MySol.GetPhiMu
                Phi = .E_Frottement
                If Phi < Phimu Then Phi = Phimu
                Zc = .E_hauteurCpv
                Zw = .E_profNappe
                Zs = .E_profSonde
                Vext0 = .MySonde.GetVs
                Coef1 = .GetMyEssaiCalib(EssaiCalib.Resistance).A0
                Coef2 = .GetMyEssaiCalib(EssaiCalib.Resistance).A1
                Coef3 = .GetMyEssaiCalib(EssaiCalib.Resistance).A2
                Coef4 = .GetMyEssaiCalib(EssaiCalib.Resistance).A3
                A = .GetMyEssaiCalib(EssaiCalib.Expansion).CoefficientA
                B = .GetMyEssaiCalib(EssaiCalib.Expansion).CoefficientB
                Sonde = .MySonde.GetRefSonde
            End With
            If CompMemb Then
                ' EpTotale0 = MyExpansion.EpMembrane
                If EpTotale0 = 0 Then MsgBox("Attention : épaisseur de membrane nulle")
                EpTotale = EpTotale0
            Else
                EpTotale = EpEau + EpAir
            End If

            ' Passage de l'épaisseur desmembranes en cm
            EpTotale = EpTotale / 10    ' Epaisseur de membrane en cm

            R0p = Math.Sqrt(Vext0 / (Pi * Lmes))   ' Rayon externe de la sonde en cm
            Vint0 = Pi * (R0p - EpTotale) ^ 2 * Lmes       ' Volume interne initial de la sonde en cm3

            If Coef1 * Coef2 * Coef3 * Coef4 = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("Calibration Coefficient are null")
                    Case 1
                        MsgBox("Les coefficients de calibration sont nuls")
                End Select
                Exit Sub
            End If

            A = Essai.GetMyEssaiCalib(EssaiCalib.Expansion).GetCoefficientA

            If A = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("Dilatation Coefficient is null")
                    Case 1
                        MsgBox("Le Coefficient de Dilatation est nul")
                End Select
                Exit Sub
            End If

            ' Calcul de la pression maximum de membrane
            result = PresMembMax(Coef1, Coef2, Coef3, Coef4)
            PmembMax = result.PresInertie

            ' Boucle de calcul sur l'ensemble des résultats
            For indice1 = 0 To Ndp - 1

                ' e_Mesures( Colonne, Ligne, valeur)
                Pres = Essai.E_Mesures(indice1).Pression
                V60 = Essai.E_Mesures(indice1).Volume60
                MesureCorrigee.RefEssai = Essai.E_Mesures(indice1).RefEssai
                MesureCorrigee.IndiceMesure = indice1
                MesureCorrigee.Pression = Essai.E_Mesures(indice1).Pression
                MesureCorrigee.Volume15 = Essai.E_Mesures(indice1).Volume15
                MesureCorrigee.Volume30 = Essai.E_Mesures(indice1).Volume30
                MesureCorrigee.Volume60 = Essai.E_Mesures(indice1).Volume60

                ' Prise en compte de la compression des membranes
                'EpTotale = EpEau + EpAir + B * Pres

                EpEau = Essai.MySonde.GetEpMembraneMesure
                EpAir = Essai.MySonde.GetEpMembraneGarde
                EpTotale0 = EpEau + EpAir

                If CompMemb Then
                    ' Prise en compte de la compression de membrane
                    EpTotale = EpTotale0 + B * Pres * 10    ' x10 pour passer le coefficient B en mm
                    ' Passage de l'épaisseur en cm
                    EpTotale = EpTotale / 10
                    ' Pas de correction de volume dans le cas de l 'épaisseur variable, la dilatation est reprise par B

                Else
                    ' La membrane reste d épaisseur constante
                    EpTotale = EpEau + EpAir    ' x10 pour passer le coefficient B en mm
                    ' Passage de l'épaisseur en cm
                    EpTotale = EpTotale / 10

                    'Calcul du volume corrigé de l'expansion dans le cas de l épaisseur constante, la dilatation est reprise par A
                    VolNF = V60 - A * Pres
                    If VolNF < 0 Then VolNF = 0
                End If

                Elast = False

                ' Calcul de la pression d'inertie à partir des mesures brutes
                result = PresMembrane(Coef1, Coef2, Coef3, Coef4, PmembMax, V60)
                Pcap = result.PresInertie

                PCAPair = Pcap - dPInert

                If PCAPair < 0 Then
                    PCAPair = 0
                End If

                If TypeCont = 0 Then
                    Lame = False
                Else
                    Lame = True
                End If

                ' ----------------------------------------------------------------------------------
                ' Calcul de la pression corrigee pour la sonde pressiométrique nue
                ' ----------------------------------------------------------------------------------
                If (TypeDefo = 0) Then

                    ' calcul de rayon maximum de la sonde nue

                    ''Coefficient AA multiplicateur de Rmax^2 dans l'équation du second degré, p.24 manuscrit
                    'AA = Pi * ((Lmes ^ 5) / 5 / (Lt ^ 4) - 2 * (Lmes ^ 3) / 3 / Lt ^ 2 + Lmes)
                    ''Coefficient BB multiplicateur de Rmax dans l'équation du second degré, p.24 manuscrit
                    'BB = -Pi * 2 * Rext0 * (Lmes ^ 5) / 5 / (Lt ^ 4)
                    'BB = BB + Pi * (2 * (Lmes ^ 3) * (Rext0 + EpTotale) / 3 / Lt ^ 2 - 2 * Lmes * EpTotale)

                    ''Coefficient CC dans l'équation du second degré, p.24 manuscrit
                    'C = Pi * ((Rext0) ^ 2 * (Lmes ^ 5) / 5 / (Lt ^ 4))
                    'CC = C + Pi * (-2 * Rext0 * EpTotale * (Lmes ^ 3) _
                    '     / 3 / Lt ^ 2 + (EpTotale) ^ 2 * Lmes) - VolNF - Vint0

                    If CompMemb Then
                        ''Calcul de la solution positive X1, de l'équation du second degré, , p.24 manuscrit - Sonde nue
                        'solu = RACINE2(AA, BB, CC)
                        solu = RMembMaxSondeG(Vext0, V60, Lt, Lmes, R0p, EpTotale)
                        'solu = RMembMaxSondeG(Vint0, V60, Lt, Lmes, Rext0, EpTotale)
                    Else
                        ''Calcul de la solution positive X1, de l'équation du second degré, , p.24 manuscrit - Sonde nue
                        'solu = RACINE2(AA, BB, CC)
                        solu = RMembMaxSondeG(Vext0, VolNF, Lt, Lmes, R0p, EpTotale)
                        'solu = RMembMaxSondeG(Vint0, V60, Lt, Lmes, Rext0, EpTotale)
                    End If
                    If solu.Erreur = False Then
                        RextMax = solu.X1
                    End If
                    'If RextMax < Rext0 Then
                    '    RextMax = Rext0
                    'Else

                    '' Correction de Rext0 pour tenir compte de Vext0
                    'R0p = Math.Sqrt(Vext0 / (Pi * Lmes))
                    ' Vérification de RMax
                    dVTheo = VolumeG(Vext0, RextMax, Lt, Lmes, R0p, EpTotale)

                    Precision = 0.1
                    If MyEssai.E_ClassPrecis = 0 Then
                        Precision = 5
                    ElseIf MyEssai.e_ClassPrecis = 1 Then
                        Precision = 1
                    ElseIf MyEssai.e_ClassPrecis = 2 Then
                        Precision = 0.1
                    End If

                    If CompMemb Then
                        If Math.Abs(dVTheo - V60) > Precision Then
                            'If Math.Abs(dVTheo - V60) > 0.1 Then


                            MsgBox("Erreur de calcul du Rayon Maximum - DV=" & Str(dVTheo - VolNF) & "; pMes =" & Str(Pres) & " V60 =" & Str(V60))
                        End If
                        'End If
                    Else
                        If Math.Abs(dVTheo - VolNF) > Precision Then
                            'If Math.Abs(dVTheo - V60) > 0.1 Then


                            MsgBox("Erreur de calcul du Rayon Maximum - DV=" & Str(dVTheo - VolNF) & "; pMes =" & Str(Pres) & " V60 =" & Str(V60))
                        End If
                        'End If
                    End If

                    'Calcul de rayon extérieur moyen du sol au contact de la sonde nue, p.22 manuscrit - Sonde nue
                    Rsol = (R0p - RextMax) * (Lmes) ^ 2 / (3 * Lt ^ 2) + RextMax
                    RayonSol = RayonMoyenSondeG(R0p, RextMax, Lt, Lmes, PrDiam / 2, PrDiam / 2, PrLong, PrLong)
                    Rsol = RayonSol.Valeur

                    ' rayon moyen
                    DRsol = (Rsol - R0p) / R0p
                    MesureCorrigee.deformation = DRsol

                    PrEps = Math.Sqrt(1 / R0p ^ 2 * PrDiam ^ 2 / 4 + (1 / R0p + (Rsol - R0p) / R0p ^ 2) ^ 2 * PrDiam ^ 2 / 4)
                    'Calcul de la surface de mesure au contact du sol, p.22 manuscrit
                    Smes = 2 * Pi * Rsol * Lmes

                    'Calcul de l'etat de contrainte vertical et horizontal

                    ' cas de la sonde sous la nappe
                    If (Zs >= Zw) And (Zw >= 0) Then
                        Sz = (Zs - Zw) * Gammadej + (Zw * Gamma)

                        'cas de la sonde au dessus de la nappe
                    ElseIf (Zs < Zw) And (Zw >= 0) Then
                        Sz = Zs * Gamma

                        'cas de la sonde avec une nappe
                        ' à la surface du sol ou au dessus(fond d'un lac)
                    ElseIf Zw < 0 Then
                        Sz = Zs * Gammadej
                    End If
                    MesureCorrigee.contrainteVerticale = Sz

                    'Calcul du K0
                    K0 = Nu / (1 - Nu)

                    'Calcul de la pression des terres au repos
                    P0 = K0 * Sz
                    If P0 < P0i Then
                        P0i = P0
                    End If
                    PsolMin = 0

                    ' calcul de l'effort interne à la sonde apporté par les circuits d'air et d'eau
                    'p.18 part de l'effort apporté par les cellules de garde

                    'calcul de la pression de l'eau
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

                    ' Force interne dans la sonde pressiométrique, p.18 manuscrit
                    'DFcel = Pair * ((Rext0 - RextMax) * (Lcel ^ 3 - Lmes ^ 3) / (3 * Lt ^ 2) _
                    '      + (RextMax - EpAir) * (Lcel - Lmes)) _
                    '      + Peau * ((Rext0 - RextMax) * Lmes ^ 3 / (3 * Lt ^ 2) + (RextMax - EpTotale) * Lmes)
                    DFcel = FCelSondeG(Lt, Lmes, Lcel, RextMax, R0p, Pair, Peau, EpAir / 10, EpTotale)

                    ' Hauteur de terrain sous la nappe phréatique
                    Heau = Zs - Zw
                    If Heau < 0 Then
                        Heau = 0
                    End If

                    ' Force totale due à la contrepression hydraulique, p.19 manuscrit - Sonde Nue
                    DFeau = 9.81 * Heau * ((R0p - RextMax) / 3 + RextMax) * Lt
                    Feau = 2 * Pi * DFeau
                    Pnappe = 9.81 * Heau
                    Feau = FNappeSondeG(Lt, RextMax, R0p, Pnappe)

                    ' Force sur la partie mesure due à la contrepression hydraulique, p.19 manuscrit - Sonde nue
                    dFeauMes = 9.81 * Heau * ((R0p - RextMax) / 3 + RextMax) * Lt
                    FeauMes = 2 * Pi * dFeauMes

                    '   Calcul de la pression maximum de réaction du sol, p.20 manuscrit - Sonde Nue
                    '    Dénominateur
                    Deno = (2 * R0p + 8 * RextMax) * Lt / 15

                    'PSOLMAX:=(DFCEL-DFEAU-P0i*LT*(REXT0/5+2*REXTMAX/15))/DENO;

                    ' PsolMax ,rel.101 p.16 Formulaire Sonde Nue
                    ' avec correction liée à la différence entre Fsol rel.86 p.13 et rel.95, p.14
                    PsolMax = PressionSondeGSolMax(Lt, Lmes, Lcel, RextMax, R0p, Pair, Peau, EpAir / 10, EpTotale, Pnappe)

                    If PsolMax <= P0 Then
                        If PsolMax < 0 Then PsolMax = 0
                        Elast = True
                    End If

                    '   Sauvegarde dans une variable locale de la pression de réaction élastique maximale du sol
                    PSolMaxElast = PsolMax
                    Psol = PressionSondeGSolMoy(Lt, Lmes, PsolMax)


                    ' Limitation de PSol à PNF.Cep
                    If Psol > Peau * (R0p - EpTotale) / R0p Then Psol = Peau * (R0p - EpTotale) / R0p
                    MesureCorrigee.contrainteRadiale = Psol

                End If '(TypeDefo='NUE')

                If indice1 = 0 Then
                    Psol = 0
                    DRsol = 0
                End If

                If DRsol < 0 Then DRsol = 0

                ' suppression des déformations négatives
                'If DRsol < 0 Then DRsol = 0

                ''Mémorisation de la pression moyenne sur la cellule de mesure
                '' SetMesures( Ligne, Colonne, Valeur)
                'Call Essai.SetMesures(indice1, 7, Math.Round(Psol, 1))
                ''Mémorisation de la déformation moyenne sur la cellule de mesure
                'Call Essai.SetMesures(indice1, 9, Math.Round(DRsol, 7))
                ''Mémorisation de la contrainte verticale sur la cellule de mesure
                'Call Essai.SetMesures(indice1, 8, Math.Round(Sz, 1))
                'Call MyEssai.SetMesures(indice1, 7, Math.Round(Psol, 1))
                'Call MyEssai.SetMesures(indice1, 9, Math.Round(DRsol, 7))
                'Call MyEssai.SetMesures(indice1, 8, Math.Round(Sz, 1))
                Essai.UpdateMesuresEssaiForage(indice1, MesureCorrigee)

            Next indice1

        End Sub


        '*********************************************************************
        'Calcul de la force de réaction élastoplastique
        'du sol pour une sonde nue Lt est la longueur totale
        'de la sonde Lmes est la longueur de la cellule de mesure
        'X3 est l abscisse du debut de la zone plastique
        '*********************************************************************
        Public Function FSolNueElastPlast(ByVal Lt As Single, ByVal Lmes As Single,
                             ByVal X3 As Single, ByVal RMAX As Single,
                             ByVal Rext0 As Single, ByVal PsolMax As Single,
                             ByVal PsolPlast As Single, ByVal P0i As Single) As Single
            Dim Fsol, PsolMin As Single


            PsolMin = 0

            ' Intégrale I1 manuscrit p.30
            Fsol = 64 / 5 * Pi / Lt ^ 4 * (PsolMin - PsolMax) * (Rext0 - RMAX)
            Fsol = Fsol * ((Lmes ^ 5) / 32 - (X3 ^ 5))
            ' + Intégrale I2 manuscrit p.30
            Fsol = Fsol + 16 / 3 * Pi / Lt ^ 2 * (PsolMin - PsolMax) * ((Lmes ^ 3) / 8 - (X3 ^ 3)) * RMAX
            ' + Intégrale I3 manuscrit p.31
            Fsol = Fsol + 16 / 3 * Pi / Lt ^ 2 * (Rext0 - RMAX) * ((Lmes ^ 3) / 8 - (X3 ^ 3)) * PsolMax
            ' + Intégrale I4 manuscrit p.31
            Fsol = Fsol + 4 * Pi * PsolMax * RMAX * ((Lmes / 2) - X3)
            ' + Intégrale I5 manuscrit p.31
            Fsol = Fsol + 16 / 3 / Lt ^ 2 * Pi * PsolPlast * (X3 ^ 3) * (Rext0 - RMAX)
            ' + Intégrale I6 manuscrit p.31
            Fsol = Fsol + 4 * Pi * PsolPlast * RMAX * X3

            FSolNueElastPlast = Fsol
        End Function



        '*********************************************************************
        'Calcul de la force de réaction élastique
        'du sol pour une sonde nue Lt est la longueur totale
        'de la sonde Lmes est la longueur de la cellule de mesure
        '*********************************************************************
        Public Function FSolSondeGElastique(ByVal Lt As Single, ByVal Lmes As Single,
                             ByVal Rmax As Single, ByVal Rext0 As Single, ByVal PMax As Single, ByVal P0i As Single) As Single
            Dim Fsol, PsolMin As Single


            PsolMin = 0

            ' Formule 83, p.13 formulaire Sonde Nue du manuscrit
            'Fsol = 256 * 11 / 315 * Pi * PSolMax * (Rext0 - RMAX) * Lt
            'Fsol = Fsol + 64 / 3 * Pi * PSolMax * Lt * Rext0
            Fsol = Pi * PMax * (2 / 3 * (Rext0 - Rmax) * Lt + 2 * Rmax * Lt _
           - 2 / 5 * (Rext0 - Rmax) * Lt - 2 / 3 * Rmax * Lt)

            FSolSondeGElastique = Fsol
        End Function

        Public Function FSolSondeGElastGauss(ByVal Lt As Single, ByVal RMax As Single, ByVal Rext0 As Single, ByVal Pmax As Single) As Single
            Dim Fsol, Z, Pi As Single
            Dim H(5), A(5) As Single
            Dim i As Integer

            ' Sol en élasticité
            ' La Formule 163, p.16 formulaire Tube Fendu du manuscrit ne marche pas
            Pi = 3.141592654
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
            For i = 1 To 5
                Z = A(i) * Lt / 2
                Fsol = Fsol + H(i) * Lt / 2 * ForcePressionSondeG(Lt, RMax, Rext0, Pmax, Z)
            Next i

            ''Fsol = Pi * PSolmax * (256 / 315 * DRmax * Lt + 64 / 3 * Rext0 * Lt)
            'Ssol = Pi * (1 / 630 * DRmax * Lt + 1 / 30 * Rext0 * Lt)
            'Fsol = PSolmax * Ssol

            FSolSondeGElastGauss = Fsol
        End Function

        ' ***************************************************************************************
        ' Fonction Produit Pression Surface élémentaire
        ' ***************************************************************************************
        Public Function ForcePressionSondeG(ByVal Lt As Single, ByVal RMax As Single, ByVal Rext0 As Single, ByVal Pmax As Single, ByVal X As Single) As Single
            Dim R, P, Pi As Single

            Pi = 3.141592654
            R = 4 * (Rext0 - RMax) * (X ^ 2 / Lt ^ 2) + RMax
            P = -4 * Pmax * (X ^ 2 / Lt ^ 2) + Pmax

            ForcePressionSondeG = 2 * Pi * R * P
        End Function

        ' **********************************************************************************
        ' Calcul de la Force transmise à la sonde par les pressions internes d'air et d'eau
        ' **********************************************************************************
        Public Function FCelSondeG(ByVal Lt As Single, ByVal Lmes As Single, ByVal Lcel As Single, ByVal RMax As Single, ByVal Rext0 As Single,
                                     ByVal PAir As Single, ByVal PEau As Single, ByVal EpAir As Single, ByVal EpEau As Single) As Single
            Dim FAir, FEau, Pi, Sair, Seau As Single

            Pi = 3.141592654
            'I4 = 1 / (24 * Lt ^ 2) * (Lcel ^ 3 - Lmes ^ 3)
            'I5 = 1 / (12 * Lt ^ 3) * Lmes ^ 3
            'I6 = 1 / 2 * (RMax - EpAir) * (Lcel - Lmes)
            'I7 = (RMax - EpEau) * Lmes

            ' Pression des cellules de garde, eq.98 p.10 Formulaire Sonde nue
            Sair = 2 / 3 * Pi * (Rext0 - RMax) * (Lcel ^ 3 - Lmes ^ 3) / Lt ^ 2 + 2 * Pi * (RMax - EpAir) * (Lcel - Lmes)
            FAir = PAir * Sair

            ' Pression de la cellule de mesure eq.98 p.10 Formulaire Sonde nue
            Seau = 2 / 3 * Pi * (Rext0 - RMax) * Lmes ^ 3 / Lt ^ 2 + 2 * Pi * (RMax - EpEau) * Lmes
            FEau = PEau * Seau

            FCelSondeG = FAir + FEau
            If FCelSondeG < 0 Then FCelSondeG = 0
        End Function

        ' **************************************************************************************************
        ' Calcul de la  pression Maximum du sol élastique
        ' **************************************************************************************************
        Public Function PressionSondeGSolMax(ByVal Lt As Single, ByVal Lmes As Single, ByVal Lcel As Single,
        ByVal RMax As Single, ByVal Rext0 As Single, ByVal PAir As Single, ByVal PEau As Single, ByVal EpAir As Single, ByVal EpEau As Single,
        ByVal Pnappe As Single) As Single
            ' calcul de pression de réaction maximum élastique du sol
            Dim Psol, FCell, Fw, Pi As Single

            Pi = 3.141592654
            FCell = FCelSondeG(Lt, Lmes, Lcel, RMax, Rext0, PAir, PEau, EpAir, EpEau)
            Fw = FNappeSondeG(Lt, RMax, Rext0, Pnappe)
            ' relation 169 p.17 du formulaire Tube Fendu
            Psol = (FCell - Fw) / (Pi * (2 / 3 * (Rext0 - RMax) * Lt + 2 * RMax * Lt _
           - 2 / 5 * (Rext0 - RMax) * Lt - 2 / 3 * RMax * Lt))

            ' Correction de la pression de réaction quand la déformation est nulle
            If (Math.Abs(Rext0 - RMax) < 0.01) Then Psol = PEau * (Rext0 - EpEau) / Rext0

            If Psol < 0 Then Psol = 0
            PressionSondeGSolMax = Psol
        End Function

        ' **************************************************************************************************
        ' Calcul de la  pression Maximum du sol élastique avec le correctif de la méthode de Gauss
        ' **************************************************************************************************
        Public Function PressionSondeGSolMaxGauss(ByVal Lt As Single, ByVal Lmes As Single, ByVal Lcel As Single,
        ByVal RMax As Single, ByVal Rext0 As Single, ByVal PAir As Single, ByVal PEau As Single, ByVal EpAir As Single, ByVal EpEau As Single,
        ByVal Pnappe As Single) As Single
            ' calcul de pression de réaction maximum élastique du sol
            Dim Psol, FCell, Fw, Pi, FsolGauss As Single

            Pi = 3.141592654

            ' Calcul de PSolMax par la méthode intégrale
            FCell = FCelSondeG(Lt, Lmes, Lcel, RMax, Rext0, PAir, PEau, EpAir, EpEau)
            Fw = FNappeSondeG(Lt, RMax, Rext0, Pnappe)

            ' relation 89 p.14 du formulaire Sonde Nue
            Psol = (FCell - Fw) / (Pi * (2 / 3 * (Rext0 - RMax) * Lt + 2 * RMax * Lt _
               - 2 / 5 * (Rext0 - RMax) * Lt - 2 / 3 * RMax * Lt))
            If Psol < 0 Then Psol = 0

            ' Correction de PsolMax pour tenir compte de son écart par rapport à la méthode de Gauss
            FsolGauss = FSolSondeGElastGauss(Lt, RMax, Rext0, Psol)

            PressionSondeGSolMaxGauss = Psol
        End Function



        ' **************************************************************************************************
        ' Calcul de la  pression Moyenne sur la cellule de Mesure du sol élastique
        ' **************************************************************************************************
        Public Function PressionSondeGSolMoy(ByVal Lt As Single, ByVal Lmes As Single, ByVal PMax As Single) As Single
            Dim Psol As Single

            ' relation 96 p.15 du formulaire SondeG
            Psol = -PMax * Lmes ^ 2 / (3 * Lt ^ 2) + PMax

            PressionSondeGSolMoy = Psol
        End Function

        ' **********************************************************************************
        ' Calcul de la Force transmise à la sonde par la nappe phréatique
        ' **********************************************************************************
        Public Function FNappeSondeG(ByVal Lt As Single, ByVal RMax As Single, ByVal Rext0 As Single, ByVal Pnappe As Single) As Single
            Dim Pi, Sext, Fnappe As Single

            Pi = 3.141592654

            ' Pression des cellules de garde, eq.98 p.10 Formulaire Tube Fendu
            Sext = 2 / 3 * Pi * (Rext0 - RMax) * Lt + 2 * Pi * RMax * Lt
            Fnappe = Pnappe * Sext

            FNappeSondeG = Fnappe
            If FNappeSondeG < 0 Then FNappeSondeG = 0
        End Function

        '*********************************************
        ' Calcul de la correction de rigidité
        ' de la lanterne
        '*********************************************
        Public Function PresMembrane(ByVal Coef1 As Single, ByVal Coef2 As Single,
                             ByVal Coef3 As Single, ByVal Coef4 As Single,
                             ByVal PmembMax As Single, ByVal V60 As Single) As Inertie

            Dim X1, X2 As Single
            Dim A3, A2, A1, A0 As Single
            Dim Solution As solution3D

            A3 = Coef4
            A2 = Coef3
            A1 = Coef2

            A0 = Coef1 - V60

            X1 = 0
            X2 = PmembMax
            If (A3 = 0) And (A2 = 0) And (A1 = 0) Then

                ' S'il n'y a pas d'inertie, aucune correction n'est appliquée
                PresMembrane.PresInertie = 0
                PresMembrane.Erreur = False
            Else
                Solution = RACINE3D(A3, A2, A1, A0, X1, X2)
                If Solution.Erreur = False Then
                    PresMembrane.PresInertie = Solution.x
                    PresMembrane.Erreur = False
                Else
                    PresMembrane.PresInertie = 0
                    PresMembrane.Erreur = True
                End If
            End If
        End Function


        '*********************************************
        ' Calcul de la pression de membrane maximum
        '*********************************************
        Public Function PresMembMax(ByVal Coef1 As Single, ByVal Coef2 As Single,
                             ByVal Coef3 As Single, ByVal Coef4 As Single) As Inertie
            ' Recherche la pression Pmax correspondant à l'expansion maximum de la sonde (850cm3)

            Dim L As Integer
            Dim Vmax, PasPres, Pres, Vol, Vol0 As Single
            Dim fin As Boolean

            L = 0
            Vmax = 850
            Vol0 = 0
            PasPres = 10
            Pres = 0
            fin = False

            'boucle de teste sur le volume
            Do
                L = L + 1
                Vol = Coef1 + Coef2 * Pres + Coef3 * Pres ^ 2 + Coef4 * Pres ^ 3

                'Initialisation
                If L = 1 Then Vol0 = Vol

                'Test de croissance
                If Vol < Vol0 Then
                    fin = True
                Else
                    Vol0 = Vol
                End If

                'Test d'existence
                If Vol >= Vmax Then
                    fin = True
                End If

                Pres = Pres + PasPres
            Loop Until (L > 6000) Or fin 'fin de la boucle

            PresMembMax.PresInertie = Pres
            PresMembMax.Erreur = False
        End Function

        '**********************************************************************
        '   Fonction qui Calcule les invariants de contraintes principales S1, S2, S3
        '   Soct = 1/3*(S1+S2+S3)
        '
        '**********************************************************************
        Public Function InvarOct1(ByVal s1 As Single, ByVal S2 As Single, ByVal s3 As Single) As Single

            Dim SOCT As Single

            SOCT = (s1 + S2 + s3) / 3
            InvarOct1 = SOCT

        End Function

        '**********************************************************************
        '   Fonction qui Calcule les invariants de contraintes principales S1, S2, S3
        '   Toct = 1/3*Sqr(^(S1 - S2) + ^(S2 - S3) + ^(S3 - S1))
        '**********************************************************************
        Public Function InvarOct2(ByVal s1 As Single, ByVal S2 As Single, ByVal s3 As Single) As Single
            Dim Toct As Single
            Toct = Math.Sqrt((s1 - S2) ^ 2 + (S2 - s3) ^ 2 + (s3 - s1) ^ 2) / 3
            InvarOct2 = Toct
        End Function

        ' ***************************************************
        ' Fonction de calcul du Rayon maximum sonde G
        ' ***************************************************
        Public Function RMembMaxSondeG(ByVal Vext0 As Single, ByVal dV As Single, ByVal Lt As Single, ByVal Lmes As Single,
                             ByVal Rext0 As Single, ByVal Ep As Single) As Solution
            Dim Coef0, Coef1, Coef2, Vint0, R0p As Single
            Dim Resol As Solution

            ' Correction du rayon R0
            R0p = Math.Sqrt(Vext0 / (Pi * Lmes))

            ' Calcul du Volume Interne de la sonde
            Vint0 = Vext0 - Pi * Ep * (2 * R0p - Ep) * Lmes

            ' Calcul des coefficients de l'équation du second degré
            Coef2 = Pi * ((Lmes ^ 5) / (5 * (Lt ^ 4)) - 2 * (Lmes ^ 3) / (3 * (Lt ^ 2)) + Lmes)
            Coef1 = Pi * (-2 * R0p * (Lmes ^ 5) / (5 * (Lt ^ 4)) + 2 * (Lmes ^ 3) / (3 * (Lt ^ 2)) * (R0p + Ep) - 2 * Lmes * Ep)
            Coef0 = Pi * (R0p ^ 2 * (Lmes ^ 5) / (5 * (Lt ^ 4)) - 2 * R0p * Ep * (Lmes ^ 3) / (3 * (Lt ^ 2)) + (Ep ^ 2) * Lmes)
            Coef0 = Coef0 - Vint0 - dV

            ' Résolution de l'équation du second degré
            Resol = RACINE2(Coef2, Coef1, Coef0)

            ' Affectation des résultats en terme de rayon extérieur Maximum
            With Resol
                RMembMaxSondeG.X1 = .X1
                RMembMaxSondeG.X2 = .X2
                RMembMaxSondeG.Erreur = .Erreur
            End With
        End Function

        ' ****************************************************************************************
        ' Function de calcul du Rayon moyen sur la cellule de mesure de la  sonde Tube Fendu
        ' ****************************************************************************************
        Public Function RayonMoyenSondeG(ByVal Rext0 As Single, ByVal RMax As Single, ByVal Lt As Single, ByVal Lmes As Single,
                                     ByVal PrRext0 As Single, ByVal PrRmax As Single, ByVal PrLmes As Single, ByVal PrLt As Single) As ValeurPression
            Dim Rayon, dR_Rext0, dR_Rmax, dR_Lmes, dR_Lt As Single

            Rayon = (Rext0 - RMax) * Lmes ^ 2 / (3 * Lt ^ 2) + RMax
            '    Rayon = 4 * (Rmax - Rext0) * (X / Lt) ^ 2 + Rext0
            dR_Rmax = 1 + Lmes ^ 2 / (3 * Lt ^ 2)
            dR_Rext0 = Lmes ^ 2 / (3 * Lt ^ 2)
            dR_Lmes = 2 * (Rext0 - RMax) * Lmes / (3 * Lt ^ 2)
            dR_Lt = 2 * (Rext0 - RMax) * Lmes ^ 2 / (3 * Lt ^ 3)
            RayonMoyenSondeG.Valeur = Rayon
            RayonMoyenSondeG.Precis = Math.Sqrt(dR_Rmax ^ 2 * PrRmax ^ 2 + dR_Rext0 ^ 2 * PrRext0 ^ 2 + dR_Lmes ^ 2 * PrLmes ^ 2 + dR_Lt ^ 2 * PrLt ^ 2)
        End Function

        ' *******************************************************
        ' Fonction de calcul du Volume de mesure de la  sonde G
        ' *******************************************************
        Public Function VolumeG(ByVal Vext0 As Single, ByVal Rmax As Single, ByVal Lt As Single, ByVal Lmes As Single,
                             ByVal Rext0 As Single, ByVal Ep As Single) As Single
            Dim Coef0, Coef1, Coef2, Vint0 As Single

            ' Calcul du Volume Interne de la sonde
            Vint0 = Vext0 - Pi * Ep * (2 * Rext0 - Ep) * Lmes

            Coef2 = Pi * ((Lmes ^ 5) / (5 * (Lt ^ 4)) - 2 * (Lmes ^ 3) / (3 * (Lt ^ 2)) + Lmes)
            Coef1 = Pi * (-2 * Rext0 * (Lmes ^ 5) / (5 * (Lt ^ 4)) + 2 * (Lmes ^ 3) / (3 * (Lt ^ 2)) * (Rext0 + Ep) - 2 * Lmes * Ep)
            Coef0 = Pi * (Rext0 ^ 2 * (Lmes ^ 5) / (5 * (Lt ^ 4)) - 2 * Rext0 * Ep * (Lmes ^ 3) / (3 * (Lt ^ 2)) + (Ep ^ 2) * Lmes)
            Coef0 = Coef0 - Vint0
            VolumeG = Coef2 * Rmax ^ 2 + Coef1 * Rmax + Coef0
        End Function



    End Module
End Namespace