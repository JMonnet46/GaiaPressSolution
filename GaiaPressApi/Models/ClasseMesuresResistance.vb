Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPress.Core.V17.Models

    Public Class MesuresResistance

        ' Définition de la clef primaire de la Table LesMesuresResistance
        <Key>
        Public Property Id As Integer

        <Required>
        Public Property RefEssai As String

        <Required>
        Public Property IndiceMesure As Integer

        Public Property Pression As Single

        Public Property Volume15 As Single

        Public Property Volume30 As Single

        Public Property Volume60 As Single

        ' === NAVIGATION VERS L'ESSAI PARENT ===
        <ForeignKey("RefEssai")>
        Public Overridable Property Essai_Resistance As Essai_Resistance
    End Class
End Namespace