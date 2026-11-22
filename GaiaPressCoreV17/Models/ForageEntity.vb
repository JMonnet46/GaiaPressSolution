Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models

    <Table("LesForages")>
    Public Class ForageEntity

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.None)>
        Public Property refForage As String = String.Empty

        Public Property refChantier As String
        Public Property numeroForage As Integer
        Public Property nomForage As String
        Public Property dateForage As DateTime
        Public Property materiel As String
        Public Property outil As String

        Public Property abscisseX As Single
        Public Property ordonneeY As Single
        Public Property altitudeZ As Single
        Public Property coteInit As Single
        Public Property coteFinale As Single

        Public Property EventsSuspendus As Boolean
        Public Property IsSaved As Boolean

        Public Property FichierChantier As String

    End Class

End Namespace