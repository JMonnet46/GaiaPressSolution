Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models

    <Table("LesForagesDb", Schema:="dbo")>
    Public Class Forage

        ' Clé primaire
        <Key>
        Public Property RefForage As String

        ' Clé étrangère vers le chantier (mappée par OnModelCreating)
        Public Property RefChantier As String

        ' Navigation vers le chantier parent
        Public Property ChantierAssocie As Chantier

        ' Liste des essais rattachés à ce forage
        Public Property ListEssais As List(Of Essai_Forage) = New List(Of Essai_Forage)()

        ' Propriétés du forage
        Public Property numeroForage As Integer
        Public Property NomForage As String
        Public Property dateForage As Date
        Public Property materiel As String
        Public Property outil As String
        Public Property abscisseX As Single
        Public Property ordonneeY As Single
        Public Property altitudeZ As Single
        Public Property coteInit As Single
        Public Property coteFinale As Single

        ' Propriétés non mappées en base
        <NotMapped>
        Public Property EventsSuspendus As Boolean

        <NotMapped>
        Public Property IsSaved As Boolean

        <NotMapped>
        Public Property FichierChantier As String

        Public Sub New()
        End Sub

    End Class

End Namespace