' Imports pour ASP.NET Core MVC
Imports System.IO
' Imports pour GaiaPress
' Imports pour Entity Framework Core
' Imports pour QuestPDF

Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Classe de logique métier pour la gestion des essais de forage.
    ''' </summary>
    Public Class EssaisForageLogic
        Inherits ControllerBase

        ' ✅ Déclarer _dbContext avec le bon type
        Public _dbContext As GaiaPressDbContext
        Public _context As GaiaPressDbContext

        ' Propriété pour stocker l'essai de forage actuel
        Public Property MonEssaiForage As Essai_Forage
        Public Property MyEssaiForage As Essai_Forage

        ' Propriété pour stocker le chantier associé
        'Public Property MonChantier As LesChantiers

        ''' <summary>
        ''' Définit l'essai de forage actuel.
        ''' </summary>
        Public Sub SetMyEssaiForage(essai As Essai_Forage)
            MyEssaiForage = essai
        End Sub

        ''' <summary>
        ''' Définit le chantier associé.
        ''' </summary>
        'Public Sub SetMonChantier(chantier As LesChantiers)
        '    MonChantier = chantier
        'End Sub

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
        ' Gestion des Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Essai de forage correspondant ou NotFound.</returns>
        Public Async Function GetEssaiForageByRef(refEssai As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai avec la référence {refEssai} introuvable."})
                End If

                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Données de l'essai mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpdateEssaiForage(refEssai As String, essai As Essai_Forage) As Task(Of IActionResult)
            Dim existingEssai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
            If existingEssai Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingEssai = essai

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function

        ' ======================
        ' Gestion des Mesures d'Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère les mesures associées à un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Liste des mesures de l'essai.</returns>
        Public Async Function GetMesuresByEssai(refEssai As String) As Task(Of IActionResult)

            Dim mesures = Await _context.MesuresEssaiForage _
                .Where(Function(m) m.RefEssai = refEssai) _
                .ToListAsync()

            If mesures Is Nothing OrElse mesures.Count = 0 Then
                Return NotFound()
            End If

            Return Ok(mesures)

        End Function

        Public Async Function AddEssaisForage(essai As Essai_Forage) As Task(Of IActionResult)
            Try
                If essai Is Nothing Then
                    Return BadRequest(New With {Key .Message = "L'essai de forage ne peut pas être vide."})
                End If
                _context.EssaisForage.Add(essai)
                Await _context.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetEssaiForageByRef), New With {Key .refEssai = essai.E_RefEssai}, essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function



        ''' <summary>
        Public Async Function DeleteEssaiForage(refEssai As String) As Task(Of IActionResult)
            Try
                If String.IsNullOrEmpty(refEssai) Then
                    Return BadRequest(New With {Key .Message = "La référence de l'essai ne peut pas être vide."})
                End If

                Dim essaiToDelete = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

                If essaiToDelete Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai avec la référence {refEssai} introuvable."})
                End If

                _context.EssaisForage.Remove(essaiToDelete)
                Await _context.SaveChangesAsync()
                Return Ok(New With {Key .Message = $"Essai {refEssai} supprimé avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function


        ' ======================
        ' Génération de Rapports PDF
        ' ======================

        ''' <summary>
        ''' Génère un rapport PDF pour un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportEssai(refEssai As String) As Task(Of IActionResult)
            ' Récupérer l'essai
            Dim essai = Await _context.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
            If essai Is Nothing Then
                Return NotFound()
            End If

            ' Récupérer les mesures associées
            'Dim mesures = Await _context.MesuresEssaiForage
            '.Where(Function(m) m.E_RefEssai = refEssai)
            '.ToListAsync()
            Dim mesures = Await _context.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync()

            ' Générer le PDF avec QuestPDF
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                                                                PaddingVertical(20).
                                                                Column(Sub(column)
                                                                           ' Titre
                                                                           column.Item().Text("Rapport pour l'essai : " & essai.E_NomEssai).
                                                                               FontSize(20).
                                                                               Bold()

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Référence
                                                                           column.Item().Text("Référence : " & essai.E_RefEssai).
                                                                               FontSize(14)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Forage associé
                                                                           column.Item().Text("Forage : " & essai.E_RefForage).
                                                                               FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Chantier associé
                                                                           column.Item().Text("Chantier : " & essai.E_RefChantier).
                                                                               FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(5)

                                                                           ' Date
                                                                           column.Item().Text("Date : " & essai.E_dateEssai.ToString("dd/MM/yyyy")).
                                                                               FontSize(12)

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Titre pour les mesures
                                                                           column.Item().Text("Mesures associées :").
                                                                               FontSize(16).
                                                                               Bold()

                                                                           ' Liste des mesures
                                                                           For Each mesure In mesures
                                                                               column.Item().Text($"- Indice {mesure.indiceMesure}: {mesure.ToString()}").
                                                                                   FontSize(12)
                                                                           Next
                                                                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", $"rapport_essai_{refEssai}.pdf")
        End Function

        ''' <summary>
        ''' Génère un rapport PDF pour toutes les mesures d'un essai.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapportMesures(refEssai As String) As Task(Of IActionResult)
            ' Récupérer les mesures associées à l'essai
            'Dim mesures = Await _context.MesuresEssaiForage
            '.Where(Function(m) m.E_RefEssai = refEssai)
            '.ToListAsync()
            Dim mesures = Await _context.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync()


            If mesures Is Nothing OrElse mesures.Count = 0 Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                                                                PaddingVertical(20).
                                                                Column(Sub(column)
                                                                           ' Titre
                                                                           column.Item().Text("Rapport des mesures pour l'essai : " & refEssai).
                                                                               FontSize(20).
                                                                               Bold()

                                                                           ' Espacement
                                                                           column.Item().PaddingVertical(10)

                                                                           ' Liste des mesures
                                                                           For Each mesure In mesures
                                                                               column.Item().Text($"- Indice {mesure.indiceMesure}: {mesure.ToString()}").
                                                                                   FontSize(12)
                                                                           Next
                                                                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", $"rapport_mesures_{refEssai}.pdf")
        End Function
    End Class
End Namespace