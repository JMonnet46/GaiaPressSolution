' Imports pour ASP.NET Core MVC
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports GaiaPress.core.V17.Data
Imports GaiaPress.core.V17.Models
Imports Microsoft.AspNetCore.Mvc
' Imports pour Entity Framework Core
Imports Microsoft.EntityFrameworkCore
' Imports pour QuestPDF
Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure


Namespace Models
    ''' <summary>
    ''' Classe de logique métier pour la gestion des chantiers.
    ''' </summary>
    Public Class ChantierLogic
        Inherits ControllerBase



        Private ReadOnly _context As GaiaPressDbContext
        Public Property MyChantier As LesChantiers

        ''' <summary>
        ''' Constructeur pour injecter le contexte de base de données.
        ''' </summary>
        ''' <param name="context">Contexte de base de données GaiaPress.</param>
        Public Sub New(context As GaiaPressDbContext)
            _context = context
        End Sub

        ''' <summary>
        ''' Définit le chantier courant.
        ''' </summary>
        ''' <param name="chant">Chantier à définir comme courant.</param>
        Public Sub SetMyChantier(chant As LesChantiers)
            MyChantier = chant
        End Sub

        ''' <summary>
        ''' Récupère la liste de tous les chantiers.
        ''' </summary>
        ''' <returns>Liste des chantiers.</returns>
        'Public Function GetChantiers() As List(Of LesChantiers)
        '    Return _context.LesChantiers.ToList()
        'End Function

        ''' <summary>
        ''' Génère un rapport PDF pour un chantier donné.
        ''' </summary>
        ''' <param name="chantierId">ID du chantier.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapport(chantierId As Integer) As Task(Of IActionResult)
            ' Récupérer le chantier
            Dim chantier = Await _context.LesChantiers.FindAsync(chantierId)
            If chantier Is Nothing Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF (syntaxe moderne et correcte)
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                PaddingVertical(20).
                Column(Sub(column)
                           ' Titre
                           column.Item().Text("Rapport pour le chantier : " & chantier.nomChantier).
                        FontSize(20).
                        Bold()

                           ' Espacement
                           column.Item().PaddingVertical(10)

                           ' Date
                           column.Item().Text("Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm")).
                        FontSize(14)

                           ' Espacement
                           column.Item().PaddingVertical(5)

                           ' Référence
                           column.Item().Text("Référence : " & chantier.C_refChantier).
                        FontSize(12)
                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", "rapport_chantier.pdf")
        End Function
        ''' Récupère un chantier par son ID.
        ''' </summary>
        ''' <param name="chantierId">ID du chantier.</param>
        ''' <returns>Chantier correspondant ou NotFound.</returns>
        Public Async Function GetChantier(chantierId As Integer) As Task(Of IActionResult)
            Dim chantier = Await _context.LesChantiers.FindAsync(chantierId)
            If chantier Is Nothing Then
                Return NotFound()
            End If
            Return Ok(chantier)
        End Function

        ''' <summary>
        ''' Met à jour un chantier.
        ''' </summary>
        ''' <param name="chantierId">ID du chantier à mettre à jour.</param>
        ''' <param name="chantier">Données du chantier mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpdateChantier(chantierId As Integer, chantier As LesChantiers) As Task(Of IActionResult)
            Dim existingChantier = Await _context.LesChantiers.FindAsync(chantierId)
            If existingChantier Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingChantier.C_nomChantier = chantier.C_nomChantier
            existingChantier.C_descriptifChantier = chantier.C_descriptifChantier
            existingChantier.C_refClient = chantier.C_refClient
            existingChantier.C_entreprise = chantier.C_entreprise
            existingChantier.C_adresse = chantier.C_adresse
            existingChantier.C_telephone = chantier.C_telephone
            existingChantier.C_reference = chantier.C_reference

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function
    End Class
End Namespace