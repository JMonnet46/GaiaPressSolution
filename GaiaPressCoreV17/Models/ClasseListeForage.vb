''' <summary>
''' Classe dédiée à la gestion d'une liste de forages en mémoire
''' </summary>
''' Imports System.Collections.Generic
Imports System.Collections.Generic
Imports System.Linq
Imports GaiaPressCoreV17.Models
Imports ContexteForages = GaiaPressCoreV17.Data.GaiaPressDbContext
Public Class ClasseListeForage


    ''' <summary>
    ''' Liste des forages en session (en mémoire)
    ''' </summary>
    Public Property ListeForageSession As New List(Of Forage)


    Public Sub New()

    End Sub


    ''' <summary>
    ''' Charge tous les forages depuis la base de données
    ''' </summary>
    Public Sub LoadForages(context As ContexteForages)
        ListeForageSession = context.ListeForageDb.ToList()
    End Sub

    ''' <summary>
    ''' Ajoute un nouveau forage à la liste et à la base de données
    ''' </summary>
    Public Sub AddForage(forage As Forage, context As ContexteForages)
        context.ListeForageDb.Add(forage)
        context.SaveChanges()

        ListeForageSession.Add(forage)
    End Sub

    Public Function IsRefForageUnique(
    ByVal ref As String,
    context As ContexteForages
) As Boolean

        Return Not context.ListeForageDb.Any(
        Function(f) f.RefForage = ref)
    End Function

    ''' <summary>
    ''' Met à jour un forage existant dans la liste et en base de données
    ''' </summary>
    Public Sub UpdateForage(forageToUpdate As Forage, context As GaiaPressDbContext)
        Dim existingForage = Me.ListeForageSession.FirstOrDefault(
            Function(f) f.RefForage = forageToUpdate.RefForage
        )

        If existingForage IsNot Nothing Then
            context.Entry(existingForage).CurrentValues.SetValues(forageToUpdate)
            context.SaveChanges()
        Else
            AddForage(forageToUpdate, context)
        End If
    End Sub

    ''' <summary>
    ''' Supprime un forage de la liste et de la base de données
    ''' </summary>
    Public Sub DeleteForage(refForage As String, context As GaiaPressDbContext)
        Dim forageToDelete = Me.ListeForageSession.FirstOrDefault(
            Function(f) f.RefForage = refForage
        )

        If forageToDelete IsNot Nothing Then
            Me.ListeForageSession.Remove(forageToDelete)
            ' ✅ Correction : Utilisation de ForagesDb
            context.ListeForageDb.Remove(forageToDelete)
            context.SaveChanges()
        End If
    End Sub

    ''' <summary>
    ''' Recherche un forage par sa référence
    ''' </summary>
    Public Function GetForageByRef(refForage As String) As Forage
        Return Me.ListeForageSession.FirstOrDefault(
            Function(f) f.RefForage = refForage
        )
    End Function
End Class
