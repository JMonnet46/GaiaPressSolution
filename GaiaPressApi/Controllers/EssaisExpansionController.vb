Imports System.Threading

Namespace GaiaPressApi.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class EssaisExpansionController
        Inherits ControllerBase

        Private _dbContext As GaiaPressDbContext

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
                Dim essais = Await _dbContext.EssaisExpansion.ToListAsync()
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
                Dim essai = Await _dbContext.EssaisExpansion.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai, CancellationToken.None)

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


<ApiController>
<Route("[controller]")>
Public Class EssaisExpansionController
    Inherits ControllerBase

    Private ReadOnly _dbContext As GaiaPressDbContext

    Public Sub New(dbContext As GaiaPressDbContext)
        _dbContext = dbContext
    End Sub

    <HttpGet>
    Public Async Function GetAllEssaisExpansion() As Task(Of IActionResult)
        Try
            Dim essais = Await _dbContext.EssaisExpansion.ToListAsync()
            Return Ok(essais)
        Catch ex As Exception
            Return BadRequest(New With {.Message = $"Erreur : {ex.Message}"})
        End Try
    End Function

    <HttpGet("{refEssai}")>
    Public Async Function GetEssaiExpansionByRef(refEssai As String) As Task(Of IActionResult)
        Try
            Dim essai = Await _dbContext.EssaisExpansion.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
            If essai Is Nothing Then
                Return NotFound(New With {.Message = $"Essai avec la référence {refEssai} introuvable."})
            End If
            Return Ok(essai)
        Catch ex As Exception
            Return BadRequest(New With {.Message = $"Erreur : {ex.Message}"})
        End Try
    End Function
End Class