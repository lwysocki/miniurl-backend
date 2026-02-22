using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MiniUrl.ApiGateway.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.localhost.json");

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);
app.Run();
