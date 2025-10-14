using ShellUI.Core.Models;
using ShellUI.Templates.Templates;

namespace ShellUI.Templates;

public static class ComponentRegistry
{
    public static readonly Dictionary<string, ComponentMetadata> Components = new()
    {
        { "button", ButtonTemplate.Metadata },
        { "theme-toggle", ThemeToggleTemplate.Metadata },
        { "input", InputTemplate.Metadata },
        { "card", CardTemplate.Metadata },
        { "alert", AlertTemplate.Metadata },
        { "badge", BadgeTemplate.Metadata },
        { "label", LabelTemplate.Metadata },
        { "textarea", TextareaTemplate.Metadata },
        { "dialog", DialogTemplate.Metadata }
    };

    public static string? GetComponentContent(string componentName)
    {
        return componentName.ToLower() switch
        {
            "button" => ButtonTemplate.Content,
            "theme-toggle" => ThemeToggleTemplate.Content,
            "input" => InputTemplate.Content,
            "card" => CardTemplate.Content,
            "alert" => AlertTemplate.Content,
            "badge" => BadgeTemplate.Content,
            "label" => LabelTemplate.Content,
            "textarea" => TextareaTemplate.Content,
            "dialog" => DialogTemplate.Content,
            _ => null
        };
    }

    public static IEnumerable<ComponentMetadata> GetByCategory(ComponentCategory category)
    {
        return Components.Values.Where(c => c.Category == category);
    }

    public static IEnumerable<ComponentMetadata> SearchByTag(string tag)
    {
        return Components.Values.Where(c => c.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }

    public static ComponentMetadata? GetMetadata(string componentName)
    {
        return Components.TryGetValue(componentName.ToLower(), out var metadata) ? metadata : null;
    }

    public static bool Exists(string componentName)
    {
        return Components.ContainsKey(componentName.ToLower());
    }
}

