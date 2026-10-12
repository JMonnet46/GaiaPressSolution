Imports GaiaPressCoreV17.Data
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading
Imports System.Collections.Generic


Namespace GaiaPressCoreV17.Services

    ''' <summary>
    ''' Service pour la gestion des forages et de leurs essais.
    ''' </summary>
    Public Class ForageService
        Public ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        ''' </summary>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ' ======================
        ' Gestion des Forages
        ' ======================

        ''' <summary>
        ''' Récupère tous les forages.
        ''' </summary>
        Public Function GetAllForages() As List(Of Forage)
            Return _dbContext.ListeForageDb.ToList()
        End Function

        Public Sub SaveForage(forage As Forage)
            If forage.RefForage = "" Then
                _dbContext.ListeForageDb.Add(forage)
            Else
                _dbContext.ListeForageDb.Update(forage)
            End If
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Récupère un forage par sa référence.
        ''' </summary>
        Public Function GetForageByRef(refForage As String) As Forage
            Return _dbContext.ListeForageDb.FirstOrDefault(Function(f) f.RefForage = refForage)
        End Function

        ''' <summary>
        ''' Ajoute un nouveau forage.
        ''' </summary>
        Public Sub AddForage(forage As Forage)
            _dbContext.ListeForageDb.Add(forage)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Met à jour un forage existant.
        ''' </summary>
        Public Sub UpdateForage(forage As Forage)
            Dim existingForage = _dbContext.ListeForageDb.FirstOrDefault(Function(f) f.RefForage = forage.RefForage)
            If existingForage IsNot Nothing Then
                _dbContext.Entry(existingForage).CurrentValues.SetValues(forage)
                _dbContext.SaveChanges()
            End If
        End Sub

        ''' <summary>
        ''' Supprime un forage par sa référence.
        ''' </summary>
        Public Sub DeleteForage(refForage As String)
            Dim forage = _dbContext.ListeForageDb.FirstOrDefault(Function(f) f.RefForage = refForage)
            If forage IsNot Nothing Then
                _dbContext.ListeForageDb.Remove(forage)
                _dbContext.SaveChanges()
            End If
        End Sub

        ' ======================
        ' Gestion des Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère tous les essais d'un forage.
        ''' </summary>
        Public Function GetEssaisByForage(refForage As String) As List(Of Essai_Forage)
            Return _dbContext.ListeEssaiForageDb.Where(Function(e) e.RefForage = refForage).ToList()
        End Function

        ''' <summary>
        ''' Ajoute un essai à un forage.
        ''' </summary>
        Public Sub AddEssaiToForage(refForage As String, essai As Essai_Forage)
            essai. RefForage = refForage
            _dbContext.ListeEssaiForageDb.Add(essai)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Supprime un essai par sa référence.
        ''' </summary>
        Public Sub DeleteEssai(refEssai As String)
            Dim essai = _dbContext.ListeEssaiForageDb.FirstOrDefault(Function(e) e.refEssai = refEssai)
            If essai IsNot Nothing Then
                _dbContext.ListeEssaiForageDb.Remove(essai)
                _dbContext.SaveChanges()
            End If
        End Sub
    End Class
End Namespace
