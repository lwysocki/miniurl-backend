using GrpcAssociation;
using GrpcKeysManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Association.Settings;
using MiniUrl.Shared.Domain;
using MiniUrl.Shared.Infrastructure;
using System;
using System.Reflection;

namespace MiniUrl.Association
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GrpcUrls>(Configuration.GetSection(GrpcUrls.Section));

            services.AddGrpc();

            services.AddDbContext<AssociationContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionString"], npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    npgsqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                });
            });

            services.AddGrpcServices();

            services.Configure<KeyConverter.KeyConverterSettings>(Configuration.GetSection(KeyConverter.KeyConverterSettings.Section));
            services.AddTransient<IKeyConverter, KeyConverter>();
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
                endpoints.MapGrpcService<AssociationService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddGrpcClient<KeysManager.KeysManagerClient>((services, options) =>
            {
                var keysManagerServiceUrl = services.GetRequiredService<IOptions<GrpcUrls>>().Value.KeysManagerService;
                options.Address = new Uri(keysManagerServiceUrl);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}
