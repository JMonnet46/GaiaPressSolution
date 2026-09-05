Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des essais d'Resistance.
    ''' </summary>
    Public Class EssaisResistanceService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de EssaisResistanceService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'Resistance.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'Resistance.</returns>
        Public Async Function GetAllEssaisResistance() As Task(Of List(Of Essai_Resistance))
            Return Await _dbContext.EssaisResistance.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un essai d'Resistance par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'Resistance.</param>
        ''' <returns>Essai d'Resistance correspondant ou Nothing.</returns>
        Public Async Function GetEssaiResistanceByRef(refEssai As String) As Task(Of Essai_Resistance)
            Return Await _dbContext.EssaisResistance.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
        End Function

        ''' <summary>
        ''' Ajoute un essai d'Resistance.
        ''' </summary>
        ''' <param name="essai">Essai d'Resistance à ajouter.</param>
        ''' <returns>Essai d'Resistance ajouté.</returns>
        Public Async Function AddEssaiResistance(essai As Essai_Resistance) As Task(Of Essai_Resistance)
            _dbContext.EssaisResistance.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Met à jour un essai d'Resistance.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'Resistance avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateEssaiResistance(refEssai As String, essai As Essai_Resistance) As Task(Of Boolean)
            Dim existingEssai = Await _dbContext.EssaisResistance.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

            If existingEssai Is Nothing Then
                Return False
            End If

            ' Mise à jour des propriétés
            existingEssai = essai
            'existingEssai.E_RefEssai = essai.E_RefEssai
            'existingEssai.E_RefChantier = essai.E_RefChantier
            'existingEssai.E_RefForage = essai.E_RefForage
            'existingEssai.E_RefSonde = essai.E_RefSonde
            'existingEssai.E_nomEssai = essai.E_nomEssai
            'existingEssai.E_dateEssai = essai.E_dateEssai
            'existingEssai.E_heureEssai = essai.E_heureEssai
            '' Ajoutez ici les autres propriétés de Essai_Resistance

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un essai d'Resistance par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteEssaiResistance(refEssai As String) As Task(Of Boolean)
            Dim essaiToDelete = Await _dbContext.EssaisResistance.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

            If essaiToDelete Is Nothing Then
                Return False
            End If

            _dbContext.EssaisResistance.Remove(essaiToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de refEssai.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'Resistance pour le forage spécifié.</returns>
        Public Async Function GetEssaisResistanceByRefEssai(refEssai As String) As Task(Of List(Of Essai_Resistance))
            Return Await _dbContext.EssaisResistance.Where(Function(e) e.E_RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'Resistance pour le chantier spécifié.</returns>
        Public Async Function GetEssaisResistanceByRefSonde(refSonde As String) As Task(Of List(Of Essai_Resistance))
            Return Await _dbContext.EssaisResistance.Where(Function(e) e.E_RefSonde = refSonde).ToListAsync()
        End Function
    End Class
End Namespace
