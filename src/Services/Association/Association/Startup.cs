using GrpcAssociation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Association.Infrastructure.Repositories;
using MiniUrl.Shared.Domain;
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
            services.AddGrpc();

            services.AddDbContext<AssociationContext>(options =>
            {
                options.UseNpgsql(Configuration["AssociationsConnectionString"], npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    npgsqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                });
            });
            services.AddDbContext<KeyContext>(options =>
            {
                options.UseNpgsql(Configuration["KeysManagerConnectionString"]);
            });

            services.Configure<KeyConverter.KeyConverterSettings>(Configuration.GetSection(KeyConverter.KeyConverterSettings.Section));
            services.AddTransient<IKeyConverter, KeyConverter>();
            services.AddTransient<IKeyRepository, KeyRepository>();
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
}
