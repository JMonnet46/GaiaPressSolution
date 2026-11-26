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
    ''' Classe de logique métier pour la gestion des Chantiers.
    ''' </summary>
    Public Class ChantierLogic
        Inherits ControllerBase



        Private ReadOnly _context As GaiaPress_Db_Context
        Public Property MyChantier As Chantier

        ''' <summary>
        ''' Constructeur pour injecter le contexte de base de données.
        ''' </summary>
        ''' <param name="context">Contexte de base de données GaiaPress.</param>
        Public Sub New(context As GaiaPress_Db_Context)
            _context = context
        End Sub

        ''' <summary>
        ''' Définit le Chantier courant.
        ''' </summary>
        ''' <param name="chant">Chantier à définir comme courant.</param>
        Public Sub SetMyChantier(chant As Chantier)
            MyChantier = chant
        End Sub

        ''' <summary>
        ''' Récupère la liste de tous les Chantiers.
        ''' </summary>
        ''' <returns>Liste des Chantiers.</returns>
        'Public Function GetChantiers() As List(Of Chantier)
        '    Return _context.Chantier.ToList()
        'End Function

        ''' <summary>
        ''' Génère un rapport PDF pour un Chantier donné.
        ''' </summary>
        ''' <param name="ChantierId">ID du Chantier.</param>
        ''' <returns>Résultat d'action avec le PDF généré.</returns>
        Public Async Function GenerateRapport(ChantierId As Integer) As Task(Of IActionResult)
            ' Récupérer le Chantier
            Dim Chantier = Await _context.ChantierDb.FindAsync(ChantierId)
            If Chantier Is Nothing Then
                Return NotFound()
            End If

            ' Générer le PDF avec QuestPDF (syntaxe moderne et correcte)
            Dim pdfBytes = Document.Create(Sub(doc)
                                               doc.Page(Sub(page)
                                                            page.Content().
                PaddingVertical(20).
                Column(Sub(column)
                           ' Titre
                           column.Item().Text("Rapport pour le Chantier : " & Chantier.NomChantier).
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
                           column.Item().Text("Référence : " & Chantier.RefChantier).
                        FontSize(12)
                       End Sub)
                                                        End Sub)
                                           End Sub).GeneratePdf()

            ' Retourner le PDF
            Return File(pdfBytes, "application/pdf", "rapport_Chantier.pdf")
        End Function
        ''' Récupère un Chantier par son ID.
        ''' </summary>
        ''' <param name="ChantierId">ID du Chantier.</param>
        ''' <returns>Chantier correspondant ou NotFound.</returns>
        Public Async Function GetChantier(ChantierId As Integer) As Task(Of IActionResult)
            Dim Chantier = Await _context.ChantierDb.FindAsync(ChantierId)
            If Chantier Is Nothing Then
                Return NotFound()
            End If
            Return Ok(Chantier)
        End Function

        ''' <summary>
        ''' Met à jour un Chantier.
        ''' </summary>
        ''' <param name="ChantierId">ID du Chantier à mettre à jour.</param>
        ''' <param name="Chantier">Données du Chantier mises à jour.</param>
        ''' <returns>Résultat de l'opération.</returns>
        Public Async Function UpDateChantier(ChantierId As Integer, Chantier As Chantier) As Task(Of IActionResult)
            Dim existingChantier = Await _context.ChantierDb.FindAsync(ChantierId)
            If existingChantier Is Nothing Then
                Return NotFound()
            End If

            ' Mise à jour des propriétés
            existingChantier.NomChantier = Chantier.NomChantier
            existingChantier.DescriptifChantier = Chantier.DescriptifChantier
            existingChantier.RefClient = Chantier.RefClient
            existingChantier.Entreprise = Chantier.Entreprise
            existingChantier.Adresse = Chantier.Adresse
            existingChantier.Telephone = Chantier.Telephone
            existingChantier.Reference = Chantier.Reference

            Try
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As DbUpdateConcurrencyException
                Return Conflict()
            End Try
        End Function
    End Class
End Namespace

