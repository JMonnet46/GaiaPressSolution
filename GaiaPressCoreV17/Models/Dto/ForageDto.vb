Namespace GaiaPressCoreV17.Models.Dto

    Public Class ForagesDto
        Public Property RefForage As String
        Public Property RefChantier As String
        Public Property NomForage As String
        Public Property numeroForage As Integer
        Public Property DateForage As DateTime
        Public Property materiel As String
        Public Property outil As String
        Public Property abscisseX As Single
        Public Property ordonneeY As Single
        Public Property altitudeZ As Single
        Public Property coteInit As Single
        Public Property coteFinale As Single

        ' Déclaration de ClassForage
        'Public Property  ListEssais As New List(Of Essai_Forage)

        '' les proprietes
        'Public  RefChantier As String
        'Public  RefForage As String
        'Public  numeroForage As Integer
        'Public  NomForage As String
        'Public  dateForage As Date
        'Public  materiel As String
        'Public  outil As String
        'Public  abscisseX As Single
        'Public  ordonneeY As Single
        'Public  altitudeZ As Single
        'Public  coteInit As Single
        'Public  coteFinale As Single
        'Public  EventsSuspendus As Boolean
        'Public  IsSaved As Boolean
        'Public Event  HasBeenModified()
        'Public  FichierChantier As String

    End Class

End Namespace
