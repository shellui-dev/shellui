using ShellUI.Components;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class ShellCnTests
{
    [Theory]
    [InlineData("max-w-md p-4", "max-w-lg", "p-4 max-w-lg")]
    [InlineData("px-4 py-2", "px-6", "py-2 px-6")]
    [InlineData("px-2", "p-4", "p-4")]
    [InlineData("p-4", "px-2", "p-4 px-2")]
    [InlineData("bg-primary text-primary-foreground", "bg-destructive", "text-primary-foreground bg-destructive")]
    [InlineData("text-sm text-muted-foreground", "text-lg", "text-muted-foreground text-lg")]
    [InlineData("text-sm text-muted-foreground", "text-red-500", "text-sm text-red-500")]
    [InlineData("border border-input", "border-2", "border-input border-2")]
    [InlineData("border border-input", "border-destructive", "border border-destructive")]
    [InlineData("rounded-md", "rounded-none", "rounded-none")]
    [InlineData("rounded-t-md", "rounded-lg", "rounded-lg")]
    [InlineData("flex items-center", "hidden", "items-center hidden")]
    [InlineData("hover:bg-accent", "hover:bg-muted", "hover:bg-muted")]
    [InlineData("md:hover:bg-accent", "hover:md:bg-muted", "hover:md:bg-muted")]
    [InlineData("data-[state=open]:bg-accent", "data-[state=open]:bg-muted", "data-[state=open]:bg-muted")]
    [InlineData("mt-2", "-mt-4", "-mt-4")]
    [InlineData("w-10 h-10", "size-8", "size-8")]
    [InlineData("shadow-sm", "shadow-lg", "shadow-lg")]
    [InlineData("ring-2 ring-ring", "ring-0", "ring-ring ring-0")]
    [InlineData("font-medium font-sans", "font-semibold", "font-sans font-semibold")]
    [InlineData("text-sm", "text-[13px]", "text-[13px]")]
    [InlineData("text-foreground", "text-[#ff0000]", "text-[#ff0000]")]
    [InlineData("[--sidebar-width:16rem]", "[--sidebar-width:20rem]", "[--sidebar-width:20rem]")]
    public void LaterClassWins(string defaults, string overrides, string expected) =>
        Assert.Equal(expected, Shell.Cn(defaults, overrides));

    [Theory]
    [InlineData("p-4 md:p-6")]
    [InlineData("bg-primary hover:bg-primary/90")]
    [InlineData("ring-2 ring-ring ring-offset-2 ring-offset-background")]
    [InlineData("border-b border-border")]
    [InlineData("text-sm leading-none")]
    [InlineData("shadow-sm shadow-black/5")]
    [InlineData("max-w-lg max-w-lg!")]
    [InlineData("translate-x-1/2 -translate-y-1/2")]
    [InlineData("group peer custom-class another-custom")]
    [InlineData("flex flex-col gap-2 overflow-hidden overflow-y-auto")]
    public void DifferentPropertiesOrVariants_AreKept(string classes) =>
        Assert.Equal(classes, Shell.Cn(classes));

    [Fact]
    public void SkipsEmptyParts() =>
        Assert.Equal("a b", Shell.Cn(null, "a", "", "  ", "b"));

    [Fact]
    public void CliTemplate_IsThePackageHelper()
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (!File.Exists(Path.Combine(root.FullName, "ShellUI.slnx"))) root = root.Parent!;
        var package = File.ReadAllText(Path.Combine(root.FullName, "src", "ShellUI.Components", "Shell.cs")).Replace("\r\n", "\n");
        var template = ComponentRegistry.GetComponentContent("shell")!
            .Replace("YourProjectNamespace.Components.UI", "ShellUI.Components").Replace("\r\n", "\n");

        Assert.Equal(package, template);
    }
}
