using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ShellUI.Components;
using ShellUI.Components.Services;

namespace ShellUI.Tests;

public class PackageRenderTests
{
    public sealed class Row
    {
        public string Name { get; set; } = "";
    }

    // Other render failures come from a missing parent or required parameter; these two are always bugs.
    private static readonly string[] BindingErrors =
    {
        "does not have a property matching the name",
        "declares more than one parameter matching the name",
    };

    [Fact]
    public void EveryPackageComponent_RendersWithoutParameterBindingErrors()
    {
        var types = typeof(Shell).Assembly.GetExportedTypes()
            .Where(t => typeof(IComponent).IsAssignableFrom(t) && !t.IsAbstract && t.Namespace == "ShellUI.Components")
            .ToList();
        Assert.NotEmpty(types);

        var offenders = new List<string>();
        foreach (var type in types)
        {
            var closed = type.IsGenericTypeDefinition ? Close(type) : type;
            if (closed is null)
            {
                offenders.Add($"{type.Name}: could not choose type arguments");
                continue;
            }

            using var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddShellUISonner();
            ctx.Services.AddScoped<IThemeService, ThemeService>();

            try
            {
                ctx.Render<DynamicComponent>(p => p.Add(c => c.Type, closed));
            }
            catch (Exception ex)
            {
                var message = Flatten(ex);
                if (BindingErrors.Any(message.Contains)) offenders.Add($"{type.Name}: {message}");
            }
        }

        Assert.True(offenders.Count == 0, "Package components that fail parameter binding:\n  " + string.Join("\n  ", offenders));
    }

    private static Type? Close(Type definition)
    {
        foreach (var candidate in new[] { typeof(Row), typeof(string), typeof(int) })
        {
            try
            {
                return definition.MakeGenericType(definition.GetGenericArguments().Select(_ => candidate).ToArray());
            }
            catch (ArgumentException) { }
        }
        return null;
    }

    private static string Flatten(Exception ex) =>
        ex is AggregateException agg
            ? string.Join(" | ", agg.Flatten().InnerExceptions.Select(Flatten))
            : ex.InnerException is null ? ex.Message : ex.Message + " | " + Flatten(ex.InnerException);
}
