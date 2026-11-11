using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace GaiaPressBlazorWebAPP.Services
{
    /// <summary>
    /// État de connexion SQL de la session Blazor courante. Ce service ne représente
    /// pas l'identité d'authentification de l'application.
    /// </summary>
    public class ConnectionService
    {
        private static readonly TimeSpan ConnectionTestTimeout = TimeSpan.FromSeconds(60);

        public string? ConnectionString { get; private set; }
        public string? CurrentUser { get; private set; }
        public bool IsConnected { get; private set; }

        public event Action? OnConnectionChanged;

        /// <summary>
        /// Valide l'ouverture d'une connexion vers le serveur et la base visés.
        /// L'état connecté est publié uniquement après réussite; tout échec de test
        /// invalide l'état affiché. Les détails SQL/identifiants ne sont pas retournés.
        /// </summary>
        public async Task<bool> TrySetConnectionAsync(
            string connectionString,
            string user,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(user))
            {
                ClearConnection();
                return false;
            }

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(ConnectionTestTimeout);

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    ConnectTimeout = Math.Max(1, (int)Math.Ceiling(ConnectionTestTimeout.TotalSeconds))
                };
                await using var connection = new SqlConnection(builder.ConnectionString);
                await connection.OpenAsync(timeout.Token);

                cancellationToken.ThrowIfCancellationRequested();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    ClearConnection();
                    return false;
                }

                SetConnection(builder.ConnectionString, user);
                return true;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                ClearConnection();
                throw;
            }
            catch (OperationCanceledException)
            {
                ClearConnection();
                return false;
            }
            catch (Exception)
            {
                ClearConnection();
                return false;
            }
        }

        /// <summary>
        /// Compatibilité avec les appelants existants. N'effectue pas de test SQL;
        /// les nouveaux parcours de connexion doivent appeler TrySetConnectionAsync.
        /// </summary>
        public void SetConnection(string connectionString, string user)
        {
            ConnectionString = connectionString;
            CurrentUser = user;
            IsConnected = true;
            OnConnectionChanged?.Invoke();
        }

        public void ClearConnection()
        {
            ConnectionString = null;
            CurrentUser = null;
            IsConnected = false;
            OnConnectionChanged?.Invoke();
        }
    }
}
