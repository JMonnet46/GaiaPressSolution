' 📄 Fichier: Controllers/ChantierController.vb
Imports Microsoft.AspNetCore.Mvc
Imports GaiaPress.Core.V17.Models
Imports GaiaPress.Core.V17.Services
Imports GaiaPress.Core.V17.Data
Imports System.IO
Imports System.Threading.Tasks

Namespace GaiaPressApi.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class ChantierController
        Inherits ControllerBase

        Private ReadOnly _chantierLogic As ChantierLogic
        Private ReadOnly _dbContext As GaiaPressDbContext

        ' ✅ Injection de dépendances moderne
        Public Sub New(chantierLogic As ChantierLogic, dbContext As GaiaPressDbContext)
            _chantierLogic = chantierLogic
            _dbContext = dbContext
        End Sub

        ' ✅ ENDPOINT 1 : Générer PDF (version moderne)
        <HttpGet("pdf/{chantierId}")>
        Public Async Function GenerateChantierPDF(chantierId As Integer) As Task(Of IActionResult)
            Try
                Dim pdfBytes = Await _chantierLogic.GenerateRapportPDF(chantierId)
                Return File(pdfBytes, "application/pdf", $"Rapport_Chantier_{chantierId}.pdf")
            Catch ex As Exception
                Return BadRequest($"Erreur PDF: {ex.Message}")
            End Try
        End Function

        ' ✅ ENDPOINT 2 : Sauvegarder capture d'écran (version moderne)
        <HttpPost("screenshot")>
        Public Function SaveScreenshot(<FromBody> model As ScreenshotModel) As IActionResult
            Try
                ' ✅ VALIDATION CORRECTE (vérifie ImageData)
                If model Is Nothing OrElse model.ImageData Is Nothing OrElse model.ImageData.Length = 0 Then
                    Return BadRequest("Données d'image invalides")
                End If

                ' ✅ CHEMIN SÉCURISÉ (pas C:\Temp)
                Dim timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
                Dim sanitizedFilename = $"Screenshot_{timestamp}.png"
                Dim tempPath = Path.Combine(Path.GetTempPath(), sanitizedFilename)

                ' ✅ SAUVEGARDE
                System.IO.File.WriteAllBytes(tempPath, model.ImageData)

                Return Ok(New With {
                    .Message = "Capture sauvegardée",
                    .Path = tempPath,
                    .Size = model.ImageData.Length
                })
            Catch ex As Exception
                Return StatusCode(500, $"Erreur sauvegarde: {ex.Message}")
            End Try
        End Function

        ' ✅ ENDPOINT 3 : Créer un chantier
        <HttpPost>
        Public Async Function CreateChantier(<FromBody> chantier As Chantier) As Task(Of IActionResult)
            Try
                If String.IsNullOrWhiteSpace(chantier.C_NomChantier) Then
                    Return BadRequest("Le nom du chantier est requis")
                End If

                Dim success = Await _chantierLogic.SaveChantier(chantier)
                If success Then
                    Return CreatedAtAction(Nothing, New With {.id = chantier.Id}, chantier)
                Else
                    Return StatusCode(500, "Erreur création")
                End If
            Catch ex As Exception
                Return BadRequest($"Erreur: {ex.Message}")
            End Try
        End Function
    End Class

End Namespace