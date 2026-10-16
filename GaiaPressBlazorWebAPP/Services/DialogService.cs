using Microsoft.JSInterop;

namespace GaiaPressBlazorWebAPP.Services
{
    public class DialogService
    {
        private readonly IJSRuntime _jsRuntime;

        public DialogService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Affiche une boîte de dialogue de confirmation (OK/Annuler)
        /// </summary>
        public async Task<bool> ConfirmAsync(string message)
        {
            return await _jsRuntime.InvokeAsync<bool>("confirm", message);
        }

        /// <summary>
        /// Affiche une boîte de dialogue d'alerte
        /// </summary>
        public async Task AlertAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("alert", message);
        }

        /// <summary>
        /// Affiche une boîte de dialogue de saisie (prompt)
        /// </summary>
        public async Task<string?> PromptAsync(string message, string? defaultValue = null)
        {
            return await _jsRuntime.InvokeAsync<string?>("prompt", message, defaultValue);
        }
    }
}
