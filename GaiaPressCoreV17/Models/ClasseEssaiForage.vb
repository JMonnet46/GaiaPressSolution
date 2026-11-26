Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Collections.Generic

Namespace GaiaPressCoreV17.Models
    Public Class Essai_Forage
        ' Définition de la clef primaire de la Table LesEssaiForage
        <Key>
        Public Property RefEssai As String
        ' pour ignorer le mappage sur RefChantier
        Public Property RefChantier As String          ' colonne SQL, valeur de la clé
        <ForeignKey("RefChantier")>
        Public Overridable Property ChantierAssocie As Chantier   ' objet lié
        Public Property RefForage As String
        ' Clé étrangère : mappée sur la colonne SQL RefForage
        <ForeignKey("RefForage")>
        Public Overridable Property ForageAssocie As Forage
        Public Property RefSonde As String
        <ForeignKey("RefSonde")>
        Public Overridable Property MySonde As Probe
        Public Property MaResistance As EssaiResistance
        Public Property MaExpansion As EssaiExpansion
        'Public Property MonEssai As Essai_Forage
        Public Property RefSol As String
        <ForeignKey("RefSol")>
        Public Overridable Property MySol As Sol
        Public Property refEssaiResistancePropre As String
        Public Property refEssaiExpansionPropre As String
        Public Property refEssaiPerteDeCharge As String
        Public Property nomEssai As String
        Public Property dateEssai As String
        Public Property heureEssai As String
        Public Property Enregistreur As String
        Public Property Operateur As String
        Public Property VolSonde As Single
        Public Property ClassPrecis As Integer
        Public Property PrecisVolSonde As Single
        Public Property difPresEauAir As Single
        Public Property hauteurCpv As Single
        Public Property presP0 As Single
        Public Property presFluage As Single
        Public Property presLimite As Single
        Public Property presLimiteNF As Single
        Public Property presLimiteTheo As Single
        Public Property PrecisVolume As Single
        Public Property PrecisPression As Single
        Public Property poidsVol As Single
        Public Property poidsVolDejauge As Single
        Public Property coefPoisson As Single
        Public Property modPressio As Single
        Public Property modPressioNf As Single
        Public Property modElastique As Single
        Public Property modElastiqueNf As Single
        Public Property Cohesion As Single
        Public Property Frottement As Single
        Public Property FrottementIntergranulaire As Single
        Public Property U0 As Single
        Public Property profNappe As Single
        Public Property profSonde As Single
        Public Property coteZeroGeomeca As Single

        ' ======= Propriétés de Liste ===========
        Public Property Mesures As List(Of MesureEssaiForage)

        ' ======= Propriété de modification ===========
        Public Property EventsSuspendus As Boolean

        '========= Propriétés supplémentaires =======================
        Public Property Pe As Single
        Public Property Ie As Integer


        Public IsSaved As Boolean
        Public Event HasBeenModified()

        Public Sub New()
            Mesures = New List(Of MesureEssaiForage)()
            refEssaiResistancePropre = String.Empty
            refEssaiExpansionPropre = String.Empty
            refEssaiPerteDeCharge = Nothing
            nomEssai = String.Empty
            dateEssai = Date.Today.ToString("yyyy-MM-dd")
            heureEssai = DateTime.Now.ToString("HH:mm:ss")
            Enregistreur = String.Empty
            Operateur = String.Empty
            IsSaved = False
        End Sub

        ' Accesseurs limités aux données locales de cette instance.
        Public Function GetMesureCount() As Integer
            If Mesures Is Nothing Then Return 0
            Return Mesures.Count
        End Function

        Public Function GetRefSonde() As String
            Return RefSonde
        End Function

        Public Function GetVs() As Single
            Return VolSonde
        End Function

        Public Function GetClassPrecis() As Integer
            Return ClassPrecis
        End Function

        Public Function GetIsSaved() As Boolean
            Return IsSaved
        End Function

        Public Sub SetRefSonde(ByVal refSond As String)
            RefSonde = refSond
            IHaveBeenModified()
        End Sub

        Public Sub SetVs(ByVal volume As Single)
            VolSonde = volume
            IHaveBeenModified()
        End Sub

        Public Sub NouvelleMesure(ByVal nouvelleSerie As Boolean, ByVal indice As Integer, ByVal indiceMesure As Integer, ByVal pression As Single, ByVal volume15 As Single, ByVal volume30 As Single, ByVal volume60 As Single, ByVal contrainteRadiale As Single, ByVal contrainteVerticale As Single, ByVal deformation As Single, ByVal pnf As Single, ByVal vnf As Single, ByVal qi As Single, ByVal pi As Single, ByVal qp As Single, ByVal pp As Single)
            ' nouvelleSerie est conservé pour compatibilité avec l'appelant historique.
            If Mesures Is Nothing Then Mesures = New List(Of MesureEssaiForage)()

            Dim mesure As New MesureEssaiForage(RefEssai, indiceMesure)
            mesure.Ind = indice
            mesure.Pression = pression
            mesure.Volume15 = volume15
            mesure.Volume30 = volume30
            mesure.Volume60 = volume60
            mesure.ContrainteRadiale = contrainteRadiale
            mesure.ContrainteVerticale = contrainteVerticale
            mesure.Deformation = deformation
            mesure.PNF = pnf
            mesure.VNF = vnf
            mesure.Qi = qi
            mesure.Pi = pi
            mesure.Qp = qp
            mesure.Pp = pp
            mesure.Essai_Forage = Me
            Mesures.Add(mesure)
            IHaveBeenModified()
        End Sub

        Public Sub UpdateMesureEssaiForage(ByVal indice As Integer, ByVal mesure As MesureEssaiForage)
            If Mesures Is Nothing Then Mesures = New List(Of MesureEssaiForage)()
            If indice >= 0 AndAlso indice < Mesures.Count Then
                Mesures(indice) = mesure
            Else
                Mesures.Add(mesure)
            End If
        End Sub

        Public Sub IHaveBeenModified()
            If Not EventsSuspendus Then
                IsSaved = False
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub SuspendreEvents()
            EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            EventsSuspendus = False
        End Sub
    End Class
End Namespace
