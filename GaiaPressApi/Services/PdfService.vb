Namespace GaiaPress.core.V17.Services
    'Public Class PdfService
    '    Inherits DbContext

    '    ' Méthode pour générer un PDF pour un chantier
    '    Public Shared Function GenerateChantierPDF(chantier As lesChantiers) As Byte()
    '        Using stream = New MemoryStream()
    '            Dim writer = New PdfWriter(stream)
    '            Dim pdf = New PdfDocument(writer)
    '            Dim document = New Document(pdf)

    '            ' Add un titre
    '            document.Add(New Paragraph("Fiche du Chantier").SetFontSize(18).SetBold())

    '            ' Add les détails du chantier (exemple)
    '            document.Add(New Paragraph($"Nom : {chantier.C_nomChantier}"))
    '            document.Add(New Paragraph($"RefChantier. : {chantier.C_refChantier}"))
    '            document.Add(New Paragraph($"Descriptif : {chantier.C_descriptifChantier}"))
    '            document.Add(New Paragraph($"RefClient : {chantier.C_refClient}"))
    '            document.Add(New Paragraph($"Entreprise : {chantier.C_entreprise}"))
    '            document.Add(New Paragraph($"adresse : {chantier.C_adresse}"))
    '            document.Add(New Paragraph($"telephone : {chantier.C_telephone}"))
    '            document.Add(New Paragraph($"reference : {chantier.C_reference}"))

    '            ' Fermer le document
    '            document.Close()

    '            ' Retourner le PDF sous forme de tableau d'octets
    '            Return stream.ToArray()
    '        End Using
    '    End Function
    'End Class
End Namespace