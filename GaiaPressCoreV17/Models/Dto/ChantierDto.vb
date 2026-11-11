Imports GaiaPressCoreV17.Services

Namespace GaiaPressCoreV17.Models.Dto

    Public Class ChantierDto
        Public Property RefChantier As String
        Public Property NomConstructionSite As String
        Public Property DescriptifConstructionSite As String
        Public Property RefClient As String
        Public Property Entreprise As String
        Public Property Adresse As String
        Public Property Telephone As String
        Public Property Reference As String

        ' Déclaration de ClasseConstructionSite
        'Public Property  RefChantier As String
        'Public Property  NomConstructionSite As String
        'Public Property  DescriptifConstructionSite As String
        'Public Property  RefClient As Integer?
        'Public Property  Entreprise As String
        'Public Property  Adresse As String
        'Public Property  Telephone As String
        'Public Property  Reference As String

        'Public  FichierConstructionSite As String
        'Public  MyFenetreConstructionSite As ConstructionSiteLogic
        'Public  MyFile As String
        'Public  MyFileTitle As String
        'Public  DataSetName As String

        '' Collections (remplacez les tableaux par des List)
        'Public Property  ListForages As New List(Of Forage)
        'Public Property  ListEssaiForages As New List(Of Essai_Forage)
        'Public Property  ListMesuresForage As New List(Of MesureEssaiForage)
        'Public Property  ListSols As New List(Of Sol)
        'Public Property  ListSondes As New List(Of Probe)

    End Class

End Namespace