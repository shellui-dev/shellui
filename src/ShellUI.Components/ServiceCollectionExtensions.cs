using Microsoft.Extensions.DependencyInjection;
using ShellUI.Components.Services;

namespace ShellUI.Components;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShellUISonner(this IServiceCollection services)
    {
        services.AddScoped<SonnerService>();
        services.AddScoped<ISonnerService>(sp => sp.GetRequiredService<SonnerService>());
        return services;
    }
}
