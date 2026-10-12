' Imports pour ASP.NET Core MVC
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure
Imports GaiaPresscoreV17.Data ' ✅ Import pour GaiaPressDbContext
Imports GaiaPressCoreV17.Models ' ✅ Import pour LesChantier, MesureEssaiForage, 
Imports GaiaPressCoreV17.Services
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting

Namespace GaiaPressCoreV17.Services
    ''' <summary>
    ''' Classe de logique métier pour la gestion des Chantier.
    ''' </summary>
    Public Class ChantierLogic

        Inherits ControllerBase

        ' ✅ Déclarer _dbContext avec le bon type
        Public _dbContext As Data.GaiaPressDbContext

        ' ✅ Déclarer _context comme membre de la classe
        Public _context As GaiaPressDbContext

        Public Property MonChantier As Chantier

        Public Sub SetMyChantier(chant As Chantier)
            MyChantier = chant
        End Sub

        Public Property MyChantier As Chantier

        Public Sub New()
            ' Créer une instance de DbContextOptionsBuilder
            Dim optionsBuilder As New DbContextOptionsBuilder()
            ' Configurer la connexion (exemple avec SQL Server)
            optionsBuilder.UseSqlServer("GaiaPress")

            ' Instancier _dbContext avec les options
            _dbContext = New GaiaPressDbContext(optionsBuilder.Options)
        End Sub


        '' <param name="chantierId">ID du chantier.</param>
        '' <returns>Résultat d'action avec le PDF généré.</returns>
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
                           column.Item().Text("Rapport pour le chantier : " & chantier. nomChantier).
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
                           column.Item().Text("Référence : " & chantier. refChantier).
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
        Public Async Function UpdateChantier(chantierId As Integer, chantier As Chantier) As Task(Of IActionResult)
            Dim existingChantier = Await _context.LesChantiers.FindAsync(chantierId)
            If existingChantier Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingChantier. nomChantier = chantier. nomChantier
            existingChantier. descriptifChantier = chantier. descriptifChantier
            existingChantier. refClient = chantier. refClient
            existingChantier. entreprise = chantier. entreprise
            existingChantier. adresse = chantier. adresse
            existingChantier. telephone = chantier. telephone
            existingChantier. reference = chantier. reference

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function
    End Class
End Namespace