Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic

Public Class ClasseListeConstructionSite
    Public Property ListeConstructionSiteSession As New List(Of ConstructionSite)

    ' =========================================================
    ' Constructeur
    '==========================================================
    Public Sub New()
        '_items = New List(Of ConstructionSite)
        ' ✅ Correction : Suppression de la référence à _items
        ' Initialisation déjà faite par la déclaration de la propriété
    End Sub

    Public Function IsRefChantierUnique(ByVal ref As String) As Boolean
        ' Vérification de l'unicité d'une référence
        ' Donne une référence unique au nouveau forage
        Dim i As Integer
        Dim unique As Boolean

        unique = True

        ' i est le Numéro du Forage à vérifier
        Do While (i < ListeConstructionSiteSession.Count) And unique
            unique = (ListeConstructionSiteSession(i).RefChantier <> ref)
            If unique Then Exit Do
            i = i + 1
        Loop

        If MyConstructionSite Is Nothing Then
            MyConstructionSite = New ConstructionSite
        End If

        If unique Then MyConstructionSite = ListeConstructionSiteSession(i)

        IsRefChantierUnique = unique
    End Function


End Class
