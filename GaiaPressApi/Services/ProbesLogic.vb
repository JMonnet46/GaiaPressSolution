Namespace GaiaPress.Core.V17.Logic

    ''' <summary>
    ''' Logique métier pour la gestion des sondes (Probes).
    ''' </summary>
    Public Class ProbesLogic
        Inherits ControllerBase

        Private ReadOnly _probesService As ProbesService
        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de ProbesLogic.
        ''' </summary>
        ''' <param name="probesService">Service pour les sondes.</param>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(probesService As ProbesService, dbContext As GaiaPressDbContext)
            _probesService = probesService
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère toutes les sondes avec une logique métier spécifique.
        ''' </summary>
        ''' <returns>Liste de toutes les sondes.</returns>
        Public Async Function GetAllProbes() As Task(Of IActionResult)
            Try
                Dim probes = Await _probesService.GetAllProbes()
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
                Dim probe = Await _probesService.GetProbeByRef(refProbe)
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
        '        Dim probes = Await _probesService.GetProbesByChantier(refChantier)
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
                If String.IsNullOrEmpty(probe.P_refSonde) Then
                    Return BadRequest(New With {Key .Message = "La référence de la sonde ne peut pas être vide."})
                End If

                If String.IsNullOrEmpty(probe.P_typeSonde) Then
                    probe.P_typeSonde = "Inconnu"
                End If

                'If probe.P_Profondeu <= 0 Then
                '    Return BadRequest(New With {Key .Message = "La profondeur de la sonde doit être supérieure à 0."})
                'End If

                Dim newProbe = Await _probesService.AddProbe(probe)
                Return CreatedAtAction(NameOf(GetProbeByRef), New With {Key .refProbe = newProbe.P_refSonde}, newProbe)
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
        Public Async Function UpdateProbe(refProbe As String, probe As Probe) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant la mise à jour
                If String.IsNullOrEmpty(probe.P_typeSonde) Then
                    probe.P_typeSonde = "Inconnu"
                End If

                'If probe.P_Profondeur <= 0 Then
                '    Return BadRequest(New With {Key .Message = "La profondeur de la sonde doit être supérieure à 0."})
                'End If

                Dim success = Await _probesService.UpdateProbe(refProbe, probe)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Probe avec la référence {refProbe} introuvable."})
                End If
                Return Ok()
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour de la sonde : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime une sonde par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde à supprimer.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteProbe(refProbe As String) As Task(Of IActionResult)
            Try
                Dim success = Await _probesService.DeleteProbe(refProbe)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Probe avec la référence {refProbe} introuvable."})
                End If
                Return Ok(New With {Key .Message = $"Probe {refProbe} supprimée avec succès."})
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
        '        Dim success = Await _probesService.DeleteProbesByChantier(refChantier)
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