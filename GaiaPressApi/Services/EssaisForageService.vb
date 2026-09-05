Imports System.Threading

Namespace GaiaPress.Core.V17.Services

    Public Class EssaisForageService


        ' ✅ Utiliser une instance de ClasseChantier comme source de vérité
        'Public _chantier As LesChantiers

        ' ✅ Déclarer les membres manquants (si nécessaire pour la logique métier)
        Public _dbContext As GaiaPressDbContext
        Public _c_EventsSuspendus As Boolean = False
        Public _HasBeenModified As Boolean = False

        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub


        ' ===============================================================================
        ' Gestion de C_ListEssaisForage
        ' ===============================================================================

        ''' <summary>
        ''' Add un essai forage au chantier
        ''' </summary>
        Public Async Function AddEssaisForage(essai As Essai_Forage) As Task(Of Essai_Forage)
            If essai Is Nothing Then
                Throw New ArgumentNullException(NameOf(essai), "L'essai de forage ne peut pas être vide.")
            End If

            _dbContext.EssaisForage.Add(essai)
            Await _dbContext.SaveChangesAsync()
            Return essai
        End Function

        ''' <summary>
        ''' Récupère tous les essais de forage.
        ''' </summary>
        ''' <returns>Liste de tous les essais de forage.</returns>
        Public Async Function GetAllEssaisForage() As Task(Of List(Of Essai_Forage))
            Return Await _dbContext.EssaisForage.ToListAsync()
        End Function

        ''' <summary>
        ''' Get un essai forage par référence
        ''' </summary>
        Public Function GetEssaisForage(refEssai As String) As Essai_Forage
            ' ✅ Utiliser la liste du chantier (C_ListEssaisForage)
            Return MyForage.F_ListEssais.FirstOrDefault(Function(e) e.E_RefEssai = refEssai)
        End Function

        ''' <summary>
        ''' Delete un essai forage par référence
        ''' </summary>
        Public Async Function DeleteEssaiForage(refEssai As String) As Task(Of Boolean)
            If String.IsNullOrEmpty(refEssai) Then
                Throw New ArgumentException("La référence de l'essai ne peut pas être vide.", NameOf(refEssai))
            End If

            Dim essaiToDelete = Await _dbContext.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai, CancellationToken.None)

            If essaiToDelete Is Nothing Then
                Return False
            End If

            _dbContext.EssaisForage.Remove(essaiToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Mettre à jour un essai forage
        ''' </summary>
        Public Sub UpDateEssaisForage(nouvelEssai As Essai_Forage)
            Dim Essai As Essai_Forage = GetEssaisForage(nouvelEssai.E_RefEssai)
            If Essai IsNot Nothing Then
                Essai.E_RefChantier = nouvelEssai.E_RefChantier
                Essai.E_RefForage = nouvelEssai.E_RefForage
                Essai.E_RefEssai = nouvelEssai.E_RefEssai
                Essai.E_refSonde = nouvelEssai.E_refSonde
                Essai.E_refEssaiResistancePropre = nouvelEssai.E_refEssaiResistancePropre
                Essai.E_refEssaiExpansionPropre = nouvelEssai.E_refEssaiExpansionPropre
                Essai.E_refEssaiPerteDeCharge = nouvelEssai.E_refEssaiPerteDeCharge
                Essai.E_RefSol = nouvelEssai.E_RefSol
                Essai.E_Enregistreur = nouvelEssai.E_Enregistreur
                Essai.E_Operateur = nouvelEssai.E_Operateur
                Essai.E_VolSonde = nouvelEssai.E_VolSonde
                Essai.E_ClassPrecis = nouvelEssai.E_ClassPrecis
                Essai.E_PrecisVolSonde = nouvelEssai.E_PrecisVolSonde
                Essai.E_nomEssai = nouvelEssai.E_nomEssai
                Essai.E_dateEssai = nouvelEssai.E_dateEssai
                Essai.E_heureEssai = nouvelEssai.E_heureEssai
                Essai.E_difPresEauAir = nouvelEssai.E_difPresEauAir
                Essai.E_hauteurCpv = nouvelEssai.E_hauteurCpv
                Essai.E_presP0 = nouvelEssai.E_presP0
                Essai.E_presFluage = nouvelEssai.E_presFluage
                Essai.E_presLimite = nouvelEssai.E_presLimite
                Essai.E_presLimiteNF = nouvelEssai.E_presLimiteNF
                Essai.E_presLimiteTheo = nouvelEssai.E_presLimiteTheo
                Essai.E_PrecisVolume = nouvelEssai.E_PrecisVolume
                Essai.E_PrecisPression = nouvelEssai.E_PrecisPression
                Essai.E_poidsVol = nouvelEssai.E_poidsVol
                Essai.E_poidsVolDejauge = nouvelEssai.E_poidsVolDejauge
                Essai.E_coefPoisson = nouvelEssai.E_coefPoisson
                Essai.E_modPressio = nouvelEssai.E_modPressio
                Essai.E_modPressioNf = nouvelEssai.E_modPressioNf
                Essai.E_modElastique = nouvelEssai.E_modElastique
                Essai.E_modElastiqueNf = nouvelEssai.E_modElastiqueNf
                Essai.E_Cohesion = nouvelEssai.E_Cohesion
                Essai.E_Frottement = nouvelEssai.E_Frottement
                Essai.E_FrottementIntergranulaire = nouvelEssai.E_FrottementIntergranulaire
                Essai.E_U0 = nouvelEssai.E_U0
                Essai.E_profNappe = nouvelEssai.E_profNappe
                Essai.E_profSonde = nouvelEssai.E_profSonde
                Essai.E_coteZeroGeomeca = nouvelEssai.E_coteZeroGeomeca
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère les essais de forage par référence de forage.
        ''' </summary>
        ''' <param name="refForage">Référence du forage.</param>
        ''' <returns>Liste des essais de forage pour le forage spécifié.</returns>
        Public Async Function GetEssaisForageByRefForage(refForage As String) As Task(Of List(Of Essai_Forage))
            Return Await _dbContext.EssaisForage.Where(Function(e) e.E_RefForage = refForage).ToListAsync()
        End Function


        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        Public Async Function GetEssaiForageByRefEssai(refEssai As String) As Task(Of Essai_Forage)
            Return Await _dbContext.EssaisForage.FirstOrDefaultAsync(Function(e) e.E_RefEssai = refEssai)
        End Function

        ' ===============================================================================
        ' Gestion des Mesures associées aux Essais de Forage
        ' ===============================================================================

        ''' <summary>
        ''' Add une mesure à un essai de forage
        ''' </summary>
        Public Sub AddMesureToEssai(refEssai As String, mesure As MesuresEssaiForage)
            Dim essai = GetEssaisForage(refEssai)
            If essai IsNot Nothing Then
                MyEssai.E_Mesures.Add(mesure)
                mesure.RefEssai = refEssai
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Get une mesure par indice
        ''' </summary>
        Public Function GetMesuresForage(Indice As Integer) As MesuresEssaiForage
            Return MyEssai.E_Mesures.FirstOrDefault(Function(m) m.IndiceMesure = Indice)
        End Function

        ''' <summary>
        ''' Delete une mesure par indice
        ''' </summary>
        Public Sub DeleteMesuresForage(Indice As Integer)
            Dim MesureADelete As MesuresEssaiForage = GetMesuresForage(Indice)
            If MesureADelete IsNot Nothing Then
                MyEssai.E_Mesures.Remove(MesureADelete)
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Mettre à jour une mesure
        ''' </summary>
        Public Sub UpdateMesuresForage(Indice As Integer, nouveauNom As String, nouvelleDate As String)
            Dim Mesure As MesuresEssaiForage = GetMesuresForage(Indice)
            If Mesure IsNot Nothing Then
                ' Note: Adaptez selon les propriétés réelles de MesuresEssaiForage
                ' Mesure.E_nomEssai = nouveauNom
                ' Mesure.E_dateEssai = nouvelleDate
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Récupère toutes les mesures pour un essai spécifique
        ''' </summary>
        Public Function GetMesuresByEssai(refEssai As String) As List(Of MesuresEssaiForage)
            Return MyEssai.E_Mesures.Where(Function(m) m.RefEssai = refEssai).ToList()
        End Function        ' ===============================================================================
        ' Gestion des événements
        ' ===============================================================================

        ''' <summary>
        ''' Suspendre ou reprendre les événements
        ''' </summary>
        Public Sub SetEventsSuspendus(state As Boolean)
            _c_EventsSuspendus = state
        End Sub

        ''' <summary>
        ''' Vérifier si des modifications ont été apportées
        ''' </summary>
        Public Function HasBeenModified() As Boolean
            Return _HasBeenModified
        End Function
    End Class
End Namespace
