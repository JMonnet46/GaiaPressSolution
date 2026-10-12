
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models.Dto
    Public Class EssaiForageDto

        Public Property RefEssai As String
        Public Property RefForage As String
        Public Property RefChantier As String
        Public Property RefSol As String
        Public Property RefSonde As String
        Public Property refEssaiResistancePropre As String
        Public Property refEssaiExpansionPropre As String
        Public Property refEssaiPerteDeCharge As String
        Public Property NomEssai As String
        Public Property dateEssai As Date

        ' ← RIEN DE PLUS !***
        ' Déclaration de ClasseessaiForage

        'Public Property  RefChantier As String
        'Public Property  RefForage As String

        '<ForeignKey(" RefSol")>
        'Public Overridable Property  MySol As Sol

        '<ForeignKey(" refSonde")>
        'Public Overridable Property  MySonde As Probe

        'Public Property MaResistance As EssaiResistance

        'Public Property MaExpansion As EssaiExpansion

        'Public Property MonEssai As Essai_Forage
        'Public Property  refSonde As String
        'Public Property  refEssai As String
        'Public Property  refEssaiResistancePropre As String
        'Public Property  refEssaiExpansionPropre As String
        'Public Property  refEssaiPerteDeCharge As String
        'Public Property  RefSol As String
        'Public Property  nomEssai As String
        'Public Property  dateEssai As String
        'Public Property  heureEssai As String
        'Public Property  Enregistreur As String
        'Public Property  Operateur As String
        'Public Property  VolSonde As Single
        'Public Property  ClassPrecis As Integer
        'Public Property  PrecisVolSonde As Single
        'Public Property  difPresEauAir As Single
        'Public Property  hauteurCpv As Single
        'Public Property  presP0 As Single
        'Public Property  presFluage As Single
        'Public Property  presLimite As Single
        'Public Property  presLimiteNF As Single
        'Public Property  presLimiteTheo As Single
        'Public Property  PrecisVolume As Single
        'Public Property  PrecisPression As Single
        'Public Property  poidsVol As Single
        'Public Property  poidsVolDejauge As Single
        'Public Property  coefPoisson As Single
        'Public Property  modPressio As Single
        'Public Property  modPressioNf As Single
        'Public Property  modElastique As Single
        'Public Property  modElastiqueNf As Single
        'Public Property  Cohesion As Single
        'Public Property  Frottement As Single
        'Public Property  FrottementIntergranulaire
        'Public Property  U0 As Single
        'Public Property  profNappe As Single
        'Public Property  profSonde As Single
        'Public Property  coteZeroGeomeca As Single


        ' Déclaration dans ClasseEssaiForage
        'Public Property  RefChantier As String
        'Public Property  RefForage As String

        '<ForeignKey(" RefSol")>
        'Public Overridable Property  MySol As Sol

        '<ForeignKey(" refSonde")>
        'Public Overridable Property  MySonde As Probe

        'Public Property MaResistance As EssaiResistance

        'Public Property MaExpansion As EssaiExpansion

        'Public Property MonEssai As Essai_Forage
        'Public Property  refSonde As String
        'Public Property  refEssai As String
        'Public Property  refEssaiResistancePropre As String
        'Public Property  refEssaiExpansionPropre As String
        'Public Property  refEssaiPerteDeCharge As String
        'Public Property  RefSol As String
        'Public Property  nomEssai As String
        'Public Property  dateEssai As String
        'Public Property  heureEssai As String
        'Public Property  Enregistreur As String
        'Public Property  Operateur As String
        'Public Property  VolSonde As Single
        'Public Property  ClassPrecis As Integer
        'Public Property  PrecisVolSonde As Single
        'Public Property  difPresEauAir As Single
        'Public Property  hauteurCpv As Single
        'Public Property  presP0 As Single
        'Public Property  presFluage As Single
        'Public Property  presLimite As Single
        'Public Property  presLimiteNF As Single
        'Public Property  presLimiteTheo As Single
        'Public Property  PrecisVolume As Single
        'Public Property  PrecisPression As Single
        'Public Property  poidsVol As Single
        'Public Property  poidsVolDejauge As Single
        'Public Property  coefPoisson As Single
        'Public Property  modPressio As Single
        'Public Property  modPressioNf As Single
        'Public Property  modElastique As Single
        'Public Property  modElastiqueNf As Single
        'Public Property  Cohesion As Single
        'Public Property  Frottement As Single
        'Public Property  FrottementIntergranulaire
        'Public Property  U0 As Single
        'Public Property  profNappe As Single
        'Public Property  profSonde As Single
        'Public Property  coteZeroGeomeca As Single


    End Class

End Namespace
