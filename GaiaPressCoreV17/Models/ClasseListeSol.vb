Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class ListeSol

        ' Instance partagée du gestionnaire de sols
        Public Shared MyListeSol As Global.GaiaPressCoreV17.Models.ListeSol

        ' Indicateur d'initialisation
        Private Shared IsInitializingSols As Boolean = False
        Public Property ListeSol As New List(Of Sol)
        Public FichierListeSol As String
        'Public MyListeSol As FrmSols
        Public EventsSuspendus As Boolean

        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de List Sols
        ' ================================================================================
        ' Add un Sol

        Public Sub AddSol(Soil As Sol)
            Me.ListeSol.Add(Soil)
            ' Lier le forage au chantier (si nécessaire)
            'Myforage.RefSol = Soil.RefSol
        End Sub

        Public Function GetSol(refSol As String) As Sol
            For Each S As Sol In Me.ListeSol
                If S.RefSol = refSol Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteSol(refSol As String)
            Dim SolADelete As Sol = GetSol(refSol)
            If SolADelete IsNot Nothing Then
                Me.ListeSol.Remove(SolADelete)
            End If
        End Sub


        ' Mettre à jour un Essai
        Public Sub MettreAJourSol(refSol As String, nouveauNom As String, nouvelleDate As String)
            Dim Sol As Sol = GetSol(refSol)
            If Sol IsNot Nothing Then
                Sol.nomSol = nouveauNom
                'Sol. dateEssai = nouvelleDate
            End If
        End Sub

        Public Shared Sub InitializeListeSol()
            If IsInitializingSols Then Return

            IsInitializingSols = True

            Try
                If MyListeSol Is Nothing Then
                    MyListeSol =
                New Global.GaiaPressCoreV17.Models.ListeSol()
                End If
            Finally
                IsInitializingSols = False
            End Try
        End Sub

        Public Sub Show()
            Throw New System.NotSupportedException(
        "L'affichage des sols doit être pris en charge par l'interface utilisateur.")
        End Sub

        Public Sub CloseListeSol()
            MyListeSol = Nothing
        End Sub

        Public Sub MaskListeSol()
            MyListeSol.CloseListeSol()
        End Sub

        Public Function GetFichierName()
            GetFichierName = FichierListeSol
        End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        'Public Function SetSol(ByRef NouvSol As Sol, ByVal Index As Integer)
        '    If Index <= SolCount Then
        '        Sols(Index) = NouvSol
        '    End If
        '    SetSol = NouvSol
        'End Function


    End Class
End Namespace