using BlazorWebApp.Components;
//using BlazorWebApp.IoC;
using BlazorWebApp.RabbitMq;
using BlazorWebApp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
//builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
//builder.Services.TryAddEnumerable(
//    ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());
builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddDemoApiClientService(x => x.ApiBaseAddress = builder.Configuration.GetValue<string>("ApiBaseAddress"));
builder.Services.AddSingleton<DemoApiClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.MapRazorComponents<App>().RequireAuthorization(
//    new AuthorizeAttribute
//    {
//        AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme
//    })
//    .AddInteractiveServerRenderMode();

app.Run();
