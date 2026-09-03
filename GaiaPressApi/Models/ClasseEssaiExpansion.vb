Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPress.Core.V17.Models

    Public Class Essai_Expansion

        <Key>
        Public Property E_RefEssai As String

        Public Property E_RefSonde As String
        Public Property E_ClassPrecis As Integer
        Public Property E_typeEssai As String
        Public Property E_nomEssai As String
        Public Property E_dateEssai As Date
        Public Property E_difPresEauAir As Single
        Public Property E_hauteurCpv As Single
        Public Property E_Di As Single
        Public Property E_Vc As Single
        Public Property E_Vs As Single
        Public Property E_EpMembrane As Single
        Public Property E_Prp As Single
        Public Property E_PrV As Single
        Public Property E_coefficientA As Single
        Public Property E_coefficientB As Single
        Public Property E_PrecisVc As Single
        Public Property E_PrecisVs As Single
        Public Property E_PrEpMembrane As Single
        Public Property E_PrecisA As Single
        Public Property E_PrecisB As Single

        ' === PROPRIÉTÉS DE NAVIGATION ===
        <ForeignKey("E_RefSonde")>
        Public Overridable Property E_MySonde As Probe

        <InverseProperty("Mesures_Expansion")>
        Public Overridable Property Mesures_Expansion As ICollection(Of MesuresExpansion) = New List(Of MesuresExpansion)


        Public Sub New()
            E_dateEssai = Date.Today.ToString("yyyy-MM-dd")
        End Sub

        Public Function GetRefEssai() As String
            GetRefEssai = E_RefEssai
        End Function

        Public Function GetRefSonde() As String
            GetRefSonde = E_RefSonde
        End Function

        Public Function GetClassPrecis() As Integer
            GetClassPrecis = E_ClassPrecis
        End Function

        Public Function GetNomEssai() As String
            GetNomEssai = E_nomEssai
        End Function

        Public Function GetDateEssai() As String
            GetDateEssai = E_dateEssai
        End Function

        Public Function GetDifPresEauAir() As Single
            GetDifPresEauAir = E_difPresEauAir
        End Function

        Public Function GetHauteurCpv() As Single
            GetHauteurCpv = E_hauteurCpv
        End Function

        Public Function GetDi() As Single
            GetDi = Di
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" du Volume Vc de la sonde correspondant à RefEssai
        Public Function GetVc() As Single
            Dim i As Integer
            GetVc = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If r_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetVc = .Rows(i).Item("Vc").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" du Volume Vs de la sonde correspondant à RefEssai
        Public Function GetVs() As Single
            Dim i As Integer
            GetVs = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetVs = .Rows(i).Item("Vs").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Table "LesEssaisExpansionPropre" du Volume PrecisVc de la sonde correspondant à RefEssai
        Public Function GetPrecisVc() As Single
            Dim i As Integer
            GetPrecisVc = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetPrecisVc = .Rows(i).Item("PrecisVc").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" du Volume PrecisVs de la sonde correspondant à RefEssai
        Public Function GetPrecisVs() As Single
            Dim i As Integer
            GetPrecisVs = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetPrecisVs = .Rows(i).Item("PrecisVs").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" de l'Epaisseur de membrane de la sonde correspondant à RefEssai
        Public Function GetEpMembraneCentre() As Single
            Dim i As Integer
            GetEpMembraneCentre = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetEpMembraneCentre = .Rows(i).Item("EpMembrane").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" de l'Epaisseur de membrane de la sonde correspondant à RefEssai
        Public Function GetPrEpMembraneCentre() As Single
            Dim i As Integer
            GetPrEpMembraneCentre = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetPrEpMembraneCentre = .Rows(i).Item("EpMembrane").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Table "LesEssaisExpansionPropre" du CoefficientA de la sonde correspondant à RefEssai
        Public Function GetCoefficientA() As Single
            Dim i As Integer
            Dim refEssaiCourant As String

            refEssaiCourant = ""
            If MyEssai.MaExpansion IsNot Nothing Then
                If MyEssai.MaExpansion.E_RefEssai IsNot Nothing Then
                    refEssaiCourant = MyEssai.MaExpansion.E_RefEssai.ToString
                Else
                    refEssaiCourant = ""
                End If
            End If
            GetCoefficientA = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        refEssaiCourant = .Rows(i).Item("refEssai").ToString
                        If E_RefEssai = refEssaiCourant Then
                            GetCoefficientA = .Rows(i).Item("coefficientA").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Table "LesEssaisExpansionPropre" du CoefficientB de la sonde correspondant à RefEssai
        Public Function GetCoefficientB() As Single
            Dim i As Integer
            Dim refEssaiCourant As String

            refEssaiCourant = ""
            If MyEssai.MaExpansion IsNot Nothing Then
                If MyEssai.MaExpansion.E_RefEssai IsNot Nothing Then
                    refEssaiCourant = MyEssai.MaExpansion.E_RefEssai.ToString
                End If
            End If
            GetCoefficientB = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        refEssaiCourant = .Rows(i).Item("refEssai").ToString
                        If E_RefEssai = refEssaiCourant Then
                            GetCoefficientB = .Rows(i).Item("coefficientB").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        ' Recherche dans la Tbale "LesEssaisExpansionPropre" du PrecisA de la sonde correspondant à RefEssai
        Public Function GetPrecisA() As Single
            Dim i As Integer
            GetPrecisA = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetPrecisA = .Rows(i).Item("PrecisA").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function



        ' Recherche dans la Tbale "LesEssaisExpansionPropre" du PrecisA de la sonde correspondant à RefEssai
        Public Function GetMesures() As Single
            Dim i, j, k As Integer
            Dim pression, volume As Single
            GetMesures = 0
            j = 0
            With MaListeSondes.Tables("LesMesuresExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If E_RefEssai = .Rows(i).Item("refEssai").ToString Then
                            pression = Val(.Rows(i).Item("Pression"))
                            volume = Val(.Rows(i).Item("Volume"))
                            MyExpansion.Mesures_Expansion_Expansion(j).pression = pression
                            MyExpansion.Mesures_Expansion_Expansion(1, j) = volume
                            k = Val(.Rows(i).Item("indiceMesure"))
                            ' MyExpansion.Mesures_Expansion_Expansion(2, j) = k
                            j = j + 1
                            GetMesures = j
                            MesureCount = j
                            ' Exit For
                        End If
                    End If
                Next i

                '' Recherche de l'indice Minimum du tableau Mesures(2,j-1)
                'iMin = 100
                'For i = 0 To j - 1
                '    If  MyExpansion.Mesures_Expansion(2, i) < iMin Then iMin =  MyExpansion.Mesures_Expansion(2, i)
                'Next i

                '' Reclassement des valeurs du tableau Mesures(2,j-1) par ordre d'indice
                'k = 0
                'For i = 0 To j - 1
                '    If Mesures(2, i) = iMin Then
                '        Accu = Mesures(0, k)
                '        Mesures(0, k) = Mesures(0, iMin)
                '        Mesures(0, iMin) = Accu
                '        Accu = Mesures(1, k)
                '        Mesures(1, k) = Mesures(1, iMin)
                '        Mesures(1, iMin) = Accu
                '        Accu = Mesures(2, k)
                '        Mesures(2, k) = Mesures(2, iMin)
                '        Mesures(2, iMin) = Accu
                '        k = k + 1
                '        iMin = iMin + 1
                '    End If
                'Next i
            End With
        End Function

        ' Recherche dans la Table "LesEssaisExpansionPropre" du PrecisB de la sonde correspondant à RefEssai
        Public Function GetPrecisB() As Single
            Dim i As Integer
            GetPrecisB = 0
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).RowState <> DataRowState.Deleted Then
                        If RefEssai = .Rows(i).Item("refEssai").ToString Then
                            GetPrecisB = .Rows(i).Item("PrecisB").ToString
                            Exit For
                        End If
                    End If
                Next i
            End With
        End Function

        Public Function GetMesureCount() As Integer
            GetMesureCount = MesureCount
        End Function

        Public Sub SetRefEssai(ByVal refEssa As String)
            RefEssai = refEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetRefSonde(ByVal refSond As String)
            RefSonde = refSond
            Call IHaveBeenModified()
        End Sub

        Public Sub SetClassPrecis(ByVal Precis As Integer)
            ClassPrecis = Precis
            Call IHaveBeenModified()
        End Sub

        Public Sub SetNomEssai(ByVal nomEssa As String)
            nomEssai = nomEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDateEssai(ByVal dateEssa As String)
            dateEssai = dateEssa
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDifPresEauAir(ByVal difPresEauAi As Single)
            DifPresEauAir = difPresEauAi
            Call IHaveBeenModified()
        End Sub

        Public Sub SetHauteurCpv(ByVal hauteurCp As Single)
            HauteurCpv = hauteurCp
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMesureCount(ByVal Count As Integer)
            MesureCount = Count
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDi(ByVal D As Single)
            Di = D
            Call IHaveBeenModified()
        End Sub

        Public Sub SetVc(ByVal V As Single)
            Vc = Math.Round(V, 1)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrecisVc(ByVal V As Single)
            PrecisVc = Math.Round(V, 1)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetVs(ByVal V As Single)
            Vs = Math.Round(V, 1)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetEpMembraneCentre(ByVal Ep As Single)
            EpMembrane = Math.Round(Ep, 3)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrEpMembraneCentre(ByVal PrEp As Single)
            EpMembrane = Math.Round(PrEp, 3)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrecisVs(ByVal V As Single)
            PrecisVs = Math.Round(V, 1)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetCoefficientA(ByVal coefficient As Single)
            coefficientA = Math.Round(coefficient, 7)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetCoefficientB(ByVal coefficient As Single)
            coefficientB = Math.Round(coefficient, 9)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrecisA(ByVal V As Single)
            PrecisA = Math.Round(V, 8)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrecisB(ByVal V As Single)
            PrecisB = Math.Round(V, 10)
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMySonde(ByRef sonde As Probe)
            If MySonde IsNot Nothing Then MySonde = New Probe
            MySonde = sonde
            RefSonde = MySonde.GetRefSonde
        End Sub

        Public Sub NouvelEssai(ByVal Table As Boolean)
            Dim ObjetDataRow As DataRow

            RefEssai = ""
            RefSonde = ""
            nomEssai = "à définir"
            dateEssai = DateValue(Now)
            DifPresEauAir = 0
            HauteurCpv = 0
            Di = 0
            Vc = 0
            coefficientA = 0
            coefficientB = 0
            MyExpansionNouvelleMesure(0, 0, 0)

            If Table Then ' On ajoute un essai à MonFichier.Tables("LesEssaisExpansionPropre") seulement si Table est vrai
                If MonChantier.Tables("LesEssaisExpansionPropre") Is Nothing Then
                    MonChantier.Tables.Add("LesEssaisExpansionPropre")
                    With MonChantier.Tables("LesEssaisExpansionPropre")
                        .Columns.Add("refEssai", GetType(String))
                        .Columns.Add("refSonde", GetType(String))
                        .Columns.Add("nomEssai", GetType(String))
                        .Columns.Add("dateEssai", GetType(String))
                        .Columns.Add("difPresEauAir", GetType(String))
                        .Columns.Add("hauteurCpv", GetType(String))
                        .Columns.Add("Di", GetType(String))
                        .Columns.Add("Vc", GetType(String))
                        .Columns.Add("Vs", GetType(String))
                        .Columns.Add("EpMembrane", GetType(String))
                        .Columns.Add("PrP", GetType(String))
                        .Columns.Add("PrV", GetType(String))
                        .Columns.Add("coefficientA", GetType(String))
                        .Columns.Add("coefficientB", GetType(String))
                        .Columns.Add("PrecisVc", GetType(String))
                        .Columns.Add("PrecisVs", GetType(String))
                        .Columns.Add("PrEpMembrane", GetType(String))
                        .Columns.Add("PrecisA", GetType(String))
                        .Columns.Add("PrecisB", GetType(String))
                    End With
                Else
                    ' Ajout d'une ligne à la Table "LesEssaisExpansionPropre"
                    ObjetDataRow = MaListeSondes.Tables("LesEssaisExpansionPropre").NewRow()

                    ObjetDataRow("refEssai") = MySonde.GiveMeANewRefEssai(EssaiCalib.Expansion)
                    ObjetDataRow("refSonde") = MySonde.refSonde
                    MyExpansion.RefEssai = ObjetDataRow("refEssai").ToString

                    MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Add(ObjetDataRow)

                    'MonChantier.Tables("LesEssaisForage").Rows.Add()
                End If
            End If

        End Sub

        'Public Sub AfficheMesuresDansTab(ByRef Table As DataGridView, ByVal RefEssaiExpansion As String)
        '    Dim i As Integer
        '    Dim pression, volume As Single

        '    ' Remplissage du Tableau

        '    Table.Rows.Clear()
        '    If MesureCount = 0 Then
        '        ' Création du nouveau tableau si mesurecount est nul
        '        Me.MyExpansionNouvelleMesure(0, 0, 0)
        '    End If
        '    For i = 0 To MesureCount - 1
        '        With Table
        '            .Rows.Add()
        '            pression = Mesures(0, i)
        '            volume = Mesures(1, i)
        '            'Me.MyResistanceNouvelleMesure(i, pression, volume)
        '            .Rows(i).Cells(0).Value = pression
        '            .Rows(i).Cells(1).Value = volume
        '        End With
        '    Next
        'End Sub

        Public Sub AfficheMesuresDepuisTab(ByRef Table As DataGridView)

            ' Remplissage du Tableau
            Dim i, NbRow, k As Integer

            NbRow = Table.Rows.Count
            'ReDim Preserve Mesures(1, NbRow - 1)
            MesureCount = NbRow - 1
            ' Remplissage du Tableau
            For i = 0 To NbRow - 1
                With Table
                    Mesures_Expansion(0, i) = .Rows(i).Cells(0).Value
                    Mesures_Expansion(1, i) = .Rows(i).Cells(1).Value
                    k = .Rows(i).Cells(1).Value

                End With
            Next
        End Sub

        'Public Sub RemplirMesures(ByRef rsMesures As ADODB.Recordset)
        '    ' Création des mesures
        '    MesureCount = 0
        '    With rsMesures
        '        If Not .EOF Then .MoveFirst()
        '        Do While Not .EOF
        '            Call NouvelleMesure(
        '                .Fields("pression").Value, .Fields("volume").Value)
        '            .MoveNext()
        '        Loop
        '    End With

        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetNouvelleMesureExpansion(ByVal Ligne As Integer, ByVal colonne As Integer, ByVal Valeur As Single)
        '
        '    If Ligne > MesureCount Then ReDim Preserve Mesures(15, Ligne)
        '    Mesures(colonne, Ligne) = Valeur
        '    Call IHaveBeenModified
        '
        'End Sub
        ' *************************************************************************************************************************
        ' Ajour d'une nouvelle mesure à la Table "Mesures" de MyExpansion
        ' *************************************************************************************************************************
        Public Sub MyExpansionNouvelleMesure(ByVal indice As Integer, ByVal pression As Single, ByVal Volume As Single)
            Dim NewLine As Boolean

            NewLine = False
            ' Incrémentation du compteur de mesures
            If indice > MesureCount - 1 Then
                ' Nouvelle ligne pour la Table
                MesureCount = MesureCount + 1
                NewLine = True
            End If


            ' Redimensionnement du tableau dynamique de Essais
            'ReDim Preserve Mesures(1, MesureCount)

            Mesures_Expansion(0, indice) = pression
            Mesures_Expansion(1, indice) = Volume
            'Mesures_Expansion(2, indice) = indice

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            'Call IHaveBeenModified
        End Sub

        Public Sub DessineCourbe(ByRef graphe As WinFormsChart, ByVal Dilatap As Boolean)
            Dim Ep0, Re, Ri0, Vi, Ri, Ep As Single
            Dim MesureCount As Integer

            MesureCount = Mesures_Expansion.Count
            If MesureCount > 0 Then
                Dim row, column As Integer
                Dim Max, X2 As Single
                Dim X(MesureCount - 1)
                Dim Y(MesureCount - 1)
                Dim X1(MesureCount - 1) As Single
                Dim Y1(MesureCount - 1) As Single
                MyEssai = MyTest
                ' Calcul de l'épaisseur initiale des membranes et lanterne
                If Not (Dilatap) Then
                    If MySonde.P_typeSonde = 0 Or MySonde.P_typeSonde = 1 Then  ' Cas des sondes G et E
                        Ep0 = MySonde.P_EpMembraneMesure + MySonde.P_EpMembraneGarde
                        Re = Math.Sqrt(MySonde.P_Vs / (Pi * MySonde.P_ls / 10))
                        Ri0 = Re - Ep0 / 10
                    End If

                    If MySonde.P_typeSonde = 2 Or MySonde.P_typeSonde = 3 Then ' Cas des Tubes fendus Court et Long
                        Ep0 = MySonde.P_EpMembraneMesure + MySonde.P_EpMembraneGarde + MySonde.P_epTubeFendu
                        Re = Sqrt(MySonde.P_Vs / (Pi * MySonde.P_ls / 10))
                        Ri0 = Re - Ep0 / 10
                    End If
                    ' MySonde.EpMembrane = Ep0
                End If

                If Dilatap Then
                    For i = 0 To MesureCount - 1
                        X(i) = Mesures_Expansion(0, i)
                        Y(i) = Mesures_Expansion(1, i)
                    Next
                    'Else
                    '    For i = 0 To MesureCount - 1
                    '        X(i) = Mesures_Expansion(0, i)
                    '        If i = 0 Then Y(i) = Ep0
                    '        Y(i) = Mesures_Expansion(1, i)
                    '    Next
                End If


                With graphe
                    If MesureCount <= 1 Then Exit Sub
                    .Series.Clear()
                    .ChartAreas(0).BackColor = System.Drawing.Color.Transparent

                    Select Case Lang
                        Case 0
                            .Series.Add("Test " & nomEssai)
                            '.Series(0).ChartType = Excel_Chart.Charting.SeriesChartType.Point
                            .Series(0).ChartType = SeriesChartType.Point
                            .Series(0).Color = System.Drawing.Color.Blue
                            .Series.Add("Self Expansion " & MyEssai.E_nomEssai)
                            '.Series(1).ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                            .Series(1).ChartType = SeriesChartType.Line
                            .Series(1).Color = System.Drawing.Color.Red
                        Case 1
                            .Series.Add("Essai " & nomEssai)
                            .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                            .Series(0).Color = System.Drawing.Color.Blue
                            .Series.Add("Expansion propre " & MyEssai.E_nomEssai)
                            .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line
                            .Series(1).Color = System.Drawing.Color.Red
                    End Select

                    If Not (Dilatap) Then
                        For column = 1 To 2
                            'X(0) = 0
                            'Y(0) = 0
                            For row = 0 To MesureCount - 1
                                Select Case column
                                    Case 1
                                        X(row) = Mesures_Expansion(column - 1, row)
                                    Case 2
                                        Y(row) = Mesures_Expansion(column - 1, row)
                                        If Not (Dilatap) Then
                                            Vi = Mesures_Expansion(column - 1, row)      ' Volume
                                            If MySonde.typeSonde = 0 Or MySonde.typeSonde = 1 Then   ' Cas des sondes G et E
                                                Vi = Pi * Ri0 ^ 2 * MySonde.ls / 10 + Mesures_Expansion(column - 1, row)
                                                Ri = Sqrt(Vi / (Pi * MySonde.ls / 10))
                                                Ep = MyExpansion.Di / 20 - Ri
                                            End If
                                            If MySonde.typeSonde = 2 Or MySonde.typeSonde = 3 Then  ' Cas des Tubes fendus Court et Long
                                                Vi = Pi * Ri0 ^ 2 * MySonde.ls / 10 + Mesures_Expansion(column - 1, row)
                                                Ri = Sqrt(Vi / (Pi * MySonde.ls / 10))
                                                Ep = MyExpansion.Di / 20 - Ri
                                            End If

                                            Y(row) = Ep
                                        End If
                                End Select
                            Next row
                        Next column
                    End If

                    If Dilatap Then
                        If Not (coefficientA = 0 And Vc = 0) Then
                            Max = 0
                            For row = 0 To MesureCount - 1
                                If Mesures_Expansion(0, row) > Max Then Max = Mesures_Expansion(0, row)
                            Next row
                            For row = 0 To MesureCount - 1
                                'X2 = (row - 1) * Max / (MesureCount - 1)
                                X2 = X(row)
                                X1(row) = X2
                                Y1(row) = coefficientA * X2 + Vc
                                'If DilatAp Then Y1(row) = coefficientA * X2 + Vc
                                'If Not (CompMemb) Then Y1(row) = Vc
                            Next row
                        Else
                            Max = Mesures_Expansion(0, MesureCount - 1)
                            For row = 0 To MesureCount - 1
                                ' X2 = (row - 1) * Max / (MesureCount - 1)
                                X2 = X(row)
                                X1(row) = X2
                                Y1(row) = coefficientA * X2 + Vc
                                'If DilatAp Then Y1(row) = coefficientA * X2 + Vc
                                'If CompMemb Then Y1(row) = Vc
                            Next row
                        End If
                    Else
                        If Not (coefficientB = 0 And Vc = 0) Then
                            Max = 0
                            Ep0 = Me.EpMembrane / 10

                            For row = 0 To MesureCount - 1
                                If Mesures_Expansion(0, row) > Max Then Max = Mesures_Expansion(0, row)
                            Next row
                            For row = 0 To MesureCount - 1
                                ' X2 = (row - 1) * Max / (MesureCount - 1)
                                X2 = X(row)
                                X1(row) = X2
                                Y1(row) = coefficientB * X2 + Ep0
                                'If DilatAp Then Y1(row) = coefficientA * X2 + Vc
                                'If Not (CompMemb) Then Y1(row) = Vc
                            Next row
                        Else
                            Max = Mesures_Expansion(0, MesureCount - 1)

                            Ep0 = Me.EpMembrane / 10
                            For row = 1 To MesureCount
                                X2 = (row - 1) * Max / (MesureCount - 1)

                                X1(row) = X2
                                Y1(row) = coefficientA * X2 + Ep0
                                'If DilatAp Then Y1(row) = coefficientA * X2 + Vc
                                'If CompMemb Then Y1(row) = Vc
                            Next row
                        End If
                    End If
                    ' Ajouter les deux séries de points
                    .Series(0).Points.DataBindXY(X, Y)
                    .Series(1).Points.DataBindXY(X1, Y1)

                    ' Définition des axes
                    ' Définition axe des X
                    With .ChartAreas(0).AxisX
                        Select Case Lang
                            Case 0
                                .Title = "Pressure (kPa)"
                            Case 1
                                .Title = "Pression (kPa)"
                        End Select
                        .Minimum = 0
                        'If hy > 1 Then
                        '    .Minimum = 1
                        'Else
                        '    .Minimum = 0.1
                        'End If
                        '.Maximum = hlimx
                        .Minimum = 0
                        '.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
                        .LineWidth = 1
                        .MajorGrid.LineColor = System.Drawing.Color.LightGray
                        .RoundAxisValues()
                    End With

                    ' Définition axe des Y
                    With .ChartAreas(0).AxisY
                        Select Case Lang
                            Case 0
                                If Dilatap Then
                                    .Title = "Volume (cm^3)"
                                Else
                                    .Title = "Membrane Thickness (cm)"
                                End If

                            Case 1
                                If Dilatap Then
                                    .Title = "Volume (cm^3)"
                                Else
                                    .Title = "Epaisseur de membrane (cm)"
                                End If
                        End Select
                        .Minimum = 0
                        '.Maximum = hlimy
                        .LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .LineWidth = 1
                        .MajorGrid.LineColor = System.Drawing.Color.LightGray
                        '.MajorGrid.Interval = 1
                        .RoundAxisValues()
                        .Minimum = 0
                    End With

                End With
                graphe.Visible = True
            Else
                graphe.Visible = False
            End If
        End Sub

        ' *******************************************************************
        ' Calcul des coefficients de l'essai d'expansion
        ' *******************************************************************
        Public Sub CalculeCoefExpansion()
            Dim poly As Polynome
            Dim X() As Single
            Dim Y() As Single
            Dim Yd() As Single
            Dim Wi() As Single
            Dim A0, A1, SigR, Sig2, SX2, Precis, Swi, SwiXi, SwiXi2, SwiYi, SwiXiYi, Delta, Bet0, Bet1, deltaBeta0, deltaBeta1, deltaBetai1 As Single
            Dim Ex_2, Ey_2, SwiEy2, Ewiy2, Stud, XMoy, YMoy, xyMoy, x2Moy As Single
            Dim i, Iter, MesureCount As Integer

            ' Dimensionnement des deux tableaux de Mesures_Expansion à passer à RegPolynome
            MesureCount = Mesures_Expansion.Count
            ReDim X(MesureCount - 1)
            ReDim Y(MesureCount - 1)
            ReDim Yd(MesureCount - 1)
            ReDim Wi(MesureCount - 1)

            ' Remplissage des tableaux avec les Mesures_Expansion
            For i = 0 To MesureCount - 1
                X(i) = Mesures_Expansion(0, i)
                Y(i) = Mesures_Expansion(1, i)
            Next

            poly = RegPolynome(MesureCount, X, Y, 1)
            A0 = poly.A0
            A1 = poly.A1

            Stud = ProbStudent_InverseTab(MesureCount - 2)

            ' ----------------------------------------------------------------
            ' Calcul des termes de l'incertitude sur la regression linéaire
            ' Boucle itérative pour le calcul de la précision pondérée
            ' -----------------------------------------------------------
            For Iter = 0 To 10

                ' Calcul des paramètres l'incertitude sur la régression linéaire
                XMoy = 0
                YMoy = 0
                xyMoy = 0
                x2Moy = 0
                Sig2 = 0
                SwiEy2 = 0
                Swi = 0
                SwiXi = 0
                SwiXi2 = 0
                SwiYi = 0
                SwiXiYi = 0
                For i = 0 To MesureCount - 1
                    Yd(i) = A0 + A1 * X(i)
                    Sig2 = Sig2 + (Y(i) - Yd(i)) ^ 2
                    Wi(i) = 1 / (PrV ^ 2 + (A1 * PrP) ^ 2)
                    SwiEy2 = SwiEy2 + Wi(i) * (Y(i) - Yd(i)) ^ 2
                    Swi = Swi + Wi(i)
                    SwiXi = SwiXi + Wi(i) * X(i)
                    SwiXi2 = SwiXi2 + Wi(i) * X(i) ^ 2
                    SwiYi = SwiYi + Wi(i) * Y(i)
                    SwiXiYi = SwiXiYi + Wi(i) * X(i) * Y(i)
                    XMoy = XMoy + X(i)
                    YMoy = YMoy + Y(i)
                    xyMoy = xyMoy + X(i) * Y(i)
                    x2Moy = x2Moy + X(i) ^ 2
                Next i

                ' Calcul des valeurs moyennes
                If MesureCount > 2 Then
                    SigR = (Sig2 / (MesureCount - 2)) ^ 0.5
                    XMoy = XMoy / (MesureCount)
                    YMoy = YMoy / (MesureCount)
                    xyMoy = xyMoy / (MesureCount)
                    x2Moy = x2Moy / (MesureCount)
                Else
                    SigR = 0
                    If MesureCount = 2 Then
                        XMoy = X(MesureCount - 1) / (MesureCount)
                        XMoy = XMoy / (MesureCount)
                        YMoy = YMoy / (MesureCount)
                        xyMoy = xyMoy / (MesureCount)
                        x2Moy = x2Moy / (MesureCount)
                    Else
                        XMoy = X(MesureCount - 1)
                        YMoy = YMoy / (MesureCount)
                        xyMoy = xyMoy / (MesureCount)
                        x2Moy = x2Moy / (MesureCount)
                    End If
                End If

                Precis = 0
                SX2 = 0
                For i = 0 To MesureCount - 1
                    SX2 = SX2 + (X(i) - XMoy) ^ 2
                Next i

                ' calcul de Deltabetap1
                Delta = Swi * SwiXi2 - SwiXi ^ 2
                deltaBeta1 = DeltaBeta1p(SwiXi, SwiXi2, Swi)
                If Iter > 0 Then
                    deltaBetai1 = deltaBeta1
                End If

                ' Calcul de la précision de la régression par point de mesure
                'For j = 0 To MesureCount - 1
                If Iter = 0 Then
                    'Precis = SigmaVp(MesureCount, Stud, SX2, SwiEy2, XMoy, X(j))
                    'Precis = ProbStudent_InverseTab(MesureCount - 2) * SigR * Precis ^ 0.5

                    ' calcul de DeltaBeta0
                    deltaBeta0 = DeltaBeta0p(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet1 = A1
                    Bet0 = A0
                Else
                    ' calcul de DeltaBeta0
                    deltaBeta0 = DeltaBeta0p(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet1 = Betap1(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet0 = Betap0(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                End If


                ' Mise à jour des poids
                For i = 0 To MesureCount - 1
                    Wi(i) = Poidsi(PrP, PrV, Bet1)
                Next i

                ' Mise à jour de l'Erreur
                Ex_2 = 0
                Ey_2 = 0
                Ewiy2 = 0
                For i = 0 To MesureCount - 1
                    Ex_2 = Ex_2 + (X(i) - XMoy) ^ 2
                    Ey_2 = Ey_2 + (Y(i) - Bet0 - Bet1 * X(i)) ^ 2
                    Ewiy2 = Ewiy2 + Wi(i) * (Y(i) - Bet0 - Bet1 * X(i)) ^ 2
                Next i

                ' Calcul de l'erreur de régression linéaire
                'If Iter = 0 Then
                SigR = SigmaR(MesureCount, Ey_2)
                'Else
                'SigR = SigmaR(MesureCount, Ewiy2)
                'End If

                ' Calcul de l'erreur sur chaque mesure de volume
                ' E2x = (X(i) - XMoy) ^ 2

                '  sigR = SigmaR(N, E2y)
                PrecisVc = SigmaVp(MesureCount, SigR, Stud, Ex_2, Ey_2, XMoy, 0)

                ' Mise à jour de SigmaVp
                PrecisVc = PrecisVc + PrV

                ' Test de fin de boucle
                If (Abs(deltaBeta1 - deltaBetai1) / deltaBetai1 < 0.001) And Iter - 1 > 0 Then
                    Exit For
                End If
                'Next j ' Fin de l'itération sur les mesures 
            Next Iter
            ' -----------------------------------------------------------
            ' Fin des iteérations
            ' ----------------------------------------------------------
            ' Calcul de la précision sur le volume de la sonde Vs
            With MySonde
                Ls = .ls
            End With
            PrecisVs = SigmaVs(Ls, Di, PrLong, PrDiam, PrecisVc)
            PrRegres = SigR

            ' affectation des résultats
            If Not poly.Erreur Then
                'With MyExpansion
                coefficientA = Math.Round(poly.A1, 7)
                Vc = Math.Round(poly.A0, 1)
                MySonde.Vc = Vc
                MyExpansion.Vc = Vc
                MySonde.CalculeVolNF()
                Vs = MySonde.Vs
                PrecisA = SigR / (SX2 ^ 0.5)
                MySonde.PrecisA = PrecisA
                MySonde.refEssai = RefEssai
                MySonde.refSonde = RefSonde
                MySonde.PrP = PrP
                MySonde.PrV = PrV
                MySonde.PrLong = PrLong
                'End With
            End If

            ' Mise à jour de MySonde.Expansion(ind)
            With MySonde
                For i = 0 To .ExpansionCount - 1
                    If .Expansion(i).RefEssai = MyExpansion.RefEssai Then
                        .Expansion(i) = MyExpansion
                    End If
                Next i
            End With

            Call IHaveBeenModified()
        End Sub

        ' *******************************************************************
        ' Calcul des coefficients liés à l'écrasement de la membrane
        ' *******************************************************************
        Public Sub CalculeCoefEcrasement()
            Dim poly As Polynome
            Dim X() As Single
            Dim Y() As Single
            Dim Yd() As Single
            Dim Wi() As Single
            Dim A0, A1, B, Ri0, Ri, Re, Ep0, Ep, SigR, Sig2, SX2, Precis, Swi, SwiXi, SwiXi2, SwiYi, SwiXiYi, Delta, Bet0, Bet1, deltaBeta0, deltaBeta1, deltaBetai1, Vi As Single
            Dim Ex_2, Ey_2, SwiEy2, Ewiy2, Stud, XMoy, YMoy, xyMoy, x2Moy, SigB, PrEp0 As Single
            Dim i, Iter As Integer

            ' Dimensionnement des deux tableaux de mesures à passer à RegPolynome
            ReDim X(MesureCount - 1)
            ReDim Y(MesureCount - 1)
            ReDim Yd(MesureCount - 1)
            ReDim Wi(MesureCount - 1)
            If PrV = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("The precision on the volume is null, prv =" & PrV)
                    Case 1
                        MsgBox("La précision sur le volume est nulle, prv =" & PrV)
                End Select
            End If
            If PrP = 0 Then
                Select Case Lang
                    Case 0
                        MsgBox("The precision on the pressure is null, prp =" & PrP)
                    Case 1
                        MsgBox("La précision sur la pression est nulle, prp =" & PrP)
                End Select
            End If
            If MySonde.typeSonde = 0 Or MySonde.typeSonde = 1 Then  ' Cas des sondes G et E
                Ep0 = MySonde.EpMembraneMesure + MySonde.EpMembraneGarde
                Re = Sqrt(MySonde.Vs / (Pi * MySonde.ls / 10))
                Ri0 = Re - Ep0 / 10
            End If

            If MySonde.typeSonde = 2 Or MySonde.typeSonde = 3 Then ' Cas des Tubes fendus Court et Long
                Ep0 = MySonde.EpMembraneMesure + MySonde.EpMembraneGarde + MySonde.epTubeFendu
                Re = Sqrt(MySonde.Vs / (Pi * MySonde.ls / 10))
                Ri0 = Re - Ep0 / 10
            End If


            ' Remplissage des tableaux avec les mesures
            For i = 0 To MesureCount - 1
                X(i) = Mesures(0, i)    ' Pression
                Vi = Mesures(1, i)      ' Volume
                If MySonde.typeSonde = 0 Or MySonde.typeSonde = 1 Then   ' Cas des sondes G et E
                    ' Vi est le Volume initial INERTE de la sonde
                    Vi = Pi * Ri0 ^ 2 * MySonde.ls / 10 + Vi
                    ' Ri est la rayon interne initial de la sonde
                    Ri = Sqrt(Vi / (Pi * MySonde.ls / 10))
                    ' Ep est la différence entre le rayon interne du Tubede calibration
                    ' et le rayon externe de la sonde
                    Ep = MyExpansion.Di / 20 - Ri
                    With MySonde
                        Select Case .typeSonde
                            Case 1 And 2
                                If Ep < 0 Then
                                    Select Case Lang
                                        Case 0
                                            MsgBox("Calculation Error : The thickness of the membrane is megative - ep = " & Ep)
                                            Exit Sub
                                        Case 1
                                            MsgBox("Erreur de calcul : l'épaisseur de membrane est négative - ep = " & Ep)
                                            Exit Sub
                                    End Select
                                End If

                            Case 3 And 4
                                If Ep < .epTubeFendu Then
                                    Select Case Lang
                                        Case 0
                                            MsgBox("Calculation Error : The thickness of the membrane is smaller than the slotted tube - ep = " & Ep)
                                            Exit Sub
                                        Case 1
                                            MsgBox("Erreur de calcul : l'épaisseur de membrane est inférieure à celle du Tubefendu - ep = " & Ep)
                                            Exit Sub
                                    End Select
                                End If
                        End Select
                    End With
                End If
                If MySonde.typeSonde = 2 Or MySonde.typeSonde = 3 Then  ' Cas des Tubes fendus Court et Long
                    Vi = Pi * Ri0 ^ 2 * MySonde.ls / 10 + Vi
                    Ri = Sqrt(Vi / (Pi * MySonde.ls / 10))
                    Ep = MyExpansion.Di / 20 - Ri
                End If

                Y(i) = Ep
            Next

            poly = RegPolynome(MesureCount, X, Y, 1)
            A0 = poly.A0
            MySonde.EpInitMembrane = A0 * 10
            'MySonde.EpInitMembrane = A0 * 10
            A1 = poly.A1
            B = poly.A1
            MySonde.B = B
            MyExpansion.coefficientB = B
            MyExpansion.EpMembrane = A0 * 10

            Stud = ProbStudent_InverseTab(MesureCount - 2)

            ' ----------------------------------------------------------------
            ' Calcul des termes de l'incertitude sur la regression linéaire
            ' Boucle itérative pour le calcul de la précision pondérée
            ' -----------------------------------------------------------
            For Iter = 0 To 10

                ' Calcul des paramètres l'incertitude sur la régression linéaire
                XMoy = 0
                YMoy = 0
                xyMoy = 0
                x2Moy = 0
                Sig2 = 0
                SwiEy2 = 0
                Swi = 0
                SwiXi = 0
                SwiXi2 = 0
                SwiYi = 0
                SwiXiYi = 0
                For i = 0 To MesureCount - 1
                    Yd(i) = A0 + A1 * X(i)
                    Sig2 = Sig2 + (Y(i) - Yd(i)) ^ 2
                    Wi(i) = 1 / (PrV ^ 2 + (A1 * PrP) ^ 2)
                    SwiEy2 = SwiEy2 + Wi(i) * (Y(i) - Yd(i)) ^ 2
                    Swi = Swi + Wi(i)
                    SwiXi = SwiXi + Wi(i) * X(i)
                    SwiXi2 = SwiXi2 + Wi(i) * X(i) ^ 2
                    SwiYi = SwiYi + Wi(i) * Y(i)
                    SwiXiYi = SwiXiYi + Wi(i) * X(i) * Y(i)
                    XMoy = XMoy + X(i)
                    YMoy = YMoy + Y(i)
                    xyMoy = xyMoy + X(i) * Y(i)
                    x2Moy = x2Moy + X(i) ^ 2
                Next i

                ' Calcul des valeurs moyennes
                If MesureCount <= 1 Then
                    Exit Sub
                End If
                If MesureCount > 2 Then
                    SigR = (Sig2 / (MesureCount - 2)) ^ 0.5
                    XMoy = XMoy / (MesureCount)
                    YMoy = YMoy / (MesureCount)
                    xyMoy = xyMoy / (MesureCount)
                    x2Moy = x2Moy / (MesureCount)
                Else
                    SigR = 0
                    If MesureCount = 2 Then
                        XMoy = X(MesureCount - 1) / (MesureCount)
                        XMoy = XMoy / (MesureCount)
                        YMoy = YMoy / (MesureCount)
                        xyMoy = xyMoy / (MesureCount)
                        x2Moy = x2Moy / (MesureCount)
                    Else
                        XMoy = X(MesureCount - 1)
                        YMoy = YMoy / (MesureCount)
                        xyMoy = xyMoy / (MesureCount)
                        x2Moy = x2Moy / (MesureCount)
                    End If
                End If

                Precis = 0
                SX2 = 0
                For i = 0 To MesureCount - 1
                    SX2 = SX2 + (X(i) - XMoy) ^ 2
                Next i

                ' calcul de Deltabetap1
                Delta = Swi * SwiXi2 - SwiXi ^ 2
                deltaBeta1 = DeltaBeta1p(SwiXi, SwiXi2, Swi)
                If Iter > 0 Then
                    deltaBetai1 = deltaBeta1
                End If

                ' Calcul de la précision de la régression par point de mesure
                'For j = 0 To MesureCount - 1
                If Iter = 0 Then
                    'Precis = SigmaVp(MesureCount, Stud, SX2, SwiEy2, XMoy, X(j))
                    'Precis = ProbStudent_InverseTab(MesureCount - 2) * SigR * Precis ^ 0.5

                    ' calcul de DeltaBeta0
                    deltaBeta0 = DeltaBeta0p(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet1 = A1
                    Bet0 = A0
                Else
                    ' calcul de DeltaBeta0
                    deltaBeta0 = DeltaBeta0p(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet1 = Betap1(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                    Bet0 = Betap0(SwiXi, SwiYi, SwiXiYi, SwiXi2, Swi)
                End If


                ' Mise à jour des poids
                For i = 0 To MesureCount - 1
                    Wi(i) = Poidsi(PrP, PrV, Bet1)
                Next i

                ' Mise à jour de l'Erreur
                Ex_2 = 0
                Ey_2 = 0
                Ewiy2 = 0
                For i = 0 To MesureCount - 1
                    Ex_2 = Ex_2 + (X(i) - XMoy) ^ 2
                    Ey_2 = Ey_2 + (Y(i) - Bet0 - Bet1 * X(i)) ^ 2
                    Ewiy2 = Ewiy2 + Wi(i) * (Y(i) - Bet0 - Bet1 * X(i)) ^ 2
                Next i

                ' Calcul de l'erreur de régression linéaire
                'If Iter = 0 Then
                SigR = SigmaR(MesureCount, Ey_2)

                SigB = SigR / Sqrt(Ex_2)
                'Else
                'SigR = SigmaR(MesureCount, Ewiy2)
                'End If

                ' Calcul de l'erreur sur chaque mesure de volume
                'E2x = (X(i) - XMoy) ^ 2

                '  sigR = SigmaR(N, E2y)
                PrEp0 = SigmaVp(MesureCount, SigR, Stud, Ex_2, Ey_2, XMoy, 0)

                ' Mise à jour de SigmaVp
                ' PrecisVc = PrecisVc + PrVol

                ' Test de fin de boucle
                If (Abs(deltaBeta1 - deltaBetai1) / deltaBetai1 < 0.001) And Iter - 1 > 0 Then
                    Exit For
                End If
                'Next j ' Fin de l'itération sur les mesures 
            Next Iter
            ' -----------------------------------------------------------
            ' Fin des iteérations
            ' ===========================================================
            ' Calcul de la précision sur le volume de la sonde Vs
            ' -----------------------------------------------------------
            With MySonde
                Ls = .ls
            End With
            PrecisVs = SigmaVs(Ls, Di, PrLong, PrDiam, PrecisVc)
            PrRegres = SigR
            PrEp0 = SigR

            ' affectation des résultats
            If Not poly.Erreur Then
                With MyExpansion
                    .coefficientB = Math.Round(poly.A1, 9)
                    MySonde.B = Math.Round(poly.A1, 9)
                    .EpMembrane = Math.Round(poly.A0, 9) * 10
                    .PrecisB = SigB
                    '.EpMembrane = PrEp0 * 10  ' conversion de l'épaisseur en mm
                    MySonde.PrecisB = SigB
                    MySonde.EpInitMembrane = Math.Round(poly.A0, 9) * 10
                    MySonde.PrEpInitMembrane = PrEp0 * 10   ' conversion de l'épaisseur en mm

                    If MySonde.typeSonde = 0 Or MySonde.typeSonde = 1 Then
                        ' Cas de la sonde nue - on considère que seul la membrane de la cellule de garde s'amincit
                        MySonde.EpInitMembrane = Math.Round(poly.A0, 9) * 10
                        MySonde.PrEpInitMembrane = PrEp0 * 10   ' conversion de l'épaisseur en mm
                        MyExpansion.EpMembrane = MySonde.EpInitMembrane
                        MyExpansion.PrEpMembrane = MySonde.PrEpInitMembrane
                    Else
                        ' Cas du Tube fendu 
                        MySonde.EpInitMembrane = Math.Round(poly.A0, 9) * 10
                        MySonde.PrEpMembraneMesure = PrEp0 * 10   ' conversion de l'épaisseur en mm
                        MyExpansion.EpMembrane = MySonde.EpInitMembrane
                        MyExpansion.PrEpMembrane = MySonde.PrEpInitMembrane
                    End If
                End With

                ' Mise à jour de MySonde.Expansion(ind)
                With MySonde
                    For i = 0 To .ExpansionCount - 1
                        If .Expansion(i).RefEssai = MyExpansion.RefEssai Then
                            .Expansion(i) = MyExpansion
                            Exit For
                        End If
                    Next i

                End With
            End If

            Call IHaveBeenModified()
        End Sub

        Function SigmaVs(ByVal Ls As Single, ByVal Di As Single, ByVal SigLs As Single, ByVal SigDi As Single, ByVal SigVp As Single) As Single
            ' Student = system.windows.forms.application.WorksheetFunction.Loi.Student.Inverse(0.341, N)
            ' Cas d'une nouvelle mesure de Vp
            SigmaVs = Sqrt(0.0625 * Pi ^ 2 * (Di / 10) ^ 4 * (SigLs / 10) ^ 2 + 0.25 * Pi ^ 2 * (Ls / 10) ^ 2 * (Di / 10) ^ 2 * (SigDi / 10) ^ 2) + SigVp
            '    SigmaVp = Student * Sr * Sqr(1 / N + (X - xMoy) ^ 2 / E2x)
        End Function

        ' Fonction d'erreur sur la régression linéaire
        Function SigmaR(ByVal N As Integer, ByVal E2 As Single) As Single
            If N > 2 Then
                SigmaR = (E2 / (N - 2)) ^ 0.5
            Else
                SigmaR = 10 ^ 10
            End If
        End Function

        ' Calcul de l'erreur sur Vp en fonction de l'erreur de régression linéaire 
        Function SigmaVp(ByVal N As Integer, ByVal Sr As Single, ByVal Student As Single, ByVal E2x As Single, ByVal E2y As Single, ByVal xMoy As Single, ByVal X As Single) As Single
            ' Student = system.windows.forms.application.WorksheetFunction.Loi.Student.Inverse(0.341, N)
            ' Cas d'une nouvelle mesure de Vp
            SigmaVp = Student * Sr * Sqrt(1 + 1 / N + (X - xMoy) ^ 2 / E2x)
            '    SigmaVp = Student * Sr * Sqr(1 / N + (X - xMoy) ^ 2 / E2x)
        End Function


        ' Fonction ordonnée origine Beta0 de la régression linéaire
        Function Poidsi(ByVal Deltax As Single, ByVal Deltay As Single, ByVal Beta1 As Single) As Single
            Poidsi = 1 / (Deltay ^ 2 + (Beta1 * Deltax) ^ 2)
        End Function

        ' Fonction de régression linéaire pondérée, calul de l'ordonnée à l'origine; eq. 27, p.14, Fiche 1.1, Rapport Arscop Livrable V9
        ' Fonction ordonnée origine Beta0 de la régression linéaire
        Function Betap0(ByVal Swx As Single, ByVal Swy As Single, ByVal Swxy As Single, ByVal Swx2 As Single, ByVal Sw As Single) As Single
            Dim Delta As Single
            Delta = Sw * Swx2 - Swx ^ 2
            Betap0 = (Swy * Swx2 - Swx * Swxy) / Delta
        End Function

        ' Fonction pente Beta1 de la régression linéaire; eq. 28, p.14, Fiche 1.1, Rapport Arscop Livrable V9
        Function Betap1(ByVal Swx As Single, ByVal Swy As Single, ByVal Swxy As Single, ByVal Swx2 As Single, ByVal Sw As Single) As Single
            Dim Delta As Single
            Delta = Sw * Swx2 - Swx ^ 2
            Betap1 = (Sw * Swxy - Swx * Swy) / Delta
        End Function

        ' Fonction variation de pente Beta1 de la régression linéaire
        Function DeltaBeta1p(ByVal Swx As Single, ByVal Swx2 As Single, ByVal Sw As Single) As Single
            Dim Delta As Single
            Delta = Sw * Swx2 - Swx ^ 2
            DeltaBeta1p = (Sw / Delta) ^ 0.5
        End Function


        ' Fonction ordonnée origine Beta0 de la régression linéaire
        Function DeltaBeta0p(ByVal Swx As Single, ByVal Swy As Single, ByVal Swxy As Single, ByVal Swx2 As Single, ByVal Sw As Single) As Single
            Dim Delta As Single
            Delta = Sw * Swx2 - Swx ^ 2
            DeltaBeta0p = Sqrt(Swx2 / Delta)
        End Function

        Public Function GetMesures(ByVal Ligne As Integer, ByVal colonne As Integer) As Single
            GetMesures = Mesures(Ligne, colonne)
        End Function

        Public Sub ChargeMesures(ByVal ReferenceEssai As String)
            Dim j, k As Integer
            Dim Pression, Volume As Single
            Dim champ As String
            'Analyse du DataSet; j est l'indice de mesures
            MesureCount = 0
            k = 0
            With MaListeSondes.Tables("LesMesuresExpansionPropre")
                For j = 0 To .Rows.Count - 1
                    If .Rows(j).RowState <> DataRowState.Deleted Then
                        If ReferenceEssai = .Rows(j).Item("RefEssai").ToString Then
                            'And MesureCount = Valeurs(MaListeSondes.Tables("LesMesuresExpansionPropre").Rows(j).Item("indiceMesure").ToString) Then
                            ' Nouvel essai

                            ' Chargement des attributs de l'objet depuis la base de données
                            champ = .Rows(j).Item("pression").ToString
                            Pression = Valeurs(champ)
                            champ = .Rows(j).Item("volume").ToString
                            Volume = Valeurs(champ)

                            ' Création et enregistrement des mesures

                            Call MyExpansionNouvelleMesure(k, Math.Round(Pression, 1), Math.Round(Volume, 1))
                            k = k + 1
                        End If
                    End If
                Next j
            End With
        End Sub


        Public Function EnregistreEssai(ByVal RefEssai As String, ByVal CountEssai As Integer) As Boolean
            Dim i, index As Integer
            Dim Requete As String
            'Dim Commande As OleDb.OleDbCommand

            ' On crée une commande Builder pour renvoyer les données à la BD
            Dim CmdSondeBuild As OleDbCommandBuilder

            On Error GoTo errFind

            ' Mise à jour du DataSet MaListeSondes
            With MaListeSondes.Tables("LesMesuresExpansionPropre")

                ' Recherche du rang des mesures de résistance dans la Table "LesMesuresExpansionPropre"
                For index = 0 To .Rows.Count - 1
                    If .Rows(index).Item("refEssai") = RefEssai Then Exit For
                Next index

                ' Rangement des mesures dans la table "LesMesuresExpansionPropre"
                For i = 0 To CountEssai - 1
                    .Rows(index + i).Item("refEssai") = Me.RefEssai
                    .Rows(index + i).Item("indiceMesure") = i
                    .Rows(index + i).Item("pression") = Mesures(0, i)
                    .Rows(index + i).Item("volume") = Mesures(1, i)
                Next (i)

            End With

            ' *********************************************************************
            ' Modification de la Table "LesMesuresExpansionPropre"
            ' *********************************************************************
            'ma requete
            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresExpansionPropre "

            ' Définition de la Commande
            Commande = New OleDb.OleDbCommand(Requete, ConnSonde)

            ' Définition de la Commande de l'AdaptateurSondes
            AdaptateurSondes.SelectCommand = Commande

            '' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes
            'AdaptateurSondes.Fill(MaListeSondes, "LesEssaisExpansionPropre")

            ' Création de l'objetCommand nécessaire à l'enregistrement
            CmdSondeBuild = New OleDb.OleDbCommandBuilder(AdaptateurSondes)

            ' Sélectionne la Table "LesSondes"
            i = MaListeSondes.Tables("LesMesuresExpansionPropre").Select.Length

            '' Effacement des commandes précédentes
            'If AdaptateurSondes.SelectCommand IsNot Nothing Then
            '    AdaptateurSondes.DeleteCommand = CmdSondeBuild.GetDeleteCommand()
            'End If

            ' Mise à jour de la Base de données Access 
            AdaptateurSondes.Update(MaListeSondes, "LesMesuresExpansionPropre")

            EnregistreEssai = True
            Exit Function
errFind:
            EnregistreEssai = False
            MsgBox(Err.Description)
        End Function


        Private Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                Call MySonde.IHaveBeenModified()
                ' RaiseEvent HasBeenModified
            End If
        End Sub

        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        Public Sub AddMesures(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouterPublic Sub AddMesures() ' i est le nombre de mesures à ajouter
            Dim i As Integer

            ' Ajout d'une ligne dans le tableau Mesures de MyExpansion
            MesureCount = MesureCount + 1
            ReDim Preserve Mesures(1, MesureCount)

            ' Ajout d'une ligne dans le tableau "LesMesuresExpansionPropre" de MaListeSondes
            With MaListeSondes.Tables("LesMesuresExpansionPropre")
                .Rows.Add(.Rows.Count - 1, IndMesure, RefEssai, Pression, Volume)
                ' MyExpansion.AddMesures(Ref, IndMesure, Pression, Volume)
            End With

            ' Déplacement des mesures dans le tableau Mesures de MyExpansion 
            For i = MesureCount - 1 To IndMesure + 1 Step -1
                Mesures(0, i) = Mesures(0, i - 1)
                Mesures(1, i) = Mesures(1, i - 1)
            Next i

            Call IHaveBeenModified()
        End Sub

        Public Sub AddMesuresExpansion(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouterPublic Sub AddMesures() ' i est le nombre de mesures à ajouter


            ' Ajout d'une ligne dans le tableau Mesures de MyExpansion
            MesureCount = MesureCount + 1
            ReDim Preserve Mesures(1, MesureCount)

            Mesures(0, MesureCount - 1) = Pression
            Mesures(1, MesureCount - 1) = Volume
            'Mesures(2, MesureCount - 1) = IndMesure

            Call IHaveBeenModified()
        End Sub

        Public Sub SupprimeMesures(ByVal Ref As String, ByVal IndMesure As Integer, ByVal Pression As Single, ByVal Volume As Single) ' i est le nombre de mesures à ajouterPublic Sub AddMesures() ' i est le nombre de mesures à ajouter
            Dim i As Integer

            ' Déplacement des mesures dans le tableau Mesures de MyExpansion 
            For i = IndMesure To MesureCount - 2
                Mesures(0, i) = Mesures(0, i + 1)
                Mesures(1, i) = Mesures(1, i + 1)
                ''Mesures(2, i) = Mesures(2, i + 1)
            Next i

            ' Suppression d'une ligne dans le tableau Mesures de MyExpansion
            If MesureCount > 0 Then MesureCount = MesureCount - 1
            ReDim Preserve Mesures(1, MesureCount - 1)

            ' Suppression d'une ligne dans le tableau "LesMesuresExpansionPropre" de MaListeSondes
            'With MaListeSondes.Tables(4)
            '    .Rows.RemoveAt(IndMesure)
            'End With

            Call IHaveBeenModified()
        End Sub
    End Class
End Namespace