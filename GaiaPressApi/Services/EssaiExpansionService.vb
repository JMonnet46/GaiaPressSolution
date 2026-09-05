Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des essais d'expansion.
    ''' </summary>
    Public Class EssaisExpansionService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de EssaisExpansionService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'expansion.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'expansion.</returns>
        Public Async Function GetAllEssaisExpansion() As Task(Of List(Of Essai_Expansion))
            Return Await _dbContext.EssaisExpansion.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un essai d'expansion par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'expansion.</param>
        ''' <returns>Essai d'expansion correspondant ou Nothing.</returns>
        Public Async Function GetEssaiExpansionByRef(refEssai As String) As Task(Of Essai_Expansion)
            Return Await _dbContext.EssaisExpansion.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
        End Function

        ''' <summary>
        ''' Ajoute un essai d'expansion.
        ''' </summary>
        ''' <param name="essai">Essai d'expansion à ajouter.</param>
        ''' <returns>Essai d'expansion ajouté.</returns>
        Public Async Function AddEssaiExpansion(essai As Essai_Expansion) As Task(Of Essai_Expansion)
            _dbContext.EssaisExpansion.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Met à jour un essai d'expansion.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'expansion avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateEssaiExpansion(refEssai As String, essai As Essai_Expansion) As Task(Of Boolean)
            Dim existingEssai = Await _dbContext.EssaisExpansion.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

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
            '' Ajoutez ici les autres propriétés de Essai_Expansion

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un essai d'expansion par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteEssaiExpansion(refEssai As String) As Task(Of Boolean)
            Dim essaiToDelete = Await _dbContext.EssaisExpansion.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)

            If essaiToDelete Is Nothing Then
                Return False
            End If

            _dbContext.EssaisExpansion.Remove(essaiToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de refEssai.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'expansion pour le forage spécifié.</returns>
        Public Async Function GetEssaisExpansionByRefEssai(refEssai As String) As Task(Of List(Of Essai_Expansion))
            Return Await _dbContext.EssaisExpansion.Where(Function(e) e.E_RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'expansion pour le chantier spécifié.</returns>
        Public Async Function GetEssaisExpansionByRefSonde(refSonde As String) As Task(Of List(Of Essai_Expansion))
            Return Await _dbContext.EssaisExpansion.Where(Function(e) e.E_RefSonde = refSonde).ToListAsync()
        End Function
    End Class
End Namespace