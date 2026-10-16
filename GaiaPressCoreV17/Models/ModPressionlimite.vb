'Imports Microsoft.Office.Interop
Imports System.Math
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic


Module ModPressionlimite
    'ReadOnly XlApp As AppExcel.Application
    'ReadOnly XlApp As Global.Microsoft.Office.Interop.Excel.Application
    'Public Type Limite
    '    Plimite As Single
    '    PlimiteNF As Single
    '    Erreur As Boolean
    'End Type

    '***********************************************************************
    '    Calcul de la pression limite sur les valeurs totalement corrigées
    '***********************************************************************

    Public Function ModulePresLim(ByVal Pini As Single, ByVal Pfexp As Single, Essai As Essai_Forage) As Single

        Dim i, J, k, Ndp, Ie As Integer
        Dim Pres, Def, Pmax, PlimExp, A0L, A1L, U0 As Single
        Dim xMoy, yMoy, Num, Sig2Y, Sig2X, SigrX2, SigX, xCalc, ResStudent, Defe, DefPl As Single
        Dim Pe As Single
        Dim Regres As Regression
        Dim Resultat As Statistique
        Dim x() As Single
        Dim Y() As Single
        Dim Erreur, PeTrouve As Boolean

        ' Création des tableaux de variables
        J = 10
        ReDim Preserve x(0 To J)
        ReDim Preserve Y(0 To J)

        k = -1
        Pmax = 0
        Ndp = Essai.GetMesureCount
        PeTrouve = False

        If Ndp > 0 Then
            For i = 0 To Ndp - 1
                Pres = Essai.Mesures(i).ContrainteRadiale
                Def = Essai.Mesures(i).Deformation
                If Essai.Mesures(i).ContrainteRadiale >= Pini And PeTrouve = False Then
                    Pe = Essai.Mesures(i).ContrainteRadiale
                    Defe = Essai.Mesures(i).Deformation
                    Ie = i
                    Essai.Ie = Ie
                    Essai.Pe = Pe
                    PeTrouve = True
                End If

                If Pmax <= Pres Then
                    Pmax = Pres
                    If (Pres >= Pfexp) Then
                        If Def > 0 Then
                            k = k + 1
                            If k > J Then
                                J = J + 10
                                ReDim Preserve x(0 To J)
                                ReDim Preserve Y(0 To J)
                            End If
                            x(k) = Pres
                            Y(k) = 1 / Def
                        End If
                    End If
                End If
                If Pres < Pini Then
                    U0 = Def
                End If
            Next i

            ' Calcul de la pente moyenne
            Regres = RegresLine(k, x, Y)
            A0L = Regres.b
            A1L = Regres.A

            ' Valeur du volume limite Vl
            ' On cherche : DR1/R1 = sqrt(2)-1
            ' avec R1 = R0.( 1 + Eps1); R0 rayon initial de la sonde; Eps1 : déformation pour contact au forage
            '      dR1/[R0.( 1 + Eps1)]= sqrt(2)-1
            '      dr1= R0.( 1+ Eps1).(Sqrt(2)-1)
            '      dr1= dr0 - R0.Eps1
            '      dr0 - R0.Eps1= R0.( 1+ Eps1).(Sqrt(2)-1)
            '      dr0/R0 - Eps1 = ( 1+ Eps1).(Sqrt(2)-1)
            '      dr0/R0 = ( 1+ Eps1).(Sqrt(2)-1) + Eps1

            DefPl = (Math.Sqrt(2) - 1) + 2 * Defe
            DefPl = (1 + Defe) * (Sqrt(2) - 1) + Defe

            ' Calcul de la pression limite
            If Not Regres.Erreur Then
                PlimExp = -A0L / A1L + 1 / (A1L * DefPl)
                If PlimExp < 0 Then PlimExp = 0
                ModulePresLim = PlimExp
            Else
                GoTo errFind
            End If


            ''Calcul de Plim en inverse
            'If Not (Erreur) Then
            '    Plimi = -A0L / A1L + 1 / (A1L * (Vs + 2 * Ve))
            'End If

            ' Calcul de la régression linéaire en Y, X
            '        RegresLine(k, Abscisse, Ordonnée)
            Regres = RegresLine(k, Y, x)

            A0L = Regres.b
            A1L = Regres.A
            SigX = Regres.SigR
            PrRegresPl = Regres.SigR
            Erreur = Regres.Erreur

            ' Calcul de la moyenne de V-1
            Resultat = MoyVariance(0, k, x)
            xMoy = Resultat.Moyenne

            Resultat = MoyVariance(0, k, Y)
            yMoy = Resultat.Moyenne

            ' Calcul du dénominateur de l'expression 51 (livrable Gaiatech V6)
            ' Rapport : "Calcul de la précision des résultats pressiométriques : modules EM, pression limite plm "
            ' "et module cyclique Ee en fonction de la précision des mesures de pression et de volume"
            Sig2Y = 0
            Sig2X = 0
            SigrX2 = 0
            Num = 0
            For i = 0 To k
                Sig2Y = Sig2Y + (Y(i) - yMoy) ^ 2
                Sig2X = Sig2X + (x(i) - xMoy) ^ 2
                xCalc = A0L + A1L * Y(i)
                SigrX2 = SigrX2 + (x(i) - xCalc) ^ 2
                Num = Num + 2 * (Y(i) - yMoy) * Y(i) ^ 2
            Next i
            'SigX = 0
            'If k > 1 Then SigX = (SigrX2 / (k - 1)) ^ 0.5

            ' Calcul de la precision des variables unitaires et des dérivées partielles
            ' Precision = PrecisionVariablesUnitaires(Ie, k)

            PrVariance = 0.341 * 2
            'ResStudent = ProbStudent_Inverse(PrVariance, k - 1)    ' Loi de Student bilatérale
            'ResStudent = xlApp.loi.Student(0.1, k - 1)
            'ResStudent = xlApp.WorksheetFunction.
            ResStudent = ProbStudent_InverseTab(k - 1)
            'DVL = ResStudent / (1 + 1 / (k + 1) + (VL ^ -1 - yMoy) ^ 2 / Sig2Y) ^ 0.5
            'DVL = DVL * 2 * (VL ^ -1 - YMoy) / Sig2Y * VL ^ -2
            'DVi = ResStudent / (1 + 1 / (k + 1) + (VL ^ -1 - YMoy) ^ 2 / Sig2Y) ^ 0.5
            'DVi = DVi * ((VL ^ -1 - YMoy) ^ 2 / Sig2Y ^ 2) * Num

            PrPl = ResStudent * SigX * (1 + 1 / (k + 1) + (DefPl ^ -1 - yMoy) ^ 2 / Sig2Y) ^ 0.5
            'PrPlMenard = PrPlMenard + (DVL ^ 2 * PrVl ^ 2 + DVi ^ 2 * PrV ^ 2) ^ 0.5

        Else
            GoTo errFind
        End If
        Exit Function
errFind:
        ModulePresLim = 0
        Err.Raise(vbObjectError + 611, "Pression limite", "Calcul impossible: Nombre de points insuffisants pour une régréssion linéaire!")
        MsgBox(Err.Description)

    End Function

    ' **************************************************************************************************************************
    ' Calcul des dérivées partielles nécessaires au calcul de la précision de la pression limite
    ' iDebut est l'indice correspondant à V1
    ' iFin est l'indice correspondant à V2
    ' k est le nombre de points de mesure dans la partie linéaire de la relation V-1 = A.p + B
    ' *************************************************************************************************************************
    Public Function PrecisionVariablesUnitaires(ByVal iDebut As Integer, iFin As Integer) As PrecisionElast
        Dim Di, Ls, A0, A1, A2, A3, A, Pr1, Pr2, PV, Vr1, Vr2, Pe As Single
        Dim Pmax, Pmin, Vmax, Vmin, VL, PrVl As Single
        '  Mesures(  Colonne, Ligne, Valeur)
        Pmax = MyEssaiForage.Mesures(iFin).PNF
        Pmin = MyEssaiForage.Mesures(iDebut).PNF
        Vmax = MyEssaiForage.Mesures(iFin).VNF
        Vmin = MyEssaiForage.Mesures(iDebut).VNF
        Pr1 = MyEssaiForage.Mesures(iDebut).Pression
        Vr1 = MyEssaiForage.Mesures(iDebut).Volume60
        Pr2 = MyEssaiForage.Mesures(iFin).Pression
        Vr2 = MyEssaiForage.Mesures(iFin).Volume60
        Di = MyEssaiForage.MaExpansion.Di / 10
        Ls = MySonde.ls / 10
        Vs = MySonde.Vs
        A0 = MyEssaiForage.MaResistance.A0
        A1 = MyEssaiForage.MaResistance.A1
        A2 = MyEssaiForage.MaResistance.A2
        A3 = MyEssaiForage.MaResistance.A3
        A = MyEssaiForage.MaExpansion.coefficientA
        MyEssaiForage.MaResistance.CalculePel(Vr1)
        Pe = MyEssaiForage.MaResistance.Pe
        If MyEssaiForage.profSonde < MyEssaiForage.profNappe Then
            PV = MyEssaiForage.poidsVol
        Else
            PV = MyEssaiForage.poidsVolDejauge
        End If

        ' Calcul de la Précision sur les variables uniataires 
        'PrVs = (0.0625 * Pi ^ 2 * Di ^ 4 * PrLong ^ 2 + 0.25 * (Pi * Ls * Di * PrDiam) ^ 2 + PrVol ^ 2) ^ 0.5
        PrP = 0
        If A1 * A2 * A3 <> 0 Then
            PrP = (PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5
        End If
        PrV = (PrV ^ 2 + (A * PrP) ^ 2 + Pr1 ^ 2 * PrA ^ 2) ^ 0.5
        With PrecisionVariablesUnitaires
            .PMax = Pmax
            .PMin = Pmin
            .VMax = Vmax
            .VMin = Vmin
            VL = (Vs + 2 * Vmin)
            PrVl = PrVs + 2 * PrV

            ' Précision pour la détermination de la Pression limite
            '.DEPl_Vs = 2.66 * (Pmax - Pmin) / (Vmax - Vmin)
            '.DPl_Vl = 1.33 * (Pmax - Pmin) / (Vmax - Vmin) * (1 + 2 / (Vmax - Vmin) * (Vs + (Vmax + Vmin) / 2))
            '.DPPl_V = 2.66 * (Vs + (Vmax + Vmin) / 2) / (Vmax - Vmin)

            ' Précsion pour la détermination de pLM
            ' Valeur du volume limite Vl


            'PrVariance = 1 - 0.341

        End With
    End Function




    '***********************************************************************
    '    Calcul de la pression limite sur les valeurs normalisées
    '***********************************************************************

    Public Function ModulePresLimNF(ByVal Pinit As Single, ByVal Pf As Single,
                                    Essai As Essai_Forage) As Single

        Dim i, J, k, IP1, Ndp, Ie As Integer
        Dim Pmax, Plimi, Plimh, PlimExp, A0L, A1L, Vs, Ve, Pres, Vol, V1, VL, Pe, ResStudent As Single
        Dim Sig2Y, Sig2X, SigrX2, SigX, Num, XMoy, YMoy, XCalc, PrVl, DVL, DVi As Single
        Dim Regres As Regression
        Dim Resultat As Statistique
        Dim Erreur, Trouve, ErrTrouvee As Boolean
        Dim x() As Single
        Dim Y() As Single
        Dim Precision As PrecisionElast

        ' Initialisation
        ModulePresLimNF = 0

        ' Création des tableaux de variables
        J = 10
        ReDim Preserve x(0 To J)
        ReDim Preserve Y(0 To J)

        k = -1
        Pmax = 0
        Ndp = Essai.GetMesureCount
        'Vs = Pi * ((Essai.GetMySonde.Getds / 10) ^ 2 / 4) * (Essai.GetMySonde.Getls / 10)
        Vs = Essai.MySonde.GetVs
        Ie = Essai.Ie
        Pe = Essai.Pe
        Ve = Essai.Mesures(Ie).VNF

        If Ndp > 0 Then

            ' Calcul de Ve
            Ve = 0
            For i = 1 To Ndp - 1
                If Essai.Mesures(i).PNF >= Pinit Then
                    Pe = Essai.Mesures(i).PNF
                    Ve = Essai.Mesures(i).VNF
                    Essai.Ie = i
                    Essai.Pe = Pe
                    Ie = i
                    Exit For
                End If
            Next i
            Essai.Ie = i

            Pmax = 0
            k = -1
            'Vérification du nombre de point en fluage
            For i = 0 To Ndp - 1
                Pres = Essai.Mesures(i).PNF
                If (Pf <= Pres) And (Pmax <= Pres) Then
                    k = k + 1
                    Pmax = Pres
                End If
            Next i

            'Le nombre de points en fluage est supérieur ou égal à 3
            If k >= 2 Then

                k = -1
                Pmax = 0

                ' Calcul de PlimNF par la méthode de la courbe inverse
                For i = 0 To Ndp - 1
                    Pres = Essai.Mesures(i).PNF
                    Vol = Essai.Mesures(i).VNF
                    If Pmax <= Pres Then
                        Pmax = Pres

                        If (Pres >= Pf) Then
                            If Vol > 0 Then
                                k = k + 1
                                If k > J Then
                                    J = J + 10
                                    ReDim Preserve x(0 To J)
                                    ReDim Preserve Y(0 To J)
                                End If
                                x(k) = Pres
                                Y(k) = 1 / Vol
                            Else
                                ErrTrouvee = True
                            End If
                        End If
                    End If
                Next i

                ' Calcul de la régression linéaire en X, Y
                Regres = RegresLine(k, x, Y)
                A0L = Regres.B
                A1L = Regres.A
                Erreur = Regres.Erreur

                'Calcul de Plim en inverse
                If Not (Erreur) Then
                    Plimi = -A0L / A1L + 1 / (A1L * (Vs + 2 * Ve))
                End If

                ' Calcul de la régression linéaire en Y, X
                Regres = RegresLine(k, Y, x)
                A0L = Regres.B
                A1L = Regres.A
                Erreur = Regres.Erreur

                ' Calcul de la moyenne de V-1
                Resultat = MoyVariance(0, k, x)
                XMoy = Resultat.Moyenne

                Resultat = MoyVariance(0, k, Y)
                YMoy = Resultat.Moyenne

                ' Calcul du dénominateur de l'expression 51 (livrable Gaiatech V6)
                ' Rapport : "Calcul de la précision des résultats pressiométriques : modules EM, pression limite plm "
                ' "et module cyclique Ee en fonction de la précision des mesures de pression et de volume"
                Sig2Y = 0
                Sig2X = 0
                SigrX2 = 0
                Num = 0
                For i = 0 To k
                    Sig2Y = Sig2Y + (Y(i) - YMoy) ^ 2
                    Sig2X = Sig2X + (x(i) - XMoy) ^ 2
                    XCalc = A0L + A1L * Y(i)
                    SigrX2 = SigrX2 + (x(i) - XCalc) ^ 2
                    Num = Num + 2 * (Y(i) - YMoy) * Y(i) ^ 2
                Next i
                SigX = 0
                If k > 1 Then SigX = (SigrX2 / (k - 1)) ^ 0.5
                PrRegresPl = SigX

                ' Calcul de la precision des variables unitaires et des dérivées partielles
                Precision = PrecisionVariablesUnitaires(Ie, k)

                ' Valeur du volume limite Vl
                VL = (Vs + 2 * Ve)
                PrVl = PrVs + 2 * PrV
                'PrVariance = 1 - 0.341

                ''Student = XlApp.WorksheetFunction.loi.student.bilaterale((PrVariance) / 2, k - 2)


                '' Tente de récupérer une instance d'Excel déjà créée
                'xlApp = GetObject(, "Excel.Application")
                '' Sinon, crée une nouvelle instance d'Excel
                'If xlApp Is Nothing Then
                '    xlApp = CreateObject("Excel.Application")
                '    xlAppCreated = True
                'End If

                PrVariance = 0.341 * 2
                'ResStudent = ProbStudent_Inverse(PrVariance, k - 1)    ' Loi de Student bilatérale
                'ResStudent = xlApp.loi.Student(0.1, k - 1)
                'ResStudent = xlApp.WorksheetFunction.
                ResStudent = ProbStudent_InverseTab(k - 1)
                DVL = ResStudent / (1 + 1 / (k + 1) + (VL ^ -1 - YMoy) ^ 2 / Sig2Y) ^ 0.5
                DVL = DVL * 2 * (VL ^ -1 - YMoy) / Sig2Y * VL ^ -2
                DVi = ResStudent / (1 + 1 / (k + 1) + (VL ^ -1 - YMoy) ^ 2 / Sig2Y) ^ 0.5
                DVi = DVi * ((VL ^ -1 - YMoy) ^ 2 / Sig2Y ^ 2) * Num

                PrPlMenard = ResStudent * SigX * (1 + 1 / (k + 1) + (VL ^ -1 - YMoy) ^ 2 / Sig2Y) ^ 0.5 + PrP
                PrPlMenard = PrPlMenard + (DVL ^ 2 * PrVl ^ 2 + DVi ^ 2 * PrV ^ 2) ^ 0.5

                '' Calcul de la precision des variables unitaires et des dérivées partielles
                'Precision = PrecisionVariablesUnitaires(iDebut, iFin)

                '' Calcul de la précision sur le module
                'PrEMenard = (Precision.DEm_Vs ^ 2 * PrVs ^ 2 + 2 * Precision.DEm_V ^ 2 * PrV ^ 2 + 2 * Precision.DEm_P ^ 2 * PrP ^ 2) ^ 0.5

                'ModuleMenard = Math.Round(EMenardNF, 1)
                'PrEMenard = Math.Round(PrEMenard, 1)


                ' Calcul de PlimNF par la méthode de la courbe hyperbolique
                k = -1
                Trouve = False
                Pmax = 0
                If Pe < Pinit Then
                    Pinit = Pe
                    IP1 = Ie
                End If

                For i = 0 To Ndp - 1
                    Pres = Essai.Mesures(i).PNF
                    Vol = Essai.Mesures(i).VNF
                    If (Pres >= Pinit) And Not (Trouve) Then
                        IP1 = i
                        Trouve = True
                    End If
                    If Pmax <= Pres Then
                        Pmax = Pres
                        If Pres > Pf Then
                            If Vol > 0 Then
                                k = k + 1
                                If k > J Then
                                    J = J + 10
                                    ReDim Preserve x(0 To J)
                                    ReDim Preserve Y(0 To J)
                                End If
                                x(k) = (Vol ^ 2 - Ve ^ 2) / (Pres - Pe)
                                Y(k) = (Pres * Vol ^ 2 - Pe * Ve ^ 2) / (Pres - Pe)
                            Else
                                ErrTrouvee = True
                            End If
                        End If
                    End If
                Next i

                Regres = RegresLine(k, x, Y)
                A0L = Regres.B
                A1L = Regres.A
                Erreur = Regres.Erreur

                Ve = Essai.Mesures(Ie).VNF
                V1 = Essai.Mesures(IP1).VNF
                VL = Vs + 2 * V1

                If Not (Erreur) Then
                    ' Calcul de Plim en hyperbolique
                    Plimh = (Pe * (Ve ^ 2 - A0L) + A1L * (VL ^ 2 + Ve ^ 2)) / (VL ^ 2 - A0L)
                    If Math.Abs((Plimi - Plimh) / Plimh) >= 0.2 Then ErrTrouvee = True

                    PlimExp = Plimi
                    If (Plimh < PlimExp) And (PlimExp > 0) And (Plimh > Pf) Then
                        PlimExp = Plimh
                    End If
                    ModulePresLimNF = PlimExp

                Else
                    If Plimi = 0 Then
                        ErrTrouvee = True
                    Else
                        ModulePresLimNF = Plimi
                    End If
                End If

            Else
                ErrTrouvee = True
            End If
        Else
            ErrTrouvee = True
        End If

        ' If ErrTrouvee Then Err.Raise vbObjectError + 612, "Pression limite NF", "Calcul imprécis"

    End Function


End Module