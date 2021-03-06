using GrpcAssociation;
using GrpcUrl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MiniUrl.ApiGateway.Web.Services;
using MiniUrl.ApiGateway.Web.Settings;
using MiniUrl.Shared.Infrastructure;
using System;

namespace MiniUrl.ApiGateway.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GrpcUrls>(Configuration.GetSection(GrpcUrls.Section));

            services.AddMvcServices()
                .AddGrpcServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvcServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway.Web", Version = "v1" });
            });
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddScoped<IUrlService, UrlService>();

            services.AddGrpcClient<Url.UrlClient>((services, options) =>
            {
                var urlServiceUrl = services.GetRequiredService<IOptions<GrpcUrls>>().Value.UrlService;
                options.Address = new Uri(urlServiceUrl);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddScoped<IAssociationService, AssociationService>();

            services.AddGrpcClient<Association.AssociationClient>((services, options) =>
            {
                var associationServiceUrl = services.GetRequiredService<IOptions<GrpcUrls>>().Value.AssociationService;
                options.Address = new Uri(associationServiceUrl);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}
