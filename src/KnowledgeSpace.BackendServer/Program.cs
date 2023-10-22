using FluentAssertions.Common;
using FluentValidation.AspNetCore;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.IdentityServer;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using KnowledgeSpace.BackendServer.Service;
using KnowledgeSpace.BackendServer.Helpers;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RoleCreateRequestValidator>());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Host.UseSerilog((context, config) =>
        {
            config.WriteTo.Console();
        });

        builder.Services.AddMvc();

        builder.Services.AddDbContextPool<ApplicationDbcontext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<ApplicationDbcontext>();
        builder.Services.AddIdentityServer(x =>
        {
            x.Events.RaiseErrorEvents = true;
            x.Events.RaiseInformationEvents = true;
            x.Events.RaiseFailureEvents = true;
            x.Events.RaiseSuccessEvents = true;
        }).AddInMemoryApiResources(Config.Apis)
        .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
        .AddInMemoryIdentityResources(Config.Ids)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddAspNetIdentity<User>().AddProfileService<IdentityProfileService>().AddDeveloperSigningCredential();

        var origin = builder.Configuration["AllowOrigins"];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("KspSpecificOrigins",
            builder =>
            {
                builder.WithOrigins(origin)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }); ;

        //builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Knowledge Space API", Version = "v1", });
            x.AddSecurityDefinition("SecurityId", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(builder.Configuration["AuthorityUrl"] + "/connect/authorize"),
                        Scopes = new Dictionary<string, string> { { "api.knowledgespace", "KnowledgeSpace API" } }
                    },
                },
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "SecurityId" }
                    },
                    new List<string>{ "api.knowledgespace" }
                }
            });
        });

        builder.Services.AddAuthentication().AddLocalApi("LocalApiId", option =>
        {
            option.ExpectedScope = "api.knowledgespace";
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("IdPolicy", policy =>
            {
                policy.AddAuthenticationSchemes("LocalApiId");
                policy.RequireAuthenticatedUser();
            });
        });

        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
            {
                foreach (var selector in model.Selectors)
                {
                    var attributeRouteModel = selector.AttributeRouteModel;
                    attributeRouteModel.Order = -1;
                    attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                }
            });
        });

        builder.Services.AddTransient<IEmailSender, EmailSender>();
        //builder.Services.AddTransient<ILogger, Logger>();
        builder.Services.AddTransient<ISequenceService, SequenceService>();
        builder.Services.AddTransient<IStorageService, FileStorageService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.OAuthClientId("swagger");
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Knowledge Space API V1");
            });
        }
        
        app.UseRouting();
        app.UseStaticFiles();
        app.UseIdentityServer();
        app.UseAuthentication();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseCors("KspSpecificOrigins");

        app.MapDefaultControllerRoute();
        app.MapRazorPages();
        //app.UseMiddleware<ErrorWrappingMiddleware>();

        app.Run();
    }
}
