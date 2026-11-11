namespace GaiaPressBlazorWebAPP.Services;

public sealed class ChantierState
{
    public string? RefChantier { get; private set; }

    public event Action? OnChange;

    public void SetChantier(string? refChantier)
    {
        RefChantier = refChantier;
        OnChange?.Invoke();
    }
}
