Imports Microsoft.AspNetCore.Mvc
Imports System.Collections.Generic
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services

<ApiController>
<Route("api/[controller]")>
Public Class ChantierController
    Inherits ControllerBase

    Public Sub New()

    End Sub

    ' Récupérer un chantier par son ID
    <HttpGet("{id}")>
    Public Function GetChantier(id As String) As IActionResult
        ' Remplacez par votre logique pour récupérer le chantier depuis la base de données
        Dim chantier As New Chantier With {
            .RefChantier = id,
            .NomChantier = "Chantier Test",
            .RefClient = "CL001",
            .Entreprise = "Entreprise Test",
            .Telephone = "0123456789",
            .Adresse = "1 Rue de Test",
            .DescriptifChantier = "Description du chantier test"
        }
        Return Ok(chantier)
    End Function

    ' Sauvegarder un chantier
    <HttpPost>
    Public Function SaveChantier(<FromBody> chantier As Chantier) As IActionResult
        ' Logique pour sauvegarder le chantier dans la base de données
        ' Exemple : ChantierService.SaveChantier(chantier)
        Return Ok(New With {.Message = "Chantier sauvegardé avec succès !"})
    End Function

    ' Récupérer l'arborescence d'un chantier
    <HttpGet("{id}/arborescence")>
    Public Function GetArborescence(id As String) As IActionResult
        ' Exemple de données pour jsTree
        Dim arborescence As New List(Of Object) From {
            New With {.id = "chantier1", .parent = "#", .text = "Chantier 1"},
            New With {.id = "forage1", .parent = "chantier1", .text = "Forage 1"},
            New With {.id = "forage2", .parent = "chantier1", .text = "Forage 2"}
        }
        Return Ok(arborescence)
    End Function

    ' Récupérer les forages d'un chantier
    <HttpGet("{id}/forages")>
    Public Function GetForages(id As String) As IActionResult
        ' Exemple de données pour DataTables
        Dim forages As New List(Of Object) From {
            New With {.refForage = "F001", .nomForage = "Forage 1", .dateForage = "2023-01-01"},
            New With {.refForage = "F002", .nomForage = "Forage 2", .dateForage = "2023-01-02"}
        }
        Return Ok(forages)
    End Function

    ' Supprimer un forage
    <HttpDelete("{chantierId}/forage/{forageId}")>
    Public Function DeleteForage(chantierId As String, forageId As String) As IActionResult
        ' Logique pour supprimer le forage de la base de données
        Return Ok(New With {.Message = "Forage supprimé avec succès !"})
    End Function

    ' Générer un PDF pour un chantier
    <HttpGet("{id}/pdf")>
    Public Function GeneratePDF(id As String) As IActionResult
        ' Utilisez QuestPDF pour générer le PDF
        ' Exemple :
        ' Dim document As Document = Document.Create(...)
        ' Dim stream As New MemoryStream()
        ' document.GeneratePdf(stream)
        ' stream.Position = 0
        ' Return File(stream, "application/pdf", $"Chantie {id}.pdf")
        Return Ok()
    End Function
End Class