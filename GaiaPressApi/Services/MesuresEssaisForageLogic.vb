' Imports pour ASP.NET Core MVC
Imports System.IO
Imports System.Threading
' Imports pour GaiaPress
' Imports pour Entity Framework Core
' Imports pour QuestPDF


Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Classe de logique métier pour la gestion des mesures d'essais de forage.
    ''' </summary>
    Public Class MesuresEssaiForageLogic
        Inherits ControllerBase

        Public ReadOnly _context As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de MesuresEssaisForageLogic.
        ''' </summary>
        ''' <param name="context">Contexte de base de données.</param>
        Public Sub New(context As GaiaPressDbContext)
            _context = context
        End Sub

        ''' <summary>
        ''' Ajoute une mesure à un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <param name="mesure">Mesure à ajouter.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function AddMesureToEssai(refEssai As String, mesure As MesuresEssaiForage) As Task(Of IActionResult)
            Try
                ' ✅ Vérification des arguments
                If String.IsNullOrEmpty(refEssai) Then
                    Return BadRequest(New With {Key .Message = "La référence de l'essai ne peut pas être vide."})
                End If

                If mesure Is Nothing Then
                    Return BadRequest(New With {Key .Message = "La mesure ne peut pas être vide."})
                End If

                ' ✅ Assignation de la référence
                mesure.RefEssai = refEssai

                ' ✅ Ajout de la mesure
                _context.MesuresEssaiForage.Add(mesure)

                ' ✅ Sauvegarde asynchrone avec Await
                Await _context.SaveChangesAsync()

                ' ✅ Retour avec CreatedAtAction
                Return CreatedAtAction(NameOf(GetMesureByIndice), New With {Key .indice = mesure.indiceMesure}, mesure)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function


        ''' <summary>
        ''' Récupère une mesure par son indice.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure.</param>
        ''' <returns>Mesure correspondante ou NotFound.</returns>
        Public Async Function GetMesureByIndice(indice As Integer) As Task(Of IActionResult)
            Try
                Dim mesure = Await _context.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)

                If mesure Is Nothing Then
                    Return NotFound()
                End If

                Return Ok(mesure)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ' Propriété pour stocker la mesure actuelle
        Public Property MaMesure As MesuresEssaiForage
        Public Property MyMesure As MesuresEssaiForage

        ' Propriété pour stocker l'essai associé
        Public Property MonEssaiForage As Essai_Forage

        ''' <summary>
        ''' Définit la mesure actuelle.
        ''' </summary>
        Public Sub SetMyMesure(mesure As MesuresEssaiForage)
            MyMesure = mesure
        End Sub

        ''' <summary>
        ''' Définit l'essai associé.
        ''' </summary>
        Public Sub SetMonEssaiForage(essai As Essai_Forage)
            MonEssaiForage = essai
        End Sub


        ' ======================
        ' Gestion des Mesures d'Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère une mesure par son indice.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure.</param>
        ''' <returns>Mesure correspondante ou NotFound.</returns>
        Public Async Function GetMesure(indice As Integer) As Task(Of IActionResult)
            Dim mesure = Await _context.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)
            If mesure Is Nothing Then
                Return NotFound()
            End If
            Return Ok(mesure)
        End Function

        ''' <summary>
        ''' Met à jour une mesure.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure à mettre à jour.</param>
        ''' <param name="mesure">Données de la mesure mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpdateMesure(indice As Integer, mesure As MesuresEssaiForage) As Task(Of IActionResult)
            Dim existingMesure = Await _context.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)
            If existingMesure Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingMesure.indiceMesure = mesure.indiceMesure
            existingMesure.RefEssai = mesure.RefEssai
            ' Ajoutez d'autres propriétés selon votre modèle

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function

        ''' <summary>
        ''' Supprime une mesure par son indice.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure à Delete.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function DeleteMesure(indice As Integer) As Task(Of IActionResult)
            Dim mesure = Await _context.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)
            If mesure Is Nothing Then
                Return NotFound()
            End If

            _context.MesuresEssaiForage.Remove(mesure)
            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return BadRequest(ex.Message)
            End Try
        End Function

        ' ======================
        ' Gestion des Mesures par Essai
        ' ======================

        ''' <summary>
        ''' Récupère toutes les mesures pour un essai spécifique.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Liste des mesures de l'essai.</returns>
        Public Async Function GetMesuresByEssai(refEssai As String) As Task(Of IActionResult)
            'Dim mesures = Await _context.MesuresEssaiForage
            '.Where(Function(m) m.E_RefEssai = refEssai)
            '.ToListAsync()
            Dim mesures = Await _context.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync()

            If mesures Is Nothing OrElse mesures.Count = 0 Then
                Return NotFound()
            End If
            Return Ok(mesures)
        End Function

        ' ======================
        ' Génération de Rapports PDF
        ' ======================

        ''' <summary>
        ''' Génère un rapport PDF pour une mesure spécifique.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportMesure(indice As Integer) As Task(Of IActionResult)
            ' Récupérer la mesure
            Dim mesure = Await _context.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)
            If mesure Is Nothing Then
                Return NotFound()
            End If

            ' Récupérer l'essai associé
            Dim essai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = mesure.RefEssai, CancellationToken.None)

            'Dim essaiNom As String = If(essai IsNot Nothing, essai.E_nomEssai, "Inconnu")
            Dim essaiNom As String = If(essai IsNot Nothing, essai.E_nomEssai, "Inconnu")

            ' Générer le PDF avec QuestPDF
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                                                                PaddingVertical(20).
                                                                Column(Sub(column)
                                                                           ' Titre
                                                                           column.Item().Text("Rapport pour la mesure : Indice " & indice).
                                                                               FontSize(20).
                                                                               Bold()

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Référence de l'essai
                                                                           column.Item().Text("Essai : " & essaiNom & " (" & mesure.RefEssai & ")").FontSize(14)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Indice de la mesure
                                                                           column.Item().Text("Indice : " & mesure.indiceMesure).FontSize(12)

                                                                           ' Ajoutez d'autres propriétés de la mesure ici
                                                                           ' Exemple : column.Item().Text("Valeur : " & mesure.Valeur).FontSize(12)
                                                                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", $"rapport_mesure_{indice}.pdf")
        End Function

        '''' <summary>
        '''' Génère un rapport PDF pour toutes les mesures d'un essai.
        '''' </summary>
        '''' <param name="refEssai">Référence de l'essai.</param>
        '''' <returns>Résultat d'action avec le PDF généré.</returns>
        'Public Async Function GenerateRapportMesuresByEssai(refEssai As String) As Task(Of IActionResult)
        '    Try
        '        ' ✅ Correction: Utilisation de la bonne propriété (E_RefEssai)
        '        Dim mesures = Await _context.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync(CancellationToken.None)

        '        If mesures Is Nothing OrElse mesures.Count = 0 Then
        '            Return NotFound(New With {Key .Message = "Aucune mesure trouvée pour l'essai " & refEssai & "."})
        '        End If

        '        ' ✅ Correction: Utilisation de la bonne propriété (E_RefEssai)
        '        Dim essai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai, CancellationToken.None)

        '        ' ✅ Correction: Vérification que essai n'est pas Nothing
        '        Dim essaiNom As String = If(essai IsNot Nothing, essai.E_nomEssai, "Inconnu")

        '        ' ✅ Correction: Génération du PDF avec QuestPDF
        '        Dim pdfBytes = Document.Create(Sub(doc) doc.Page(Sub(page) page.Content().PaddingVertical(20).Column(Sub(column)
        '                                                                                                                 ' Titre
        '                                                                                                                 column.Item().Text("Rapport des mesures pour l'essai : " & essaiNom).FontSize(20).Bold()

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(10)

        '                                                                                                                 ' Référence de l'essai
        '                                                                                                                 column.Item().Text("Essai : " & refEssai).FontSize(14)

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(5)

        '                                                                                                                 ' Nombre de mesures
        '                                                                                                                 column.Item().Text("Nombre de mesures : " & mesures.Count.ToString()).FontSize(12)

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(10)

        '                                                                                                                 ' Titre pour les mesures
        '                                                                                                                 column.Item().Text("Liste des mesures :").FontSize(16).Bold()

        '                                                                                                                 ' Titre pour les mesures
        '                                                                                                                 column.Item().Text("Liste des mesures :").FontSize(16).Bold()

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(5)

        '                                                                                                                 ' En-tête du tableau des mesures
        '                                                                                                                 column.Item().Row(Sub(row)
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Indice").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Pression").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Volume15").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Volume30").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Volume60").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Contrainte Radiale").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("Déformation").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("PNF").FontSize(12).Bold())
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("VNF").FontSize(12).Bold())
        '                                                                                                                                   End Sub)

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(5)

        '                                                                                                                 ' Ligne de séparation
        '                                                                                                                 column.Item().Row(Sub(row)
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                       row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
        '                                                                                                                                   End Sub)

        '                                                                                                                 ' Espacement
        '                                                                                                                 column.Item().PaddingVertical(5)

        '                                                                                                                 ' Liste des mesures avec toutes les colonnes
        '                                                                                                                 For Each m In mesures
        '                                                                                                                     column.Item().Row(Sub(row)
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(m.indiceMesure.ToString()).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.pression.HasValue, m.pression.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume15.HasValue, m.volume15.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume30.HasValue, m.volume30.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume60.HasValue, m.volume60.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.contrainteRadiale.HasValue, m.contrainteRadiale.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.deformation.HasValue, m.deformation.Value.ToString("0.0000"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.PNF.HasValue, m.PNF.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                           row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.VNF.HasValue, m.VNF.Value.ToString("0.00"), "N/A")).FontSize(10))
        '                                                                                                                                       End Sub)
        '                                                                                                                 Next
        '                                                                                                             End Sub)
        '                                                         End Sub).GeneratePdf()

        '                                           ' Retourner le PDF
        '        Return File(pdfBytes, "application/pdf", "rapport_mesures_" & refEssai & ".pdf")
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = "Erreur : " & ex.Message})
        '    End Try
        'End Function

        ''' <summary>
        ''' Génère un rapport PDF pour toutes les mesures d'un essai.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportMesuresByEssai(refEssai As String) As Task(Of IActionResult)
            Try
                ' Récupérer les mesures associées à l'essai
                Dim mesures = Await _context.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync(CancellationToken.None)

                If mesures Is Nothing OrElse mesures.Count = 0 Then
                    Return NotFound(New With {Key .Message = "Aucune mesure trouvée pour l'essai " & refEssai & "."})
                End If

                ' Récupérer l'essai associé
                Dim essai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai, CancellationToken.None)

                ' Vérifier que essai n'est pas Nothing avant d'accéder à E_nomEssai
                Dim essaiNom As String = If(essai IsNot Nothing, essai.E_nomEssai, "Inconnu")

                ' Générer le PDF avec QuestPDF - STRUCTURE CORRIGÉE
                Dim pdfBytes = Document.Create(Sub(doc) doc.Page(Sub(page) page.Content().PaddingVertical(20).Column(Sub(column)
                                                                                                                         ' Titre
                                                                                                                         column.Item().Text("Rapport des mesures pour l'essai : " & essaiNom).FontSize(20).Bold()

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(10)

                                                                                                                         ' Référence de l'essai
                                                                                                                         column.Item().Text("Essai : " & refEssai).FontSize(14)

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(5)

                                                                                                                         ' Nombre de mesures
                                                                                                                         column.Item().Text("Nombre de mesures : " & mesures.Count.ToString()).FontSize(12)

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(10)

                                                                                                                         ' Titre pour les mesures
                                                                                                                         column.Item().Text("Liste des mesures :").FontSize(16).Bold()

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(5)

                                                                                                                         ' En-tête du tableau des mesures
                                                                                                                         column.Item().Row(
                                                                                                                        Sub(row)
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Indice").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Pression").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Volume15").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Volume30").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Volume60").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Contrainte Radiale").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("Déformation").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("PNF").FontSize(12).Bold())
                                                                                                                            row.RelativeItem().Column(Sub(col) col.Item().Text("VNF").FontSize(12).Bold())
                                                                                                                        End Sub) ' Fin de column.Item().Row(

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(5)

                                                                                                                         ' Ligne de séparation - CORRIGÉE
                                                                                                                         column.Item().Row(
                                                                                                                        Sub(row)
                                                                                                                            For i As Integer = 1 To 8
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text("-----").FontSize(12))
                                                                                                                            Next
                                                                                                                        End Sub) ' Fin de column.Item().Row(

                                                                                                                         ' Espacement
                                                                                                                         column.Item().PaddingVertical(5)

                                                                                                                         ' Liste des mesures - CORRIGÉE
                                                                                                                         For Each m In mesures
                                                                                                                             column.Item().Row(
                                                                                                                            Sub(row)
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(m.indiceMesure.ToString()).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.pression.HasValue, m.pression.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume15.HasValue, m.volume15.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume30.HasValue, m.volume30.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.volume60.HasValue, m.volume60.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.contrainteRadiale.HasValue, m.contrainteRadiale.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.deformation.HasValue, m.deformation.Value.ToString("0.0000"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.PNF.HasValue, m.PNF.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                                row.RelativeItem().Column(Sub(col) col.Item().Text(If(m.VNF.HasValue, m.VNF.Value.ToString("0.00"), "N/A")).FontSize(10))
                                                                                                                            End Sub) ' Fin de column.Item().Row(
                                                                                                                         Next ' Fin de For Each m In mesures
                                                                                                                         ' Fin de page.Content().PaddingVertical(20).Column(Sub(column)
                                                                                                                     End Sub))).GeneratePdf()

                ' Retourner le PDF
                Return File(pdfBytes, "application/pdf", "rapport_mesures_" & refEssai & ".pdf")
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = "Erreur : " & ex.Message})
            End Try
        End Function









    End Class
End Namespace