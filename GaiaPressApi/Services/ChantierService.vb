Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports GaiaPress.core.V17.Data
Imports GaiaPress.core.V17.Models
Imports Microsoft.EntityFrameworkCore


Namespace GaiaPress.core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des chantiers.
    ''' </summary>
    Public Class ChantierService

        Public ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de ChantierService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les chantiers.
        ''' </summary>
        Public Async Function GetAllChantiers() As Task(Of List(Of LesChantiers))
            Return Await _dbContext.LesChantiers.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un chantier par sa référence.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        Public Async Function GetChantierByRef(refChantier As String) As Task(Of LesChantiers)
            Return Await _dbContext.LesChantiers.FirstOrDefaultAsync(Function(c) c.C_refChantier = refChantier, CancellationToken.None)
        End Function

        ''' <summary>
        ''' Ajoute un nouveau chantier.
        ''' </summary>
        ''' <param name="chantier">Chantier à Add.</param>
        Public Async Function AddChantier(chantier As LesChantiers) As Task(Of LesChantiers)
            _dbContext.LesChantiers.Add(chantier)
            Await _dbContext.SaveChangesAsync()
            Return chantier
        End Function

        ''' <summary>
        ''' Met à jour un chantier existant.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <param name="chantier">Données du chantier mises à jour.</param>
        Public Async Function UpdateChantier(refChantier As String, chantier As LesChantiers) As Task(Of Boolean)
            Dim existingChantier = Await _dbContext.LesChantiers.FirstOrDefaultAsync(Function(c) c.C_refChantier = refChantier, CancellationToken.None)

            If existingChantier Is Nothing Then
                Return False
            End If

            _dbContext.Entry(existingChantier).CurrentValues.SetValues(chantier)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un chantier par sa référence.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        Public Async Function DeleteChantier(refChantier As String) As Task(Of Boolean)
            Dim chantier = Await _dbContext.LesChantiers.FirstOrDefaultAsync(Function(c) c.C_refChantier = refChantier, CancellationToken.None)

            If chantier Is Nothing Then
                Return False
            End If

            _dbContext.LesChantiers.Remove(chantier)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function
    End Class
End Namespace