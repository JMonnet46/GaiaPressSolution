Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Services
Imports System.Threading

Namespace GaiaPressCoreV17.Logic

    ''' <summary>
    ''' Logique métier pour la gestion des sondes (Probes).
    ''' </summary>
    Public Class SondeLogic
        Inherits ControllerBase

        Private ReadOnly _dbContext As GaiaPressDbContext
        Private ReadOnly _SondeService As SondeService

        ''' <summary>
        ''' Initialise une nouvelle instance de SondeLogic.
        ''' </summary>
        ''' <param name="SondeService">Service pour les sondes.</param>
        ''' <param name="dbContext">Contexte Debase de données.</param>
        Public Sub New(SondeService As SondeService, dbContext As GaiaPressDbContext)
            _SondeService = SondeService
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère toutes les sondes avec une logique métier spécifique.
        ''' </summary>
        ''' <returns>Liste de toutes les sondes.</returns>
        Public Async Function GetAllProbes() As Task(Of IActionResult)
            Try
                Dim probes = Await GetAllProbes()
                Return Ok(probes)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des sondes : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère une sonde par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde.</param>
        ''' <returns>Sonde correspondante ou NotFound.</returns>
        Public Async Function GetProbeByRef(refProbe As String) As Task(Of IActionResult)
            Try
                Dim probe = Await GetProbeByRef(refProbe)
                If probe Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Probe avec la référence {refProbe} introuvable."})
                End If
                Return Ok(probe)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération de la sonde : {ex.Message}"})
            End Try
        End Function

        '''' <summary>
        '''' Récupère les sondes par référence de chantier avec une logique métier spécifique.
        '''' </summary>
        '''' <param name="refChantier">Référence du chantier.</param>
        '''' <returns>Liste des sondes pour le chantier spécifié.</returns>
        'Public Async Function GetProbesByChantier(refChantier As String) As Task(Of IActionResult)
        '    Try
        '        Dim probes = Await _SondeService.GetProbesByChantier(refChantier)
        '        Return Ok(probes)
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des sondes pour le chantier {refChantier} : {ex.Message}"})
        '    End Try
        'End Function

        ''' <summary>
        ''' Ajoute une nouvelle sonde avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="probe">Sonde à ajouter.</param>
        ''' <returns>Sonde ajoutée ou BadRequest en cas d'erreur.</returns>
        Public Async Function AddProbe(probe As Probe) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant l'ajout
                If String.IsNullOrEmpty(probe.RefSonde) Then
                    Return BadRequest(New With {Key .Message = "La référence de la sonde ne peut pas être vide."})
                End If

                If String.IsNullOrEmpty(probe.TypeSonde) Then
                    probe.TypeSonde = "Inconnu"
                End If

                'If probe.Profondeu <= 0 Then
                '    Return BadRequest(New With {Key .Message = "La profondeur de la sonde doit être supérieure à 0."})
                'End If

                Dim newProbe = Await AddProbe(probe)

                Return CreatedAtAction(NameOf(GetProbeByRef), New With {Key .refProbe = probe.RefSonde}, probe)
            Catch ex As ArgumentException
                Return BadRequest(New With {Key .Message = ex.Message})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de la sonde : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour une sonde avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde à mettre à jour.</param>
        ''' <param name="probe">Sonde avec les nouvelles valeurs.</param>
        ''' <returns>Ok si la mise à jour a réussi, NotFound ou BadRequest sinon.</returns>
        Public Async Function UpdateProbe(RefSonde As String, probe As Probe) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant la mise à jour
                If String.IsNullOrEmpty(probe.TypeSonde) Then
                    probe.TypeSonde = "Inconnu"
                End If

                'If probe.Profondeur <= 0 Then
                '    Return BadRequest(New With {Key .Message = "La profondeur de la sonde doit être supérieure à 0."})
                'End If

                _SondeService.UpdateSonde(probe)
                Return Ok(New With {Key .Message = "Sonde mise à jour avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour de la sonde : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime une sonde par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde à supprimer.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteProbe(refSonde As String) As Task(Of IActionResult)
            Try
                _SondeService.DeleteSonde(refSonde)
                Return Ok(New With {Key .Message = $"Sonde {refSonde} supprimée avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression de la sonde : {ex.Message}"})
            End Try
        End Function

        '''' <summary>
        '''' Supprime toutes les sondes associées à un chantier avec une logique métier spécifique.
        '''' </summary>
        '''' <param name="refChantier">Référence du chantier.</param>
        '''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        'Public Async Function DeleteProbesByChantier(refChantier As String) As Task(Of IActionResult)
        '    Try
        '        Dim success = Await _SondeService.DeleteProbesByChantier(refChantier)
        '        If Not success Then
        '            Return NotFound(New With {Key .Message = $"Aucune probe trouvée pour le chantier {refChantier}."})
        '        End If
        '        Return Ok(New With {Key .Message = $"Probes du chantier {refChantier} supprimées avec succès."})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression des sondes du chantier : {ex.Message}"})
        '    End Try
        'End Function
    End Class
End Namespace
