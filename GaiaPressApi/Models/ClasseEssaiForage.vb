Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPress.Core.V17.Models

    Public Class Essai_Forage

        ' Définition de la clef primaire de la Table LesEssaisForage
        <Key>
        Public Property E_RefChantier As String
        Public Property E_RefForage As String

        <ForeignKey("E_RefSol")>
        Public Overridable Property E_MySol As Sol

        <ForeignKey("E_refSonde")>
        Public Property Overridable Property E_MySonde As Probe

        Public Property MaResistance As Essai_Resistance

        Public Property MaExpansion As Essai_Expansion

        Public Property MonEssai As Essai_Forage
        Public Property E_refSonde As Stringz
        Public Property E_refEssai As String
        Public Property E_refEssaiResistancePropre As String
        Public Property E_refEssaiExpansionPropre As String
        Public Property E_refEssaiPerteDeCharge As String
        Public Property E_RefSol As String
        Public Property E_nomEssai As String
        Public Property E_dateEssai As String
        Public Property E_heureEssai As String
        Public Property E_Enregistreur As String
        Public Property E_Operateur As String
        Public Property E_VolSonde As Single
        Public Property E_ClassPrecis As Integer
        Public Property E_PrecisVolSonde As Single
        Public Property E_difPresEauAir As Single
        Public Property E_hauteurCpv As Single
        Public Property E_presP0 As Single
        Public Property E_presFluage As Single
        Public Property E_presLimite As Single
        Public Property E_presLimiteNF As Single
        Public Property E_presLimiteTheo As Single
        Public Property E_PrecisVolume As Single
        Public Property E_PrecisPression As Single
        Public Property E_poidsVol As Single
        Public Property E_poidsVolDejauge As Single
        Public Property E_coefPoisson As Single
        Public Property E_modPressio As Single
        Public Property E_modPressioNf As Single
        Public Property E_modElastique As Single
        Public Property E_modElastiqueNf As Single
        Public Property E_Cohesion As Single
        Public Property E_Frottement As Single
        Public Property E_FrottementIntergranulaire
        Public Property E_U0 As Single
        Public Property E_profNappe As Single
        Public Property E_profSonde As Single
        Public Property E_coteZeroGeomeca As Single

        ' === PROPRIÉTÉS DE NAVIGATION ===
        <InverseProperty("Essai_Forage")>
        Public Overridable Property E_Mesures As ICollection(Of MesuresEssaiForage) = New List(Of MesuresEssaiForage)

        ' === CONSTRUCTEURS ===
        Public Sub New()
            E_dateEssai = Date.Today.ToString("yyyy-MM-dd")
            E_heureEssai = DateTime.Now.ToString("HH:mm:ss")
            E_Mesures = New List(Of MesuresEssaiForage)
        End Sub

        Public Function GetRefSonde() As String
            GetRefSonde = refSonde
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
            GetTypeSonde = typeSonde
        End Function

        Public Function GetNomTypeSonde() As String
            GetNomTypeSonde = 0
            Select Case typeSonde
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
            GetTypeDeformee = typeDeformee
        End Function

        Public Function GetNomTypeDeformee() As String
            GetNomTypeDeformee = 0
            Select Case typeDeformee
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
            GetTypeContrainte = typeContrainte
        End Function

        Public Function GetNomTypeContrainte() As String
            GetNomTypeContrainte = ""
            Select Case typeContrainte
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
            Getlt = Lt
        End Function

        Public Function GetLongFente() As Single
            GetLongFente = Lt
        End Function

        Public Function Getlg() As Single
            Getlg = Lg
        End Function

        Public Function Getls() As Single
            Getls = Ls
        End Function

        Public Function GetLongInjectePompage() As Single
            GetLongInjectePompage = longInjectePompage
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
            GetEpaisTubeFendu = epTubeFendu
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
                    GetEssai = New Essai_Resistance
                    result = GetIndexEssai(ref, typeEssai)
                    If result > -1 Then
                        With MaListeSondes.Tables("LesEssaisResistancePropre")
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
                    GetEssai = New Essai_Expansion
                    result = GetIndexEssai(ref, typeEssai)
                    If result > -1 Then
                        With MaListeSondes.Tables("LesEssaisExpansionPropre")
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

        Public Function GetMyliste() As ListeSondes
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
                    With MaListeSondes.Tables("LesEssaisResistancePropre")
                        For i = 0 To .Rows.Count - 1    ' On lit l'ensemble de la Tables "LesEssaisResistancePropre"
                            If .Rows(i).Item("refSonde").ToString = RefSonde Then   ' On a trouvé un Essai_Resistance lié à RefSonde
                                If (MySonde.Resistance Is Nothing) Then
                                    ReDim MySonde.Resistance(0)
                                    MySonde.Resistance(0) = New Essai_Resistance
                                End If
                                EssaiCount = EssaiCount + 1
                                ReDim Preserve MySonde.Resistance(EssaiCount)
                                MySonde.Resistance(EssaiCount - 1) = New Essai_Resistance
                                MySonde.Resistance(EssaiCount - 1).refSonde = RefSonde
                                MySonde.Resistance(EssaiCount - 1).ClassPrecis = ClassPrecis
                                MySonde.Resistance(EssaiCount - 1).refEssai = .Rows(i).Item("refEssai").ToString
                                MySonde.Resistance(EssaiCount - 1).nomEssai = .Rows(i).Item("nomEssai").ToString
                                If .Rows(i).Item("dateEssai").ToString <> "" Then MySonde.Resistance(EssaiCount - 1).dateEssai = .Rows(i).Item("dateEssai")
                                MySonde.Resistance(EssaiCount - 1).difPresEauAir = .Rows(i).Item("difPresEauAir")
                                MySonde.Resistance(EssaiCount - 1).hauteurCpv = .Rows(i).Item("hauteurCpv")
                                'MySonde.Resistance(EssaiCount - 1).MesureCount = .Rows(i).Item("MesureCount")
                                MySonde.Resistance(EssaiCount - 1).A0 = .Rows(i).Item("a0")

                                MySonde.Resistance(EssaiCount - 1).A1 = .Rows(i).Item("a1")
                                MySonde.Resistance(EssaiCount - 1).A2 = .Rows(i).Item("a2")
                                MySonde.Resistance(EssaiCount - 1).A3 = .Rows(i).Item("a3")
                                ResistanceCount = EssaiCount
                            End If
                        Next i
                    End With
                Case EssaiCalib.Expansion
                    With MaListeSondes.Tables("LesEssaisExpansionPropre")
                        For i = 0 To .Rows.Count - 1    ' On lit l'ensemble de la Tables "LesEssaisExpansionPropre"
                            If .Rows(i).Item("refSonde").ToString = RefSonde Then   ' On a trouvé un Essai_Resistance lié à RefSonde
                                If (Expansion Is Nothing) Then ReDim MySonde.Expansion(0)
                                EssaiCount = EssaiCount + 1
                                ReDim Preserve MySonde.Expansion(EssaiCount)
                                MySonde.Expansion(EssaiCount - 1) = New Essai_Expansion
                                MySonde.Expansion(EssaiCount - 1).RefSonde = RefSonde
                                MySonde.Expansion(EssaiCount - 1).RefEssai = .Rows(i).Item("refEssai").ToString
                                MySonde.Expansion(EssaiCount - 1).nomEssai = .Rows(i).Item("nomEssai").ToString
                                MySonde.Expansion(EssaiCount - 1).dateEssai = .Rows(i).Item("dateEssai").ToString
                                If .Rows(i).Item("difPresEauAir").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).DifPresEauAir = .Rows(i).Item("difPresEauAir")
                                If .Rows(i).Item("hauteurCpv").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).HauteurCpv = .Rows(i).Item("hauteurCpv")
                                If .Rows(i).Item("Di").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Di = .Rows(i).Item("Di")
                                If .Rows(i).Item("coefficientA").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).coefficientA = .Rows(i).Item("coefficientA")
                                If .Rows(i).Item("coefficientB").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).coefficientB = .Rows(i).Item("coefficientB")

                                If .Rows(i).Item("Vc").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Vc = .Rows(i).Item("Vc")
                                If .Rows(i).Item("Vs").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).Vs = .Rows(i).Item("Vs")
                                If .Rows(i).Item("PrecisVs").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisVs = .Rows(i).Item("PrecisVs")
                                If .Rows(i).Item("PrecisVc").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisVc = .Rows(i).Item("PrecisVc")
                                If .Rows(i).Item("PrecisA").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisA = .Rows(i).Item("PrecisA")
                                If .Rows(i).Item("PrecisB").ToString <> "" Then MySonde.Expansion(EssaiCount - 1).PrecisB = .Rows(i).Item("PrecisB")
                                MySonde.ExpansionCount = EssaiCount
                            End If
                        Next i
                    End With
            End Select

        End Sub

        ' ******************************************************************************************************************
        ' Recherche dans les listes MySonde.Resistance() ou Expansion() de Probe de l'essai de rang Index
        ' ******************************************************************************************************************
        Public Function GetEssaiResistanceParIndex(ByVal Index As Integer, ByVal typeEssai As Integer) As Essai_Resistance

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
        Public Function GetEssaiExpansionParIndex(ByVal Index As Integer, ByVal typeEssai As Integer) As Essai_Expansion

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiExpansionParIndex = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiExpansionParIndex = Nothing
                Case EssaiCalib.Expansion
                    If Not (Expansion Is Nothing) And Index < ExpansionCount Then
                        GetEssaiExpansionParIndex = MySonde.Expansion(Index)
                    Else
                        GetEssaiExpansionParIndex = Nothing
                    End If
            End Select

        End Function

        ' Recherche dans les listes Resistance() ou Expansion() de Me de l'essai de rang Index
        Public Function GetEssaiExpansionParRef(ByVal Ref As String, ByVal typeEssai As Integer) As Essai_Expansion
            Dim i As Integer

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiExpansionParRef = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    GetEssaiExpansionParRef = Nothing
                Case EssaiCalib.Expansion
                    If Not (MySonde.Expansion Is Nothing) Then

                        'If MyListeSondes.ExpansionCount <> MySonde.Expansion.Length Then
                        '    MessageBox.Show("Incohérence entre ExpansionCount et la taille du tableau.")
                        '    Exit Function ' ou Exit Function selon le contexte
                        'End If

                        For i = 0 To MySonde.Expansion.Length - 2
                            'If MyListeSondes.ListeSondes(i) Is Nothing Then
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
        Public Function GetEssaiResistanceParRef(ByVal Ref As String, ByVal typeEssai As Integer) As Essai_Resistance
            Dim i As Integer

            ' Recherche de l'essai par son numéro d'index
            ' Index = 0 correspond au premier essai de rang 1
            GetEssaiResistanceParRef = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    If Not (MySonde.Resistance Is Nothing) Then
                        For i = 0 To MySonde.ResistanceCount - 1
                            If MySonde.Resistance(i).refEssai = Ref Then GetEssaiResistanceParRef = MySonde.Resistance(i)
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
                    GetEssaiParRef = New Essai_Resistance
                    Nb = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count
                    If Not (MaListeSondes.Tables("LesEssaisResistancePropre") Is Nothing) Then
                        For index = 0 To Nb - 1
                            If MaListeSondes.Tables("LesEssaisResistancePropre").Rows(index).Item("RefEssai").ToString = RefNom Then Exit For
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
                        With MaListeSondes.Tables("LesEssaisResistancePropre").Rows(index)
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
                    GetEssaiParRef = New Essai_Expansion
                    Nb = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count
                    If Not (MaListeSondes.Tables("LesEssaisExpansionPropre") Is Nothing) Then
                        For index = 0 To Nb - 1
                            If MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(index).Item("RefEssai").ToString = RefNom Then
                                'GetEssaiParRef = Expansion(index)
                                With MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(index)
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
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSondes
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    nb = MySonde.ExpansionCount
                Case EssaiCalib.Resistance
                    nb = MySonde.ResistanceCount
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        'If (MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                        If (MySonde.GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                    Case EssaiCalib.Resistance
                        'If (MaListeSondes.Tables("LesEssaisResistancePropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
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
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSondes
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    nb = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count
                'nb = MySonde.ExpansionCount
                Case EssaiCalib.Resistance
                    nb = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count
                    'nb = MySonde.ResistanceCount
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        If (MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(i).RowState <> DataRowState.Deleted) Then
                            If (MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
                            'If (Me.Expansion(i).GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                        End If
                    Case EssaiCalib.Resistance
                        If (MaListeSondes.Tables("LesEssaisResistancePropre").Rows(i).RowState <> DataRowState.Deleted) Then
                            If (MaListeSondes.Tables("LesEssaisResistancePropre").Rows(i).Item("RefEssai").ToString = ref) Then Exit Do
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
            ' dans la Table "LesEssaisExpansionPropre" ou "LesEssaisResistancePropre" de MaListeSondes
            Dim i As Integer
            Dim nb As Integer

            Select Case typeEssai
                Case EssaiCalib.Expansion
                    'nb = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count
                    nb = MySonde.ExpansionCount
                Case EssaiCalib.Resistance
                    'nb = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count
                    nb = MySonde.ResistanceCount
            End Select

            Do While (i < nb)
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        If (MySonde.Expansion(i).RefEssai.ToString = ref) Then Exit Do
                    'If (Me.Expansion(i).GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                    Case EssaiCalib.Resistance
                        If (MySonde.Resistance(i).refEssai.ToString = ref) Then Exit Do
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
                    GetEssaiCount = MySonde.ResistanceCount

                Case EssaiCalib.Expansion
                    GetEssaiCount = MySonde.ExpansionCount

            End Select
        End Function

        Public Sub SetRefSonde(ByVal refSond As String)
            refSonde = refSond

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeSonde(ByVal typeSond As String)
            typeSonde = Int(Valeurs(typeSond))

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeDeformee(ByVal typeDeforme As String)
            typeDeformee = Valeurs(typeDeforme)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetTypeContrainte(ByVal typeContraint As String)
            typeContrainte = Valeurs(typeContraint)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub Setlt(ByVal longTotal As Single)
            Lt = longTotal

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
            longInjectePompage = longInjectePompag

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
            epTubeFendu = epaisTubeFend
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
                    If indice > MySonde.ResistanceCount - 1 Then Exit Function

                    ' Redimensionnement du tableau dynamique de Essais
                    '            ReDim Preserve Resistance(ResistanceCount - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Resistance(MySonde.ResistanceCount - 1) = Essai

                Case EssaiCalib.Expansion
                    ' Déclaration d'un essai de résistance propre
                    ' Incrémentation du compteur d'essais de résistance propre
                    If indice > MySonde.ExpansionCount - 1 Then Exit Function


                    ' Redimensionnement du tableau dynamique de Essais
                    '            ReDim Preserve Expansion(ExpansionCount - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Expansion(ExpansionCount - 1) = Essai

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

        Public Sub SetMySonde(ByRef Sonde As Probe)
            MySonde = Sonde
        End Sub


        Public Sub SetMyListe(ByRef liste As ListeSondes)
            MyListe = liste
        End Sub
        ' Traitement de l'évènement IHaveBeenModified
        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                IsSaved = False
                Call MyListeSondes.IHaveBeenModified()

                ' Suppression de l'évènement IHaveBeenModified
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub Show()
            Dim i As Integer
            'If MyFrmSonde Is Nothing Then MyFrmSonde = New FrmSonde()
            ' Recherche du premier essai d'expansion
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Item("RefSonde").ToString = MySonde.refSonde Then Exit For
                Next i
                If i < .Rows.Count Then
                    ' On a trouvé le premier essai d'expansion
                    MyExpansion = New Essai_Expansion
                    MyExpansion.RefSonde = .Rows(i).Item("RefSonde").ToString
                    MyExpansion.RefEssai = .Rows(i).Item("refEssai").ToString
                    MyExpansion.nomEssai = .Rows(i).Item("nomEssai").ToString
                    MyExpansion.dateEssai = .Rows(i).Item("dateEssai").ToString
                    MyExpansion.ClassPrecis = Valeurs(.Rows(i).Item("ClassPrecis").ToString)
                    MyExpansion.DifPresEauAir = Valeurs(.Rows(i).Item("difPresEauAir").ToString)
                    MyExpansion.HauteurCpv = Valeurs(.Rows(i).Item("hauteurCpv").ToString)
                    MyExpansion.Di = Valeurs(.Rows(i).Item("Di").ToString)
                    MyExpansion.coefficientA = Valeurs(.Rows(i).Item("coefficientA").ToString)
                    MyExpansion.coefficientB = Valeurs(.Rows(i).Item("coefficientB").ToString)
                    MyExpansion.Vs = Valeurs(.Rows(i).Item("Vs").ToString)
                    MyExpansion.Vc = Valeurs(.Rows(i).Item("Vc").ToString)
                    MyExpansion.EpMembrane = Valeurs(.Rows(i).Item("EpMembrane").ToString)
                    MyExpansion.PrEpMembrane = Valeurs(.Rows(i).Item("PrEpMembrane").ToString)
                    MyExpansion.Prp = Valeurs(.Rows(i).Item("Prp").ToString)
                    MyExpansion.PrV = Valeurs(.Rows(i).Item("PrV").ToString)
                    MyExpansion.PrecisVs = Valeurs(.Rows(i).Item("PrecisVs").ToString)
                    MyExpansion.PrecisVc = Valeurs(.Rows(i).Item("PrecisVc").ToString)
                    MyExpansion.PrecisA = Valeurs(.Rows(i).Item("PrecisA").ToString)
                    MyExpansion.PrecisB = Valeurs(.Rows(i).Item("PrecisB").ToString)
                    MyExpansion.ChargeMesures(MyExpansion.RefEssai)
                End If
            End With

            ' Recherche du premier essai de Résistance
            With MaListeSondes.Tables("LesEssaisResistancePropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Item("RefSonde").ToString = MySonde.refSonde Then Exit For
                Next i
                If i < .Rows.Count Then
                    ' On a trouvé le premier essai d'expansion
                    MyResistance = New Essai_Resistance
                    MyResistance.refSonde = .Rows(i).Item("RefSonde").ToString
                    MyResistance.refEssai = .Rows(i).Item("refEssai").ToString
                    MyResistance.ClassPrecis = Valeurs(.Rows(i).Item("ClassPrecis").ToString)
                    MyResistance.nomEssai = .Rows(i).Item("nomEssai").ToString
                    If .Rows(i).Item("dateEssai").ToString <> "" Then MyResistance.dateEssai = .Rows(i).Item("dateEssai")
                    MyResistance.difPresEauAir = Valeurs(.Rows(i).Item("difPresEauAir").ToString)
                    MyResistance.hauteurCpv = Valeurs(.Rows(i).Item("hauteurCpv").ToString)
                    '    MyResistance.MesureCount = Valeurs(.Rows(i).Item("MesureCount").ToString)
                    MyResistance.A0 = Valeurs(.Rows(i).Item("a0").ToString)
                    MyResistance.A1 = Valeurs(.Rows(i).Item("a1").ToString)
                    MyResistance.A2 = Valeurs(.Rows(i).Item("a2").ToString)
                    MyResistance.A3 = Valeurs(.Rows(i).Item("a3").ToString)
                End If
            End With

            ' recherche du premier essai de Resistance
            'If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
            '    MyFrmSonde = New FrmSonde()
            'End If

            ' Si le formulaire n'est pas visible, affichez-le en modal
            If Not m_FrmSondeIsShowing Then
                m_FrmSondeIsShowing = True
                'If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
                '    MyFrmSonde = New FrmSonde()
                '    End IfIf MyFrmSonde IsNot Nothing AndAlso Not MyFrmSonde.Visible Then
                If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
                    MyFrmSonde = New FrmSonde() ' Instanciation si nécessaire
                End If
                If MyFrmSonde IsNot Nothing AndAlso Not MyFrmSonde.Visible Then
                    Debug.WriteLine("Appel de ShowDialog() depuis : " & New System.Diagnostics.StackTrace().ToString())
                    MyFrmSonde.ShowDialog()
                End If
                m_FrmSondeIsShowing = False
            End If
        End Sub

        Public Sub MaskSonde()
            MyFrmSonde.Hide()
        End Sub

        Public Sub CloseEssaiForage()
            Call MaskSonde()
        End Sub

        Public Sub CloseListeSondes()
            Call MaskSonde()
        End Sub

        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        Public Sub New()

            ' Création de la feuille de sonde
            If MyFenetreSonde Is Nothing Then
                MyFenetreSonde = New FrmSonde
            End If

            ' Enregistrement de la référence de la sonde auprès de sa feuille
            Call MyFenetreSonde.SetMySonde(Me)

            'Initialisation des paramètres dimensionnels de la sonde
            refSonde = "RefSonde"
            typeSonde = TypeDeSonde.SoupleTypeE
            typeDeformee = TypeDeDeformee.BallonRugby
            typeContrainte = TypeDeContrainte.Moyenne
            Lt = 450
            Lg = 120
            Ls = 210
            longInjectePompage = 700
            Vs = 650
            Ds = 60
            EpMembraneGarde = 5
            EpMembraneMesure = 1
            EpInitMembrane = 6
            epTubeFendu = 10
            Pm = 50
        End Sub


        Public Function NouvelEssai(ByVal typeEssai As Integer) As Object

            NouvelEssai = Nothing
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Déclaration d'un essai de résistance propre
                    If MyResistance Is Nothing Then
                        MyResistance = New Essai_Resistance
                    End If

                    ' Incrémentation du compteur d'essais de résistance propre
                    ResistanceCount = ResistanceCount + 1

                    ' Redimensionnement du tableau dynamique de Essais
                    ReDim Preserve MySonde.Resistance(ResistanceCount - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Resistance(ResistanceCount - 1) = New Essai_Resistance

                    MyResistance = MySonde.Resistance(ResistanceCount - 1)

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
                        MyExpansion = New Essai_Expansion
                    End If

                    ' Incrémentation du compteur d'essais de résistance propre
                    MySonde.ExpansionCount = MySonde.ExpansionCount + 1

                    ' Redimensionnement du tableau dynamique de Essais
                    ReDim Preserve MySonde.Expansion(ExpansionCount - 1)

                    ' Enregistrement du nouveau Essai dans le tableau d'essais
                    MySonde.Expansion(ExpansionCount - 1) = New Essai_Expansion

                    MyExpansion = MySonde.Expansion(ExpansionCount - 1)

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
                    With MaListeSondes.Tables("LesEssaisResistancePropre")
                        .Rows(i).Delete()
                    End With

                Case EssaiCalib.Expansion
                    ' Effaçage de l'Essai
                    ' Effaçage de l'Essai
                    With MaListeSondes.Tables("LesEssaisExpansionPropre")
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
                    For J = Index To ResistanceCount - 2
                        MySonde.Resistance(J) = MySonde.Resistance(J + 1)
                    Next

                    ' Redimensionnement du tableau ListEssais
                    If MySonde.ResistanceCount > 1 Then
                        ReDim Preserve MySonde.Resistance(ResistanceCount - 2)
                    Else
                        ReDim MySonde.Resistance(0)
                    End If

                    ' Décrémentation du compteur de Essais
                    MySonde.ResistanceCount = MySonde.ResistanceCount - 1

                Case EssaiCalib.Expansion
                    ' Effaçage de l'Essai
                    If Index = 0 Then Index = ExpansionCount
                    MySonde.Expansion(Index) = Nothing

                    ' Décalage des Essais suivant d'une case
                    For J = Index To ExpansionCount - 2
                        MySonde.Expansion(J) = MySonde.Expansion(J + 1)
                    Next

                    ' Redimensionnement du tableau ListEssais
                    If ExpansionCount > 1 Then
                        ReDim Preserve MySonde.Expansion(ExpansionCount - 2)
                    Else
                        ReDim MySonde.Expansion(0)
                    End If

                    ' Décrémentation du compteur de Essais
                    MySonde.ExpansionCount = MySonde.ExpansionCount - 1

            End Select

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub


        Public Sub AfficheDansTab(ByRef Table As DataGridView, ByVal typeessai As Integer)

            ' Remplissage de l'enregistrement
            Dim i, taille As Integer
            Dim tabEssais()

            ' Définition des colonnes
            With Table
                .ColumnCount = 3
                .ColumnHeadersVisible = True
                Select Case Lang
                    Case 0
                        .Columns(0).HeaderText = "Ref. Test"
                        .Columns(1).HeaderText = "Name Test"
                        .Columns(2).HeaderText = "Date Test"
                    Case 1
                        .Columns(0).HeaderText = "Ref. Essai"
                        .Columns(1).HeaderText = "Nom Essai"
                        .Columns(2).HeaderText = "Date Essai"
                End Select
            End With

            ' affectation des données à TabEssais
            Select Case typeessai
                Case EssaiCalib.Expansion
                    taille = ExpansionCount
                    ReDim tabEssais(taille)
                    tabEssais = MySonde.Expansion

                    ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
                    With Table
                        .Rows.Clear()
                        For i = 0 To taille - 1
                            .Rows.Add()
                            .Rows(i).Cells(0).Value = tabEssais(i).GetRefEssai
                            .Rows(i).Cells(1).Value = tabEssais(i).GetNomEssai
                            .Rows(i).Cells(2).Value = tabEssais(i).GetDateEssai
                        Next i
                    End With
                Case EssaiCalib.Resistance
                    taille = ResistanceCount
                    ReDim tabEssais(taille)
                    tabEssais = MySonde.Resistance

                    ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
                    With Table
                        .Rows.Clear()
                        For i = 0 To taille - 1
                            .Rows.Add()
                            .Rows(i).Cells(0).Value = tabEssais(i).GetRefEssai
                            .Rows(i).Cells(1).Value = tabEssais(i).GetNomEssai
                            .Rows(i).Cells(2).Value = tabEssais(i).GetDateEssai
                        Next i
                    End With
            End Select

            With Table


            End With

        End Sub

        Public Sub AfficheEssaiDansCombo(ByRef Combo As System.Windows.Forms.ComboBox, ByVal typeEssai As Integer)

            ' Remplissage de l'enregistrement
            Dim i As Integer
            Dim essai As Object
            essai = Nothing
            Combo.Items.Clear()
            For i = 0 To GetEssaiCount(typeEssai) - 1
                Select Case typeEssai
                    Case EssaiCalib.Expansion
                        essai = GetEssaiExpansionParIndex(i, typeEssai)
                    Case EssaiCalib.Resistance
                        essai = GetEssaiResistanceParIndex(i, typeEssai)
                End Select
                Combo.Items.Add(essai.GetRefEssai & " - " & essai.GetDateEssai)
            Next i
        End Sub

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
            Do While Not MyListeSondes.IsRefEssaiUnique(ref & " " & k, typeEssai)
                k = k + 1
            Loop
            GiveMeANewRefEssai = ref & " " & k

        End Function

        Public Sub CalculeVol()
            Dim E As Single
            If typeDeformee = TypeDeDeformee.BallonRugby Then
                E = 0
            Else
                E = epTubeFendu
            End If
            Vs = Math.Round(0.001 * Pi * Ls * (Ds / 2 - EpMembraneGarde - EpMembraneMesure - E) ^ 2, 1)
        End Sub

        Public Sub CalculeVolNF()
            Dim E, Di As Single

            If typeDeformee = TypeDeDeformee.BallonRugby Then
                E = 0
            Else
                E = epTubeFendu
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
                MyExpansion = MyEssai.MaExpansion
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