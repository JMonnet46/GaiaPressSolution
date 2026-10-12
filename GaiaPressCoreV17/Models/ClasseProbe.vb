Imports System.ComponentModel.DataAnnotations
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports GaiaPressCoreV17.Services
Imports GaiaPressSolution.GaiaPressCoreV17.Services
Imports Microsoft.VisualBasic

Namespace GaiaPressCoreV17.Models
    Public Class Probe

        'Public connSonde As New OleDb.OleDbConnection()
        Public AdaptateurSondes As New OleDb.OleDbDataAdapter

        ' ----------------  Tables  -------------------------- 
        Public Resistance As List(Of EssaiResistance)
        Public Expansion As List(Of EssaiExpansion)

        '  ---------------------------------------------------
        ' ✅ AJOUTER CES PROPRIÉTÉS PUBLIQUES (ne pas les déclarer comme "As Integer")

        <Key>
        Public Property RefSonde As String  ' ✅ Doit être String pour correspondre à l'ID

        Public Property NomSonde As String
        Public ReadOnly Property ResistanceCount As Integer
            Get
                Return If(Resistance Is Nothing, 0, Resistance.Count)
            End Get
        End Property

        Public ReadOnly Property ExpansionCount As Integer
            Get
                Return If(Expansion Is Nothing, 0, Expansion.Count)
            End Get
        End Property

        Public MaExpansion As EssaiExpansion   ' Liste des EssaiExpansion de MySonde
        Public MaResistance As EssaiResistance ' Liste des EssaiResistance de MySonde

        Public RefEssai As String
        Public ClassPrecis As Integer
        Public PrecisVolSonde As Single
        Public TypeSonde As Int32
        Public TypeDeformee As Int32
        Public TypeContrainte As Int32
        Public Ls As Single  ' Longueur de la cellule de mesure
        Public Lg As Single   ' Longueur des cellules de garde
        Public lt As Single  ' Longueur totale de la partie déformable de la sonde
        Public LongInjectePompage As Single
        Public Vs As Single    ' Volume de la sonde
        Public Ds As Single    ' Diamètre intérieur du Tube de calibration
        Public EpMembraneGarde As Single
        Public PrEpMembraneGarde As Single
        Public EpMembraneMesure As Single
        Public PrEpMembraneMesure As Single
        Public EpTubeFendu As Single
        Public PrEpTubeFendu As Single
        Public EpInitMembrane As Single
        Public PrEpInitMembrane As Single
        Public PrLong, PrDiam, PrP, PrV, PrPV, PrZ, Prt As Single
        Public Pm As Single
        Public A, B, Vc, PrecisA, PrecisB, PrecisVs, PrecisVc As Single
        Public Event HasBeenModified()
        Public EventsSuspendus As Boolean
        Public MyListe As ListeSonde
        Public IsSaved As Boolean







        Public Function GetRefSonde() As String
            GetRefSonde = Me.GetRefSonde
        End Function

        Public Function GetPrEpMembraneGarde() As Single
            GetPrEpMembraneGarde = PrEpMembraneGarde
        End Function

        Public Function GetPrEpMembraneMesure() As Single
            GetPrEpMembraneMesure = PrEpMembraneMesure
        End Function

        Public Function GetPrEpTubeFendu() As Single
            GetPrEpTubeFendu = PrEpTubeFendu
        End Function

        Public Function GetTypeSonde() As Integer
            GetTypeSonde = TypeSonde
        End Function

        Public Function GetNomTypeSonde() As String
            GetNomTypeSonde = 0
            Select Case TypeSonde
                Case TypeDeSonde.SoupleTypeE
                    Select Case Lang
                        Case 0
                            GetNomTypeSonde = "Flexible, Structure E"
                        Case 1
                            GetNomTypeSonde = "Souple, Structure E"
                    End Select

                Case TypeDeSonde.SoupleTypeG
                    Select Case Lang
                        Case 0
                            GetNomTypeSonde = "Flexible, Structure G"
                        Case 1
                            GetNomTypeSonde = "Souple, Structure G"
                    End Select

                Case TypeDeSonde.TubeFenduCourte
                    Select Case Lang
                        Case 0
                            GetNomTypeSonde = "Short Slotted Tube"
                        Case 1
                            GetNomTypeSonde = "Tube fendu, courte"
                    End Select


                Case TypeDeSonde.TubeFenduLongue
                    Select Case Lang
                        Case 0
                            GetNomTypeSonde = "Long Slotted Tube"
                        Case 1
                            GetNomTypeSonde = "Tube fendu, longue"
                    End Select

            End Select
        End Function

        Public Function GetTypeDeformee() As Integer
            GetTypeDeformee = TypeDeformee
        End Function

        Public Function GetNomTypeDeformee() As String
            GetNomTypeDeformee = 0
            Select Case TypeDeformee
                Case 0      ' Nue = 0
                    Select Case Lang
                        Case 0
                            GetNomTypeDeformee = "Rugby ball"
                        Case 1
                            GetNomTypeDeformee = "Ballon Rugby"
                    End Select

                Case 1     ' Lanternee = 1
                    Select Case Lang
                        Case 0
                            GetNomTypeDeformee = "Slotted tube"
                        Case 1
                            GetNomTypeDeformee = "Tube Lanterné"
                    End Select
            End Select
        End Function

        Public Function GetTypeContrainte() As Integer
            GetTypeContrainte = TypeContrainte
        End Function

        Public Function GetNomTypeContrainte() As String
            GetNomTypeContrainte = ""
            Select Case TypeContrainte
                Case TypeDeContrainte.Moyenne
                    Select Case Lang
                        Case 0
                            GetNomTypeContrainte = "Mean Stress"
                        Case 1
                            GetNomTypeContrainte = "Contrainte moyenne"
                    End Select

                Case TypeDeContrainte.Lame
                    Select Case Lang
                        Case 0
                            GetNomTypeContrainte = "On board"
                        Case 1
                            GetNomTypeContrainte = "Contrainte sur les lames"
                    End Select

            End Select
        End Function

        Public Function Getlt() As Single
            Getlt = lt
        End Function

        Public Function GetLongFente() As Single
            GetLongFente = lt
        End Function

        Public Function Getlg() As Single
            Getlg = Lg
        End Function

        Public Function Getls() As Single
            Getls = Ls
        End Function

        Public Function GetLongInjectePompage() As Single
            GetLongInjectePompage = LongInjectePompage
        End Function

        Public Function GetVc() As Single
            GetVc = Vc
        End Function

        Public Function GetVs() As Single
            GetVs = Vs
        End Function

        Public Function Getds() As Single
            Getds = Ds
        End Function

        Public Function GetEpMembraneGarde() As Single
            GetEpMembraneGarde = EpMembraneGarde
        End Function

        Public Function GetEpMembraneMesure() As Single
            GetEpMembraneMesure = EpMembraneMesure
        End Function

        Public Function GetEpaisTubeFendu() As Single
            GetEpaisTubeFendu = EpTubeFendu
        End Function

        Public Function GetPrEpMembraneCentre() As Single
            GetPrEpMembraneCentre = PrEpInitMembrane
        End Function

        Public Function GetPm() As Integer
            GetPm = Pm
        End Function

        Public Function GetClassPrecis() As Integer
            GetClassPrecis = ClassPrecis
        End Function

        Public Function GetPrecisVolSonde() As Single
            GetPrecisVolSonde = PrecisVolSonde
        End Function

        Public Function GetEssai(ByVal ref As String, ByRef typeEssai As Integer) As Object
            Dim result As Integer
            GetEssai = 0
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssai = New EssaiResistance
                    result = GetIndexEssai(ref, typeEssai)
                    If result > -1 Then
                        With MaListeSonde.Tables("LesEssaisResistancePropre")
                            GetEssai.refEssai = .Rows(result).Item("refEssai").ToString
                            GetEssai.refSonde = .Rows(result).Item("refSonde").ToString
                            GetEssai.ClassPrecis = Valeurs(.Rows(result).Item("ClassPrecis").ToString)
                            GetEssai.nomEssai = .Rows(result).Item("nomEssai").ToString
                            GetEssai.dateEssai = .Rows(result).Item("dateEssai").ToString
                            GetEssai.difPresEauAir = .Rows(result).Item("difPresEauAir")
                            GetEssai.hauteurCpv = .Rows(result).Item("hauteurCpv")
                            'GetEssai.MesureCount = .Rows(result).Item("MesureCount")
                            GetEssai.A0 = .Rows(result).Item("a0")
                            GetEssai.A1 = .Rows(result).Item("a1")
                            GetEssai.A2 = .Rows(result).Item("a2")
                            GetEssai.A3 = .Rows(result).Item("a3")
                            'ReDim GetEssai.Mesure(1, GetEssai.MesureCount)
                            'For i = 0 To GetEssai.MesureCount - 1
                            '    GetEssai.Mesure(0, i) = .mesures(0, i)
                            '    GetEssai.Mesure(1, i) = .mesures(1, i)
                            'Next i
                        End With
                    End If
                Case EssaiCalib.Expansion
                    GetEssai = New EssaiExpansion
                    result = GetIndexEssai(ref, typeEssai)
                    If result > -1 Then
                        With MaListeSonde.Tables("LesEssaisExpansionPropre")
                            GetEssai.refEssai = .Rows(result).Item("refEssai").ToString
                            GetEssai.refSonde = .Rows(result).Item("refSonde").ToString
                            GetEssai.nomEssai = .Rows(result).Item("nomEssai").ToString
                            GetEssai.dateEssai = .Rows(result).Item("dateEssai").ToString
                            GetEssai.difPresEauAir = .Rows(result).Item("difPresEauAir")
                            GetEssai.hauteurCpv = .Rows(result).Item("hauteurCpv")
                            GetEssai.coefficientA = .Rows(result).Item("coefficientA")
                            GetEssai.coefficientB = .Rows(result).Item("coefficientB")
                            GetEssai.Vc = .Rows(result).Item("Vc")
                            GetEssai.Vs = .Rows(result).Item("Vs")
                            GetEssai.PrecisVs = .Rows(result).Item("PrecisVs")
                            GetEssai.PrecisVc = .Rows(result).Item("PrecisVc")
                            GetEssai.PrecisA = .Rows(result).Item("PrecisA")
                            GetEssai.PrecisB = .Rows(result).Item("PrecisB")
                            GetEssai.Di = .Rows(result).Item("Di")
                        End With
                    Else
                        GetEssai = Nothing
                    End If
            End Select

        End Function

        Public Function GetMyliste() As ListeSonde
            GetMyliste = MyListe
        End Function

        Public Function GetIsSaved() As Boolean
            GetIsSaved = IsSaved
        End Function

        ' ******************************************************************************************************************
        ' Chargement dans Resistance ou Expansion des essais de Probe
        ' ******************************************************************************************************************
        Public Sub GetListeResistanceExpansionParRef(ByVal RefSonde As String, ByVal typeEssai As Integer)
            Dim i, EssaiCount As Integer

            ' Recherche de l'essai par son numéro d'index
            EssaiCount = 0
            MySonde = Me
            ' Index = 0 correspond au premier essai de rang 1
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    With MaListeSonde.Tables("LesEssaisResistancePropre")
                        If MySonde.Resistance Is Nothing Then
                            MySonde.Resistance = New System.Collections.Generic.List(Of EssaiResistance)()
                        Else
                            MySonde.Resistance.Clear()
                        End If
                        For i = 0 To .Rows.Count - 1    ' On lit l'ensemble de la Tables "LesEssaisResistancePropre"
                            If .Rows(i).Item("refSonde").ToString = RefSonde Then   ' On a trouvé un EssaiResistance lié à RefSonde
                                MySonde.Resistance.Add(New EssaiResistance())
                                EssaiCount = EssaiCount + 1
                                MySonde.Resistance(EssaiCount - 1) = New EssaiResistance
                                MySonde.Resistance(EssaiCount - 1).RefSonde = RefSonde
                                MySonde.Resistance(EssaiCount - 1).ClassPrecis = ClassPrecis
                                MySonde.Resistance(EssaiCount - 1).RefEssai = .Rows(i).Item("refEssai").ToString
                                MySonde.Resistance(EssaiCount - 1).nomEssai = .Rows(i).Item("nomEssai").ToString
                                If .Rows(i).Item("dateEssai").ToString <> "" Then MySonde.Resistance(EssaiCount - 1).dateEssai = .Rows(i).Item("dateEssai")
                                MySonde.Resistance(EssaiCount - 1).difPresEauAir = .Rows(i).Item("difPresEauAir")
                                MySonde.Resistance(EssaiCount - 1).hauteurCpv = .Rows(i).Item("hauteurCpv")
                                'MySonde.Resistance(EssaiCount - 1).MesureCount = .Rows(i).Item("MesureCount")
                                MySonde.Resistance(EssaiCount - 1).A0 = .Rows(i).Item("a0")

                                MySonde.Resistance(EssaiCount - 1).A1 = .Rows(i).Item("a1")
                                MySonde.Resistance(EssaiCount - 1).A2 = .Rows(i).Item("a2")
                                MySonde.Resistance(EssaiCount - 1).A3 = .Rows(i).Item("a3")
                                'ResistanceCount = EssaiCount
                            End If
                        Next i
                    End With
                Case EssaiCalib.Expansion
                    With MaListeSonde.Tables("LesEssaisExpansionPropre")
                        If MySonde.Expansion Is Nothing Then
                            MySonde.Expansion = New System.Collections.Generic.List(Of EssaiExpansion)()
                        Else
                            MySonde.Expansion.Clear()
                        End If
                        For i = 0 To .Rows.Count - 1    ' On lit l'ensemble de la Tables "LesEssaisExpansionPropre"
                            If .Rows(i).Item("refSonde").ToString = RefSonde Then   ' On a trouvé un EssaiResistance lié à RefSonde
                                MySonde.Expansion.Add(New EssaiExpansion())
                                EssaiCount = EssaiCount + 1
                                MySonde.Expansion(EssaiCount - 1) = New EssaiExpansion
                                MySonde.Expansion(EssaiCount - 1).RefSonde = RefSonde
                                MySonde.Expansion(EssaiCount - 1).RefEssai = .Rows(i).Item("refEssai").ToString
                                MySonde.Expansion(EssaiCount - 1).nomEssai = .Rows(i).Item("nomEssai").ToString
                                MySonde.Expansion(EssaiCount - 1).dateEssai = .Rows(i).Item("dateEssai").ToString
                                If .Rows(i).Item("difPresEauAir").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).difPresEauAir = .Rows(i).Item("difPresEauAir")
                                If .Rows(i).Item("hauteurCpv").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).hauteurCpv = .Rows(i).Item("hauteurCpv")
                                If .Rows(i).Item("Di").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Di = .Rows(i).Item("Di")
                                If .Rows(i).Item("coefficientA").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).coefficientA = .Rows(i).Item("coefficientA")
                                If .Rows(i).Item("coefficientB").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).coefficientB = .Rows(i).Item("coefficientB")

                                If .Rows(i).Item("Vc").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Vc = .Rows(i).Item("Vc")
                                If .Rows(i).Item("Vs").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Vs = .Rows(i).Item("Vs")
                                If .Rows(i).Item("PrecisVs").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisVs = .Rows(i).Item("PrecisVs")
                                If .Rows(i).Item("PrecisVc").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisVc = .Rows(i).Item("PrecisVc")
                                If .Rows(i).Item("PrecisA").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisA = .Rows(i).Item("PrecisA")
                                If .Rows(i).Item("PrecisB").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisB = .Rows(i).Item("PrecisB")
                                'MySonde.ExpansionCount = EssaiCount
                            End If
                        Next i
                    End With
            End Select

        End Sub

        ' ******************************************************************************************************************
        ' Recherche dans les listes MySonde.Resistance() ou Expansion() de Probe de l'essai de rang Index
        ' ******************************************************************************************************************
        Public Function GetEssaiResistanceParIndex(ByVal Index As Integer, ByVal typeEssai As Integer) As EssaiResistance

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiResistanceParIndex = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    If Not (MySonde.Resistance Is Nothing) Then
                        GetEssaiResistanceParIndex = MySonde.Resistance(Index)
                    Else
                        GetEssaiResistanceParIndex = Nothing
                    End If
                Case EssaiCalib.Expansion
                    GetEssaiResistanceParIndex = Nothing
            End Select

        End Function

        ' Recherche dans les listes Resistance() ou Expansion() de Me de l'essai de rang Index
        Public Function GetEssaiExpansionParIndex(ByVal Index As Integer, ByVal typeEssai As Integer) As EssaiExpansion

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiExpansionParIndex = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiExpansionParIndex = Nothing
                Case EssaiCalib.Expansion
                    If Not (Expansion Is Nothing) And Index < Expansion.Count Then
                        GetEssaiExpansionParIndex = MySonde.Expansion(Index)
                    Else
                        GetEssaiExpansionParIndex = Nothing
                    End If
            End Select

        End Function

        ' Recherche dans les listes Resistance() ou Expansion() de Me de l'essai de rang Index
        Public Function GetEssaiExpansionParRef(ByVal Ref As String, ByVal typeEssai As Integer) As EssaiExpansion
            Dim i As Integer

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiExpansionParRef = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiExpansionParRef = Nothing
                Case EssaiCalib.Expansion
                    If Not (MySonde.Expansion Is Nothing) Then

                        'If MyListeSonde.Expansion.Count <> MySonde.Expansion.Length Then
                        '    MessageBox.Show("Incohérence entre Expansion.Count et la taille du tableau.")
                        '    Exit Function ' ou Exit Function selon le contexte
                        'End If

                        For i = 0 To MySonde.Expansion.Count - 2
                            'If MyListeSonde.ListeSonde(i) Is Nothing Then
                            '    ' Si MySonde.Expansion(i) est Nothing, passez à l'itération suivante
                            '    Continue For
                            'End If
                            If MySonde.Expansion(i).RefEssai = Ref Then
                                GetEssaiExpansionParRef = MySonde.Expansion(i)
                                Exit For
                            End If
                            If MySonde.Expansion(i).RefEssai = Ref Then GetEssaiExpansionParRef = MySonde.Expansion(i)
                        Next i
                        Exit Function
                    Else
                        Exit Function
                    End If
            End Select

        End Function

        ' Recherche dans les listes Resistance() ou Expansion() de Me de l'essai de rang Index
        Public Function GetEssaiResistanceParRef(ByVal Ref As String, ByVal typeEssai As Integer) As EssaiResistance
            Dim i As Integer

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiResistanceParRef = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    If Not (MySonde.Resistance Is Nothing) Then
                        For i = 0 To MySonde.Resistance.Count - 1
                            If MySonde.Resistance(i).RefEssai = Ref Then GetEssaiResistanceParRef = MySonde.Resistance(i)
                        Next i
                        Exit Function
                    Else
                        Exit Function
                    End If
                Case EssaiCalib.Expansion
                    GetEssaiResistanceParRef = Nothing
            End Select

        End Function

        ' ************************************************************************************************************
        ' Sélection de l'Essai de Résistance ou Expansion d'après sa référence
        ' ************************************************************************************************************
        Public Function GetEssaiParRef(ByVal RefNom As String, ByVal typeEssai As Integer) As Object
            Dim index, Nb As Integer
            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiParRef = Nothing

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiParRef = New EssaiResistance
                    Nb = MaListeSonde.Tables("LesEssaisResistancePropre").Rows.Count
                    If Not (MaListeSonde.Tables("LesEssaisResistancePropre") Is Nothing) Then
                        For index = 0 To Nb - 1
                            If MaListeSonde.Tables("LesEssaisResistancePropre").Rows(index).Item("RefEssai").ToString = RefNom Then Exit For
                        Next index
                        'GetEssaiParRef = Resistance(index)
                        If index = Nb Then
                            Select Case Lang
                                Case 0
                                    MsgBox("The Resistance test " & RefNom & " is not found")
                                Case 1
                                    MsgBox("L'Essai de Résistance " & RefNom & "n'est pas trouvé")
                            End Select
                            Exit Function
                        End If
                        With MaListeSonde.Tables("LesEssaisResistancePropre").Rows(index)
                            GetEssaiParRef.refessai = .Item("refEssai").ToString
                            GetEssaiParRef.refsonde = .Item("refSonde").ToString
                            GetEssaiParRef.ClassPrecis = Valeurs(.Item("ClassPrecis").ToString)
                            GetEssaiParRef.nomEssai = .Item("nomEssai").ToString
                            GetEssaiParRef.dateEssai = .Item("dateEssai").ToString
                            GetEssaiParRef.difPresEauAir = .Item("difPresEauAir").ToString
                            GetEssaiParRef.HauteurCpv = .Item("HauteurCpv").ToString
                            'GetEssaiParRef.MesureCount = .Item("MesureCount").ToString
                            GetEssaiParRef.A0 = .Item("A0").ToString
                            GetEssaiParRef.A1 = .Item("A1").ToString
                            GetEssaiParRef.A2 = .Item("A2").ToString
                            GetEssaiParRef.A3 = .Item("A3").ToString
                            GetEssaiParRef.ChargeMesures(GetEssaiParRef.refessai)
                            'GetEssaiParRef.mesurecount = GetEssaiParRef.Mesures.rows.count
                        End With
                    Else
                        GetEssaiParRef = Nothing
                    End If
                Case EssaiCalib.Expansion
                    GetEssaiParRef = New EssaiExpansion
                    Nb = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
                    If Not (MaListeSonde.Tables("LesEssaisExpansionPropre") Is Nothing) Then
                        For index = 0 To Nb - 1
                            If MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(index).Item("RefEssai").ToString = RefNom Then
                                'GetEssaiParRef = Expansion(index)
                                With MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(index)
                                    GetEssaiParRef.refessai = .Item("refEssai").ToString
                                    GetEssaiParRef.refsonde = .Item("refSonde").ToString
                                    GetEssaiParRef.ClassPrecis = Valeurs(.Item("ClassPrecis").ToString)
                                    GetEssaiParRef.nomEssai = .Item("nomEssai").ToString
                                    GetEssaiParRef.dateEssai = .Item("dateEssai").ToString
                                    GetEssaiParRef.difPresEauAir = Valeurs(.Item("difPresEauAir").ToString)
                                    GetEssaiParRef.HauteurCpv = Valeurs(.Item("HauteurCpv").ToString)
                                    GetEssaiParRef.Di = Valeurs(.Item("Di").ToString)
                                    GetEssaiParRef.Vc = Valeurs(.Item("Vc").ToString)
                                    GetEssaiParRef.Vs = Valeurs(.Item("VS").ToString)
                                    GetEssaiParRef.EpMembrane = Valeurs(.Item("EpMembrane").ToString)
                                    GetEssaiParRef.PrEpMembrane = Valeurs(.Item("PrEpMembrane").ToString)
                                    'GetEssaiParRef.MesureCount = Valeurs(.Item("MesureCount").ToString)
                                    GetEssaiParRef.Prp = Valeurs(.Item("Prp").ToString)
                                    GetEssaiParRef.PrV = Valeurs(.Item("PrV").ToString)
                                    GetEssaiParRef.coefficientA = Valeurs(.Item("coefficientA").ToString)
                                    GetEssaiParRef.coefficientB = Valeurs(.Item("coefficientB").ToString)
                                    GetEssaiParRef.PrecisVc = Valeurs(.Item("PrecisVc").ToString)
                                    GetEssaiParRef.PrecisVs = Valeurs(.Item("PrecisVs").ToString)
                                    GetEssaiParRef.PrEpMembrane = Valeurs(.Item("PrEpMembrane").ToString)
                                    GetEssaiParRef.PrecisA = Valeurs(.Item("PrecisA").ToString)
                                    GetEssaiParRef.PrecisB = Valeurs(.Item("PrecisB").ToString)
                                    GetEssaiParRef.ChargeMesures(GetEssaiParRef.refessai)

                                    'GetEssaiParRef.mesurecount = GetEssaiParRef.Mesures.rows.count
                                    Exit For
                                End With
                            End If

                        Next index

                    Else
                        GetEssaiParRef = Nothing
                    End If
            End Select

        End Function

        ' **********************************************************************************************
        ' Recherche le l'indice de l'Essai de Résistance ou d'Expansion dans MySonde
        ' **********************************************************************************************
        Public Function GetIndexSondeEssai(ByVal ref As String, ByVal typeEssai As Integer) As Integer
            ' Recherche de l'essai ayant pour référence 'ref' avec arrêt dès que possible
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSonde
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    nb = MySonde.Expansion.Count
                Case EssaiCalib.Resistance
                    nb = MySonde.Resistance.Count
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        'If (MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                        If (MySonde.GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                    Case EssaiCalib.Resistance
                        'If (MaListeSonde.Tables("LesEssaisResistancePropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                        If (MySonde.GetEssaiResistanceParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                End Select

                i = i + 1
            Loop
            ' Si la sonde n'a pas été trouvée, la valeur -1 est renvoyée
            If i = nb Then i = -1

            GetIndexSondeEssai = i
        End Function


        ' **********************************************************************************************
        ' Recherche le l'indice de l'Essai de Résistance ou d'Expansion d'après sa référence
        ' **********************************************************************************************
        Public Function GetIndexEssai(ByVal ref As String, ByVal typeEssai As Integer) As Integer
            ' Recherche de l'essai ayant pour référence 'ref' avec arrêt dès que possible
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSonde
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    nb = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
                'nb = MySonde.Expansion.Count
                Case EssaiCalib.Resistance
                    nb = MaListeSonde.Tables("LesEssaisResistancePropre").Rows.Count
                    'nb = MySonde.Resistance.Count
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        If (MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).RowState <> DataRowState.Deleted) Then
                            If (MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                            'If (Me.Expansion(i).GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                        End If
                    Case EssaiCalib.Resistance
                        If (MaListeSonde.Tables("LesEssaisResistancePropre").Rows(i).RowState <> DataRowState.Deleted) Then
                            If (MaListeSonde.Tables("LesEssaisResistancePropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                            'If (Resistance(i).MGetEssaiResistanceParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                        End If
                End Select

                i = i + 1
            Loop
            ' Si la sonde n'a pas été trouvée, la valeur -1 est renvoyée
            If i = nb Then i = -1

            GetIndexEssai = i
        End Function

        ' **********************************************************************************************
        ' Recherche le l'indice de l'Essai de Résistance ou d'Expansion d'après sa référence
        ' **********************************************************************************************
        Public Function GetMySondeIndexEssai(ByVal ref As String, ByVal typeEssai As Integer) As Integer
            ' Recherche de l'essai ayant pour référence 'ref' avec arrêt dès que possible
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSonde
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    'nb = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
                    nb = MySonde.Expansion.Count
                Case EssaiCalib.Resistance
                    'nb = MaListeSonde.Tables("LesEssaisResistancePropre").Rows.Count
                    nb = MySonde.Resistance.Count
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        If (MySonde.Expansion(i).RefEssai.ToString = ref) Then Exit Do
                    'If (Me.Expansion(i).GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                    Case EssaiCalib.Resistance
                        If (MySonde.Resistance(i).RefEssai.ToString = ref) Then Exit Do
                        'If (Me.Resistance(i).MGetEssaiResistanceParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                End Select

                i = i + 1
            Loop
            ' Si la sonde n'a pas été trouvée, la valeur -1 est renvoyée
            If i = nb Then i = -1

            GetMySondeIndexEssai = i
        End Function


        Public Function GetEssaiCount(ByVal typeEssai As Integer) As Integer
            GetEssaiCount = 0
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiCount = MySonde.Resistance.Count

                Case EssaiCalib.Expansion
                    GetEssaiCount = MySonde.Expansion.Count

            End Select
        End Function

        Public Sub SetRefSonde(ByVal refSond As String)
            RefSonde = refSond

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeSonde(ByVal typeSond As String)
            TypeSonde = Int(Valeurs(typeSond))

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeDeformee(ByVal typeDeforme As String)
            TypeDeformee = Valeurs(typeDeforme)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeContrainte(ByVal typeContraint As String)
            TypeContrainte = Valeurs(typeContraint)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub Setlt(ByVal longTotal As Single)
            lt = longTotal

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub


        Public Sub Setlg(ByVal longCelGard As Single)
            Lg = longCelGard

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub Setls(ByVal longCelMesur As Single)
            Ls = longCelMesur
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetLongInjectePompage(ByVal longInjectePompag As Single)
            LongInjectePompage = longInjectePompag

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub Setds(ByVal diamInitia As Single)
            Ds = diamInitia
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetEpMembraneGarde(ByVal epaisMenbrane As Single)
            EpMembraneGarde = epaisMenbrane
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetEpMembraneCentre(ByVal EpMembraneMesur As Single)
            EpMembraneMesure = EpMembraneMesur

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetEpMembraneMesure(ByVal EpMembraneMes As Single)
            EpMembraneMesure = EpMembraneMes
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SetEpMembraneCentre(ByVal EpMembraneCen As Single)
        '    EpMembraneCentr = EpMembraneCen
        '    'Call CalculeVol()
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        Public Sub SetEpaisTubeFendu(ByVal epaisTubeFend As Single)
            EpTubeFendu = epaisTubeFend
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetVc(ByVal Volume As Single)

            'Call CalculeVol
            Vc = Volume
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetVs(ByVal Volume As Single)

            'Call CalculeVol
            Vs = Volume
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub
        Public Sub SetPm(ByVal p As Integer)
            Pm = p

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrecisVolSonde(ByVal p As Single)
            PrecisVolSonde = p

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrDiam(ByVal Pr As Single)
            PrDiam = Pr

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrEpMembraneGarde(ByVal Pr As Single)
            PrEpMembraneGarde = Pr

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrEpTubeFendu(ByVal Pr As Single)
            PrEpTubeFendu = Pr

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetPrEpMembraneMesure(ByVal Pr As Single)
            PrEpMembraneMesure = Pr

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub


        Public Sub SetClassPrecis(ByVal p As Integer)
            ClassPrecis = p

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Function SetEssai(ByVal typeEssai As Integer, ByRef Essai As Object, ByVal indice As Integer)

            '    Dim NewEssai As Object
            SetEssai = Nothing

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Déclaration d'un essai de résistance propre
                    If indice > MySonde.Resistance.Count - 1 Then Exit Function

                    ' Redimensionnement du tableau dynamique de Essais
                    '            ReDim Preserve Resistance(Resistance.Count - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Resistance(MySonde.Resistance.Count - 1) = Essai

                Case EssaiCalib.Expansion
                    ' Déclaration d'un essai de résistance propre
                    ' Incrémentation du compteur d'essais de résistance propre
                    If indice > MySonde.Expansion.Count - 1 Then Exit Function


                    ' Redimensionnement du tableau dynamique de Essais
                    '            ReDim Preserve Expansion(Expansion.Count - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Expansion(Expansion.Count - 1) = Essai

            End Select

            ' Enregistrement de la sonde auprès de son nouvel Essai
            '    Call NewEssai.SetMySonde(Me)

            ' Attribution d'une référence au nouvel essai
            '    NewEssai.SetRefEssai (GiveMeANewRefEssai(typeEssai))

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

            ' passe 1

            '    Set NouvelEssai = NewEssai

        End Function

        Public Sub SetMyListe(ByRef liste As ListeSonde)
            MyListe = liste
        End Sub

        ' Traitement de l'évènement IHaveBeenModified
        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                IsSaved = False
                Call MyListeSonde.IHaveBeenModified()

                ' Suppression de l'évènement IHaveBeenModified
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Private _readService As DatabaseReadService

        ''' <summary>
        ''' Récupère le premier essai de résistance de la sonde
        ''' (Anciennement : Show())
        ''' </summary>
        Public Function GetFirstResistanceEssai() As Dictionary(Of String, Object)
            Dim essai As New Dictionary(Of String, Object)

            Try
                ' Utilise le service de lecture au lieu de DataSet
                Dim dataTable = _readService.GetEssaisResistanceBySonde(Me.RefSonde)

                If dataTable.Rows.Count > 0 Then
                    Dim firstRow = dataTable.Rows(0)

                    essai.Add("refEssai", firstRow("refEssai").ToString())
                    essai.Add("refSonde", firstRow("refSonde").ToString())
                    essai.Add("nomEssai", firstRow("nomEssai").ToString())
                    essai.Add("dateEssai", Convert.ToDateTime(firstRow("dateEssai")))
                    essai.Add("ClassPrecis", CInt(firstRow("ClassPrecis")))
                    essai.Add("Prp", CSng(firstRow("Prp")))
                    essai.Add("PrV", CSng(firstRow("PrV")))
                    essai.Add("A", CSng(firstRow("A")))
                    essai.Add("B", CSng(firstRow("B")))
                    essai.Add("PrecisVc", CSng(firstRow("PrecisVc")))
                    essai.Add("PrecisVs", CSng(firstRow("PrecisVs")))
                    ' ... autres colonnes ...

                    Return essai
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Console.WriteLine($"❌ Erreur GetFirstResistanceEssai: {ex.Message}")
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Récupère le premier essai d'expansion de la sonde
        ''' </summary>
        Public Function GetFirstExpansionEssai() As Dictionary(Of String, Object)
            Dim essai As New Dictionary(Of String, Object)

            Try
                Dim dataTable = _readService.GetEssaisExpansionBySonde(Me.RefSonde)

                If dataTable.Rows.Count > 0 Then
                    Dim firstRow = dataTable.Rows(0)

                    essai.Add("refEssai", firstRow("refEssai").ToString())
                    essai.Add("refSonde", firstRow("refSonde").ToString())
                    essai.Add("nomEssai", firstRow("nomEssai").ToString())
                    essai.Add("dateEssai", Convert.ToDateTime(firstRow("dateEssai")))
                    essai.Add("ClassPrecis", CInt(firstRow("ClassPrecis")))
                    essai.Add("difPresEauAir", CSng(firstRow("difPresEauAir")))
                    essai.Add("hauteurCpv", CSng(firstRow("hauteurCpv")))
                    essai.Add("Di", CSng(firstRow("Di")))
                    essai.Add("Vc", CSng(firstRow("Vc")))
                    essai.Add("Vs", CSng(firstRow("Vs")))
                    essai.Add("EpMembrane", CSng(firstRow("EpMembrane")))
                    essai.Add("Prp", CSng(firstRow("Prp")))
                    essai.Add("PrV", CSng(firstRow("PrV")))
                    essai.Add("coefficientA", CSng(firstRow("coefficientA")))
                    essai.Add("coefficientB", CSng(firstRow("coefficientB")))
                    essai.Add("PrecisVc", CSng(firstRow("PrecisVc")))
                    essai.Add("PrecisVs", CSng(firstRow("PrecisVs")))
                    essai.Add("PrEpMembrane", CSng(firstRow("PrEpMembrane")))
                    essai.Add("PrecisA", CSng(firstRow("PrecisA")))
                    essai.Add("PrecisB", CSng(firstRow("PrecisB")))
                    ' ... autres colonnes ...

                    Return essai
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Console.WriteLine($"❌ Erreur GetFirstExpansionEssai: {ex.Message}")
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' SUPPRIME : Show() et MaskSonde() n'existent plus
        ''' ❌ Ces méthodes sont remplacées par des appels contrôleur/vue
        ''' </summary>
        ' SUPPRIMÉ : Public Sub Show()
        ' SUPPRIMÉ : Public Sub MaskSonde()

        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        '' Les conserver si elles ne sont pas liées aux Windows Forms
        'Public refSonde As String
        'Public typeSonde As Int32
        'Public typeDeformee As Int32
        ' ... autres propriétés métier

        ' ═════════════════════════════════════════════════════
        ' NOUVEAU CONSTRUCTEUR SIMPLIFIÉ
        ' ═════════════════════════════════════════════════════

        ' ═══════════════════════════════════════════════════════
        ' CONSTRUCTEUR
        ' ═══════════════════════════════════════════════════════

        Public Sub New()
            ' Initialisation des listes
            Resistance = New List(Of EssaiResistance)
            Expansion = New List(Of EssaiExpansion)
            InitialiserParametresParDefaut()
            RefSonde = "refSonde"
            Me.SetRefSonde(RefSonde)
        End Sub


        ''' <summary>
        ''' Initialise les paramètres dimensionnels par défaut
        ''' </summary>
        Private Sub InitialiserParametresParDefaut()
            ' Initialisation des paramètres dimensionnels de la sonde
            RefSonde = "RefSonde"
            TypeSonde = TypeDeSonde.SoupleTypeE
            TypeDeformee = TypeDeDeformee.BallonRugby
            TypeContrainte = TypeDeContrainte.Moyenne
            lt = 450      ' Longueur totale
            Lg = 120      ' Longueur des cellules de garde
            Ls = 210      ' Longueur de la cellule de mesure
            LongInjectePompage = 700
            Vs = 650      ' Volume de la sonde
            Ds = 60       ' Diamètre intérieur du Tube
            EpMembraneGarde = 5
            EpMembraneMesure = 1
            EpInitMembrane = 6
            EpTubeFendu = 10
            Pm = 50

            ' Initialisation des listes
            'ResistanceCount = 0
            'Expansion.Count = 0
            IsSaved = True
            EventsSuspendus = False
        End Sub

        ''' <summary>
        ''' Réinitialise la sonde aux valeurs par défaut
        ''' </summary>
        Public Sub ResetToDefaults()
            InitialiserParametresParDefaut()
        End Sub

        ' ═══════════════════════════════════════════════════════
        ' REMPLACER TOUS LES "ReDim Preserve" PAR DES MÉTHODES List
        ' ═══════════════════════════════════════════════════════

        ''' <summary>
        ''' Charge les essais de résistance depuis la base de données
        ''' Remplace GetListeResistanceExpansionParRef (ancien code)
        ''' </summary>
        Public Sub GetListeResistanceParRef(RefSonde As String)
            Try
                ' Vider la liste existante
                Resistance.Clear()

                ' Charger depuis la base de données
                Dim dataTable = MaListeSonde.Tables("LesEssaisResistancePropre")

                If dataTable IsNot Nothing Then
                    For Each row In dataTable.Rows
                        If row("refSonde").ToString = RefSonde Then
                            Dim essai As New EssaiResistance
                            essai.RefEssai = row("refEssai").ToString()
                            essai.RefSonde = row("refSonde").ToString()
                            essai.nomEssai = row("nomEssai").ToString()
                            essai.dateEssai = row("dateEssai")
                            essai.ClassPrecis = Valeurs(row("ClassPrecis"))
                            essai.typeEssai = Valeurs(row("typeEssai"))
                            essai.difPresEauAir = Valeurs(row("difPresEauAir"))
                            essai.hauteurCpv = Valeurs(row("hauteurCpv"))
                            essai.A0 = Valeurs(row("A0"))
                            essai.A1 = Valeurs(row("A1"))
                            essai.A2 = Valeurs(row("A2"))
                            essai.A3 = Valeurs(row("A3"))

                            ' Ajouter à la liste
                            Resistance.Add(essai)
                        End If
                    Next
                End If

                ' Mettre à jour le compteur
                ' Resistance.Count = Resistance.Count

            Catch ex As Exception
                Console.WriteLine($"❌ Erreur GetListeResistanceParRef: {ex.Message}")
            End Try
        End Sub


        Public Function NouvelEssai(ByVal typeEssai As Integer) As Object

            NouvelEssai = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Déclaration d'un essai de résistance propre
                    If MyResistance Is Nothing Then
                        MyResistance = New EssaiResistance
                    End If

                    ' Incrémentation du compteur d'essais de résistance propre
                    'Resistance.Count = ResistanceCount + 1

                    ' Redimensionnement du tableau dynamique de Essais
                    'ReDim Preserve MySonde.Resistance(Resistance.Count - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Resistance(Resistance.Count - 1) = New EssaiResistance

                    MyResistance = MySonde.Resistance(Resistance.Count - 1)

                    ' Enregistrement de la sonde auprès de son nouvel Essai
                    Call MyResistance.SetMySonde(Me)

                    ' Attribution d'une référence au nouvel essai
                    With MyResistance
                        ' affectation de la nouvelle référence d'essai
                        .SetRefEssai(GiveMeANewRefEssai(typeEssai))
                    End With

                    NouvelEssai = MyResistance

                Case EssaiCalib.Expansion
                    ' Déclaration d'un essai de résistance propre
                    If MyExpansion Is Nothing Then
                        MyExpansion = New EssaiExpansion
                    End If

                    ' Incrémentation du compteur d'essais de résistance propre
                    'MySonde.Expansion.Count = MySonde.Expansion.Count + 1

                    ' Redimensionnement du tableau dynamique de Essais
                    'ReDim Preserve MySonde.Expansion(Expansion.Count - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Expansion(Expansion.Count - 1) = New EssaiExpansion

                    MyExpansion = MySonde.Expansion(Expansion.Count - 1)

                    ' Enregistrement de la sonde auprès de son nouvel Essai
                    'Call MyExpansion.SetMySonde(Me)

                    ' Attribution d'une référence au nouvel essai
                    With MyExpansion
                        ' affectation de la nouvelle référence d'essai
                        .SetRefEssai(GiveMeANewRefEssai(typeEssai))
                    End With

                    NouvelEssai = MyExpansion

            End Select

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()


        End Function

        Public Sub SupEssaiDansListe(ByVal Ref As String, ByVal typeEssai As Integer)
            Dim i As Integer    ' Index de l'essai à supprimer

            i = GetIndexEssai(Ref, typeEssai)

            Select Case typeEssai
                Case EssaiCalib.Resistance

                    ' Effaçage de l'Essai
                    With MaListeSonde.Tables("LesEssaisResistancePropre")
                        .Rows(i).Delete()
                    End With

                Case EssaiCalib.Expansion
                    ' Effaçage de l'Essai
                    ' Effaçage de l'Essai
                    With MaListeSonde.Tables("LesEssaisExpansionPropre")
                        .Rows(i).Delete()
                    End With


            End Select

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub


        Public Sub SupSondeEssai(ByVal Index As Integer, ByVal typeEssai As Integer)
            'Dim i As Integer    ' Index de l'essai à supprimer
            Dim J As Integer

            'i = GetIndexEssai(ref, typeEssai)

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Effaçage de l'Essai
                    MySonde.Resistance(Index) = Nothing

                    ' Décalage des Essais suivant d'une case
                    For J = Index To Resistance.Count - 2
                        MySonde.Resistance(J) = MySonde.Resistance(J + 1)
                    Next

                    ' Redimensionnement du tableau ListEssais
                    MySonde.Resistance.RemoveAt(Index)

                    ' Décrémentation du compteur de Essais
                    'MySonde.Resistance.Count = MySonde.ResistanceCount - 1

                Case EssaiCalib.Expansion
                    ' Effaçage de l'Essai
                    MySonde.Expansion(Index) = Nothing

                    ' Décalage des Essais suivant d'une case
                    For J = Index To Expansion.Count - 2
                        MySonde.Expansion(J) = MySonde.Expansion(J + 1)
                    Next

                    ' Redimensionnement du tableau ListEssais
                    MySonde.Expansion.RemoveAt(Index)

                    ' Décrémentation du compteur de Essais
                    ' MySonde.ExpansionCount = MySonde.ExpansionCount - 1

            End Select

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub


        ''' <summary>
        ''' Récupère les essais d'une sonde pour affichage en HTML
        ''' Remplace AfficheDansTab (DataGridView)
        ''' </summary>
        Public Function GetEssaisForHtmlDisplay(typeEssai As Integer) As List(Of Dictionary(Of String, Object))
            Dim essais As New List(Of Dictionary(Of String, Object))

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    ' Récupère tous les essais d'expansion de cette sonde
                    Dim dataTable = _readService.GetEssaisExpansionBySonde(Me.RefSonde)

                    For Each row In dataTable.Rows
                        Dim essai As New Dictionary(Of String, Object) From {
                            {"refEssai", row("refEssai").ToString()},
                            {"nomEssai", row("nomEssai").ToString()},
                            {"dateEssai", Convert.ToDateTime(row("dateEssai")).ToString("dd/MM/yyyy")},
                            {"type", "Expansion"}
                        }
                        essais.Add(essai)
                    Next

                Case EssaiCalib.Resistance
                    ' Récupère tous les essais de résistance de cette sonde
                    Dim dataTable = _readService.GetEssaisResistanceBySonde(Me.RefSonde)

                    For Each row In dataTable.Rows
                        Dim essai As New Dictionary(Of String, Object) From {
                            {"refEssai", row("refEssai").ToString()},
                            {"nomEssai", row("nomEssai").ToString()},
                            {"dateEssai", Convert.ToDateTime(row("dateEssai")).ToString("dd/MM/yyyy")},
                            {"type", "Résistance"}
                        }
                        essais.Add(essai)
                    Next
            End Select

            Return essais
        End Function

        ''' <summary>
        ''' Version avec langue (0=Anglais, 1=Français)
        ''' </summary>
        Public Function GetEssaisForHtmlDisplay(typeEssai As Integer, lang As Integer) As Dictionary(Of String, Object)
            Dim result As New Dictionary(Of String, Object)

            ' En-têtes des colonnes
            Dim headers As New List(Of String)
            Select Case lang
                Case 0 ' Anglais
                    headers.Add("Ref. Test")
                    headers.Add("Name Test")
                    headers.Add("Date Test")
                Case 1 ' Français
                    headers.Add("Ref. Essai")
                    headers.Add("Nom Essai")
                    headers.Add("Date Essai")
            End Select

            result("headers") = headers
            result("essais") = GetEssaisForHtmlDisplay(typeEssai)
            result("typeEssai") = If(typeEssai = EssaiCalib.Expansion, "Expansion", "Résistance")

            Return result
        End Function

        'Public Sub AfficheEssaiDansCombo(ByRef Combo As System.Windows.Forms.ComboBox, ByVal typeEssai As Integer)

        '    ' Remplissage de l'enregistrement
        '    Dim i As Integer
        '    Dim essai As Object
        '    essai = Nothing
        '    Combo.Items.Clear()
        '    For i = 0 To GetEssaiCount(typeEssai) - 1
        '        Select Case typeEssai
        '            Case EssaiCalib.Expansion
        '                essai = GetEssaiExpansionParIndex(i, typeEssai)
        '            Case EssaiCalib.Resistance
        '                essai = GetEssaiResistanceParIndex(i, typeEssai)
        '        End Select
        '        Combo.Items.Add(essai.GetRefEssai & " - " & essai.GetDateEssai)
        '    Next i
        'End Sub

        Public Function GiveMeANewRefEssai(ByVal typeEssai As Integer)
            ' Détermination d'une nouvelle référence pour un essai
            Dim k As Integer
            Dim ref As String
            k = 1

            ref = ""
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ref = "Resistance"
                Case EssaiCalib.Expansion
                    ref = "Expansion"
            End Select

            ' Recherche d'une nouvelle référence d'essai qui soit unique dans la liste des EssaisResistance ou des EssaisExpansion
            Do While Not MyListeSonde.IsRefEssaiUnique(ref & " " & k, typeEssai)
                k = k + 1
            Loop
            GiveMeANewRefEssai = ref & " " & k

        End Function

        Public Sub CalculeVol()
            Dim E As Single
            If TypeDeformee = TypeDeDeformee.BallonRugby Then
                E = 0
            Else
                E = EpTubeFendu
            End If
            Vs = Math.Round(0.001 * Pi * Ls * (Ds / 2 - EpMembraneGarde - EpMembraneMesure - E) ^ 2, 1)
        End Sub

        Public Sub CalculeVolNF()
            Dim E, Di As Single

            If TypeDeformee = TypeDeDeformee.BallonRugby Then
                E = 0
            Else
                E = EpTubeFendu
            End If
            If MyExpansion IsNot Nothing Then
                Vc = MyExpansion.GetVc
                Di = MyExpansion.GetDi
                Vs = Math.Round(0.001 * Pi * Ls * (Di / 2) ^ 2, 1) - Vc
                MyExpansion.SetVs(Vs)
                MySonde.MaExpansion = MyExpansion
                MySonde.Vs = Vs
                MySonde.PrecisVs = MyExpansion.PrecisVs
            Else
                MyExpansion = MyEssaiForage.MaExpansion
                If MyExpansion IsNot Nothing Then
                    Vc = MyExpansion.GetVc
                    Di = MyExpansion.GetDi
                    Vs = Math.Round(0.001 * Pi * Ls * (Di / 2) ^ 2, 1) - Vc
                    MyExpansion.SetVs(Vs)
                    MySonde.MaExpansion = MyExpansion
                    MySonde.Vs = MyExpansion.GetVs
                    MySonde.PrecisVs = MyExpansion.GetPrecisVs
                Else
                    Vc = MySonde.Vc
                    'Di = MySonde.Ds
                    'Vs = Math.Round(0.001 * Pi * ls * (Di / 2) ^ 2, 1) - Vc
                    'MySonde.Vs = Vs
                    Vs = MySonde.Vs
                End If

            End If
        End Sub
    End Class
End Namespace
