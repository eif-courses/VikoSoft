using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using VikoSoft.Components;
using VikoSoft.Components.Account;
using VikoSoft.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDataGridEntityFrameworkAdapter();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/sign-oidc";

})
.AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = "6fa1f6e3-8571-45aa-bd39-771cc546800e"; // Application (client) ID
    microsoftOptions.ClientSecret = builder.Configuration["Microsoft:Secret"];
    microsoftOptions.CallbackPath = "/signin-oidc";
    microsoftOptions.Scope.Add("openid offline_access");
    
}).AddIdentityCookies();


// EnableSensitiveDataLogging includes application data in exception messages and framework logging.
#if DEBUG
builder.Services.AddDbContextFactory<VikoDbContext>(opt =>
    opt.UseSqlite($"Data Source=VikoSoftware.db")
        .EnableSensitiveDataLogging());
#else
    services.AddDbContextFactory<VikoDbContext>(opt =>
        opt.UseSqlite($"Data Source=VikoSoftware.db"));
#endif


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<VikoDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<IdentityUser>, IdentityNoOpEmailSender>();



builder.Services.AddFluentUIComponents();
builder.Services.AddLocalization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();