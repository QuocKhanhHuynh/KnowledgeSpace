using FluentAssertions.Common;
using FluentValidation.AspNetCore;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});
builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<KnowledgeBaseCreateRequestValidator>()); ;
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["Authorization:AuthorityUrl"];
    options.RequireHttpsMetadata = false;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = builder.Configuration["Authorization:ClientId"];
    options.ClientSecret = builder.Configuration["Authorization:ClientSecret"];
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("offline_access");
    options.Scope.Add("api.knowledgespace");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddTransient<IKnowledgeBaseApiClient, KnowledgeBaseApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<ILabelApiClient, LabelApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();


/*
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (enviroment == Environments.Development)
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "My KBs",
    pattern: "/my-kbs",
    new { controller = "Account", action = "MyKnowledgeBases" });

app.MapControllerRoute(
    name: "ListByCategoryId",
    pattern: "/cat/{categoryAlias}-{id}",
    new { controller = "KnowledgeBase", action = "ListByCategoryId" });

app.MapControllerRoute(
    name: "KnowledgeBaseDetails",
    pattern: "/kb/{seoAlias}-{id}",
    new { controller = "KnowledgeBase", action = "Details" });

app.MapControllerRoute(
    name: "EDIT KB",
    pattern: "/edit-kb/{id}",
    new { controller = "Account", action = "EditKnowledgeBase" });

app.MapControllerRoute(
    name: "Search Kb",
    pattern: "/search",
    new { controller = "KnowledgeBase", action = "Search" });

app.MapControllerRoute(
    name: "List By Tag Id",
    pattern: "/tag/{tagId}",
    new { controller = "KnowledgeBase", action = "ListByTag" });

app.MapControllerRoute(
    name: "New KB",
    pattern: "/new-kb",
    new { controller = "Account", action = "CreateNewKnowledgeBase" });

app.Run();