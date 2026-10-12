// GaiaPressBlazorWebApp/Charting/ForageChartGenerator.cs
namespace GaiaPressBlazorWebApp.Charting
{
    /// <summary>
    /// Génère des données de graphique pour les essais de forage.
    /// </summary>
    public class EssaiChartGenerator
    {
        // Méthode pour générer des données de courbe (ancienne DessineCourbe adaptée)
        public Dictionary<string, List<double>> GeneratePressureVolumeChart(Essai_Forage essai)
        {
            var data = new Dictionary<string, List<double>>();

            // Exemple : Ajoutez des séries de données
            data.Add("Pression (bar)", new List<double> {
                essai.E_presP0,
                essai.E_presFluage,
                essai.E_presLimite
            });

            data.Add("Volume (m³)", new List<double> {
                essai.E_VolSonde,
                essai.E_VolSonde * 1.2,
                essai.E_VolSonde * 1.5
            });

            return data;
        }

        // Méthode pour générer des données de courbe de fluage
        public (double[] xValues, double[] yValues) GenerateCreepCurve(Essai_Forage essai)
        {
            // Exemple : Simuler une courbe de fluage
            var xValues = new double[] { 0, 1, 2, 3, 4, 5 };
            var yValues = new double[] {
                essai.E_presP0,
                essai.E_presP0 * 0.95,
                essai.E_presP0 * 0.90,
                essai.E_presP0 * 0.85,
                essai.E_presP0 * 0.80,
                essai.E_presP0 * 0.75
            };

            return (xValues, yValues);
        }
    }
}

//Public Sub DessineCourbe(ByRef graphe As Winforms_Chart, ByVal Index As Integer)
//        Dim MinX, MaxX, MinY, MaxY As Single
//        ' Pour chart1(0)
//        ' index = 0 : Contrainte/Déformation
//        ' index = 1 : Contrainte/Volume
//        ' index = 2 : Volume/Contrainte
//        ' index = 3 : Fluage/Contrainte
//        ' index = 4 : Fluage/PressionNF
//        ' index = 5 : PressionNF/VolumeNF
//        ' index = 6 : VolumeNF/PressionNF

//        If E_MesureCount > 0 Then
//            Dim row As Integer
//            Dim X(E_MesureCount - 1) As Single
//            Dim Y(E_MesureCount - 1) As Single
//            Dim X1(E_MesureCount - 1) As Single
//            Dim Y1(E_MesureCount - 1) As Single

//            With graphe
//                .BackColor = System.Drawing.Color.Transparent
//                .Series.Clear()
//                Select Case Lang
//                    Case 0
//                        .Series.Add(MyEssai.E_nomEssai)
//                    Case 1
//                        .Series.Add(MyEssai.E_nomEssai)
//                End Select

//                ' Définition des axes
//                ' Définition axe des Y
//                With .ChartAreas(0).AxisY
//                    Select Case Lang
//                        Case 0
//                            Select Case Index
//                                Case 0 ' index = 0 : Contrainte/Déformation
//                                    .Title = "Pressure (kPa)"
//                                Case 1  ' index = 1 : Contrainte/Volume
//                                    .Title = "Pressure (kPa)"
//                                Case 2 ' index = 2 : Volume/Contrainte
//                                    .Title = "Volume (cm^3)"
//                                Case 3  ' index = 3 : Fluage/Pression
//                                    .Title = "Creep (cm^3)"
//                                Case 4 ' index = 3 : Fluage/PressionNF
//                                    .Title = "Creep (cm^3)"
//                                Case 5 ' index = 5 : PressionNF/VolumeNF
//                                    .Title = "PressureNF (kPa)"
//                                Case 6  ' index = 6 : VolumeNF/PressionNF
//                                    .Title = "VolumeNF (cm^3)"
//                            End Select
//                        Case 1
//                            Select Case Index
//                                Case 0 ' index = 0 : Contrainte/Déformation
//                                    .Title = "Pression (kPa)"
//                                Case 1  ' index = 1 : Contrainte/Volume
//                                    .Title = "Pression (kPa)"
//                                Case 2 ' index = 2 : Volume/Contrainte
//                                    .Title = "Volume (cm^3)"
//                                Case 3  ' index = 3 : Fluage/Pression
//                                    .Title = "Fluage (cm^3)"
//                                Case 4 ' index = 3 : Fluage/PressionNF
//                                    .Title = "Fluage (cm^3)"
//                                Case 5 ' index = 5 : PressionNF/VolumeNF
//                                    .Title = "PressionNF (kPa)"
//                                Case 6  ' index = 6 : VolumeNF/PressionNF
//                                    .Title = "VolumeNF (cm^3)"
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
//                                Case 0 ' index = 0 : Contrainte/Déformation
//                                    .Title = "Deformation"
//                                Case 1  ' index = 1 : Contrainte/Volume
//                                    .Title = "Volume (cm^3)"
//                                Case 2 ' index = 2 : Volume/Contrainte
//                                    .Title = "Pressure (kPa)"
//                                Case 3  ' index = 3 : Fluage/Pression
//                                    .Title = "Pressure (kPa)"
//                                Case 4 ' index = 4 : Fluage/PressionNF
//                                    .Title = "PressureNF (kPa)"
//                                Case 5 ' index = 5 : PressionNF/VolumeNF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 6  ' index = 6 : VolumeNF/PressionNF
//                                    .Title = "PressureNF (kPa)"
//                            End Select
//                            .Title = "Volume (cm^3)"
//                        Case 1
//                            Select Case Index
//                                Case 0 ' index = 0 : Contrainte/Déformation
//                                    .Title = "Deformation"
//                                Case 1  ' index = 1 : Contrainte/Volume
//                                    .Title = "Volume (cm^3)"
//                                Case 2 ' index = 2 : Volume/Contrainte
//                                    .Title = "Pression (kPa)"
//                                Case 3  ' index = 3 : Fluage/Pression
//                                    .Title = "Pression (kPa)"
//                                Case 4 ' index = 4 : Fluage/PressionNF
//                                    .Title = "PressionNF (kPa)"
//                                Case 5 ' index = 5 : PressionNF/VolumeNF
//                                    .Title = "VolumeNF (cm^3)"
//                                Case 6  ' index = 6 : VolumeNF/PressionNF
//                                    .Title = "PressionNF (kPa)"
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

//            ReDim X(0 To E_MesureCount - 1)
//            ReDim Y(0 To E_MesureCount - 1)
//            MinX = 0
//            MaxX = 0
//            MinY = 0
//            MaxY = 0
//            With graphe

//                'Mesures(0, E_MesureCount - 1) = Math.Round((.Fields("pression").Value), 1)
//                'Mesures(1, E_MesureCount - 1) = Math.Round((.Fields("volume15").Value), 1)
//                'Mesures(2, E_MesureCount - 1) = Math.Round((.Fields("volume30").Value), 1)
//                'Mesures(3, E_MesureCount - 1) = Math.Round((.Fields("volume60").Value), 1)
//                'Mesures(4, E_MesureCount - 1) = Math.Round((.Fields("contrainteRadiale").Value), 1)
//                'Mesures(5, E_MesureCount - 1) = Math.Round((.Fields("deformation").Value), 7)
//                'Mesures(6, E_MesureCount - 1) = Math.Round((.Fields("PNF").Value), 1)
//                'Mesures(7, E_MesureCount - 1) = Math.Round((.Fields("VNF").Value), 1)
//                'Mesures(8, E_MesureCount - 1) = Math.Round((.Fields("Qi").Value), 2)
//                'Mesures(9, E_MesureCount - 1) = Math.Round((.Fields("Pi").Value), 1)
//                'Mesures(10, E_MesureCount - 1) = Math.Round((.Fields("Qp").Value), 2)
//                'Mesures(11, E_MesureCount - 1) = Math.Round((.Fields("Pp").Value), 1)
//                'Mesures(12, E_MesureCount - 1) = Math.Round((.Fields("contrainteVerticale").Value), 1)

//                For row = 0 To E_MesureCount - 1
//                    If E_Mesures(0, row) Is Nothing Then Exit For

//                    Select Case Index
//                        Case 0 ' index = 0 : Contrainte/Déformation
//                            X(row) = E_Mesures(9, row)
//                            Y(row) = E_Mesures(7, row)
//                        Case 1  ' index = 1 : Contrainte/Volume
//                            X(row) = E_Mesures(6, row)
//                            Y(row) = E_Mesures(7, row)
//                        Case 2 ' index = 2 : Volume/Contrainte
//                            X(row) = E_Mesures(7, row)
//                            Y(row) = E_Mesures(6, row)
//                        Case 3  ' index = 3 : Fluage/Pression
//                            X(row) = E_Mesures(7, row)
//                            If row = 0 Then
//                                Y(row) = 0
//                            Else
//                                Y(row) = E_Mesures(6, row) - E_Mesures(5, row)
//                            End If
//                        Case 4 ' index = 4 : Fluage/PressionNF
//                            X(row) = E_Mesures(10, row)
//                            If row <= 1 Then
//                                Y(row) = 0
//                            Else
//                                Y(row) = E_Mesures(6, row) - E_Mesures(5, row)
//                            End If

//                        Case 5 ' index = 5 : PressionNF/VolumeNF
//                            X(row) = E_Mesures(11, row)
//                            Y(row) = E_Mesures(10, row)
//                        Case 6  ' index = 6 : VolumeNF/PressionNF
//                            X(row) = E_Mesures(10, row)
//                            Y(row) = E_Mesures(11, row)
//                    End Select
//                    If X(row) < MinX Then MinX = X(row)
//                    If X(row) > MaxX Then MaxX = X(row)
//                    If Y(row) < MinY Then MinY = Y(row)
//                    If Y(row) > MaxY Then MaxY = Y(row)
//                Next row

//                ' Définition du type de ligne
//                .Series(0).ChartType = SeriesChartType.Line
//                .Series(0).Color = System.Drawing.Color.Red
//                .ChartAreas(0).BackColor = System.Drawing.Color.Transparent
//                .ChartAreas(0).AxisY.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
//                .ChartAreas(0).AxisY.LineWidth = 1
//                .ChartAreas(0).AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray

//                ' recherche de la première arrondi supérieure à MaxY
//                .ChartAreas(0).AxisY.Minimum = 0
//                .ChartAreas(0).AxisY.RoundAxisValues()

//                .ChartAreas(0).AxisY.Maximum = Math.Truncate((MaxY * 100 + 1000) / 100)
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
//                .ChartAreas(0).AxisX.RoundAxisValues()
//                .ChartAreas(0).BackColor = System.Drawing.Color.Transparent

//                ' Ajouter les deux séries de points
//                .Series(0).Points.DataBindXY(X, Y)

//            End With
//            graphe.Visible = True
//        Else
//            graphe.Visible = False
//        End If
//    End Sub
