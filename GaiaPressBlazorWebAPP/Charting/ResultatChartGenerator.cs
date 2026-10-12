using GaiaPressCoreV17.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GaiaPressBlazorWebApp.Charting
{
    /// <summary>
    /// Génère des données de graphique pour les essais de forage.
    /// Remplace les anciennes fonctions utilisant DataVisualization.
    /// </summary>
    public class ResultatChartGenerator
    {        // Exemple : Générer des données pour un graphique Pression/Volume
        public Dictionary<string, List<double>> GeneratePressureVolumeChart(Essai_Forage essai)
        {
            var data = new Dictionary<string, List<double>>();

            data.Add("Pression (bar)", new List<double> {
                essai.presP0,
                essai.presFluage,
                essai.presLimite
            });

            data.Add("Volume (m³)", new List<double> {
                essai.VolSonde,
                essai.VolSonde * 1.2,
                essai.VolSonde * 1.5
            });

            return data;
        }

        // Exemple : Générer des données pour une courbe de fluage
        public (double[] xValues, double[] yValues) GenerateCreepCurve(Essai_Forage essai)
        {
            var xValues = new double[] { 0, 1, 2, 3, 4, 5 };
            var yValues = new double[] {
                essai.presP0,
                essai.presP0 * 0.95,
                essai.presP0 * 0.90,
                essai.presP0 * 0.85,
                essai.presP0 * 0.80,
                essai.presP0 * 0.75
            };

            return (xValues, yValues);
        }

        // Exemple : Générer des données pour un graphique de limite hyperbolique
        public (double[] xValues, double[] yValues) GenerateHyperbolicLimitChart(Essai_Forage essai)
        {
            var xValues = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            var yValues = new double[xValues.Length];

            for (int i = 0; i < xValues.Length; i++)
            {
                yValues[i] = 1.0 / xValues[i]; // Exemple : 1/VolumeNF
            }

            return (xValues, yValues);
        }
    }
}

//Public Sub DessineResultat(ByRef graphe As Winforms_Chart, ByVal Index As Integer, Pf As Single)
//        Dim MinX, MaxX, MinY, Maxy As Single

//        ' Pour chart1(2)
//        ' index = 0 : Module Pressiométrique
//        ' index = 1 : Module Pressiométrique NF
//        ' index = 2 : Module Elastique
//        ' index = 3 : Module Elastique NF
//        ' index = 4 : Pression Limite
//        ' index = 5 : Pression Limite NF
//        ' index = 6 : Pression Limite 1/V
//        ' index = 7 : Pression Limite NF 1/V
//        ' index = 8 : Pression Limite Hyperbole
//        ' index = 9 : Pression Limite Hyperbole NF

//        If E_MesureCount > 0 Then
//            Dim row As Integer
//            Dim X(E_MesureCount) As Single
//            Dim Y(E_MesureCount) As Single
//            Dim X1(E_MesureCount) As Single
//            Dim Y1(E_MesureCount) As Single

//            With graphe
//                .Series.Clear()
//                Select Case Lang
//                    Case 0
//                        .Series.Add("Experience")
//                    Case 1
//                        .Series.Add("Expérience")
//                End Select

//                ' Définition des axes
//                ' Définition axe des Y
//                With .ChartAreas(0).AxisY
//                    Select Case Lang
//                        Case 0
//                            Select Case Index
//                                Case 0 ' index = 0 : Module Pressiométrique
//                                    .Title = "Pressure (kPa)"
//                                Case 1  ' index = 1 : Module Pressiométrique NF
//                                    .Title = "PressureNF (kPa)"
//                                Case 2  ' index = 2 : Module Elastique
//                                    .Title = "Pressure (kPa)"
//                                Case 3 ' index = 3 : Module Elastique NF
//                                    .Title = "PressureNF (kPa)"
//                                Case 4  ' index = 4 : Pression Limite
//                                    .Title = "Pressure (kPa)"
//                                Case 5 ' index = 5 : Pression Limite NF
//                                    .Title = "PressionNF (kPa)"
//                                Case 6 ' index = 6 : Pression Limite 1/V
//                                    .Title = "1/(2.Epsr)"
//                                Case 7  ' index = 7 : Pression Limite NF 1/V
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 8 ' index = 8 : Pression Limite Hyperbole
//                                    .Title = "1/(2.Epsr)"
//                                Case 9  ' index = 9 : Pression Limite Hyperbole NF
//                                    .Title = "1/VolumeNF (cm^3)"
//                            End Select
//                        Case 1
//                            Select Case Index
//                                Case 0 ' index = 0 : Module Pressiométrique
//                                    .Title = "Pression (kPa)"
//                                Case 1  ' index = 1 : Module Pressiométrique NF
//                                    .Title = "PressionNF (kPa)"
//                                Case 2  ' index = 2 : Module Elastique
//                                    .Title = "Pression (kPa)"
//                                Case 3 ' index = 3 : Module Elastique NF
//                                    .Title = "PressionNF (kPa)"
//                                Case 4  ' index = 4 : Pression Limite
//                                    .Title = "Pression (kPa)"
//                                Case 5 ' index = 5 : Pression Limite NF
//                                    .Title = "PressionNF (kPa)"
//                                Case 6 ' index = 6 : Pression Limite 1/V
//                                    .Title = "1/(2.Epsr)"
//                                Case 7  ' index = 7 : Pression Limite NF 1/V
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 8 ' index = 8 : Pression Limite Hyperbole
//                                    .Title = "1/(2.Epsr)"
//                                Case 9  ' index = 9 : Pression Limite Hyperbole NF
//                                    .Title = "1/VolumeNF (cm^3)"
//                            End Select

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
//                    .MajorGrid.LineColor = System.Drawing.Color.LightGray
//                    '.MajorGrid.Interval = 1
//                    .RoundAxisValues()
//                End With

//                ' Définition axe des X
//                With .ChartAreas(0).AxisX
//                    Select Case Lang
//                        Case 0
//                            Select Case Index
//                                Case 0 ' index = 0 : Module Pressiométrique
//                                    .Title = "Deformation"
//                                Case 1  ' index = 1 : Module Pressiométrique NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 2  ' index = 2 : Module Elastique
//                                    .Title = "Deformation"
//                                Case 3 ' index = 3 : Module Elastique NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 4  ' index = 4 : Pression Limite
//                                    .Title = "Deformation"
//                                Case 5 ' index = 5 : Pression Limite NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 6 ' index = 6 : Pression Limite 1/V
//                                    .Title = "Pressure (kPa)"
//                                Case 7  ' index = 7 : Pression Limite NF 1/V
//                                    .Title = "PressureNF (kPa)"
//                                Case 8 ' index = 8 : Pression Limite Hyperbole
//                                    .Title = "Pressure (kPa)"
//                                Case 9  ' index = 9 : Pression Limite Hyperbole NF
//                                    .Title = "PressureNF (kPa)"
//                            End Select
//                            .Title = "Volume (cm^3)"
//                        Case 1
//                            Select Case Index
//                                Case 0 ' index = 0 : Module Pressiométrique
//                                    .Title = "Deformation"
//                                Case 1  ' index = 1 : Module Pressiométrique NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 2  ' index = 2 : Module Elastique
//                                    .Title = "Deformation"
//                                Case 3 ' index = 3 : Module Elastique NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 4  ' index = 4 : Pression Limite
//                                    .Title = "Deformation"
//                                Case 5 ' index = 5 : Pression Limite NF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 6 ' index = 6 : Pression Limite 1/V
//                                    .Title = "Pression (kPa)"
//                                Case 7  ' index = 7 : Pression Limite NF 1/V
//                                    .Title = "PressionNF (kPa)"
//                                Case 8 ' index = 8 : Pression Limite Hyperbole
//                                    .Title = "Pression (kPa)"
//                                Case 9  ' index = 9 : Pression Limite Hyperbole NF
//                                    .Title = "PressureNF (kPa)"
//                            End Select

//                    End Select
//                    .Minimum = 0
//                    '.Maximum = hlimy
//                    .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
//                    .LineWidth = 1
//                    .MajorGrid.LineColor = System.Drawing.Color.LightGray
//                    '.MajorGrid.Interval = 1
//                    .RoundAxisValues()
//                End With

//            End With

//            ReDim X(0 To E_MesureCount)
//            ReDim Y(0 To E_MesureCount)

//            'Mesures(0, E_MesureCount - 1) = Math.Round((.Fields("pression").Value), 1)
//            'Mesures(1, E_MesureCount - 1) = Math.Round((.Fields("volume15").Value), 1)
//            'Mesures(2, E_MesureCount - 1) = Math.Round((.Fields("volume30").Value), 1)
//            'Mesures(3, E_MesureCount - 1) = Math.Round((.Fields("volume60").Value), 1)
//            'Mesures(4, E_MesureCount - 1) = Math.Round((.Fields("contrainteRadiale").Value), 1)
//            'Mesures(5, E_MesureCount - 1) = Math.Round((.Fields("deformation").Value), 7)
//            'Mesures(6, E_MesureCount - 1) = Math.Round((.Fields("PNF").Value), 1)
//            'Mesures(7, E_MesureCount - 1) = Math.Round((.Fields("VNF").Value), 1)
//            'Mesures(8, E_MesureCount - 1) = Math.Round((.Fields("Qi").Value), 2)
//            'Mesures(9, E_MesureCount - 1) = Math.Round((.Fields("Pi").Value), 1)
//            'Mesures(10, E_MesureCount - 1) = Math.Round((.Fields("Qp").Value), 2)
//            'Mesures(11, E_MesureCount - 1) = Math.Round((.Fields("Pp").Value), 1)
//            'Mesures(12, E_MesureCount - 1) = Math.Round((.Fields("contrainteVerticale").Value), 1)

//            With graphe
//                MinX = 0
//                MaxX = 0
//                MinY = 0
//                Maxy = 0

//                For row = 1 To E_MesureCount
//                    Select Case Index
//                        Case 0 ' index = 0 : Module Pressiométrique
//                            X(row) = E_Mesures(5, row)        ' Déformation
//                            Y(row) = E_Mesures(4, row)        ' Pression
//                        Case 1 ' index = 1 : Module Pressiométrique NF
//                            X(row) = E_Mesures(7, row)        ' Volume NF
//                            Y(row) = E_Mesures(6, row)        ' Pression NF
//                        Case 2 ' index = 2 : Module Elastique
//                            X(row) = E_Mesures(6, row)        ' Pression NF
//                            Y(row) = E_Mesures(4, row)        ' Volume NF
//                        Case 3 ' index = 3 : Module Elastique NF
//                            X(row) = E_Mesures(7, row)
//                            Y(row) = E_Mesures(6, row)
//                        Case 4  ' index = 4 : Pression Limite
//                            X(row) = E_Mesures(6, row)
//                            Y(row) = E_Mesures(4, row)
//                        Case 5 ' index = 5 : Pression Limite NF
//                            X(row) = E_Mesures(7, row)
//                            Y(row) = E_Mesures(6, row)
//                        Case 6 ' index = 6 : Pression Limite 1/V
//                            X(row) = E_Mesures(4, row)
//                            If E_Mesures(6, row) > 0 Then
//                                Y(row) = 1 / (2 * E_Mesures(6, row))
//                            Else
//                                Y(row) = 10
//                            End If
//                        Case 7  ' index = 7 : Pression Limite NF 1/V
//                            X(row) = E_Mesures(7, row)
//                            If E_Mesures(8, row) > 0 Then
//                                Y(row) = 1 / E_Mesures(8, row)
//                            Else
//                                Y(row) = 10
//                            End If
//                        Case 8  ' index = 8 : Pression Limite Hyperbole
//                        Case 9  ' index = 9 : Pression Limite Hyperbole NF

//                    End Select
//                    If X(row) < MinX Then MinX = X(row)
//                    If X(row) > MaxX Then MaxX = X(row)
//                    If Y(row) < MinY Then MinY = Y(row)
//                    If Y(row) > Maxy Then Maxy = Y(row)
//                Next row

//                ' Définition du type de ligne
//                .Series(0).ChartType = SeriesChartType.Line
//                .Series(0).Color = System.Drawing.Color.Red

//                .Series(0).ChartType = SeriesChartType.Line
//                .Series(0).Color = System.Drawing.Color.Red
//                .ChartAreas(0).BackColor = System.Drawing.Color.Transparent
//                .ChartAreas(0).AxisY.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
//                .ChartAreas(0).AxisY.LineWidth = 1
//                .ChartAreas(0).AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray

//                ' recherche de la première arrondi supérieure à MaxY
//                .ChartAreas(0).AxisY.Minimum = 0
//                .ChartAreas(0).AxisY.RoundAxisValues()
//                .ChartAreas(0).AxisY.Maximum = Math.Truncate((Maxy * 100 + 1000) / 100)
//                .ChartAreas(0).AxisY.MajorGrid.Interval = Math.Round(.ChartAreas(0).AxisY.Maximum / 10)
//                .ChartAreas(0).AxisY.RoundAxisValues()

//                ' recherche de la première arrondi supérieure à MaxX
//                .ChartAreas(0).AxisX.Minimum = 0
//                .ChartAreas(0).AxisX.RoundAxisValues()
//                Select Case Index
//                    Case 0
//                        ' recherche de la première arrondi supérieure à MaxX
//                        .ChartAreas(0).AxisX.Maximum = Math.Truncate(MaxX * 10 + 1) / 10
//                    Case 1 To 6
//                        .ChartAreas(0).AxisX.Maximum = Math.Truncate((MaxX * 100 + 1000) / 100)
//                End Select
//                .ChartAreas(0).AxisX.MajorGrid.Interval = Math.Round(.ChartAreas(0).AxisX.Maximum / 10)
//                .ChartAreas(0).AxisX.RoundAxisValues()
//                .ChartAreas(0).BackColor = System.Drawing.Color.Transparent

//                ' Ajouter les deux séries de points
//                .Series(0).Points.DataBindXY(X, Y)

//            End With

//        Else
//            graphe.Visible = False
//        End If

//        '        If E_MesureCount > 0 Then
//        '            Dim points() As Object
//        '            Dim i As Integer
//        '            Dim math.absc As Integer
//        '            Dim ordo As Integer
//        '            Dim nommath.absc As String
//        '            Dim nomOrdo As String

//        '            Select Case Index

//        '                ' index = 0 : Module Pressiométrique
//        '                Case 0      ' module pressiometrique
//        '                    Math.absc = 5
//        '                    ordo = 4
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Déformation"
//        '                        nomOrdo = "Contrainte (kPa)"
//        '                    Else
//        '                        nommath.absc = "Strain"
//        '                        nomOrdo = "Pressure (kPa)"
//        '                    End If

//        '                    ' index = 1 : Module Pressiométrique NF
//        '                Case 1      ' Module Pressiométrique NF
//        '                    Math.absc = 7
//        '                    ordo = 6
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Volume NF(cm^3)"
//        '                        nomOrdo = "Contrainte NF(kPa)"
//        '                    Else
//        '                        nommath.absc = "Volume NF (cm^3)"
//        '                        nomOrdo = "Pressure NF (kPa)"
//        '                    End If

//        '                    ' index = 2 : Module Elastique
//        '                Case 2      ' Module Elastique
//        '                    Math.absc = 5
//        '                    ordo = 4
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Déformation"
//        '                        nomOrdo = "Contrainte (kPa)"
//        '                    Else
//        '                        nommath.absc = "Strain"
//        '                        nomOrdo = "Pressure (kPa)"
//        '                    End If

//        '                    ' index = 3 : Module Elastique NF
//        '                Case 3     ' Module Elastique NF
//        '                    Math.absc = 7
//        '                    ordo = 6
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Volume NF(cm^3)"
//        '                        nomOrdo = "Contrainte NF (kPa)"
//        '                    Else
//        '                        nommath.absc = "Volume NF(cm^3)"
//        '                        nomOrdo = "Pressure NF(kPa)"
//        '                    End If

//        '                    ' index = 4 : Pression Limite
//        '                Case 4     ' Pression Limite
//        '                    Math.absc = 5
//        '                    ordo = 4
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Déformation"
//        '                        nomOrdo = "Contrainte (kPa)"
//        '                    Else
//        '                        nommath.absc = "Strain"
//        '                        nomOrdo = "Pressure (kPa)"
//        '                    End If

//        '                    ' index = 5 : Pression Limite NF
//        '                Case 5      ' Pression Limite NF
//        '                    Math.absc = 7
//        '                    ordo = 6
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Volume NF (cm^3)"
//        '                        nomOrdo = "Pression NF (kPa)"
//        '                    Else
//        '                        nommath.absc = "Volume NF (cm^3)"
//        '                        nomOrdo = "Pressure NF (kPa)"
//        '                    End If

//        '                    ' index = 6 : Pression Limite 1 / Epsr
//        '                Case 6      ' Pression Limite 1/Epsr
//        '                    Math.absc = 4
//        '                    ordo = 15
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Pression (kPa)"
//        '                        nomOrdo = "1/Epsr"
//        '                    Else
//        '                        nommath.absc = "Pressure (kPa)"
//        '                        nomOrdo = "1/Epsr"
//        '                    End If

//        '                    ' index = 7 : Pression Limite NF 1/V
//        '                Case 7      ' Pression Limite 1/V
//        '                    Math.absc = 6
//        '                    ordo = 17
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Pression NF (kPa)"
//        '                        nomOrdo = "1/Volume NF (cm^3)"
//        '                    Else
//        '                        nommath.absc = "Pressure NF (kPa)"
//        '                        nomOrdo = "1/Volume NF (cm^3)"
//        '                    End If

//        '                    ' index = 8 : Pression Limite Hyperbole
//        '                Case 8      ' Pression Limite 1/V
//        '                    Math.absc = 4
//        '                    ordo = 15
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Pression (kPa)"
//        '                        nomOrdo = "1/Volume (cm^3)"
//        '                    Else
//        '                        nommath.absc = "Pressure (kPa)"
//        '                        nomOrdo = "1/Volume (cm^3)"
//        '                    End If

//        '                    ' index = 9 : Pression Limite Hyperbole NF
//        '                Case 9      ' Pression Limite 1/V
//        '                    Math.absc = 6
//        '                    ordo = 17
//        '                    If Lang = 1 Then
//        '                        nommath.absc = "Pression NF (kPa)"
//        '                        nomOrdo = "1/Volume NF (cm^3)"
//        '                    Else
//        '                        nommath.absc = "Pressure NF (kPa)"
//        '                        nomOrdo = "1/Volume NF (cm^3)"
//        '                    End If

//        '            End Select

//        'suite1:
//        '            ReDim points(0 To E_MesureCount - 1, 2)

//        '            For i = 0 To E_MesureCount - 1
//        '                X(i) = E_Mesures(Math.absc, i)
//        '                Select Case ordo
//        '                    Case 15
//        '                        If (E_Mesures(5, i) > 0) And (E_Mesures(4, i) >= Pf) Then points(i, 1) = 1 / E_Mesures(5, i)
//        '                    Case 17
//        '                        If (E_Mesures(7, i) > 0) And (E_Mesures(6, i) >= Pf) Then points(i, 1) = 1 / E_Mesures(7, i) * 100
//        '                    Case Else
//        '                        points(i, 1) = E_Mesures(ordo, i)
//        '                End Select


//        '            Next
//        '            GoTo fin

//        'suite2:
//        '            Dim Max As Single
//        '            Dim J As Integer
//        '            Dim points2() As Object
//        '            Dim A As Single
//        '            Max = E_Mesures(0, 0)

//        '            For i = 0 To E_MesureCount - 1
//        '                If E_Mesures(0, i) >= Max Then
//        '                    J = J + 1
//        '                    Max = E_Mesures(0, i)
//        '                    ReDim Preserve points2(2, J)
//        '                    If Index = 4 Then
//        '                        points2(0, J - 1) = E_Mesures(6, i)
//        '                    Else
//        '                        points2(0, J - 1) = E_Mesures(4, i)
//        '                    End If
//        '                    A = points2(0, J - 1)
//        '                    points2(1, J - 1) = E_Mesures(3, i) - E_Mesures(2, i)
//        '                End If
//        '            Next

//        '            ' Renversement du tableau pour l'affichage dans le graphe
//        '            ReDim points(J, 2)
//        '            For i = 0 To J - 1
//        '                X(i) = points2(0, i)
//        '                points(i, 1) = points2(1, i)
//        '            Next

//        '            ' Attribution des noms d'axes
//        '            If Lang = 1 Then
//        '                If Index = 3 Then
//        '                    nommath.absc = "Pression (kPa)"
//        '                Else
//        '                    nommath.absc = "PressionNF (kPa)"
//        '                End If
//        '                nomOrdo = "Fluage (cm^3)"
//        '            Else
//        '                If Index = 3 Then
//        '                    nommath.absc = "Pressure (kPa)"
//        '                Else
//        '                    nommath.absc = "Pressure NF (kPa)"
//        '                End If
//        '                nomOrdo = "Creep volume (cm^3)"
//        '            End If
//        'fin:
//        '            With graphe
//        '                .Visible = True
//        '                .ChartData = points
//        '                .Charttype = 16
//        '                .Plot.UniformAxis = False
//        '                .Plot.Axis(VtChAxisIdX).AxisTitle = nommath.absc
//        '                .Plot.Axis(VtChAxisIdY).AxisTitle = nomOrdo
//        '            End With
//        '        Else
//        '            graphe.Visible = False
//        '        End If
//    End Sub

