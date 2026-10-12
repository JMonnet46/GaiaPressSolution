Imports System.IO

Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Data
Imports System.Threading.Tasks
Imports System.Linq
Imports QuestPDF.Fluent

Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Classe de logique métier pour la gestion des forages.
    ''' </summary>
    Public Class ForageLogic
        Inherits ControllerBase

        ' ✅ Déclarer _dbContext avec le bon type
        Public _dbContext As GaiaPressDbContext
        Public _context As GaiaPressDbContext

        ' Propriété pour stocker le forage actuel
        Public Property MonForage As Forage
        Public Property MyForage As Forage

        ''' <summary>
        ''' Définit le forage actuel.
        ''' </summary>
        Public Sub SetMyForage(forage As Forage)
            MyForage = forage
        End Sub

        ''' <summary>
        ''' Constructeur par défaut.
        ''' </summary>
        Public Sub New()
            ' Créer une instance de DbContextOptionsBuilder
            Dim optionsBuilder As New DbContextOptionsBuilder()
            ' Configurer la connexion (exemple avec SQL Server)
            optionsBuilder.UseSqlServer("GaiaPress")

            ' Instancier _dbContext avec les options
            _dbContext = New GaiaPressDbContext(optionsBuilder.Options)
            _context = _dbContext
        End Sub

        ' ======================
        ' Gestion des Forages
        ' ======================

        ''' <summary>
        ''' Récupère un forage par sa référence.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Forage correspondant ou NotFound.</returns>
        Public Async Function AddEssaiToForage(refForage As String, essai As Essai_Forage) As Task(Of IActionResult)
            Try
                ' ✅ Vérification des arguments
                If String.IsNullOrEmpty(refForage) Then
                    Return BadRequest(New With {Key .Message = "La référence du forage ne peut pas être vide."})
                End If

                If essai Is Nothing Then
                    Return BadRequest(New With {Key .Message = "L'essai ne peut pas être vide."})
                End If

                ' ✅ Assignation de la référence
                essai.RefForage = refForage

                ' ✅ Ajout de l'essai
                _dbContext.ListeEssaiForageDb.Add(essai)

                ' ✅ Sauvegarde asynchrone avec Await
                Await _context.SaveChangesAsync()

                ' ✅ Retour avec CreatedAtAction
                Return CreatedAtAction(NameOf(GetEssaiForageByRef), New With {Key .refEssai = essai.refEssai}, essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function


        ''' <summary>
        ''' Met à jour un forage.
        ''' </summary>
        ''' <param name="refForage">Référence du forage à mettre à jour.</param>
        ''' <param name="forage">Données du forage mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpdateForage(refForage As String, forage As Forage) As Task(Of IActionResult)

            If forage Is Nothing Then
                Return BadRequest("Les données du forage sont absentes.")
            End If

            Dim existingForage = Await _dbContext.ListeForageDb.
        FirstOrDefaultAsync(Function(f) f.RefForage = refForage)

            If existingForage Is Nothing Then
                Return NotFound()
            End If

            existingForage.RefChantier = forage.RefChantier
            existingForage.numeroForage = forage.numeroForage
            existingForage.NomForage = forage.NomForage
            existingForage.dateForage = forage.dateForage
            existingForage.materiel = forage.materiel
            existingForage.outil = forage.outil
            existingForage.abscisseX = forage.abscisseX
            existingForage.ordonneeY = forage.ordonneeY
            existingForage.altitudeZ = forage.altitudeZ
            existingForage.coteInit = forage.coteInit
            existingForage.coteFinale = forage.coteFinale

            Try
                Await _dbContext.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function


        ' ======================
        ' Gestion des Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère les essais associés à un forage.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais de forage.</returns>
        Public Async Function GetEssaisByForage(refForage As String) As Task(Of IActionResult)
            Dim essais = Await _dbContext.ListeEssaiForageDb.Where(Function(e) e.RefForage = refForage).ToListAsync()

            If essais Is Nothing OrElse essais.Count = 0 Then
                Return NotFound()
            End If
            Return Ok(essais)
        End Function

        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Essai de forage correspondant ou NotFound.</returns>
        Public Async Function GetEssaiForageByRef(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _dbContext.ListeEssaiForageDb.FirstOrDefaultAsync(Function(e) e.refEssai = refEssai)

                If essai Is Nothing Then
                    Return NotFound()
                End If

                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ' ======================
        ' Génération de Rapports PDF
        ' ======================

        ''' <summary>
        ''' Génère un rapport PDF pour un forage.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportForage(refForage As String) As Task(Of IActionResult)
            ' Récupérer le forage
            Dim forage = Await _dbContext.ListeForageDb.FirstOrDefaultAsync(Function(f) f.RefForage = refForage)
            If forage Is Nothing Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                                                                PaddingVertical(20).
                                                                Column(Sub(column)
                                                                           ' Titre
                                                                           column.Item().Text("Rapport pour le forage : " & forage. NomForage).
                                                                               FontSize(20).
                                                                               Bold()

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Référence
                                                                           column.Item().Text("Référence : " & forage. RefForage).
                                                                               FontSize(14)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Date
                                                                           ' ✅ Correction explicite (si  dateForage est Date/DateTime)
                                                                           column.Item().Text("Date : " & forage. dateForage.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Chantier associé
                                                                           column.Item().Text("Chantier : " & forage. RefChantier).
                                                                               FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Titre pour les essais
                                                                           column.Item().Text("Essais associés :").
                                                                               FontSize(16).
                                                                               Bold()

                                                                           ' Liste des essais
                                                                           Dim essais = _dbContext.ListeEssaiForageDb.Where(Function(e) e.RefForage = refForage).ToList()
                                                                           For Each essai In essais
                                                                               column.Item().Text($"- {essai.refEssai} : {essai.nomEssai}").
                                                                                   FontSize(12)
                                                                           Next
                                                                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", $"rapport_forag {refForage}.pdf")
        End Function

        ''' <summary>
        ''' Génère un rapport PDF pour un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportEssai(refEssai As String) As Task(Of IActionResult)
            ' Récupérer l'essai
            Dim essai = Await _dbContext.ListeEssaiForageDb.FirstOrDefaultAsync(Function(e) e.refEssai = refEssai)
            If essai Is Nothing Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                                                                PaddingVertical(20).
                                                                Column(Sub(column)
                                                                           ' Titre
                                                                           column.Item().Text("Rapport pour l'essai : " & essai. NomEssai).
                                                                               FontSize(20).
                                                                               Bold()

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Référence
                                                                           column.Item().Text("Référence : " & essai. RefEssai).
                                                                               FontSize(14)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Forage associé
                                                                           column.Item().Text("Forage : " & essai. RefForage).
                                                                               FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Date
                                                                           column.Item().Text("Date : " & essai. dateEssai.ToString("dd/MM/yyyy")).
                                                                               FontSize(12)
                                                                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", $"rapport_essai_{refEssai}.pdf")
        End Function
    End Class
End Namespace