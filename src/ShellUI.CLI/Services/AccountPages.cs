using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ShellUI.CLI.Services;

// init removes Bootstrap; unmodified Identity pages get ShellUI's Button, Input and Alert classes and edited ones are reported.
public static class AccountPages
{
    // `dotnet new blazor --auth Individual` for net8.0/net9.0/net10.0 and every interactivity option, hashed after Normalize.
    private static readonly Dictionary<string, HashSet<string>> StockHashes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Pages/AccessDenied.razor"] = new(StringComparer.Ordinal) { "b6109bf4a8706bb962cf64c45f67571aca2cbc9a45d2ffebb63d7cf4bc74b626" },
        ["Pages/ConfirmEmail.razor"] = new(StringComparer.Ordinal)
        {
            "3c84325a7c0d679532b52298a7df1a93b74020f24804ef1378d40a5753778d45",
            "4468ca180062585d6356dd71de5f44e3bfd463ef843ddac5f8ac223cc48e2b9d"
        },
        ["Pages/ConfirmEmailChange.razor"] = new(StringComparer.Ordinal)
        {
            "5263b5c2d262ce8555ac3fa6b474772a0e31e0d980972a6f61debe2963889874",
            "d689f04ee7510c53193df567163765ad0aeb80c911880aa7d5ff3487a05b3918"
        },
        ["Pages/ExternalLogin.razor"] = new(StringComparer.Ordinal)
        {
            "2170aea76f696f655e94fe614c33200c78dd3a955b11b81f12b67fbbbcea1a3f",
            "62e4ec0107d81e5d1829beb7d7bb2df0324e795bb4a8857d05720a8dd0852306",
            "6fc645d7deb82670172d2dc1f3ab1e9c6f45f64bf570f3e9f95aafe851da2fd5"
        },
        ["Pages/ForgotPassword.razor"] = new(StringComparer.Ordinal)
        {
            "5053ecbb6b9d836388491fdda1d5d7a1534a31224eaa32e83b6b0f15d2954e8d",
            "6bf4a0ab8ffd045a645ba45d813aa8d3a78c80c909959b10c4d99917a22538da",
            "e63a3097ff744175ec91c7059ecb540a355484b4c708fdb1546dd6aac74b2f36"
        },
        ["Pages/ForgotPasswordConfirmation.razor"] = new(StringComparer.Ordinal)
        {
            "74724076354ddd81dc1cb715d3a29cd45a014d71cfa589d94f53140a1a9669d0",
            "f3ffe30484369b81cddb5fb8a35100a89c0712902bb57b998e6b405d52ec36c1"
        },
        ["Pages/InvalidPasswordReset.razor"] = new(StringComparer.Ordinal)
        {
            "19b56fd47b9b95acbf09b259793ac8c702ca11c201742d200f77d5ba7c012a96",
            "6375738630c240a80afe3e1b3e799a2811f4321cff1301c979bd7fe1399a62f1"
        },
        ["Pages/InvalidUser.razor"] = new(StringComparer.Ordinal) { "285b6a382eab7f48d6340d0f9a65a7285a8ea5329a699709cce7dcd5fdad520e" },
        ["Pages/Lockout.razor"] = new(StringComparer.Ordinal)
        {
            "469e986117725dc7f49edb72ce14b317ad4b6cb95583142f5141c84846c7eb32",
            "c2d910ba6d150392146fa98f69e1d379aab98564bb890d29c4359a4723f2b39b"
        },
        ["Pages/Login.razor"] = new(StringComparer.Ordinal)
        {
            "0e5eb7e1ef66dd84026fad8fc254bf994117dd2934388a6b228c74fe6eb68b6c",
            "3e9068fc3d32ebfeb9b5d5873b6c5083c00c5babcc61e010309ba703bd884e0d",
            "e11c78b3ddb87480e7bc6b14b9c8e006ab313a028da865b4cfc5cb258ced33c6"
        },
        ["Pages/LoginWith2fa.razor"] = new(StringComparer.Ordinal)
        {
            "3d7907a51eb1e610dd1047a4b67774da2ac9509b8b9f4a554e57fa7c13c0e867",
            "bd8337ce2f31c255df3c8a0e64e08855e563bd283e930f6f2c0b53530268f667",
            "e2728080c241e022781d6e24fc1c9b1bc408f23dad0cd5c67f5cec715e9c2f35"
        },
        ["Pages/LoginWithRecoveryCode.razor"] = new(StringComparer.Ordinal)
        {
            "9a7b775fb0fbeae7757c07ebe7fa88bd69071cf182c9bcc8d9853832a4ee654f",
            "b4724a2118e31135dce481b7f9280dea2ee03df5efee96517f78016db6050012",
            "d3987452d4ed72842d8ab9da24b811923a943aa8a84308a9b7fc6980826472de"
        },
        ["Pages/Manage/ChangePassword.razor"] = new(StringComparer.Ordinal)
        {
            "65b42b8849bbad1435a91e6745f88601cd62bb7361c0b35abf1beabcb36c6f3c",
            "74ef2785beb8153240612b94c35d56a5b6d920bfc33c3fd4bcdc16d7c270773f",
            "ddf28a69da05c15b0b58c06ed60ac749760ddace24180dcf20b38e43271b4f4f"
        },
        ["Pages/Manage/DeletePersonalData.razor"] = new(StringComparer.Ordinal)
        {
            "297fd574621f11597dd71a5b8598dbe5c3a19c3cb665a3505b2698979ed4ae5c",
            "2afd3abf4a94a09f2d2a8aca213bf999a1d55c79cdba6e343e4e84d75a39b005",
            "99ceff259f9c6315cead560a29cacee54a9c73cc350f059d79c1c96485ddb17f"
        },
        ["Pages/Manage/Disable2fa.razor"] = new(StringComparer.Ordinal)
        {
            "4f940f7b8871956e32b59822e7a78fdbb107f542cdf18d24ca007ac470ad77f1",
            "6d14e673a473ce33b274287acf1acd03f1ca52cca18efb8f73108e2d630a42f2"
        },
        ["Pages/Manage/Email.razor"] = new(StringComparer.Ordinal)
        {
            "81918687ef7ba8925321532cfc02ae78b2303809b1b6a0b94c592365e424d646",
            "a816c45c996e2a2db21775da93858a3db261c9416442bf78e376f53b426d5424",
            "cd4a3e01a35d0b1f59088ecdef8d7496bd147b0c7e72fe080f0207148f0f9b77"
        },
        ["Pages/Manage/EnableAuthenticator.razor"] = new(StringComparer.Ordinal)
        {
            "1d5f0148c5bc85ff2b17f7acbdfffd3a87f50e6b059ab9fb9830f081ac77ee5b",
            "69a1a925d591b871045274f5face85c1e5c1bb8ca769bda05074dd5b2ec90ded",
            "dde6c58980dbf1ed582b042391e94a9ac35271feb479545f03be510ba454312b"
        },
        ["Pages/Manage/ExternalLogins.razor"] = new(StringComparer.Ordinal)
        {
            "03a4c82975f6d0ae8e32cc43d749c3da6d5cbc5ef0c44ae851e8dc662d270a54",
            "640a28f70f27c4f382e2d7dff1e972b4e376ea2b7f6a38e89ed6f7c25edc7404"
        },
        ["Pages/Manage/GenerateRecoveryCodes.razor"] = new(StringComparer.Ordinal)
        {
            "7297a499efa1e89a4c82521ebbebe75d522cf8ddae7b018387ca494f57af2bec",
            "b252c7dc03946cb0a8fa620a3f9e433937d98f1bce39118f5137bdfcdac8037e"
        },
        ["Pages/Manage/Index.razor"] = new(StringComparer.Ordinal)
        {
            "3b44bb0cda2ebf94d475241647e1c5b1a06ee3d8e9e758128af9551fde8766b7",
            "4dfb4be606a212b18620ca5f499069d63ea0b138b43915a37c2c9298b0cf489e",
            "7029a2d3a766e569d30050ea7a1fa35c622f8f9082090963e080944de1e63800"
        },
        ["Pages/Manage/Passkeys.razor"] = new(StringComparer.Ordinal) { "2887ff8db7f40fbd207763942b0ac0c4286d7a6094d12ccc37e25f5941d94f4f" },
        ["Pages/Manage/PersonalData.razor"] = new(StringComparer.Ordinal)
        {
            "049f1eee3d298f4a75a006c10869500d34be195d5bd5e2b57aba79849c795c9d",
            "2af37ab52bc87caaf5d6d38a5670a77534eb23a8158a1878f93c33b2ac4c64ec"
        },
        ["Pages/Manage/RenamePasskey.razor"] = new(StringComparer.Ordinal) { "d7260ab13e73f04bedae5affc695d5eec2f82c8c210a913a5d72c5623e5040a6" },
        ["Pages/Manage/ResetAuthenticator.razor"] = new(StringComparer.Ordinal)
        {
            "267cef205ebf7675dcc334e1a9e67659465eb67ae7b3d7c6cf3dc393262929d9",
            "f2fcba0e007184f416ba726b02d804557f1a6da2aeaec7d0b0e90bd3d65d5dd8"
        },
        ["Pages/Manage/SetPassword.razor"] = new(StringComparer.Ordinal)
        {
            "0bcbe17a16e1951aa73f68173495a8c80c577cfe08646a983ca81f51f89329cf",
            "b900d9e0937e07de101be5ac3b01dd3be36e7200eb95c248aef30d55b67ec71f",
            "ea58057070f27fa73cb02e17376148e2a6d2e34e1961ffc9b539f84efd482626"
        },
        ["Pages/Manage/TwoFactorAuthentication.razor"] = new(StringComparer.Ordinal)
        {
            "198321c9815c183c395418a13677682eff6b53b100cfa96fdfac78946f45c6d7",
            "be60a9dc1f5280c3ed49b4db06af55c77f939a2d0556cbbfdf51ea6a7e08c52a"
        },
        ["Pages/Manage/_Imports.razor"] = new(StringComparer.Ordinal) { "3e222370c041c607a47d00cf66676a54b647446b8bbd24284170dcc698ffb365" },
        ["Pages/Register.razor"] = new(StringComparer.Ordinal)
        {
            "5a6cc61bb9ef9303744e93aaf5710516995744c8ae816f5097ac2f4e3ecae045",
            "740eee29369bb3134aeaf0ac46d868099279920b63fc97a098697edb37a9d828",
            "c468cc4906e60d31322a4fef6ac843d9749193913102c97fd1ed83e1e3f7cc28"
        },
        ["Pages/RegisterConfirmation.razor"] = new(StringComparer.Ordinal)
        {
            "4af952f873c71c5b19e5b8e101730271214ef86ebbbb56432e8f1399594092d8",
            "8c95cf01919d4ea348f87c826982db6a2417834c3fd6a3801b9798a83ea4e3e1",
            "fdcd4fb41f5611500dbf951534d950c3ac3cff4443082fec68e9da66d648617c"
        },
        ["Pages/ResendEmailConfirmation.razor"] = new(StringComparer.Ordinal)
        {
            "12b33198dc0e38618cd889e9f475891bb7738d969350da42828006bfc3f81697",
            "32a07c6d44c0c64fd7d07fca7f79af25c46e25e2cc0eacbb8cb0477f4367733a",
            "40955af96e0ba7064fcec07493fd1116fbebab30c8472bfae31cfcd2187a9b34"
        },
        ["Pages/ResetPassword.razor"] = new(StringComparer.Ordinal)
        {
            "477d1e8a2c08a010bf8570fd4cdb65464257301eb63b3c2a26a45f0ba8115266",
            "ae84c4c4a8b8f7efc53636ae86201131beb3984aec20538b239a7f801dc3bd62",
            "c741fc9428b6d527e32c27300014292c5163939ea9946a048f6da122d70ac110"
        },
        ["Pages/ResetPasswordConfirmation.razor"] = new(StringComparer.Ordinal)
        {
            "844c48259e2b6d532b3c77ed6e409af7f859b676d69536b11bd8d734ca404403",
            "91002bdc2454b2e6f9b563b66cc849e7c00622912ad250d16695694c7aa037fe"
        },
        ["Pages/_Imports.razor"] = new(StringComparer.Ordinal)
        {
            "2d0533eb9dff4d504a97c3ceeffc0466e2d916034f41b0f07a196dd0b956f191",
            "a38d03e90c73981b8a977fa0de16078868577b65ba1afb7291f8fa20f3bb24ba"
        },
        ["Shared/AccountLayout.razor"] = new(StringComparer.Ordinal)
        {
            "7b63158d82ddfaae90650438ec37a027a47c892e9bc85bc91d43b5e84ed3eff2",
            "ffa1e3e7a65e814feed52bd61228b29df431981a531f12cb2767408f19eab708"
        },
        ["Shared/ExternalLoginPicker.razor"] = new(StringComparer.Ordinal) { "a4a6361e8b508209de14bf7b04fd67773697e09e83d4f93b732c187ff057ba93" },
        ["Shared/ManageLayout.razor"] = new(StringComparer.Ordinal)
        {
            "07a5f54f17187dc07e9ebaf19dc57c5ae7dd63cfd650f14375f0425d56a95c20",
            "57e4978bd9196add1f3ffe6d85ba582bb4a6b54cd6987c5ff64609bb9405ccd8",
            "b87a94172f72a2fa3f5fcead12aad3e9dae48984febbc453da1106548d38a3b7"
        },
        ["Shared/ManageNavMenu.razor"] = new(StringComparer.Ordinal)
        {
            "4c2ade9171e6e75b9e17ce5db4130a188a1c7c720cc67e90b3fef10b536bf281",
            "bded26c673d0c8bfbd4904452291d2f493bfb5413e7e50201beda16ca3a681b1"
        },
        ["Shared/PasskeySubmit.razor"] = new(StringComparer.Ordinal) { "9dae91807e527b9887350045a9c37fe3b632c5c73f6158fb5f28efdbc6fbe94d" },
        ["Shared/RedirectToLogin.razor"] = new(StringComparer.Ordinal) { "e8ceb9bd9cf1466f0da3292e068fcc0249fde051c753a5ae4af1277b32a8b26a" },
        ["Shared/ShowRecoveryCodes.razor"] = new(StringComparer.Ordinal) { "d93d5b74ad731c56ee72ad285df1727d833a0ffb819a216ae1403c65a7561b87" },
        ["Shared/StatusMessage.razor"] = new(StringComparer.Ordinal) { "e271d48b5024056c4412441f89a71951360a103b84328bb3b35805bd0983410a" }
    };

    private const string Button = "inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50";
    private const string Alert = "relative mb-4 w-full rounded-lg border border-border p-4 text-sm";

    private static readonly Dictionary<string, string> ClassMap = new(StringComparer.Ordinal)
    {
        ["alert"] = Alert,
        ["alert-danger"] = "text-destructive",
        ["alert-info"] = "text-blue-700 dark:text-blue-400",
        ["alert-success"] = "text-green-700 dark:text-green-400",
        ["alert-warning"] = "text-yellow-700 dark:text-yellow-400",
        ["checkbox"] = "",
        ["col-lg-3"] = "col-span-12 @3xl:col-span-3 @3xl:not-last:pr-8",
        ["col-lg-4"] = "col-span-12 @3xl:col-span-4 @3xl:not-last:pr-8",
        ["col-lg-6"] = "col-span-12 @3xl:col-span-6 @3xl:not-last:pr-8",
        ["col-lg-9"] = "col-span-12 @3xl:col-span-9 @3xl:not-last:pr-8",
        ["col-lg-offset-2"] = "",
        ["col-md-3"] = "col-span-12 @2xl:col-span-3 @2xl:not-last:pr-8",
        ["col-md-4"] = "col-span-12 @2xl:col-span-4 @2xl:not-last:pr-8",
        ["col-md-6"] = "col-span-12 @2xl:col-span-6 @2xl:not-last:pr-8",
        ["col-md-9"] = "col-span-12 @2xl:col-span-9 @2xl:not-last:pr-8",
        ["col-md-12"] = "col-span-12",
        ["col-md-offset-2"] = "",
        ["col-xl-6"] = "col-span-12 @4xl:col-span-6 @4xl:not-last:pr-8",
        ["control-label"] = "text-sm font-medium",
        ["d-flex"] = "flex",
        ["darker-border-checkbox"] = "",
        ["flex-column"] = "flex-col",
        ["font-weight-bold"] = "font-bold",
        ["form-check-input"] = "size-4 shrink-0 rounded-sm border border-primary accent-primary",
        ["form-control"] = "flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50",
        // Bootstrap floats the label over the input that precedes it; here it moves above the input.
        ["form-floating"] = "grid gap-2 [&>label]:order-first [&>label]:col-span-full [&>label]:text-sm [&>label]:font-medium [&>label]:leading-none",
        ["form-horizontal"] = "space-y-4",
        ["form-label"] = "text-sm font-medium leading-none",
        ["glyphicon"] = "",
        ["glyphicon-warning-sign"] = "",
        ["h-100"] = "h-full",
        ["input-group"] = "grid-cols-[1fr_auto] gap-x-2",
        ["input-group-append"] = "flex",
        ["input-group-text"] = "inline-flex items-center rounded-md border border-input px-3",
        ["list"] = "mb-4 list-decimal space-y-3 pl-6",
        ["nav"] = "flex gap-1",
        ["nav-item"] = "",
        ["nav-link"] = "flex w-full items-center rounded-md px-3 py-2 text-sm font-medium text-muted-foreground transition-colors hover:bg-accent hover:text-accent-foreground [&.active]:bg-accent [&.active]:text-accent-foreground",
        ["nav-pills"] = "",
        ["recovery-code"] = "font-mono text-sm",
        // Container queries stack the columns in a narrow auth card; gutters are padding because column gaps overflow phones.
        ["row"] = "@container grid grid-cols-12 gap-y-6",
        ["table"] = "w-full text-sm",
        ["text-danger"] = "text-destructive",
        ["text-info"] = "text-blue-700 dark:text-blue-400",
        ["text-secondary"] = "text-muted-foreground",
        ["text-success"] = "text-green-600 dark:text-green-500",
        ["w-100"] = "w-full"
    };

    // Tailwind's preflight removes the heading, paragraph and link styles Bootstrap restored.
    private static readonly Dictionary<string, string> TagDefaults = new(StringComparer.Ordinal)
    {
        ["h1"] = "mb-4 text-2xl font-semibold tracking-tight",
        ["h2"] = "mb-2 text-lg font-semibold",
        ["h3"] = "mb-2 text-lg font-semibold",
        ["h4"] = "mb-2 font-semibold",
        ["hr"] = "my-4 border-border",
        ["p"] = "mb-3",
        ["kbd"] = "rounded bg-muted px-1.5 py-0.5 font-mono text-sm",
        ["td"] = "py-2 pr-4 align-middle",
        ["a"] = "font-medium underline underline-offset-4"
    };

    private static readonly Regex StaticClass = new(@"(?<=\sclass="")[^""@]*(?="")", RegexOptions.Compiled);
    private static readonly Regex TagStart = new(@"<(h[1-4]|hr|p|kbd|td|a)(?=[\s/>])", RegexOptions.Compiled);
    private static readonly Regex ClassAttribute = new(@"\sclass=""", RegexOptions.Compiled);

    public record Result(List<string> Restyled, List<string> Notes);

    internal static string Normalize(string content, string rootNamespace)
    {
        var text = content.TrimStart('﻿').Replace("\r\n", "\n").TrimEnd();
        // The dashboard setup retargets `@layout MainLayout` to its own layout; that alone doesn't make a page edited.
        text = DashboardLayoutDirective.Replace(text, "$1MainLayout");
        if (string.IsNullOrEmpty(rootNamespace) || rootNamespace == DashboardSetup.StockProjectName) return text;
        return Regex.Replace(text, $@"(?<![\w.]){Regex.Escape(rootNamespace)}(?=\.)", DashboardSetup.StockProjectName);
    }

    private static readonly Regex DashboardLayoutDirective = new(@"^(@layout[ \t]+(?:[\w.]+\.)?)DashboardLayout0\d(?=[ \t]*$)", RegexOptions.Compiled | RegexOptions.Multiline);

    internal static bool IsStock(string relativePath, string content, string rootNamespace)
    {
        if (!StockHashes.TryGetValue(relativePath.Replace('\\', '/'), out var hashes)) return false;
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Normalize(content, rootNamespace)))).ToLowerInvariant();
        return hashes.Contains(hash);
    }

    internal static string MapClasses(string classes)
    {
        var tokens = classes.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var output = new List<string>();
        if (tokens.Contains("btn"))
        {
            output.Add(Button);
            output.Add(tokens.Contains("btn-lg") ? "h-11 px-8" : "h-10 px-4 py-2");
            if (tokens.Contains("btn-primary")) output.Add("bg-primary text-primary-foreground hover:bg-primary/90");
            if (tokens.Contains("btn-danger")) output.Add("bg-destructive text-destructive-foreground hover:bg-destructive/90");
            if (tokens.Contains("btn-link")) output.Add("text-primary underline-offset-4 hover:underline");
        }
        foreach (var token in tokens)
        {
            if (token is "btn" or "btn-lg" or "btn-primary" or "btn-danger" or "btn-link") continue;
            output.Add(ClassMap.TryGetValue(token, out var mapped) ? mapped : token);
        }
        return string.Join(" ", output.SelectMany(o => o.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Distinct());
    }

    internal static string Restyle(string content)
    {
        // Razor reads `@` in markup as code; `@@` writes a literal `@`, and Tailwind still finds the class.
        var text = StaticClass.Replace(content, m => MapClasses(m.Value).Replace("@", "@@"));
        text = text
            .Replace("? \"danger\" : \"success\"", "? \"text-destructive\" : \"text-green-700 dark:text-green-400\"")
            .Replace("class=\"alert alert-@statusMessageClass\"", $"class=\"{Alert} @statusMessageClass\"");
        return AddTagDefaults(text);
    }

    private static string AddTagDefaults(string text)
    {
        var builder = new StringBuilder(text.Length + 1024);
        var index = 0;
        foreach (Match match in TagStart.Matches(text))
        {
            if (match.Index < index) continue;
            var end = TagEnd(text, match.Index);
            if (end < 0) continue;

            var name = match.Groups[1].Value;
            var tag = text[match.Index..(end + 1)];
            var hasClass = ClassAttribute.IsMatch(tag);
            if (hasClass && name == "a") continue;

            var defaults = TagDefaults[name];
            var updated = hasClass
                ? ClassAttribute.Replace(tag, m => $"{m.Value}{defaults} ", 1)
                : tag.Insert(match.Length, $" class=\"{defaults}\"");
            builder.Append(text, index, match.Index - index).Append(updated);
            index = end + 1;
        }
        builder.Append(text, index, text.Length - index);
        return builder.ToString();
    }

    // Razor attribute values can hold @(...) expressions with quotes and '>' inside, so the tag end is found by scanning.
    private static int TagEnd(string text, int start)
    {
        var depth = 0;
        var inQuote = false;
        for (var i = start + 1; i < text.Length; i++)
        {
            var c = text[i];
            if (depth > 0)
            {
                if (c == '(') depth++;
                else if (c == ')') depth--;
            }
            else if (c == '"') inQuote = !inQuote;
            else if (inQuote && c == '(' && text[i - 1] == '@') depth = 1;
            else if (!inQuote && c == '>') return i;
            else if (!inQuote && c == '<') return -1;
        }
        return -1;
    }

    internal static bool UsesBootstrap(string content) =>
        StaticClass.Matches(content).Any(m => m.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Any(token => ClassMap.ContainsKey(token) || token.StartsWith("btn", StringComparison.Ordinal)));

    public static Result Apply(string cwd, string rootNamespace)
    {
        var restyled = new List<string>();
        var edited = new List<string>();
        foreach (var file in DashboardSetup.EnumerateSources(cwd, "*.razor"))
        {
            var rel = Path.GetRelativePath(cwd, file).Replace('\\', '/');
            var marker = rel.IndexOf("Components/Account/", StringComparison.OrdinalIgnoreCase);
            if (marker < 0) continue;

            var content = File.ReadAllText(file);
            if (IsStock(rel[(marker + "Components/Account/".Length)..], content, rootNamespace))
            {
                var updated = Restyle(content);
                if (updated == content) continue;
                File.WriteAllText(file, updated);
                restyled.Add(rel);
            }
            else if (UsesBootstrap(content))
            {
                edited.Add(rel);
            }
        }

        var notes = edited
            .Select(f => $"Kept {f} because it was modified. It still uses Bootstrap classes, which no longer have styles.")
            .ToList();
        return new Result(restyled, notes);
    }
}
