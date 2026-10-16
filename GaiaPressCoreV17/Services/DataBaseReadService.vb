Imports System
Imports System.Collections.Generic
Imports System.Data
Imports GaiaPressCoreV17.Models
Imports Microsoft.Data.SqlClient

Namespace GaiaPressCoreV17.Services
    Public Class DatabaseReadService

        Private ReadOnly _connectionString As String

        Public Sub New()
            _connectionString = DatabaseConnection.ConnectionString
        End Sub

        ''' <summary>
        ''' Récupère toutes les lignes d'une table
        ''' </summary>
        Public Function GetAllFromTable(tableName As String) As DataTable
            Dim table As New DataTable()
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{tableName}]"
                    Dim adapter As New SqlDataAdapter(query, connection)
                    adapter.Fill(table)
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture {tableName} : {ex.Message}")
            End Try
            Return table
        End Function

        ''' <summary>
        ''' Récupère les lignes filtrées d'une table
        ''' </summary>
        Public Function GetFromTable(tableName As String, whereClause As String) As DataTable
            Dim table As New DataTable()
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{tableName}] WHERE {whereClause}"
                    Dim adapter As New SqlDataAdapter(query, connection)
                    adapter.Fill(table)
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture {tableName} : {ex.Message}")
            End Try
            Return table
        End Function

        ''' <summary>
        ''' Récupère une ligne spécifique par ID (String)
        ''' </summary>
        Public Function GetByRefId(tableName As String, idColumnName As String, id As String) As DataRow
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{tableName}] WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", id)

                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    If table.Rows.Count > 0 Then
                        Return table.Rows(0)
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture {tableName} (ID={id}) : {ex.Message}")
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Récupère une ligne spécifique par ID (Integer)
        ''' </summary>
        Public Function GetById(tableName As String, idColumnName As String, id As Integer) As DataRow
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{tableName}] WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", id)

                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    If table.Rows.Count > 0 Then
                        Return table.Rows(0)
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture {tableName} (ID={id}) : {ex.Message}")
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Récupère les enfants d'un parent (par clé étrangère - String)
        ''' </summary>
        Public Function GetChildrenByParentRef(childTable As String, foreignKeyColumn As String, parentId As String) As DataTable
            Dim table As New DataTable()
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{childTable}] WHERE [{foreignKeyColumn}] = @ParentID ORDER BY Ind"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ParentID", parentId)

                    Dim adapter As New SqlDataAdapter(command)
                    adapter.Fill(table)
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture enfants : {ex.Message}")
            End Try
            Return table
        End Function

        ''' <summary>
        ''' Récupère les enfants d'un parent (par clé étrangère - Integer)
        ''' </summary>
        Public Function GetChildrenByParentId(childTable As String, foreignKeyColumn As String, parentId As Integer) As DataTable
            Dim table As New DataTable()
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"SELECT * FROM [{childTable}] WHERE [{foreignKeyColumn}] = @ParentID ORDER BY Ind"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ParentID", parentId)

                    Dim adapter As New SqlDataAdapter(command)
                    adapter.Fill(table)
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture enfants : {ex.Message}")
            End Try
            Return table
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES AUX Chantier & FORAGES
        ' ============================================

        ''' <summary>
        ''' Récupère tous les Chantier
        ''' </summary>
        Public Function GetAllChantier() As DataTable
            Return GetAllFromTable("LesChantier")
        End Function

        ''' <summary>
        ''' Récupère un chantier par sa référence
        ''' </summary>
        Public Function GetChantierById(refChantier As String) As DataRow
            Return GetByRefId("LesChantier", "refChantier", refChantier)
        End Function

        ''' <summary>
        ''' Récupère tous les forages
        ''' </summary>
        Public Function GetAllForages() As DataTable
            Return GetAllFromTable("LesForages")
        End Function

        ''' <summary>
        ''' Récupère les forages d'un chantier
        ''' </summary>
        Public Function GetForagesByChantier(chantierId As String) As DataTable
            Return GetChildrenByParentRef("LesForages", "refChantier", chantierId)
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES AUX ESSAIS DE FORAGE
        ' ============================================

        ''' <summary>
        ''' Récupère tous les essais de forage
        ''' </summary>
        Public Function GetAllEssaiForages() As DataTable
            Return GetAllFromTable("LesEssaiForages")
        End Function

        ''' <summary>
        ''' Récupère les essais d'un forage
        ''' </summary>
        Public Function GetEssaisByForage(forageId As String) As DataTable
            Return GetChildrenByParentRef("LesEssaiForages", "refForage", forageId)
        End Function

        ''' <summary>
        ''' Récupère un essai de forage par sa référence
        ''' </summary>
        Public Function GetEssaiForageByRef(refEssai As String) As DataRow
            Return GetByRefId("LesEssaiForages", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère les mesures d'un essai de forage
        ''' </summary>
        Public Function GetMesuresByEssaiForage(refEssai As String) As DataTable
            Return GetChildrenByParentRef("LesMesuresForages", "refEssai", refEssai)
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES AUX SONDES
        ' ============================================

        ''' <summary>
        ''' Récupère toutes les sondes
        ''' </summary>
        Public Function GetAllSondes() As DataTable
            Return GetAllFromTable("LesSondes")
        End Function

        ''' <summary>
        ''' Récupère une sonde par sa référence
        ''' </summary>
        Public Function GetSondeByRef(refSonde As String) As DataRow
            Return GetByRefId("LesSondes", "refSonde", refSonde)
        End Function

        ' ============================================
        ' ESSAIS D'EXPANSION - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Récupère tous les essais d'expansion
        ''' </summary>
        Public Function GetAllEssaisExpansion() As DataTable
            Return GetAllFromTable("LesEssaisExpansionPropre")
        End Function

        ''' <summary>
        ''' Récupère les essais d'expansion d'une sonde
        ''' </summary>
        Public Function GetEssaisExpansionBySonde(refSonde As String) As DataTable
            Return GetChildrenByParentRef("LesEssaisExpansionPropre", "refSonde", refSonde)
        End Function

        ''' <summary>
        ''' Récupère un essai d'expansion par sa référence
        ''' </summary>
        Public Function GetEssaiExpansionByRef(refEssai As String) As DataRow
            Return GetByRefId("LesEssaisExpansionPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère les mesures d'un essai d'expansion
        ''' </summary>
        Public Function GetMesureExpansionByEssai(refEssai As String) As DataTable
            Return GetChildrenByParentRef("LesMesureExpansionPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère une mesure d'expansion par son indice
        ''' </summary>
        Public Function GetMesureExpansionByIndice(refEssai As String, indiceMesure As Integer) As DataRow
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "SELECT * FROM [LesMesureExpansionPropre] WHERE [refEssai] = @refEssai AND [indiceMesure] = @indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)

                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    If table.Rows.Count > 0 Then
                        Return table.Rows(0)
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture mesure expansion : {ex.Message}")
            End Try
            Return Nothing
        End Function

        ' ============================================
        ' ESSAIS DE RÉSISTANCE - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Récupère tous les essais de résistance
        ''' </summary>
        Public Function GetAllEssaisResistanceDb() As DataTable
            Return GetAllFromTable("LesEssaisResistanceDbPropre")
        End Function

        ''' <summary>
        ''' Récupère les essais de résistance d'une sonde
        ''' </summary>
        Public Function GetEssaisResistanceDbBySonde(refSonde As String) As DataTable
            Return GetChildrenByParentRef("LesEssaisResistanceDbPropre", "refSonde", refSonde)
        End Function

        ''' <summary>
        ''' Récupère un essai de résistance par sa référence
        ''' </summary>
        Public Function GetEssaiResistanceByRef(refEssai As String) As DataRow
            Return GetByRefId("LesEssaisResistanceDbPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère les mesures d'un essai de résistance
        ''' </summary>
        Public Function GetMesuresResistanceDbByEssai(refEssai As String) As DataTable
            Return GetChildrenByParentRef("LesMesuresResistanceDbPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère une mesure de résistance par son indice
        ''' </summary>
        Public Function GetMesureResistanceByIndice(refEssai As String, indiceMesure As Integer) As DataRow
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "SELECT * FROM [LesMesuresResistanceDbPropre] WHERE [refEssai] = @refEssai AND [indiceMesure] = @indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)

                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    If table.Rows.Count > 0 Then
                        Return table.Rows(0)
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture mesure résistance : {ex.Message}")
            End Try
            Return Nothing
        End Function

        ' ============================================
        ' ESSAIS DE PERTE DE CHARGE - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Récupère tous les essais de perte de charge
        ''' </summary>
        Public Function GetAllEssaisPerteDeCharge() As DataTable
            Return GetAllFromTable("LesEssaisPerteDeCharge")
        End Function

        ''' <summary>
        ''' Récupère les essais de perte de charge d'une sonde
        ''' </summary>
        Public Function GetEssaisPerteDeChargeBySonde(refSonde As String) As DataTable
            Return GetChildrenByParentRef("LesEssaisPerteDeCharge", "refSonde", refSonde)
        End Function

        ''' <summary>
        ''' Récupère un essai de perte de charge par sa référence
        ''' </summary>
        Public Function GetEssaiPerteDeChargeByRef(refEssai As String) As DataRow
            Return GetByRefId("LesEssaisPerteDeCharge", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère les mesures d'un essai de perte de charge
        ''' </summary>
        Public Function GetMesuresPerteDeChargeByEssai(refEssai As String) As DataTable
            Return GetChildrenByParentRef("LesMesuresPerteDeCharge", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Récupère une mesure de perte de charge par son indice
        ''' </summary>
        Public Function GetMesurePerteDeChargeByIndice(refEssai As String, indiceMesure As Integer) As DataRow
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "SELECT * FROM [LesMesuresPerteDeCharge] WHERE [refEssai] = @refEssai AND [indiceMesure] = @indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)

                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    If table.Rows.Count > 0 Then
                        Return table.Rows(0)
                    End If
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur lecture mesure perte de charge : {ex.Message}")
            End Try
            Return Nothing
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES AUX SOLS
        ' ============================================

        ''' <summary>
        ''' Récupère tous les sols
        ''' </summary>
        Public Function GetAllSols() As DataTable
            Return GetAllFromTable("LesSols")
        End Function

        ''' <summary>
        ''' Récupère un sol par ID
        ''' </summary>
        Public Function GetSolById(solId As Integer) As DataRow
            Return GetById("LesSols", "SolId", solId)
        End Function

        ''' <summary>
        ''' Récupère un sol par sa référence
        ''' </summary>
        Public Function GetSolByRef(refSol As String) As DataRow
            Return GetByRefId("LesSols", "refSol", refSol)
        End Function

    End Class
End Namespace