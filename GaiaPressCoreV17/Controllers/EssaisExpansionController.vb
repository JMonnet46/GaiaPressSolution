Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Models
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class EssaisExpansionController
        Inherits ControllerBase

        Public _dbContext As GaiaPressDbContext

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'expansion.
        ''' </summary>
        <HttpGet>
        Public Async Function GetAllEssaisExpansion() As Task(Of IActionResult)
            Try
                ' ✅ CORRECTION: Retourne une IActionResult avec la liste
                Dim essais = Await _dbContext.ListeEssaiExpansionsDb.ToListAsync()
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai d'expansion par sa référence.
        ''' </summary>
        <HttpGet("{refEssai}")>
        Public Async Function GetEssaiExpansionByRef(refEssai As String) As Task(Of IActionResult)
            Try
                ' ✅ CORRECTION: Utilisation de FirstOrDefaultAsync
                Dim essai = Await _dbContext.ListeEssaiExpansionsDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai, CancellationToken.None)

                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai avec la référence {refEssai} introuvable."})
                End If
                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace