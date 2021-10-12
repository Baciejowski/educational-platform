using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueCliMiddleware;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "default",
                                  builder => builder.WithOrigins(Configuration.GetSection("Cors").Get<string[]>()));
            });
            services.AddGrpc();
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../ClientApp";
            });

            var connectionString = Configuration["DbContextSettings:ConnectionString"];
            services.AddDbContext<DataContext>(opts => opts.UseNpgsql(connectionString));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthorization();

            app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GameConnector>();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "../ClientApp/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }

            });
        }
    }
}
