Imports System.Drawing
Imports System.Drawing.Imaging
Imports GaiaPress.core.V17.Data
Imports GaiaPress.core.V17.GaiaPress.core.V17.Models  ' Remplacez par le namespace correct de vos modèles
Imports GaiaPress.core.V17.GaiaPress.core.V17.Services  ' Remplacez par le namespace correct de vos services
Imports GaiaPress.core.V17.Models
Imports Microsoft.AspNetCore.Mvc


<ApiController>
<Route("api/[controller]")>
Public Class ChantierController
    Inherits ControllerBase
    Private ReadOnly _context As GaiaPressDbContext
    Private ReadOnly _chantierLogic As ChantierLogic

    Public Sub New(context As GaiaPressDbContext)
        _context = context
        _chantierLogic = New ChantierLogic(context)
    End Sub

    ' Récupérer la liste des chantiers
    <HttpGet>
    Public Function GetChantiers() As IActionResult
        Dim chantiers As List(Of LesChantiers) = ChantierService.GetAllChantiers()
        Return Ok(chantiers)
    End Function

    ' Générer un PDF pour un chantier
    <HttpGet("{id}/pdf")>
    Public Function GeneratePDF(id As String) As IActionResult
        Dim chantier As LesChantiers = ChantierService.GetChantierById(id)
        Dim pdfBytes As Byte() = PDFService.GenerateChantierPDF(chantier)
        Return File(pdfBytes, "application/pdf", $"Chantier_{id}.pdf")
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