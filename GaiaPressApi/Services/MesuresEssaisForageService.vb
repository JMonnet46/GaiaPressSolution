Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des mesures d'essais de forage.
    ''' </summary>
    Public Class MesuresEssaisForageervice

        ' ✅ Utiliser une instance de LesChantiers comme source de vérité
        'Public _chantier As LesChantiers

        ' ✅ Ajoutez ces déclarations en haut de la classe
        Public _c_EventsSuspendus As Boolean = False
        Public _HasBeenModified As Boolean = False

        Public ReadOnly _dbContext As GaiaPressDbContext
        Public ReadOnly _EssaisForageService As EssaisForageService



        ' ✅ Constructeur pour injecter un chantier ET le DbContext
        Public Sub New(dbContext As GaiaPressDbContext, EssaisForageService As EssaisForageService)
            _dbContext = dbContext
            _EssaisForageService = EssaisForageService
        End Sub

        ' ===============================================================================
        ' Gestion de C_ListMesuresForage
        ' ===============================================================================

        ''' <summary>
        ''' Add une mesure d'essai de forage au chantier
        ''' </summary>
        Public Sub AddMesuresEssaisForage(Mesure As MesuresEssaiForage)
            ' ✅ Utiliser la liste du chantier (C_ListMesuresForage)
            MyEssai.E_Mesures.Add(Mesure)
            ' Lier la mesure au chantier (si nécessaire)
            ' Mesure.E_RefChantier = _chantier.C_refChantier
        End Sub

        ''' <summary>
        ''' Get une mesure d'essai de forage par indice
        ''' </summary>
        Public Function GetMesuresForage(Indice As Integer) As MesuresEssaiForage
            ' ✅ Utiliser la liste du chantier (C_ListMesuresForage)
            Return MyEssai.E_Mesures.FirstOrDefault(Function(m) m.IndiceMesure = Indice)
        End Function

        ''' <summary>
        ''' Delete une mesure d'essai de forage par indice
        ''' </summary>
        Public Sub DeleteMesuresForage(Indice As Integer)
            Dim MesureADelete As MesuresEssaiForage = GetMesuresForage(Indice)
            If MesureADelete IsNot Nothing Then
                ' ✅ Utiliser la liste du chantier (C_ListMesuresForage)
                MyEssai.E_Mesures.Remove(MesureADelete)
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Mettre à jour une mesure d'essai de forage
        ''' </summary>
        Public Sub MettreAJourMesuresForage(Indice As Integer, nouveauNom As String, nouvelleDate As String)
            Dim Mesure As MesuresEssaiForage = GetMesuresForage(Indice)
            If Mesure IsNot Nothing Then
                ' Adaptez selon les propriétés réelles de MesuresEssaiForage
                ' Mesure.E_nomEssai = nouveauNom
                ' Mesure.E_dateEssai = nouvelleDate
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures d'essais de forage.
        ''' </summary>
        Public Async Function GetAllMesuresForage() As Task(Of List(Of MesuresEssaiForage))
            Return Await _dbContext.MesuresEssaiForage.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère une mesure d'essai de forage par son indice
        ''' </summary>
        Public Function GetMesuresForageByIndice(Indice As Integer) As MesuresEssaiForage
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
        'Public Async Function AddMesureToEssai(refEssai As String, mesure As MesuresEssaiForage) As Task(Of IActionResult)
        '    mesure.RefEssai = refEssai
        '    _context.MesuresEssaiForage.Add(mesure)

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
        Public Async Function GetMesureForageByIndice(indice As Integer) As Task(Of MesuresEssaiForage)
            Return Await _dbContext.MesuresEssaiForage.FirstOrDefaultAsync(Function(m) m.indiceMesure = indice)
        End Function


        ''' <summary>
        ''' Met à jour une mesure d'essai de forage.
        ''' </summary>
        Public Async Function UpdateMesuresEssaiForage(indice As Integer, nouvelleMesure As MesuresEssaiForage) As Task
            Dim mesure As MesuresEssaiForage = Await GetMesureForageByIndice(indice)
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
        Public Sub AddMesureToEssai(refEssai As String, mesure As MesuresEssaiForage)
            mesure.RefEssai = refEssai
            _dbContext.MesuresEssaiForage.Add(mesure)
            _dbContext.SaveChanges()  ' ✅ Pas de Await ici car la méthode n'est pas Async
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures associées à un essai.
        ''' </summary>
        ''' <param name="refEssai">Référence de l'essai.</param>
        ''' <returns>Liste des mesures associées à l'essai.</returns>
        Public Async Function GetMesuresByRefEssai(refEssai As String) As Task(Of List(Of MesuresEssaiForage))
            Return Await _dbContext.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync()
        End Function

        ''' <summary>
        ''' Supprime toutes les mesures associées à un essai
        ''' </summary>
        ' ✅ Correct
        Public Async Function DeleteMesuresByEssai(refEssai As String) As Task
            Dim mesuresToRemove = Await _dbContext.MesuresEssaiForage.Where(Function(m) m.RefEssai = refEssai).ToListAsync()

            _dbContext.MesuresEssaiForage.RemoveRange(mesuresToRemove)
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
            _c_EventsSuspendus = state
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
        '    'Return _chantier.C_ListMesuresForage.Count(Function(m) m.RefEssai = refEssai)
        '    Return _chantier.C_ListMesuresForage.Any(Function(m) m.RefEssai = refEssai AndAlso m.indiceMesure = indice)
        'End Function
        Public Function GetMesuresCountByEssai(refEssai As String) As Integer
            ' ✅ Correction : Utiliser Where + Count()
            Return MyEssai.E_Mesures.Where(Function(m) m.RefEssai = refEssai).Count()
        End Function

        ''' <summary>
        ''' Vérifie si une mesure existe pour un essai donné
        ''' </summary>
        Public Function MesureExistsForEssai(refEssai As String, indice As Integer) As Boolean
            Return MyEssai.E_Mesures.Any(Function(m) m.RefEssai = refEssai AndAlso m.IndiceMesure = indice)
        End Function
    End Class
End Namespace
