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

        public Task<UrlAssociationData> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
