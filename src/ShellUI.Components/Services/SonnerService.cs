namespace ShellUI.Components.Services;

public class SonnerToastItem
{
    public string Id { get; } = Guid.NewGuid().ToString("N")[..8];
    public string Message { get; set; } = "";
    public string? Description { get; set; }
    public string Variant { get; set; } = "default";
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public interface ISonnerService
{
    IReadOnlyList<SonnerToastItem> Toasts { get; }
    void Show(string message, string? description = null, string variant = "default");
}

public class SonnerService : ISonnerService
{
    private readonly List<SonnerToastItem> _toasts = new();
    public IReadOnlyList<SonnerToastItem> Toasts => _toasts;
    public event Action? OnChange;

    public void Show(string message, string? description = null, string variant = "default")
    {
        _toasts.Add(new SonnerToastItem { Message = message, Description = description, Variant = variant });
        OnChange?.Invoke();
    }

    public void Remove(string id)
    {
        _toasts.RemoveAll(t => t.Id == id);
        OnChange?.Invoke();
    }
}
