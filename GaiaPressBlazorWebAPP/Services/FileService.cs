// Services/FileService.cs
using Microsoft.JSInterop;

namespace GaiaPressBlazorWebAPP.Services
{
    public class FileService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<FileService> _logger;

        // Événement pour notifier les changements d'état
        public event Action? OnFileStateChanged;

        // État du fichier courant
        public bool HasUnsavedChanges { get; private set; }
        public string? CurrentFilePath { get; private set; }
        public bool IsNewFile { get; private set; }

        public FileService(IJSRuntime jsRuntime, ILogger<FileService> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        // ============================================
        // Créer un nouveau fichier
        // ============================================
        public async Task<bool> CreateNewFileAsync()
        {
            try
            {
                // Vérifier les modifications non enregistrées
                if (HasUnsavedChanges)
                {
                    // L'appelant doit gérer la confirmation
                    return false;
                }

                // Réinitialiser l'état
                await ResetStateAsync();

                IsNewFile = true;
                CurrentFilePath = null;

                // Notifier les composants
                OnFileStateChanged?.Invoke();

                _logger.LogInformation("Nouveau fichier créé");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'un nouveau fichier");
                return false;
            }
        }

        // ============================================
        // Réinitialiser l'état
        // ============================================
        private async Task ResetStateAsync()
        {
            HasUnsavedChanges = false;
            CurrentFilePath = null;
            IsNewFile = true;

            // Nettoyer le localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gaiaPress_currentFile");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gaiaPress_hasUnsavedChanges");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gaiaPress_currentChantier");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gaiaPress_currentForage");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gaiaPress_currentEssai");
        }

        // ============================================
        // Marquer comme modifié
        // ============================================
        public async Task MarkAsModifiedAsync()
        {
            HasUnsavedChanges = true;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gaiaPress_hasUnsavedChanges", "true");
            OnFileStateChanged?.Invoke();
        }

        // ============================================
        // Marquer comme enregistré
        // ============================================
        public async Task MarkAsSavedAsync(string filePath)
        {
            HasUnsavedChanges = false;
            CurrentFilePath = filePath;
            IsNewFile = false;

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gaiaPress_currentFile", filePath);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gaiaPress_hasUnsavedChanges", "false");
            OnFileStateChanged?.Invoke();
        }

        // ============================================
        // Obtenir l'état actuel
        // ============================================
        public async Task LoadStateAsync()
        {
            var filePath = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gaiaPress_currentFile");
            var hasChanges = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gaiaPress_hasUnsavedChanges");

            CurrentFilePath = filePath;
            HasUnsavedChanges = hasChanges == "true";
            IsNewFile = string.IsNullOrEmpty(filePath);
        }
    }
}