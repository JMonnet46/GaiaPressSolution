Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models

    Public Class MesureExpansion

        ' Définition de la clef primaire de la Table LesMesureExpansion
        <Key>
        Public Property Ind As Integer

        <Required>
        Public Property RefEssai As String

        <Required>
        Public Property IndiceMesure As Integer

        Public Property Pression As Single

        Public Property Volume As Single

        ' === NAVIGATION VERS L'ESSAI PARENT ===
        <ForeignKey("RefEssai")>
        Public Overridable Property EssaiExpansion As EssaiExpansion
    End Class
End Namespace