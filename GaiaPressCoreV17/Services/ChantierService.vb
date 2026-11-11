Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service de gestion en mémoire des ConstructionSite.
    ''' Cette classe remplace l'ancien contexte LesSols pour la gestion des ConstructionSite.
    ''' </summary>
    Public Class ConstructionSiteService

        Private ReadOnly _dataService As DataService ' Injection de DataServices

        Private _dbContext As GaiaPressDbContext

        Public Property LesConstructionSite As List(Of ConstructionSite)
        Public Property FichierListeLesConstructionSite As String
        Public Property EventsSuspendus As Boolean
        Public Property IsSaved As Boolean

        Public Event HasBeenModified()

        Public Sub New(dataService As DataService, dbContext As GaiaPressDbContext)
            _dataService = dataService
            _dbContext = dbContext
            LesConstructionSite = New List(Of ConstructionSite)()
            FichierListeLesConstructionSite = String.Empty
            EventsSuspendus = False
            IsSaved = True
            _dbContext = dbContext
        End Sub

        'Public Sub New(dataService As DataService)
        '    _dataService = dataService
        'End Sub

        ''' <summary>
        ''' Ajoute un ConstructionSite à la liste en mémoire.
        ''' </summary>
        Public Sub AddConstructionSite(ConstructionSite As ConstructionSite)
            If ConstructionSite Is Nothing Then Return

            LesConstructionSite.Add(ConstructionSite)
            IHaveBeenModified()
        End Sub

        ''' <summary>
        ''' Recherche un ConstructionSite par sa référence.
        ''' </summary>
        Public Function GetConstructionSite(RefChantier As String) As ConstructionSite
            Return _dbContext.ConstructionSitesDb.FirstOrDefault(Function(c) c.RefChantier = RefChantier)
        End Function

        Public Function GetAllConstructionSites() As List(Of ConstructionSite)
            Return _dataService.GetAllConstructionSite()
        End Function


        ''' <summary>
        ''' Génère une nouvelle référence de ConstructionSite unique.
        ''' </summary>
        Public Function GiveMeANewRefChantier() As String
            Dim k As Integer = 1
            Dim nouvelleReference As String

            Do
                nouvelleReference = "ConstructionSite " & k.ToString()
                k += 1
            Loop Until IsRefChantierUnique(nouvelleReference)

            Return nouvelleReference
        End Function

        ''' <summary>
        ''' Vérifie qu'une référence n'est pas déjà utilisée.
        ''' </summary>
        Public Function IsRefChantierUnique(RefChantier As String) As Boolean
            Return Not LesConstructionSite.Any(Function(c) c IsNot Nothing AndAlso c.RefChantier = RefChantier)
        End Function

        ''' <summary>
        ''' Met à jour le ConstructionSite correspondant à la référence fournie.
        ''' </summary>
        Public Sub UpDateConstructionSite(ConstructionSite As ConstructionSite)
            If ConstructionSite Is Nothing Then Return

            Dim ConstructionSiteExistant As ConstructionSite = GetConstructionSite(ConstructionSite.RefChantier)
            If ConstructionSiteExistant Is Nothing Then
                LesConstructionSite.Add(ConstructionSite)
            Else
                Dim index As Integer = LesConstructionSite.IndexOf(ConstructionSiteExistant)
                LesConstructionSite(index) = ConstructionSite
            End If

            IHaveBeenModified()
        End Sub

        ''' <summary>
        ''' Supprime un ConstructionSite de la liste en mémoire.
        ''' </summary>
        Public Sub SupprimeConstructionSite(ConstructionSite As ConstructionSite)
            If ConstructionSite Is Nothing Then Return

            If LesConstructionSite.Remove(ConstructionSite) Then
                IHaveBeenModified()
            End If
        End Sub

        Public Sub SaveConstructionSite(ConstructionSite As ConstructionSite)
            _dbContext.ConstructionSitesDb.Update(ConstructionSite)
            _dbContext.SaveChanges()
        End Sub



        ''' <summary>
        ''' Recharge les ConstructionSite depuis la base de données.
        ''' </summary>
        Public Sub ChargerDepuisBase()
            If _dataService Is Nothing Then Return

            Dim ConstructionSite As List(Of ConstructionSite) = _dataService.GetAllConstructionSite()
            If ConstructionSite Is Nothing Then
                LesConstructionSite = New List(Of ConstructionSite)()
            Else
                LesConstructionSite = New List(Of ConstructionSite)(ConstructionSite)
            End If

            IsSaved = True
        End Sub

        ''' <summary>
        ''' Enregistre les ConstructionSite dans la base de données.
        ''' </summary>
        'Public Function EnregistreLesConstructionSite() As Boolean
        '    If _dataService Is Nothing Then Return False

        '    Try
        '        _dataService.SaveAllConstructionSites(ListeConstructionSite)
        '        IsSaved = True
        '        Return True
        '    Catch ex As Exception
        '        ' L'appelant peut journaliser l'exception selon les besoins de l'application.
        '        IsSaved = False
        '        Return False
        '    End Try
        'End Function
        ' --- Méthode pour sauvegarder un ConstructionSite ---
        Public Function EnregistreLesConstructionSite(myConstructionSite As ConstructionSite) As Boolean
            Try
                _dataService.SaveAllConstructionSites()
                Return True
            Catch ex As Exception
                ' Journaliser l'erreur si nécessaire
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Retourne le nom du fichier associé à la liste des ConstructionSite.
        ''' </summary>
        Public Function GetFichierName() As String
            If String.IsNullOrWhiteSpace(FichierListeLesConstructionSite) Then Return String.Empty
            Return Path.GetFileName(FichierListeLesConstructionSite)
        End Function

        ''' <summary>
        ''' Indique que les données ont été modifiées.
        ''' </summary>
        Public Sub IHaveBeenModified()
            If Not EventsSuspendus Then
                IsSaved = False
                RaiseEvent HasBeenModified()
            End If
        End Sub

        ''' <summary>
        ''' Suspend la notification des modifications.
        ''' </summary>
        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        ''' <summary>
        ''' Rétablit la notification des modifications.
        ''' </summary>
        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub

        ' Si ces méthodes ne sont pas encore présentes dans DataService, les ajouter :
        ' -------------------------------------------------------------------------
        ' Public Function GetAllConstructionSite() As List(Of ConstructionSite)
        '     Return _context.ConstructionsSitesDbs.ToList()
        ' End Function
        '
        ' Public Sub SaveConstructionSite(ConstructionSite As List(Of ConstructionSite))
        '     _context.ConstructionsSitesDbs.UpdateRange(ConstructionSite)
        '     _context.SaveChanges()
        ' End Sub
        ' -------------------------------------------------------------------------

    End Class

End Namespace

