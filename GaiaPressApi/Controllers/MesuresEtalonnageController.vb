Imports GaiaPress.Core.V17.Datas
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPress.Core.V17
Imports System.Threading

Namespace GaiaPress.Core.V17.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class MesuresExpansionController
        Inherits ControllerBase

        Public ReadOnly _dbContext As GaiaPressDbContext

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures d'étalonnage.
        ''' </summary>
        <HttpGet>
        Public Async Function GetAllMesuresExpansion() As Task(Of IActionResult)
            Try
                ' ✅ Correction: Utilisation de ToListAsync
                Dim mesures = Await _dbContext.MesuresExpansion.ToListAsync()
                Return Ok(mesures)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère une mesure d'étalonnage par son indice.
        ''' </summary>
        <HttpGet("{indice}")>
        Public Async Function GetMesureEtalonnageByIndice(indice As Integer) As Task(Of IActionResult)
            Try
                ' ✅ Correction: Utilisation de FirstOrDefaultAsync
                Dim mesure = Await _dbContext.MesuresExpansion.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)

                If mesure Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Mesure avec l'indice {indice} introuvable."})
                End If
                Return Ok(mesure)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Ajoute une nouvelle mesure d'étalonnage.
        ''' </summary>
        <HttpPost>
        Public Async Function AddMesureEtalonnage(mesure As MesuresExpansion) As Task(Of IActionResult)
            Try
                If mesure Is Nothing Then
                    Return BadRequest(New With {Key .Message = "Le modèle de mesure est vide."})
                End If

                _dbContext.MesuresExpansion.Add(mesure)
                Await _dbContext.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetMesureEtalonnageByIndice), New With {Key .indice = mesure.IndiceMesure}, mesure)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Met à jour une mesure d'étalonnage existante.
        ''' </summary>
        <HttpPut("{indice}")>
        Public Async Function UpdateMesureEtalonnage(indice As Integer, mesure As MesuresExpansion) As Task(Of IActionResult)
            Try
                Dim existingMesure = Await _dbContext.MesuresExpansion.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)

                If existingMesure Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Mesure avec l'indice {indice} introuvable."})
                End If

                _dbContext.Entry(existingMesure).CurrentValues.SetValues(mesure)
                Await _dbContext.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace