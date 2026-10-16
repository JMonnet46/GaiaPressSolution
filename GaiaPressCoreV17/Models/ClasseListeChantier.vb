Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic

Public Class ClasseListeChantier
    Public Property ListeChantierSession As New List(Of Chantier)

    ' =========================================================
    ' Constructeur
    '==========================================================
    Public Sub New()
        '_items = New List(Of Chantier)
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
        Do While (i < ListeChantierSession.Count) And unique
            unique = (ListeChantierSession(i).refChantier <> ref)
            If unique Then Exit Do
            i = i + 1
        Loop

        If Mychantier Is Nothing Then
            Mychantier = New Chantier
        End If

        If unique Then MyChantier = ListeChantierSession(i)

        IsRefChantierUnique = unique
    End Function


End Class
