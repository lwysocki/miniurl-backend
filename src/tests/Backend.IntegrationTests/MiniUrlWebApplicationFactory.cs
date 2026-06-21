using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace MiniUrl.IntegrationTests;

class MiniUrlWebApplicationFactory<TEntryPoint>(Action<IHostBuilder> configureBuilder) : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        configureBuilder(builder);

        return base.CreateHost(builder);
    }
}
