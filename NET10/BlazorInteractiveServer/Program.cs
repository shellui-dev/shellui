using BlazorInteractiveServer.Components;
using BlazorInteractiveServer.Components.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SonnerService>();
builder.Services.AddScoped<ISonnerService>(sp => sp.GetRequiredService<SonnerService>());
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
