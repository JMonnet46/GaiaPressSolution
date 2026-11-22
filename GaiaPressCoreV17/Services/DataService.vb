Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Common
Imports GaiaPressCoreV17.Models
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17.Data

Namespace GaiaPressCoreV17.Services
    ''' <summary>
    ''' Service métier pour accéder aux données via Entity Framework Core
    ''' </summary>
    Public Class DataService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ' Déclaration des DataTables (optionnel, pour compatibilité legacy)
        Public ObjetDataConstructionsSitesDb As New DataTable()
        Public ObjetDataLesForages As New DataTable()
        Public ObjetDataLesEssaiForage As New DataTable()
        Public ObjetDataMesuresForage As New DataTable()
        Public ObjetDataSols As New DataTable()
        Public ObjetDataListeSonde As New DataTable()
        Public ObjetDataLesEssaisResistanceDbPropre As New DataTable()
        Public ObjetDataLesMesuresResistanceDbPropre As New DataTable()
        Public ObjetDataLesEssaisExpansionPropre As New DataTable()
        Public ObjetDataLesMesuresExpansionPropre As New DataTable()

        ' ================================================================
        ' Constructeur
        ' ================================================================

        ' Constructeur : injecter le DbContext
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub


        ' ============================================================
        ' MÉTHODES Chantier
        ' ============================================================

        ''' <summary>
        ''' Récupère tous les Chantiers depuis la base de données.
        ''' </summary>
        Public Function GetAllChantier() As List(Of Chantier)
            Return _dbContext.ChantiersDb.ToList()
        End Function

        ''' <summary>
        ''' Récupère un Chantier par sa référence.
        ''' </summary>
        Public Function GetChantierByRef(RefChantier As String) As Chantier
            Return _dbContext.ChantiersDb.FirstOrDefault(Function(c) c.RefChantier = RefChantier)
        End Function

        ''' <summary>
        ''' Ajoute un nouveau Chantier.
        ''' </summary>
        Public Sub AddChantier(Chantier As Chantier)
            If Chantier Is Nothing Then Return
            _dbContext.ChantiersDb.Add(Chantier)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Met à jour un Chantier existant.
        ''' </summary>
        Public Sub UpDateChantier(Chantier As Chantier)
            If Chantier Is Nothing Then Return
            _dbContext.ChantiersDb.Update(Chantier)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Supprime un Chantier.
        ''' </summary>
        Public Sub DeleteChantier(RefChantier As String)
            Dim Chantier = GetChantierByRef(RefChantier)
            If Chantier IsNot Nothing Then
                _dbContext.ChantiersDb.Remove(Chantier)
                _dbContext.SaveChanges()
            End If
        End Sub

        ''' <summary>
        ''' Sauvegarde tous les Chantiers.
        ''' </summary>
        Public Function SaveAllChantiers() As Boolean
            Try
                _dbContext.SaveChanges()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ' ============================================================
        ' MÉTHODES FORAGE
        ' ============================================================

        Public Function GetAllForage() As List(Of Forage)
            Return _dbContext.ListeForageDb.ToList()
        End Function

        Public Function GetForagesByChantier(ChantierId As String) As List(Of Forage)
            Return _dbContext.ListeForageDb _
                .Where(Function(f) f.RefChantier = ChantierId) _
                .ToList()
        End Function

        ' ============================================================
        ' MÉTHODES ESSAI FORAGE
        ' ============================================================

        Public Function GetEssaiForage(refEssai As String) As Essai_Forage
            Return _dbContext.ListeEssaiForageDb _
                .FirstOrDefault(Function(e) e.refEssai = refEssai)
        End Function

        Public Function GetAllEssaiForage() As List(Of Essai_Forage)
            Return _dbContext.ListeEssaiForageDb.ToList()
        End Function

        ' ============================================================
        ' MÉTHODES UTILITAIRES
        ' ============================================================

        Public Function GenerateReferenceEssai(typeEssai As Integer) As String
            Dim random As New Random()
            Return $"ESSAI-{typeEssai}-{random.Next(1000, 9999)}"
        End Function

        Public Function ValidateModel(model As Object) As Boolean
            Return True ' À adapter selon vos besoins
        End Function

        ''' <summary>
        ''' Charge les données depuis la base de données (pour initialisation)
        ''' </summary>
        Public Sub LoadDataFromDatabase(ChantierId As String)
            ' Les données sont accessibles via _dbContext directement
            ' Cette méthode peut rester vide si on utilise EF Core partout
        End Sub

        '----------------------------------------------------------
        ' Lecture des données sur la Base de données externe
        '----------------------------------------------------------
    End Class
End Namespace


