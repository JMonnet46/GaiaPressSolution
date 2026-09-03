Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports GaiaPress.core.V17.Data
Imports GaiaPress.core.V17.Models
Imports GaiaPress.core.V17.Services  ' Remplacez par le namespace correct de vos services
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore

<ApiController>
<Route("api/[controller]")>
Public Class ChantierController
    Inherits ControllerBase
    Private ReadOnly _context As GaiaPressDbContext
    Private ReadOnly _chantierLogic As ChantierLogic
    Private ReadOnly _chantierService As ChantierService
    Private ReadOnly _dbContext As GaiaPressDbContext  ' ✅ Ajoutez cette ligne

    Public Sub New(context As GaiaPressDbContext, chantierLogic As ChantierLogic)
        _context = context
        _chantierLogic = chantierLogic
        _chantierService = New ChantierService(context)
        _dbContext = context  ' ✅ Initialisez _dbContext
    End Sub

    <HttpGet("{refChantier}")>
    Public Async Function GetChantier(refChantier As String) As Task(Of IActionResult)
        Dim chantier As Chantier = Await _chantierService.GetChantierWithDetailsAsync(refChantier)
        If chantier Is Nothing Then
            Return NotFound()
        End If
        Return Ok(chantier)
    End Function

    ' Récupérer la liste des chantiers
    <HttpGet>
    Public Function GetChantiers(refChantier As String) As IActionResult
        ' ✅ Nouvelle version
        Dim Monchantier As Chantier = _dbContext.LesChantiers.FirstOrDefault()  ' ✅ Utilisation correcte
        Return Ok(Monchantier)
    End Function

    ' Générer un PDF pour un chantier
    <HttpGet("{id}/pdf")>
    Public Function GeneratePDF(refChantier As String) As IActionResult
        ' ✅ Nouvelle version
        Dim Monchantier As Chantier = _dbContext.LesChantiers.FirstOrDefault()  ' ✅ Utilisation correcte
        Dim pdfBytes As Byte() = PdfService.GenerateChantierPDF(Monchantier)
        Return File(pdfBytes, "application/pdf", $"Chantier_{refChantier}.pdf")
    End Function

    ' Recevoir une capture d'écran du client
    <HttpPost("screenshot")>
    Public Function SaveScreenshot(<FromBody> model As ScreenshotModel) As IActionResult
        ' Sauvegarder l'image reçue du client
        Dim imageBytes As Byte() = model.ImageData

        System.IO.File.WriteAllBytes($"C:\Temp\Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png", imageBytes)
        Return Ok()
    End Function
End Class