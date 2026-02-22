using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager;
using MiniUrl.KeyManager.Infrastructure;
using MiniUrl.KeyManager.Services;
using MiniUrl.Shared.WebHost.Extensions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.WebHost.ConfigureKestrel((context, options) =>
{
    var grpcPort = context.Configuration.GetValue("GrpcPort", 5012);
    options.Listen(IPAddress.Any, grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);
app.MigrateDbContext<KeysManagerContext>((context, services) =>
{
    var logger = services.GetService<ILogger<KeyManagerContextSeed>>();
    var keysGenerator = services.GetService<IKeysGeneratorService>();

    new KeyManagerContextSeed()
        .SeedAsync(context, keysGenerator, logger)
        .Wait();
});

app.Run();
