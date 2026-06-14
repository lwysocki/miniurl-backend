using GrpcAssociation;
using MiniUrl.ApiGateway.Web.Models;
using System.Threading.Tasks;

namespace MiniUrl.ApiGateway.Web.Services
{
    public class AssociationService(Association.AssociationClient associationClient) : IAssociationService
    {
        private readonly Association.AssociationClient _associationClient = associationClient;

        public async Task<UrlAssociationData> AddUrlAsync(Models.UrlRequest url)
        {
            var response = await _associationClient.AddUrlAsync(new GrpcAssociation.UrlRequest() { Address = url.Address });

            var data = new UrlAssociationData()
            {
                Key = response.Key,
                Address = response.Address
            };

            return data;
        }
    }
}
