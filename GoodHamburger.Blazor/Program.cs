using GoodHamburger.Blazor.Components;
using GoodHamburger.Blazor.Configuration;
using GoodHamburger.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(nameof(ApiSettings)));

var apiSettings = builder.Configuration
    .GetSection(nameof(ApiSettings))
    .Get<ApiSettings>()
    ?? throw new InvalidOperationException("ApiSettings não configurado.");

if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
    throw new InvalidOperationException("ApiSettings:BaseUrl não configurado.");

builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});

builder.Services.AddScoped<MenuApiService>();
builder.Services.AddScoped<OrderApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
