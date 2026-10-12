Imports System.ComponentModel.DataAnnotations

Namespace GaiaPressCoreV17.Models.Dto

    Public Class MesureResistanceDto
        <Key>
        Public Property Ind As Integer
        Public Property RefEssai As String
        Public Property IndiceMesure As Integer


        ' Déclaration de ClasseMesureResistance

        Public Property Pression As Single

        Public Property Volume As Single

    End Class

End Namespace
