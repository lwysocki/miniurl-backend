using Microsoft.AspNetCore.Hosting;
using MiniUrl.ApiGateway.Web.Models;
using MiniUrl.IntegrationTests.Services.ApiGateway;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationScenarios : IClassFixture<IntegrationFixture>
    {
        private IntegrationFixture _fixture;

        public IntegrationScenarios(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SendAssociationRequestShouldReturnAssociatedKey()
        {
            var apiGatewayClient = _fixture.ApiGatewayServer.CreateClient();

            var urlRequest = new UrlRequest { Address = IntegrationFixture.url };

            var response = await apiGatewayClient.PostAsync("urls",
                new StringContent(JsonSerializer.Serialize(urlRequest), UTF8Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var urlAssociation = JsonSerializer.Deserialize<UrlAssociationData>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _fixture.Key = urlAssociation.Key;

            Assert.True(!string.IsNullOrEmpty(_fixture.Key));
        }

        [Fact]
        public async Task SendValidKeyShouldReturnAssociatedUrl()
        {
            var apiGatewayClient = _fixture.ApiGatewayServer.CreateClient();

            var getResponse = await apiGatewayClient.GetAsync("urls/" + _fixture.Key);
            var getContent = getResponse.Headers.Location.AbsoluteUri;

            Assert.True(getContent.Contains(IntegrationFixture.url));
        }
    }
}
