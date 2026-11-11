' Imports pour ASP.NET Core MVC
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Models
Imports Microsoft.AspNetCore.Mvc
' Imports pour Entity Framework Core
Imports Microsoft.EntityFrameworkCore
' Imports pour QuestPDF
Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure


Namespace GaiaPressCoreV17.Logic
    ''' <summary>
    ''' Classe de logique métier pour la gestion des ConstructionSites.
    ''' </summary>
    Public Class ConstructionSiteLogic
        Inherits ControllerBase



        Private ReadOnly _context As GaiaPressDbContext
        Public Property MyConstructionSite As ConstructionSite

        ''' <summary>
        ''' Constructeur pour injecter le contexte de base de données.
        ''' </summary>
        ''' <param name="context">Contexte de base de données GaiaPress.</param>
        Public Sub New(context As GaiaPressDbContext)
            _context = context
        End Sub

        ''' <summary>
        ''' Définit le ConstructionSite courant.
        ''' </summary>
        ''' <param name="chant">ConstructionSite à définir comme courant.</param>
        Public Sub SetMyConstructionSite(chant As ConstructionSite)
            MyConstructionSite = chant
        End Sub

        ''' <summary>
        ''' Récupère la liste de tous les ConstructionSites.
        ''' </summary>
        ''' <returns>Liste des ConstructionSites.</returns>
        'Public Function GetConstructionSites() As List(Of ConstructionSite)
        '    Return _context.ConstructionSite.ToList()
        'End Function

        ''' <summary>
        ''' Génère un rapport PDF pour un ConstructionSite donné.
        ''' </summary>
        ''' <param name="ConstructionSiteId">ID du ConstructionSite.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapport(ConstructionSiteId As Integer) As Task(Of IActionResult)
            ' Récupérer le ConstructionSite
            Dim ConstructionSite = Await _context.ConstructionSitesDb.FindAsync(ConstructionSiteId)
            If ConstructionSite Is Nothing Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF (syntaxe moderne et correcte)
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                PaddingVertical(20).
                Column(Sub(column)
                           ' Titre
                           column.Item().Text("Rapport pour le ConstructionSite : " & ConstructionSite.NameConstructionSite).
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
                           column.Item().Text("Référence : " & ConstructionSite.RefChantier).
                        FontSize(12)
                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", "rapport_ConstructionSite.pdf")
        End Function
        ''' Récupère un ConstructionSite par son ID.
        ''' </summary>
        ''' <param name="ConstructionSiteId">ID du ConstructionSite.</param>
        ''' <returns>ConstructionSite correspondant ou NotFound.</returns>
        Public Async Function GetConstructionSite(ConstructionSiteId As Integer) As Task(Of IActionResult)
            Dim ConstructionSite = Await _context.ConstructionSitesDb.FindAsync(ConstructionSiteId)
            If ConstructionSite Is Nothing Then
                Return NotFound()
            End If
            Return Ok(ConstructionSite)
        End Function

        ''' <summary>
        ''' Met à jour un ConstructionSite.
        ''' </summary>
        ''' <param name="ConstructionSiteId">ID du ConstructionSite à mettre à jour.</param>
        ''' <param name="ConstructionSite">Données du ConstructionSite mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpDateConstructionSite(ConstructionSiteId As Integer, ConstructionSite As ConstructionSite) As Task(Of IActionResult)
            Dim existingConstructionSite = Await _context.ConstructionSitesDb.FindAsync(ConstructionSiteId)
            If existingConstructionSite Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingConstructionSite.NameConstructionSite = ConstructionSite.NameConstructionSite
            existingConstructionSite.DescriptionConstructionSite = ConstructionSite.DescriptionConstructionSite
            existingConstructionSite.RefClient = ConstructionSite.RefClient
            existingConstructionSite.Entreprise = ConstructionSite.Entreprise
            existingConstructionSite.Adresse = ConstructionSite.Adresse
            existingConstructionSite.Telephone = ConstructionSite.Telephone
            existingConstructionSite.Reference = ConstructionSite.Reference

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function
    End Class
End Namespace

