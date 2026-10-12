Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Services

Namespace GaiaPressCoreV17.Models

    Public Class ListeEssaiExpansion

        ' ======= Propriétés de Liste ===========
        Public Property ListeExpansion As New List(Of EssaiExpansion)

        Public FichierListeExpansion As String
        'Public MyListeExpansion As FrmExpansion
        Public EventsSuspendus As Boolean

        Public IsSaved As Boolean

        ' ✅ Liste interne
        Private _items As List(Of EssaiExpansion)

        Public Sub New()
            _items = New List(Of EssaiExpansion)
        End Sub

        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de List Expansion
        ' ================================================================================
        ' Add un Sol

        Public Sub AddExpansion(EssaiExpansion As EssaiExpansion)
            Me.ListeExpansion.Add(EssaiExpansion)
            ' Lier le forage au chantier (si nécessaire)
            'EssaiExpansion.SetRefEssaiExpansion(EssaiExpansion.GetRefEssai)
        End Sub

        Public Function GetEssaiExpansionParRefEssai(refEssai As String) As EssaiExpansion
            For Each E As EssaiExpansion In Me.ListeExpansion
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        Public Function GetEssaiExpansionParRefSonde(refSonde As String, rang As Integer) As EssaiExpansion
            Dim i As Integer
            For Each E As EssaiExpansion In Me.ListeExpansion
                If E.GetRefSonde = refSonde And i >= rang Then
                    Return E
                End If
                i = i + 1
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssaiExpansion(refEssai As String)
            Dim EssaiADelete As EssaiExpansion = GetEssaiExpansionParRefEssai(refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.ListeExpansion.Remove(EssaiADelete)
            End If
        End Sub


        ' Mettre à jour un Essai
        Public Sub MettreAJourEssaiExpansion(refEssai As String, nouvelEssai As EssaiExpansion)
            Dim EssaiExpansion As EssaiExpansion = GetEssaiExpansionParRefEssai(refEssai)
            If EssaiExpansion IsNot Nothing Then
                EssaiExpansion = nouvelEssai
                'Sol. dateEssai = nouvelleDate
            End If
        End Sub

        Public Sub CloseListeExpansion()

            ' fermeture de la Liste des Sondes (fermeture de sa fenêtre)
            'Call MyListeSonde.CloseChantier()

            ' Extinction des menus liés au chantier
            'Call ActivationMenus(False)

            ' Effacement de la référence vers le chantier
            MyListeEssaiExpansionSession = Nothing

            ' Nettoyage de l'arborescence
            'TreeViewArbo.Nodes.Clear()
        End Sub

        Public Sub Show()
            MyListeSonde.Show()
        End Sub


        Public Function GetFichierName()
            GetFichierName = FichierListeExpansion
        End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

    End Class
End Namespace