Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPress.Core.V17.Models

    Public Class Essai_Resistance

        <Key>
        Public Property R_RefEssai As String
        Public Property R_RefSonde As String

        Public Property R_ClassPrecis As Integer
        Public Property R_nomEssai As String
        Public Property R_dateEssai As Date
        Public Property R_typeEssai As Single
        Public Property R_difPresEauAir As Single
        Public Property R_hauteurCpv As Single
        Public Property R_A0 As Single
        Public Property R_A1 As Single
        Public Property R_A2 As Single
        Public Property R_A3 As Single

        Public R_Pel As Single

        Public R_Pe As Single

        Public R_MySondeResistance As Probe

        Public R_MesureCount As Integer

        ' === PROPRIÉTÉ DE NAVIGATION ===
        <ForeignKey("E_RefSonde")>
        Public Overridable Property E_MySonde As Probe

        <InverseProperty("Mesures_Resistance")>
        Public Overridable Property Mesures_Resistance As ICollection(Of MesuresResistance) = New List(Of MesuresResistance)

        Public Sub New()
            R_dateEssai = Date.Today.ToString("yyyy-MM-dd")
        End Sub

        Public Function GetRefEssai() As String
            GetRefEssai = r_refEssai
        End Function

        Public Function GetRefSonde() As String
            GetRefSonde = R_RefSonde
        End Function

        Public Function GetClassPrecis() As Integer
            GetClassPrecis = R_ClassPrecis
        End Function

        Public Function GetNomEssai() As String
            GetNomEssai = R_nomEssai
        End Function

        Public Function GetDateEssai() As String
            GetDateEssai = R_dateEssai
        End Function

        Public Function GetDifPresEauAir() As Single
            GetDifPresEauAir = R_difPresEauAir
        End Function

        Public Function GetHauteurCpv() As Single
            GetHauteurCpv = R_hauteurCpv
        End Function

        Public Function GetA0() As Single
            GetA0 = A0
        End Function

        Public Function GetA1() As Single
            GetA1 = A1
        End Function

        Public Function GetA2() As Single
            GetA2 = A2
        End Function

        Public Function GetA3() As Single
            GetA3 = A3
        End Function

        Public Function GetPel() As Single
            GetPel = Pel
        End Function

        Public Function GetMesureCount() As Integer
            GetMesureCount = Mesures_Resistance.Count
        End Function

        Public Function GetMesures(Ligne As Integer, colonne As Integer) As Single
            Select Case colonne
                Case 0 : GetMesures = Mesures_Resistance(Ligne).Id
                Case 1 : GetMesures = Mesures_Resistance(Ligne).RefEssai
                Case 2 : GetMesures = Mesures_Resistance(Ligne).IndiceMesure
                Case 3 : GetMesures = Mesures_Resistance(Ligne).Pression
                Case 4 : GetMesures = Mesures_Resistance(Ligne).Volume15
                Case 5 : GetMesures = Mesures_Resistance(Ligne).Volume30
                Case 6 : GetMesures = Mesures_Resistance(Ligne).Volume60
            End Select
        End Function

        Public Sub SetRefEssai(ByVal refEssa As String)
            R_RefEssai = refEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetRefSonde(ByVal refSond As String)
            R_RefSonde = refSond
            Call IHaveBeenModified()
        End Sub

        Public Sub SetClassPrecis(ByVal Precis As Integer)
            R_ClassPrecis = Precis
            Call IHaveBeenModified()
        End Sub

        Public Sub SetNomEssai(ByVal nomEssa As String)
            R_nomEssai = nomEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDateEssai(ByVal dateEssa As Date)
            R_dateEssai = dateEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDifPresEauAir(ByVal difPresEauAi As Single)
            R_difPresEauAir = difPresEauAi
            Call IHaveBeenModified()
        End Sub

        Public Sub SetHauteurCpv(ByVal hauteurCp As Single)
            R_hauteurCpv = hauteurCp
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMesureCount(ByVal Count As Integer)
            R_MesureCount = Mesures_Resistance.Count
            Call IHaveBeenModified()
        End Sub

        Public Sub SetA0(ByVal A As Single)
            A0 = A
            Call IHaveBeenModified()
        End Sub

        Public Sub SetA1(ByVal A As Single)
            A1 = A
            Call IHaveBeenModified()
        End Sub

        Public Sub SetA2(ByVal A As Single)
            A2 = A
            Call IHaveBeenModified()
        End Sub

        Public Sub SetA3(ByVal A As Single)
            A3 = A
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPel(ByVal Pression As Single)
            Pel = Pression
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMySonde(ByRef sonde As Probe)
            If MySonde IsNot Nothing Then MySonde = New Probe
            MySonde = sonde

            'Ici sonde n'est déjà plus valide
            R_RefSonde = MySonde.GetRefSonde
        End Sub

        ' ********************************************************************************************
        ' Crée un Nouvel EssaiResistance
        ' ********************************************************************************************
        Public Sub NouvelEssai(ByVal Table As Boolean)
            Dim ObjetDataRow As DataRow

            R_RefEssai = MySonde.GiveMeANewRefEssai(EssaiCalib.Resistance)
            R_RefSonde = ""
            R_nomEssai = "à définir"
            R_dateEssai = DateValue(Now)
            R_difPresEauAir = 0
            R_hauteurCpv = 0
            R_A0 = 0
            R_A1 = 0
            R_A2 = 0
            R_A3 = 0
            setResistanceNouvelleMesure(0, 0, 0)

            If Table Then ' On ajoute un essai à MonFichier.Tables("LesEssaisResistancePropre") seulement si Table est vrai
                If MonChantier.Tables("LesEssaisResistancePropre") Is Nothing Then
                    MonChantier.Tables.Add("LesEssaisResistancePropre")
                    With MonChantier.Tables("LesEssaisResistancePropre")
                        .Columns.Add("refEssai", GetType(String))
                        .Columns.Add("refSonde", GetType(String))
                        .Columns.Add("nomEssai", GetType(String))
                        .Columns.Add("dateEssai", GetType(String))
                        .Columns.Add("difPresEauAir", GetType(String))
                        .Columns.Add("hauteurCpv", GetType(String))
                        .Columns.Add("MesureCount", GetType(String))
                        .Columns.Add("a0", GetType(String))
                        .Columns.Add("a1", GetType(String))
                        .Columns.Add("a2", GetType(String))
                        .Columns.Add("a3", GetType(String))
                    End With
                Else
                    ' Ajout d'une ligne à la Table "LesEssaisForage"
                    ObjetDataRow = MaListeSondes.Tables("LesEssaisResistancePropre").NewRow()

                    ObjetDataRow("refEssai") = MySonde.GiveMeANewRefEssai(EssaiCalib.Resistance)
                    ObjetDataRow("refSonde") = MySonde.P_refSonde
                    MyResistance.R_RefEssai = ObjetDataRow("refEssai").ToString

                    MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Add(ObjetDataRow)


                    'MonChantier.Tables("LesEssaisResistancePropre").Rows.Add()
                End If
            End If

        End Sub

        ' **********************************************************************************
        ' Affiche les MesuresResistances dans DgridResistance de FrmSonde
        ' **********************************************************************************
        Public Sub AfficheMesuresDansTab(ByRef Table As DataGridView, ByVal RefEssaiResistance As String)
            Dim i As Integer
            Dim pression, volume As Single

            ' Remplissage du Tableau

            Table.Rows.Clear()
            If Mesures_Resistance.Count = 0 Then
                ' Création du nouveau tableau si mesurecount est nul
                Me.MyResistanceNouvelleMesure(0, 0, 0)
            End If
            For i = 0 To Mesures_Resistance.Count - 1
                With Table
                    .Rows.Add()
                    pression = Mesures_Resistance(0).Pression
                    volume = Mesures_Resistance(0).Volume60
                    'Me.MyResistanceNouvelleMesure(i, pression, volume)
                    .Rows(i).Cells(0).Value = pression
                    .Rows(i).Cells(1).Value = volume
                End With
            Next
        End Sub

        ' ********************************************************************************
        ' Enregistre les Mesuresresistance depuis Table
        ' ********************************************************************************
        'Public Sub AfficheMesuresDepuisTab(ByRef Table As DataGridView)
        '    Dim i, NbRow As Integer


        '    NbRow = Table.Rows.Count
        '    R_MesureCount = Mesures_Resistance.Count - 1

        '    ' Remplissage du Tableau
        '    For i = 0 To NbRow - 1
        '        With Table
        '            Mesures_Resistance(i).Pression = .Rows(i).Cells(0).Value
        '            Mesures_Resistance(i).Volume60 = .Rows(i).Cells(1).Value
        '        End With
        '    Next
        'End Sub

        ' *************************************************************************************************************************
        ' Ajour d'une nouvelle mesure à la Table "LesMesuresResistancePropre"
        ' si Table est Faux, il faut créer la Table, sinon on la remplit avec une nouvelle ligne de mesure
        ' *************************************************************************************************************************

        Public Sub LesMesuresResistancePropreNouvelleMesure(ByVal Table As Boolean, ByVal Reference As String, ByVal Indice As Integer, ByVal pression As Single, ByVal Volume As Single)
            Dim NbCount As Integer
            Dim NewLine As Boolean

            NewLine = False
            ' Incrémentation du compteur de mesures
            If Indice > R_MesureCount - 1 Then
                ' Nouvelle ligne pour la Table
                R_MesureCount = R_MesureCount + 1
                NewLine = True
            End If

            ' La Table existe, on peut la remplir
            If MaListeSondes.Tables("LesMesuresResistancePropre") Is Nothing Then
                Select Case Lang
                    Case 0
                        MsgBox("Table 'LesMesuresResistancePropre' is not created")
                    Case 1
                        MsgBox("La Table 'LesMesuresResistancePropre' n'existe pas")
                End Select
                Exit Sub
            End If

            ' Ajout d'une ligne à la Table "LesMesuresResistancePropre"
            'Reference = MySonde.GiveMeANewRefEssai(EssaiCalib.Resistance)
            Reference = MyResistance.R_RefEssai
            MaListeSondes.Tables("LesMesuresResistancePropre").Rows.Add(Reference, Indice, 0, 0)
            'MonChantier.Tables("LesMesuresResistancePropre").Rows.Add()

            ' Remplissage des nouvelles Valeurss
            With MaListeSondes.Tables("LesMesuresResistancePropre")
                NbCount = .Rows.Count
                If NewLine Then
                    ' Ajout d'une nouvelle ligne de mesure
                    .Rows.Add(R_MesureCount - 1, MyResistance.R_RefEssai, pression, Volume)
                Else
                    ' Remplissage de la Table
                    .Rows(Indice).Item("refEssai") = MyResistance.R_RefEssai
                    .Rows(Indice).Item("indiceMesure") = Indice + 1
                    .Rows(Indice).Item("pression") = pression
                    .Rows(Indice).Item("volume") = Volume
                End If
                'Indice = Valeurs(.Rows(NbCount - 2).Item("indiceMesure").ToString)
            End With

        End Sub


        ' *************************************************************************************************************************
        ' Ajour d'une nouvelle mesure à la Table "Mesures" de MyResistance
        ' *************************************************************************************************************************
        Public Sub MyResistanceNouvelleMesure(ByVal Indice As Integer, ByVal pression As Single, ByVal Volume As Single)
            Dim NewLine As Boolean

            NewLine = False
            ' Incrémentation du compteur de mesures
            If Indice > R_MesureCount - 1 Then
                ' Nouvelle ligne pour la Table
                R_MesureCount = R_MesureCount + 1
                NewLine = MyResistance.Mesures_Resistance
            End If


            ' Redimensionnement du tableau dynamique de Essais

            MyResistance.Mesures_Resistance(R_MesureCount - 1) = pression
            MyResistance.Mesures_Resistance(R_MesureCount - 1) = Volume

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            'Call IHaveBeenModified
        End Sub

        '' *****************************************************************************
        '' Dessine les courbes dans un Chart
        '' *****************************************************************************
        'Public Sub DessineCourbe(ByRef graphe As Winforms_chart)
        '    ' Définition des variables utilisées pour le graphique
        '    Dim Max As Single
        '    Dim X(MesureCount)
        '    Dim Y(MesureCount)
        '    Dim X1(MesureCount)
        '    Dim Y1(MesureCount)

        '    If MesureCount > 0 Then
        '        Dim row, column As Integer

        '        With graphe
        '            .Series.Clear()
        '            .ChartAreas(0).BackColor = System.Drawing.Color.Transparent
        '            ' format the chart area

        '            ' Intitulé de la courbe expérimentale
        '            Select Case Lang
        '                Case 0
        '                    .Series.Add("Test " & nomEssai)
        '                    .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
        '                    .Series(0).Color = System.Drawing.Color.Blue
        '                    .Series.Add("Self Resistance " & MyEssai.E_nomEssai)
        '                    .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line
        '                    .Series(1).Color = System.Drawing.Color.Red
        '                Case 1
        '                    .Series.Add("Essai " & nomEssai)
        '                    .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
        '                    .Series(0).Color = System.Drawing.Color.Blue
        '                    .Series.Add("Résistance propre " & MyEssai.E_nomEssai)
        '                    .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line
        '                    .Series(1).Color = System.Drawing.Color.Red
        '            End Select

        '            With .ChartAreas(0).AxisX
        '                .Title = "Pression (kPa)"
        '                .Minimum = 0
        '                .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
        '                .LineWidth = 1
        '                .MajorGrid.LineColor = System.Drawing.Color.LightGray
        '                .RoundAxisValues()
        '            End With

        '            With .ChartAreas(0).AxisY
        '                .Title = "Volume (cm^3)"
        '                .Minimum = 0
        '                .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
        '                .LineWidth = 1
        '                .MajorGrid.LineColor = System.Drawing.Color.LightGray
        '                .RoundAxisValues()
        '            End With

        '            ' Dessin de la courbe expérimentale
        '            Max = 0
        '            For row = 1 To MesureCount
        '                X(row - 1) = Mesures(0, row - 1)
        '                Y(row - 1) = Mesures(1, row - 1)
        '                If Mesures(0, row - 1) >= Max Then Max = Mesures(0, row - 1)
        '            Next row
        '            .Series(0).Points.DataBindXY(X, Y)

        '            ' Dessin de la courbe théorique si ls coefficients ont étés calculés
        '            If Not (A0 = 0 And A1 = 0 And A2 = 0 And A3 = 0) Then
        '                If Not (A0 = 0 And A1 = 0 And A2 = 0 And A3 = 0) Then
        '                    For row = 1 To MesureCount
        '                        If MesureCount > 1 Then
        '                            X1(row - 1) = (row - 1) * Max / (MesureCount - 1)
        '                            Y1(row - 1) = A3 * X1(row - 1) ^ 3 + A2 * X1(row - 1) ^ 2 + A1 * X1(row - 1) + A0
        '                        Else
        '                            X1(row - 1) = X(row - 1)
        '                            Y1(row - 1) = Y(row - 1)
        '                        End If
        '                    Next row
        '                Else
        '                    For column = 3 To 4
        '                        For row = 1 To MesureCount
        '                            If MesureCount > 1 Then
        '                                X1(row - 1) = (row - 1) * Max / (MesureCount - 1)
        '                                Y1(row - 1) = 0
        '                            Else
        '                                X1(row - 1) = X(row - 1)
        '                                Y1(row - 1) = 0
        '                            End If

        '                        Next row
        '                    Next column
        '                End If
        '                .Series(1).Points.DataBindXY(X1, Y1)
        '            End If
        '            .Series(0).Points.DataBindXY(X, Y)
        '        End With
        '        graphe.Visible = True
        '    Else
        '        graphe.Visible = False
        '    End If
        'End Sub

        Private Function VolResistance(ByVal Pres As Double) As Double
            VolResistance = A3 * Pres ^ 3 + A2 * Pres ^ 2 + A1 * Pres + A0
        End Function

        ' ********************************************************************************
        ' Calcul de la pression pel
        ' ********************************************************************************
        Public Sub CalculePel(ByVal Vol As Single)

            ' Recherche par dichotomie de la valeur de Pel
            Dim Pression, XMin, XMax, Volume, VolPel As Double
            Dim i As Integer
            XMin = 0
            XMax = 0

            ' Recherche de la borne maximum
            For i = 0 To 100
                XMax = XMax + 100
                Volume = VolResistance(XMax)

                ' Calcul de VolPel 
                If Vol = 0 Then
                    Select Case MySonde.GetTypeSonde

                    '    SoupleTypeE = 0
                    '    SoupleTypeG = 1
                    '    TubeFenduCourte = 2
                    '    TubeFenduLongue = 3
                    '    Geomecametre = 4
                    '    Autre = 5
                        Case TypeDeSonde.SoupleTypeE
                            VolPel = 700
                            If Volume > 700 Then Exit For
                        Case TypeDeSonde.SoupleTypeG
                            VolPel = 700
                            If Volume > 700 Then Exit For
                        Case TypeDeSonde.TubeFenduCourte
                            VolPel = 550
                            If Volume > 550 Then Exit For
                        Case TypeDeSonde.TubeFenduLongue
                            VolPel = 700
                            If Volume > 550 Then Exit For
                        Case TypeDeSonde.Autre
                            VolPel = 700
                            If Volume > 700 Then Exit For
                    End Select
                Else
                    ' Calcul de Pe
                    VolPel = Vol
                    If Volume > 700 Then Exit For
                End If
            Next i

            ' Recherche de Pel par dichotomie
            For i = 0 To 1000
                Pression = (XMin + XMax) / 2
                Volume = VolResistance(Pression)
                ' Test de fin de boucle
                If Math.Abs(Volume - VolPel) < 0.5 Then Exit For
                If Volume > VolPel Then XMax = Pression
                If Volume <= VolPel Then XMin = Pression
            Next i

            If Vol = 0 Then
                Pel = Math.Round(Pression, 0)
            Else
                Pe = Math.Round(Pression, 0)
            End If

        End Sub

        ' ****************************************************************************
        ' Calcul les coefficients de la courbe de Résistance
        ' ****************************************************************************
        Public Sub CalculeCoef()
            Dim poly As Polynome
            Dim x() As Single
            Dim Y() As Single
            Dim MesureCount As Integer

            ' Dimensionnement des deux tableaux de mesures à passer à RegPolynome
            MesureCount = MyResistance.Mesures_Resistance.Count
            ReDim x(MesureCount - 1)
            ReDim Y(MesureCount - 1)

            ' Remplissage des tableaux avec les mesures
            Dim i As Integer
            For i = 0 To MesureCount - 1
                x(i) = MyResistance.Mesures_Resistance(i).Pression
                Y(i) = MyResistance.Mesures_Resistance(i).Volume60
            Next

            poly = RegPolynome(MesureCount, x, Y, 3)

            If Not poly.Erreur Then
                A0 = Math.Round(poly.A0, 7)
                A1 = Math.Round(poly.A1, 7)
                A2 = Math.Round(poly.A2, 7)
                A3 = Math.Round(poly.A3, 7)
            End If

            MySonde.P_MaResistance = MyResistance

            ' Mise à jour de MySonde.Resistance(ind)
            With MySonde
                For i = 0 To MyResistance.Mesures_Resistance.Count - 1
                    If .Resistance(i).refEssai = MyResistance.R_RefEssai Then
                        .Resistance(i).ind = MyResistance.Mesures_Resistance(i).ind
                    End If
                Next i
            End With

            Call IHaveBeenModified()
        End Sub

        ' *******************************************************************************************************
        ' Charge les Mesures depuis MaListeSondes.Tables("LesMesuresResistancePropre") vers MyResistance.Mesures
        ' *******************************************************************************************************
        Public Sub ChargeMesures(ByVal ReferenceEssai As String)
            Dim j, k As Integer
            Dim Pression, Volume As Single
            Dim champ As String
            'Analyse du DataSet; j est l'indice de mesures
            R_MesureCount = 0
            k = 0
            If MyResistance Is Nothing Then MyResistance = New Essai_Resistance
            For j = 0 To MaListeSondes.Tables("LesMesuresResistancePropre").Rows.Count - 1
                If MaListeSondes.Tables("LesMesuresResistancePropre").Rows(j).RowState <> DataRowState.Deleted Then
                    If ReferenceEssai = MaListeSondes.Tables("LesMesuresResistancePropre").Rows(j).Item("RefEssai").ToString Then
                        'And MesureCount = Valeurs(MaListeSondes.Tables("LesMesuresResistancePropre").Rows(j).Item("indiceMesure").ToString) Then
                        ' Nouvel essai

                        ' Chargement des attributs de l'objet depuis la base de données
                        With Me
                            champ = MaListeSondes.Tables("LesMesuresResistancePropre").Rows(j).Item("pression").ToString
                            Pression = Valeurs(champ)
                            champ = MaListeSondes.Tables("LesMesuresResistancePropre").Rows(j).Item("volume").ToString
                            Volume = Valeurs(champ)

                            ' Création et enregistrement des mesures dans MyResistanceMesures
                            Call MyResistanceNouvelleMesure(k, Math.Round(Pression, 1), Math.Round(Volume, 1))
                            k = k + 1
                        End With
                    End If
                End If
            Next j
        End Sub


        'Private Sub IHaveBeenModified()
        '    ' Génère un événement indiquant que des modifications ont eu lieu
        '    If Not EventsSuspendus Then
        '        Call MySonde.IHaveBeenModified()
        '        ' RaiseEvent HasBeenModified
        '    End If
        'End Sub

        'Public Sub SuspendreEvents()
        '    EventsSuspendus = True
        'End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        ' *******************************************************************************
        ' Ajoute une nouvelle ligne à MaListeSondes.Tables("LesMesuresResistancePropre")
        ' Ajoute une nouvelle ligne à MyResistance.Mesures
        ' *******************************************************************************
        Public Sub AddMesures(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouter

            ' Ajout d'une ligne dans le Tableau Mesures
            MesureCount = MesureCount + 1
            ReDim Preserve Mesures(1, MesureCount)

            ' Ajout d'une ligne dans la Table "LesMesuresresistancePropre"
            With MaListeSondes.Tables(3)
                .Rows.Add(.Rows.Count - 1, IndMesure, Pression, Volume)
            End With

            Call IHaveBeenModified()
        End Sub

        Public Sub AddMesuresResistance(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouter

            ' Ajout d'une ligne dans le Tableau Mesures
            MesureCount = MesureCount + 1
            ReDim Preserve Mesures(1, MesureCount)

            Mesures(0, MesureCount - 1) = Pression
            Mesures(1, MesureCount - 1) = Volume

            Call IHaveBeenModified()
        End Sub

        Public Sub SupprimeMesures(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouterPublic Sub AddMesures() ' i est le nombre de mesures à ajouter
            Dim i As Integer

            ' Déplacement des mesures dans le tableau Mesures de MyResistance 
            For i = IndMesure To MesureCount - 2
                Mesures(0, i) = Mesures(0, i + 1)
                Mesures(1, i) = Mesures(1, i + 1)
            Next i

            ' Suppression d'une ligne dans le tableau Mesures de MyResistance
            MesureCount = MesureCount - 1
            ReDim Preserve Mesures(1, MesureCount - 1)

            ' Suppression d'une ligne dans le tableau "LesMesuresResistancePropre" de MaListeSondes
            'With MaListeSondes.Tables(4)
            '    .Rows.RemoveAt(IndMesure)
            'End With

            Call IHaveBeenModified()
        End Sub
    End Class
End Namespace