Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports Microsoft.EntityFrameworkCore
Imports System.Collections.Generic

Namespace GaiaPressCoreV17.Services

    Public Class EssaiForageService

        Private _dbContext As GaiaPressDbContext

        Private _listeEssaiForageSession As List(Of Essai_Forage)

        ' ✅ Déclarer les membres manquants (si nécessaire pour la logique métier)
        Public _EventsSuspendus As Boolean = False
        Public _HasBeenModified As Boolean = False

        ' Liste des variables

        '=========================================================================
        ' ✅ Constructeur qui injecte le DbContext
        '=========================================================================
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
            _listeEssaiForageSession = New List(Of Essai_Forage)()
        End Sub

        '' ✅ Déclaration de toutes les méthodes utilisées dans le contrôleur
        'Public Function GetAllEssaisForages() As List(Of EssaiForage)
        '    Return _context.EssaisForage.ToList()
        'End Function

        'Public Function GetEssaiForageByRef(refEssai As String) As Essai_Forage

        '    ' ------------------------------------
        '    ' Listing modifié début
        '    '-------------------------------------
        '    Dim i As Integer = 0
        '    Dim unique As Boolean

        '    unique = True

        '    Do While (i < MyListeEssaiForageSession.Count) And unique
        '        unique = (MyListeEssaiForageSession(i).refEssai <> ref)
        '        i = i + 1
        '    Loop

        '    ' ------------------------------------
        '    ' Listing modifié Fin
        '    '-------------------------------------

        '    Return _context.ListeEssaiForageDb.FirstOrDefault(Function(e) e.E_RefEssai = refEssai)

        '    ' IsRefUnique = unique '

        'End Function

        Public Function GetEssaiForageByRefEssai(refEssai As String) As Essai_Forage

            If _listeEssaiForageSession Is Nothing Then
                Return Nothing
            End If

            Return _listeEssaiForageSession.FirstOrDefault(
        Function(e) e.refEssai = refEssai)
        End Function

        'Public Function UpdateEssaiForage(essai As EssaiForage) As EssaiForage
        '    _context.EssaisForage.Update(essai)
        '    _context.SaveChanges()
        '    Return essai
        'End Function

        'Public Function DeleteEssaiForage(refEssai As String) As Boolean
        '    Dim essai = _context.EssaisForage.FirstOrDefault(Function(e) e.E_RefEssai = refEssai)
        '    If essai IsNot Nothing Then
        '        _context.EssaisForage.Remove(essai)
        '        _context.SaveChanges()
        '        Return True
        '    End If
        '    Return False
        'End Function

        '====================================================================
        ' Proposition de Le Chat
        '====================================================================


        ' Vos autres méthodes...

        Public Async Function DeleteEssaiForage(refEssai As String) As Task(Of Boolean)
            Dim essai = Await _dbContext.ListeEssaiForageDb.
            FirstOrDefaultAsync(Function(e) e.refEssai = refEssai)

            If essai Is Nothing Then Return False

                _dbContext.ListeEssaiForageDb.Remove(essai)
                Await _dbContext.SaveChangesAsync()
                Return True
            End Function



        'Public Async Function DeleteEssaiForage(refEssai As String) As Task(Of IActionResult)
        '    Try
        '        ' Vérifiez que le service retourne bien une Task
        '        'Dim essai = Await _essaiForageService.ListeEssaiForageDb.GetEssaiForageByRefEssai(refEssai)
        '        Dim essai = Await _essaiForageService.ListeEssaiForageDb.GetEssaiForageByRefEssai(refEssai)

        '        If essai Is Nothing Then
        '            Return NotFound(New With {Key .Message = $"Essai de forage avec la référence {refEssai} introuvable."})
        '        End If

        '        ' Assurez-vous que DeleteEssaiForage retourne Task
        '        Await _essaiForageService.DeleteEssaiForage(refEssai)

        '        Return Ok(New With {Key .Message = "Essai de forage supprimé avec succès."})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression de l'essai de forage : {ex.Message}"})
        '    End Try
        'End Function

        ' ✅ Ajoutez ici toutes vos autres méthodes existantes (214 lignes)

        ' ===============================================================================
        ' Gestion de  ListEssaisForage
        ' ===============================================================================

        ''' <summary>
        ''' Add un essai forage au chantier
        ''' </summary>
        ' Charger les essais depuis la base vers la mémoire
        Public Async Function ChargerEssaisDepuisBase() As Task
            _listeEssaiForageSession = Await _dbContext.ListeEssaiForageDb.ToListAsync()
        End Function

        ' Sauvegarder les essais de la mémoire vers la base
        'Public Sub SauvegarderEssaisEnBase()
        '    For Each essai In _listeEssaisEnMemoire
        '        If _dbContext.ListeEssaiForage.Any(Function(e) e.refEssai = essai.RefEssai) Then
        '            _dbContext.ListeEssaiForage.Update(essai)
        '        Else
        '            _dbContext.ListeEssaiForage.Add(essai)
        '        End If
        '    Next
        '    _dbContext.SaveChanges()
        'End Sub

        Public Sub SaveEssaiForage(essaiForage As Essai_Forage)
            If essaiForage.refEssai = "" Then
                _dbContext.ListeEssaiForageDb.Add(essaiForage)
            Else
                _dbContext.ListeEssaiForageDb.Update(essaiForage)
            End If
            _dbContext.SaveChanges()
        End Sub


        Public Async Function AddEssaiForage(essai As Essai_Forage) As Task(Of Essai_Forage)
            If essai Is Nothing Then
                Throw New ArgumentNullException(NameOf(essai), "L'essai de forage ne peut pas être vide.")
            End If

            _dbContext.ListeEssaiForageDb.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Récupère tous les essais de forage.
        ''' </summary>
        ''' <returns>Liste de tous les essais de forage.</returns>
        Public Function GetAllEssaisForages() As List(Of Essai_Forage)
            Dim essais = _dbContext.ListeEssaiForageDb.ToList()

            ' Mettre à jour la liste globale
            ModBibliotheque.MyListeEssaiForageSession = essais

            Return essais
            ' Récupérer depuis la base de données
            ' Return _context.EssaiForages.ToList()
        End Function

        ''' <summary>
        ''' Mettre à jour un essai forage
        ''' </summary>
        Public Sub UpDateEssaisForage(NouvelEssai As Essai_Forage)

            '    Public Async Function Exemple(refEssai As String) As Task(Of Essai_Forage)
            '       Dim essai As Essai_Forage =
            '        Await GetEssaiForageByRefEssai(refEssai)

            '       Return essai
            '    End Function

            Dim essai As Essai_Forage = GetEssaiForageByRefEssai(NouvelEssai.refEssai)
            If essai IsNot Nothing Then
                essai.RefChantier = NouvelEssai.RefChantier
                essai.RefForage = NouvelEssai.RefForage
                essai.refEssai = NouvelEssai.refEssai
                essai.refSonde = NouvelEssai.refSonde
                essai.refEssaiResistancePropre = NouvelEssai.refEssaiResistancePropre
                essai.refEssaiExpansionPropre = NouvelEssai.refEssaiExpansionPropre
                essai.refEssaiPerteDeCharge = NouvelEssai.refEssaiPerteDeCharge
                essai.RefSol = NouvelEssai.RefSol
                essai.Enregistreur = NouvelEssai.Enregistreur
                essai.Operateur = NouvelEssai.Operateur
                essai.VolSonde = NouvelEssai.VolSonde
                essai.ClassPrecis = NouvelEssai.ClassPrecis
                essai.PrecisVolSonde = NouvelEssai.PrecisVolSonde
                essai.nomEssai = NouvelEssai.nomEssai
                essai.dateEssai = NouvelEssai.dateEssai
                essai.heureEssai = NouvelEssai.heureEssai
                essai.difPresEauAir = NouvelEssai.difPresEauAir
                essai.hauteurCpv = NouvelEssai.hauteurCpv
                essai.presP0 = NouvelEssai.presP0
                essai.presFluage = NouvelEssai.presFluage
                essai.presLimite = NouvelEssai.presLimite
                essai.presLimiteNF = NouvelEssai.presLimiteNF
                essai.presLimiteTheo = NouvelEssai.presLimiteTheo
                essai.PrecisVolume = NouvelEssai.PrecisVolume
                essai.PrecisPression = NouvelEssai.PrecisPression
                essai.poidsVol = NouvelEssai.poidsVol
                essai.poidsVolDejauge = NouvelEssai.poidsVolDejauge
                essai.coefPoisson = NouvelEssai.coefPoisson
                essai.modPressio = NouvelEssai.modPressio
                essai.modPressioNf = NouvelEssai.modPressioNf
                essai.modElastique = NouvelEssai.modElastique
                essai.modElastiqueNf = NouvelEssai.modElastiqueNf
                essai.Cohesion = NouvelEssai.Cohesion
                essai.Frottement = NouvelEssai.Frottement
                essai.FrottementIntergranulaire = NouvelEssai.FrottementIntergranulaire
                essai.U0 = NouvelEssai.U0
                essai.profNappe = NouvelEssai.profNappe
                essai.profSonde = NouvelEssai.profSonde
                essai.coteZeroGeomeca = NouvelEssai.coteZeroGeomeca
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère les essais de forage par référence de forage.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais de forage pour le forage spécifié.</returns>
        Public Async Function GetEssaisForageByRefForage(refForage As String) As Task(Of List(Of Essai_Forage))
            Return Await _dbContext.ListeEssaiForageDb.Where(Function(e) e.RefForage = refForage).ToListAsync()
        End Function


        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        '' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' Retourne Nothing si aucun essai ne correspond.
        ''' </summary>
        'Public Async Function GetEssaiForageByRefEssai(refEssai As String) As Task(Of Essai_Forage)
        'Public Async Function GetEssaiForageByRefEssai(refEssai As String) As Task(Of Essai_Forage)

        '    Return Await _dbContext.ListeEssaiForageDb _
        '    .FirstOrDefaultAsync(Function(e) e.refEssai = refEssai)
        'End Function

        ' ===============================================================================
        ' Gestion des Mesures associées aux Essais de Forage
        ' ===============================================================================

        ''' <summary>
        ''' Add une mesure à un essai de forage
        ''' </summary>
        Public Sub AddMesureToEssai(refEssai As String, mesure As MesureEssaiForage)
            Dim essai = GetEssaiForageByRefEssai(refEssai)
            If essai IsNot Nothing Then
                MyEssaiForage.Mesures.Add(mesure)
                mesure.RefEssai = refEssai
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Get une mesure par indice
        ''' </summary>
        Public Function GetMesuresForage(Indice As Integer) As MesureEssaiForage
            Return MyEssaiForage.Mesures.FirstOrDefault(Function(m) m.IndiceMesure = Indice)
        End Function

        ''' <summary>
        ''' Delete une mesure par indice
        ''' </summary>
        Public Sub DeleteMesuresForage(Indice As Integer)
            Dim MesureADelete As MesureEssaiForage = GetMesuresForage(Indice)
            If MesureADelete IsNot Nothing Then
                MyEssaiForage.Mesures.Remove(MesureADelete)
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Mettre à jour une mesure
        ''' </summary>
        Public Sub UpdateMesuresForage(Indice As Integer, nouveauNom As String, nouvelleDate As String)
            Dim Mesure As MesureEssaiForage = GetMesuresForage(Indice)
            If Mesure IsNot Nothing Then
                ' Note: Adaptez selon les propriétés réelles de MesureEssaiForage
                ' Mesure. nomEssai = nouveauNom
                ' Mesure. dateEssai = nouvelleDate
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures pour un essai spécifique
        ''' </summary>
        Public Function GetMesuresByEssai(refEssai As String) As List(Of MesureEssaiForage)
            Return MyEssaiForage.Mesures.Where(Function(m) m.RefEssai = refEssai).ToList()
        End Function

        ' ===============================================================================
        ' Gestion des événements
        ' ===============================================================================

        ''' <summary>
        ''' Suspendre ou reprendre les événements
        ''' </summary>
        Public Sub SetEventsSuspendus(state As Boolean)
            _EventsSuspendus = state
        End Sub

        ''' <summary>
        ''' Vérifier si des modifications ont été apportées
        ''' </summary>
        Public Function HasBeenModified() As Boolean
            Return _HasBeenModified
        End Function
    End Class
End Namespace
