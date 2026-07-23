namespace ShellUI.Components.Services;

public class SonnerToastItem
{
    public string Id { get; } = Guid.NewGuid().ToString("N")[..8];
    public string Message { get; set; } = "";
    public string? Description { get; set; }
    public string Variant { get; set; } = "default";
    // Zero or negative disables auto-dismiss (toast stays until manually closed).
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(4);
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public interface ISonnerService
{
    IReadOnlyList<SonnerToastItem> Toasts { get; }
    void Show(string message, string? description = null, string variant = "default", TimeSpan? duration = null);
    void Success(string message, string? description = null, TimeSpan? duration = null);
    void Error(string message, string? description = null, TimeSpan? duration = null);
    void Info(string message, string? description = null, TimeSpan? duration = null);
    void Warning(string message, string? description = null, TimeSpan? duration = null);
    void Remove(string id);
}

public class SonnerService : ISonnerService
{
    private readonly List<SonnerToastItem> _toasts = new();
    public IReadOnlyList<SonnerToastItem> Toasts => _toasts;
    public event Action? OnChange;

    public void Show(string message, string? description = null, string variant = "default", TimeSpan? duration = null)
    {
        var toast = new SonnerToastItem
        {
            Message = message,
            Description = description,
            Variant = variant,
            Duration = duration ?? TimeSpan.FromSeconds(4)
        };
        _toasts.Add(toast);
        OnChange?.Invoke();

        if (toast.Duration > TimeSpan.Zero)
        {
            _ = AutoDismissAsync(toast.Id, toast.Duration);
        }
    }

    public void Success(string message, string? description = null, TimeSpan? duration = null)
        => Show(message, description, "success", duration);

    public void Error(string message, string? description = null, TimeSpan? duration = null)
        => Show(message, description, "destructive", duration);

    public void Info(string message, string? description = null, TimeSpan? duration = null)
        => Show(message, description, "info", duration);

    public void Warning(string message, string? description = null, TimeSpan? duration = null)
        => Show(message, description, "warning", duration);

    public void Remove(string id)
    {
        var removed = _toasts.RemoveAll(t => t.Id == id);
        if (removed > 0) OnChange?.Invoke();
    }

    private async Task AutoDismissAsync(string id, TimeSpan delay)
    {
        try
        {
            await Task.Delay(delay);
            Remove(id);
        }
        catch { }
    }
}
