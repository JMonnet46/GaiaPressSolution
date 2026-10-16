Imports System.Collections.Generic
Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports Microsoft.Build.Evaluation
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class ClasseListeEssaiForage

        ' ======= Propriétés de Liste ===========
        ' ⚠️ Collection en mémoire pour manipulation temporaire
        Public Property ListeEssaiForageSession As List(Of Essai_Forage)

        ' ⚠️ Méthode pour charger les essais depuis la base de données
        Public Function LoadEssaiForage(context As GaiaPressDbContext) As List(Of Essai_Forage)
            ListeEssaiForageSession = context.ListeEssaiForageDb.ToList()
            Return ListeEssaiForageSession
        End Function

        ' ⚠️ Ajoutez cette ligne :
        'Public Property ListeEssaiForageDb As DbSet(Of Essai_Forage)

        Public FichierListeExpansion As String
        'Public MyListeExpansion As FrmExpansion
        Public EventsSuspendus As Boolean

        Public IsSaved As Boolean

        Public Sub New()

        End Sub

        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de ListeEssaiForageSession
        ' ================================================================================

        ' ⚠️ Méthode pour ajouter un essai (via le DbContext)
        Public Function AddEssai(context As GaiaPressDbContext, essai As Essai_Forage) As Boolean
            Try
                context.ListeEssaiForageDb.Add(essai)
                context.SaveChanges()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ' ⚠️ Méthode pour trouver un essai par RefEssai
        Public Function GetEssaiForageByRef(context As GaiaPressDbContext, refEssai As String) As Essai_Forage
            Return context.ListeEssaiForageDb.FirstOrDefault(Function(e) e.refEssai = refEssai)
        End Function

        Public Function GetEssaiCount() As Integer
            GetEssaiCount = ListeEssaiForageSession.Count
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssaiForage(context As GaiaPressDbContext, refEssai As String)
            Dim EssaiADelete As Essai_Forage = GetEssaiForageByRef(context, refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.ListeEssaiForageSession.Remove(EssaiADelete)
            End If
        End Sub

        ' ⚠️ Méthode pour modifier un essai
        Public Function ModifyEssaiForage(context As GaiaPressDbContext, essai As Essai_Forage) As Boolean
            Try
                context.ListeEssaiForageDb.Update(essai)
                context.SaveChanges()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ' ================================================================
        ' Suite du listing
        '=================================================================

        '' Mettre à jour un Essai
        'Public Sub MettreAJourEssaiForage(refEssai As String, nouvelEssai As Essai_Forage)
        '    Dim EssaiForage As Essai_Forage = GetEssaiForage(refEssai)
        '    If EssaiForage IsNot Nothing Then
        '        EssaiForage = nouvelEssai
        '        'Sol. dateEssai = nouvelleDate
        '    End If
        'End Sub

        ' ******************************************************************************************************************
        ' Chargement dans Resistance ou Expansion des essais de Probe
        ' Recherche d'un EssaiResistance ou EssaiExpansion dans ListeExpansion ou ListeResistance par RefSonde
        ' Construction de la Liste des Resistance ou de la Liste des Expansions d'une Sonde
        ' ******************************************************************************************************************
        'Public Sub SetListeResistanceExpansionParRefSonde(ByVal RefSonde As String, ByVal typeEssai As Integer)
        '    Dim i As Integer = 0

        '    ' Vérification de la sonde
        '    If MySonde Is Nothing Then
        '        MySonde = New Probe() ' Ou votre classe Probe
        '    End If

        '    ' Initialisation des listes
        '    Select Case typeEssai
        '        Case EssaiCalib.Resistance
        '            ' Initialiser la liste des résistances
        '            MySonde.Resistance = New List(Of EssaiResistance)()

        '            ' Parcourir la liste des essais de résistance
        '            For Each essai In MyListeLesEssaiForage
        '                If essai.GetType() Is GetType(EssaiResistance) AndAlso
        '                   essai.RefSonde = RefSonde Then

        '                    MySonde.Resistance.Add(CType(essai, EssaiResistance))
        '                    i += 1
        '                End If
        '            Next

        '        Case EssaiCalib.Expansion
        '            ' Initialiser la liste des expansions
        '            MySonde.Expansion = New List(Of EssaiExpansion)()

        '            ' Parcourir la liste des essais d'expansion
        '            For Each essai In MyListeLesEssaiForage
        '                If essai.GetType() Is GetType(EssaiExpansion) AndAlso
        '                   essai.RefSonde = RefSonde Then

        '                    MySonde.Expansion.Add(CType(essai, EssaiExpansion))
        '                    i += 1
        '                End If
        '            Next
        '    End Select
        'End Sub
        'Public Sub SetListeResistanceExpansionParRefSonde(ByVal RefSonde As String, ByVal typeEssai As Integer)
        '    If MySonde Is Nothing Then
        '        MySonde = New Probe()
        '    End If

        '    Select Case typeEssai
        '        Case EssaiCalib.Resistance
        '            Dim Essai_Resistance As EssaiResistance
        '            MySonde.Resistance = New List(Of EssaiResistance)()
        '            For Each Essai_Resistance In ModBibliotheque.MyListeMesureEssaiResistanceSession
        '                If TypeOf Essai_Resistance Is EssaiResistance AndAlso Essai_Resistance.RefSonde = RefSonde Then
        '                    MySonde.Resistance.Add(Essai_Resistance)
        '                End If
        '            Next

        '        Case EssaiCalib.Expansion
        '            Dim Essai_Expansion As EssaiExpansion
        '            MySonde.Expansion = New List(Of EssaiExpansion)()
        '            For Each Essai_Expansion In ModBibliotheque.MyListeEssaiExpansionSession
        '                If TypeOf Essai_Expansion Is EssaiExpansion AndAlso Essai_Expansion.RefSonde = RefSonde Then
        '                    MySonde.Expansion.Add(Essai_Expansion)
        '                End If
        '            Next
        '    End Select
        'End Sub

        '         'Public Shared Sub InitializeListeExpansion()
        '    If Not MyListeEssaiExpansion.IsSaved Then
        '        ListeExpansion.IsSaved = True
        '        If MyListeExpansion Is Nothing Then
        '            MyListeExpansion = New ListeExpansion
        '        End If
        '        IsInitializingExpansion = False
        '    End If
        'End Sub

        'Public Sub New()

        '    ' Création de la feuille de liste
        '    'If MyListeExpansion Is Nothing Then
        '    '    InitializeListeExpansion()
        '    'End If

        '    '' Enregistrement de la référence de la liste Sol auprès de sa feuille
        '    'Call MyListeExpansion.GiveMeANewRefSol()

        '    ' -----------------------------------------------------------------
        '    Dim Index As Integer

        '    ' Création de la feuille de liste
        '    If MyListeExpansion Is Nothing Then
        '        Index = 0
        '        MyListeExpansion = Me
        '        ' Initialize other properties or perform other setup tasks here
        '    Else
        '        If MySol Is Nothing Then
        '            MyListeExpansion = Nothing
        '            Exit Sub
        '        End If
        '        Index = GetIndexSol(MySol.GetNomSol)
        '        If Index >= 0 Then Call MyListeExpansion.SetSol(MySol, Index)
        '    End If

        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    ' Call MyListeSonde.SetSonde(MySonde, Index)
        '    ' -----------------------------------------------------

        'End Sub


        Public Sub CloseListeEssaiForage()

            ' fermeture de la Liste des Sondes (fermeture de sa fenêtre)
            'Call MyListeSonde.CloseChantier()

            ' Extinction des menus liés au chantier
            'Call ActivationMenus(False)

            ' Effacement de la référence vers le chantier
            ListeEssaiForageSession = Nothing

            ' Nettoyage de l'arborescence
            'TreeViewArbo.Nodes.Clear()
        End Sub

        ' **********************************************************************
        ' Fonction d'écriture dans le fichier Access des Expansion
        ' **********************************************************************

        Public Function IsRefEssaiUnique(ByVal ref As String) As Boolean
            For Each essai In ListeEssaiForageSession
                If essai.refEssai = ref Then
                    Return False
                End If
            Next

            Return True
        End Function

        Public Function IsNomEssaiUnique(ByVal Nom As String) As Boolean
            ' Vérification de l'unicité d'une référence
            Dim i As Integer
            Dim unique As Boolean

            unique = True

            Do While (i < ListeEssaiForageSession.Count) And unique
                unique = (ListeEssaiForageSession(i).nomEssai <> Nom)
                i = i + 1
            Loop

            IsNomEssaiUnique = unique
        End Function

        'Public Function GetSolCount()
        '    GetSolCount = SolCount
        'End Function

        'Public Function GetSolParIndex(ByVal Index As Integer) As Sol
        '    GetSolParIndex = Expansion(Index)
        'End Function

        'Public Function GetSolParRef(ByVal ref As String) As Sol
        '    GetSolParRef = Expansion(GetIndexSol(ref))
        'End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        'Public Function SetSol(ByRef NouvSol As Sol, ByVal Index As Integer)
        '    If Index <= SolCount Then
        '        Expansion(Index) = NouvSol
        '    End If
        '    SetSol = NouvSol
        'End Function


    End Class
End Namespace