Imports Microsoft.EntityFrameworkCore

Namespace GaiaPress.core.V17.Services
    Public Class ChantiersService
        Inherits DbContext
        ' Méthode pour récupérer tous les LesChantierss
        'Public Shared Function GetAllChantiers() As List(Of LesChantiers)
        '    ' Exemple : Remplacez par votre logique métier (ex: appel à une base de données)
        '    Return New List(Of LesChantiers) From {
        '            New LesChantiers With {.Id = "1", .Nom = "Chantiers 1"},
        '            New LesChantiers With {.Id = "2", .Nom = "Chantiers 2"}
        '        }
        'End Function

        ' Méthode pour récupérer un LesChantiers par ID
        'Public Shared Function GetChantiersById(id As String) As LesChantiers
        '    ' Exemple : Remplacez par votre logique
        '    Return New LesChantiers With {.id = id, .Nom = $"Chantiers {id}"}
        'End Function

        ' ================================================================================
        ' Gestion de  C_ListForages
        ' ================================================================================
        ' Ajouter un forage
        Public Sub AjouterForage(forage As Forage)
            Me.C_ListForages.Add(forage)
            ' Lier le forage au chantier (si nécessaire)
        End Sub

        Public Function TrouverForage(refForage As String) As Forage
            For Each f As Forage In Me.C_ListForages
                If f.F_RefForage = refForage Then
                    Return f
                End If
            Next
            Return Nothing
        End Function

        ' Supprimer un forage par référence
        Public Sub SupprimerForage(refForage As String)
            Dim forageASupprimer As Forage = TrouverForage(refForage)
            If forageASupprimer IsNot Nothing Then
                Me.C_ListForages.Remove(forageASupprimer)
            End If
        End Sub

        ' Mettre à jour un forage
        Public Sub MettreAJourForage(refForage As String, nouveauNom As String, nouvelleDate As String)
            Dim forage As Forage = TrouverForage(refForage)
            If forage IsNot Nothing Then
                forage.F_NomForage = nouveauNom
                forage.F_dateForage = nouvelleDate
            End If
        End Sub


        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications du Chantier ont eu lieu
            If Not C_EventsSupendus Then
                RaiseEvent HasBeenModified()
                C_IsSaved = False
            End If
        End Sub

        ' ===============================================================================
        ' Gestion de  C_ListEssaisForages
        ' ===============================================================================
        ' Ajouter un EssaisForages
        Public Sub AjouterEssaisForages(Essai As Essai_Forage)
            Me.C_ListEssaisForages.Add(Essai)
            ' Lier l essai au chantier (si nécessaire)
            Essai.E_RefChantier = Me.C_refChantier
        End Sub

        ' Trouver un EssaisForages
        Public Function TrouverEssaisForages(refEssai As String) As Essai_Forage
            For Each e As Essai_Forage In Me.C_ListEssaisForages
                If e.E_RefEssai = refEssai Then
                    Return e
                End If
            Next
            Return Nothing
        End Function

        ' Supprimer un EssaisForages par référence
        Public Sub SupprimerEssaisForages(refEssai As String)
            Dim EssaiASupprimer As Essai_Forage = TrouverEssaisForages(refEssai)
            If EssaiASupprimer IsNot Nothing Then
                Me.C_ListEssaisForages.Remove(EssaiASupprimer)
            End If
        End Sub

        ' Mettre à jour un EssaisForages
        Public Sub MettreAJourEssaisForages(refEssai As String, nouveauNom As String, nouvelleDate As String)
            Dim Essai As Essai_Forage = TrouverEssaisForages(refEssai)
            If Essai IsNot Nothing Then
                Essai.E_nomEssai = nouveauNom
                Essai.E_dateEssai = nouvelleDate
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de  C_ListMesuresForage
        ' ===============================================================================
        ' Ajouter une MesuresForage
        Public Sub AjouterMesuresForage(Mesure As MesuresEssaiForage)
            Me.C_ListMesuresForage.Add(Mesure)
            ' Lier la mesure au chantier (si nécessaire)
            'Mesure.E_RefChantier = Me.C_refChantier
        End Sub

        ' Trouver une MesuresForage
        Public Function TrouverMesuresForage(Indice As Integer) As MesureEssaiForage
            For Each M As MesureEssaiForage In Me.C_ListMesuresForage
                If M.IndiceMesure = Indice Then
                    Return M
                End If
            Next
            Return Nothing
        End Function

        ' Supprimer une MesuresForage par référence
        Public Sub SupprimerMesuresForage(Indice As Integer)
            Dim MesureASupprimer As MesureEssaiForage = TrouverMesuresForage(Indice)
            If MesureASupprimer IsNot Nothing Then
                Me.C_ListMesuresForage.Remove(MesureASupprimer)
            End If
        End Sub

        ' Mettre à jour une MesuresForage
        Public Sub MettreAJourMesuresForage(refEssai As String, nouveauNom As String, nouvelleDate As String)
            Dim Essai As Essai_Forage = TrouverEssaisForages(refEssai)
            If Essai IsNot Nothing Then
                Essai.E_nomEssai = nouveauNom
                Essai.E_dateEssai = nouvelleDate
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de  C_ListSols
        ' ===============================================================================
        ' Ajouter un Sol
        Public Sub AjouterSol(Sol As Sol)
            Me.C_ListSols.Add(Sol)
        End Sub

        ' Trouver un Sol
        Public Function TrouverSol(sol As String) As Sol
            For Each S As Sol In Me.C_ListSols
                If S.GetNomSol = sol Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Supprimer un Sol par référence
        Public Sub SupprimerSol(sol As String)
            Dim SolASupprimer As Sol = TrouverSol(sol)
            If SolASupprimer IsNot Nothing Then
                Me.C_ListSols.Remove(SolASupprimer)
            End If
        End Sub

        ' Mettre à jour un Sol
        Public Sub MettreAJourSol(Sol As String, nouveauNom As String)
            Dim SolAModifier As Sol = TrouverSol(Sol)
            If SolAModifier IsNot Nothing Then
                SolAModifier.SetNomSol(nouveauNom)
                'SolAModifier.E_dateEssai = nouvelleDate
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de  C_ListSondes
        ' ===============================================================================
        ' Ajouter une Sonde
        Public Sub AjouterSonde(Sonde As Probe)
            Me.C_ListSondes.Add(Sonde)
        End Sub

        ' Trouver une Sonde
        Public Function TrouverSonde(RefSonde As String) As Probe
            For Each P As Probe In Me.C_ListSondes
                If P.P_refSonde = RefSonde Then
                    Return P
                End If
            Next
            Return Nothing
        End Function

        ' Supprimer une Sonde par référence
        Public Sub SupprimerSonde(refsonde As String)
            Dim SondeASupprimer As Probe = TrouverSonde(refsonde)
            If SondeASupprimer IsNot Nothing Then
                Me.C_ListSondes.Remove(SondeASupprimer)
            End If
        End Sub


        ' Mettre à jour une Sonde
        Public Sub MettreAJourSonde(refsonde As String, reEssai As String)
            Dim SondeAModifier As Probe = TrouverSonde(refsonde)
            If SondeAModifier IsNot Nothing Then
                SondeAModifier.P_refEssai = reEssai
            End If
        End Sub


    End Class
End Namespace
