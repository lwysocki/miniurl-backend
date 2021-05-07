using GrpcKeysManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Infrastructure;
using MiniUrl.KeyManager.Infrastructure.Repositories;
using MiniUrl.KeyManager.Services;
using System;
using System.Reflection;

namespace MiniUrl.KeyManager
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
            services.AddGrpc();
            services.AddDbContext<KeyManagerContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionString"], npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    npgsqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                });
            });

            services.AddTransient<IKeysManagerRepository, KeysManagerRepository>();

            services.Configure<KeysManagerService.KeysManagerSettings>(Configuration.GetSection(KeysManagerService.KeysManagerSettings.Section));

            services.Configure<KeysGeneratorService.KeysGeneratorSettings>(Configuration.GetSection(KeysGeneratorService.KeysGeneratorSettings.Section));
            services.AddSingleton<IKeysGeneratorService, KeysGeneratorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<KeysManagerService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
