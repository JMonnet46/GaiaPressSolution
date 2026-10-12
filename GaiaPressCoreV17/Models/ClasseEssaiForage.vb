Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Controllers
Imports GaiaPressCoreV17.Services

Namespace GaiaPressCoreV17.Models

    Public Class Essai_Forage

        '===============================================================
        ' Propriétés de base
        '===============================================================

        ' Définition de la clef primaire de la Table LesEssaisForage
        <Key>
        Public Property refEssai As String
        Public Property RefChantier As String
        Public Property RefForage As String
        Public Overridable Property MySol As Sol
        <ForeignKey(" refSonde")>
        Public Overridable Property MySonde As Probe
        Public Property MaResistance As EssaiResistance
        Public Property MaExpansion As EssaiExpansion
        Public Property MonEssai As Essai_Forage
        'Public Property MyEssaiForage As Essai_Forage
        Public Property refSonde As String
        Public Property refEssaiResistancePropre As String
        Public Property refEssaiExpansionPropre As String
        Public Property refEssaiPerteDeCharge As String
        Public Property RefSol As String
        Public Property nomEssai As String
        Public Property dateEssai As String
        Public Property heureEssai As String
        Public Property Enregistreur As String
        Public Property Operateur As String
        Public Property VolSonde As Single
        Public Property ClassPrecis As Integer
        Public Property PrecisVolSonde As Single
        Public Property difPresEauAir As Single
        Public Property hauteurCpv As Single
        Public Property presP0 As Single
        Public Property presFluage As Single
        Public Property presLimite As Single
        Public Property presLimiteNF As Single
        Public Property presLimiteTheo As Single
        Public Property PrecisVolume As Single
        Public Property PrecisPression As Single
        Public Property poidsVol As Single
        Public Property poidsVolDejauge As Single
        Public Property coefPoisson As Single
        Public Property modPressio As Single
        Public Property modPressioNf As Single
        Public Property modElastique As Single
        Public Property modElastiqueNf As Single
        Public Property Cohesion As Single
        Public Property Frottement As Single
        Public Property FrottementIntergranulaire
        Public Property U0 As Single
        Public Property profNappe As Single
        Public Property profSonde As Single
        Public Property coteZeroGeomeca As Single

        ' ======= Propriétés de Liste ===========
        Public Property Mesures As List(Of MesureEssaiForage)

        ' ======= Propriété de modification ===========
        Public Property EventsSuspendus As Boolean

        '========= Propriétés supplémentaires =======================
        Public Property Pe As Single

        Public Property Ie As Integer
        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' === PROPRIÉTÉS DE NAVIGATION ===
        '<InverseProperty("Essai_Forage")>

        'Public Overridable Property  Mesures As ICollection(Of MesureEssaiForage) = New List(Of MesureEssaiForage)

        ' === CONSTRUCTEURS ===
        Public Sub New()
            RefChantier = MyChantier.GetRefChantier
            RefForage = MyForage.GetRefForage
            MySol = New Sol
            MySonde = New Probe
            MySonde = New Probe
            MaResistance = MySonde.MaResistance
            MaExpansion = MySonde.MaExpansion
            refSonde = MySonde.RefSonde
            refEssai = MyForage.GiveMeANewRefEssai()
            nomEssai = MyForage.GiveMeANewNomEssai()
            dateEssai = Date.Today.ToString("yyyy-MM-dd")
            heureEssai = DateTime.Now.ToString("HH:mm:ss")
            Mesures = New List(Of MesureEssaiForage)
            MonEssai = New Essai_Forage
            refEssaiResistancePropre = ""
            refEssaiExpansionPropre = ""
            refEssaiPerteDeCharge = Nothing
            RefSol = Nothing
            nomEssai = ""
            dateEssai = Now
            heureEssai = Now.ToLocalTime
            Enregistreur = ""
            Operateur = ""
            VolSonde = 0
            ClassPrecis = 0
            PrecisVolSonde = 0
            difPresEauAir = 0
            hauteurCpv = 0
            presP0 = 0
            presFluage = 0
            presLimite = 0
            presLimiteNF = 0
            presLimiteTheo = 0
            PrecisVolume = 0
            PrecisPression = 0
            poidsVol = 0
            poidsVolDejauge = 0
            coefPoisson = 0
            modPressio = 0
            modPressioNf = 0
            modElastique = 0
            modElastiqueNf = 0
            Cohesion = 0
            Frottement = 0
            FrottementIntergranulaire = 0
            U0 = 0
            profNappe = 0
            profSonde = 0
            coteZeroGeomeca = 0

        End Sub

        Public Function IsRefUnique(ByVal ref As String) As Boolean
            ' Vérification de l'unicité d'une référence
            ' Donne une référence unique au nouveau forage
            Dim i As Integer
            Dim unique As Boolean

            '    MsgBox "IsRefUnique"

            unique = True

            ' i est le Numéro du Forage à vérifier
            Do While (i < MyListeEssaiForageSession.Count) And unique
                unique = (MyListeEssaiForageSession(i).RefForage <> ref)
                If unique Then Exit Do
                i = i + 1
            Loop
            If MyEssaiForage Is Nothing Then
                MyEssaiForage = New Essai_Forage
                'ReDim Preserve MyEssaiForage. ListEssais(i)
                MyEssaiForage = MyChantier.ListEssaisForages(i)
            End If

            IsRefUnique = unique
        End Function

        Public Function IsNomUnique(ByVal Nom As String) As Boolean
            ' Vérification de l'unicité d'une référence
            ' Donne une référence unique au nouveau forage
            Dim i As Integer
            Dim unique As Boolean

            '    MsgBox "IsNomUnique"

            unique = True

            ' i est le Numéro du Forage à vérifier
            Do While (i < MyListeEssaiForageSession.Count) And unique
                unique = (MyListeEssaiForageSession(i).nomEssai <> Nom)
                If unique Then Exit Do
                i = i + 1
            Loop
            If MyEssaiForage Is Nothing Then
                MyEssaiForage = New Essai_Forage
                'ReDim Preserve MyEssaiForage. ListEssais(i)
                MyEssaiForage = MyChantier.ListEssaisForages(i)
            End If

            IsNomUnique = unique
        End Function

        Public Function GetMesureCount() As Integer
            GetMesureCount = Mesures.Count
        End Function

        Public Function GetRefSonde() As String
            GetRefSonde = refSonde
        End Function

        Public Function GetPrEpMembraneGarde() As Single
            GetPrEpMembraneGarde = MySonde.GetPrEpMembraneGarde
        End Function

        Public Function GetPrEpMembraneMesure() As Single
            GetPrEpMembraneMesure = MySonde.GetPrEpMembraneMesure
        End Function

        Public Function GetPrEpTubeFendu() As Single
            GetPrEpTubeFendu = MySonde.GetPrEpTubeFendu
        End Function

        Public Function GetTypeSonde() As Integer
            GetTypeSonde = MySonde.GetTypeSonde
        End Function

        Public Function GetNomTypeSonde() As String
            GetNomTypeSonde = 0
            Select Case MySonde.GetTypeSonde
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
            GetTypeDeformee = MySonde.GetTypeDeformee
        End Function

        Public Function GetNomTypeDeformee() As String
            GetNomTypeDeformee = 0
            Select Case MySonde.GetTypeDeformee
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
            GetTypeContrainte = MySonde.GetTypeContrainte
        End Function

        Public Function GetNomTypeContrainte() As String
            GetNomTypeContrainte = ""
            Select Case MySonde.GetTypeContrainte
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
            GetLongInjectePompage = MySonde.GetLongInjectePompage
        End Function

        Public Function GetVc() As Single
            GetVc = MySonde.GetVc
        End Function

        Public Function GetVs() As Single
            GetVs = VolSonde
        End Function

        Public Function Getds() As Single
            Getds = Ds
        End Function

        Public Function GetEpMembraneGarde() As Single
            GetEpMembraneGarde = MySonde.GetEpMembraneGarde
        End Function

        Public Function GetEpMembraneMesure() As Single
            GetEpMembraneMesure = MySonde.GetEpMembraneMesure
        End Function

        Public Function GetEpaisTubeFendu() As Single
            GetEpaisTubeFendu = MySonde.GetEpaisTubeFendu
        End Function

        'Public Function GetEssaiForageCoefResistance() As CoefResistance
        '    With GetEssaiForageCoefResistance
        '        .A0 = MaResistance.A0
        '        .A1 = MaResistance.A1
        '        .A2 = MaResistance.A2
        '        .A3 = MaResistance.A3
        '    End With
        'End Function

        Public Function GetPrEpMembraneCentre() As Single
            GetPrEpMembraneCentre = MySonde.GetPrEpMembraneCentre
        End Function

        Public Function GetPm() As Integer
            GetPm = MySonde.GetPm
        End Function

        Public Function GetClassPrecis() As Integer
            GetClassPrecis = MySonde.GetClassPrecis
        End Function

        Public Function GetPrecisVolSonde() As Single
            GetPrecisVolSonde = MySonde.GetPrecisVolSonde
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
                            GetEssai.ClassPrecis = .Rows(result).Item("ClassPrecis").Tointeger
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
            GetMyliste = MySonde.GetMyliste
        End Function

        Public Function GetIsSaved() As Boolean
            GetIsSaved = MySonde.GetIsSaved
        End Function

        '' ******************************************************************************************************************
        '' Chargement dans Resistance ou Expansion des essais de Probe
        '' Recherche d'un EssaiResistance ou EssaiExpansion dans ListeExpansion ou ListeResistance par RefSonde
        '' Construction de la Liste des Resistance ou de la Liste des Expansions d'une Sonde
        '' ******************************************************************************************************************
        'Public Sub SetListeResistanceExpansionParRefSonde(ByVal RefSonde As String, ByVal typeEssai As Integer)
        '    Dim i, EssaiCount As Integer
        '    Dim Essai_Expansion As EssaiExpansion
        '    Dim Essai_Resistance As EssaiResistance

        '    ' Recherche de l'essai par son numéro d'index
        '    EssaiCount = 0
        '    MySonde = Me.MySonde
        '    ' Index = 0 correspond au premier essai de rang 1
        '    Select Case typeEssai

        '        ' Construction de la liste des Resistances Propres par Sonde
        '        Case EssaiCalib.Resistance
        '            With MaListeSonde.Tables("LesEssaisResistancePropre")
        '                EssaiCount = MaListeSonde.Tables("LesEssaisResistancePropre").Rows.Count
        '                For i = 0 To EssaiCount - 1    ' On lit l'ensemble de la Tables "LesEssaisResistancePropre"

        '                    Essai_Resistance = MyListeEssaiForageSession.GetEssaiResistanceParRefSonde(RefSonde, i)
        '                    'MaListeSonde.Tables("LesEssaisExpansionPropre")
        '                    If Essai_Resistance IsNot (Nothing) Then
        '                        MySonde.Resistance(i) = New EssaiResistance
        '                        MySonde.Resistance(i) = Essai_Resistance
        '                    End If
        '                Next i
        '            End With

        '        ' Construction de la liste des Expansions Propres par Sonde
        '        Case EssaiCalib.Expansion

        '            EssaiCount = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
        '            With MaListeSonde.Tables("LesEssaisExpansionPropre")
        '                For i = 0 To EssaiCount - 1    ' On lit l'ensemble de la Tables "LesEssaisExpansionPropre"
        '                    If .Rows(i).Item("refSonde").ToString = RefSonde Then   ' On a trouvé un EssaiResistance lié à RefSonde
        '                        Essai_Expansion = MyListeEssaiForageSession.GetEssaiExpansionParRefSonde(RefSonde, i)
        '                        'MaListeSonde.Tables("LesEssaisExpansionPropre")
        '                        If Essai_Expansion IsNot (Nothing) Then
        '                            MySonde.Expansion(i) = New EssaiExpansion
        '                            MySonde.Expansion(i) = Essai_Expansion
        '                        End If 'MySonde.Expansion.Count = EssaiCount
        '                    End If
        '                Next i
        '            End With
        '    End Select

        'End Sub

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
                    If Not (MyExpansion Is Nothing) And Index < MySonde.Expansion.Count Then
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
                            If MySonde.Expansion(i).GetRefEssai = Ref Then
                                GetEssaiExpansionParRef = MySonde.Expansion(i)
                                Exit For
                            End If
                            If MySonde.Expansion(i).GetRefEssai = Ref Then GetEssaiExpansionParRef = MySonde.Expansion(i)
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
                            If MySonde.Resistance(i).GetRefEssai = Ref Then GetEssaiResistanceParRef = MySonde.Resistance(i)
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
                            GetEssaiParRef.ClassPrecis = .Item("ClassPrecis").Tointeger
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
                                    GetEssaiParRef.ClassPrecis = .Item("ClassPrecis")
                                    GetEssaiParRef.nomEssai = .Item("nomEssai").ToString
                                    GetEssaiParRef.dateEssai = .Item("dateEssai").ToString
                                    GetEssaiParRef.difPresEauAir = .Item("difPresEauAir")
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
                        If (MySonde.Expansion(i).GetRefEssai = ref) Then Exit Do
                    'If (Me.Expansion(i).GetEssaiExpansionParIndex(i, typeEssai).GetRefEssai = ref) Then Exit Do
                    Case EssaiCalib.Resistance
                        If (MySonde.Resistance(i).GetRefEssai = ref) Then Exit Do
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
            refSonde = refSond

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SetTypeSonde(ByVal typeSond As String)
        '    typeSonde = Int(Valeurs(typeSond))

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetTypeDeformee(ByVal typeDeforme As String)
        '    typeDeformee = Valeurs(typeDeforme)

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetTypeContrainte(ByVal typeContraint As String)
        '    typeContrainte = Valeurs(typeContraint)

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

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

        'Public Sub SetLongInjectePompage(ByVal longInjectePompag As Single)
        '    longInjectePompage = longInjectePompag

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        Public Sub Setds(ByVal diamInitia As Single)
            Ds = diamInitia
            'Call CalculeVol()
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SetEpMembraneGarde(ByVal epaisMenbrane As Single)
        '    EpMembraneGarde = epaisMenbrane
        '    'Call CalculeVol()
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetEpMembraneCentre(ByVal EpMembraneMesur As Single)
        '    EpMembraneMesure = EpMembraneMesur

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetEpMembraneMesure(ByVal EpMembraneMes As Single)
        '    EpMembraneMesure = EpMembraneMes
        '    'Call CalculeVol()
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetEpMembraneCentre(ByVal EpMembraneCen As Single)
        '    EpMembraneCentr = EpMembraneCen
        '    'Call CalculeVol()
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetEpaisTubeFendu(ByVal epaisTubeFend As Single)
        '    epTubeFendu = epaisTubeFend
        '    'Call CalculeVol()
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetVc(ByVal Volume As Single)

        '    'Call CalculeVol
        '    Vc = Volume
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        Public Sub SetVs(ByVal Volume As Single)

            'Call CalculeVol
            VolSonde = Volume
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub
        'Public Sub SetPm(ByVal p As Integer)
        '    Pm = p

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetPrecisVolSonde(ByVal p As Single)
        '    PrecisVolSonde = p

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetPrDiam(ByVal Pr As Single)
        '    PrDiam = Pr

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetPrEpMembraneGarde(ByVal Pr As Single)
        '    PrEpMembraneGarde = Pr

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetPrEpTubeFendu(ByVal Pr As Single)
        '    PrEpTubeFendu = Pr

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        'Public Sub SetPrEpMembraneMesure(ByVal Pr As Single)
        '    PrEpMembraneMesure = Pr

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub


        'Public Sub SetClassPrecis(ByVal p As Integer)
        '    ClassPrecis = p

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()
        'End Sub

        Public Sub NouvelleMesure(ByVal nouvelleSerie As Boolean, ByVal indice As Integer, ByVal indiceMesure As Integer, ByVal pression As Single, ByVal volume15 As Single, ByVal volume30 As Single, ByVal volume60 As Single, ByVal contrainteRadiale As Single, ByVal contrainteVerticale As Single, ByVal deformation As Single, ByVal pnf As Single, ByVal vnf As Single, ByVal qi As Single, ByVal pi As Single, ByVal qp As Single, ByVal pp As Single)
            ' Ajoute une mesure dans la collection réellement portée par Essai_Forage.
            ' nouvelleSerie est conservé dans la signature historique de l'importeur.
            If Mesures Is Nothing Then Mesures = New List(Of MesureEssaiForage)

            Dim mesure As New MesureEssaiForage(refEssai, indiceMesure)
            mesure.Ind = indice
            mesure.Pression = pression
            mesure.Volume15 = volume15
            mesure.Volume30 = volume30
            mesure.Volume60 = volume60
            mesure.ContrainteRadiale = contrainteRadiale
            mesure.ContrainteVerticale = contrainteVerticale
            mesure.Deformation = deformation
            mesure.PNF = pnf
            mesure.VNF = vnf
            mesure.Qi = qi
            mesure.Pi = pi
            mesure.Qp = qp
            mesure.Pp = pp
            mesure.Essai_Forage = Me
            Mesures.Add(mesure)
            IHaveBeenModified()
        End Sub

        Public Function SetEssai(ByVal typeEssai As Integer, ByRef Essai As Object, ByVal indice As Integer) As Object

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
                    MySonde.Expansion(MySonde.Expansion.Count - 1) = Essai

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


        Public Sub SetMyListe(ByRef liste As ListeSonde)
            MyListeSonde = liste
        End Sub

        ' Traitement de l'évènement IHaveBeenModified
        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not MyEssaiForage.EventsSuspendus Then
                IsSaved = False
                Call MyListeSonde.IHaveBeenModified()

                ' Suppression de l'évènement IHaveBeenModified
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub Show()
            Dim i As Integer
            'If MyFrmSonde Is Nothing Then MyFrmSonde = New FrmSonde()
            ' Recherche du premier essai d'expansion
            With MaListeSonde.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Item("RefSonde").ToString = MySonde.RefSonde Then Exit For
                Next i
                If i < .Rows.Count Then
                    ' On a trouvé le premier essai d'expansion
                    MyExpansion = New EssaiExpansion
                    MyExpansion.RefSonde = .Rows(i).Item("RefSonde").ToString
                    MyExpansion.RefEssai = .Rows(i).Item("refEssai").ToString
                    MyExpansion.nomEssai = .Rows(i).Item("nomEssai").ToString
                    MyExpansion.dateEssai = .Rows(i).Item("dateEssai").ToString
                    MyExpansion.ClassPrecis = Valeurs(.Rows(i).Item("ClassPrecis").ToString)
                    MyExpansion.difPresEauAir = Valeurs(.Rows(i).Item("difPresEauAir").ToString)
                    MyExpansion.hauteurCpv = Valeurs(.Rows(i).Item("hauteurCpv").ToString)
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
            With MaListeSonde.Tables("LesEssaisResistancePropre")
                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Item("RefSonde").ToString = MySonde.RefSonde Then Exit For
                Next i
                If i < .Rows.Count Then
                    ' On a trouvé le premier essai d'expansion
                    MyResistance = New EssaiResistance
                    MyResistance.RefSonde = .Rows(i).Item("RefSonde").ToString
                    MyResistance.RefEssai = .Rows(i).Item("refEssai").ToString
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

            ' ===============================================================================
            ' Code Obsolète
            '================================================================================

            '' recherche du premier essai de Resistance
            ''If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
            ''    MyFrmSonde = New FrmSonde()
            ''End If

            '' Si le formulaire n'est pas visible, affichez-le en modal
            'If Not m_FrmSondeIsShowing Then
            '    m_FrmSondeIsShowing = True
            '    'If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
            '    '    MyFrmSonde = New FrmSonde()
            '    '    End IfIf MyFrmSonde IsNot Nothing AndAlso Not MyFrmSonde.Visible Then
            '    If MyFrmSonde Is Nothing OrElse MyFrmSonde.IsDisposed Then
            '        MyFrmSonde = New FrmSonde() ' Instanciation si nécessaire
            '    End If
            '    If MyFrmSonde IsNot Nothing AndAlso Not MyFrmSonde.Visible Then
            '        Debug.WriteLine("Appel de ShowDialog() depuis : " & New System.Diagnostics.StackTrace().ToString())
            '        MyFrmSonde.ShowDialog()
            '    End If
            '    m_FrmSondeIsShowing = False
            'End If
        End Sub

        'Public Sub MaskSonde()
        '    MyFrmSonde.Hide()
        'End Sub

        'Public Sub CloseEssaiForage()
        '    Call MaskSonde()
        'End Sub

        'Public Sub CloseListeSonde()
        '    Call MaskSonde()
        'End Sub

        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        'Public Sub New()

        '    ' Création de la feuille de sonde
        '    If MyFenetreSonde Is Nothing Then
        '        MyFenetreSonde = New FrmSonde
        '    End If

        '    ' Enregistrement de la référence de la sonde auprès de sa feuille
        '    Call MyFenetreSonde.SetMySonde(Me)

        '    'Initialisation des paramètres dimensionnels de la sonde
        '    refSonde = "RefSonde"
        '    typeSonde = TypeDeSonde.SoupleTypeE
        '    typeDeformee = TypeDeDeformee.BallonRugby
        '    typeContrainte = TypeDeContrainte.Moyenne
        '    Lt = 450
        '    Lg = 120
        '    Ls = 210
        '    longInjectePompage = 700
        '    Vs = 650
        '    Ds = 60
        '    EpMembraneGarde = 5
        '    EpMembraneMesure = 1
        '    EpInitMembrane = 6
        '    epTubeFendu = 10
        '    Pm = 50
        'End Sub


        'Public Function NouvelEssai(ByVal typeEssai As Integer) As Object

        '    NouvelEssai = Nothing
        '    Select Case typeEssai
        '        Case EssaiCalib.Resistance
        '            ' Déclaration d'un essai de résistance propre
        '            If MyResistance Is Nothing Then
        '                MyResistance = New EssaiResistance
        '            End If

        '            ' Incrémentation du compteur d'essais de résistance propre
        '            'ResistanceCount = ResistanceCount + 1

        '            ' Redimensionnement du tableau dynamique de Essais
        '            ReDim Preserve MySonde.Resistance(Resistance.Count - 1)

        '            ' Enregistrement du nouveau Essai dans le tableau d'essais
        '            MySonde.Resistance(Resistance.Count - 1) = New EssaiResistance

        '            MyResistance = MySonde.Resistance(Resistance.Count - 1)

        '            ' Enregistrement de la sonde auprès de son nouvel Essai
        '            Call MyResistance.SetMySonde(Me)

        '            ' Attribution d'une référence au nouvel essai
        '            With MyResistance
        '                ' affectation de la nouvelle référence d'essai
        '                .SetRefEssai(GiveMeANewRefEssai(typeEssai))
        '            End With

        '            NouvelEssai = MyResistance

        '        Case EssaiCalib.Expansion
        '            ' Déclaration d'un essai de résistance propre
        '            If MyExpansion Is Nothing Then
        '                MyExpansion = New EssaiExpansion
        '            End If

        '            ' Incrémentation du compteur d'essais de résistance propre
        '            'MySonde.Expansion.Count = MySonde.Expansion.Count + 1

        '            ' Redimensionnement du tableau dynamique de Essais
        '            ReDim Preserve MySonde.Expansion(Expansion.Count - 1)

        '            ' Enregistrement du nouveau Essai dans le tableau d'essais
        '            MySonde.Expansion(Expansion.Count - 1) = New EssaiExpansion

        '            MyExpansion = MySonde.Expansion(Expansion.Count - 1)

        '            ' Enregistrement de la sonde auprès de son nouvel Essai
        '            'Call MyExpansion.SetMySonde(Me)

        '            ' Attribution d'une référence au nouvel essai
        '            With MyExpansion
        '                ' affectation de la nouvelle référence d'essai
        '                .SetRefEssai(GiveMeANewRefEssai(typeEssai))
        '            End With

        '            NouvelEssai = MyExpansion

        '    End Select

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()


        'End Function

        ' ******************************************************************************************************************
        ' Création d'un nouvel essai de résistance ou d'expansion pour la sonde actuelle
        ' ******************************************************************************************************************
        Public Function NouvelEssai(ByVal typeEssai As Integer) As Object
            Dim nouvel_Essai As Object = Nothing

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Créer un nouvel essai de résistance
                    Dim essaiResistance As New EssaiResistance()

                    ' Attribution d'une référence unique à l'essai
                    essaiResistance.SetRefEssai(GiveMeANewRefEssai(typeEssai))

                    ' Enregistrer la sonde auprès du nouvel essai
                    essaiResistance.SetMySonde(MySonde)

                    ' Ajouter l'essai à la liste globale
                    ModBibliotheque.MyListeEssaiResistanceSession.Add(essaiResistance)

                    ' Mettre à jour la référence locale
                    MyResistance = essaiResistance

                    ' Mettre à jour la liste de la sonde
                    If MySonde.Resistance Is Nothing Then
                        MySonde.Resistance = New List(Of EssaiResistance)()
                    End If
                    MySonde.Resistance.Add(essaiResistance)

                    NouvelEssai = essaiResistance

                Case EssaiCalib.Expansion
                    ' Créer un nouvel essai d'expansion
                    Dim essaiExpansion As New EssaiExpansion()

                    ' Attribution d'une référence unique à l'essai
                    essaiExpansion.SetRefEssai(GiveMeANewRefEssai(typeEssai))

                    ' Enregistrer la sonde auprès du nouvel essai
                    essaiExpansion.SetMySonde(MySonde)

                    ' Ajouter l'essai à la liste globale
                    ModBibliotheque.MyListeEssaiExpansionSession.Add(essaiExpansion)

                    ' Mettre à jour la référence locale
                    MyExpansion = essaiExpansion

                    ' Mettre à jour la liste de la sonde
                    If MySonde.Expansion Is Nothing Then
                        MySonde.Expansion = New List(Of EssaiExpansion)()
                    End If
                    MySonde.Expansion.Add(essaiExpansion)

                    NouvelEssai = essaiExpansion
            End Select

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

            Return NouvelEssai
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
            ' Suppression d'un essai de résistance ou d'expansion en utilisant les List(Of T)

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    ' Suppression de l'essai de résistance
                    If Index >= 0 AndAlso Index < MySonde.Resistance.Count Then
                        MySonde.Resistance.RemoveAt(Index)
                    End If

                Case EssaiCalib.Expansion
                    ' Suppression de l'essai d'expansion
                    If Index >= 0 AndAlso Index < MySonde.Expansion.Count Then
                        MySonde.Expansion.RemoveAt(Index)
                    End If
            End Select
        End Sub

        Public Function GiveMeANewRefEssai(ByVal typeEssai As Integer) As String
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
            Dim listeEssais As New ClasseListeEssaiForage()
            listeEssais.ListeEssaiForageSession = MyListeEssaiForageSession

            Do While Not listeEssais.IsRefEssaiUnique(ref & " " & k)
                k += 1
            Loop

            GiveMeANewRefEssai = ref & " " & k

        End Function

        Public Function GiveMeANewNomssai(ByVal typeEssai As Integer) As String
            ' Détermination d'une nouvelle référence pour un essai
            Dim k As Integer
            Dim Nom As String
            k = 0

            Nom = ""
            Select Case typeEssai
                Case EssaiCalib.Resistance
                    Nom = "Resistance"
                Case EssaiCalib.Expansion
                    Nom = "Expansion"
            End Select

            ' Recherche d'une nouvelle référence d'essai qui soit unique dans la liste des EssaisResistance ou des EssaisExpansion
            Dim listeEssais As New ClasseListeEssaiForage()
            listeEssais.ListeEssaiForageSession = MyListeEssaiForageSession

            Do While Not listeEssais.IsNomEssaiUnique(Nom & " " & k)
                k += 1
            Loop

            GiveMeANewNomssai = Nom & " " & k

        End Function


        Public Sub CalculeVol()
            Dim S As Single
            If MySonde.TypeDeformee = TypeDeDeformee.BallonRugby Then
                S = 0
            Else
                S = MySonde.EpTubeFendu
            End If
            'Vs = Math.Round(0.001 * Pi * Ls * (Ds / 2 - MySonde.EpMembraneGarde - MySonde.EpMembraneMesure - S) ^ 2, 1)
        End Sub

        Public Sub CalculeVolNF()
            Dim E, Di, Vs, Vc As Single
            Dim EssaiExpansion As EssaiExpansion

            If MySonde.TypeDeformee = TypeDeDeformee.BallonRugby Then
                E = 0
            Else
                E = MySonde.EpTubeFendu
            End If
            If MyExpansion IsNot Nothing Then
                Vc = MyExpansion.GetVc
                Di = MyExpansion.GetDi
                Vs = Math.Round(0.001 * Pi * MySonde.Ls * (Di / 2) ^ 2, 1) - Vc
                MyExpansion.SetVs(Vs)
                MySonde.MaExpansion = MyExpansion
                MySonde.Vs = Vs
                MySonde.PrecisVs = MyExpansion.PrV
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
                    Vc = MyExpansion.GetVc
                    Di = MyExpansion.GetDi
                    Vs = Math.Round(0.001 * Pi * Ls * (Di / 2) ^ 2, 1) - Vc
                    MySonde.Vs = Vs
                    'Vs = MySonde.Vs
                End If

            End If
        End Sub

        Public Sub UpdateMesureEssaiForage(ByVal indice As Integer, ByVal mesure As MesureEssaiForage)
            If indice >= 0 AndAlso indice < Me.Mesures.Count Then
                Me.Mesures(indice) = mesure
            Else
                Me.Mesures.Add(mesure)
            End If
        End Sub

        'Public Function GetMyEssaiForageCalibExpansion(Essai As EssaiCalib) As Object
        '    ' Exemple : Récupérer l'essai de forage calibré en fonction du type (Resistance, Expansion, etc.)
        '    ' À adapter selon votre logique métier
        '    Select Case typeEssai
        '        Case 0 ' expansion
        '            GetMyEssaiForageCalib = MaExpansion
        '        Case 1 ' resistance
        '            GetMyEssaiForageCalib = MaResistance
        '    End Select
        '    For Each essai In Me.ListeEssaiCalib
        '            If essai.TypeEssai = typeEssai Then
        '                Return essai
        '            End If
        '        Next
        '        Return Nothing
        'End Function
    End Class
End Namespace