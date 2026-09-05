Namespace GaiaPress.Core.V17.Logic

    ''' <summary>
    ''' Logique métier pour la gestion des essais d'Resistance.
    ''' </summary>
    Public Class EssaiResistanceLogic
        Inherits Controller

        Private ReadOnly _essaisResistanceService As EssaisResistanceService
        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de EssaiResistanceLogic.
        ''' </summary>
        ''' <param name="essaisResistanceService">Service pour les essais d'Resistance.</param>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(essaisResistanceService As EssaisResistanceService, dbContext As GaiaPressDbContext)
            _essaisResistanceService = essaisResistanceService
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'Resistance avec une logique métier spécifique.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'Resistance.</returns>
        Public Async Function GetAllEssaisResistance() As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisResistanceService.GetAllEssaisResistance()
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'Resistance : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai d'Resistance par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'Resistance.</param>
        ''' <returns>Essai d'Resistance correspondant ou NotFound.</returns>
        Public Async Function GetEssaiResistanceByRef(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _essaisResistanceService.GetEssaiResistanceByRef(refEssai)
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai d'Resistance avec la référence {refEssai} introuvable."})
                End If
                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération de l'essai d'Resistance : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Ajoute un essai d'Resistance avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="essai">Essai d'Resistance à ajouter.</param>
        ''' <returns>Essai d'Resistance ajouté ou BadRequest en cas d'erreur.</returns>
        Public Async Function AddEssaiResistance(essai As Essai_Resistance) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant l'ajout
                If String.IsNullOrEmpty(essai.E_RefEssai) Then
                    Return BadRequest(New With {Key .Message = "La référence de l'essai ne peut pas être vide."})
                End If

                If String.IsNullOrEmpty(essai.R_nomEssai) Then
                    essai.R_nomEssai = "Essai sans nom"
                End If

                Dim newEssai = Await _essaisResistanceService.AddEssaiResistance(essai)
                Return CreatedAtAction(NameOf(GetEssaiResistanceByRef), New With {Key .refEssai = newEssai.E_RefEssai}, newEssai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de l'essai d'Resistance : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour un essai d'Resistance avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'Resistance avec les nouvelles valeurs.</param>
        ''' <returns>Ok si la mise à jour a réussi, NotFound ou BadRequest sinon.</returns>
        Public Async Function UpdateEssaiResistance(refEssai As String, essai As Essai_Resistance) As Task(Of IActionResult)
            Try
                ' Logique métier spécifique avant la mise à jour
                If String.IsNullOrEmpty(essai.R_nomEssai) Then
                    essai.R_nomEssai = "Essai sans nom"
                End If

                Dim success = Await _essaisResistanceService.UpdateEssaiResistance(refEssai, essai)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Essai d'Resistance avec la référence {refEssai} introuvable."})
                End If
                Return Ok()
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour de l'essai d'Resistance : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime un essai d'Resistance par sa référence avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>Ok si la suppression a réussi, NotFound sinon.</returns>
        Public Async Function DeleteEssaiResistance(refEssai As String) As Task(Of IActionResult)
            Try
                Dim success = Await _essaisResistanceService.DeleteEssaiResistance(refEssai)
                If Not success Then
                    Return NotFound(New With {Key .Message = $"Essai d'Resistance avec la référence {refEssai} introuvable."})
                End If
                Return Ok(New With {Key .Message = $"Essai d'Resistance {refEssai} supprimé avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression de l'essai d'Resistance : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de forage avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'Resistance pour le forage spécifié.</returns>
        Public Async Function GetEssaisResistanceByForage(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisResistanceService.GetEssaisResistanceByRefEssai(refEssai)
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'Resistance pour l Essai {refEssai} : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de chantier avec une logique métier spécifique.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'Resistance pour le chantier spécifié.</returns>
        Public Async Function GetEssaisResistanceByrefSonde(refSonde As String) As Task(Of IActionResult)
            Try
                Dim essais = Await _essaisResistanceService.GetEssaisResistanceByRefSonde(refSonde)
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais d'Resistance pour la Sonde {refSonde} : {ex.Message}"})
            End Try
        End Function

        Public Async Function GenerateRapportEssaiResistance(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _essaisResistanceService.GetEssaiResistanceByRef(refEssai)
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai d'Resistance avec la référence {refEssai} introuvable."})
                End If

                ' ✅ Correction complète de la génération du PDF avec QuestPDF
                ' Générer le PDF avec QuestPDF
                Dim pdfBytes = Document.Create(Sub(doc)
                                                   doc.Page(Sub(page)
                                                                page.Content().
                                                                    PaddingVertical(20).
                                                                    Column(Sub(column)
                                                                               ' Titre
                                                                               column.Item().Text("Rapport pour l'essai d'Resistance : " & essai.R_nomEssai).FontSize(20).Bold()

                                                                               ' Espacement
                                                                               column.Item().PaddingVertical(10)

                                                                               ' Référence
                                                                               column.Item().Text("Référence : " & essai.e_refEssai).FontSize(14)

                                                                               ' Espacement
                                                                               column.Item().PaddingVertical(5)

                                                                               '' Date
                                                                               'column.Item().Text("Date : " & essai.R_dateEssai.ToString("dd/MM/yyyy")).FontSize(12)
                                                                               ' ✅ Correction avec IFormatProvider explicite
                                                                               column.Item().Text("Date : " & essai.E_dateEssai.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).FontSize(12)
                                                                           End Sub)
                                                            End Sub)
                                               End Sub).GeneratePdf()

                ' Retourner le PDF
                Return File(pdfBytes, "application/pdf", $"rapport_mesures_{refEssai}.pdf")
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la génération du rapport : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace