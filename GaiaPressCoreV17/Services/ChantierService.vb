Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports GaiaPressSolution.GaiaPressCoreV17.Services
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service de gestion en mémoire des Chantier.
    ''' Cette classe remplace l'ancien contexte LesSols pour la gestion des Chantier.
    ''' </summary>
    Public Class ChantierService

        Private ReadOnly _dataService As DataService ' Injection de DataServices

        Private _dbContext As GaiaPressDbContext

        Public Property LesChantier As List(Of Chantier)
        Public Property FichierListeLesChantier As String
        Public Property EventsSuspendus As Boolean
        Public Property IsSaved As Boolean

        Public Event HasBeenModified()

        Public Sub New(dataService As DataService, dbContext As GaiaPressDbContext)
            _dataService = dataService
            _dbContext = dbContext
            LesChantier = New List(Of Chantier)()
            FichierListeLesChantier = String.Empty
            EventsSuspendus = False
            IsSaved = True
            _dbContext = dbContext
        End Sub

        'Public Sub New(dataService As DataService)
        '    _dataService = dataService
        'End Sub

        ''' <summary>
        ''' Ajoute un chantier à la liste en mémoire.
        ''' </summary>
        Public Sub AddChantier(chantier As Chantier)
            If chantier Is Nothing Then Return

            LesChantier.Add(chantier)
            IHaveBeenModified()
        End Sub

        ''' <summary>
        ''' Recherche un chantier par sa référence.
        ''' </summary>
        Public Function GetChantier(refChantier As String) As Chantier
            Return _dbContext.LesChantiers.FirstOrDefault(Function(c) c.refChantier = refChantier)
        End Function

        Public Function GetAllChantiers() As List(Of Chantier)
            Return _dataService.GetAllChantier()
        End Function


        ''' <summary>
        ''' Génère une nouvelle référence de chantier unique.
        ''' </summary>
        Public Function GiveMeANewRefChantier() As String
            Dim k As Integer = 1
            Dim nouvelleReference As String

            Do
                nouvelleReference = "Chantier " & k.ToString()
                k += 1
            Loop Until IsRefChantierUnique(nouvelleReference)

            Return nouvelleReference
        End Function

        ''' <summary>
        ''' Vérifie qu'une référence n'est pas déjà utilisée.
        ''' </summary>
        Public Function IsRefChantierUnique(refChantier As String) As Boolean
            Return Not LesChantier.Any(Function(c) c IsNot Nothing AndAlso c.refChantier = refChantier)
        End Function

        ''' <summary>
        ''' Met à jour le chantier correspondant à la référence fournie.
        ''' </summary>
        Public Sub UpdateChantier(chantier As Chantier)
            If chantier Is Nothing Then Return

            Dim chantierExistant As Chantier = GetChantier(chantier.refChantier)
            If chantierExistant Is Nothing Then
                LesChantier.Add(chantier)
            Else
                Dim index As Integer = LesChantier.IndexOf(chantierExistant)
                LesChantier(index) = chantier
            End If

            IHaveBeenModified()
        End Sub

        ''' <summary>
        ''' Supprime un chantier de la liste en mémoire.
        ''' </summary>
        Public Sub SupprimeChantier(chantier As Chantier)
            If chantier Is Nothing Then Return

            If LesChantier.Remove(chantier) Then
                IHaveBeenModified()
            End If
        End Sub

        Public Sub SaveChantier(chantier As Chantier)
            _dbContext.LesChantiers.Update(chantier)
            _dbContext.SaveChanges()
        End Sub



        ''' <summary>
        ''' Recharge les Chantier depuis la base de données.
        ''' </summary>
        Public Sub ChargerDepuisBase()
            If _dataService Is Nothing Then Return

            Dim Chantier As List(Of Chantier) = _dataService.GetAllChantier()
            If Chantier Is Nothing Then
                LesChantier = New List(Of Chantier)()
            Else
                LesChantier = New List(Of Chantier)(Chantier)
            End If

            IsSaved = True
        End Sub

        ''' <summary>
        ''' Enregistre les Chantier dans la base de données.
        ''' </summary>
        'Public Function EnregistreLesChantier() As Boolean
        '    If _dataService Is Nothing Then Return False

        '    Try
        '        _dataService.SaveAllChantiers(ListeChantier)
        '        IsSaved = True
        '        Return True
        '    Catch ex As Exception
        '        ' L'appelant peut journaliser l'exception selon les besoins de l'application.
        '        IsSaved = False
        '        Return False
        '    End Try
        'End Function
        ' --- Méthode pour sauvegarder un chantier ---
        Public Function EnregistreLesChantier(myChantier As Chantier) As Boolean
            Try
                _dataService.SaveAllChantiers()
                Return True
            Catch ex As Exception
                ' Journaliser l'erreur si nécessaire
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Retourne le nom du fichier associé à la liste des Chantier.
        ''' </summary>
        Public Function GetFichierName() As String
            If String.IsNullOrWhiteSpace(FichierListeLesChantier) Then Return String.Empty
            Return Path.GetFileName(FichierListeLesChantier)
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
        ' Public Function GetAllChantier() As List(Of Chantier)
        '     Return _context.LesChantierss.ToList()
        ' End Function
        '
        ' Public Sub SaveChantier(Chantier As List(Of Chantier))
        '     _context.LesChantierss.UpdateRange(Chantier)
        '     _context.SaveChanges()
        ' End Sub
        ' -------------------------------------------------------------------------

    End Class

End Namespace

