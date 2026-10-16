Imports GaiaPressCoreV17.Data
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading.Tasks
Imports System.Linq
Imports System.Collections.Generic


Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service pour la gestion des essais d'Resistance.
    ''' </summary>
    Public Class EssaisResistanceDbService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de EssaisResistanceDbService.
        ''' </summary>
        ''' <param name="dbContext">Contexte Debase de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les essais d'Resistance.
        ''' </summary>
        ''' <returns>Liste de tous les essais d'Resistance.</returns>
        Public Async Function GetAllEssaisResistanceDb() As Task(Of List(Of EssaiResistance))
            Return Await _dbContext.ListeEssaiResistancesDb.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un essai d'Resistance par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai d'Resistance.</param>
        ''' <returns>Essai d'Resistance correspondant ou Nothing.</returns>
        Public Async Function GetEssaiResistanceByRef(refEssai As String) As Task(Of EssaiResistance)
            Return Await _dbContext.ListeEssaiResistancesDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)
        End Function

        ''' <summary>
        ''' Ajoute un essai d'Resistance.
        ''' </summary>
        ''' <param name="essai">Essai d'Resistance à ajouter.</param>
        ''' <returns>Essai d'Resistance ajouté.</returns>
        Public Async Function AddEssaiResistance(essai As EssaiResistance) As Task(Of EssaiResistance)
            _dbContext.ListeEssaiResistancesDb.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Met à jour un essai d'Resistance.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à mettre à jour.</param>
        ''' <param name="essai">Essai d'Resistance avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateEssaiResistance(refEssai As String, essai As EssaiResistance) As Task(Of Boolean)
            Dim existingEssai = Await _dbContext.ListeEssaiResistancesDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)

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
            '' Ajoutez ici les autres propriétés de EssaiResistance

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un essai d'Resistance par sa référence.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteEssaiResistance(refEssai As String) As Task(Of Boolean)
            Dim essaiToDelete = Await _dbContext.ListeEssaiResistancesDb.FirstOrDefaultAsync(Function(e) e.RefEssai = refEssai)

            If essaiToDelete Is Nothing Then
                Return False
            End If

            _dbContext.ListeEssaiResistancesDb.Remove(essaiToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de refEssai.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais d'Resistance pour le forage spécifié.</returns>
        Public Async Function GetEssaisResistanceDbByRefEssai(refEssai As String) As Task(Of List(Of EssaiResistance))
            Return Await _dbContext.ListeEssaiResistancesDb.Where(Function(e) e.RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère les essais d'Resistance par référence de chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des essais d'Resistance pour le chantier spécifié.</returns>
        Public Async Function GetEssaisResistanceDbByRefSonde(refSonde As String) As Task(Of List(Of EssaiResistance))
            Return Await _dbContext.ListeEssaiResistancesDb.Where(Function(e) e.RefSonde = refSonde).ToListAsync()
        End Function
    End Class
End Namespace
