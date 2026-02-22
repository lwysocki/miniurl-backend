using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using MiniUrl.Association;
using MiniUrl.Association.Infrastructure;
using MiniUrl.Shared.WebHost.Extensions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.localhost.json");
builder.WebHost.ConfigureKestrel((context, options) =>
{
    var grpcPort = context.Configuration.GetValue("GrpcPort", 5010);
    options.Listen(IPAddress.Any, grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);
app.MigrateDbContext<AssociationContext>((context, services) => { });

app.Run();
