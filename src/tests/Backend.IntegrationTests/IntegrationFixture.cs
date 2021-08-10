using System;
using Microsoft.AspNetCore.TestHost;
using MiniUrl.IntegrationTests.Services.ApiGateway;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IDisposable
    {
        public const string url = "google.com";

        public TestServer ApiGatewayServer { get; private set; }
        public string Key { get; set; }

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
