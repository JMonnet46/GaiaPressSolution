' =============================================================================
' ClasseListeForages.vb
' Projet : GaiaPress.Core.V17
'
' Classe en mémoire de gestion de la liste des Forages.
' Version VB.NET propre, compilable, sans héritage DbContext.
'
' S'appuie sur DataService :
'   - DataService.ObjetDataForage (DataTable) : persistance des Forages.
'
' Property PascalCase utilisées sur la classe Forage :
'   RefForage, NomForage, ZoneGeo, Famille, ClasseForage, SousClasse,
'   ClasseInferieure, SousClasseInferieure, NomForageNormalise,
'   Dmax, Tamisat50mm, Tamisat2mm, Tamisat80microns,
'   VBS, IP, Es, PhiMu
'
' NOTE : Tout le code VB6 commenté en bas de la version originale a été
' supprimé volontairement pour produire un fichier propre et compilable.
' =============================================================================

Imports System.Data
Imports System.Linq
Imports GaiaPress.Core.V17.Models
Imports GaiaPress.Core.V17.Services

Namespace GaiaPressSolution.GaiaPressCoreV17.Models

    Public Class LesForages

        ' ---------------------------------------------------------------------
        ' Liste interne des Forages
        ' ---------------------------------------------------------------------
        Public F_ListeForages As List(Of Forage)

        ' ---------------------------------------------------------------------
        ' Référence vers le DataService
        ' ---------------------------------------------------------------------
        Public ReadOnly _dataService As DataService

        ' Indicateur de modification
        Public _isModified As Boolean

        ' Nom de fichier logique (utilisé par GetFichierName)
        Public _fichierName As String

        ' Compteur pour GiveMeANewRefForage
        Public _nextRefForageCounter As Integer

        Public C_EventsSuspendus As Boolean
        Public C_IsSaved As Boolean
        Public Event HasBeenModified()
        ' ---------------------------------------------------------------------
        ' Constructeur
        ' ---------------------------------------------------------------------
        Public Sub New(dataService As DataService)
            If dataService Is Nothing Then
                Throw New ArgumentNullException(NameOf(dataService))
            End If

            _dataService = dataService
            F_ListeForages = New List(Of Forage)()
            _isModified = False
            _fichierName = "Forages"
            _nextRefForageCounter = 0
        End Sub

        ' ---------------------------------------------------------------------
        ' API publique : liste exposée en lecture seule
        ' ---------------------------------------------------------------------
        Public ReadOnly Property C_LesForages As List(Of Forage)
            Get
                Return F_ListeForages
            End Get
        End Property

        'Public ReadOnly Property IHaveBeenModified As Boolean
        '    Get
        '        Return _isModified
        '    End Get
        'End Property

        Public Function GetFichierName() As String
            Return _fichierName
        End Function



        ' =====================================================================
        ' Gestion CRUD des Forages
        ' =====================================================================

        Public Sub AjouterForage(forage As Forage)
            F_ListeForages.Add(forage)
            C_IsSaved = False
            If Not C_EventsSuspendus Then RaiseEvent HasBeenModified()
        End Sub
        Public Function TrouverForage(refForage As String) As Forage
            For Each f As Forage In F_ListeForages
                If f.F_RefForage = refForage Then Return f
            Next
            Return Nothing
        End Function
        Public Function DeleteForage(refForage As String) As Boolean
            Dim Forage As Forage = TrouverForage(refForage)
            If Forage Is Nothing Then Return False
            F_ListeForages.Remove(Forage)
            _isModified = True
            Return True
        End Function

        Public Function MettreAJourForage(Forage As Forage) As Boolean
            If Forage Is Nothing Then Return False
            If String.IsNullOrEmpty(Forage.F_RefForage) Then Return False

            Dim index As Integer = GetIndexForage(Forage.F_RefForage)
            If index < 0 Then Return False

            F_ListeForages(index) = Forage
            _isModified = True
            Return True
        End Function

        Public Function GetIndexForage(refForage As String) As Integer
            If String.IsNullOrEmpty(refForage) Then Return -1
            For i As Integer = 0 To F_ListeForages.Count - 1
                Dim s As Forage = F_ListeForages(i)
                If s IsNot Nothing AndAlso s.F_RefForage = refForage Then
                    Return i
                End If
            Next
            Return -1
        End Function

        ' =====================================================================
        ' Gestion des références (refForage)
        ' =====================================================================

        Public Function IsRefForageUnique(refForage As String,
                                       excludeExisting As Forage) As Boolean
            If String.IsNullOrEmpty(refForage) Then Return False

            For Each s As Forage In F_ListeForages
                If s Is Nothing Then Continue For
                If excludeExisting IsNot Nothing AndAlso ReferenceEquals(s, excludeExisting) Then
                    Continue For
                End If
                If s.F_RefForage = refForage Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Function GiveMeANewRefForage() As String
            ' Génère une nouvelle référence unique au format "Forage-N".
            Dim candidate As String
            Do
                _nextRefForageCounter += 1
                candidate = String.Format("Forage-{0}", _nextRefForageCounter)
            Loop While Not IsRefForageUnique(candidate, Nothing)
            Return candidate
        End Function

        ' =====================================================================
        ' Chargement / sauvegarde
        ' =====================================================================

        Public Sub ChargerDepuisBase()
            F_ListeForages.Clear()
            _isModified = False

            Dim dt As DataTable = _dataService.ObjetDataForage
            If dt Is Nothing Then Return

            For Each row As DataRow In dt.Rows
                Dim Forage As New Forage()

                If dt.Columns.Contains("refChantier") AndAlso Not row.IsNull("refChantier") Then
                    Forage.F_RefChantier = CStr(row("refChantier"))
                End If
                If dt.Columns.Contains("refForage") AndAlso Not row.IsNull("refForage") Then
                    Forage.F_RefForage = CStr(row("refForage"))
                End If
                If dt.Columns.Contains("numeroForage") AndAlso Not row.IsNull("numeroForage") Then
                    Forage.F_numeroForage = CStr(row("numeroForage"))
                End If
                If dt.Columns.Contains("nomForage") AndAlso Not row.IsNull("nomForage") Then
                    Forage.F_NomForage = CStr(row("nomForage"))
                End If
                If dt.Columns.Contains("DateForage") AndAlso Not row.IsNull("DateForage") Then
                    Forage.F_dateForage = CStr(row("DateForage"))
                End If
                If dt.Columns.Contains("materiel") AndAlso Not row.IsNull("materiel") Then
                    Forage.F_materiel = CStr(row("materiel"))
                End If
                If dt.Columns.Contains("outil") AndAlso Not row.IsNull("outil") Then
                    Forage.F_outil = CStr(row("outil"))
                End If
                If dt.Columns.Contains("abscisseX") AndAlso Not row.IsNull("abscisseX") Then
                    Forage.F_abscisseX = CStr(row("abscisseX"))
                End If
                If dt.Columns.Contains("ordonneeY") AndAlso Not row.IsNull("ordonneeY") Then
                    Forage.F_ordonneeY = CStr(row("ordonneeY"))
                End If
                If dt.Columns.Contains("altitudeZ") AndAlso Not row.IsNull("altitudeZ") Then
                    Forage.F_altitudeZ = CStr(row("altitudeZ"))
                End If
                If dt.Columns.Contains("coteInit") AndAlso Not row.IsNull("coteInit") Then
                    Forage.F_coteInit = CStr(row("coteInit"))
                End If
                If dt.Columns.Contains("coteFinale") AndAlso Not row.IsNull("coteFinale") Then
                    Forage.F_coteFinale = CSng(row("coteFinale"))
                End If

                F_ListeForages.Add(Forage)
            Next

            ' Mise à jour du compteur pour GiveMeANewRefForage.
            Dim maxCounter As Integer = 0
            For Each s As Forage In F_ListeForages
                If s Is Nothing OrElse String.IsNullOrEmpty(s.F_RefForage) Then Continue For
                Dim prefix As String = "Forage-"
                If s.F_RefForage.StartsWith(prefix) Then
                    Dim n As Integer
                    If Integer.TryParse(s.F_RefForage.Substring(prefix.Length), n) Then
                        If n > maxCounter Then maxCounter = n
                    End If
                End If
            Next
            _nextRefForageCounter = maxCounter
        End Sub

        ' ✅ BON : ne copier que les propriétés de Forage
        Public Sub SaveForagesToDataTable()
            Dim dt As DataTable = _dataService.ObjetDataForage
            If dt Is Nothing Then Return

            ' Effacer les anciennes lignes
            dt.Rows.Clear()

            For Each Forage As Forage In F_ListeForages
                If Forage Is Nothing Then Continue For

                Dim row As DataRow = dt.NewRow()
                row("RefChantier") = Forage.F_RefChantier
                row("RefForage") = Forage.F_RefForage
                row("NumeroForage") = Forage.F_numeroForage
                row("NomForage") = Forage.F_NomForage
                row("DateForage") = Forage.F_dateForage
                row("Materiel") = Forage.F_materiel
                row("Outil") = Forage.F_outil
                row("AbscisseX") = Forage.F_abscisseX
                row("OrdonneeY") = Forage.F_ordonneeY
                row("AltitudeZ") = Forage.F_altitudeZ
                row("CoteInit") = Forage.F_coteInit
                row("CoteFinale") = Forage.F_coteFinale

                dt.Rows.Add(row)
            Next

            _isModified = False
        End Sub

    End Class

End Namespace

