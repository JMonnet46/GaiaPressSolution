Namespace GaiaPress.core.V17.Models

    Public Class ListeSols
        Inherits DbContext
        Public Property S_ListeSols As New List(Of Sol)
        Public FichierListeSols As String
        'Public MyListeSols As FrmSols
        Public EventsSuspendus As Boolean

        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de Liste_Sols
        ' ================================================================================
        ' Add un Sol

        Public Sub AddSol(Sol As Sol)
            Me.S_ListeSols.Add(Sol)
            ' Lier le forage au chantier (si nécessaire)
            Sol.SetRefSol(Sol.GetRefSol)
        End Sub

        Public Function GetSol(refSol As String) As Sol
            For Each S As Sol In Me.S_ListeSols
                If S.GetRefSol = refSol Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteSol(refSol As String)
            Dim SolADelete As Sol = GetSol(refSol)
            If SolADelete IsNot Nothing Then
                Me.S_ListeSols.Remove(SolADelete)
            End If
        End Sub


        ' Mettre à jour un Essai
        Public Sub MettreAJourSol(refSol As String, nouveauNom As String, nouvelleDate As String)
            Dim Sol As Sol = GetSol(refSol)
            If Sol IsNot Nothing Then
                Sol.SetNomSol(nouveauNom)
                'Sol.E_dateEssai = nouvelleDate
            End If
        End Sub

        Public Shared Sub InitializeListeSols()
            If Not IsInitializingSols Then
                IsInitializingSols = True
                If MyListeSols Is Nothing Then
                    MyListeSols = New ListeSols
                End If
                IsInitializingSols = False
            End If
        End Sub

        'Public Sub New()

        '    ' Création de la feuille de liste
        '    'If MyListeSols Is Nothing Then
        '    '    InitializeListeSols()
        '    'End If

        '    '' Enregistrement de la référence de la liste Sol auprès de sa feuille
        '    'Call MyListeSols.GiveMeANewRefSol()

        '    ' -----------------------------------------------------------------
        '    Dim Index As Integer

        '    ' Création de la feuille de liste
        '    If MyListeSols Is Nothing Then
        '        Index = 0
        '        MyListeSols = Me
        '        ' Initialize other properties or perform other setup tasks here
        '    Else
        '        If MySol Is Nothing Then
        '            MyListeSols = Nothing
        '            Exit Sub
        '        End If
        '        Index = GetIndexSol(MySol.GetNomSol)
        '        If Index >= 0 Then Call MyListeSols.SetSol(MySol, Index)
        '    End If

        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    ' Call MyListeSondes.SetSonde(MySonde, Index)
        '    ' -----------------------------------------------------

        'End Sub


        Public Sub CloseListeSols()

            ' fermeture de la Liste des Sondes (fermeture de sa fenêtre)
            'Call MyListeSondes.CloseChantier()

            ' Extinction des menus liés au chantier
            'Call ActivationMenus(False)

            ' Effacement de la référence vers le chantier
            MyListeSols = Nothing

            ' Nettoyage de l'arborescence
            'TreeViewArbo.Nodes.Clear()
        End Sub

        ' **********************************************************************
        ' Fonction d'écriture dans le fichier Access des Sols
        ' **********************************************************************
        Public Function EnregistreSols(ByVal FichSols As String) As Boolean
            On Error GoTo errFind

            ' On crée une commande Builder pour renvoyer les données à la BD

            ' Création de l'objetCommand nécessaire à l'enregistrement

            ' Effacement des commandes précédentes

            ' Mise à jour de la BD
            iTable = 4
            ObjetDataSols = MaListeSols.Tables("LesSols")
            MsgBox("Enregistrement des sols réussi")

            EnregistreSols = True
            IsSaved = True

            Exit Function

errFind:
            MsgBox(Err.Description, vbExclamation, Err.Source)
            Err.Clear()
            'FrmTable.Show()
            EnregistreSols = False

        End Function


        'Public Sub CloseListeSols()
        '    Call MaskListeSols()
        '    Dim i As Integer
        '    For i = 0 To SolCount - 1
        '        ListeSols(i).CloseListeSols()
        '        ListeSols(i) = Nothing
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

        '    Taille = MyListeSols.GetSolCount
        '    With Table
        '        .Rows.Clear()
        '        ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
        '        For i = 0 To Taille - 1
        '            .Rows.Add()
        '            .Rows(i).Cells(0).Value = Sols(i).GetRefSol
        '            .Rows(i).Cells(1).Value = Sols(i).GetNomSol
        '            .Rows(i).Cells(2).Value = Sols(i).GetZoneGeo
        '            .Rows(i).Cells(3).Value = Sols(i).GetFamille
        '            .Rows(i).Cells(4).Value = Sols(i).GetPhiMu
        '        Next i

        '    End With

        'End Sub

        'Public Sub AfficheDansCombo(ByRef Combo As System.Windows.Forms.ComboBox)
        '    ' Remplissage de l'enregistrement
        '    Dim i As Integer
        '    Combo.Items.Clear()
        '    For i = 0 To SolCount - 1
        '        Combo.Items.Add(Sols(i).GetNomSol & " - " & Sols(i).GetPhiMu & " °")
        '    Next i
        'End Sub
        Public Sub Show()
            MyListeSondes.Show()
        End Sub

        Public Sub MaskListeSols()
            MyListeSols.CloseListeSols()
        End Sub

        'Public Function NouveauSol(ByVal NewSoil As Boolean, ByVal NewTable As Boolean) As Sol

        '    ' Déclaration d'un sol

        '    MySol = Nothing
        '    ' Incrémentation du compteur de forages
        '    'SolCount = SolCount + 1

        '    ' Redimensionnement du tableau dynamique des sols
        '    ReDim Preserve Sols(SolCount - 1)

        '    ' Instanciation du nouveau sol
        '    If MySol Is Nothing Then
        '        MySol = New Sol
        '    End If

        '    ' Enregistrement du nouveau sol dans le tableau de sols
        '    Sols(SolCount - 1) = MySol

        '    ' Attribution d'une référence au nouveau Sol
        '    With MySol
        '        .SuspendreEvents()
        '        .SetRefSol(GiveMeANewRefSol)
        '        .SetMyListe(Me)
        '        .RetablirEvents()
        '    End With


        '    ' Ajout d'une ligne à la Table "LesSols"
        '    If NewSoil Then

        '        ' Ajout d'une ligne à la Table "LesEssaisForage"
        '        'ObjetDataRow = MonChantier.Tables("LesSols").NewRow()

        '        'ObjetDataRow("refSol") = MyListeSols.GiveMeANewRefSol
        '        If MyListeSols.GetSolParIndex(SolCount - 1) IsNot (Nothing) Then
        '            MyListeSols.Sols(SolCount - 1) = MySol
        '        End If
        '        'MyListeSols.Sols.Add(ObjetDataRow)

        '    End If
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()

        '    NouveauSol = MySol

        'End Function

        'Public Function Modifier(ByRef Soil As Sol) As Boolean
        '    Dim IndiceSol As Integer

        '    Modifier = False
        '    With MaListeSols
        '        IndiceSol = MyListeSols.GetIndexSol(Soil.GetRefSol)
        '        .Tables("LesSols").Rows(IndiceSol).Item("refSol") = Soil.GetRefSol
        '        .Tables("LesSols").Rows(IndiceSol).Item("nomSol") = Soil.GetNomSol
        '        .Tables("LesSols").Rows(IndiceSol).Item("zoneGeo") = Soil.GetZoneGeo
        '        .Tables("LesSols").Rows(IndiceSol).Item("famille") = Soil.GetFamille
        '        .Tables("LesSols").Rows(IndiceSol).Item("classeSol") = Soil.GetClasseSol
        '        .Tables("LesSols").Rows(IndiceSol).Item("sousClasse") = Soil.GetSousClasse
        '        .Tables("LesSols").Rows(IndiceSol).Item("classeInférieure") = Soil.GetClasseInférieure
        '        .Tables("LesSols").Rows(IndiceSol).Item("sousClasseInférieure") = Soil.GetSousClasseInférieure
        '        .Tables("LesSols").Rows(IndiceSol).Item("nomSolNormalise") = Soil.GetNomSolNormalise
        '        .Tables("LesSols").Rows(IndiceSol).Item("dmax") = Soil.GetDmax
        '        .Tables("LesSols").Rows(IndiceSol).Item("tamisat50mm") = Soil.GetTamisat50mm
        '        .Tables("LesSols").Rows(IndiceSol).Item("tamisat2mm") = Soil.GetTamisat2mm
        '        .Tables("LesSols").Rows(IndiceSol).Item("tamisat80microns") = Soil.GetTamisat80microns
        '        .Tables("LesSols").Rows(IndiceSol).Item("VBS") = Soil.GetVBS
        '        .Tables("LesSols").Rows(IndiceSol).Item("IP") = Soil.GetIP
        '        .Tables("LesSols").Rows(IndiceSol).Item("Es") = Soil.GetEs
        '        .Tables("LesSols").Rows(IndiceSol).Item("phiMu") = Soil.GetPhiMu
        '    End With
        '    Modifier = True
        'End Function

        '        Public Function SupSol(ByVal ref As String) As Boolean
        '            Dim i As Integer    ' Index du sol à Delete
        '            Dim J As Integer

        '            On Error GoTo errFind

        '            ' Récupération de l'indice du sol d'après sa référence
        '            i = GetIndexSol(ref)

        '            ' Enregistrement du sol dans le tableau des sols supprimés:
        '            ' Incrémentation du compteur de sols à Delete
        '            SolSupCount = SolSupCount + 1

        '            ' Redimensionnement du tableau dynamique des sols à Delete
        '            ReDim Preserve SolsSup(SolSupCount - 1)

        '            ' Effaçage du sol
        '            SolsSup(SolSupCount - 1) = Sols(i)

        '            ' Décalage des sols suivants d'une case
        '            For J = i To SolCount - 2
        '                Sols(J) = Sols(J + 1)
        '            Next

        '            ' Redimensionnement du tableau Sols
        '            If SolCount > 1 Then
        '                ReDim Preserve Sols(SolCount - 2)
        '            Else
        '                ReDim Sols(0)
        '            End If

        '            ' Décrémentation du compteur de Sols
        '            SolCount = SolCount - 1

        '            ' Suppression du Sol dans MaListeSols
        '            With MaListeSols.Tables("LesSols")
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
        '        If (Sols(i).GetRefSol = ref) Then Exit Do
        '        i = i + 1
        '    Loop

        '    ' Si le Sol n'a pas été trouvé, la valeur -1 est renvoyée
        '    If i = SolCount Then i = -1
        '    GetIndexSol = i
        'End Function
        Public Function GetFichierName()
            GetFichierName = FichierListeSols
        End Function

        'Public Function GetIsSaved() As Boolean
        '    If Not IsSaved And SolCount > 0 Then
        '        Dim i As Integer
        '        IsSaved = True
        '        For i = 0 To SolCount - 1
        '            IsSaved = IsSaved And Sols(i).GetIsSaved
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
        '        unique = (Sols(i).GetRefSol <> ref)
        '        i = i + 1
        '    Loop

        '    IsRefUnique = unique
        'End Function

        'Public Function GetSolCount()
        '    GetSolCount = SolCount
        'End Function

        'Public Function GetSolParIndex(ByVal Index As Integer) As Sol
        '    GetSolParIndex = Sols(Index)
        'End Function

        'Public Function GetSolParRef(ByVal ref As String) As Sol
        '    GetSolParRef = Sols(GetIndexSol(ref))
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
        '        Sols(Index) = NouvSol
        '    End If
        '    SetSol = NouvSol
        'End Function


    End Class
End Namespace