using ApiGateway.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcUrl;

namespace ApiGateway.Web.Services
{
    public class UrlService : IUrlService
    {
        private readonly Url.UrlClient _urlClient;

        public UrlService(Url.UrlClient urlClient)
        {
            _urlClient = urlClient;
        }

        public async Task<UrlAssociationData> GetByIdAsync(string id)
        {
            var response = await _urlClient.GetUrlByIdAsync(new GrpcUrl.UrlRequest { Id = int.Parse(id) });

            var data = new UrlAssociationData()
            {
                Key = response.Id.ToString(),
                Address = response.Address
            };

            return data;
        }
    }
}
