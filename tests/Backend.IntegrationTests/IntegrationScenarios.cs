using MiniUrl.ApiGateway.Web.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationScenarios(IntegrationFixture fixture) : IClassFixture<IntegrationFixture>
    {
        private readonly IntegrationFixture _fixture = fixture;

        [Fact]
        public async Task RequestedUrlShouldBeRetrievableByAssociatedKey()
        {
            var urlRequest = new UrlRequest { Address = IntegrationFixture.url };

            var postResponse = await _fixture.ApiGatewayClient.PostAsync("urls",
                new StringContent(JsonSerializer.Serialize(urlRequest), Encoding.UTF8, "application/json"));
            var postContent = await postResponse.Content.ReadAsStringAsync();
            var urlAssociation = JsonSerializer.Deserialize<UrlAssociationData>(postContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.False(string.IsNullOrEmpty(urlAssociation.Key));

            var getResponse = await _fixture.ApiGatewayClient.GetAsync("urls/" + urlAssociation.Key);
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();

            Assert.Contains(IntegrationFixture.url, getContent);
        }
    }
}
