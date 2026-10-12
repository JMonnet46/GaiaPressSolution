Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading

Namespace GaiaPressCoreV17.Services

    Module ModPressionNF
        'Dim ObjExcel As excel.
        Public Sub PressionNF(essai As Essai_Forage)

            '***************************************************
            ' Calcul de la pression normalisée
            ' et de la variation du volume normalisée NFP 94-110
            '***************************************************

            'Ph pression due à la charge hydraulique
            'GAMMAw est le poids volumique de l'eau=9,81
            'Zc cote du CPV
            Dim Erreur As Boolean
            Dim i, Ndp As Integer
            Dim result As Inertie
            Dim Pres, V60, PmembMax, PresNF, VolNF As Single
            Dim Zc, Zs, Ph, NormePh As Single
            Dim Coef1, Coef2, Coef3, Coef4, A, dPInert As Single
            Dim MesureCorrigee As MesureEssaiForage

            MesureCorrigee = New MesureEssaiForage
            Erreur = False
            Const GammaW = 9.81
            With essai
                Zc = . hauteurCpv
                Zs = . profSonde
                dPInert = . difPresEauAir
            End With

            Ph = GammaW * (Zc + Zs)
            NormePh = Ph

            'Prendre les coeffcients d'inertie
            With essai.GetEssaiResistanceParRef(essai.refEssai, EssaiCalib.Resistance)
                Coef1 = .GetA0
                Coef2 = .GetA1
                Coef3 = .GetA2
                Coef4 = .GetA3
            End With

            If Coef1 * Coef2 * Coef3 * Coef4 = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("Calibration Coefficient are null")
                    Case 1
                        MsgBox("Les coefficients de calibration sont nuls")
                End Select
                Exit Sub
            End If

            A = essai.GetEssaiExpansionParRef(essai.refEssai, EssaiCalib.Expansion).GetCoefficientA

            If A = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("Dilatation Coefficient is null")
                    Case 1
                        MsgBox("Le Coefficient de Dilatation est nul")
                End Select
                Exit Sub
            End If

            Ndp = essai. Mesures.Count

            result = PresMembMax(Coef1, Coef2, Coef3, Coef4)
            PmembMax = result.PresInertie


            'Calcul de la pression NF corrigée
            If essai. Mesures Is Nothing Then Exit Sub
            For i = 0 To Ndp - 1

                ' GetMesures( Ligne, Colonne, Valeur)
                Pres = essai. Mesures(i).Pression
                V60 = essai. Mesures(i).Volume60
                MesureCorrigee.RefEssai = essai. Mesures(i).RefEssai
                MesureCorrigee.IndiceMesure = i
                MesureCorrigee.Pression = essai. Mesures(i).Pression
                MesureCorrigee.Volume15 = essai. Mesures(i).Volume15
                MesureCorrigee.Volume30 = essai. Mesures(i).Volume30
                MesureCorrigee.Volume60 = essai. Mesures(i).Volume60

                'Calcul de la résistance propre
                result = PresMembrane(Coef1, Coef2, Coef3, Coef4, PmembMax, V60)
                If Not (result.Erreur) Then
                    PresNF = Pres + Ph - result.PresInertie
                Else
                    Erreur = True
                End If


                'calcul de la correction due à l'expansion propre
                VolNF = V60 - A * Pres

                'Mémorisation des pressions et volumes corrigés NF
                If i = 0 Then
                    PresNF = 0
                    VolNF = 0
                End If

                If Not (Erreur) Then
                    If PresNF < 0 Then PresNF = 0
                    If VolNF < 0 Then VolNF = 0
                    MesureCorrigee.PNF = PresNF
                    MesureCorrigee.VNF = VolNF
                    ' SetMesures( Ligne, Colonne, Valeur)
                    'Call essai.SetMesures(i, 10, Math.Round(PresNF, 1))
                    'Call essai.SetMesures(i, 11, Math.Round(VolNF, 1))
                    'Call MyEssaiForage.SetMesures(i, 10, Math.Round(PresNF, 1))
                    'Call MyEssaiForage.SetMesures(i, 11, Math.Round(VolNF, 1))
                    essai.UpdateMesureEssaiForage(i, MesureCorrigee)
                End If
            Next
        End Sub


    End Module
End Namespace