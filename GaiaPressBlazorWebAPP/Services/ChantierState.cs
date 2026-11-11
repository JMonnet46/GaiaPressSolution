namespace GaiaPressBlazorWebAPP.Services;

/// <summary>
/// Holds the currently selected construction-site reference for the Blazor Server circuit.
/// </summary>
public sealed class ChantierState
{
    public string? RefChantier { get; private set; }

    public event Action? OnChange;

    public void SetChantier(string? refChantier)
    {
        var normalized = string.IsNullOrWhiteSpace(refChantier)
            ? null
            : refChantier.Trim();

        if (string.Equals(RefChantier, normalized, StringComparison.Ordinal))
            return;

        RefChantier = normalized;
        OnChange?.Invoke();
    }
}

