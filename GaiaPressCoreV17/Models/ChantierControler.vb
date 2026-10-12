Imports System.Drawing
Imports System.Drawing.Imaging
Imports GaiaPress.core.V17.Data
Imports GaiaPress.core.V17.Models
Imports GaiaPress.core.V17.Services  ' Remplacez par le namespace correct de vos services

Imports Microsoft.AspNetCore.Mvc



<ApiController>
<Route("api/[controller]")>
Public Class ChantierController
    Inherits ControllerBase
    Private ReadOnly _context As GaiaPressDbContext
    Private ReadOnly _chantierLogic As ChantierLogic

    Public Sub New(context As GaiaPressDbContext)
        _context = context
        '_chantierLogic = New GaiaPress.core.V17.Services.ChantierLogic(context)
        ' ✅ Utilisez le nom complet et vérifiez que context est bien un GaiaPressDbContext
        _chantierLogic = New GaiaPress.core.V17.Services.ChantierLogic()
    End Sub

    ' Récupérer la liste des chantiers
    <HttpGet>
    Public Function GetChantiers(refChantier As String) As IActionResult
        'Dim refChantier As String = "VOTRE_REFERENCE"  ' Remplacez par la référence du chantier
        Dim Chantier As LesChantiers = ChantierService.GetChantierByRef(refChantier)
        Return Ok(Chantier)
    End Function

    ' Générer un PDF pour un chantier
    <HttpGet("{id}/pdf")>
    Public Function GeneratePDF(refChantier As String) As IActionResult
        Dim chantier As LesChantiers = ChantierService.GetChantierByRef(refChantier)
        Dim pdfBytes As Byte() = PdfService.GenerateChantierPDF(chantier)
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