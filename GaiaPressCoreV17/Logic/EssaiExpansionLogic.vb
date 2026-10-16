Imports System.Threading
Imports GaiaPressCoreV17
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Services
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports QuestPDF.Fluent

Namespace GaiaPressCoreV17.Logic

    ''' <summary>
    ''' Logique métier pour la gestion des essais d'expansion.
    ''' </summary>
    Public Class EssaiExpansionLogic
        Inherits ControllerBase

        Private ReadOnly _essaisExpansionService As EssaiExpansionService
        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de EssaiExpansionLogic.
        ''' </summary>
        ''' <param name="essaisExpansionService">Service pour les essais d'expansion.</param>
        ''' <param name="dbContext">Contexte Debase de données.</param>
        Public Sub New(essaisExpansionService As EssaiExpansionService,
               dbContext As GaiaPressDbContext)

            _essaisExpansionService = essaisExpansionService
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'expansion avec une logique métier spécifique.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'expansion.</returns>
        Public Async Function GetAllEssaisExpansion() As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisExpansionService.GetAllEssaisExpansion()
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'expansion : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai d'expansion par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'expansion.</param>
        ''' <returns>Essai d'expansion correspondant ou NotFound.</returns>
        Public Async Function GetEssaiExpansionByRef(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _essaisExpansionService.GetEssaiExpansionByRef(refEssai)
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai d'expansion avec la référence {refEssai} introuvable."})
                End If
                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération de l'essai d'expansion : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Ajoute un essai d'expansion avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="essai">Essai d'expansion à ajouter.</param>
        ''' <returns>Essai d'expansion ajouté ou BadRequest en cas d'erreur.</returns>
        Public Async Function AddEssaiExpansion(essai As EssaiExpansion) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant l'ajout
                If String.IsNullOrEmpty(essai.RefEssai) Then
                    Return BadRequest(New With {Key .Message = "La référence de l'essai ne peut pas être vide."})
                End If

                If String.IsNullOrEmpty(essai.nomEssai) Then
                    essai.nomEssai = "Essai sans nom"
                End If

                Dim newEssai = Await _essaisExpansionService.AddEssaiExpansion(essai)
                Return CreatedAtAction(NameOf(GetEssaiExpansionByRef), New With {Key .refEssai = newEssai.RefEssai}, newEssai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de l'essai d'expansion : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour un essai d'expansion avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'expansion avec les nouvelles valeurs.</param>
        ''' <returns>Ok si la mise à jour a réussi, NotFound ou BadRequest sinon.</returns>
        Public Async Function UpdateEssaiExpansion(refEssai As String, essai As EssaiExpansion) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant la mise à jour
                If String.IsNullOrEmpty(essai.nomEssai) Then
                    essai.nomEssai = "Essai sans nom"
                End If

                Dim success = Await _essaisExpansionService.UpdateEssaiExpansion(refEssai, essai)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Essai d'expansion avec la référence {refEssai} introuvable."})
                End If
                Return Ok()
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour de l'essai d'expansion : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime un essai d'expansion par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteEssaiExpansion(refEssai As String) As Task(Of IActionResult)
            Try
                Dim success = Await _essaisExpansionService.DeleteEssaiExpansion(refEssai)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Essai d'expansion avec la référence {refEssai} introuvable."})
                End If
                Return Ok(New With {Key .Message = $"Essai d'expansion {refEssai} supprimé avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression de l'essai d'expansion : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de forage avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'expansion pour le forage spécifié.</returns>
        Public Async Function GetEssaisExpansionByForage(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisExpansionService.GetEssaisExpansionByRefEssai(refEssai)
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'expansion pour l Essai {refEssai} : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de chantier avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'expansion pour le chantier spécifié.</returns>
        Public Async Function GetEssaisExpansionByrefSonde(refSonde As String) As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisExpansionService.GetEssaiExpansionByRef(refSonde)
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'expansion pour la Sonde {refSonde} : {ex.Message}"})
            End Try
        End Function

        Public Async Function GenerateRapportEssaiExpansion(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _essaisExpansionService.GetEssaiExpansionByRef(refEssai)
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai d'expansion avec la référence {refEssai} introuvable."})
                End If

                ' ✅ Correction complète de la génération du PDF avec QuestPDF
                ' Générer le PDF avec QuestPDF
                Dim pdfBytes = Document.Create(Sub(doc)
                                                   doc.Page(Sub(page)
                                                                page.Content().
                                                                    PaddingVertical(20).
                                                                    Column(Sub(column)
                                                                               ' Titre
                                                                               column.Item().Text("Rapport pour l'essai d'expansion : " & essai.nomEssai).FontSize(20).Bold()

                                                                               ' Espacement
                                                                               column.Item().PaddingVertical(10)

                                                                               ' Référence
                                                                               column.Item().Text("Référence : " & essai.RefEssai).FontSize(14)

                                                                               ' Espacement
                                                                               column.Item().PaddingVertical(5)

                                                                               ' Date
                                                                               column.Item().Text("Date : " & essai.dateEssai.ToString("dd/MM/yyyy")).FontSize(12)
                                                                           End Sub)
                                                            End Sub)
                                               End Sub).GeneratePdf()

                ' Retourner le PDF
                Return File(pdfBytes, "application/pdf", $"rapport_mesure{refEssai}.pdf")
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la génération du rapport : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace
