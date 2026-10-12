Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Controllers
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Services
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports System.IO
Imports System

Namespace GaiaPressCoreV17.Controllers
    <ApiController>
    <Route("api/[controller]")>
    Public Class ChantierController
        Inherits ControllerBase

        ' Déclaration des dépendances avec injection de dépendances
        Private ReadOnly _context As GaiaPressDbContext
        Private ReadOnly _chantierLogic As ChantierLogic

        ' Constructeur avec injection de dépendances
        Private ReadOnly _chantierService As ChantierService

        Private ReadOnly _dataService As DataService  ' Ajoutez cette ligne
        Private ReadOnly _pdfService As PdfService  ' ✅ Déclaration

        Public Sub New(context As GaiaPressDbContext)
            _context = context
            _dataService = New DataService(_context)  ' ✅ Passez _context à DataService
            _chantierService = New ChantierService(_dataService, _context)
            _chantierLogic = New ChantierLogic()
            _pdfService = New PdfService()            ' ✅ PDFService n'a PAS besoin de dbContext
        End Sub

        ' Récupérer la liste des chantiers
        <HttpGet>
        Public Function GetChantiers() As IActionResult
            Try
                Dim chantiers As List(Of Chantier) = _chantierService.GetAllChantiers()  ' ✅ Utilise l'instance
                If chantiers Is Nothing Then
                    Return NotFound("Aucun chantier trouvé.")
                End If
                Return Ok(chantiers)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des chantiers: {ex.Message}")
            End Try
        End Function

        ' Récupérer un chantier par son ID
        '<HttpGet("{id}")>
        'Public Function GetChantierById(refChantier As String) As IActionResult
        '    Try
        '        Dim chantier As Chantier = _chantierService.GetById(Integer.Parse(c))  ' ✅ Utilisez le paramètre `c` et la méthode `GetById`
        '        If chantier Is Nothing Then
        '            Return NotFound($"Le chantier avec l'ID {id} n'existe pas.")
        '        End If
        '        Return Ok(chantier)
        '    Catch ex As Exception
        '        Return StatusCode(500, $"Erreur lors de la récupération du chantier: {ex.Message}")
        '    End Try
        'End Function

        ' Générer un PDF pour un chantier
        <HttpGet("{id}/pdf")>
        Public Function GeneratePDF(refChantier As String) As IActionResult
            Try
                Dim chantier As Chantier = _chantierService.GetChantier(refChantier)
                If chantier Is Nothing Then
                    Return NotFound($"Le chantier avec l'ID {refChantier} n'existe pas.")
                End If

                Dim pdfBytes As Byte() = PdfService.GenerateChantierPDF(chantier)
                If pdfBytes Is Nothing OrElse pdfBytes.Length = 0 Then
                    Return BadRequest("Impossible de générer le PDF.")
                End If

                Return File(pdfBytes, "application/pdf", $"Chantier_{refChantier}.pdf")
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la génération du PDF: {ex.Message}")
            End Try
        End Function

        ' Recevoir une capture d'écran du client
        <HttpPost("screenshot")>
        Public Function SaveScreenshot(<FromBody> model As ScreenshotModel) As IActionResult
            Try
                ' Vérification des données reçues
                If model Is Nothing OrElse model.ImageData Is Nothing OrElse model.ImageData.Length = 0 Then
                    Return BadRequest("Aucune donnée d'image reçue.")
                End If

                ' Sauvegarder l'image dans le dossier temporaire
                Dim tempPath As String = Path.Combine(Path.GetTempPath(), $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png")
                System.IO.File.WriteAllBytes(tempPath, model.ImageData)

                Return Ok(New With {.Message = "Capture d'écran enregistrée avec succès.", .Path = tempPath})
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de l'enregistrement de la capture d'écran: {ex.Message}")
            End Try
        End Function
    End Class
End Namespace