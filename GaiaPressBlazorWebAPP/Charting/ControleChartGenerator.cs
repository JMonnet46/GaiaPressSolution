using System;
using System.Collections.Generic;
using System.Linq;
using GaiaPressCoreV17.Models;

namespace GaiaPressBlazorWebAPP.Charting
{
    /// <summary>
    /// GÃ©nÃ¨re des donnÃ©es de graphique pour les essais de forage.
    /// </summary>
    public class ControleChartGenerator
    {
        // PropriÃ©tÃ© pour stocker l'essai courant
        public Essai_Forage CurrentEssai { get; set; }

        /// <summary>
        /// GÃ©nÃ¨re des donnÃ©es pour un graphique Pression/Volume
        /// </summary>
        public Dictionary<string, List<double>> GeneratePressureVolumeChart()
        {
            if (CurrentEssai == null)
                throw new InvalidOperationException("Aucun essai de forage n'est dÃ©fini.");

            var data = new Dictionary<string, List<double>>();

            data.Add("Pression (bar)", new List<double> {
                CurrentEssai.presP0,
                CurrentEssai.presFluage,
                CurrentEssai.presLimite
            });

            data.Add("Volume (mÂ³)", new List<double> {
                CurrentEssai.VolSonde,
                CurrentEssai.VolSonde * 1.2,
                CurrentEssai.VolSonde * 1.5
            });

            return data;
        }

        /// <summary>
        /// GÃ©nÃ¨re des donnÃ©es pour une courbe de fluage
        /// </summary>
        public (double[] xValues, double[] yValues) GenerateCreepCurve()
        {
            if (CurrentEssai == null)
                throw new InvalidOperationException("Aucun essai de forage n'est dÃ©fini.");

            var xValues = new double[] { 0, 1, 2, 3, 4, 5 };
            var yValues = new double[] {
                CurrentEssai.presP0,
                CurrentEssai.presP0 * 0.95,
                CurrentEssai.presP0 * 0.90,
                CurrentEssai.presP0 * 0.85,
                CurrentEssai.presP0 * 0.80,
                CurrentEssai.presP0 * 0.75
            };

            return (xValues, yValues);
        }

        /// <summary>
        /// GÃ©nÃ¨re des donnÃ©es pour un graphique de limite hyperbolique (1/VolumeNF)
        /// </summary>
        public (double[] xValues, double[] yValues) GenerateHyperbolicLimitChart()
        {
            if (CurrentEssai == null)
                throw new InvalidOperationException("Aucun essai de forage n'est dÃ©fini.");

            var xValues = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            var yValues = new double[xValues.Length];

            for (int i = 0; i < xValues.Length; i++)
            {
                yValues[i] = 1.0 / xValues[i]; // 1/VolumeNF
            }

            return (xValues, yValues);
        }
    }
}

//Public Sub DessineControle(ByRef graphe As Winforms_Chart)
//        Dim row As Integer
//        Dim Pres As Single
//        Dim Theorie As Courbe
//        Dim Sz, Dext, Nu, Eelast, Phimu, Coh, Phi, UaMax, Pmax, U0 As Single
//        Dim A, K0, P0, Lambda, Mu, Phie, N, Np As Single
//        Dim MinX, MinY, MaxX, MaxY As Single

//        Dim Theo As Boolean

//        'ParamÃ©trage de la courbe
//        If MySonde Is Nothing Then
//            If Lang = 1 Then
//                MsgBox("Vous devez sÃ©lectionner une sonde", vbOKOnly)
//            Else
//                MsgBox("You must select a probe", vbOKOnly)
//            End If
//            Exit Sub
//        End If
//        If MySol Is Nothing Then
//            If Lang = 1 Then
//                MsgBox("Vous devez sÃ©lectionner un sol", vbOKOnly)
//            Else
//                MsgBox("You must select a soil", vbOKOnly)
//            End If
//            Exit Sub
//        End If

//        With MyEssai
//            Dext = MySonde.Getds
//            Nu = .E_coefPoisson
//            Eelast = .E_modElastique
//            Phimu = .E_FrottementIntergranulaire
//            Coh = .E_Cohesion
//            Phi = .E_Frottement
//            U0 = .E_U0
//        End With


//        'Dessin en Contrainte/DÃ©formation

//        If (E_MesureCount > 0) And (Eelast > 0) And (Nu > 0) And (Phi + Coh > 0) Then
//            Dim X(E_MesureCount - 1) As Single
//            Dim Y(E_MesureCount - 1) As Single
//            Dim X1(E_MesureCount - 1) As Single
//            Dim Y1(E_MesureCount - 1) As Single

//            ' VÃ©rification des paramÃ¨tre gÃ©omÃ©caniques
//            UaMax = 0
//            Pmax = 0
//            Theo = False

//            'PrÃ©paration du calcul thÃ©orique
//            A = Dext / 2000
//            K0 = Nu / (1 - Nu)

//            Lambda = Eelast * Nu / (1 + Nu) / (1 - 2 * Nu)
//            Mu = Eelast / 2 / (1 + Nu)
//            Phie = Dilatance(Phi, Phimu, 1)
//            Sz = E_Mesures(8, 0)
//            P0 = K0 * Sz
//            N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
//            Np = (1 - Math.Sin(DTOR(Phie))) / (1 + Math.Sin(DTOR(Phie)))

//            With graphe
//                .Series.Clear()
//                Select Case Lang
//                    Case 0
//                        .Series.Add("Experemental data " & MyEssai.E_nomEssai)
//                        .Series.Add("Theory " & MyEssai.E_nomEssai)
//                    Case 1
//                        .Series.Add("Experience " & MyEssai.E_nomEssai)
//                        .Series.Add("ThÃ©orie " & MyEssai.E_nomEssai)
//                End Select
//            End With

//            'E_Mesures(0, E_MesureCount - 1) = Math.Round((.Fields("pression").Value), 1)
//            'E_Mesures(1, E_MesureCount - 1) = Math.Round((.Fields("volume15").Value), 1)
//            'E_Mesures(2, E_MesureCount - 1) = Math.Round((.Fields("volume30").Value), 1)
//            'E_Mesures(3, E_MesureCount - 1) = Math.Round((.Fields("volume60").Value), 1)
//            'E_Mesures(7, E_MesureCount - 1) = Math.Round((.Fields("contrainteRadiale").Value), 1)
//            'E_Mesures(9, E_MesureCount - 1) = Math.Round((.Fields("deformation").Value), 7)
//            'E_Mesures(6, E_MesureCount - 1) = Math.Round((.Fields("PNF").Value), 1)
//            'E_Mesures(7, E_MesureCount - 1) = Math.Round((.Fields("VNF").Value), 1)
//            'E_Mesures(8, E_MesureCount - 1) = Math.Round((.Fields("Qi").Value), 2)
//            'E_Mesures(9, E_MesureCount - 1) = Math.Round((.Fields("Pi").Value), 1)
//            'E_Mesures(10, E_MesureCount - 1) = Math.Round((.Fields("Qp").Value), 2)
//            'E_Mesures(11, E_MesureCount - 1) = Math.Round((.Fields("Pp").Value), 1)
//            'E_Mesures(12, E_MesureCount - 1) = Math.Round((.Fields("contrainteVerticale").Value), 1)


//            With graphe

//                ' Dessin de la courbe expÃ©rimentale
//                MinX = 0
//                MinY = 0
//                MaxX = 0
//                MaxY = 0
//                For row = 0 To E_MesureCount - 1
//                    X(row) = E_Mesures(9, row)
//                    If X(row) < MinX Then MinX = X(row)
//                    If X(row) > MaxX Then MaxX = X(row)
//                    Y(row) = E_Mesures(7, row)
//                    If Y(row) < MinY Then MinY = Y(row)
//                    If Y(row) > MaxY Then MaxY = Y(row)
//                Next row

//                ' Dessin de la courbe thÃ©orique
//                For row = 0 To E_MesureCount - 1
//                    X1(row) = E_Mesures(9, row)
//                    Pres = E_Mesures(7, row)
//                    Sz = E_Mesures(8, row)
//                    ' Calcul du comportement thÃ©orique du sol selon la thÃ©orie
//                    Theorie = ComptPres(Dext, A, Nu, Eelast, Sz, Phimu, Coh, Phi, U0, UaMax,
//                       Pres, Pmax, K0, P0, Lambda, Mu, Phie, N, Np, True)

//                    'MÃ©morisation des pressions et dÃ©formations maximum
//                    Pmax = Theorie.Pmax
//                    UaMax = Theorie.UaMax
//                    If Theorie.Plim > 0 Then
//                        presLimiteTheo = Math.Round(Theorie.Plim, 1)
//                    End If
//                    If Theorie.Pf > 0 Then
//                        presFluage = Math.Round(Theorie.Pf, 1)
//                    End If
//                    Y1(row) = Theorie.Pres
//                    X1(row) = Theorie.Def
//                Next row

//                ' DÃ©finition du type de ligne
//                With .Series(0)

//                    .ChartType = SeriesChartType.Line
//                    '.Series(0).BorderDashStyle = BorderStyle.FixedSingle
//                    .BorderWidth = 1
//                    .Color = System.Drawing.Color.Red
//                End With

//                .Series(1).ChartType = SeriesChartType.Line
//                .Series(1).Color = System.Drawing.Color.Blue
//                '.Series(1).BorderDashStyle = BorderStyle.FixedSingle
//                .Series(1).BorderWidth = 1

//                ' Ajouter les deux sÃ©ries de points
//                .Series(0).Points.DataBindXY(X, Y)
//                .Series(1).Points.DataBindXY(X1, Y1)

//                ' DÃ©finition axe des Y
//                With .ChartAreas(0).AxisY
//                    Select Case Lang
//                        Case 0
//                            .Title = "Pressure (kPa)"
//                        Case 1
//                            .Title = "Pression (kPa)"
//                    End Select

//                    .Minimum = 0
//                    'If hy > 1 Then
//                    '    .Minimum = 1
//                    'Else
//                    '    .Minimum = 0.1
//                    'End If
//                    '.Maximum = hlimx
//                    .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
//                    .LineWidth = 1
//                    .MajorGrid.LineColor = System.Drawing.Color.Gray

//                    ' recherche de la premiÃ¨re arrondi supÃ©rieure Ã  MaxX
//                    .Minimum = 0
//                    .RoundAxisValues()
//                    .Maximum = Math.Truncate((MaxY * 100 + 1000) / 100)
//                    .MajorGrid.Interval = Math.Round(.Maximum / 10)

//                    .RoundAxisValues()
//                End With

//                ' DÃ©finition axe des X
//                With .ChartAreas(0).AxisX
//                    Select Case Lang
//                        Case 0
//                            .Title = "Deformation"
//                        Case 1
//                            .Title = "Deformation"
//                    End Select
//                    .Minimum = 0
//                    '.Maximum = hlimy
//                    .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
//                    .LineWidth = 1
//                    .MajorGrid.LineColor = System.Drawing.Color.Gray
//                    .MajorGrid.Interval = Math.Round(MaxX / 20)
//                    .RoundAxisValues()
//                    .Minimum = 0
//                    .RoundAxisValues()

//                    ' recherche de la premiÃ¨re arrondi supÃ©rieure Ã  MaxX
//                    .Maximum = Math.Truncate(MaxX * 10 + 1) / 10
//                    .MajorGrid.Interval = Math.Round(.Maximum / 10)
//                End With

//            End With
//            graphe.Visible = True

//        Else
//            graphe.Visible = False
//        End If

//    End Sub


