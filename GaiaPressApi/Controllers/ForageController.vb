Imports System.Threading

Namespace GaiaPressApi.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class ForageController
        Inherits ControllerBase

        Public ReadOnly _dbContext As GaiaPressDbContext

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Ajoute un nouveau forage.
        ''' </summary>
        <HttpPost>
        Public Async Function AddForage(forage As Forage) As Task(Of IActionResult)
            Try
                If forage Is Nothing Then
                    Return BadRequest(New With {Key .Message = "Le modèle de forage est vide."})
                End If

                _dbContext.Forages.Add(forage)
                Await _dbContext.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetForageByRef), New With {Key .refForage = forage.F_RefForage}, forage)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un forage par sa référence.
        ''' </summary>
        <HttpGet("{refForage}")>
        Public Async Function GetForageByRef(refForage As String) As Task(Of IActionResult)
            Try
                Dim forage = Await _dbContext.Forages.FirstOrDefaultAsync(Function(f) f.F_RefForage = refForage, CancellationToken.None)

                If forage Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Forage avec la référence {refForage} introuvable."})
                End If
                Return Ok(forage)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace

