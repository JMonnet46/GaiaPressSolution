Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports GaiaPressCoreV17.Data
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class ListeEssaiForage

        ' Autres propriétés existantes...
        Public Property RefForage As String
        Public Property RefEssai As String
        Public Property NomEssai As String


        ' Contexte de données fourni de l'extérieur (Core ne connaît pas le projet Blazor)
        Private ReadOnly _context As GaiaPress_Db_Context

        ' ======= Propriétés de Liste ===========
        ' Collection en mémoire pour manipulation temporaire
        Public Property ListeEssaiForageSession As List(Of Essai_Forage)

        Public FichierListeExpansion As String
        Public EventsSuspendus As Boolean
        Public IsSaved As Boolean

        Public Event HasBeenModified()

        ' Le contexte est obligatoire : la classe ne se crée plus sans base de données
        Public Sub New(context As GaiaPress_Db_Context)
            _context = context
            ListeEssaiForageSession = New List(Of Essai_Forage)()
        End Sub

        ' ================================================================================
        ' Chargement
        ' ================================================================================

        ' Charge les essais depuis la base dans la liste de session
        Public Function LoadEssaiForage() As List(Of Essai_Forage)
            ListeEssaiForageSession = _context.ListeEssaiForageDb.ToList()
            Return ListeEssaiForageSession
        End Function

        Public Async Function LoadEssaiForageAsync() As Task(Of List(Of Essai_Forage))
            ListeEssaiForageSession = Await _context.ListeEssaiForageDb.ToListAsync()
            Return ListeEssaiForageSession
        End Function

        ' ================================================================================
        ' Gestion de ListeEssaiForageSession (base de données)
        ' ================================================================================

        Public Function AddEssai(essai As Essai_Forage) As Boolean
            Try
                _context.ListeEssaiForageDb.Add(essai)
                _context.SaveChanges()
                ListeEssaiForageSession.Add(essai)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
        Public Function GetEssaiForageByRef(refEssai As String) As Essai_Forage
            Return _context.ListeEssaiForageDb.FirstOrDefault(Function(e) e.RefEssai = refEssai)
        End Function

        Public Function ModifyEssaiForage(essai As Essai_Forage) As Boolean
            Try
                _context.ListeEssaiForageDb.Update(essai)
                _context.SaveChanges()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub DeleteEssaiForage(refEssai As String)
            Dim essaiADelete As Essai_Forage = GetEssaiForageByRef(refEssai)
            If essaiADelete IsNot Nothing Then
                _context.ListeEssaiForageDb.Remove(essaiADelete)
                _context.SaveChanges()
                ListeEssaiForageSession.Remove(essaiADelete)
            End If
        End Sub

        ' ================================================================================
        ' Accès à la liste de session
        ' ================================================================================

        Public Function GetEssaiCount() As Integer
            Return ListeEssaiForageSession.Count
        End Function

        Public Function IsRefEssaiUnique(ref As String) As Boolean
            Return Not ListeEssaiForageSession.Any(Function(e) e.RefEssai = ref)
        End Function

        Public Function IsNomEssaiUnique(nom As String) As Boolean
            Return Not ListeEssaiForageSession.Any(Function(e) e.nomEssai = nom)
        End Function

        Public Sub CloseListeEssaiForage()
            ListeEssaiForageSession = Nothing
        End Sub

        Public Sub IHaveBeenModified()
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

    End Class
End Namespace