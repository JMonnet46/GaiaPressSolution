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

        ' Méthode pour générer un PDF pour un chantier
        Public Shared Function GenerateChantierPDF(chantier As Chantier) As Byte()
            Using stream = New MemoryStream()
                Dim writer = New PdfWriter(stream)
                Dim pdf = New PdfDocument(writer)
                Dim document = New Document(pdf)

                ' Add un titre
                document.Add(New Paragraph("Fiche du Chantier").SetFontSize(18).SetBold())

                ' Add les détails du chantier (exemple)
                document.Add(New Paragraph($"Nom : {chantier.nomChantier}"))
                document.Add(New Paragraph($"RefChantier. : {chantier.refChantier}"))
                document.Add(New Paragraph($"Descriptif : {chantier.descriptifChantier}"))
                document.Add(New Paragraph($"RefClient : {chantier.refClient}"))
                document.Add(New Paragraph($"Entreprise : {chantier.entreprise}"))
                document.Add(New Paragraph($"adresse : {chantier.adresse}"))
                document.Add(New Paragraph($"telephone : {chantier.telephone}"))
                document.Add(New Paragraph($"reference : {chantier.reference}"))

                ' Fermer le document
                document.Close()

                ' Retourner le PDF sous forme de tableau d'octets
                Return stream.ToArray()
            End Using
        End Function
    End Class
End Namespace