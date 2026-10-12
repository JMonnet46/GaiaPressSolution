Imports System.ComponentModel

Namespace GaiaPressCoreV17.Models.Dto

    ''' <summary>
    ''' DTO pour les variables de précision dans les calculs élastiques
    ''' </summary>
    Public Class PrecisionElastDto

        ''' <summary>
        ''' Précision maximale pour la pression (P)
        ''' </summary>
        <Description("Précision maximale pour la pression (P)")>
        Public Property PMax As Single

        <Description("Précision minimale pour la pression (P)")>
        Public Property PMin As Single

        <Description("Précision maximale pour le volume Max (D)")>
        Public Property VMax As Single

        <Description("Précision minimale pour le volume Min (D)")>
        Public Property VMin As Single

        <Description("??")>
        Public Property DEm_Vs As Single

        <Description("Module Ménard par rapport à V")>
        Public Property DEm_V As Single

        <Description("??")>
        Public Property DEm_P As Single

        <Description("Volume Limite")>
        Public Property VL As Single

        <Description("Précision sur Volume Limite")>
        Public Property PrVl As Single

    End Class

End Namespace