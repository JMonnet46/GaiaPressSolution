Imports GaiaPressCoreV17.Data
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading.Tasks
Imports System.Linq


Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service pour la gestion des essais d'expansion.
    ''' </summary>
    Public Class EssaiExpansionService

        Private ReadOnly _dbContext As GaiaPressDbContext



        ''' <summary>
        ''' Initialise une nouvelle instance de EssaisExpansionService.
        ''' </summary>
        ''' <param name="dbContext">Contexte Debase de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'expansion.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'expansion.</returns>
        Public Async Function GetAllEssaisExpansion() As Task(Of List(Of EssaiExpansion))
            Return Await _dbContext.ListeEssaiExpansionsDb.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un essai d'expansion par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'expansion.</param>
        ''' <returns>Essai d'expansion correspondant ou Nothing.</returns>
        Public Async Function GetEssaiExpansionByRef(refEssai As String) As Task(Of EssaiExpansion)
            Return Await _dbContext.ListeEssaiExpansionsDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)
        End Function

        ''' <summary>
        ''' Ajoute un essai d'expansion.
        ''' </summary>
        ''' <param name="essai">Essai d'expansion à ajouter.</param>
        ''' <returns>Essai d'expansion ajouté.</returns>
        Public Async Function AddEssaiExpansion(essai As EssaiExpansion) As Task(Of EssaiExpansion)
            _dbContext.ListeEssaiExpansionsDb.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Met à jour un essai d'expansion.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'expansion avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateEssaiExpansion(refEssai As String, essai As EssaiExpansion) As Task(Of Boolean)
            Dim existingEssai = Await _dbContext.ListeEssaiExpansionsDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)

            If existingEssai Is Nothing Then
                Return False
            End If

            ' Mise à jour des propriétés
            existingEssai = essai
            'existingEssai. RefEssai = essai. RefEssai
            'existingEssai. RefChantier = essai. RefChantier
            'existingEssai. RefForage = essai. RefForage
            'existingEssai. RefSonde = essai. RefSonde
            'existingEssai. nomEssai = essai. nomEssai
            'existingEssai. dateEssai = essai. dateEssai
            'existingEssai. heureEssai = essai. heureEssai
            '' Ajoutez ici les autres propriétés de EssaiExpansion

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un essai d'expansion par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteEssaiExpansion(refEssai As String) As Task(Of Boolean)
            Dim essaiToDelete = Await _dbContext.ListeEssaiExpansionsDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)

            If essaiToDelete Is Nothing Then
                Return False
            End If

            _dbContext.ListeEssaiExpansionsDb.Remove(essaiToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de refEssai.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'expansion pour le forage spécifié.</returns>
        Public Async Function GetEssaisExpansionByRefEssai(refEssai As String) As Task(Of List(Of EssaiExpansion))
            Return Await _dbContext.ListeEssaiExpansionsDb.Where(Function(e) e.RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion par référence de chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'expansion pour le chantier spécifié.</returns>
        Public Async Function GetEssaisExpansionByRefSonde(refSonde As String) As Task(Of List(Of EssaiExpansion))
            Return Await _dbContext.ListeEssaiExpansionsDb.Where(Function(e) e.RefSonde = refSonde).ToListAsync()
        End Function
    End Class
End Namespace