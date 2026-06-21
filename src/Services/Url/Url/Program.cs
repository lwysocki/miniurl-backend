using GrpcUrl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniUrl.Shared.Domain;
using MiniUrl.Url.Infrastructure;
using System;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    var grpcPort = context.Configuration.GetValue("GrpcPort", 5011);
    options.Listen(IPAddress.Any, grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddGrpc();

builder.Services.AddDbContext<UrlContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionString"], npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly(typeof(UrlContext).GetTypeInfo().Assembly.GetName().Name);
        npgsqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
    });
});

builder.Services.Configure<KeyConverter.KeyConverterSettings>(builder.Configuration.GetSection(KeyConverter.KeyConverterSettings.Section));
builder.Services.AddTransient<IKeyConverter, KeyConverter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGrpcService<UrlService>();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
});

app.Run();
