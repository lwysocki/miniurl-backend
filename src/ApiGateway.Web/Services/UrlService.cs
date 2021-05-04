using GrpcUrl;
using MiniUrl.ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Services
{
    public class UrlService : IUrlService
    {
        private readonly Url.UrlClient _urlClient;

        public UrlService(Url.UrlClient urlClient)
        {
            _urlClient = urlClient;
        }

        public async Task<UrlAssociationData> GetByKeyAsync(string key)
        {
            var response = await _urlClient.GetUrlByKeyAsync(new KeyRequest { Key = key });

            var data = new UrlAssociationData()
            {
                Key = response.Key,
                Address = response.Address
            };

            return data;
        }
    }
}
