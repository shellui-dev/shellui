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

    // A component that renders empty but throws once it has a value fails here (the TimePicker "HH:mm" bug).
    [Fact]
    public void ValueBearingComponents_RenderWithASampleValue()
    {
        var types = typeof(Shell).Assembly.GetExportedTypes()
            .Where(t => typeof(IComponent).IsAssignableFrom(t) && !t.IsAbstract && t.Namespace == "ShellUI.Components")
            .Select(t => t.IsGenericTypeDefinition ? Close(t) : t)
            .OfType<Type>()
            .ToList();

        var tried = 0;
        var offenders = new List<string>();
        foreach (var type in types)
        {
            var values = SampleValues(type);
            if (values.Count == 0 || !Renders(type, new Dictionary<string, object>(), out _)) continue;

            tried++;
            if (!Renders(type, values, out var error))
                offenders.Add($"{type.Name} ({string.Join(", ", values.Keys)}): {error}");
        }

        Assert.True(tried >= 10, $"Only {tried} components had a value to set; the sample types may need extending.");
        Assert.True(offenders.Count == 0, "Package components that throw once a value is set:\n  " + string.Join("\n  ", offenders));
    }

    private static readonly System.Text.RegularExpressions.Regex ValueLike = new("Value|Date|Time|Selected");

    private static Dictionary<string, object> SampleValues(Type type)
    {
        var values = new Dictionary<string, object>();
        foreach (var p in type.GetProperties())
        {
            if (!p.IsDefined(typeof(ParameterAttribute), true) || !p.CanWrite || !ValueLike.IsMatch(p.Name)) continue;
            var t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            object? sample =
                t == typeof(string) ? "b" :
                t == typeof(DateTime) ? new DateTime(2026, 3, 14) :
                t == typeof(DateOnly) ? new DateOnly(2026, 3, 14) :
                t == typeof(TimeSpan) ? new TimeSpan(9, 5, 0) :
                t == typeof(TimeOnly) ? new TimeOnly(9, 5) :
                t == typeof(int) ? 42 :
                t == typeof(double) ? 42.5 :
                t == typeof(decimal) ? 42.5m :
                t == typeof(bool) ? true :
                t == typeof(List<string>) ? new List<string> { "a", "b" } :
                t == typeof(IEnumerable<string>) ? new List<string> { "a", "b" } :
                null;
            if (sample != null) values[p.Name] = sample;
        }
        return values;
    }

    private static bool Renders(Type type, Dictionary<string, object> parameters, out string error)
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ctx.Services.AddShellUISonner();
        ctx.Services.AddScoped<IThemeService, ThemeService>();
        try
        {
            ctx.Render<DynamicComponent>(p => p.Add(c => c.Type, type).Add(c => c.Parameters, parameters));
            error = "";
            return true;
        }
        catch (Exception ex)
        {
            error = Flatten(ex);
            return false;
        }
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
