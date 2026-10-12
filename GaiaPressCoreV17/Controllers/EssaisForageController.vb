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
    Public Class EssaisForageController
        Inherits ControllerBase

        Public ReadOnly _dbContext As GaiaPressDbContext

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais de forage.
        ''' </summary>
        <HttpGet>
        Public Async Function GetAllEssaisForages() As Task(Of IActionResult)
            Try
                ' ✅ Correction: Utilisation de ToListAsync avec CancellationToken
                Dim essais = Await _dbContext.ListeEssaiForageDb.ToListAsync(CancellationToken.None)
                ' ✅ Correction: Utilisation de Ok() avec le bon namespace
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        <HttpGet("{refEssai}")>
        Public Async Function GetEssaiForageByRef(refEssai As String) As Task(Of IActionResult)
            Try
                ' ✅ Correction: Utilisation de FirstOrDefaultAsync avec CancellationToken
                Dim essai = Await _dbContext.ListeEssaiForageDb.FirstOrDefaultAsync(Function(e) e.refEssai = refEssai, CancellationToken.None)

                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai avec la référence {refEssai} introuvable."})
                End If
                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai de forage par référence de forage.
        ''' </summary>
        <HttpGet("byForage/{refForage}")>
        Public Async Function GetEssaiForageByForageRef(refForage As String) As Task(Of IActionResult)
            Try
                ' ✅ Correction: Utilisation de Where et ToListAsync
                Dim essais = Await _dbContext.ListeEssaiForageDb.Where(Function(e) e.RefForage = refForage).ToListAsync(CancellationToken.None)

                If essais Is Nothing OrElse essais.Count = 0 Then
                    Return NotFound(New With {Key .Message = $"Aucun essai trouvé pour le forage {refForage}."})
                End If
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace