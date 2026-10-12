Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading
Imports GaiaPressCoreV17.Datas

Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service pour la gestion des mesures d'essais de forage.
    ''' </summary>
    Public Class MesuresEssaisForageervice

        ' ✅ Utiliser une instance de LesChantier comme source de vérité
        'Public _chantier As LesChantier

        ' ✅ Ajoutez ces déclarations en haut de la classe
        Public _EventsSuspendus As Boolean = False
        Public _HasBeenModified As Boolean = False

        Public ReadOnly _dbContext As GaiaPressDbContext
        Public ReadOnly _EssaiForageService As EssaiForageService



        ' ✅ Constructeur pour injecter un chantier ET le DbContext
        Public Sub New(dbContext As GaiaPressDbContext, EssaiForageService As EssaiForageService)
            _dbContext = dbContext
            _EssaiForageService = EssaiForageService
        End Sub

        ' ===============================================================================
        ' Gestion de  ListMesuresForage
        ' ===============================================================================

        ''' <summary>
        ''' Add une mesure d'essai de forage au chantier
        ''' </summary>
        Public Sub AddMesuresEssaisForage(Mesure As MesureEssaiForage)
            ' ✅ Utiliser la liste du chantier ( ListMesuresForage)
            MyEssaiForage. Mesures.Add(Mesure)
            ' Lier la mesure au chantier (si nécessaire)
            ' Mesure. RefChantier = _chantier. refChantier
        End Sub

        ''' <summary>
        ''' Get une mesure d'essai de forage par indice
        ''' </summary>
        Public Function GetMesuresForage(Indice As Integer) As MesureEssaiForage
            ' ✅ Utiliser la liste du chantier ( ListMesuresForage)
            Return MyEssaiForage. Mesures.FirstOrDefault(Function(m) m.IndiceMesure = Indice)
        End Function

        ''' <summary>
        ''' Delete une mesure d'essai de forage par indice
        ''' </summary>
        Public Sub DeleteMesuresForage(Indice As Integer)
            Dim MesureADelete As MesureEssaiForage = GetMesuresForage(Indice)
            If MesureADelete IsNot Nothing Then
                ' ✅ Utiliser la liste du chantier ( ListMesuresForage)
                MyEssaiForage. Mesures.Remove(MesureADelete)
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Mettre à jour une mesure d'essai de forage
        ''' </summary>
        Public Sub MettreAJourMesuresForage(Indice As Integer, nouveauNom As String, nouvelleDate As String)
            Dim Mesure As MesureEssaiForage = GetMesuresForage(Indice)
            If Mesure IsNot Nothing Then
                ' Adaptez selon les propriétés réelles de MesureEssaiForage
                ' Mesure. nomEssai = nouveauNom
                ' Mesure. dateEssai = nouvelleDate
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures d'essais de forage.
        ''' </summary>
        Public Async Function GetAllMesuresForage() As Task(Of List(Of MesureEssaiForage))
            Return Await _dbContext.ListeMesureEssaiForageDb.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère une mesure d'essai de forage par son indice
        ''' </summary>
        Public Function GetMesuresForageByIndice(Indice As Integer) As MesureEssaiForage
            Return GetMesuresForage(Indice)
        End Function

        ' ===============================================================================
        ' Gestion des Mesures par Essai
        ' ===============================================================================

        ''' <summary>
        ''' Add une mesure à un essai spécifique
        ''' </summary>
        '''  ''' <summary>
        ''' Ajoute une mesure à un essai de forage.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <param name="mesure">Mesure à Add.</param>
        ''' <returns>Résultat de l'opération.</returns>
        'Public Async Function AddMesureToEssai(refEssai As String, mesure As MesureEssaiForage) As Task(Of IActionResult)
        '    mesure.RefEssai = refEssai
        '    _context.MesureEssaiForage.Add(mesure)

        '    Try
        '        Await _context.SaveChangesAsync()
        '        Return CreatedAtAction(NameOf(GetMesureByIndice), New {indice = mesure.indiceMesure}, mesure)
        '    Catch ex As Exception
        '        Return BadRequest(ex.Message)
        '    End Try
        'End Function

        ''' <summary>
        ''' Récupère une mesure d'essai de forage par son indice.
        ''' </summary>
        Public Async Function GetMesureForageByIndice(indice As Integer) As Task(Of MesureEssaiForage)
            Return Await _dbContext.ListeMesureEssaiForageDb.FirstOrDefaultAsync(Function(m) m.IndiceMesure = indice)
        End Function


        ''' <summary>
        ''' Met à jour une mesure d'essai de forage.
        ''' </summary>
        Public Async Function UpdateMesureEssaiForage(indice As Integer, nouvelleMesure As MesureEssaiForage) As Task
            Dim mesure As MesureEssaiForage = Await GetMesureForageByIndice(indice)
            If mesure IsNot Nothing Then
                ' ✅ Correction: Utilisation des bonnes propriétés
                mesure.RefEssai = nouvelleMesure.RefEssai
                mesure = nouvelleMesure
                Await _dbContext.SaveChangesAsync()
            End If
        End Function

        ''' <summary>
        ''' Ajoute une mesure à un essai.
        ''' </summary>
        Public Sub AddMesureToEssai(refEssai As String, mesure As MesureEssaiForage)
            mesure.RefEssai = refEssai
            _dbContext.ListeMesureEssaiForageDb.Add(mesure)
            _dbContext.SaveChanges()  ' ✅ Pas de Await ici car la méthode n'est pas Async
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures associées à un essai.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Liste des mesures associées à l'essai.</returns>
        Public Async Function GetMesuresByRefEssai(refEssai As String) As Task(Of List(Of MesureEssaiForage))
            Return Await _dbContext.ListeMesureEssaiForageDb.Where(Function(m) m.RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Supprime toutes les mesures associées à un essai
        ''' </summary>
        ' ✅ Correct
        Public Async Function DeleteMesuresByEssai(refEssai As String) As Task
            Dim mesuresToRemove = Await _dbContext.ListeMesureEssaiForageDb.Where(Function(m) m.RefEssai = refEssai).ToListAsync()

            _dbContext.ListeMesureEssaiForageDb.RemoveRange(mesuresToRemove)
            Await _dbContext.SaveChangesAsync()
            _HasBeenModified = True
        End Function

        ' ===============================================================================
        ' Gestion des événements
        ' ===============================================================================

        ''' <summary>
        ''' Suspend ou reprend les événements.
        ''' </summary>
        ''' <param name="state">État des événements.</param>
        Public Sub SetEventsSuspendus(state As Boolean)
            _EventsSuspendus = state
        End Sub

        ''' <summary>
        ''' Vérifie si des modifications ont été apportées.
        ''' </summary>
        ''' <returns>True si des modifications ont été apportées, False sinon.</returns>
        Public Function HasBeenModified() As Boolean
            Return _HasBeenModified
        End Function


        ' ===============================================================================
        ' Méthodes utilitaires
        ' ===============================================================================

        ''' <summary>
        ''' Récupère le nombre total de mesures pour un essai
        ''' </summary>
        'Public Function GetMesuresCountByEssai(refEssai As String) As Integer
        '    'Return _chantier. ListMesuresForage.Count(Function(m) m.RefEssai = refEssai)
        '    Return _chantier. ListMesuresForage.Any(Function(m) m.RefEssai = refEssai AndAlso m.indiceMesure = indice)
        'End Function
        Public Function GetMesuresCountByEssai(refEssai As String) As Integer
            ' ✅ Correction : Utiliser Where + Count()
            Return MyEssaiForage. Mesures.Where(Function(m) m.RefEssai = refEssai).Count()
        End Function

        ''' <summary>
        ''' Vérifie si une mesure existe pour un essai donné
        ''' </summary>
        Public Function MesureExistsForEssai(refEssai As String, indice As Integer) As Boolean
            Return MyEssaiForage. Mesures.Any(Function(m) m.RefEssai = refEssai AndAlso m.IndiceMesure = indice)
        End Function
    End Class
End Namespace
