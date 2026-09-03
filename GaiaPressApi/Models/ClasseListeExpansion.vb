Namespace GaiaPress.core.V17.Models

    Public Class ListeEssaisExpansion
        Inherits DbContext
        Public Property S_ListeExpansion As New List(Of Essai_Expansion)
        Public FichierListeExpansion As String
        'Public MyListeExpansion As FrmExpansion
        Public EventsSuspendus As Boolean

        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de Liste_Expansion
        ' ================================================================================
        ' Add un Sol

        Public Sub AddExpansion(EssaiExpansion As Essai_Expansion)
            Me.S_ListeExpansion.Add(EssaiExpansion)
            ' Lier le forage au chantier (si nécessaire)
            'EssaiExpansion.SetRefEssaiExpansion(EssaiExpansion.GetRefEssai)
        End Sub

        Public Function GetEssaiExpansion(refEssai As String) As Essai_Expansion
            For Each E As Essai_Expansion In Me.S_ListeExpansion
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssaiExpansion(refEssai As String)
            Dim EssaiADelete As Essai_Expansion = GetEssaiExpansion(refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.S_ListeExpansion.Remove(EssaiADelete)
            End If
        End Sub


        ' Mettre à jour un Essai
        Public Sub MettreAJourEssaiExpansion(refEssai As String, nouvelEssai As Essai_Expansion)
            Dim EssaiExpansion As Essai_Expansion = GetEssaiExpansion(refEssai)
            If EssaiExpansion IsNot Nothing Then
                EssaiExpansion = nouvelEssai
                'Sol.E_dateEssai = nouvelleDate
            End If
        End Sub

        'Public Shared Sub InitializeListeExpansion()
        '    If Not MyListeEssaisExpansion.IsSaved Then
        '        ListeExpansion.IsSaved = True
        '        If MyListeExpansion Is Nothing Then
        '            MyListeExpansion = New ListeExpansion
        '        End If
        '        IsInitializingExpansion = False
        '    End If
        'End Sub

        'Public Sub New()

        '    ' Création de la feuille de liste
        '    'If MyListeExpansion Is Nothing Then
        '    '    InitializeListeExpansion()
        '    'End If

        '    '' Enregistrement de la référence de la liste Sol auprès de sa feuille
        '    'Call MyListeExpansion.GiveMeANewRefSol()

        '    ' -----------------------------------------------------------------
        '    Dim Index As Integer

        '    ' Création de la feuille de liste
        '    If MyListeExpansion Is Nothing Then
        '        Index = 0
        '        MyListeExpansion = Me
        '        ' Initialize other properties or perform other setup tasks here
        '    Else
        '        If MySol Is Nothing Then
        '            MyListeExpansion = Nothing
        '            Exit Sub
        '        End If
        '        Index = GetIndexSol(MySol.GetNomSol)
        '        If Index >= 0 Then Call MyListeExpansion.SetSol(MySol, Index)
        '    End If

        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    ' Call MyListeSondes.SetSonde(MySonde, Index)
        '    ' -----------------------------------------------------

        'End Sub


        Public Sub CloseListeExpansion()

            ' fermeture de la Liste des Sondes (fermeture de sa fenêtre)
            'Call MyListeSondes.CloseChantier()

            ' Extinction des menus liés au chantier
            'Call ActivationMenus(False)

            ' Effacement de la référence vers le chantier
            MyListeEssaisExpansion = Nothing

            ' Nettoyage de l'arborescence
            'TreeViewArbo.Nodes.Clear()
        End Sub

        ' **********************************************************************
        ' Fonction d'écriture dans le fichier Access des Expansion
        ' **********************************************************************
        '        Public Function EnregistreListeExpansion(ByVal FichExpansion As String) As Boolean
        '            On Error GoTo errFind

        '            ' On crée une commande Builder pour renvoyer les données à la BD

        '            ' Création de l'objetCommand nécessaire à l'enregistrement

        '            ' Effacement des commandes précédentes

        '            ' Mise à jour de la BD
        '            iTable = 4
        '            ObjetDataExpansion = MaListeExpansion.Tables("LesExpansion")
        '            MsgBox("Enregistrement des Expansion réussi")

        '            EnregistreExpansion = True
        '            IsSaved = True

        '            Exit Function

        'errFind:
        '            MsgBox(Err.Description, vbExclamation, Err.Source)
        '            Err.Clear()
        '            'FrmTable.Show()
        '            EnregistreExpansion = False

        '        End Function


        'Public Sub CloseListeExpansion()
        '    Call MaskListeExpansion()
        '    Dim i As Integer
        '    For i = 0 To SolCount - 1
        '        ListeExpansion(i).CloseListeExpansion()
        '        ListeExpansion(i) = Nothing
        '    Next
        'End Sub

        'Public Sub AfficheDansTab(ByRef Table As DataGridView)
        '    Dim Taille As Integer

        '    ' Définition des colonnes
        '    With Table
        '        .ColumnCount = 5
        '        .ColumnHeadersVisible = True
        '        Select Case Lang
        '            Case 0
        '                .Columns(0).HeaderText = "Reference"
        '                .Columns(1).HeaderText = "Name Soil"
        '                .Columns(2).HeaderText = "Geographic Area"
        '                .Columns(3).HeaderText = "Family"
        '                .Columns(4).HeaderText = "Interparticle friction"
        '            Case 1
        '                .Columns(0).HeaderText = "Référence"
        '                .Columns(1).HeaderText = "Nom Sol"
        '                .Columns(2).HeaderText = "Zone géographique"
        '                .Columns(3).HeaderText = "Famille"
        '                .Columns(4).HeaderText = "Frottement intergranulaire"
        '        End Select
        '    End With

        '    Taille = MyListeExpansion.GetSolCount
        '    With Table
        '        .Rows.Clear()
        '        ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
        '        For i = 0 To Taille - 1
        '            .Rows.Add()
        '            .Rows(i).Cells(0).Value = Expansion(i).GetRefSol
        '            .Rows(i).Cells(1).Value = Expansion(i).GetNomSol
        '            .Rows(i).Cells(2).Value = Expansion(i).GetZoneGeo
        '            .Rows(i).Cells(3).Value = Expansion(i).GetFamille
        '            .Rows(i).Cells(4).Value = Expansion(i).GetPhiMu
        '        Next i

        '    End With

        'End Sub

        'Public Sub AfficheDansCombo(ByRef Combo As System.Windows.Forms.ComboBox)
        '    ' Remplissage de l'enregistrement
        '    Dim i As Integer
        '    Combo.Items.Clear()
        '    For i = 0 To SolCount - 1
        '        Combo.Items.Add(Expansion(i).GetNomSol & " - " & Expansion(i).GetPhiMu & " °")
        '    Next i
        'End Sub
        Public Sub Show()
            MyListeSondes.Show()
        End Sub

        'Public Sub MaskListeExpansion()
        '    MyListeEssaisExpansion.CloseListeExpansion
        'End Sub

        'Public Function NouveauSol(ByVal NewSoil As Boolean, ByVal NewTable As Boolean) As Sol

        '    ' Déclaration d'un sol

        '    MySol = Nothing
        '    ' Incrémentation du compteur de forages
        '    'SolCount = SolCount + 1

        '    ' Redimensionnement du tableau dynamique des Expansion
        '    ReDim Preserve Expansion(SolCount - 1)

        '    ' Instanciation du nouveau sol
        '    If MySol Is Nothing Then
        '        MySol = New Sol
        '    End If

        '    ' Enregistrement du nouveau sol dans le tableau de Expansion
        '    Expansion(SolCount - 1) = MySol

        '    ' Attribution d'une référence au nouveau Sol
        '    With MySol
        '        .SuspendreEvents()
        '        .SetRefSol(GiveMeANewRefSol)
        '        .SetMyListe(Me)
        '        .RetablirEvents()
        '    End With


        '    ' Ajout d'une ligne à la Table "LesExpansion"
        '    If NewSoil Then

        '        ' Ajout d'une ligne à la Table "LesEssaisForage"
        '        'ObjetDataRow = MonChantier.Tables("LesExpansion").NewRow()

        '        'ObjetDataRow("refSol") = MyListeExpansion.GiveMeANewRefSol
        '        If MyListeExpansion.GetSolParIndex(SolCount - 1) IsNot (Nothing) Then
        '            MyListeExpansion.Expansion(SolCount - 1) = MySol
        '        End If
        '        'MyListeExpansion.Expansion.Add(ObjetDataRow)

        '    End If
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()

        '    NouveauSol = MySol

        'End Function

        'Public Function Modifier(ByRef Soil As Sol) As Boolean
        '    Dim IndiceSol As Integer

        '    Modifier = False
        '    With MaListeExpansion
        '        IndiceSol = MyListeExpansion.GetIndexSol(Soil.GetRefSol)
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("refSol") = Soil.GetRefSol
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("nomSol") = Soil.GetNomSol
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("zoneGeo") = Soil.GetZoneGeo
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("famille") = Soil.GetFamille
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("classeSol") = Soil.GetClasseSol
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("sousClasse") = Soil.GetSousClasse
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("classeInférieure") = Soil.GetClasseInférieure
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("sousClasseInférieure") = Soil.GetSousClasseInférieure
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("nomSolNormalise") = Soil.GetNomSolNormalise
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("dmax") = Soil.GetDmax
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("tamisat50mm") = Soil.GetTamisat50mm
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("tamisat2mm") = Soil.GetTamisat2mm
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("tamisat80microns") = Soil.GetTamisat80microns
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("VBS") = Soil.GetVBS
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("IP") = Soil.GetIP
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("Es") = Soil.GetEs
        '        .Tables("LesExpansion").Rows(IndiceSol).Item("phiMu") = Soil.GetPhiMu
        '    End With
        '    Modifier = True
        'End Function

        '        Public Function SupSol(ByVal ref As String) As Boolean
        '            Dim i As Integer    ' Index du sol à Delete
        '            Dim J As Integer

        '            On Error GoTo errFind

        '            ' Récupération de l'indice du sol d'après sa référence
        '            i = GetIndexSol(ref)

        '            ' Enregistrement du sol dans le tableau des Expansion supprimés:
        '            ' Incrémentation du compteur de Expansion à Delete
        '            ExpansionupCount = ExpansionupCount + 1

        '            ' Redimensionnement du tableau dynamique des Expansion à Delete
        '            ReDim Preserve ExpansionSup(ExpansionupCount - 1)

        '            ' Effaçage du sol
        '            ExpansionSup(ExpansionupCount - 1) = Expansion(i)

        '            ' Décalage des Expansion suivants d'une case
        '            For J = i To SolCount - 2
        '                Expansion(J) = Expansion(J + 1)
        '            Next

        '            ' Redimensionnement du tableau Expansion
        '            If SolCount > 1 Then
        '                ReDim Preserve Expansion(SolCount - 2)
        '            Else
        '                ReDim Expansion(0)
        '            End If

        '            ' Décrémentation du compteur de Expansion
        '            SolCount = SolCount - 1

        '            ' Suppression du Sol dans MaListeExpansion
        '            With MaListeExpansion.Tables("LesExpansion")
        '                .Rows(i).Delete()
        '            End With

        '            ' Appel de la procédure indiquant que des modifications ont eu lieu
        '            Call IHaveBeenModified()

        '            SupSol = True
        '            Exit Function

        'errFind:
        '            SupSol = False
        '            MsgBox(Err.Description)
        '        End Function

        'Public Function GetIndexSol(ByVal ref As String) As Integer
        '    ' Recherche du Sol ayant pour référence 'ref' avec arrêt dès que possible
        '    Dim i As Integer
        '    Do While (i < SolCount)
        '        If (Expansion(i).GetRefSol = ref) Then Exit Do
        '        i = i + 1
        '    Loop

        '    ' Si le Sol n'a pas été trouvé, la valeur -1 est renvoyée
        '    If i = SolCount Then i = -1
        '    GetIndexSol = i
        'End Function
        Public Function GetFichierName()
            GetFichierName = FichierListeExpansion
        End Function

        'Public Function GetIsSaved() As Boolean
        '    If Not IsSaved And SolCount > 0 Then
        '        Dim i As Integer
        '        IsSaved = True
        '        For i = 0 To SolCount - 1
        '            IsSaved = IsSaved And Expansion(i).GetIsSaved
        '            If Not IsSaved Then Exit For
        '        Next
        '    End If
        '    IsSaved = IsSaved
        'End Function
        'Public Function GetIslsSaved() As Boolean
        '    ' Logique pour vérifier si les données sont sauvegardées
        '    If Not IsSaved And SolCount > 0 Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function


        'Public Function GiveMeANewRefSol()
        '    ' Détermination d'une nouvelle référence pour un forage
        '    Dim k As Integer
        '    k = 1

        '    Do While Not IsRefUnique("RefSol " & k)
        '        k = k + 1
        '    Loop
        '    GiveMeANewRefSol = "RefSol " & k

        'End Function

        'Public Function IsRefUnique(ByVal ref As String) As Boolean
        '    ' Vérification de l'unicité d'une référence
        '    Dim i As Integer
        '    Dim unique As Boolean

        '    unique = True

        '    Do While (i < SolCount) And unique
        '        unique = (Expansion(i).GetRefSol <> ref)
        '        i = i + 1
        '    Loop

        '    IsRefUnique = unique
        'End Function

        'Public Function GetSolCount()
        '    GetSolCount = SolCount
        'End Function

        'Public Function GetSolParIndex(ByVal Index As Integer) As Sol
        '    GetSolParIndex = Expansion(Index)
        'End Function

        'Public Function GetSolParRef(ByVal ref As String) As Sol
        '    GetSolParRef = Expansion(GetIndexSol(ref))
        'End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        'Public Function SetSol(ByRef NouvSol As Sol, ByVal Index As Integer)
        '    If Index <= SolCount Then
        '        Expansion(Index) = NouvSol
        '    End If
        '    SetSol = NouvSol
        'End Function


    End Class
End Namespace