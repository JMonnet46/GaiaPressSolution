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

        ' Méthode pour générer un PDF pour un Chantier
        Public Shared Function GenerateChantierPDF(Chantier As Chantier) As Byte()
            Using stream = New MemoryStream()
                Dim writer = New PdfWriter(stream)
                Dim pdf = New PdfDocument(writer)
                Dim document = New Document(pdf)

                ' Add un titre
                document.Add(New Paragraph("Fiche du Chantier").SetFontSize(18).SetBold())

                ' Add les détails du Chantier (exemple)
                document.Add(New Paragraph($"Nom : {Chantier.NomChantier}"))
                document.Add(New Paragraph($"RefChantier. : {Chantier.RefChantier}"))
                document.Add(New Paragraph($"Descriptif : {Chantier.DescriptifChantier}"))
                document.Add(New Paragraph($"RefClient : {Chantier.RefClient}"))
                document.Add(New Paragraph($"Entreprise : {Chantier.Entreprise}"))
                document.Add(New Paragraph($"Adresse : {Chantier.Adresse}"))
                document.Add(New Paragraph($"Telephone : {Chantier.Telephone}"))
                document.Add(New Paragraph($"Reference : {Chantier.Reference}"))

                ' Fermer le document
                document.Close()

                ' Retourner le PDF sous forme de tableau d'octets
                Return stream.ToArray()
            End Using
        End Function
    End Class
End Namespace