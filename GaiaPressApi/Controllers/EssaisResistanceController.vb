Namespace GaiaPressApi.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class EssaisResistanceController
        Inherits ControllerBase

        Public ReadOnly _dbContext As GaiaPressDbContext

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais de résistance.
        ''' </summary>
        <HttpGet>
        Public Async Function GetAllEssaisResistance() As Task(Of IActionResult)
            Try
                Dim essais = Await _dbContext.EssaisResistance.ToListAsync()
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai de résistance par sa référence.
        ''' </summary>
        <HttpGet("{refEssai}")>
        Public Async Function GetEssaiResistanceByRef(refEssai As String) As Task(Of IActionResult)
            Try
                ' ✅ CORRECTION: Utilisation de la bonne propriété (E_RefEssai doit exister dans Essai_Resistance)
                Dim essai = Await _dbContext.EssaisResistance.FirstOrDefaultAsync(Function(e) e.GetRefEssai = refEssai)

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