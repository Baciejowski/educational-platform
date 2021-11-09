using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using Backend.Auth;
using Backend.Services;
using Backend.Services.ClassManagement;
using Backend.Services.EmailProvider;
using Backend.Services.EmailProvider.Settings;
using Backend.Services.InMemory;
using Backend.Services.ScenarioManagement;
using VueCliMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Backend
{
    public class Startup
    {
        private static class Flags
        {
            public static bool RunVue = false;
            public static bool RunAI = false;
        }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            void LoadFlags()
            {
                Flags.RunVue = Configuration.GetSection("Flags")["RunVue"].Equals("True");
                Flags.RunAI = Configuration.GetSection("Flags")["RunAI"].Equals("True");
            }

            Configuration = configuration;
            LoadFlags();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            void AddDatabase()
            {
                var connectionString = Configuration["DbContextSettings:ConnectionString"];
                services.AddDbContext<DataContext>(opts => opts.UseNpgsql(connectionString));
            }

            void AddHttpServices()
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: "default",
                                      builder => builder.WithOrigins(Configuration.GetSection("Cors").Get<string[]>())
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod());
                });
                services.AddControllers();
                if (Flags.RunVue)
                    services.AddSpaStaticFiles(configuration => configuration.RootPath = "../ClientApp");
            }

            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            }); 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            AddDatabase();
            services.AddGrpc();
            AddHttpServices();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            services.AddScoped<IClassManagementService, ClassManagementService>();
            services.AddSingleton<IScenarioManagementService, ScenarioManagementService>();
            services.AddSingleton<IInMemory, InMemory>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            void AddHttpServices()
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();

                app.UseRouting();

                if (Flags.RunVue)
                    app.UseSpaStaticFiles();

                app.UseCors("default");
                app.UseAuthorization();
            }

            void AddEndpoints()
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapGrpcService<GameConnector>();
                });
            }

            void RunAI()
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(Configuration.GetSection("Variables")["AIScriptLocation"])
                {
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };
                p.Start();
                logger.LogInformation("AI script started");
            }

            void RunVue()
            {
                app.UseSpa(spa =>
                {
                    if (env.IsDevelopment())
                        spa.Options.SourcePath = "../ClientApp/";
                    else
                        spa.Options.SourcePath = "dist";

                    if (env.IsDevelopment())
                    {
                        logger.LogInformation("Vue building starts");
                        spa.UseVueCli(npmScript: "serve", forceKill: true);
                    }
                });
            }

            app.UseAuthentication();
            AddHttpServices();
            AddEndpoints();
            if (Flags.RunAI) RunAI();
            if (Flags.RunVue) RunVue();
        }
    }
}