Imports System.Data
Imports Microsoft.Data.SqlClient

Namespace GaiaPressCoreV17.Services
    Public Class DatabaseConnection

        '=============================================================================
        ' se connecte à la BD externe et l'ouvre
        '=============================================================================

        ' Chaîne de connexion sécurisée
        Private Shared ReadOnly _connectionString As String =
            "Server=tcp:your-server.database.windows.net,1433;" &
            "Initial Catalog=you database;" &
            "Persist Security Info=False;" &
            "User ID=you username;" &
            "Password=you password;" &
            "MultipleActiveResultSets=False;" &
            "Encrypt=True;" &
            "TrustServerCertificate=False;" &
            "Connection Timeout=30;"

        ''' <summary>
        ''' Obtient la chaîne de connexion
        ''' </summary>
        Public Shared ReadOnly Property ConnectionString As String
            Get
                Return _connectionString
            End Get
        End Property

        ''' <summary>
        ''' Teste la connexion à la BD
        ''' </summary>
        Public Shared Function TestConnection() As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    connection.Open()
                    Return connection.State = ConnectionState.Open
                End Using
            Catch ex As Exception
                Console.WriteLine($"Erreur de connexion : {ex.Message}")
                Return False
            End Try
        End Function

    End Class
End Namespace
