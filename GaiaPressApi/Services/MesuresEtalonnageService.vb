Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des mesures d'étalonnage.
    ''' </summary>
    Public Class MesuresExpansionService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de MesuresExpansionService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures d'étalonnage.
        ''' </summary>
        ''' <returns>Liste de toutes les mesures d'étalonnage.</returns>
        Public Async Function GetAllMesuresExpansion() As Task(Of List(Of MesuresExpansion))
            Return Await _dbContext.MesuresExpansion.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère une mesure d'étalonnage par son indice.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure d'étalonnage.</param>
        ''' <returns>Mesure d'étalonnage correspondante ou Nothing.</returns>
        Public Async Function GetMesureEtalonnageByIndice(indice As Integer) As Task(Of MesuresExpansion)
            Return Await _dbContext.MesuresExpansion.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)
        End Function

        ''' <summary>
        ''' Récupère les mesures d'étalonnage par référence d'essai.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Liste des mesures d'étalonnage pour l'essai spécifié.</returns>
        Public Async Function GetMesuresExpansionByEssai(refEssai As String) As Task(Of List(Of MesuresExpansion))
            Return Await _dbContext.MesuresExpansion.Where(Function(m) m.RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Ajoute une mesure d'étalonnage.
        ''' </summary>
        ''' <param name="mesure">Mesure d'étalonnage à ajouter.</param>
        ''' <returns>Mesure d'étalonnage ajoutée.</returns>
        Public Async Function AddMesureEtalonnage(mesure As MesuresExpansion) As Task(Of MesuresExpansion)
            _dbContext.MesuresExpansion.Add(mesure)
            Await _dbContext.SaveChangesAsync()
            Return mesure
        End Function

        ''' <summary>
        ''' Met à jour une mesure d'étalonnage.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure à mettre à jour.</param>
        ''' <param name="mesure">Mesure d'étalonnage avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateMesureEtalonnage(indice As Integer, mesure As MesuresExpansion) As Task(Of Boolean)
            Dim existingMesure = Await _dbContext.MesuresExpansion.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)

            If existingMesure Is Nothing Then
                Return False
            End If

            ' Mise à jour des propriétés
            existingMesure = mesure
            'existingMesure.RefEssai = mesure.RefEssai
            'existingMesure.IndiceMesure = mesure.IndiceMesure
            'existingMesure.Pression = mesure.Pression
            'existingMesure.Volume = mesure.Volume
            'existingMesure.Temperature = mesure.Temperature

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime une mesure d'étalonnage par son indice.
        ''' </summary>
        ''' <param name="indice">Indice de la mesure à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteMesureEtalonnage(indice As Integer) As Task(Of Boolean)
            Dim mesureToDelete = Await _dbContext.MesuresExpansion.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)

            If mesureToDelete Is Nothing Then
                Return False
            End If

            _dbContext.MesuresExpansion.Remove(mesureToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function
    End Class
End Namespace