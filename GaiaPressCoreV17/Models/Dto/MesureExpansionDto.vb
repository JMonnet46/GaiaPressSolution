Imports System.ComponentModel.DataAnnotations

Namespace GaiaPressCoreV17.Models.Dto

    Public Class MesureExpansionDto
        ' Définition de la clef primaire de la Table LesMesureExpansion
        <Key>
        Public Property Ind As Integer

        <Required>
        Public Property RefEssai As String

        <Required>
        Public Property IndiceMesure As Integer

        'Déclaration de ClasseMesureExpansion
        ' Définition de la clef primaire de la Table LesMesureExpansion

        Public Property Pression As Single

        Public Property Volume As Single

    End Class

End Namespace
