Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading
Imports GaiaPressCoreV17.Services


Namespace GaiaPressCoreV17.Logic

    ''' <summary>
    ''' Logique métier pour la gestion des sols.
    ''' </summary>
    Public Class SolsLogic
        Inherits ControllerBase

        Private ReadOnly _solService As SolsService
        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de SolLogic.
        ''' </summary>
        ''' <param name="solService">Service pour les sols.</param>
        ''' <param name="dbContext">Contexte Debase de données.</param>
        Public Sub New(solService As SolsService, dbContext As GaiaPressDbContext)
            _solService = solService
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les sols avec une logique métier spécifique.
        ''' </summary>
        ''' <returns>Liste de tous les sols.</returns>
        Public Async Function GetAllSols() As Task(Of IActionResult)
            Try
                Dim sols = Await _solService.GetAllSols()
                Return Ok(sols)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des sols : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un sol par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refSol">Référence du sol.</param>
        ''' <returns>Sol correspondant ou NotFound.</returns>
        Public Async Function GetSolByRef(refSol As String) As Task(Of IActionResult)
            Try
                Dim sol = Await _solService.GetSolByRefSol(refSol)
                If sol Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Sol avec la référence {refSol} introuvable."})
                End If
                Return Ok(sol)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération du sol : {ex.Message}"})
            End Try
        End Function

        '''' <summary>
        '''' Récupère les sols par référence de chantier avec une logique métier spécifique.
        '''' </summary>
        '''' <param name="refSol">Référence du chantier.</param>
        '''' <returns>Liste des sols pour le chantier spécifié.</returns>
        'Public Async Function GetSolsByrefSol(refSol As String) As Task(Of IActionResult)
        '    Try
        '        Dim sols = Await _solService.GetSolByRefSol(refSol)
        '        Return Ok(sols)
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des sols pour le chantier {refSol} : {ex.Message}"})
        '    End Try
        'End Function

        ''' <summary>
        ''' Ajoute un nouveau sol avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="sol">Sol à ajouter.</param>
        ''' <returns>Sol ajouté ou BadRequest en cas d'erreur.</returns>
        Public Async Function AddSol(sol As Sol) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant l'ajout
                If String.IsNullOrEmpty(sol.RefSol) Then
                    Return BadRequest(New With {Key .Message = "La référence du sol ne peut pas être vide."})
                End If

                If String.IsNullOrEmpty(sol.nomSol) Then
                    sol.nomSol = "Sol sans nom"
                End If

                If String.IsNullOrEmpty(sol.nomSol) Then
                    sol.nomSol = "Inconnu"
                End If

                Dim newSol = Await _solService.AddSol(sol)
                Return CreatedAtAction(NameOf(GetSolByRef), New With {Key .refSol = newSol.RefSol}, newSol)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout du sol : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour un sol avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refSol">Référence du sol à mettre à jour.</param>
        ''' <param name="sol">Sol avec les nouvelles valeurs.</param>
        ''' <returns>Ok si la mise à jour a réussi, NotFound ou BadRequest sinon.</returns>
        Public Async Function UpdateSol(refSol As String, sol As Sol) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant la mise à jour
                If String.IsNullOrEmpty(sol.nomSol) Then
                    sol.nomSol = "Sol sans nom"
                End If

                If String.IsNullOrEmpty(sol.nomSol) Then
                    sol.nomSol = "Inconnu"
                End If

                Dim success = Await _solService.UpdateSol(refSol, sol)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Sol avec la référence {refSol} introuvable."})
                End If
                Return Ok()
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour du sol : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime un sol par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refSol">Référence du sol à supprimer.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteSol(refSol As String) As Task(Of IActionResult)
            Try
                Dim success = Await _solService.DeleteSol(refSol)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Sol avec la référence {refSol} introuvable."})
                End If
                Return Ok(New With {Key .Message = $"Sol {refSol} supprimé avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression du sol : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime tous les sols associés à un chantier avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refSol">Référence du chantier.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteSolsByrefSol(refSol As String) As Task(Of IActionResult)
            Try
                Dim success = Await _solService.DeleteSolsByrefSol(refSol)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Aucun sol trouvé pour le chantier {refSol}."})
                End If
                Return Ok(New With {Key .Message = $"Sols du chantier {refSol} supprimés avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression des sols du chantier : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace