using System;
using Microsoft.AspNetCore.TestHost;
using MiniUrl.IntegrationTests.Services.ApiGateway;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IDisposable
    {
        public TestServer ApiGatewayServer { get; private set; }

        public IntegrationFixture()
        {
            ApiGatewayServer = ApiGatewaySetup.CreateServer();
        }

        public void Dispose()
        {
            ApiGatewayServer.Dispose();
        }
    }
}
