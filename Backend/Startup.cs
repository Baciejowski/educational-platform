using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Backend.Analysis_module;
using Backend.Analysis_module.SessionModule;
using Backend.Auth;
using Backend.Services;
using Backend.Services.ClassManagement;
using Backend.Services.EmailProvider;
using Backend.Services.EmailProvider.Settings;
using Backend.Services.Report;
using Backend.Services.ScenarioManagement;
using Backend.Services.TeacherManagement;
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
        }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            void LoadFlags()
            {
                Flags.RunVue = Configuration.GetSection("Flags")["RunVue"].Equals("True");
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
                        //builder => builder.WithOrigins(Configuration.GetSection("Cors").Get<string[]>())
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            //.AllowCredentials()
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
                options.AddPolicy("read:messages",
                    policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            AddDatabase();
            services.AddGrpc();
            AddHttpServices();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            services.AddScoped<IClassManagementService, ClassManagementService>();
            services.AddScoped<IScenarioManagementService, ScenarioManagementService>();
            services.AddScoped<ITeacherManagementService, TeacherManagementService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddSingleton<ISessionFactory, SessionFactory>();
            services.AddSingleton<IAnalysisModuleService, AnalysisModuleService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            void AddHttpServices()
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();

                app.UseRouting();

                app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

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
                    endpoints.MapGrpcService<GreeterService>();
                });
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
            if (Flags.RunVue) RunVue();
        }
    }
}