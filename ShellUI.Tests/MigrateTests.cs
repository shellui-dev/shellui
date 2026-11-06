using ShellUI.CLI.Services;

namespace ShellUI.Tests;

public class MigrateTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), $"migrate-{Guid.NewGuid():N}");

    public MigrateTests()
    {
        Directory.CreateDirectory(Path.Combine(_root, "Components", "UI"));
        Directory.CreateDirectory(Path.Combine(_root, "Components", "Pages"));
        Directory.CreateDirectory(Path.Combine(_root, "obj"));
        File.WriteAllText(Path.Combine(_root, "MyApp.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
    }

    public void Dispose()
    {
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    private string Page(string name, string content)
    {
        var path = Path.Combine(_root, "Components", "Pages", name);
        File.WriteAllText(path, content);
        return path;
    }

    private MigrationEdit SingleEdit(params string[] components) => Assert.Single(MigrateService.Plan(_root, components).Edits);

    [Fact]
    public void Input_DropsTheNestedVariantEnum()
    {
        Page("Login.razor", "<Input Variant=\"Input.InputVariant.Error\" />\n@code { var v = Input.InputVariant.Default; }");

        var edit = SingleEdit("input");

        Assert.Equal("<Input Variant=\"InputVariant.Error\" />\n@code { var v = InputVariant.Default; }", edit.After);
    }

    [Fact]
    public void DatePicker_RenamesSelectedDateButLeavesCalendarAlone()
    {
        Page("Booking.razor",
            "<DatePicker @bind-SelectedDate=\"_date\" @bind-SelectedDate:after=\"Save\" />\n" +
            "<DatePicker SelectedDate=\"_other\"\n            SelectedDateChanged=\"OnPicked\" Placeholder=\"a > b\" />\n" +
            "<Calendar @bind-SelectedDate=\"_day\" />");

        var edit = SingleEdit("date-picker");

        Assert.Equal(
            "<DatePicker @bind-Value=\"_date\" @bind-Value:after=\"Save\" />\n" +
            "<DatePicker Value=\"_other\"\n            ValueChanged=\"OnPicked\" Placeholder=\"a > b\" />\n" +
            "<Calendar @bind-SelectedDate=\"_day\" />", edit.After);
    }

    [Fact]
    public void TimePicker_RenamesSelectedTime()
    {
        Page("Alarm.razor", "<TimePicker @bind-SelectedTime=\"_time\" Step=\"15\" />");

        Assert.Equal("<TimePicker @bind-Value=\"_time\" Step=\"15\" />", SingleEdit("time-picker").After);
    }

    [Fact]
    public void Pagination_RenamesOnPageChangeUnlessCurrentPageIsBound()
    {
        Page("List.razor",
            "<Pagination CurrentPage=\"_page\" TotalPages=\"9\" OnPageChange=\"Load\" />\n" +
            "<Pagination @bind-CurrentPage=\"_page\" OnPageChange=\"Log\" />");

        var edit = SingleEdit("pagination");

        Assert.Equal(
            "<Pagination CurrentPage=\"_page\" TotalPages=\"9\" CurrentPageChanged=\"Load\" />\n" +
            "<Pagination @bind-CurrentPage=\"_page\" OnPageChange=\"Log\" />", edit.After);
    }

    [Fact]
    public void Plan_SkipsInstalledComponentsAndBuildOutput()
    {
        File.WriteAllText(Path.Combine(_root, "Components", "UI", "Input.razor"), "Input.InputVariant.Error");
        File.WriteAllText(Path.Combine(_root, "obj", "Generated.cs"), "Input.InputVariant.Error");
        var page = Page("Form.razor", "Input.InputVariant.Error");

        var plan = MigrateService.Plan(_root);

        Assert.Equal(page, Assert.Single(plan.Edits).Path);
    }

    [Fact]
    public void Plan_WithNoComponentsRunsEveryMigrationAndListsWhatItApplied()
    {
        Page("All.razor", "<TimePicker SelectedTime=\"_t\" />\n<Input Variant=\"Input.InputVariant.Error\" />");

        var edit = SingleEdit();

        Assert.Equal(2, edit.Applied.Count);
        Assert.Equal("<TimePicker Value=\"_t\" />\n<Input Variant=\"InputVariant.Error\" />", edit.After);
    }

    [Fact]
    public void Calendar_FlagsHandlersThatTakeADateTime()
    {
        var page = Page("Plan.razor",
            "<Calendar SelectedDateChanged=\"OnDay\" />\n@code {\n    void OnDay(DateTime day) { }\n    void Other(DateTime d) { }\n}");

        var plan = MigrateService.Plan(_root);

        Assert.Empty(plan.Edits);
        var note = Assert.Single(plan.Notes);
        Assert.Equal(page, note.Path);
        Assert.Equal(3, note.Line);
    }

    [Fact]
    public void Plan_IsEmptyForAnUpToDateApp()
    {
        Page("Fine.razor", "<DatePicker @bind-Value=\"_d\" />\n<Calendar SelectedDateChanged=\"OnDay\" />\n@code { void OnDay(DateTime? d) { } }");

        var plan = MigrateService.Plan(_root);

        Assert.Empty(plan.Edits);
        Assert.Empty(plan.Notes);
    }
}
