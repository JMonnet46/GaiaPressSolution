Imports System.Data
Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services

Namespace GaiaPressCoreV17.Models

    Public Class LesForages

        ' ---------------------------------------------------------------------
        ' Liste interne des Forages
        ' ---------------------------------------------------------------------
        Public ListeForages As List(Of Forage)

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
            ListeForages = New List(Of Forage)()
            _isModified = False
            _fichierName = "Forages"
            _nextRefForageCounter = 0
        End Sub

        ' ---------------------------------------------------------------------
        ' API publique : liste exposée en lecture seule
        ' ---------------------------------------------------------------------
        Public ReadOnly Property C_LesForages As List(Of Forage)
            Get
                Return ListeForages
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
            ListeForages.Add(forage)
            C_IsSaved = False
            If Not C_EventsSuspendus Then RaiseEvent HasBeenModified()
        End Sub
        Public Function TrouverForage(refForage As String) As Forage
            For Each f As Forage In ListeForages
                If f.RefForage = refForage Then Return f
            Next
            Return Nothing
        End Function
        Public Function DeleteForage(refForage As String) As Boolean
            Dim Forage As Forage = TrouverForage(refForage)
            If Forage Is Nothing Then Return False
            ListeForages.Remove(Forage)
            _isModified = True
            Return True
        End Function

        Public Function MettreAJourForage(Forage As Forage) As Boolean
            If Forage Is Nothing Then Return False
            If String.IsNullOrEmpty(Forage.RefForage) Then Return False

            Dim index As Integer = GetIndexForage(Forage.RefForage)
            If index < 0 Then Return False

            ListeForages(index) = Forage
            _isModified = True
            Return True
        End Function

        Public Function GetIndexForage(refForage As String) As Integer
            If String.IsNullOrEmpty(refForage) Then Return -1
            For i As Integer = 0 To ListeForages.Count - 1
                Dim s As Forage = ListeForages(i)
                If s IsNot Nothing AndAlso s.RefForage = refForage Then
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

            For Each s As Forage In ListeForages
                If s Is Nothing Then Continue For
                If excludeExisting IsNot Nothing AndAlso ReferenceEquals(s, excludeExisting) Then
                    Continue For
                End If
                If s.RefForage = refForage Then
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
            ListeForages.Clear()
            _isModified = False

            Dim dt As DataTable = _dataService.ObjetDataLesForages
            If dt Is Nothing Then Return

            For Each row As DataRow In dt.Rows
                Dim Forage As New Forage()

                If dt.Columns.Contains("refChantier") AndAlso Not row.IsNull("refChantier") Then
                    Forage.RefChantier = CStr(row("refChantier"))
                End If
                If dt.Columns.Contains("refForage") AndAlso Not row.IsNull("refForage") Then
                    Forage.RefForage = CStr(row("refForage"))
                End If
                If dt.Columns.Contains("numeroForage") AndAlso Not row.IsNull("numeroForage") Then
                    Forage.numeroForage = CStr(row("numeroForage"))
                End If
                If dt.Columns.Contains("nomForage") AndAlso Not row.IsNull("nomForage") Then
                    Forage.NomForage = CStr(row("nomForage"))
                End If
                If dt.Columns.Contains("DateForage") AndAlso Not row.IsNull("DateForage") Then
                    Forage.dateForage = CStr(row("DateForage"))
                End If
                If dt.Columns.Contains("materiel") AndAlso Not row.IsNull("materiel") Then
                    Forage.materiel = CStr(row("materiel"))
                End If
                If dt.Columns.Contains("outil") AndAlso Not row.IsNull("outil") Then
                    Forage.outil = CStr(row("outil"))
                End If
                If dt.Columns.Contains("abscisseX") AndAlso Not row.IsNull("abscisseX") Then
                    Forage.abscisseX = CStr(row("abscisseX"))
                End If
                If dt.Columns.Contains("ordonneeY") AndAlso Not row.IsNull("ordonneeY") Then
                    Forage.ordonneeY = CStr(row("ordonneeY"))
                End If
                If dt.Columns.Contains("altitudeZ") AndAlso Not row.IsNull("altitudeZ") Then
                    Forage.altitudeZ = CStr(row("altitudeZ"))
                End If
                If dt.Columns.Contains("coteInit") AndAlso Not row.IsNull("coteInit") Then
                    Forage.coteInit = CStr(row("coteInit"))
                End If
                If dt.Columns.Contains("coteFinale") AndAlso Not row.IsNull("coteFinale") Then
                    Forage.coteFinale = CSng(row("coteFinale"))
                End If

                ListeForages.Add(Forage)
            Next

            ' Mise à jour du compteur pour GiveMeANewRefForage.
            Dim maxCounter As Integer = 0
            For Each s As Forage In ListeForages
                If s Is Nothing OrElse String.IsNullOrEmpty(s.RefForage) Then Continue For
                Dim prefix As String = "Forage-"
                If s.RefForage.StartsWith(prefix) Then
                    Dim n As Integer
                    If Integer.TryParse(s.RefForage.Substring(prefix.Length), n) Then
                        If n > maxCounter Then maxCounter = n
                    End If
                End If
            Next
            _nextRefForageCounter = maxCounter
        End Sub

        ' ✅ BON : ne copier que les propriétés de Forage
        Public Sub SaveForagesToDataTable()
            Dim dt As DataTable = _dataService.ObjetDataLesForages
            If dt Is Nothing Then Return

            ' Effacer les anciennes lignes
            dt.Rows.Clear()

            For Each Forage As Forage In ListeForages
                If Forage Is Nothing Then Continue For

                Dim row As DataRow = dt.NewRow()
                row("RefChantier") = Forage.RefChantier
                row("RefForage") = Forage.RefForage
                row("NumeroForage") = Forage.numeroForage
                row("NomForage") = Forage.NomForage
                row("DateForage") = Forage.dateForage
                row("Materiel") = Forage.materiel
                row("Outil") = Forage.outil
                row("AbscisseX") = Forage.abscisseX
                row("OrdonneeY") = Forage.ordonneeY
                row("AltitudeZ") = Forage.altitudeZ
                row("CoteInit") = Forage.coteInit
                row("CoteFinale") = Forage.coteFinale

                dt.Rows.Add(row)
            Next

            _isModified = False
        End Sub

    End Class

End Namespace

