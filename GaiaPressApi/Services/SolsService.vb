Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des sols.
    ''' </summary>
    Public Class SolsService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de SolService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère tous les sols.
        ''' </summary>
        ''' <returns>Liste de tous les sols.</returns>
        Public Async Function GetAllSols() As Task(Of List(Of Sol))
            Return Await _dbContext.Sols.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère un sol par sa référence.
        ''' </summary>
        ''' <param name="refSol">Référence du sol.</param>
        ''' <returns>Sol correspondant ou Nothing.</returns>
        Public Async Function GetSolByRefSol(refSol As String) As Task(Of Sol)
            Return Await _dbContext.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = refSol)
        End Function

        '''' <summary>
        '''' Récupère les sols par référence de chantier.
        '''' </summary>
        '''' <param name="refChantier">Référence du chantier.</param>
        '''' <returns>Liste des sols pour le chantier spécifié.</returns>
        'Public Async Function GetSolsByrefSol(refSol As String) As Task(Of List(Of Sol))
        '    Return Await _dbContext.Sols.Where(Function(s) s.RefSol = refSol).ToListAsync()
        'End Function

        ''' <summary>
        ''' Ajoute un nouveau sol.
        ''' </summary>
        ''' <param name="sol">Sol à ajouter.</param>
        ''' <returns>Sol ajouté.</returns>
        Public Async Function AddSol(sol As Sol) As Task(Of Sol)
            _dbContext.Sols.Add(sol)
            Await _dbContext.SaveChangesAsync()
            Return sol
        End Function

        ''' <summary>
        ''' Met à jour un sol existant.
        ''' </summary>
        ''' <param name="refSol">Référence du sol à mettre à jour.</param>
        ''' <param name="sol">Sol avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateSol(refSol As String, sol As Sol) As Task(Of Boolean)
            Dim existingSol = Await _dbContext.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = refSol)

            If existingSol Is Nothing Then
                Return False
            End If

            ' Mise à jour des propriétés
            existingSol = sol
            'existingSol.RefSol = sol.RefSol
            'existingSol.RefChantier = sol.RefChantier
            'existingSol.NomSol = sol.nomSol
            'existingSol.TypeSol = sol.TypeSol
            'existingSol.Profondeur = sol.Profondeur
            'existingSol.Densite = sol.Densite
            '' Ajoutez ici les autres propriétés de Sol

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime un sol par sa référence.
        ''' </summary>
        ''' <param name="refSol">Référence du sol à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteSol(refSol As String) As Task(Of Boolean)
            Dim solToDelete = Await _dbContext.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = refSol)

            If solToDelete Is Nothing Then
                Return False
            End If

            _dbContext.Sols.Remove(solToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime tous les sols associés à un chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteSolsByrefSol(refSol As String) As Task(Of Boolean)
            Dim solsToDelete = Await _dbContext.Sols.Where(Function(s) s.RefSol = refSol).ToListAsync()

            If solsToDelete.Count = 0 Then
                Return False
            End If

            _dbContext.Sols.RemoveRange(solsToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function
    End Class
End Namespace