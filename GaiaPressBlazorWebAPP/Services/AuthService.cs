using Microsoft.JSInterop;

namespace GaiaPressBlazorWebAPP.Services;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "gaiaPressAuthUser";

    public string? CurrentUserName { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public event Action? OnAuthStateChanged;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // À appeler au démarrage de l'application
    public async Task InitializeAsync()
    {
        try
        {
            var stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(stored))
            {
                CurrentUserName = stored;
                OnAuthStateChanged?.Invoke();
            }
        }
        catch
        {
            // JS non disponible pendant le prerendering
        }
    }

    public async Task<bool> ChangePasswordAsync(string userName, string newPassword)
    {
        // TODO: Implémenter le stockage sécurisé du nouveau mot de passe
        // Pour l'instant, on simule le succès
        await Task.Delay(100);
        return true;
    }

    public async Task LoginAsync(string userName, bool rememberMe = false)  // ← 2 paramètres
    {
        CurrentUserName = userName;
        IsAuthenticated = true;

        // Optionnel : stocker dans localStorage si "Se souvenir de moi"
        if (rememberMe)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_user", userName);
        }

        OnAuthStateChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        IsAuthenticated = false;
        CurrentUserName = null;

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_user");
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "auth_user");

        OnAuthStateChanged?.Invoke();
    }
}