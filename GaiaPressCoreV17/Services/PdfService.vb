Imports System.IO ' ✅ Pour MemoryStream
Imports iText.Kernel.Pdf ' ✅ Pour PdfWriter, PdfDocument
Imports iText.Layout ' ✅ Pour Document
Imports iText.Layout.Element ' ✅ Pour Paragraph
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.AspNetCore.Mvc
Imports GaiaPressCoreV17
Imports System.Threading

Namespace GaiaPressCoreV17.Services
    Public Class PdfService
        Inherits DbContext

        ' Méthode pour générer un PDF pour un ConstructionSite
        Public Shared Function GenerateConstructionSitePDF(ConstructionSite As ConstructionSite) As Byte()
            Using stream = New MemoryStream()
                Dim writer = New PdfWriter(stream)
                Dim pdf = New PdfDocument(writer)
                Dim document = New Document(pdf)

                ' Add un titre
                document.Add(New Paragraph("Fiche du ConstructionSite").SetFontSize(18).SetBold())

                ' Add les détails du ConstructionSite (exemple)
                document.Add(New Paragraph($"Nom : {ConstructionSite.NameConstructionSite}"))
                document.Add(New Paragraph($"RefChantier. : {ConstructionSite.RefChantier}"))
                document.Add(New Paragraph($"Descriptif : {ConstructionSite.DescriptionConstructionSite}"))
                document.Add(New Paragraph($"RefClient : {ConstructionSite.RefClient}"))
                document.Add(New Paragraph($"Entreprise : {ConstructionSite.Entreprise}"))
                document.Add(New Paragraph($"Adresse : {ConstructionSite.Adresse}"))
                document.Add(New Paragraph($"Telephone : {ConstructionSite.Telephone}"))
                document.Add(New Paragraph($"Reference : {ConstructionSite.Reference}"))

                ' Fermer le document
                document.Close()

                ' Retourner le PDF sous forme de tableau d'octets
                Return stream.ToArray()
            End Using
        End Function
    End Class
End Namespace