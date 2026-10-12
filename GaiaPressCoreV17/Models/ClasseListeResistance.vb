Imports GaiaPressCoreV17.Services

Namespace GaiaPressCoreV17.Models

    Public Class ListeEssaiResistance

        Implements ICollection(Of EssaiResistance)

        ' ======= Propriétés de Liste ===========
        Public Property ListeResistance As New List(Of EssaiResistance)

        ' ✅ Propriété HasBeenModified (événement)
        Public Event HasBeenModified As EventHandler

        ' ✅ Liste interne
        Private _items As List(Of EssaiResistance)

        ' ✅ Propriété Count
        Public ReadOnly Property Count As Integer Implements ICollection(Of EssaiResistance).Count
            Get
                Return _items.Count
            End Get
        End Property

        ' ✅ Propriété IsReadOnly
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of EssaiResistance).IsReadOnly
            Get
                Return False
            End Get
        End Property

        ' =====================================================================================
        ' Constructeur
        ' =====================================================================================
        Public Sub New()
            _items = New List(Of EssaiResistance)
        End Sub

        ' ====================================================================================
        ' Méthodes
        ' ====================================================================================

        ' ✅ Méthode Add
        Public Sub Add(item As EssaiResistance) Implements ICollection(Of EssaiResistance).Add
            _items.Add(item)
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

        ' ✅ Méthode Clear
        Public Sub Clear() Implements ICollection(Of EssaiResistance).Clear
            _items.Clear()
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

        ' ✅ Méthode Contains
        Public Function Contains(item As EssaiResistance) As Boolean Implements ICollection(Of EssaiResistance).Contains
            Return _items.Contains(item)
        End Function

        ' ✅ Méthode CopyTo
        Public Sub CopyTo(array As EssaiResistance(), arrayIndex As Integer) Implements ICollection(Of EssaiResistance).CopyTo
            _items.CopyTo(array, arrayIndex)
        End Sub

        ' ✅ Méthode Remove
        Public Function Remove(item As EssaiResistance) As Boolean Implements ICollection(Of EssaiResistance).Remove
            Dim result = _items.Remove(item)
            If result Then
                RaiseEvent HasBeenModified(Me, EventArgs.Empty)
            End If
            Return result
        End Function

        ' ✅ Méthode GetEnumerator
        Public Function GetEnumerator() As IEnumerator(Of EssaiResistance) Implements IEnumerable(Of EssaiResistance).GetEnumerator
            Return _items.GetEnumerator()
        End Function

        ' ✅ Méthode GetEnumerator (IEnumerable)
        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _items.GetEnumerator()
        End Function


        ' Indexeur (si nécessaire)
        Default Public Property Item(index As Integer) As EssaiResistance
            Get
                Return _items(index)
            End Get
            Set(value As EssaiResistance)
                _items(index) = value
            End Set
        End Property


        'Public Sub AddResistance(EssaiResistance As EssaiResistance)
        '    Me.ListResistance.Add(EssaiResistance)
        '    ' Logique pour lier au chantier si nécessaire
        '    EssaiResistance.SetRefEssaiResistance(EssaiResistance.GetRefEssai)
        'End Sub

        ' ================================================================================
        ' Gestion de ListeResistance
        ' ================================================================================

        Public Sub AddResistance(EssaiResistance As EssaiResistance)
            _items.Add(EssaiResistance)  ' ✅ Utiliser _items au lieu de ListResistance
        End Sub

        Public Function GetEssaiResistance(refEssai As String) As EssaiResistance
            For Each E As EssaiResistance In _items  ' Utiliser _items
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        Public Function GetEssaiResistanceParRefEssai(refEssai As String) As EssaiResistance
            For Each E As EssaiResistance In Me.ListeResistance
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        Public Function GetEssaiResistanceParRefSonde(refSonde As String, rang As Integer) As EssaiResistance
            Dim i As Integer
            For Each E As EssaiResistance In Me.ListeResistance
                If E.GetRefSonde = refSonde And i >= rang Then
                    Return E
                End If
                i = i + 1
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssaiResistance(refEssai As String)
            Dim EssaiADelete As EssaiResistance = GetEssaiResistance(refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.Remove(EssaiADelete)
            End If
        End Sub

        ' Mettre à jour un Essai
        Public Sub MettreAJourEssaiResistance(refEssai As String, nouvelEssai As EssaiResistance)
            Dim EssaiResistance As EssaiResistance = GetEssaiResistance(refEssai)
            If EssaiResistance IsNot Nothing Then
                EssaiResistance = nouvelEssai
                'Sol. dateEssai = nouvelleDate
            End If
        End Sub

        'Public Shared Sub InitializeListResistance()
        '    If Not MyListeEssaiResistance.IsSaved Then
        '        ListResistance.IsSaved = True
        '        If MyListResistance Is Nothing Then
        '            MyListResistance = New ListResistance
        '        End If
        '        IsInitializinResistance = False
        '    End If
        'End Sub

        'Public Sub New()

        '    ' Création de la feuille de liste
        '    'If MyListResistance Is Nothing Then
        '    '    InitializeListResistance()
        '    'End If

        '    '' Enregistrement de la référence de la liste Sol auprès de sa feuille
        '    'Call MyListResistance.GiveMeANewRefSol()

        '    ' -----------------------------------------------------------------
        '    Dim Index As Integer

        '    ' Création de la feuille de liste
        '    If MyListResistance Is Nothing Then
        '        Index = 0
        '        MyListResistance = Me
        '        ' Initialize other properties or perform other setup tasks here
        '    Else
        '        If MySol Is Nothing Then
        '            MyListResistance = Nothing
        '            Exit Sub
        '        End If
        '        Index = GetIndexSol(MySol.GetNomSol)
        '        If Index >= 0 Then Call MyListResistance.SetSol(MySol, Index)
        '    End If

        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    ' Call MyListeSonde.SetSonde(MySonde, Index)
        '    ' -----------------------------------------------------

        'End Sub


        Public Sub CloseListResistance()
            ' Fermeture et nettoyage
            _items.Clear()
        End Sub


        ' **********************************************************************
        ' Fonction d'écriture dans le fichier Access desResistance
        ' **********************************************************************
        '        Public Function EnregistreListResistance(ByVal FicResistance As String) As Boolean
        '            On Error GoTo errFind

        '            ' On crée une commanDebuilder pour renvoyer les données à la BD

        '            ' Création de l'objetCommand nécessaire à l'enregistrement

        '            ' Effacement des commandes précédentes

        '            ' Mise à jour de la BD
        '            iTable = 4
        '            ObjetDatResistance = MaListResistance.Tables("LeResistance")
        '            MsgBox("Enregistrement desResistance réussi")

        '            EnregistrResistance = True
        '            IsSaved = True

        '            Exit Function

        'errFind:
        '            MsgBox(Err.Description, vbExclamation, Err.Source)
        '            Err.Clear()
        '            'FrmTable.Show()
        '            EnregistrResistance = False

        '        End Function


        'Public Sub CloseListResistance()
        '    Call MaskListResistance()
        '    Dim i As Integer
        '    For i = 0 To SolCount - 1
        '        ListResistance(i).CloseListResistance()
        '        ListResistance(i) = Nothing
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

        '    Taille = MyListResistance.GetSolCount
        '    With Table
        '        .Rows.Clear()
        '        ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
        '        For i = 0 To Taille - 1
        '            .Rows.Add()
        '            .Rows(i).Cells(0).Value =Resistance(i).GetRefSol
        '            .Rows(i).Cells(1).Value =Resistance(i).GetNomSol
        '            .Rows(i).Cells(2).Value =Resistance(i).GetZoneGeo
        '            .Rows(i).Cells(3).Value =Resistance(i).GetFamille
        '            .Rows(i).Cells(4).Value =Resistance(i).GetPhiMu
        '        Next i

        '    End With

        'End Sub

        'Public Sub AfficheDansCombo(ByRef Combo As System.Windows.Forms.ComboBox)
        '    ' Remplissage de l'enregistrement
        '    Dim i As Integer
        '    Combo.Items.Clear()
        '    For i = 0 To SolCount - 1
        '        Combo.Items.AddResistance(i).GetNomSol & " - " &Resistance(i).GetPhiMu & " °")
        '    Next i
        'End Sub
        'Public Sub Show()
        '    MyListeSonde.Show()
        'End Sub

        'Public Sub MaskListResistance()
        '    MyListeEssaiResistance.CloseListResistance
        'End Sub

        'Public Function NouveauSol(ByVal NewSoil As Boolean, ByVal NewTable As Boolean) As Sol

        '    ' Déclaration d'un sol

        '    MySol = Nothing
        '    ' Incrémentation du compteur de forages
        '    'SolCount = SolCount + 1

        '    ' Redimensionnement du tableau dynamique desResistance
        '    ReDim PreserveResistance(SolCount - 1)

        '    ' Instanciation du nouveau sol
        '    If MySol Is Nothing Then
        '        MySol = New Sol
        '    End If

        '    ' Enregistrement du nouveau sol dans le tableau deResistance
        '   Resistance(SolCount - 1) = MySol

        '    ' Attribution d'une référence au nouveau Sol
        '    With MySol
        '        .SuspendreEvents()
        '        .SetRefSol(GiveMeANewRefSol)
        '        .SetMyListe(Me)
        '        .RetablirEvents()
        '    End With


        '    ' Ajout d'une ligne à la Table "LeResistance"
        '    If NewSoil Then

        '        ' Ajout d'une ligne à la Table "LesEssaisForage"
        '        'ObjetDataRow = MonChantier.Tables("LeResistance").NewRow()

        '        'ObjetDataRow("refSol") = MyListResistance.GiveMeANewRefSol
        '        If MyListResistance.GetSolParIndex(SolCount - 1) IsNot (Nothing) Then
        '            MyListResistanceResistance(SolCount - 1) = MySol
        '        End If
        '        'MyListResistanceResistance.Add(ObjetDataRow)

        '    End If
        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()

        '    NouveauSol = MySol

        'End Function

        'Public Function Modifier(ByRef Soil As Sol) As Boolean
        '    Dim IndiceSol As Integer

        '    Modifier = False
        '    With MaListResistance
        '        IndiceSol = MyListResistance.GetIndexSol(Soil.GetRefSol)
        '        .Tables("LeResistance").Rows(IndiceSol).Item("refSol") = Soil.GetRefSol
        '        .Tables("LeResistance").Rows(IndiceSol).Item("nomSol") = Soil.GetNomSol
        '        .Tables("LeResistance").Rows(IndiceSol).Item("zoneGeo") = Soil.GetZoneGeo
        '        .Tables("LeResistance").Rows(IndiceSol).Item("famille") = Soil.GetFamille
        '        .Tables("LeResistance").Rows(IndiceSol).Item("classeSol") = Soil.GetClasseSol
        '        .Tables("LeResistance").Rows(IndiceSol).Item("sousClasse") = Soil.GetSousClasse
        '        .Tables("LeResistance").Rows(IndiceSol).Item("classeInférieure") = Soil.GetClasseInférieure
        '        .Tables("LeResistance").Rows(IndiceSol).Item("sousClasseInférieure") = Soil.GetSousClasseInférieure
        '        .Tables("LeResistance").Rows(IndiceSol).Item("nomSolNormalise") = Soil.GetNomSolNormalise
        '        .Tables("LeResistance").Rows(IndiceSol).Item("dmax") = Soil.GetDmax
        '        .Tables("LeResistance").Rows(IndiceSol).Item("tamisat50mm") = Soil.GetTamisat50mm
        '        .Tables("LeResistance").Rows(IndiceSol).Item("tamisat2mm") = Soil.GetTamisat2mm
        '        .Tables("LeResistance").Rows(IndiceSol).Item("tamisat80microns") = Soil.GetTamisat80microns
        '        .Tables("LeResistance").Rows(IndiceSol).Item("VBS") = Soil.GetVBS
        '        .Tables("LeResistance").Rows(IndiceSol).Item("IP") = Soil.GetIP
        '        .Tables("LeResistance").Rows(IndiceSol).Item("Es") = Soil.GetEs
        '        .Tables("LeResistance").Rows(IndiceSol).Item("phiMu") = Soil.GetPhiMu
        '    End With
        '    Modifier = True
        'End Function

        '        Public Function SupSol(ByVal ref As String) As Boolean
        '            Dim i As Integer    ' Index du sol à Delete
        '            Dim J As Integer

        '            On Error GoTo errFind

        '            ' Récupération de l'indice du sol d'après sa référence
        '            i = GetIndexSol(ref)

        '            ' Enregistrement du sol dans le tableau desResistance supprimés:
        '            ' Incrémentation du compteur deResistance à Delete
        '           ResistanceupCount =ResistanceupCount + 1

        '            ' Redimensionnement du tableau dynamique desResistance à Delete
        '            ReDim PreserveResistanceSupResistanceupCount - 1)

        '            ' Effaçage du sol
        '           ResistanceSupResistanceupCount - 1) =Resistance(i)

        '            ' Décalage desResistance suivants d'une case
        '            For J = i To SolCount - 2
        '               Resistance(J) =Resistance(J + 1)
        '            Next

        '            ' Redimensionnement du tableauResistance
        '            If SolCount > 1 Then
        '                ReDim PreserveResistance(SolCount - 2)
        '            Else
        '                ReDimResistance(0)
        '            End If

        '            ' Décrémentation du compteur deResistance
        '            SolCount = SolCount - 1

        '            ' Suppression du Sol dans MaListResistance
        '            With MaListResistance.Tables("LeResistance")
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
        '        If Resistance(i).GetRefSol = ref) Then Exit Do
        '        i = i + 1
        '    Loop

        '    ' Si le Sol n'a pas été trouvé, la valeur -1 est renvoyée
        '    If i = SolCount Then i = -1
        '    GetIndexSol = i
        'End Function

        Public Function GetFichierName() As String
            ' À implémenter selon vos besoins
            GetFichierName = MyResistance. RefEssai.tostring
            Return ""
        End Function

        'Public Function GetIsSaved() As Boolean
        '    If Not IsSaved And SolCount > 0 Then
        '        Dim i As Integer
        '        IsSaved = True
        '        For i = 0 To SolCount - 1
        '            IsSaved = IsSaved AndResistance(i).GetIsSaved
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
        '        unique = Resistance(i).GetRefSol <> ref)
        '        i = i + 1
        '    Loop

        '    IsRefUnique = unique
        'End Function

        'Public Function GetSolCount()
        '    GetSolCount = SolCount
        'End Function

        'Public Function GetSolParIndex(ByVal Index As Integer) As Sol
        '    GetSolParIndex =Resistance(Index)
        'End Function

        'Public Function GetSolParRef(ByVal ref As String) As Sol
        '    GetSolParRef =Resistance(GetIndexSol(ref))
        'End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

        'Public Function SetSol(ByRef NouvSol As Sol, ByVal Index As Integer)
        '    If Index <= SolCount Then
        '       Resistance(Index) = NouvSol
        '    End If
        '    SetSol = NouvSol
        'End Function


    End Class
End Namespace